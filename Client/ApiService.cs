using Microsoft.Extensions.Logging;

namespace Client;

public class ApiService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ApiService> _logger;

    public ApiService(HttpClient httpClient, ILogger<ApiService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<HttpResponseMessage> GetTestDataAsync()
    {
        var response = await _httpClient.GetAsync("WeatherForecast");

        if (!response.IsSuccessStatusCode)
        {
            // Logs a warning with the status code and the reason phrase
            Console.WriteLine($"Request to WeatherForecast failed with status code {response.StatusCode} and reason: {response.ReasonPhrase}");
        }

        return response;
    }
}
