using DevExpress.Mvvm;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Input;
using WpfApp1.Models;
using WpfApp1.Services;
using WpfApp1.Views;

namespace WpfApp1.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly PageService _pageService;
        private readonly ApiService _apiService;
        private ObservableCollection<Currency> _currencies;
        private ObservableCollection<Currency> _filteredCurrencies;
        private string _errorMessage;
        private Page _pageSource;
        private int _itemCount = 10;
        private string _searchTerm;

        public Page PageSource
        {
            get { return _pageSource; }
            set { _pageSource = value; OnPropertyChanged(); }
        }

        public PageService PageService => _pageService;

        public MainViewModel(PageService pageService, ApiService apiService)
        {
            _pageService = pageService;
            _apiService = apiService;
            ShowDetailsCommand = new AsyncCommand<Currency>(ShowDetails, CanShowDetails);
            NavigateToConverterCommand = new DelegateCommand(NavigateToConvertPageCommand);
            SearchCommand = new DelegateCommand(ExecuteSearch);
            SwitchToLightThemeCommand = new DelegateCommand(SwitchToLightTheme);
            SwitchToDarkThemeCommand = new DelegateCommand(SwitchToDarkTheme);
            SearchTerm = string.Empty;  
        }

        public ObservableCollection<Currency> Currencies
        {
            get { return _currencies; }
            set
            {
                _currencies = value;
                OnPropertyChanged();
                FilterCurrencies();
            }
        }

        public ObservableCollection<Currency> FilteredCurrencies
        {
            get { return _filteredCurrencies; }
            private set
            {
                _filteredCurrencies = value;
                OnPropertyChanged();
            }
        }

        public int ItemCount
        {
            get { return _itemCount; }
            set
            {
                _itemCount = value;
                OnPropertyChanged();
                LoadCurrencies();
            }
        }

        public string ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                _errorMessage = value;
                OnPropertyChanged();
            }
        }

        public string SearchTerm
        {
            get { return _searchTerm; }
            set
            {
                _searchTerm = value;
                OnPropertyChanged();
                FilterCurrencies();
            }
        }

        public ICommand ShowDetailsCommand { get; }
        public ICommand NavigateToConverterCommand { get; }
        public ICommand SearchCommand { get; }
        public ICommand SwitchToLightThemeCommand { get; }
        public ICommand SwitchToDarkThemeCommand { get; }

        private bool CanShowDetails(Currency currency)
        {
            return currency != null;
        }

        public async Task ShowDetails(Currency currency)
        {
            if (currency == null) return;

            var detailViewModel = ViewModelLocator.CreateCurrencyDetailViewModel();

            try
            {
                await detailViewModel.LoadCurrencyDetailAsync(currency.Id);
                Console.WriteLine("Currency detail loaded successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading currency details: {ex.Message}");
                return;
            }

            var detailPage = new CurrencyDetailPage(detailViewModel);
            _pageService.ChangePage(detailPage);
        }

        private void NavigateToConvertPageCommand()
        {
            _pageService.ChangePage(new CurrencyConvertPage());
        }

        private void FilterCurrencies()
        {
            if (_currencies == null)
            {
                FilteredCurrencies = new ObservableCollection<Currency>();
                return;
            }

            if (string.IsNullOrEmpty(_searchTerm))
            {
                FilteredCurrencies = new ObservableCollection<Currency>(_currencies);
            }
            else
            {
                FilteredCurrencies = new ObservableCollection<Currency>(
                    _currencies.Where(c => c.Name.Contains(_searchTerm, StringComparison.OrdinalIgnoreCase) ||
                                          c.Id.Contains(_searchTerm, StringComparison.OrdinalIgnoreCase)));
            }
        }

        private void ExecuteSearch()
        {
            FilterCurrencies();
        }

        public async Task LoadCurrencies()
        {
            try
            {
                var currencies = await _apiService.GetTopCurrenciesAsync(_itemCount);
                Currencies = new ObservableCollection<Currency>(currencies);
            }
            catch (HttpRequestException ex)
            {
                ErrorMessage = $"Failed to load currencies: {ex.Message}";
            }
            finally
            {
                LoadCurrenciesCompleted?.Invoke(this, EventArgs.Empty);
            }
        }

        public event EventHandler LoadCurrenciesCompleted;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private List<int> _itemCountOptions = new List<int> { 5, 10, 20, 50, 100 };

        public List<int> ItemCountOptions
        {
            get { return _itemCountOptions; }
        }

        private void SwitchToLightTheme()
        {
            App.ChangeTheme(new Uri("pack://application:,,,/Themes/LightTheme.xaml"));
        }

        private void SwitchToDarkTheme()
        {
            App.ChangeTheme(new Uri("pack://application:,,,/Themes/DarkTheme.xaml"));
        }
    }
}
