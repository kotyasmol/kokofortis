using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;
using LiveCharts;
using LiveCharts.Wpf;
using Stylet;
using TFortisDeviceManager.Models;

namespace TFortisDeviceManager.ViewModels
{
    public sealed class GraphicsViewModel : Screen, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly Random _random = new Random();
        private readonly Queue<QGraphics> _dataQueue = new Queue<QGraphics>();
        private readonly Dictionary<string, Queue<double>> _lineDataDict;
        private readonly int _maxQueueSize = 10;
        private readonly int _maxPoints = 50;

        private SeriesCollection _seriesCollection;
        public SeriesCollection SeriesCollection
        {
            get { return _seriesCollection; }
            set
            {
                _seriesCollection = value;
                OnPropertyChanged(nameof(SeriesCollection));
            }
        }

        private SeriesCollection _lineSeriesCollection;
        public SeriesCollection LineSeriesCollection
        {
            get { return _lineSeriesCollection; }
            set
            {
                _lineSeriesCollection = value;
                OnPropertyChanged(nameof(LineSeriesCollection));
            }
        }

        public GraphicsViewModel()
        {
            _lineDataDict = new Dictionary<string, Queue<double>>
            {
                { "Series1", new Queue<double>(Enumerable.Repeat(0.0, _maxPoints)) }
            };

            SeriesCollection = new SeriesCollection();
            LineSeriesCollection = new SeriesCollection();
            Labels = Enumerable.Range(0, _maxPoints).Select(i => i.ToString()).ToList();
            InitializeData();
            UpdateDataAsync();
        }

        private List<string> _labels;
        public List<string> Labels
        {
            get { return _labels; }
            set
            {
                _labels = value;
                OnPropertyChanged(nameof(Labels));
            }
        }

        private void InitializeData()
        {
            FillQueueWithZeros();
            InitializePieChart();
            InitializeLineChart();
        }

        private void FillQueueWithZeros()
        {
            for (int i = 0; i < _maxQueueSize; i++)
            {
                _dataQueue.Enqueue(new QGraphics
                {
                    Value = 0,
                    Title = $"Параметр {i + 1}"
                });
            }
        }

        private void AddRandomData()
        {
            var data = new QGraphics
            {
                Value = _random.Next(1, 10),
                Title = $"Параметр {_dataQueue.Count + 1}"
            };
            _dataQueue.Enqueue(data);
        }

        private async void UpdateDataAsync()
        {
            while (true)
            {
                await Task.Delay(TimeSpan.FromSeconds(1));

                if (_dataQueue.Count >= _maxQueueSize)
                {
                    _dataQueue.Dequeue();
                }

                AddRandomData();

                foreach (var key in _lineDataDict.Keys.ToList())
                {
                    if (_lineDataDict[key].Count >= _maxPoints)
                    {
                        _lineDataDict[key].Dequeue();
                    }
                    _lineDataDict[key].Enqueue(_random.Next(1, 100));
                }

                UpdatePieChart();
                UpdateLineChart();
            }
        }

        private void InitializePieChart()
        {
            var colors = new List<Color>
            {
                Color.FromRgb(0, 32, 46),
                Color.FromRgb(44, 72, 117),
                Color.FromRgb(138, 80, 143),
                Color.FromRgb(188, 80, 144),
                Color.FromRgb(255, 99, 97),
                Color.FromRgb(255, 133, 49),
                Color.FromRgb(255, 166, 0),
                Color.FromRgb(128, 211, 83),
                Color.FromRgb(96, 159, 63),
                Color.FromRgb(64, 106, 42)
            };

            int index = 0;
            foreach (var data in _dataQueue)
            {
                SeriesCollection.Add(new PieSeries
                {
                    Title = data.Title,
                    Values = new ChartValues<double> { data.Value },
                    DataLabels = true,
                    Fill = new SolidColorBrush(colors[index % colors.Count])
                });
                index++;
            }
        }

        private void UpdatePieChart()
        {
            var colors = new List<Color>
            {
                Color.FromRgb(0, 32, 46),
                Color.FromRgb(44, 72, 117),
                Color.FromRgb(138, 80, 143),
                Color.FromRgb(188, 80, 144),
                Color.FromRgb(255, 99, 97),
                Color.FromRgb(255, 133, 49),
                Color.FromRgb(255, 166, 0),
                Color.FromRgb(128, 211, 83),
                Color.FromRgb(96, 159, 63),
                Color.FromRgb(64, 106, 42)
            };

            int index = 0;

            foreach (var data in _dataQueue)
            {
                if (index >= SeriesCollection.Count)
                {
                    SeriesCollection.Add(new PieSeries
                    {
                        Title = data.Title,
                        Values = new ChartValues<double> { data.Value },
                        DataLabels = true,
                        Fill = new SolidColorBrush(colors[index % colors.Count])
                    });
                }
                else
                {
                    var series = SeriesCollection[index] as PieSeries;
                    if (series != null)
                    {
                        series.Values[0] = data.Value;
                        series.Title = data.Title;
                        series.Fill = new SolidColorBrush(colors[index % colors.Count]);
                    }
                }
                index++;
            }

            while (SeriesCollection.Count > _dataQueue.Count)
            {
                SeriesCollection.RemoveAt(SeriesCollection.Count - 1);
            }
        }

        private void InitializeLineChart()
        {
            var colors = new List<Color>
            {
                Color.FromRgb(255, 0, 0), // Red
                Color.FromRgb(0, 255, 0), // Green
                Color.FromRgb(0, 0, 255), // Blue
                Color.FromRgb(255, 255, 0) // Yellow
            };

            int index = 0;
            foreach (var key in _lineDataDict.Keys)
            {
                LineSeriesCollection.Add(new LineSeries
                {
                    Title = key,
                    Values = new ChartValues<double>(_lineDataDict[key]),
                    PointGeometry = null,
                    Stroke = new SolidColorBrush(colors[index % colors.Count]),
                    Fill = Brushes.Transparent,
                });
                index++;
            }
        }

        private void UpdateLineChart()
        {
            int index = 0;
            foreach (var key in _lineDataDict.Keys)
            {
                var series = LineSeriesCollection[index] as LineSeries;
                if (series != null)
                {
                    var values = series.Values as ChartValues<double>;
                    if (values != null)
                    {
                        values.Clear();
                        values.AddRange(_lineDataDict[key]);
                    }
                }
                index++;
            }
            Labels = Enumerable.Range(0, _maxPoints).Select(i => (i + 1).ToString()).ToList();
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
