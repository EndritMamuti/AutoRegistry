using System;
using System.Threading.Tasks;
using AutoRegistry.Core.Interfaces;
using AutoRegistry.Core.Models;
using AutoRegistry.Core.Services;
using Moq;
using Xunit;

namespace AutoRegistry.Tests.Services
{
    public class VehicleInspectionServiceTests
    {
        private readonly Mock<IVehicleRepository> _repositoryMock;
        private readonly VehicleInspectionService _service;

        public VehicleInspectionServiceTests()
        {
            _repositoryMock = new Mock<IVehicleRepository>();
            _service = new VehicleInspectionService(_repositoryMock.Object);
        }

        [Fact]
        public async Task ValidVehicle_ReturnsPassedStatus()
        {
            var vehicleId = Guid.NewGuid();
            var inspectionDate = DateTime.UtcNow.AddDays(-10);
            var inspection = new VehicleInspection { VehicleId = vehicleId, Passed = true, InspectionDate = inspectionDate };
            _repositoryMock.Setup(r => r.GetLatestInspectionAsync(vehicleId)).ReturnsAsync(inspection);
            var result = await _service.GetInspectionStatusAsync(vehicleId);
            Assert.Contains("passed", result.ToLower());
            Assert.Contains("valid until", result.ToLower());
        }

        [Fact]
        public async Task FailedInspection_ReturnsFailureReason()
        {
            var vehicleId = Guid.NewGuid();
            var inspection = new VehicleInspection { VehicleId = vehicleId, Passed = false, InspectionDate = DateTime.UtcNow.AddDays(-5), FailureReason = "brakes" };
            _repositoryMock.Setup(r => r.GetLatestInspectionAsync(vehicleId)).ReturnsAsync(inspection);
            var result = await _service.GetInspectionStatusAsync(vehicleId);
            Assert.Contains("failed", result.ToLower());
            Assert.Contains("brakes", result.ToLower());
        }

        [Fact]
        public async Task ExpiredInspection_ReturnsExpiredMessage()
        {
            var vehicleId = Guid.NewGuid();
            var inspectionDate = DateTime.UtcNow.AddYears(-2);
            var inspection = new VehicleInspection { VehicleId = vehicleId, Passed = true, InspectionDate = inspectionDate };
            _repositoryMock.Setup(r => r.GetLatestInspectionAsync(vehicleId)).ReturnsAsync(inspection);
            var result = await _service.GetInspectionStatusAsync(vehicleId);
            Assert.Contains("expired", result.ToLower());
        }

        [Fact]
        public async Task VehicleNotFound_ReturnsNotFoundMessage()
        {
            var vehicleId = Guid.NewGuid();
            _repositoryMock.Setup(r => r.GetLatestInspectionAsync(vehicleId)).ReturnsAsync((VehicleInspection?)null);
            var result = await _service.GetInspectionStatusAsync(vehicleId);
            Assert.Equal("Vehicle not found", result);
        }

        [Fact]
        public async Task InspectionExactlyToday_ValidResult()
        {
            var vehicleId = Guid.NewGuid();
            var inspection = new VehicleInspection { VehicleId = vehicleId, Passed = true, InspectionDate = DateTime.UtcNow.Date };
            _repositoryMock.Setup(r => r.GetLatestInspectionAsync(vehicleId)).ReturnsAsync(inspection);
            var result = await _service.GetInspectionStatusAsync(vehicleId);
            Assert.Contains("valid", result.ToLower());
        }

        [Fact]
        public async Task InspectionWithinValidPeriod_IsStillValid()
        {
            var vehicleId = Guid.NewGuid();
            var inspection = new VehicleInspection { VehicleId = vehicleId, Passed = true, InspectionDate = DateTime.UtcNow.AddMonths(-11) };
            _repositoryMock.Setup(r => r.GetLatestInspectionAsync(vehicleId)).ReturnsAsync(inspection);
            var result = await _service.GetInspectionStatusAsync(vehicleId);
            Assert.Contains("valid", result.ToLower());
        }

        [Fact]
        public async Task EmptyVehicleId_ThrowsException()
        {
            var vehicleId = Guid.Empty;
            await Assert.ThrowsAsync<ArgumentException>(() => _service.GetInspectionStatusAsync(vehicleId));
        }

        [Fact]
        public async Task RepositoryThrowsException_HandlesGracefully()
        {
            var vehicleId = Guid.NewGuid();
            _repositoryMock.Setup(r => r.GetLatestInspectionAsync(vehicleId)).ThrowsAsync(new Exception("Repository error"));
            var result = await _service.GetInspectionStatusAsync(vehicleId);
            Assert.Contains("error", result.ToLower());
        }

        [Fact]
        public async Task FailedInspectionWithoutReason_ReturnsGenericMessage()
        {
            var vehicleId = Guid.NewGuid();
            var inspection = new VehicleInspection { VehicleId = vehicleId, Passed = false, InspectionDate = DateTime.UtcNow.AddDays(-2), FailureReason = null };
            _repositoryMock.Setup(r => r.GetLatestInspectionAsync(vehicleId)).ReturnsAsync(inspection);
            var result = await _service.GetInspectionStatusAsync(vehicleId);
            Assert.Contains("failed", result.ToLower());
        }
    }
}