namespace Railway_Management.Models
{
    public class TrainDetailsViewModel
    {
     public  List<TrainsDetailsRunning> Trains {  get; set; }
        public List<SourceStationDetails> Stations { get; set; }
        public List<BStationDetails> BStations { get; set; }
    }

    public class SourceStationDetails
    {
        public string StationName { get; set; }
        public string StationCode { get; set; }
        public DateTime DepartureTime { get; set; }
    }
    public class BStationDetails
    {
        public string BStationName { get; set; }
        public string BStationCode { get; set; }
        public DateTime ArrivalTime { get; set; }
    }
    public class TrainsDetailsRunning
    {
        public string TrainName { get; set; }
        public string TrainNumber { get; set; }
        public string distance { get; set; }
    }
    public class ModelUserTrainSearchDetails
    {
        public string TrainName { get; set; }
        public string TrainNumber { get; set; }
        public double distance { get; set; }
        public string BStationName { get; set; }
        public string BStationCode { get; set; }
        public string ArrivalTime { get; set; }
        public string StationName { get; set; }
        public string StationCode { get; set; }
        public TimeSpan DepartureTime { get; set; }
        public int Fare {  get; set; }  
    }

}
