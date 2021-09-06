using System;
using System.Collections.Generic;
using System.Text;
using LiveCharts.Wpf;
using LiveCharts.Defaults;
using LiveCharts;
using TPR_lab6.Models;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;

namespace TPR_lab6.ViewModels
{
    public class MainViewModel : BaseViewModel
    {

        private string[] labels;
        private double xPointer;
        private double yPointer;
        private string selectedGT;
        private string selectedInd;
        private bool isEnabled;
        private Visibility isVisible;
        private Dictionary<string, bool> indStatus;
        private RelayCommand importCommand;

        public ObservableCollection<StockData> Data { get; set; }
        public SeriesCollection Graphic { get; set; }
        public SeriesCollection VolGraphic { get; set; }
        public SeriesCollection IndGraphic { get; set; }
        public Func<double, string> Formatter { get; set; }
        public string[] Labels
        {
            get => labels;
            set
            {
                labels = value;
                OnPropertyChanged("Labels");
            }
        }
        public double XPointer
        {
            get => xPointer;
            set
            {
                if (value.Equals(xPointer)) return;
                xPointer = value;
                OnPropertyChanged(nameof(XPointer));
            }
        }
        public double YPointer
        {
            get => yPointer;
            set
            {
                if (value.Equals(yPointer)) return;
                yPointer = value;
                OnPropertyChanged(nameof(YPointer));
            }
        }
        public string SelectedGT
        {
            get => selectedGT;
            set
            {
                selectedGT = value.Split(':')[1].Trim();
                OnPropertyChanged(nameof(SelectedGT));
                if (Data != null)
                {
                    SetGraphic();
                    OnPropertyChanged(nameof(Graphic));
                }
            }
        }
        public string SelectedInd
        {
            get => selectedInd;
            set
            {
                selectedInd = value.Split(':')[1].Trim();
                indStatus[selectedInd] = !indStatus[selectedInd];
                OnPropertyChanged(nameof(SelectedInd));
                if (Data != null)
                {
                    SetIndGraphic();
                    OnPropertyChanged(nameof(IndGraphic));
                }
            }
        }
        public bool IsEnabled
        {
            get => isEnabled;
            set
            {
                isEnabled = value;
                OnPropertyChanged(nameof(IsEnabled));
                if (value == true)
                    IsVisible = Visibility.Visible;
                else
                    IsVisible = Visibility.Hidden;
            }
        }
        public Visibility IsVisible
        {
            get => isVisible;
            set
            {
                isVisible = value;
                OnPropertyChanged(nameof(isVisible));
            }
        }

        // commands
        public RelayCommand ImportCommand => importCommand ?? (importCommand = new RelayCommand(func => Import()));

        public MainViewModel()
        {
            XPointer = -5;
            YPointer = -5;
            selectedGT = "Свечи";
            selectedInd = "RSI";
            isEnabled = true;
            isVisible = Visibility.Visible;
            Formatter = x => (x > 1000 || x < -1000) ? ((x > 1000000 || x < -1000000) ? (x / 1000000).ToString("N2") + "M" : (x / 1000).ToString("N2") + "k") : x.ToString("N2");
            
            indStatus = new Dictionary<string, bool>();
            indStatus.Add("RSI", false);
            indStatus.Add("MACD", false);
            indStatus.Add("SMA", false);
            indStatus.Add("EMA", false);
            indStatus.Add("A/D", false);
        }

        private void Import()
        {
            OpenFileDialog OFD = new OpenFileDialog
            {
                InitialDirectory = "C:\\Users\\timzl\\Documents\\7_Semester\\Теория принятия решения\\6lab",
                Filter = "CSV (*.csv) |*.csv",
                FilterIndex = 1,
                RestoreDirectory = true
            };
            var fileName = OFD.ShowDialog() == true ? OFD.FileName : null;
            if (fileName != null)
            {
                Data = new ObservableCollection<StockData>(DataImport.ReadCSV(fileName));

                SetLabels();
                Graphic = new SeriesCollection();
                SetGraphic();
                VolGraphic = new SeriesCollection();
                SetVolGraphic();
                IndGraphic = new SeriesCollection();
                SetIndGraphic();
            }
        }
        private void SetLabels()
        {
            Labels = new string[Data.Count];
            for (int i = 0; i < Data.Count; i++)
                Labels[i] = Data[i].DateTime.Date.ToString("dd MMMM");
        }
        private void SetGraphic()
        {
            Graphic = new SeriesCollection();
            switch (SelectedGT)
            {
                case "Свечи":
                    {
                        ChartValues<OhlcPoint> points = new ChartValues<OhlcPoint>();
                        foreach (var el in Data)
                            points.Add(new OhlcPoint(el.Open, el.High, el.Low, el.Close));
                        CandleSeries candleSeries = new CandleSeries { Values = points, Title = Data[0].Ticker };
                        Graphic.Add(candleSeries);
                        break;
                    }
                case "Бары":
                    {
                        ChartValues<OhlcPoint> points = new ChartValues<OhlcPoint>();
                        foreach (var el in Data)
                            points.Add(new OhlcPoint(el.Open, el.High, el.Low, el.Close));
                        OhlcSeries ohlcSeries = new OhlcSeries { Values = points, Title = Data[0].Ticker };
                        Graphic.Add(ohlcSeries);
                        break;
                    }
                case "Линии":
                    {
                        ChartValues<double> points = new ChartValues<double>();
                        foreach (var el in Data)
                            points.Add(el.Close);
                        LineSeries lineSeries = new LineSeries
                        {
                            Values = points,
                            StrokeThickness = 2,
                            LineSmoothness = 0,
                            PointGeometry = null,
                            Fill = System.Windows.Media.Brushes.Transparent,
                            Title = Data[0].Ticker
                        };
                        Graphic.Add(lineSeries);
                        break;
                    }
            }

            OnPropertyChanged(nameof(Graphic));
        }
        private void SetVolGraphic()
        {
            ChartValues<double> points = new ChartValues<double>();
            foreach (var el in Data)
                points.Add(el.Vol);
            VolGraphic.Add(new ColumnSeries { Values = points });
            OnPropertyChanged(nameof(VolGraphic));
        }
        private void SetIndGraphic()
        {
            switch (SelectedInd)
            {
                case "RSI":
                    {

                        IndGraphic = RSI(14);
                        break;
                    }
                case "SMA":
                    {
                        if (indStatus[SelectedInd])
                            Graphic.Add(SMA(14));
                        else
                            try
                            {
                                for (int i = 0; i < Graphic.Count; i++)
                                    if (Graphic[i].Title == "SMA")
                                    {
                                        Graphic.RemoveAt(i);
                                        i--;
                                    };
                            }
                            catch { }
                        break;
                    }
                case "MACD":
                    {

                        IndGraphic = MACD(12, 26, 9);
                        break;
                    }
                case "EMA":
                    {
                        if (indStatus[SelectedInd])
                            Graphic.Add(EMA(6));
                        else
                            try
                            {
                                for (int i = 0; i < Graphic.Count; i++)
                                    if (IndGraphic[i].Title == "EMA")
                                    {
                                        Graphic.Remove(i);
                                        i--;
                                    }
                            }
                            catch { }
                        break;
                    }
                case "A/D":
                    {
                        IndGraphic = AD();
                        break;
                    }
            }
            OnPropertyChanged(nameof(Graphic));
            OnPropertyChanged(nameof(IndGraphic));
        }


        #region RSI
        public SeriesCollection RSI(int period)
        {
            ChartValues<double> points = new ChartValues<double>();
            for (int i = 0; i < period; i++)
                points.Add(double.NaN);
            for (int i = period; i < Data.Count; i++)
                points.Add(100 - (100 / (1 + getU(i, period) / getD(i, period))));

            LineSeries lineSeries = new LineSeries
            {
                Values = points,
                StrokeThickness = 2,
                LineSmoothness = 0,
                PointGeometry = null,
                Fill = System.Windows.Media.Brushes.Transparent,
                Stroke = System.Windows.Media.Brushes.Red,
                Title = "RSI"
            };

            // y=70
            ChartValues<double> points70 = new ChartValues<double>();
            for (int i = 0; i < Data.Count; i++)
                points70.Add(70);
            LineSeries lines70 = new LineSeries
            {
                Values = points70,
                StrokeThickness = 1,
                LineSmoothness = 0,
                PointGeometry = null,
                Fill = System.Windows.Media.Brushes.Transparent,
                Stroke = System.Windows.Media.Brushes.DarkRed,
                Title = "RSI",
                Focusable = false,

            };

            // y=30
            ChartValues<double> points30 = new ChartValues<double>();
            for (int i = 0; i < Data.Count; i++)
                points30.Add(30);
            LineSeries lines30 = new LineSeries
            {
                Values = points30,
                StrokeThickness = 1,
                LineSmoothness = 0,
                PointGeometry = null,
                Fill = System.Windows.Media.Brushes.Transparent,
                Stroke = System.Windows.Media.Brushes.DarkRed,
                Title = "RSI"
            };

            // y=50
            ChartValues<double> points50 = new ChartValues<double>();
            for (int i = 0; i < Data.Count; i++)
                points50.Add(50);
            LineSeries lines50 = new LineSeries
            {
                Values = points50,
                StrokeThickness = 1,
                LineSmoothness = 0,
                PointGeometry = null,
                Fill = System.Windows.Media.Brushes.Transparent,
                Stroke = System.Windows.Media.Brushes.DarkOrange,
                Title = "RSI"
            };

            ChartValues<double> buyP = new ChartValues<double>();
            for (int i = 0; i < Data.Count; i++)
                if (points[i] < 30)
                    buyP.Add(points[i]);
                else
                    buyP.Add(double.NaN);
            LineSeries buySeries = new LineSeries
            {
                Values = buyP,
                StrokeThickness = 0,
                PointGeometrySize = 10,
                PointGeometry = DefaultGeometries.Circle,
                Fill = System.Windows.Media.Brushes.Transparent,
                PointForeground = System.Windows.Media.Brushes.Red,
                Title = "RSI",
                Focusable = false
            };

            ChartValues<double> sellP = new ChartValues<double>();
            for (int i = 0; i < Data.Count; i++)
                if (points[i] > 70)
                    sellP.Add(points[i]);
                else
                    sellP.Add(double.NaN);
            LineSeries sellSeries = new LineSeries
            {
                Values = sellP,
                StrokeThickness = 0,
                PointGeometry = DefaultGeometries.Circle,
                PointGeometrySize = 10,
                Fill = System.Windows.Media.Brushes.Transparent,
                PointForeground = System.Windows.Media.Brushes.Green,
                Title = "RSI",
                Focusable = false
            };

            return new SeriesCollection { lines70, lines30, lines50, lineSeries, buySeries, sellSeries };
        }
        private double getU(int index, int period)
        {
            double sum = 0;
            for (int i = ((index - period) <= 1) ? 1 : (index - period); i <= index; i++)
            {
                if (Data[i].Close > Data[i - 1].Close)
                    sum += Data[i].Close;
            }
            if (sum == 0)
                sum = Data[0].Close;
            return sum / (index + 1);
        }
        private double getD(int index, int period)
        {
            double sum = 0;
            for (int i = ((index - period) <= 1) ? 1 : (index - period); i <= index; i++)
            {
                if (Data[i].Close <= Data[i - 1].Close)
                    sum += Data[i].Close;
            }
            if (sum == 0)
                sum = Data[0].Close;
            return sum / (index + 1);
        }
        #endregion

        #region MACD
        private SeriesCollection MACD(int m, int n, int k)
        {
            ChartValues<double> points = new ChartValues<double>();
            var emam = getSMAi(m - 1, m);
            var eman = getSMAi(n - 1, n);
            for (int i = 0; i < n; i++)
                points.Add(double.NaN);
            for (int i = n; i < Data.Count; i++)
            {
                eman = getEMAi(eman, n, i);
                emam = getEMAi(emam, m, i);
                points.Add(emam - eman);
            }
            LineSeries lineSeries = new LineSeries
            {
                Values = points,
                StrokeThickness = 2,
                LineSmoothness = 1,
                PointGeometry = null,
                Fill = System.Windows.Media.Brushes.Transparent,
                Stroke = System.Windows.Media.Brushes.Violet,
                Title = "MACD"
            };

            ChartValues<double> points2 = new ChartValues<double>();
            var emak = points[n];
            for (int i = 0; i < n; i++)
                points2.Add(double.NaN);
            for (int i = n; i < Data.Count; i++)
            {
                emak = points[i] * 2 / (k + 1) + emak * (1 - 2 / (k + 1));
                points2.Add(emak);
            }
            LineSeries lineSeries2 = new LineSeries
            {
                Values = points2,
                StrokeThickness = 2,
                StrokeDashArray = new DoubleCollection { 1 },
                LineSmoothness = 1,
                PointGeometry = null,
                Fill = System.Windows.Media.Brushes.Transparent,
                Stroke = System.Windows.Media.Brushes.Violet,
                Title = "MACD"
            };

            ChartValues<double> points3 = new ChartValues<double>();
            for (int i = 0; i < n; i++)
                points3.Add(0);
            for (int i = n; i < Data.Count; i++)
                points3.Add(points[i] - points2[i]);
            ColumnSeries columnSeries = new ColumnSeries
            {
                Values = points3,
                Title = "MACD"
            };



            return new SeriesCollection { columnSeries, lineSeries, lineSeries2 };
        }
        #endregion

        #region SMA
        private Series SMA(int period)
        {
            ChartValues<double> points = new ChartValues<double>();
            for (int i = 0; i < period; i++)
                points.Add(double.NaN);
            for (int i = period; i < Data.Count; i++)
                points.Add(getSMAi(i, period));

            LineSeries lineSeries = new LineSeries
            {
                Values = points,
                StrokeThickness = 2,
                LineSmoothness = 1,
                PointGeometry = null,
                Fill = System.Windows.Media.Brushes.Transparent,
                Stroke = System.Windows.Media.Brushes.Blue,
                Title = "SMA"
            };
            return lineSeries;
        }
        private double getSMAi(int index, int period)
        {
            double sum = 0;
            for (int i = index; i > index - period; i--)
                sum += Data[i].Close;
            return sum / period;
        }
        #endregion

        #region EMA
        private Series EMA(int period)
        {
            ChartValues<double> points = new ChartValues<double>();
            var EMAl = getSMAi(period - 1, period);
            for (int i = 0; i < period; i++)
                points.Add(double.NaN);
            for (int i = period; i < Data.Count; i++)
            {
                EMAl = getEMAi(EMAl, period, i);
                points.Add(EMAl);
            }

            LineSeries lineSeries = new LineSeries
            {
                Values = points,
                StrokeThickness = 2,
                LineSmoothness = 1,
                PointGeometry = null,
                Fill = System.Windows.Media.Brushes.Transparent,
                Stroke = System.Windows.Media.Brushes.White,
                Title = "EMA"
            };

            return lineSeries;
        }
        private double getEMAi(double EMAl, double period, int index)
        {
            return Data[index].Close * 2 / (period + 1) + EMAl * (1 - 2 / (period + 1));
        }
        #endregion

        #region A/D
        private SeriesCollection AD()
        {
            ChartValues<double> points = new ChartValues<double>();
            double ad = 0;
            for (int i = 0; i < Data.Count; i++)
            {
                ad = getAD(ad, i);
                points.Add(ad);
            }
            LineSeries lineSeries = new LineSeries
            {
                Values = points,
                StrokeThickness = 2,
                LineSmoothness = 0,
                PointGeometry = null,
                Fill = System.Windows.Media.Brushes.Transparent,
                Stroke = System.Windows.Media.Brushes.Red,
                Title = "A/D"
            };
            return new SeriesCollection { lineSeries };
        }
        private double getAD(double ad, int i)
        {
            return (((Data[i].Close - Data[i].Low) - (Data[i].High - Data[i].Close)) / (Data[i].High - Data[i].Low)) * Data[i].Vol + ad;
        }
        #endregion
    }
}
