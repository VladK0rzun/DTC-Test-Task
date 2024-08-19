using DevExpress.Mvvm;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using WpfApp1.Services;

namespace WpfApp1.ViewModels
{
    public class CurrencyConvertViewModel : INotifyPropertyChanged
    {
        private readonly ApiService _apiService;
        private readonly PageService _pageService;
        private decimal _amount;
        private string _fromCurrency;
        private decimal _result;
        private string _errorMessage;
        private List<string> _currencies;

        public CurrencyConvertViewModel(ApiService apiService, PageService pageService)
        {
            _apiService = apiService;
            _pageService = pageService;
            ConvertCommand = new AsyncCommand(ConvertCurrency);
            BackToMainMenuCommand = new DelegateCommand(OnBackToMainMenu);
            LoadCurrencies();
        }

        public decimal Amount
        {
            get => _amount;
            set { _amount = value; OnPropertyChanged(); }
        }

        public string FromCurrency
        {
            get => _fromCurrency;
            set { _fromCurrency = value; OnPropertyChanged(); }
        }

        public decimal Result
        {
            get => _result;
            set { _result = value; OnPropertyChanged(); }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set { _errorMessage = value; OnPropertyChanged(); }
        }

        public List<string> Currencies
        {
            get => _currencies;
            private set { _currencies = value; OnPropertyChanged(); }
        }

        public ICommand ConvertCommand { get; }
        public ICommand BackToMainMenuCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private async void LoadCurrencies()
        {
            try
            {
                var currencies = await _apiService.GetTopCurrenciesAsync(50);
                Currencies = currencies.Select(c => c.Id).ToList();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Failed to load currencies: {ex.Message}";
            }
        }

        private async Task ConvertCurrency()
        {
            try
            {
                if (string.IsNullOrEmpty(FromCurrency))
                {
                    ErrorMessage = "Please select both source and target currencies.";
                    return;
                }

                var rate = await _apiService.GetExchangeRateAsync(FromCurrency);
                Result = Amount * rate;
                ErrorMessage = string.Empty;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Failed to convert currency: {ex.Message}";
            }
        }

        private void OnBackToMainMenu()
        {
            _pageService.ShowMainWindow();
        }
    }
}
