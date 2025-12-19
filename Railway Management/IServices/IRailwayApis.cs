using Railway_Management.Models;

namespace Railway_Management.IServices
{
    public interface IRailwayApis
    {
        public Task<StatationDetails?> GetStationDetailsAsync(string stationCode);
        Task<TrainDetailsApiResponse?> GetTrainsAsync(string query);
        Task<TrainsBetweenStation?> GetTrainScheduleAsync(
       string fromStation,
       string toStation,
       string date
   );
    }
}
