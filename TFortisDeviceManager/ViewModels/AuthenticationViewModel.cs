using Stylet;

namespace TFortisDeviceManager.ViewModels
{
    public sealed class AuthenticationViewModel : ValidatingModelBase
    {
        public static string Title => Properties.Resources.AuthenticationTitle;

        private string? _login;
        public string? Login
        {
            get => _login;
            set => SetAndNotify(ref _login, value);
        }

        private string? _password;
        public string? Password
        {
            get => _password;
            set => SetAndNotify(ref _password, value);
        }
    }
}
