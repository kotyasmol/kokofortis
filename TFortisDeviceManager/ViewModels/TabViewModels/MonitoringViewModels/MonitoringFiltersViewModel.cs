using HandyControl.Tools.Extension;
using Stylet;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TFortisDeviceManager.Models;

namespace TFortisDeviceManager.ViewModels
{
    public sealed class MonitoringFiltersViewModel : Screen, IDisposable
    {
        private List<string?> models = new();
        private List<string?> addresses = new();
        private List<string?> parameters = new();
        private List<string?> statuses = new();

        public ObservableCollection<string> ModelsFromDataFromDb { get; set; } = new();
        public ObservableCollection<string> IpsFromDataFromDb { get; set; } = new();
        public ObservableCollection<string> ParametersFromDb { get; set; } = new();
        public ObservableCollection<string> EventStatusFromDb { get; set; } = new();

        public List<string> SelectedModels { get; set; } = new();
        public List<string> SelectedAddresses { get; set; } = new();
        public List<string> SelectedParameters { get; set; } = new();
        public List<string> SelectedStates { get; set; } = new();

        public void InitComboBoxes(List<EventModel> eventsFromDb)
        {
            models = eventsFromDb.Select(x => x.DeviceName).Distinct().ToList();
            ModelsFromDataFromDb.Clear();
            models.Sort();
            ModelsFromDataFromDb.AddRange(models);
            
            addresses = eventsFromDb.Select(x => x.Ip).Distinct().ToList();
            IpsFromDataFromDb.Clear();
            addresses.Sort();
            IpsFromDataFromDb.AddRange(addresses);

            parameters = eventsFromDb.Select(x => x.SensorName).Distinct().ToList();
            ParametersFromDb.Clear();
            parameters.Sort();
            ParametersFromDb.AddRange(parameters);

            statuses = eventsFromDb.Select(x => x.Status).Distinct().ToList();
            EventStatusFromDb.Clear();
            statuses.Sort();
            EventStatusFromDb.AddRange(statuses);          
        }

        public MonitoringFiltersViewModel() 
        {
            DisplayName = "Filters";
        }

        public void Dispose()
        {
            //
        }
    }
}
