using System.Text.Json.Serialization;

namespace Railway_Management.Models
{
    public class TrainsBetweenStation
    {
        public bool Status { get; set; }
        public string? Message { get; set; }
        public long Timestamp { get; set; }
        public List<TrainScheduleDto> Data { get; set; } = new();
    }
    public class TrainScheduleDto
    {
        //make all string nullable

        [JsonPropertyName("train_number")]
        public string? TrainNumber { get; set; }

        [JsonPropertyName("train_name")]
        public string? TrainName { get; set; }

        [JsonPropertyName("run_days")]
        public List<string>? RunDays { get; set; } = new();

        [JsonPropertyName("train_src")]
        public string? TrainSource { get; set; }

        [JsonPropertyName("train_dstn")]
        public string? TrainDestination { get; set; }

        [JsonPropertyName("from_std")]
        public string? FromStd { get; set; }

        [JsonPropertyName("from_sta")]
        public string? FromSta { get; set; }

        [JsonPropertyName("local_train_from_sta")]
        public int LocalTrainFromSta { get; set; }

        [JsonPropertyName("to_sta")]
        public string? ToSta { get; set; }

        [JsonPropertyName("to_std")]
        public string? ToStd { get; set; }

        [JsonPropertyName("from_day")]
        public int FromDay { get; set; }

        [JsonPropertyName("to_day")]
        public int ToDay { get; set; }

        [JsonPropertyName("d_day")]
        public int DDay { get; set; }

        [JsonPropertyName("from")]
        public string? FromStationCode { get; set; }

        [JsonPropertyName("to")]
        public string? ToStationCode { get; set; }

        [JsonPropertyName("from_station_name")]
        public string? FromStationName { get; set; }

        [JsonPropertyName("to_station_name")]
        public string? ToStationName { get; set; }

        [JsonPropertyName("duration")]
        public string? Duration { get; set; }

        [JsonPropertyName("special_train")]
        public bool IsSpecialTrain { get; set; }

        [JsonPropertyName("train_type")]
        public string? TrainType { get; set; }

        [JsonPropertyName("train_date")]
        public string? TrainDate { get; set; }

        [JsonPropertyName("class_type")]
        public List<string> ClassTypes { get; set; } = new();
    }
}
