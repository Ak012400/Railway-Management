using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Railway_Management.Services
{
    public class TrainBetweenStationsService
    {
        private readonly HttpClient _client;
        private readonly string _apiKey = "62df7e9527msh91d01349709588fp1512eejsnd125ba6347ca";
        private readonly string _host = "irctc1.p.rapidapi.com";

        // Constructor to initialize HttpClient
        public TrainBetweenStationsService(HttpClient client)
        {
            _client = client;
        }

        // Method to fetch trains between stations
        public async Task<TrainBetweenStationsResponse> GetTrainsAsync(string fromStationCode, string toStationCode)
        {
            // Construct the dynamic URI
            var uri = $"https://{_host}/api/v3/trainBetweenStations?fromStationCode={fromStationCode}&toStationCode={toStationCode}";

            // Create the request
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(uri),
                Headers =
                {
                    { "x-rapidapi-key", _apiKey },
                    { "x-rapidapi-host", _host }
                }
            };

            // Send the request and process the response
            using (var response = await _client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                // Deserialize the JSON response into the response model
                var trainResponse = JsonConvert.DeserializeObject<TrainBetweenStationsResponse>(body);
                return trainResponse; // Return the parsed data
            }
        }
    }

    // Response model for trainBetweenStations API
    public class TrainBetweenStationsResponse
    {
        public bool status { get; set; }
        public string message { get; set; }
        public TrainData[] data { get; set; }
    }

    public class TrainData
    {
        public string trainNo { get; set; }
        public string trainName { get; set; }
        public string fromStationCode { get; set; }
        public string toStationCode { get; set; }
        public string departureTime { get; set; }
        public string arrivalTime { get; set; }
        public string travelTime { get; set; }
        public string daysOfRun { get; set; }
    }
}
