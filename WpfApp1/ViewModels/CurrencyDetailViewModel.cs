using DevExpress.Mvvm;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using WpfApp1.Models;
using WpfApp1.Services;

namespace WpfApp1.ViewModels
{
    public class CurrencyDetailViewModel : INotifyPropertyChanged
    {
        private readonly ApiService _apiService;
        private readonly PageService _pageService;
        private CurrencyDetails _currencyDetail;
        private PlotModel _plotModel;

        public CurrencyDetailViewModel(ApiService apiService, PageService pageService)
        {
            _apiService = apiService;
            _pageService = pageService;
            PlotModel = new PlotModel { Title = "Currency Chart" };
            BackToMainMenuCommand = new DelegateCommand(OnBackToMainMenu);  
        }

        public CurrencyDetails CurrencyDetail
        {
            get { return _currencyDetail; }
            set
            {
                _currencyDetail = value;
                OnPropertyChanged();
            }
        }

        public PlotModel PlotModel
        {
            get { return _plotModel; }
            private set
            {
                _plotModel = value;
                OnPropertyChanged();
            }
        }

        public async Task LoadCurrencyDetailAsync(string currencyId)
        {
            try
            {
                CurrencyDetail = await _apiService.GetCurrencyDetailAsync(currencyId);

                var chartData = await _apiService.GetChartDataAsync(currencyId);
                UpdatePlot(chartData);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading currency details: {ex.Message}");
            }
        }

        private void UpdatePlot(IEnumerable<ChartData> chartData)
        {
            var newPlotModel = new PlotModel { Title = $"{CurrencyDetail.Name} Chart" };

            var xAxis = new DateTimeAxis
            {
                Position = AxisPosition.Bottom,
                StringFormat = "MM/dd",
                Title = "Date",
                IntervalType = DateTimeIntervalType.Days,
                MinorIntervalType = DateTimeIntervalType.Hours,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot
            };

            var yAxis = new LinearAxis
            {
                Position = AxisPosition.Left,
                Title = "Price (USD)",
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot
            };

            var series = new LineSeries
            {
                Title = "Price",
                Color = OxyColors.Black
            };

            foreach (var data in chartData)
            {
                series.Points.Add(new DataPoint(DateTimeAxis.ToDouble(data.Date), data.PricedUsd));
            }

            newPlotModel.Axes.Add(xAxis);
            newPlotModel.Axes.Add(yAxis);
            newPlotModel.Series.Add(series);

            PlotModel = newPlotModel;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public ICommand BackToMainMenuCommand { get; }

        private void OnBackToMainMenu()
        {
            _pageService.ShowMainWindow();
        }
    }
}
