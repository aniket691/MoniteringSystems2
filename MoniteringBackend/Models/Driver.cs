using System.ComponentModel.DataAnnotations;

namespace MoniteringBackend.Models
{
    public class Driver
    {
        [Key]
        public int DriverId { get; set; }
        public string FullName { get; set; }
        public string LicenseNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime DateHired { get; set; }
    }
}
