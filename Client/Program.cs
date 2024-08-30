// See https://aka.ms/new-console-template for more information
using Client;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;

Console.WriteLine("Hello, World!");
IServiceCollection services = new ServiceCollection();
services.AddHttpClient<ApiService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7291");
})
.AddPolicyHandler(HttpPolicyExtensions
    .HandleTransientHttpError()
    .OrResult(msg => !msg.IsSuccessStatusCode)
    .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
        onRetry: (outcome, timespan, retryAttempt, context) =>
        {
            Console.WriteLine($"Delaying for {timespan.TotalSeconds} seconds, then making retry {retryAttempt}");
        }));

var serviceProvider = services.BuildServiceProvider();

var apiService = serviceProvider.GetRequiredService<ApiService>();
HttpResponseMessage response = await apiService.GetTestDataAsync();
Console.ReadKey();