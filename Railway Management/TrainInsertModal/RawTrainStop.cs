using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Railway_Management.TrainInsertModal
{
    public class RawTrainStop
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }   // SERIAL PRIMARY KEY

        [Column("trainno")]
        public string? TrainNo { get; set; }

        [Column("trainname")]
        public string? TrainName { get; set; }

        [Column("seq")]
        public string? Seq { get; set; }

        [Column("stationcode")]
        public string? StationCode { get; set; }

        [Column("stationname")]
        public string? StationName { get; set; }

        [Column("arrivaltime")]
        public string? ArrivalTime { get; set; }

        [Column("departuretime")]
        public string? DepartureTime { get; set; }

        [Column("distance")]
        public string? Distance { get; set; }

        [Column("sourcestation")]
        public string? SourceStation { get; set; }

        [Column("sourcestationname")]
        public string? SourceStationName { get; set; }

        [Column("destinationstation")]
        public string? DestinationStation { get; set; }

        [Column("destinationstationname")]
        public string? DestinationStationName { get; set; }
    }
}
