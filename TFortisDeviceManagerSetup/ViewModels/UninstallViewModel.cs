using Microsoft.Win32;
using Stylet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFortisDeviceManagerSetup.ViewModels
{
    public sealed class UninstallViewModel : Screen, IDisposable
    {
        private string _currentMessage = Properties.Resources.ProgrammWillDeleted;
        public string CurrentMessage
        {
            get => _currentMessage;
            set => SetAndNotify(ref _currentMessage, value);
        }

        private string _currentAction = Properties.Resources.Delete;
        public string CurrentAction
        {
            get => _currentAction;
            set => SetAndNotify(ref _currentAction, value);
        }

        private bool _isProgress = false;
        public bool IsProgress
        {
            get => _isProgress;
            set => SetAndNotify(ref _isProgress, value);
        }

        private int _uninstallProgress = 0;
        public int UninstallProgress
        {
            get => _uninstallProgress;
            set => SetAndNotify(ref _uninstallProgress, value);
        }

        public async Task UninstallAppCommand()
        {
            if (CurrentAction == Properties.Resources.Delete)
            {
                string applicationName = "TFortisDeviceManager";

                CurrentAction = Properties.Resources.Finish;

                IsProgress = true;

                CurrentMessage = string.Empty;

                Uninstall(applicationName);

                DeleteShortcut();

                while (UninstallProgress != 100)
                {
                    UninstallProgress++;
                    await Task.Delay(10);
                }

                IsProgress = false;

                CurrentMessage = Properties.Resources.ProgrammDeleted;

            }
            else
            {
                System.Windows.Application.Current.Shutdown();
            }
        }

        static void DeleteShortcut()
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            File.Delete(Path.Combine(desktopPath, "TFortisDeviceManager.lnk"));
        }

        static void Uninstall(string applicationName)
        {       
            try
            {
                RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall");

                string installPath = GetProgramInstallPath(applicationName);
                if (Directory.Exists(installPath))
                {
                    Directory.Delete(installPath, true);
                    Console.WriteLine("Файлы и папки удалены.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при удалении файлов и папок: {ex.Message}");
            }

            try
            {
                RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall", true);
                key.DeleteSubKeyTree(applicationName);
                Console.WriteLine("Записи из реестра удалены.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при удалении записей из реестра: {ex.Message}");
            }
        }

        static string GetProgramInstallPath(string programName)
        {
            RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall");

            if (key != null)
            {
                foreach (string subKeyName in key.GetSubKeyNames())
                {
                    RegistryKey subKey = key.OpenSubKey(subKeyName);

                    object displayName = subKey.GetValue("DisplayName");
                    object installLocation = subKey.GetValue("InstallLocation");

                    if (displayName != null && displayName.ToString() == programName)
                    {
                        return installLocation?.ToString();
                    }
                }
            }

            return null;
        }

        public void Dispose()
        {
            //
        }
    }
}
