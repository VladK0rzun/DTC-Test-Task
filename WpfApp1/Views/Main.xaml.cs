using System.Windows;
using System.Windows.Controls;

namespace WpfApp1.Views
{
    public partial class Main : Window
    {
        public Main()
        {
            InitializeComponent();
            var viewModel = ViewModelLocator.MainViewModel;
            viewModel.LoadCurrenciesCompleted += OnLoadCurrenciesCompleted;
            viewModel.PageService.OnPageChanged += OnPageChanged;
            DataContext = viewModel;
            viewModel.LoadCurrencies();
        }

        private void OnLoadCurrenciesCompleted(object sender, EventArgs e) { }

        private void OnPageChanged(Page newPage)
        {
            this.Content = newPage;
        }
    }
}
