using Stylet;
using System;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Tar;
using System.Diagnostics;
using IWshRuntimeLibrary;
using Microsoft.Win32;
using System.Linq;
using System.Security;
using System.Net;
using System.Windows;

namespace TFortisDeviceManagerSetup.ViewModels
{
    public sealed class InstallViewModel : Screen, IDisposable
    {
        private readonly string progName = "TFortisDeviceManager";
        private readonly string progDir = @"C:\Program Files (x86)\TFortisDeviceManager";
        private readonly string progIcon = @"C:\Program Files (x86)\TFortisDeviceManager\icon.ico";
        private readonly string progDeleteString = @"C:\Program Files (x86)\TFortisDeviceManager\uninst.exe";

        private bool? _startScreenVisibility = true;
        public bool? StartScreenVisibility
        {
            get => _startScreenVisibility;
            set => SetAndNotify(ref _startScreenVisibility, value);
        }

        private bool? _setupScreenVisibility = false;
        public bool? SetupScreenVisibility
        {
            get => _setupScreenVisibility;
            set => SetAndNotify(ref _setupScreenVisibility, value);
        }

        private bool? _exitScreenVisibility = false;
        public bool? ExitScreenVisibility
        {
            get => _exitScreenVisibility;
            set => SetAndNotify(ref _exitScreenVisibility, value);
        }

        private int _setupProgress;
        public int SetupProgress
        {
            get => _setupProgress;
            set => SetAndNotify(ref _setupProgress, value);
        }

        private string? _selectedFolder = @"C:\Program Files (x86)\TFortisDeviceManager";
        public string? SelectedFolder
        {
            get => _selectedFolder;
            set => SetAndNotify(ref _selectedFolder, value);
        }

        private bool _needInstallDatabase = true;
        public bool NeedInstallDatabase
        {
            get => _needInstallDatabase;
            set => SetAndNotify(ref _needInstallDatabase, value);
        }

        private bool _needInstallDotNetRuntime = true;
        public bool NeedInstallDotNetRuntime
        {
            get => _needInstallDotNetRuntime;
            set => SetAndNotify(ref _needInstallDotNetRuntime, value);
        }

        private bool _needCreateShortcut = true;
        public bool NeedCreateShortcut
        {
            get => _needCreateShortcut;
            set => SetAndNotify(ref _needCreateShortcut, value);
        }

        private bool _runApplicationAfterExit = true;
        public bool RunApplicationAfterExit
        {
            get => _runApplicationAfterExit;
            set => SetAndNotify(ref _runApplicationAfterExit, value);
        }


        public async Task StartSetupCommand()
        {
            StartScreenVisibility = false;
            SetupScreenVisibility = true;

            await ExtractFiles();
        }

        private async Task ExtractFiles()
        {
            try
            {
                while (SetupProgress != 67)
                {
                    await Task.Delay(30);
                    SetupProgress++;
                }

                Directory.CreateDirectory(SelectedFolder);

                ExtractTGZ("data.dat", SelectedFolder);

                while (SetupProgress != 100)
                {
                    await Task.Delay(30);
                    SetupProgress++;
                }

                if (NeedInstallDatabase)
                {
                    using TfortisdbContext database = new();

                    InstallDatabase();

                    try
                    {
                        bool isDbExist = database.Database.CanConnect();

                        if (!isDbExist)
                        {
                            CreateDatabase();
                        }
                    }
                    catch
                    {
                        CreateDatabase();
                    }

                    if (!CheckDatabaseFilling())
                    {
                        FillDatabase();
                    }
                }

                if (NeedInstallDotNetRuntime)
                {
                    InstallDotNetRuntime();
                }

                if (NeedCreateShortcut)
                {
                    CreateShortcut();
                }

                AddProgramToControlPanel(progName, progIcon, progDeleteString);

                SetupScreenVisibility = false;
                ExitScreenVisibility = true;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public static void Decompress(FileInfo fileToDecompress)
        {
            using FileStream originalFileStream = fileToDecompress.OpenRead();
            string currentFileName = fileToDecompress.FullName;
            string newFileName = currentFileName.Remove(currentFileName.Length - fileToDecompress.Extension.Length);

            using FileStream decompressedFileStream = System.IO.File.Create(newFileName);
            using GZipStream decompressionStream = new(originalFileStream, CompressionMode.Decompress);
            decompressionStream.CopyTo(decompressedFileStream);
            Console.WriteLine($"Decompressed: {fileToDecompress.Name}");
        }

        public static void ExtractTGZ(String gzArchiveName, String destFolder)
        {
            Stream inStream = System.IO.File.OpenRead(gzArchiveName);
            Stream gzipStream = new GZipInputStream(inStream);

            TarArchive tarArchive = TarArchive.CreateInputTarArchive(gzipStream);
            tarArchive.ExtractContents(destFolder);
            tarArchive.Close();

            gzipStream.Close();
            inStream.Close();
        }

        private static void InstallDatabase()
        {
            Process installation = Process.Start($"{System.Windows.Forms.Application.StartupPath}\\Redist\\PostgreSQL_15.4_64bit_Setup");

            installation.WaitForExit();
        }

        private static void InstallDotNetRuntime()
        {
            Process installation = Process.Start($"{System.Windows.Forms.Application.StartupPath}\\Redist\\windowsdesktop-runtime-7.0.14-win-x64");

            installation.WaitForExit();
        }

        private static void CreateDatabase()
        {
            ProcessStartInfo startInfo = new()
            {
                CreateNoWindow = false,
                UseShellExecute = false,
                FileName = $"{System.Windows.Forms.Application.StartupPath}" + @"Redist\pgsql\createdb.exe",
                WindowStyle = ProcessWindowStyle.Normal,
                Arguments = $" -U postgres tfortis",
            };

            try
            {
                using Process exeProcess = Process.Start(startInfo);
                exeProcess.WaitForExit();
            }
            catch
            {
                //
            }
        }

        private static void FillDatabase()
        {
            ProcessStartInfo startInfo = new()
            {
                CreateNoWindow = false,
                UseShellExecute = false,
                FileName = $"{System.Windows.Forms.Application.StartupPath}" + @"Redist\pgsql\psql.exe",
                WindowStyle = ProcessWindowStyle.Normal,
                Arguments = $" -p 5432 -U postgres -d tfortis -f tfortis.sql",
            };

            try
            {
                using Process exeProcess = Process.Start(startInfo);

                exeProcess.WaitForExit();
            }
            catch
            {
                //
            }
        }

        private static bool CheckDatabaseFilling()
        {
            try
            {
                using TfortisdbContext database = new();

                var deviceTypes = database.DeviceTypes.Count();

                var oidsForDevices = database.OidsForDevices.Count();

                var trapOids = database.TrapOids.Count();

                if (deviceTypes != 0 && oidsForDevices != 0 && trapOids != 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch 
            {

                return false;
            }
        }

        private void CreateShortcut()
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\TFortisDeviceManager.lnk";
            string appPath = SelectedFolder + @"\TFortisDeviceManager.exe";

            WshShell wshShell = new();

            IWshShortcut Shortcut = (IWshShortcut)wshShell.CreateShortcut(desktopPath);

            Shortcut.TargetPath = appPath;

            Shortcut.WorkingDirectory = SelectedFolder;

            Shortcut.Save();
        }

        public void CloseAppCommand()
        {
            if (RunApplicationAfterExit)
            {
                ProcessStartInfo startInfo = new()
                {
                    WorkingDirectory = SelectedFolder,
                    FileName = SelectedFolder + @"\TFortisDeviceManager.exe",
                    Verb = "runas"
                };

                Process.Start(startInfo);
            }

            System.Windows.Application.Current.Shutdown();
        }

        public void SelectFolderCommand()
        {
            using var dialog = new System.Windows.Forms.FolderBrowserDialog
            {
                Description = Properties.Resources.FolderSelect,
                UseDescriptionForTitle = true,
                SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)
                    + Path.DirectorySeparatorChar,
                ShowNewFolderButton = true
            };

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                SelectedFolder = dialog.SelectedPath + @"\TFortisDeviceManager";
            }
        }

        public void AddProgramToControlPanel(string progName, string progIcon, string progDeleteString)
        {
            try
            {
                string registryLocation = @"Software\Microsoft\Windows\CurrentVersion\Uninstall";

                RegistryKey regKey = Registry.LocalMachine.OpenSubKey(registryLocation, true);

                RegistryKey progKey = regKey.CreateSubKey(progName);
                progKey.SetValue("DisplayName", progName, RegistryValueKind.String);
                progKey.SetValue("InstallLocation", SelectedFolder, RegistryValueKind.ExpandString);
                progKey.SetValue("DisplayIcon", progIcon, RegistryValueKind.String);
                progKey.SetValue("UninstallString", progDeleteString, RegistryValueKind.ExpandString);
                progKey.SetValue("DisplayVersion", "3.0.1", RegistryValueKind.String);
                progKey.SetValue("Publisher", "FortTelecom", RegistryValueKind.String);

                Console.WriteLine("Программа была успешно добавлена в Установка и удаление программ");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Во время добавления программы произошла ошибка. Ошибка: {0}", ex.Message);
            }
        }

        public void Dispose()
        {
            //
        }
    }
}
