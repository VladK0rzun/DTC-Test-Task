using Microsoft.Extensions.DependencyInjection;
using WpfApp1.Services;
using WpfApp1.ViewModels;

namespace WpfApp1
{
    class ViewModelLocator
    {
        private static ServiceProvider _provider;

        public static void Init()
        {
            var services = new ServiceCollection();

            services.AddSingleton<PageService>();
            services.AddTransient<ApiService>();

            services.AddTransient<MainViewModel>();
            services.AddTransient<CurrencyDetailViewModel>();
            services.AddTransient<CurrencyConvertViewModel>();

            _provider = services.BuildServiceProvider();
        }
        public static MainViewModel MainViewModel => _provider.GetRequiredService<MainViewModel>();
        public static CurrencyDetailViewModel CreateCurrencyDetailViewModel() =>
            _provider.GetRequiredService<CurrencyDetailViewModel>();
        public static CurrencyConvertViewModel CurrencyConvertViewModel => _provider.GetRequiredService<CurrencyConvertViewModel>();
    }
}
