using Stylet;
using System;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using System.Windows.Forms;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Tar;
using System.Diagnostics;
using IWshRuntimeLibrary;
using Microsoft.Win32;
using System.Linq;
using System.Threading;
using System.Globalization;

namespace TFortisDeviceManagerSetup.ViewModels
{
    public class RootViewModel : Conductor<IScreen>.Collection.OneActive, IDisposable
    {
        private readonly InstallViewModel _installScreen;
        private readonly UninstallViewModel _uninstallScreen;

        private bool _actionSelected = false;
        public bool ActionSelected
        {
            get => _actionSelected;
            set => SetAndNotify(ref _actionSelected, value);
        }

        private bool _uninstallSelected = true;
        public bool UninstallSelected
        {
            get => _uninstallSelected;
            set => SetAndNotify(ref _uninstallSelected, value);
        }

        private bool _repairSelected = true;
        public bool RepairSelected
        {
            get => _repairSelected;
            set => SetAndNotify(ref _repairSelected, value);
        }

        private string _appLanguage = "ru-RU";

        public RootViewModel(InstallViewModel installScreen, UninstallViewModel uninstallScreen)
        {
            _installScreen = installScreen ?? throw new System.ArgumentNullException(nameof(installScreen));
            _uninstallScreen = uninstallScreen ?? throw new System.ArgumentNullException(nameof(uninstallScreen));

           /* CultureInfo culture = _appLanguage == "en"
                ? CultureInfo.CreateSpecificCulture("en")
                : CultureInfo.CreateSpecificCulture("ru-RU");

            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;*/

            if (!CheckInstalled("TFortisDeviceManager"))
            {
                ActiveItem = _installScreen;
                ActionSelected = true;
            }
        }

        public void SetAppLanguage()
        {
            CultureInfo culture = _appLanguage == "en"
               ? CultureInfo.CreateSpecificCulture("ru-RU")
               : CultureInfo.CreateSpecificCulture("en");

            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        public static bool CheckInstalled(string appName)
        {
            string displayName;

            string registryKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
            RegistryKey key = Registry.LocalMachine.OpenSubKey(registryKey);
            if (key != null)
            {
                foreach (RegistryKey subkey in key.GetSubKeyNames().Select(keyName => key.OpenSubKey(keyName)))
                {
                    displayName = subkey.GetValue("DisplayName") as string;
                    if (displayName != null && displayName.Contains(appName))
                    {
                        return true;
                    }
                }
                key.Close();
            }
    
            return false;
        }

        public void ChangeActiveItemCommand()
        {
            ActionSelected = true;

            if (RepairSelected)
            {
                ActiveItem = _installScreen;
            }else
            {
                if (UninstallSelected)
                {
                    ActiveItem = _uninstallScreen;
                }
            }
        }

        public void Dispose()
        {
            //
        }
    }
}
