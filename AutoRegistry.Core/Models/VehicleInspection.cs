using System;

namespace AutoRegistry.Core.Models
{
    public class VehicleInspection
    {
        public Guid VehicleId { get; set; }
        public DateTime InspectionDate { get; set; }
        public bool Passed { get; set; }
        public string? FailureReason { get; set; }

        public DateTime? ValidUntil
        {
            get
            {
                return Passed ? InspectionDate.AddYears(1) : null;
            }
        }
    }
}