using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoRegistry.Core.Interfaces;
using AutoRegistry.Core.Models;

namespace AutoRegistry.API.Repositories
{
    public class InMemoryVehicleRepository : IVehicleRepository
    {
        private readonly List<VehicleInspection> _inspections = new()
        {
            new VehicleInspection
            {
                VehicleId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                InspectionDate = DateTime.UtcNow.AddMonths(-2),
                Passed = true
            },
            new VehicleInspection
            {
                VehicleId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                InspectionDate = DateTime.UtcNow.AddMonths(-14),
                Passed = true
            },
            new VehicleInspection
            {
                VehicleId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                InspectionDate = DateTime.UtcNow.AddMonths(-1),
                Passed = false,
                FailureReason = "emissions"
            }
        };

        public Task<VehicleInspection?> GetLatestInspectionAsync(Guid vehicleId)
        {
            var inspection = _inspections.FirstOrDefault(i => i.VehicleId == vehicleId);
            return Task.FromResult<VehicleInspection?>(inspection);
        }
    }
}