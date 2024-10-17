    using System;
    using System.ComponentModel.DataAnnotations;

    namespace MoniteringBackend.Models
    {
        public class Invoice
        {
            [Key]//generate unique id this
            public int InvoiceId { get; set; }

           //make this primary key not null 
            public string VehicleId { get; set; } = "Not Found"; // Default to "Not Found"

            public string PartNumber { get; set; } = "Not Found"; // Default to "Not Found"

            public decimal LabourCost { get; set; }

            public decimal HoursWorked { get; set; }

            public string RepairType { get; set; } = "Not Found"; // Default to "Not Found"

            public DateTime ServiceDate { get; set; } = DateTime.MinValue; // Default to minimum DateTime

            public string Description { get; set; } = "Not Found"; // Default to "Not Found"
        }
    }
