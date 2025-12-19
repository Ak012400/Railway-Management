using Railway_Management.IServices;
using Railway_Management.Models;
using System.Text.Json;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Railway_Management.APIServices
{
    public class APIService : IRailwayApis
    {
        public readonly HttpClient _httpClient;
        public readonly IConfiguration _configuration;
        public readonly string _baseUrl;
        public readonly string _apiKey;
        public APIService(IConfiguration configuration,HttpClient httpClient)
        {
            _httpClient= httpClient;
            _configuration = configuration;
            _baseUrl = _configuration.GetValue<string>("AppSettings:RailwayAPIUrl") ?? string.Empty;
            _apiKey =  _configuration.GetValue<string>("AppSettings:Apikey") ?? string.Empty;

        }
        async Task<StatationDetails?> IRailwayApis.GetStationDetailsAsync(string stationCode)
        {
            var request = new HttpRequestMessage(
           HttpMethod.Get,
           _baseUrl + $"/searchStation?query={stationCode}"
       );

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<StatationDetails>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        async Task<TrainDetailsApiResponse?> IRailwayApis.GetTrainsAsync(string query)
        {
            string url= _baseUrl+$"/searchTrain?query={query}";
            var response = await _httpClient.GetAsync(_baseUrl);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<TrainDetailsApiResponse>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

       async Task<TrainsBetweenStation?> IRailwayApis.GetTrainScheduleAsync(string fromStation, string toStation, string date)
        {
            var endpoint =
             $"https://irctc1.p.rapidapi.com/api/v3/searchTrainBetweenStations" +
             $"?fromStationCode={fromStation}" +
             $"&toStationCode={toStation}" +
             $"&dateOfJourney={date}";

            var response = await _httpClient.GetAsync(endpoint);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<TrainsBetweenStation>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
    }
}
