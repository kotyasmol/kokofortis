using HandyControl.Tools.Extension;
using Microsoft.VisualBasic.Logging;
using Stylet;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace TFortisDeviceManagerSetup.ViewModels
{
    public sealed class LanguageSelectorViewModel : ValidatingModelBase
    {
        private readonly IWindowManager _windowManager;
        private readonly Func<RootViewModel> _rootViewModelFactory;
        private readonly Func<UninstallViewModel> _unstallViewModelFactory;
        private readonly Func<InstallViewModel> _installViewModelFactory;

        public LanguageSelectorViewModel(IWindowManager windowManager, 
            Func<RootViewModel> rootViewModelFactory, 
            Func<UninstallViewModel> unstallViewModelFactory, 
            Func<InstallViewModel> installViewModelFactory)
        {
            _windowManager = windowManager;
            _rootViewModelFactory = rootViewModelFactory;
            _installViewModelFactory = installViewModelFactory;
            _unstallViewModelFactory = unstallViewModelFactory;
        }

        public void SelectRussianCommand()
        {
            CultureInfo culture = CultureInfo.CreateSpecificCulture("ru-RU");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            var rootViewModel = _rootViewModelFactory();

            _windowManager.ShowWindow(rootViewModel);

            Application.Current.Windows[0].Close();
        }

        public void SelectEnglishCommand()
        {
            CultureInfo culture = CultureInfo.CreateSpecificCulture("en");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            var rootViewModel = _rootViewModelFactory();

            _windowManager.ShowWindow(rootViewModel);

            Application.Current.Windows[0].Close();
        }

    }
}
