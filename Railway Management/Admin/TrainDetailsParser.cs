using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Railway_Management.Models;
using Railway_Management.Validations;
using System;
using System.Collections.Generic;
using System.IO;
using static Railway_Management.Models.AllDataDetails;

public class GeoJsonReader
{
    private readonly IDbContextFactory<ConnectionContext> _connectionContext;

    public GeoJsonReader(IDbContextFactory<ConnectionContext> context)
    {
        this._connectionContext = context;
    }
    public List<TrainDetails> TrainDetails { get; set; }
    public List<TrainsRunningCoordinates> Coordinates { get; set; }

    public  ExtractionJsonResult ReadGeoJson(string filePath)
    {
        string jsonContent = File.ReadAllText(filePath);
        List<GeoJsonFeature> geoJsonFeatures = null;
        try
        {
            var geoJsonRoot = JsonConvert.DeserializeObject<GeoJsonRoot>(jsonContent);


            if (geoJsonRoot?.Features != null)
            {
                foreach (var feature in geoJsonRoot.Features)
                {
                    Console.WriteLine($"Feature Type: {feature.Type}, Name: {feature.Properties.name}");
                    Console.WriteLine("\n");

                }
            }
            else
            {
                Console.WriteLine("No features found in the JSON.");
            }

            TrainDetails = new List<TrainDetails>();
            Coordinates = new List<TrainsRunningCoordinates>();
            int result;
            int totalvalue = 0;
            using(var dx=_connectionContext.CreateDbContext())
            {
                totalvalue=dx.Train_Details.Count();
            }

            foreach (var feature in geoJsonRoot.Features)
            {
                totalvalue ++;
                
                string fromStation = feature.Properties.from_station_name;
                TimeSpan? departure = feature.Properties.departure == "None" || feature.Properties.departure == "" ? TimeSpan.Zero : TimeSpan.Parse(feature.Properties.departure);
                TimeSpan? arrival = feature.Properties.arrival == "None" || feature.Properties.arrival == "" ? TimeSpan.Zero : TimeSpan.Parse(feature.Properties.arrival);
                string trainName = feature.Properties.name;
                int? Sleepser =   feature.Properties.Sleeper==""||feature.Properties.Sleeper=="None"? 0 :int.Parse(feature.Properties.Sleeper);
                int? firstClass = int.TryParse(feature.Properties.first_class, out result) ? result : 0;
                int? duration_m = int.TryParse(feature.Properties.duration_m, out result) ? result : 0;
                string? zone = "";
                if (!feature.Properties.Zone.Contains("?"))
                {
                     zone = feature.Properties.Zone;
                }
               
                string fromStationCode = feature.Properties.from_station_code;

                // Train Details

                TrainDetails.Add(new TrainDetails
                {
                    trainID=totalvalue,
                    third_ac = int.TryParse(feature.Properties.third_ac, out result) ? result : 0,
                    arrival_time = arrival,
                    from_station_code = fromStationCode,

                    trainName = trainName,
                    zone = zone,
                    chair_car = int.TryParse(feature.Properties.chair_car, out result) ? result : 0,
                    first_class = firstClass,
                    duration_min = duration_m,
                    sleeper = Sleepser,
                    from_station_name = feature.Properties.from_station_name,
                    trainNO = feature.Properties.number,
                    departure_time = departure,
                    returntrain = feature.Properties.return_train,
                    tostationcode = feature.Properties.to_station_code,
                    seconndac = int.TryParse(feature.Properties.second_ac, out result) ? result : 0,
                    Classes = feature.Properties.classes,
                    tostationname = feature.Properties.to_station_name,
                    duration_hrs = int.TryParse(feature.Properties.duration_h, out result) ? result : 0,
                    trainType = feature.Properties.type,
                    firstac = int.TryParse(feature.Properties.first_ac, out result) ? result : 0,
                    distance = int.TryParse(feature.Properties.distance, out result) ? result : 0,


                });

                // Coordinates
                foreach (var coord in feature.Geometry.Coordinates)
                {
                    Coordinates.Add(new TrainsRunningCoordinates
                    {
                        trainID=totalvalue,
                        Latitude = (decimal)coord[1],
                        Longitude = (decimal)coord[0],

                    });
                    string value = coord[0].ToString();
                }
            }
            ExtractionJsonResult result1=new ExtractionJsonResult();
            result1.trainDetails = TrainDetails;
            result1.coordinates = Coordinates;
           

            Console.WriteLine("All data has written");
            return result1;

        }
        catch (Exception ex)
        {
            throw new Exception("There is some exception while reading the file for Train data"+ex.Message, ex);
            
        }

    }
}
