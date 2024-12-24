using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using static Railway_Management.Models.AllDataDetails;

namespace Railway_Management.Models
{
    public class CreateDataForTrainDetailsAndFare
    {
        private readonly IDbContextFactory<ConnectionContext> _connectionContextFactory;
        public CreateDataForTrainDetailsAndFare(IDbContextFactory<ConnectionContext> dbContext)
        {
            _connectionContextFactory = dbContext;
            
        }
        public  async Task<double> GetDistanceBetweenStations(string sourcecode,string destinationcode)
        {
            AllStations sourceStation=new AllStations();
            AllStations destinationStation = new AllStations();
            using (var dx= _connectionContextFactory.CreateDbContext())
            {
                sourceStation= await dx.AllStations1.Where(x=>x.station_code==sourcecode).FirstOrDefaultAsync();
                destinationStation= await dx.AllStations1.Where(y=>y.station_code==destinationcode).FirstOrDefaultAsync();
            }

         double distance=   DistanceCalculator.CalculateDistance((double)sourceStation.latitude, (double)sourceStation.longitude, (double)destinationStation.latitude, (double)destinationStation.longitude);

            return distance;

        }
        public  TrainDetailsViewModel CreateDetailsForUserSearched(List<TrainSchedule> sourceStations,double distance,List<TrainSchedule> destinationStations)
        {
            List<SourceStationDetails> sourceStationDetails = new List<SourceStationDetails>();
            List<BStationDetails> bStationDetails = new List<BStationDetails>();
            List<TrainsDetailsRunning> tra=new List<TrainsDetailsRunning>();    
             foreach (var sourceStation in sourceStations)
             {
                sourceStationDetails.Add(new SourceStationDetails{StationName=sourceStation.Station_Name,StationCode=sourceStation.Station_Code,DepartureTime=DateTime.MinValue.Add(sourceStation.Departure)});

             }
            foreach (var bStation in bStationDetails)
            {
                bStationDetails.Add(new BStationDetails { BStationName = bStation.BStationName, BStationCode = bStation.BStationCode, ArrivalTime = bStation.ArrivalTime });

            }

            return null;

        }


    }
}
