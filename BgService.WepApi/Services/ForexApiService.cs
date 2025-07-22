using BgService.WepApi.Models;

namespace BgService.WepApi.Services
{
    public class ForexApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ForexApiService> _logger;
        private readonly string _baseUrl = "https://api.frankfurter.dev/v1/";  //https://api.frankfurter.dev/v1/latest?base=USD

        public ForexApiService(HttpClient httpClient, ILogger<ForexApiService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<decimal?> GetForrex(string baseCurrency, string targetCurrency)
        {
            try
            {
                string requestUrl = $"{_baseUrl}latest?base={baseCurrency}";
                _logger.LogInformation($"Sending GET request to: {requestUrl}");
                var response = await _httpClient.GetAsync(requestUrl);
                response.EnsureSuccessStatusCode();

                var jsonstring = await response.Content.ReadAsStringAsync();
                var apiResponse = System.Text.Json.JsonSerializer.Deserialize<FrankfurterApiResponse>(jsonstring);

                if (apiResponse != null && apiResponse.Rates != null && apiResponse.Rates.TryGetValue(targetCurrency, out decimal rate))
                {
                    _logger.LogInformation($"Exchange rate for {baseCurrency} to {targetCurrency} is {rate}");
                    return rate;
                }
                else
                {
                    _logger.LogWarning($"No exchange rate found for {baseCurrency} to {targetCurrency}");
                    return null;
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error fetching exchange rate from Frankfurter API");
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while fetching exchange rate");
                return null;
            }
        }
    }
}
