using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
namespace VictuzApp.Services;
public class BestActivityService
{
    protected HttpClient _httpClient;

    public BestActivityService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<BestActivity>> GetDiscountsAsync()
    {
        var response = await _httpClient.GetAsync("https://localhost:7153/api/bestactivity");
        if (response.IsSuccessStatusCode)
        {
            var bestActivities = await response.Content.ReadFromJsonAsync<List<BestActivity>>();
            return bestActivities;
        }
        return new List<BestActivity>();
    }
}

public class BestActivity
{
    public string Name { get; set; }
    public string Description { get; set; }
}
