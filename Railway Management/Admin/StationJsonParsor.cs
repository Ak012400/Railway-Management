using Microsoft.Data.SqlClient;
using Newtonsoft.Json.Linq;
using Railway_Management.Models;
using static Railway_Management.Models.AllDataDetails;

namespace Railway_Management.Admin
{
    public class StationJsonParsor
    {

        public  List<AllStations> ImportStations(string jsonFilePath)
        {
            // Read the JSON file
            string jsonContent = File.ReadAllText(jsonFilePath);
            JObject jsonData = JObject.Parse(jsonContent);

            // Extract the features array
            var features = jsonData["features"];

            // Open database connection
            int totalFeatures =features.Count();
            List<AllStations> allStations = new List<AllStations>();
         
         
            for (int i = 0; i < totalFeatures; i++)
            {

                var feature = features[i]; // Access feature by index

                var geometry = feature["geometry"];
                double? latitude = null, longitude = null;

                // Validate geometry and coordinates
                if (geometry != null && geometry.HasValues)
                {
                    latitude = geometry?["coordinates"]?[1]?.ToObject<double>();
                    longitude = geometry?["coordinates"]?[0]?.ToObject<double>();
                }

                // Create a new AllStations object for each iteration
                AllStations allStations1 = new AllStations();

                // Set latitude and longitude (0 if not available)
                allStations1.latitude = (decimal?)(latitude ?? 0);
                allStations1.longitude = (decimal?)(longitude ?? 0);

                var properties = feature["properties"];

                allStations1.station_code = properties["code"]?.ToString();
                allStations1.station_name = properties["name"]?.ToString();
                allStations1.state = properties["state"]?.ToString() ?? ""; // Default empty string if null
                allStations1.zone = properties["zone"]?.ToString() ?? "";  // Default empty string if null
                allStations1.address = properties["address"]?.ToString() ?? ""; // Default empty string if null

                // Add the new AllStations object to the list
                allStations.Add(allStations1);

            }


            int val=allStations.Count;

            return allStations;

        }
    }
}