using Railway_Management.Services;
using System.Text.Json.Serialization;

namespace Railway_Management.Models
{
    public class TrainDetailsApiResponse
    {
        public bool Status { get; set; }
        public string? Message { get; set; }
        public long Timestamp { get; set; }
        public List<TrainDto> Data { get; set; } = new();
    }
    public class TrainDto
    {
        [JsonPropertyName("train_number")]
        public string? TrainNumber { get; set; }

        [JsonPropertyName("train_name")]
        public string? TrainName { get; set; }

        [JsonPropertyName("eng_train_name")]
        public string? EngTrainName { get; set; }

        [JsonPropertyName("new_train_number")]
        public string? NewTrainNumber { get; set; }

        [JsonPropertyName("is_fav")]
        public bool IsFav { get; set; }

        [JsonPropertyName("src_stn_code")]
        public string? SourceStationCode { get; set; }

        [JsonPropertyName("src_stn_name")]
        public string? SourceStationName { get; set; }

        [JsonPropertyName("dstn_stn_code")]
        public string? DestinationStationCode { get; set; }

        [JsonPropertyName("dstn_stn_name")]
        public string? DestinationStationName { get; set; }
    }
}
