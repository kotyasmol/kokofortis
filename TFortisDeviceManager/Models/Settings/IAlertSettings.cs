using System.Collections.Generic;
using System.ComponentModel;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TFortisDeviceManager.Models;

public interface IAlertSettings : INotifyPropertyChanged
{
    [DefaultValue("")]
    string FromEmail { get; set; }

    [DefaultValue("")]
    string ToEmails { get; set; }

    [DefaultValue("smtp.yandex.ru")]
    string SmtpServer { get; set; }

    [DefaultValue("465")]
    int EmailPort { get; set; }

    [DefaultValue(true)]
    bool EnableSSL { get; set; }

    [DefaultValue("")]
    string Login { get; set; }

    [DefaultValue("")]
    string PasswordForEmail { get; set; }

    [DefaultValue("")]
    string SubjectForEmail { get; set; }

    [DefaultValue(true)]
    bool AuthenticationEnabled { get; set; }

    [DefaultValue(true)]
    bool AccidentExpanding { get; set; }

    [DefaultValue("")]
    string BotToken { get; set; }

    [DefaultValue("")]
    string UserList { get; set; }

    [DefaultValue("")]
    string ChatIdToNotifyList { get; set; }
}