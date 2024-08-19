using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Net.Http;
using WpfApp1.Models;

namespace WpfApp1.Services
{
    public class ApiService
    {
        private readonly string apiKey = "cdc9c9b2-0baf-43ce-8a5b-f992846ad96d";
        private static readonly HttpClient _httpClient;

        static ApiService()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.3");
        }

        public async Task<List<Currency>> GetTopCurrenciesAsync(int itemCount)
        {
            var url = "https://api.coincap.io/v2/assets";

            try
            {
                Console.WriteLine("Відправка запиту до API...");
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();


                var content = await response.Content.ReadAsStringAsync();
                var jsonResponse = JObject.Parse(content);

                var allCurrencies = jsonResponse["data"].ToObject<List<Currency>>();

                var topCurrencies = allCurrencies.Take(itemCount).ToList();

                Console.WriteLine("Відповідь від API отримана успішно");

                return topCurrencies;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Request error: {ex.Message}");
                throw;
            }
        }

        public async Task<CurrencyDetails> GetCurrencyDetailAsync(string id)
        {
            var url = $"https://api.coincap.io/v2/assets/{id}";

            try
            {
                Console.WriteLine("Sending request to API for currency detail...");
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var jsonResponse = JObject.Parse(content);

                var currencyDetail = jsonResponse["data"].ToObject<CurrencyDetails>();

                Console.WriteLine("Currency detail received successfully");

                return currencyDetail;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Request error: {ex.Message}");
                throw;
            }
        }

        public async Task<decimal> GetExchangeRateAsync(string fromCurrency)
        {
            var url = $"https://api.coincap.io/v2/rates/{fromCurrency}";

            try
            {
                Console.WriteLine("Sending request to API for exchange rate...");
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var jsonResponse = JObject.Parse(content);

                var rate = jsonResponse["data"]["rateUsd"].Value<decimal>();

                Console.WriteLine("Exchange rate received successfully");

                return rate;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Request error: {ex.Message}");
                throw;
            }
        }

        public async Task<List<ChartData>> GetChartDataAsync(string currencyId)
        {
            var url = $"https://api.coincap.io/v2/assets/{currencyId}/history?interval=d1";

            try
            {
                var response = await _httpClient.GetStringAsync(url);
                var jsonResponse = JObject.Parse(response);

                var data = new List<ChartData>();

                foreach (var item in jsonResponse["data"])
                {
                    data.Add(new ChartData
                    {
                        Date = DateTime.Parse(item["date"].ToString()),
                        PricedUsd = double.Parse(item["priceUsd"].ToString(), CultureInfo.InvariantCulture)
                    });
                }

                return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching chart data: {ex.Message}");
                throw;
            }
        }
    }
}
