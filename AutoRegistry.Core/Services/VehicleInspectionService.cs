using System;
using System.Threading.Tasks;
using AutoRegistry.Core.Interfaces;
using AutoRegistry.Core.Models;

namespace AutoRegistry.Core.Services
{
    public class VehicleInspectionService
    {
        private readonly IVehicleRepository _repository;

        public VehicleInspectionService(IVehicleRepository repository)
        {
            _repository = repository;
        }

        public async Task<string> GetInspectionStatusAsync(Guid vehicleId)
        {
            if (vehicleId == Guid.Empty)
                throw new ArgumentException("Vehicle ID cannot be empty.", nameof(vehicleId));

            try
            {
                var inspection = await _repository.GetLatestInspectionAsync(vehicleId);
                if (inspection == null)
                    return "Vehicle not found";

                if (!inspection.Passed)
                    return $"Inspection failed: {inspection.FailureReason}";

                if (inspection.ValidUntil < DateTime.UtcNow)
                    return $"Inspection expired on {inspection.ValidUntil?.ToShortDateString()}";

                return $"Inspection passed. Valid until {inspection.ValidUntil?.ToShortDateString()}";
            }
            catch (Exception)
            {
                return "Error retrieving inspection.";
            }
        }
    }
}