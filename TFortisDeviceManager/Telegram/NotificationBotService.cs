using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using TFortisDeviceManager.Models;
using Stylet;
using TFortisDeviceManager.Database;
using System.Net;
using TFortisDeviceManager.Services;
using Serilog;
using System.Globalization;
using Microsoft.Extensions.Hosting;
using HandyControl.Tools.Extension;

namespace TFortisDeviceManager.Telegram
{
    public interface INotificationBot
    {
        public Task StartAsync(CancellationToken cancellationToken);
        public Task SendTelegramNotificationAsync(string message, bool sendNotification);
    }

    public class NotificationBotService : INotificationBot, IHostedService
    {
        private  readonly IUserSettings _userSettings;
        private  readonly IDeviceSearcher _deviceSearcher;

        private readonly ITelegramBotClient _botClient;

        private readonly ReceiverOptions _receiverOptions;
        public static BindableCollection<NetworkDevice> CurrentFoundDevices { get; } = new BindableCollection<NetworkDevice>();

        public  NotificationBotService(IDeviceSearcher deviceSearcher, ISettingsProvider settingsProvider)
        {
            _userSettings = settingsProvider.UserSettings;
            _deviceSearcher = deviceSearcher ?? throw new ArgumentNullException(nameof(deviceSearcher));

            _botClient = new TelegramBotClient(_userSettings.AlertSettings.BotToken); 
            _receiverOptions = new ReceiverOptions 
            {
                AllowedUpdates = new[] 
            {
                UpdateType.Message, 
                UpdateType.CallbackQuery 
            },
                ThrowPendingUpdates = true,
            };
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _botClient.StartReceiving(UpdateHandler, ErrorHandler, _receiverOptions, cancellationToken);

            var me = await _botClient.GetMeAsync(cancellationToken);

            await Task.Delay(-1, cancellationToken);
        }

        private async Task UpdateHandler(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            try
            {             
                var message = update.Message;

                var user = message.From;

                var chat = message.Chat;

                var split = message?.Text?.Split('-');

                bool isIpAddress = IPAddress.TryParse(split[0], out IPAddress? tmp) &&
                             (split.Length == 1 || split.Length == 2 && IPAddress.TryParse(split[1], out tmp));


                if (!_userSettings.AlertSettings.UserList.Contains(chat.Id.ToString()))
                {
                    await botClient.SendTextMessageAsync(chat.Id, $"{Properties.Resources.AuthorisationError}\n\n{Properties.Resources.YourUniqueId} {chat.Id}\n\n{Properties.Resources.AddToMailingList}", cancellationToken: cancellationToken);

                    Log.Information($"Попытка авторизации пользователя без доступа. Username: {chat.Username}");

                    return;
                }

                if (isIpAddress)
                {
                    string ip = message.Text;

                    await IpAddressMessageProcessing(botClient, ip, chat, cancellationToken);

                }

                string helpMessage = GetHelpMessage();

                switch (message.Text)
                {
                    case "/start":
                        {
                            await StartMessageProcessing(botClient, chat, cancellationToken);
                            break;
                        }
                    case "/stop":
                        {
                            StopMessageProcessing(chat);
                            break;
                        }
                    case "/devicelist":
                        {
                            await GetDeviceListMessageProcessing(botClient, chat, cancellationToken);
                            break;
                        }
                    case var value when value == $"{Properties.Resources.DeviceList}":
                        {
                            await GetDeviceListMessageProcessing(botClient, chat, cancellationToken);
                            break;
                        }
                    case "/startsearching":
                        {
                            await StartSearchingMessageProcessing(update, botClient, chat, cancellationToken);
                            break;
                        }
                    case var value when value == $"{Properties.Resources.StartSearching}":
                        {
                            await StartSearchingMessageProcessing(update, botClient, chat, cancellationToken);
                            break;
                        }
                    case "/help":
                        {
                            await botClient.SendTextMessageAsync(chat.Id,
                                                                 helpMessage,
                                                                 parseMode: ParseMode.Html,
                                                                 cancellationToken: cancellationToken);
                            break;
                        }
                    default:
                        {
                            await botClient.SendTextMessageAsync(chat.Id,
                                  helpMessage,
                                  parseMode: ParseMode.Html,
                                  cancellationToken: cancellationToken);
                            break;
                        }
                }                                                             
            }
            catch (Exception ex)
            {
                Log.Error($"Ошибка отправки сообщения в Telegram: {ex.Message}");
                Console.WriteLine(ex.ToString());
            }
        }

        private static Task ErrorHandler(ITelegramBotClient botClient, Exception error, CancellationToken cancellationToken)
        {
            var ErrorMessage = error switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => error.ToString()
            };

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }

        public async Task SendTelegramNotificationAsync(string message, bool sendNotification)
        {
            List<string> chatIdList = _userSettings.AlertSettings.ChatIdToNotifyList.Split(' ').Distinct().ToList();
            try
            {
                if (sendNotification)
                {
                    foreach (var id in chatIdList)
                    {
                        if (!id.IsNullOrEmpty())
                        {
                            await _botClient.SendTextMessageAsync(id.ToString(),
                                                               message,
                                                               parseMode: ParseMode.Html);
                        }
                    }
                }
            }
            catch
            {
                Console.WriteLine("Ошибка отправки телеграм-сообщения");
            }
        }

        private async Task StartMessageProcessing(ITelegramBotClient botClient, Chat chat, CancellationToken cancellationToken)
        {
            var replyKeyboard = new ReplyKeyboardMarkup(
                                new List<KeyboardButton[]>()
                                {
                                    new KeyboardButton[]
                                    {
                                        new KeyboardButton($"{Properties.Resources.DeviceList}"),
                                    },
                                    new KeyboardButton[]
                                    {
                                        new KeyboardButton($"{Properties.Resources.StartSearching}")
                                    },

                                })
            {
                ResizeKeyboard = true,
            };

            if(!_userSettings.AlertSettings.ChatIdToNotifyList.Contains(chat.Id.ToString()))
            _userSettings.AlertSettings.ChatIdToNotifyList += (chat.Id.ToString() + " ");

            string helpMessage = GetHelpMessage();

            await botClient.SendTextMessageAsync(chat.Id,
                                                 helpMessage,
                                                 parseMode: ParseMode.Html,
                                                 replyMarkup: replyKeyboard,
                                                 cancellationToken: cancellationToken);

            Log.Information($"Сообщение для {chat.Username} отправлено в чат {chat.Id}");

            return;
        }

        private void StopMessageProcessing(Chat chat)
        {
            _userSettings.AlertSettings.ChatIdToNotifyList = _userSettings.AlertSettings.ChatIdToNotifyList.Replace((chat.Id.ToString(CultureInfo.InvariantCulture) + " "), "");
        }

        private static async Task GetDeviceListMessageProcessing(ITelegramBotClient botClient, Chat chat, CancellationToken cancellationToken)
        {
            StringBuilder stringBuilder = new();

            stringBuilder.Append(CultureInfo.InvariantCulture, $"<b>{Properties.Resources.DevicesInMonitoringList}:</b>\n\n");

            var monitoringDevices = PGDataAccess.GetDevicesForMonitoring();

            if (monitoringDevices.Count != 0)
            {

                for (int i = 0; i < monitoringDevices.Count; i++)
                {
                    var hostStatus = PGDataAccess.GetDeviceHostStatus(monitoringDevices[i].IpAddress, monitoringDevices[i].Name);

                    string emoji;

                    if (hostStatus == Properties.Resources.HostStatusDisabled || hostStatus == "Устройство недоступно")
                        emoji = "❌";
                    else emoji = "✅";

                    stringBuilder.Append(CultureInfo.InvariantCulture, $"{i + 1}. <b>{monitoringDevices[i].Name}</b>\n");
                    stringBuilder.Append(monitoringDevices[i].GetDeviceInfo());
                    stringBuilder.Append(CultureInfo.InvariantCulture, $"{emoji} {hostStatus}\n\n");
                }

                string devices = stringBuilder.ToString();

                await botClient.SendTextMessageAsync(chat.Id,
                                                     devices,
                                                     parseMode: ParseMode.Html,
                                                     cancellationToken: cancellationToken);

                Log.Information($"Сообщение для {chat.Username} отправлено в чат {chat.Id}");
            }
            else
            {
                await botClient.SendTextMessageAsync(chat.Id,
                                                    $"{Properties.Resources.DeviceListIsEmpty}",
                                                    parseMode: ParseMode.Html,
                                                    cancellationToken: cancellationToken);
            }
        }

        private async Task StartSearchingMessageProcessing(Update update, ITelegramBotClient botClient, Chat chat, CancellationToken cancellationToken)
        {
            int timeout = 10;
            using CancellationTokenSource cancelTokenSource = new();
            cancelTokenSource.CancelAfter(TimeSpan.FromSeconds(timeout));

            CancellationToken token = cancelTokenSource.Token;

            IEnumerable<NetworkDevice> devices;

            await botClient.SendTextMessageAsync(chat.Id,
                                                 $"⌛ <b><i>{Properties.Resources.SearchDevices}...</i></b>",
                                                 parseMode: ParseMode.Html,
                                                 cancellationToken: cancellationToken);

            Log.Information($"Сообщение для {chat.Username} отправлено в чат {chat.Id}");

            devices = await _deviceSearcher.SearchBroadcastAsync(token);

            Message lastMessage = update.Message;

            await botClient.DeleteMessageAsync(lastMessage.Chat.Id, lastMessage.MessageId + 1, cancellationToken);

            if (devices != null && devices.ToList().Count != 0)
            {
                StringBuilder stringBuilder = new();

                stringBuilder.Append(CultureInfo.InvariantCulture, $"<b>{Properties.Resources.FoundedDevicesList}:</b>\n\n");

                foreach (var device in devices)
                {
                    stringBuilder.Append(device.GetDeviceInfo());                
                }

                await botClient.SendTextMessageAsync(chat.Id,
                                                     stringBuilder.ToString(),
                                                     parseMode: ParseMode.Html,
                                                     cancellationToken: cancellationToken);

                Log.Information($"Сообщение для {chat.Username} отправлено в чат {chat.Id}");

            }
            else
            {
                await botClient.SendTextMessageAsync(chat.Id,
                                                     $"{Properties.Resources.DevicesNotFound}",
                                                     parseMode: ParseMode.Html,
                                                     cancellationToken: cancellationToken);

                Log.Information($"Сообщение для {chat.Username} отправлено в чат {chat.Id}");

            }
            return;
        }

        private static async Task IpAddressMessageProcessing(ITelegramBotClient botClient, string message, Chat chat, CancellationToken cancellationToken)
        {
            var events = PGDataAccess.LoadEventsWithoutOldWithDeviceByIp(message).OrderBy(x => x.Status);

            if (events != null)
            {
                StringBuilder eventList = new();

                eventList.Append(CultureInfo.InvariantCulture, $"<b>{Properties.Resources.EventListWithAddress} {message}</b>\n\n");

                foreach (var evnt in events)
                {
                    eventList.Append(evnt.ToString());
                }

                await botClient.SendTextMessageAsync(chat.Id,
                                                     eventList.ToString(),
                                                     parseMode: ParseMode.Html,
                                                     cancellationToken: cancellationToken);

                Log.Information($"Сообщение для {chat.Username} отправлено в чат {chat.Id}");

                return;
            }
            else
            {
                await botClient.SendTextMessageAsync(chat.Id,
                                                     $"{Properties.Resources.DeviceWithAddressNotFound}",
                                                     cancellationToken: cancellationToken);

                Log.Information($"Сообщение для {chat.Username} отправлено в чат {chat.Id}");

                return;
            }
        }

        private static string GetHelpMessage()
        {

            return $"<b>{Properties.Resources.ChooseAction}:</b>\n" +
             $"/devicelist - {Properties.Resources.GetDeviceListInMonitoring}\n" +
             $"/startsearching - {Properties.Resources.StartTelegramSearching}" +
             $"\n\n{Properties.Resources.SendIpAddressToGetSensorList}";
                                            
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
