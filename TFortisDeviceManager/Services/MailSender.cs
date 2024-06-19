using MailKit.Net.Smtp;
using MimeKit;
using Serilog;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TFortisDeviceManager.Converters;
using TFortisDeviceManager.Models;

namespace TFortisDeviceManager.Services
{
    public interface IMailSender
    {
        Task SendEmailIfNeeded(EventModel evnt, bool shouldSendEmail);
    }

    public class MailSender : IMailSender
    {
        private readonly IUserSettings? _userSettings;

        public MailSender(ISettingsProvider settingsProvider)
        {       
            _userSettings = settingsProvider.UserSettings ?? throw new ArgumentNullException(nameof(settingsProvider)); 
          
        }

        public async Task SendEmailIfNeeded(EventModel evnt, bool shouldSendEmail)
        {
            if (shouldSendEmail)
            {
                await SendEmailAsync(evnt);
            }
        }

        private async Task SendEmailAsync(EventModel evnt)
        {

            string fromEmail = _userSettings.AlertSettings.FromEmail;
            string toEmails = _userSettings.AlertSettings.ToEmails;
            string smtpServer = _userSettings.AlertSettings.SmtpServer;
            int emailPort = _userSettings.AlertSettings.EmailPort;
            bool enableSSL = _userSettings.AlertSettings.EnableSSL;
            string login = _userSettings.AlertSettings.Login;
            string pass = _userSettings.AlertSettings.PasswordForEmail;

            if (fromEmail.Length == 0 || smtpServer.Length == 0) 
                return;

            InternetAddressList toEmailList = GetEmailList(toEmails);

            if (!toEmailList.Any())
            {
                Log.Information($"Адресов для отправки нет. Email не будет отправлен");
                return;
            }

            var message = new MimeMessage();

            message.From.Add(new MailboxAddress(fromEmail));
            message.To.AddRange(toEmailList);

            message.Subject = _userSettings.AlertSettings.SubjectForEmail;

            BodyBuilder bodyBuilder = CreateBody(evnt);
            message.Body = bodyBuilder.ToMessageBody();

            using var client = new SmtpClient();
            using var timeout = new CancellationTokenSource();
            timeout.CancelAfter(5000);

            try
            {
                await client.ConnectAsync(smtpServer, emailPort, enableSSL, timeout.Token);

                await client.AuthenticateAsync(login, pass, timeout.Token);
                await client.SendAsync(message, timeout.Token);

                Log.Information($"Сообщение отправлено {smtpServer}:{emailPort} From:{fromEmail} To:{toEmailList}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Сообщение не отправлено. Ошибка: {ex.Message}");
                Log.Error($"Сообщение не отправлено. Ошибка: {ex.Message}");
            }
            finally
            {
                client.Disconnect(true);
            }
        }

        private static InternetAddressList GetEmailList(string toEmails)
        {
            InternetAddressList toEmailList = new();

            string[] toEmailArray = toEmails.Split(';');

            foreach (var email in toEmailArray)
            {
                if (IsValidEmail(email))
                {
                     toEmailList.Add(new MailboxAddress(email.Trim()));
                }
                else
                {
                    Log.Information($"Адрес [{email}] некорректный");
                }
            }

            return toEmailList;
        }

        private static BodyBuilder CreateBody(EventModel evnt)
        {
            string emailHeaderTime = "Time";
            string emailHeaderModel = "Model";
            string emailHeaderIp = "Ip";
            string emailHeaderSensor = "Sensor";
            string emailHeaderSensorValue = "SensorValue";
            string emailHeaderSensorDescription = "SensorDescription";
            string emailHeaderStatus = "Status";
            string emailHeaderDeviceLocation = "DeviceLocation";
            string emailHeaderDeviceDescription = "DeviceDescription";

            string color;
            if (evnt.Status == Properties.Resources.StatusProblem)
                color = "#FF8D8D";
            else if (evnt.Status == Properties.Resources.StatusOk)
                color = "#9EE3A0";
            else if (evnt.Status == Properties.Resources.StatusInfo || evnt.Status == Properties.Resources.StatusError)
                color = "#FFFF8D";
            else
                color = "#FFFFFF";


            string? evntDescription = evnt.Description;

            if (Thread.CurrentThread.CurrentCulture.Name == "en-US" && LanguageConverter.translationDictionary.ContainsKey(evnt.Description))
            {
                evntDescription = LanguageConverter.translationDictionary[evnt.Description];
            }

            return new BodyBuilder
            {
                HtmlBody = $"<table style=\"border-collapse:collapse;border: 4px double black;\">" +
                            $"<tr>" +
                            $"<th style=\"text-align: left;background: #ccc;padding: 5px;border: 1px solid black;\">{emailHeaderTime}</th>" +
                            $"<th style=\"text-align: left;background: #ccc;padding: 5px;border: 1px solid black;\">{emailHeaderModel}</th>" +
                            $"<th style=\"text-align: left;background: #ccc;padding: 5px;border: 1px solid black;\">{emailHeaderIp}</th>" +
                            $"<th style=\"text-align: left;background: #ccc;padding: 5px;border: 1px solid black;\">{emailHeaderSensor}</th>" +
                            $"<th style=\"text-align: left;background: #ccc;padding: 5px;border: 1px solid black;\">{emailHeaderSensorValue}</th>" +
                            $"<th style=\"text-align: left;background: #ccc;padding: 5px;border: 1px solid black;\">{emailHeaderSensorDescription}</th>" +
                            $"<th style=\"text-align: left;background: #ccc;padding: 5px;border: 1px solid black;\">{emailHeaderStatus}</th>" +
                            $"<th style=\"text-align: left;background: #ccc;padding: 5px;border: 1px solid black;\">{emailHeaderDeviceLocation}</th>" +
                            $"<th style=\"text-align: left;background: #ccc;padding: 5px;border: 1px solid black;\">{emailHeaderDeviceDescription}</th>" +
                            $"</tr>" +
                            $"<tr bgcolor={color} >" +
                            $"<td style=\"background: {color};padding: 5px;border: 1px solid black;\">{evnt.Time}</td>" +
                            $"<td style=\"background: {color};padding: 5px;border: 1px solid black;\">{evnt.DeviceName}</td>" +
                            $"<td style=\"background: {color};padding: 5px;border: 1px solid black;\">{evnt.Ip}</td>" +
                            $"<td style=\"background: {color};padding: 5px;border: 1px solid black;\">{evnt.SensorName}</td>" +
                            $"<td style=\"background: {color};padding: 5px;border: 1px solid black;\">{evnt.SensorValueText}</td>" +
                            $"<td style=\"background: {color};padding: 5px;border: 1px solid black;\">{evntDescription}</td>" +
                            $"<td style=\"background: {color};padding: 5px;border: 1px solid black;\">{evnt.Status}</td>" +
                            $"<td style=\"background: {color};padding: 5px;border: 1px solid black;\">{evnt.DeviceLocation}</td>" +
                            $"<td style=\"background: {color};padding: 5px;border: 1px solid black;\">{evnt.DeviceDescription}</td>" +
                            $"</tr>" +
                            $"</table>",

                TextBody = $"{emailHeaderTime}: {evnt.Time} \n" +
                            $"{emailHeaderModel}: {evnt.DeviceName} \n" +
                            $"{emailHeaderIp}: {evnt.Ip} \n" +
                            $"{emailHeaderSensor}: {evnt.SensorName} \n" +
                            $"{emailHeaderSensorValue}: {evnt.SensorValueText} \n" +
                            $"{emailHeaderSensorDescription}: {evntDescription} \n" +
                            $"{emailHeaderStatus}: {evnt.Status} \n" +
                            $"{emailHeaderDeviceLocation}: {evnt.DeviceLocation} \n" +
                            $"{emailHeaderDeviceDescription}: {evnt.DeviceDescription}"

            };
        }

        private static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
