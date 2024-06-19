using System;
using MimeKit;
using System.Threading;
using System.Windows.Media;
using Stylet;
using TFortisDeviceManager.Models;
using MailKit.Net.Smtp;
using System.Threading.Tasks;
using Serilog;
using DocumentFormat.OpenXml.Wordprocessing;
using HandyControl.Controls;
using HandyControl.Data;
using TFortisDeviceManager.Services;
using TFortisDeviceManager.Telegram;

namespace TFortisDeviceManager.ViewModels;

public sealed class AlertSettingsViewModel : Screen, ISettingsState, IDisposable
{
    private readonly IUserSettings _userSettings;
    private readonly INotificationBot _notificationBot;


    private string? _botTokenInitialValue;

    public event EventHandler<SettingsStateEventArgs>? SettingsChanged;

    public string FromEmail
    {
        get => _userSettings.AlertSettings.FromEmail;
        set
        {
            _userSettings.AlertSettings.FromEmail = value;
            NotifyOfPropertyChange(nameof(FromEmail));
        }
    }

    public string ToEmails
    {
        get => _userSettings.AlertSettings.ToEmails;
        set
        {
            _userSettings.AlertSettings.ToEmails = value;
            NotifyOfPropertyChange(nameof(ToEmails));
        }
    }

    public string Login
    {
        get => _userSettings.AlertSettings.Login;
        set
        {
            _userSettings.AlertSettings.Login = value;
            NotifyOfPropertyChange(nameof(Login));
        }
    }

    public string PasswordForEmail
    {
        get => _userSettings.AlertSettings.PasswordForEmail;
        set
        {
            _userSettings.AlertSettings.PasswordForEmail = value;
            NotifyOfPropertyChange(nameof(PasswordForEmail));
        }
    }

    public string SubjectForEmail
    {
        get => _userSettings.AlertSettings.SubjectForEmail;
        set
        {
            _userSettings.AlertSettings.SubjectForEmail = value;
            NotifyOfPropertyChange(nameof(SubjectForEmail));
        }
    }

    public bool EnableSSL
    {
        get => _userSettings.AlertSettings.EnableSSL;
        set
        {
            _userSettings.AlertSettings.EnableSSL = value;
            NotifyOfPropertyChange(nameof(EnableSSL));
        }
    }

    public bool AuthenticationEnabled
    {
        get => _userSettings.AlertSettings.AuthenticationEnabled;
        set
        {
            _userSettings.AlertSettings.AuthenticationEnabled = value;
            NotifyOfPropertyChange(nameof(AuthenticationEnabled));
        }
    }

    public bool AccidentExpanding
    {
        get => _userSettings.AlertSettings.AccidentExpanding;
        set
        {
            _userSettings.AlertSettings.AccidentExpanding = value;
            NotifyOfPropertyChange(nameof(AccidentExpanding));
        }
    }

    public string SmtpServer
    {
        get => _userSettings.AlertSettings.SmtpServer;
        set
        {
            _userSettings.AlertSettings.SmtpServer = value;
            NotifyOfPropertyChange(nameof(SmtpServer));
        }
    }

    public int EmailPort
    {
        get => _userSettings.AlertSettings.EmailPort;
        set
        {
            _userSettings.AlertSettings.EmailPort = value;
            NotifyOfPropertyChange(nameof(EmailPort));
        }
    }

    public string BotToken
    {
        get => _userSettings.AlertSettings.BotToken;
        set
        {
            _userSettings.AlertSettings.BotToken = value;
            NotifyOfPropertyChange(nameof(BotToken));
        }
    }

    public string UserList
    {
        get => _userSettings.AlertSettings.UserList;
        set
        {
            _userSettings.AlertSettings.UserList = value;
            NotifyOfPropertyChange(nameof(UserList));
        }
    }

    public string ChatIdToNotifyList
    {
        get => _userSettings.AlertSettings.ChatIdToNotifyList;
        set
        {
            _userSettings.AlertSettings.ChatIdToNotifyList = value;
            NotifyOfPropertyChange(nameof(ChatIdToNotifyList));
        }
    }

    private string? _status;
    public string? Status
    {
        get => _status;
        set => SetAndNotify(ref _status, value);
    }

    private SolidColorBrush? _foreground;
    public SolidColorBrush? Foreground
    {
        get => _foreground;
        set => SetAndNotify(ref _foreground, value);
    }

    public async Task SendTestEmailCommand()
    {
        Log.Information($"Нажата кнопка отправки тестового уведомления");

        Growl.WarningGlobal(new GrowlInfo
        {
            StaysOpen = false,
            WaitTime = 10,
            Message = Properties.Resources.SendTestNotificationClicked
        });

        await _notificationBot.SendTelegramNotificationAsync("Это тестовое сообщение отправлено программой \"TFortis Device Manager\" для проверки работы Telegram-оповещений.", true);

        string fromEmail = FromEmail;
        string toEmail = ToEmails;
        string smtpServer = SmtpServer;
        bool enableSSL = EnableSSL;

        string login = Login;
        string pass = PasswordForEmail;

        int emailPort = EmailPort;
    
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(fromEmail));
        message.To.Add(new MailboxAddress(toEmail));
        message.Subject = "TFortis Device Manager - Тестовое сообщение";

        var bodyBuilder = new BodyBuilder
        {
            HtmlBody = "Это тестовое сообщение отправлено программой \"TFortis Device Manager\" для проверки работы Email.",
            TextBody = "Это тестовое сообщение отправлено программой \"TFortis Device Manager\" для проверки работы Email."
        };
        message.Body = bodyBuilder.ToMessageBody();

        using var client = new SmtpClient();
        using var timeout = new CancellationTokenSource();

        timeout.CancelAfter(5000);

        try
        {
            Status = "Отправка сообщения...";

            await client.ConnectAsync(smtpServer, emailPort, enableSSL, timeout.Token);

            if (AuthenticationEnabled == true)
            {
                await client.AuthenticateAsync(login, pass, timeout.Token);
            }

            await client.SendAsync(message, timeout.Token);

            Foreground = new SolidColorBrush(Colors.Green);
            Status = $"Сообщение  отправлено.";

            Log.Information($"Сообщение отправлено {smtpServer}:{emailPort} From:{fromEmail} To:{toEmail}");

        }
        catch (Exception ex)
        {
            Foreground = new SolidColorBrush(Colors.Red);
            Status = $"Сообщение не отправлено. Ошибка: {ex.Message}";

            Log.Error($"Сообщение не отправлено. Ошибка: {ex.Message}");

        }
        finally
        {
            client.Disconnect(true);
        }
    }

    public AlertSettingsViewModel(IUserSettings userSettings, INotificationBot notificationBot)
    {
        this.DisplayName = Properties.Resources.AlertSettings;

        _userSettings = userSettings ?? throw new ArgumentNullException(nameof(userSettings));
        _notificationBot = notificationBot ?? throw new ArgumentNullException(nameof(notificationBot));

    }

    protected override void OnInitialActivate()
    {
        _botTokenInitialValue = BotToken;

        base.OnInitialActivate();
    }

    protected override void OnPropertyChanged(string propertyName)
    {
        if (propertyName == nameof(BotToken))
        {
            var needRestartApp = BotToken != _botTokenInitialValue;
            SettingsChanged?.Invoke(this, new SettingsStateEventArgs(true));
        }

        base.OnPropertyChanged(propertyName);
    }

    public void Dispose()
    {
        //
    }
}