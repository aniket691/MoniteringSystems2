using System.ComponentModel.DataAnnotations;

namespace MoniteringBackend.Models
{
    public class ServiceRecord
    {
        [Key]
        public int ServiceRecordId { get; set; }
        public int VehicleId { get; set; }
        public string RepairType { get; set; }
        public double LabourCost { get; set; }
        public int HoursWorked { get; set; }
        public DateTime ServiceDate { get; set; }
        public string Description { get; set; }
    }

}
