using System;
using System.Threading.Tasks;
using AutoRegistry.Core.Models;

namespace AutoRegistry.Core.Interfaces
{
    public interface IVehicleRepository
    {
        Task<VehicleInspection?> GetLatestInspectionAsync(Guid vehicleId);
    }
}