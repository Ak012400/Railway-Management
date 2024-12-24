using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Railway_Management.Services
{
    public class FareService
    {
        private readonly HttpClient _client;
        private readonly string _apiKey = "62df7e9527msh91d01349709588fp1512eejsnd125ba6347ca";
        private readonly string _host = "irctc1.p.rapidapi.com";

        // Constructor to initialize HttpClient
        public FareService(HttpClient client)
        {
            _client = client;
        }

        // Method to get fare details
        public async Task<FareDetails> GetFareAsync(string trainNo, string fromStationCode, string toStationCode)
        {
            // Construct the dynamic URI with train number and station codes
            var uri = $"https://{_host}/api/v2/getFare?trainNo={trainNo}&fromStationCode={fromStationCode}&toStationCode={toStationCode}";

            // Create the request message
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

            // Send the request and get the response
            using (var response = await _client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                // Deserialize the response to a C# object
                var fareDetails = JsonConvert.DeserializeObject<FareDetails>(body);
                return fareDetails; // Return the deserialized result
            }
        }
    }

    // Class to represent the fare details response
    public class FareDetails
    {
        public bool status { get; set; }
        public string message { get; set; }
        public FareData data { get; set; }
    }

    public class FareData
    {
        public string trainNo { get; set; }
        public string fromStationCode { get; set; }
        public string toStationCode { get; set; }
        public FareClass[] fareClasses { get; set; }
    }

    public class FareClass
    {
        public string classCode { get; set; }
        public string className { get; set; }
        public double fare { get; set; }
        public int availability { get; set; }
    }
}
