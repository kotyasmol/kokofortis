using HandyControl.Controls;
using HandyControl.Data;
using System;
using System.Media;
using System.Threading.Tasks;
using TFortisDeviceManager.Models;
using TFortisDeviceManager.Properties;
using TFortisDeviceManager.Telegram;

namespace TFortisDeviceManager.Services
{
    public interface INotificationService
    {
        public Task SendNotificationAsync(EventModel evnt, bool notificationEnabled);
    }

    public class NotificationService : INotificationService, IDisposable
    {
        private readonly IUserSettings _userSettings;
        private readonly IMailSender _mailSender;
        private readonly INotificationBot _notificationBot;
        private readonly SoundPlayer _notificationPlayer = new(Resources.NotificationSound);

        public NotificationService(ISettingsProvider settingsProvider, IMailSender mailSender, INotificationBot notificationBot)
        {
            _userSettings = settingsProvider.UserSettings;
            _notificationBot = notificationBot ?? throw new ArgumentNullException(nameof(notificationBot));
            _mailSender = mailSender;
        }

        public async Task SendNotificationAsync(EventModel evnt, bool notificationEnabled)
        {
            if (evnt.Status == "Problem")
            {
                ShowPushNotification(evnt.FormPushNotification());

               await  _notificationBot.SendTelegramNotificationAsync(evnt.FormTelegramNotification(), notificationEnabled);
            }

            await _mailSender.SendEmailIfNeeded(evnt, notificationEnabled);

        }

        private void ShowPushNotification(string message)
        {
                _notificationPlayer.Play();

                Growl.WarningGlobal(new GrowlInfo
                {
                    StaysOpen = !_userSettings.MonitoringSettings.IsAutoHideNotifications,
                    WaitTime = _userSettings.MonitoringSettings.HideNotificationsAfter,
                    Message = message
                });        
        }

        public void Dispose()
        {
            //
        }
    }
}
