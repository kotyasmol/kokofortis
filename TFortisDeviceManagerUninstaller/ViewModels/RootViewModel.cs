﻿using Stylet;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace TFortisDeviceManagerUninstaller.ViewModels
{
    public class RootViewModel : Conductor<IScreen>.Collection.OneActive, IDisposable
    {

        private static string _applicationPath = "";
        public static string ApplicationPath
        {
            get => _applicationPath;
            set =>  _applicationPath = value;
        }

        private static bool _isUninstallFinished = false;
        public static bool IsUninstallFinished
        {
            get => _isUninstallFinished;
            set => _isUninstallFinished = value;
        }

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

        static void Uninstall(string applicationName)
        {
            try
            {
                DeleteShortcut();

                IsUninstallFinished = true;
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

        static void DeleteShortcut()
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            System.IO.File.Delete(Path.Combine(desktopPath, "TFortisDeviceManager.lnk"));
        }

        public void Dispose()
        {
            //
        }
    }
}
