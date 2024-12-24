using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Railway_Management.TrainServices
{
    public class TrainStatusService
    {
        private readonly HttpClient _client;
        private readonly string _apiKey = "62df7e9527msh91d01349709588fp1512eejsnd125ba6347ca";
        private readonly string _host = "indian-railway-irctc.p.rapidapi.com";

        // Constructor to initialize HttpClient
        public TrainStatusService(HttpClient client)
        {
            _client = client;
        }

        // Method to get train status by dynamic train number and date
        public async Task<TrainStatus> GetTrainStatusAsync(string trainNumber, string departureDate)
        {
            // Construct the dynamic URI with train number and departure date
            var uri = $"https://{_host}/api/trains/v1/train/status?departure_date={departureDate}&isH5=true&client=web&train_number={trainNumber}";

            // Create the request message
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(uri),
                Headers =
                {
                    { "x-rapidapi-key", _apiKey },
                    { "x-rapidapi-host", _host },
                    { "x-rapid-api", "rapid-api-database" }
                }
            };

            // Send the request and get the response
            using (var response = await _client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                // Deserialize the response to a C# object
                var trainStatus = JsonConvert.DeserializeObject<TrainStatus>(body);
                return trainStatus; // Return the deserialized result
            }
        }
    }

    // Class to represent the train status response
    public class TrainStatus
    {
        public Status status { get; set; }
        public Body body { get; set; }
    }

    public class Status
    {
        public string result { get; set; }
        public Message message { get; set; }
    }

    public class Message
    {
        public string title { get; set; }
        public string message { get; set; }
    }

    public class Body
    {
        public string time_of_availability { get; set; }
        public string current_station { get; set; }
        public bool terminated { get; set; }
        public string server_timestamp { get; set; }
        public string train_status_message { get; set; }
        public Station[] stations { get; set; }
    }

    public class Station
    {
        public string stationCode { get; set; }
        public string stationName { get; set; }
        public string arrivalTime { get; set; }
        public string departureTime { get; set; }
        public string routeNumber { get; set; }
        public string haltTime { get; set; }
        public string distance { get; set; }
        public string dayCount { get; set; }
        public string stnSerialNumber { get; set; }
        public bool boardingDisabled { get; set; }
        public string actual_arrival_date { get; set; }
        public string actual_departure_date { get; set; }
        public string actual_arrival_time { get; set; }
        public string actual_departure_time { get; set; }
    }
}
