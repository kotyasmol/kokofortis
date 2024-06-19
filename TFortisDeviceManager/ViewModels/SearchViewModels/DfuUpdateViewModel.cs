using Stylet;
using System.Threading;
using System;
using System.Threading.Tasks;
using TFortisDeviceManager.Models;
using TFortisDeviceManager.Services;
using System.Diagnostics.CodeAnalysis;
using Serilog;
using System.ComponentModel;
using System.Collections.ObjectModel;
using Microsoft.IdentityModel.Tokens;
using System.IO;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Net.Http.Headers;
using Microsoft.Win32;
using HandyControl.Controls;
using DocumentFormat.OpenXml.Wordprocessing;
using Org.BouncyCastle.Utilities.Net;
using System.Collections.Generic;
using HandyControl.Tools.Extension;

namespace TFortisDeviceManager.ViewModels
{
    public sealed class DfuUpdateViewModel : Screen, IDisposable
    {
        private NetworkDevice _networkDevice;
        public NetworkDevice NetworkDevice
        {
            get { return _networkDevice; }
            set { _networkDevice = value; OnPropertyChanged(nameof(NetworkDevice)); }
        }

        private string _ipAddress = "";
        public string IpAddress
        {
            get => _ipAddress;
            set
            {
                _ipAddress = value;
                _networkDevice.IpAddress = value;
                OnPropertyChanged(nameof(IpAddress));
            }
        }

        public List<NetworkDevice> FoundDevices { get; set; } = new List<NetworkDevice>();

        private string _dfuUpdatingProgress = "0";
        public string DfuUpdatingProgress
        {
            get => _dfuUpdatingProgress;
            set => SetAndNotify(ref _dfuUpdatingProgress, value);
        }

        private bool _dfuUpdatingInProgress;
        public bool DfuUpdatingInProgress
        {
            get => _dfuUpdatingInProgress;
            set => SetAndNotify(ref _dfuUpdatingInProgress, value);
        }

        private bool _canStartUpload;
        public bool CanStartUpload
        {
            get => _canStartUpload;
            set => SetAndNotify(ref _canStartUpload, value);
        }

        private bool _canStartUpdate;
        public bool CanStartUpdate
        {
            get => _canStartUpdate;
            set => SetAndNotify(ref _canStartUpdate, value);
        }

        private bool _canSelectFile;
        public bool CanSelectFile
        {
            get => _canSelectFile;
            set => SetAndNotify(ref _canSelectFile, value);
        }

        private bool _isDfuFileSelected;
        public bool IsDfuFileSelected
        {
            get => _isDfuFileSelected;
            set => SetAndNotify(ref _isDfuFileSelected, value);
        }

        private string? _message;
        public string? Message
        {
            get => _message;
            set => SetAndNotify(ref _message, value);
        }

        private string? _selectedDfuFile = "";
        public string? SelectedDfuFile
        {
            get => _selectedDfuFile;
            set => SetAndNotify(ref _selectedDfuFile, value);
        }

        public async Task UploadDfuCommand()
        {
            if (IpAddress == null) return;

            if(DfuUpdatingInProgress) return;

            DfuUpdatingProgress = "0";

            DfuUpdatingInProgress = true;
            CanStartUpdate = false;
            CanStartUpload = false;
            CanSelectFile = false;

            if (SelectedDfuFile.IsNullOrEmpty())
            {
                Message = Properties.Resources.ChooseDfuFile;
                return;
            }

            HttpClient _client = new();

                string ip = IpAddress;
            try
            {
                Message = Properties.Resources.PreparingToUpload;

                await _client.GetAsync($"http://{ip}/clear.shtml");
            
                await Task.Delay(10000);

                await _client.GetAsync($"http://{ip}/mngt/update.shtml?Upload=Upload");              
            }
            catch
            {
                MessageBox.Show(Properties.Resources.ErrorWhileClearingFlash);
                Message = Properties.Resources.ErrorWhileClearingFlash;
                return;
            }

            var filePath = SelectedDfuFile;

            byte[] fileToBytes = await File.ReadAllBytesAsync(filePath);

            try
            {
                Message = Properties.Resources.FileUploading;

                UploadFilesToServer(new Uri($"http://{ip}/mngt/update.shtml"), Path.GetFileName(SelectedDfuFile), "application/octet-stream", fileToBytes);

                DfuUpdatingProgress = "0";

                while (DfuUpdatingProgress != "99" && DfuUpdatingProgress != "100")
                {
                    DfuUpdatingProgress = UpdateUploadProgress(ip);

                    if (DfuUpdatingProgress == "99")
                    {
                        DfuUpdatingProgress = "100";

                        return;
                    }

                    await Task.Delay(500);
                }
            }
            catch
            {
                MessageBox.Show(Properties.Resources.ErrorWhileUploading);
                Message = Properties.Resources.ErrorWhileUploading;
                return;
            }
        }

        public async Task UpdateDfuCommand()
        {
            if (IpAddress.IsNullOrEmpty()) return;

            if (DfuUpdatingInProgress) return;

            DfuUpdatingProgress = "0";
            DfuUpdatingInProgress = true;
            CanStartUpdate = false;

            HttpClient _client = new();

            string ip = IpAddress;

            try
            {
                await _client.GetAsync($"http://{ip}/mngt/update.shtml?Update=Update&update=1");

                Message = $"{Properties.Resources.UpdatingWait}...";

                int progress = 0;

                for (int i = 0; i < 40; i++)
                {
                    await Task.Delay(1000);
                    progress += 2;
                    DfuUpdatingProgress = progress.ToString();
                }

                for (int i = 0; i < 20; i++)
                {
                    await Task.Delay(50);
                    progress += 1;
                    DfuUpdatingProgress = progress.ToString();
                }

                DfuUpdatingProgress = "100";
            }
            catch
            {
                MessageBox.Show(Properties.Resources.ErrorWhileUpdating);
                Message = Properties.Resources.ErrorWhileUpdating;
                return;
            }

            Message = Properties.Resources.DfuUpdated;
            CanStartUpload = false;
            CanStartUpdate = false;
        }

        public void SelectFileCommand()
        {
            if (DfuUpdatingInProgress) return;

            OpenFileDialog openFileDialog = new()
            {
                Filter = "IMG files (*.img)|*.img|All files (*.*)|*.*",
                FilterIndex = 0
            };

            if (openFileDialog.ShowDialog() != true)
                return;

            string filePath = openFileDialog.FileName;

            SelectedDfuFile = filePath;

            if (!SelectedDfuFile.IsNullOrEmpty())
            {
                IsDfuFileSelected = true;
                Message = $"{Properties.Resources.SelectedFile}: {Path.GetFileName(SelectedDfuFile)}";

                CanStartUpdate = false;
                CanStartUpload = true;
            }
        }

        public void SetDevice(object? sender, ChangingSelectedDeviceEventArgs? e)
        {
            if (!IsNotNull(e) || !IsNotNull(e.Device)) return;

            NetworkDevice device = e.Device;

            NetworkDevice = device;

            IpAddress = device.IpAddress;

            FoundDevices = e.FoundDevices;

            DfuUpdatingInProgress = false;

            if (SelectedDfuFile.IsNullOrEmpty())
            {
                CanStartUpload = false;
            }
            else
            {
                CanStartUpload = true;
            }

            CanSelectFile = true;

            CanStartUpdate = false;

            Message = "";
        }

        private static string UpdateUploadProgress(string ip)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create($"http://{ip}/updateprogres.shtml ");

            string responseData = string.Empty;
            HttpWebResponse httpResponse = (HttpWebResponse)webRequest.GetResponse();
            using (StreamReader responseReader = new(httpResponse.GetResponseStream()))
            {
                responseData = responseReader.ReadToEnd();
            }

            return responseData;
        }

        private void UploadFilesToServer(Uri uri, string fileName, string fileContentType, byte[] fileData)
        {
            string boundary = "----------------------------723690991551375881941828858";
            HttpWebRequest httpWebRequest = WebRequest.Create(uri) as HttpWebRequest;
            httpWebRequest.ContentType = "multipart/form-data; boundary=" + boundary;
            httpWebRequest.Method = "POST";
            httpWebRequest.BeginGetRequestStream((result) =>
            {
                try
                {
                    HttpWebRequest request = result.AsyncState as HttpWebRequest;
                    using (Stream requestStream = request.EndGetRequestStream(result))
                    {
                        WriteMultipartForm(requestStream, boundary, fileName, fileContentType, fileData);
                    }
                    request.BeginGetResponse(a =>
                    {
                        try
                        {
                            var response = request.EndGetResponse(a);
                            var responseStream = response.GetResponseStream();
                            using var sr = new StreamReader(responseStream);
                            using StreamReader streamReader = new(response.GetResponseStream());
                            string responseString = streamReader.ReadToEnd();

                            if (responseString.Contains("Версия загруженной прошивки"))
                            {
                                Message = $"{Properties.Resources.File} {Path.GetFileName(SelectedDfuFile)}\n{Properties.Resources.UploadedSuccessful}";
                                DfuUpdatingInProgress = false;
                                DfuUpdatingProgress = "0";
                                CanStartUpdate = true;
                            }
                            else
                            {
                                if (responseString.Contains("Ошибка"))
                                {
                                    Message = Properties.Resources.ErrorWhileUpdating;
                                    CanStartUpdate = false;
                                }
                            }
                        }
                        catch (Exception)
                        {

                        }
                    }, null);
                }
                catch (Exception)
                {

                }
            }, httpWebRequest);
        }

        private void WriteMultipartForm(Stream s, string boundary, string fileName, string fileContentType, byte[] fileData)
        {
            byte[] boundarybytes = Encoding.UTF8.GetBytes("--" + boundary + "\r\n");
            byte[] trailer = Encoding.UTF8.GetBytes("\r\n--" + boundary + "--\r\n");

            string formdataTemplate = $"Content-Disposition: form-data; name=\"updatefile\"; filename=\"{SelectedDfuFile}\";\r\nContent-Type: application/octet-stream\r\n\r\n";

            bool bNeedsCRLF = false;

            if (bNeedsCRLF)
                WriteToStream(s, "\r\n");

            WriteToStream(s, boundarybytes);
            WriteToStream(s, string.Format(formdataTemplate, "file", fileName, fileContentType));

            WriteToStream(s, fileData);
            WriteToStream(s, trailer);
        }

        private static void WriteToStream(Stream s, string txt)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(txt);
            s.Write(bytes, 0, bytes.Length);
        }

        private static void WriteToStream(Stream s, byte[] bytes)
        {
            s.Write(bytes, 0, bytes.Length);
        }

        private static bool IsNotNull([NotNullWhen(true)] object? obj) => obj != null;      

        public void Dispose()
        {
            //
        }
    }
}
