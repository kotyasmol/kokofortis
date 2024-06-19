using System.Windows;

namespace TFortisDeviceManager.Services
{
    public interface IMessageBoxService
    {
        MessageBoxResult ShowQuestion(string message, string title);
        MessageBoxResult ShowNotification(string message, string title);
        MessageBoxResult ShowError(string message, string title);
    }

    public class MessageBoxService : IMessageBoxService
    {
        public MessageBoxResult ShowQuestion(string message, string title)
        {
            return HandyControl.Controls.MessageBox.Show(message, title, button: MessageBoxButton.YesNo, icon: MessageBoxImage.Question, MessageBoxResult.None);
        }        

        public MessageBoxResult ShowNotification(string message, string title)
        {
            return HandyControl.Controls.MessageBox.Show(message, title, button: MessageBoxButton.OK, icon: MessageBoxImage.Information, MessageBoxResult.None);
        }

        public MessageBoxResult ShowError(string message, string title)
        {
            return HandyControl.Controls.MessageBox.Show(message, title, button: MessageBoxButton.OK, icon: MessageBoxImage.Error, MessageBoxResult.None);
        }
    }
}
