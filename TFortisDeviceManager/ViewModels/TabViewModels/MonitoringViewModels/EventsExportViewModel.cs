using HandyControl.Tools.Extension;
using Microsoft.Win32;
using Stylet;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ClosedXML.Excel;
using TFortisDeviceManager.Database;
using TFortisDeviceManager.Models;
using System.Collections;

namespace TFortisDeviceManager.ViewModels
{
    public sealed class EventsExportViewModel : ValidatingModelBase
    {
        public BindableCollection<EventModel> MonitoringEvents { get; set; } = new();

        public ObservableCollection<string> EventModels { get; } = new();
        public ObservableCollection<string> EventIps { get; } = new();
        public ObservableCollection<string> EventParameters { get; } = new();
        public ObservableCollection<string> EventStates { get; } = new();

        public List<string> SelectedModels { get; } = new();
        public List<string> SelectedAddresses { get; } = new();
        public List<string> SelectedParameters { get; } = new();
        public List<string> SelectedStates { get; } = new();


        List<EventModel> eventsToExport = new();

        private DateTime _fromDate;
        public DateTime FromDate
        {
            get => _fromDate;
            set => SetAndNotify(ref _fromDate, value);
        }

        private DateTime _toDate;
        public DateTime ToDate
        {
            get => _toDate;
            set => SetAndNotify(ref _toDate, value);
        }

        public EventsExportViewModel()
        {
            FromDate = DateTime.Today.AddDays(-30);
            ToDate = DateTime.Today;

            RefreshTableCommand();
            UpdateFilters(MonitoringEvents.ToList());
        }

        public void ExportEventsCommand()
        {
            var workBookName = DateTime.Now.ToString("ddMMMyyyy-HHmmss");

            SaveFileDialog saveFileDialog = new()
            {
                Filter = "Excel files (*.xlsx)|*.xlsx",
                FilterIndex = 0,
                RestoreDirectory = true,
                CreatePrompt = true,
                FileName = $"Report {workBookName}.xlsx",
                Title = "Export"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                var wb = new XLWorkbook();

                var ws = wb.Worksheets.Add("Report");

                ws.Cell("A1").Value = Properties.Resources.Time;
                ws.Cell("B1").Value = Properties.Resources.Name;
                ws.Cell("D1").Value = Properties.Resources.Sensor;
                ws.Cell("E1").Value = Properties.Resources.Value;
                ws.Cell("F1").Value = Properties.Resources.Age;
                ws.Cell("G1").Value = Properties.Resources.Description;
                ws.Cell("H1").Value = Properties.Resources.State;
                ws.Cell("I1").Value = Properties.Resources.Location;
                ws.Cell("J1").Value = Properties.Resources.DeviceDescription;

                var row = 1;
                foreach (var evnt in MonitoringEvents)
                {
                    row++;
                    ws.Cell($"A{row}").Value = evnt.Time;
                    ws.Cell($"B{row}").Value = evnt.DeviceName;
                    ws.Cell($"C{row}").Value = evnt.Ip;
                    ws.Cell($"D{row}").Value = evnt.SensorName;
                    ws.Cell($"E{row}").Value = evnt.SensorValueText;
                    ws.Cell($"F{row}").Value = evnt.Age;
                    ws.Cell($"G{row}").Value = evnt.Description;
                    ws.Cell($"H{row}").Value = evnt.Status;
                    ws.Cell($"I{row}").Value = evnt.DeviceLocation;
                    ws.Cell($"J{row}").Value = evnt.DeviceDescription;

                    var cells = ws.Range($"A{row}:J{row}");
                }

                var rngTable = ws.Range($"A1:J{row}");

                var rngHeaders = rngTable.Range("A1:J1");
                rngHeaders.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                rngHeaders.Style.Font.Bold = true;

                rngTable.Style.Border.BottomBorder = XLBorderStyleValues.Thin;

                ws.Columns(1, 10).AdjustToContents();

                wb.SaveAs(saveFileDialog.FileName);
            }
        }

        public void RefreshTableCommand()
        {
            eventsToExport.Clear();
            eventsToExport = PGDataAccess.LoadEventsFromDateToDate(FromDate, ToDate);

            eventsToExport = AgregateEvents(eventsToExport);

            eventsToExport =  eventsToExport.OrderBy(x => x.Time).ToList();

            MonitoringEvents.Clear();
            MonitoringEvents.AddRange(eventsToExport);
        }

        public void SetFiltersCommand(object parameters)
        {
            var filters = (object[])parameters;
            var selectedModels = (IList)filters[0];
            var selectedAddresses = (IList)filters[1];
            var selectedParameters = (IList)filters[2];
            var selectedStates = (IList)filters[3];

            SelectedModels.Clear();
            SelectedAddresses.Clear();
            SelectedParameters.Clear();
            SelectedStates.Clear();

            if (selectedModels.Count > 0)
            {
                foreach (var model in selectedModels)
                {
                    SelectedModels.Add(model.ToString());
                }
            }

            if (selectedAddresses.Count > 0)
            {
                foreach (var address in selectedAddresses)
                {
                    SelectedAddresses.Add(address.ToString());
                }
            }

            if (selectedParameters.Count > 0)
            {
                foreach (var parameter in selectedParameters)
                {
                    SelectedParameters.Add(parameter.ToString());
                }
            }

            if (selectedStates.Count > 0)
            {
                foreach (var state in selectedStates)
                {
                    SelectedStates.Add(state.ToString());
                }
            }

            RefreshTableCommand();
        }

        public void SetDefaultFiltersCommand()
        {
            EventModels.Clear();
            EventIps.Clear();
            EventParameters.Clear();
            EventStates.Clear();

            SelectedModels.Clear();
            SelectedAddresses.Clear();
            SelectedParameters.Clear();
            SelectedStates.Clear();

            RefreshTableCommand();
            UpdateFilters(MonitoringEvents.ToList());
        }

        private List<EventModel> AgregateEvents(List<EventModel> events)
        {
            List<EventModel> agregatedEvents = new();

            List<EventModel> agregatedModels = new();
            List<EventModel> agregatedAddresses = new();
            List<EventModel> agregatedParameters = new();
            List<EventModel> agregatedStates = new();


            agregatedEvents.AddRange(events);

            if (SelectedModels.Count > 0)
            {
                foreach (var model in SelectedModels)
                {
                    agregatedModels.AddRange(agregatedEvents.Where(t => t.DeviceName == model).ToList());
                    agregatedEvents.AddRange(agregatedModels);
                }

                agregatedEvents.Clear();
                agregatedEvents.AddRange(agregatedModels);
            }

            if (SelectedAddresses.Count > 0)
            {
                foreach (var address in SelectedAddresses)
                {
                    agregatedAddresses.AddRange(agregatedEvents.Where(t => t.Ip == address).ToList());
                    agregatedEvents.AddRange(agregatedAddresses);
                }

                agregatedEvents.Clear();
                agregatedEvents.AddRange(agregatedAddresses);
            }

            if (SelectedParameters.Count > 0)
            {
                foreach (var parameter in SelectedParameters)
                {
                    agregatedParameters.AddRange(agregatedEvents.Where(t => t.SensorName == parameter).ToList());
                    agregatedEvents.AddRange(agregatedParameters);
                }

                agregatedEvents.Clear();
                agregatedEvents.AddRange(agregatedParameters);
            }

            if (SelectedStates.Count > 0)
            {
                foreach (var state in SelectedStates)
                {
                    agregatedStates.AddRange(agregatedEvents.Where(t => t.Status == state).ToList());
                    agregatedEvents.AddRange(agregatedStates);
                }
                agregatedEvents.Clear();
                agregatedEvents.AddRange(agregatedStates);
            }

            agregatedEvents = agregatedEvents.Distinct().ToList();

            return agregatedEvents;

        }

        public void UpdateFilters(List<EventModel> eventsToExport)
        {
            var models = eventsToExport.Select(x => x.DeviceName).Distinct().ToList();
            models.Sort();

            EventModels.Clear();
            EventModels.AddRange(models);

            var addresses = eventsToExport.Select(x => x.Ip).Distinct().ToList();
            addresses.Sort();

            EventIps.Clear();
            EventIps.AddRange(addresses);

            var parameters = eventsToExport.Select(x => x.SensorName).Distinct().ToList();
            parameters.Sort();

            EventParameters.Clear();
            EventParameters.AddRange(parameters);

            var statuses = eventsToExport.Select(x => x.Status).Distinct().ToList();
            statuses.Sort();

            EventStates.Clear();
            EventStates.AddRange(statuses);
        }

    }
}
