using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFortisDeviceManager.Models
{
    public interface IMailSenderSettings : INotifyPropertyChanged
    {   
        [DefaultValue("")]
        string FromEmail { get; set; }

        [DefaultValue("")]
        string ToEmails { get; set; }

        [DefaultValue("")]
        string SmtpServer { get; set; }

        [DefaultValue("")]
        int EmailPort { get; set; }

        [DefaultValue(false)]
        bool EnableSSL { get; set; }

        [DefaultValue("")]
        string Login { get; set; }

        [DefaultValue("")]
        string PasswordForEmail { get; set; }

        [DefaultValue("")]
        string SubjectForEmail { get; set; }
    }
}
