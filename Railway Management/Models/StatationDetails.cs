using System.Text.Json.Serialization;

namespace Railway_Management.Models
{
    public class StatationDetails
    {
        public bool Status { get; set; }
        public string? Message { get; set; }
        public long Timestamp { get; set; }
        public List<StationDto>? Data { get; set; }
    }
    public class StationDto
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("eng_name")]
        public string? EngName { get; set; }

        [JsonPropertyName("code")]
        public string? Code { get; set; }

        [JsonPropertyName("state_name")]
        public string? StateName { get; set; }
    }
}
