using System.ComponentModel.DataAnnotations;

namespace MoniteringBackend.Models
{
    public class Vehicle
    {
        [Key]
        public int VehicleId { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string LicensePlate { get; set; }
        public DateTime LastServiceDate { get; set; }
    }

}
