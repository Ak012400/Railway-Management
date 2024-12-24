using static Railway_Management.Models.AllDataDetails;

public class GeoJsonRoot
{
    public string Type { get; set; }
    public List<GeoJsonFeature> Features { get; set; }
}

public class GeoJsonFeature
{
    public string Type { get; set; }
    public Geometry Geometry { get; set; }
    public Properties Properties { get; set; }
}

public class Geometry
{
    public string Type { get; set; }
    public List<List<double>> Coordinates { get; set; }
}

public class Properties
{
    public string number { get; set; } // 
    public string name { get; set; }
    public string third_ac { get; set; }
    public string arrival { get; set; }
    public string from_station_code { get; set; }
    public string Zone { get; set; }
    public string chair_car { get; set; }
    public string first_class { get; set; }
    public string Sleeper { get; set; }
    public string duration_m { get; set; }
    public string departure { get; set; }
    public string from_station_name { get; set; }
    public string type {  get; set; }
    public string distance { get; set; }
    public string first_ac { get; set; }
    public string duration_h {  get; set; }
    public string to_station_name { get; set; } 
    public string classes { get; set; }
    public string second_ac { get; set; }
   public string to_station_code { get; set; }
    public string return_train {  get; set; }





}
public class ExtractionJsonResult
{
    public List<TrainDetails> trainDetails { get; set; }
    public List<TrainsRunningCoordinates> coordinates { get; set; }
}
public class TrainScheduleJson
{
    public string Id { get; set; } // Primary Key
    public string Arrival { get; set; }
    public string Day { get; set; }
    public string Train_Name { get; set; }
    public string Station_Name { get; set; }
    public string Station_Code { get; set; }
    public string Train_Number { get; set; }
    public string Departure { get; set; }
}


