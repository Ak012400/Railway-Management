using Newtonsoft.Json;
using static Railway_Management.Models.AllDataDetails;

namespace Railway_Management.Admin
{
    public class JsonParsorTrainSchedule
    {

        public List<TrainSchedule> jsonParseScheduleTain(string path)
        {
            List<TrainSchedule> data = new List<TrainSchedule>();
            // Deserializing the JSON string to a List of TrainSchedule objects
            try
            {
                var trainSchedules = JsonConvert.DeserializeObject<TrainScheduleJson>(path);
                //foreach (var schedule in trainSchedules)
                //{
                //    Console.WriteLine($"Train: {schedule.Train_Name}, Station: {schedule.Station_Name}, Departure: {schedule.Departure}");
                //    Console.WriteLine("StationCode:" + schedule.Station_Code);
                //}

                return data;
            }
            catch(Exception Ex)
            {
                Console.WriteLine(Ex.InnerException);
                Console.WriteLine(Ex.Message);
                return null;
            }
           

            // Looping through the deserialized list to display the values
            
        }



    }
    




}

