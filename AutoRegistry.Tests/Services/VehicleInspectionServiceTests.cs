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
            // Arrange
            var vehicleId = Guid.NewGuid();
            var inspectionDate = DateTime.UtcNow.AddDays(-10);

            var inspection = new VehicleInspection
            {
                VehicleId = vehicleId,
                Passed = true,
                InspectionDate = inspectionDate
            };

            _repositoryMock
                .Setup(r => r.GetLatestInspectionAsync(vehicleId))
                .ReturnsAsync(inspection);

            // Act
            var result = await _service.GetInspectionStatusAsync(vehicleId);

            // Assert
            Assert.Contains("passed", result.ToLower());
            Assert.Contains("valid until", result.ToLower());
        }

        [Fact]
        public async Task FailedInspection_ReturnsFailureReason()
        {
            // Arrange
            var vehicleId = Guid.NewGuid();
            var inspection = new VehicleInspection
            {
                VehicleId = vehicleId,
                Passed = false,
                InspectionDate = DateTime.UtcNow.AddDays(-5),
                FailureReason = "brakes"
            };

            _repositoryMock
                .Setup(r => r.GetLatestInspectionAsync(vehicleId))
                .ReturnsAsync(inspection);

            // Act
            var result = await _service.GetInspectionStatusAsync(vehicleId);

            // Assert
            Assert.Contains("failed", result.ToLower());
            Assert.Contains("brakes", result.ToLower());
        }

        [Fact]
        public async Task ExpiredInspection_ReturnsExpiredMessage()
        {
            // Arrange
            var vehicleId = Guid.NewGuid();
            var inspectionDate = DateTime.UtcNow.AddYears(-2); // expired

            var inspection = new VehicleInspection
            {
                VehicleId = vehicleId,
                Passed = true,
                InspectionDate = inspectionDate
            };

            _repositoryMock
                .Setup(r => r.GetLatestInspectionAsync(vehicleId))
                .ReturnsAsync(inspection);

            // Act
            var result = await _service.GetInspectionStatusAsync(vehicleId);

            // Assert
            Assert.Contains("expired", result.ToLower());
            Assert.Contains(inspection.ValidUntil?.ToShortDateString(), result);
        }

        [Fact]
        public async Task VehicleNotFound_ReturnsNotFoundMessage()
        {
            // Arrange
            var vehicleId = Guid.NewGuid();

            _repositoryMock
                .Setup(r => r.GetLatestInspectionAsync(vehicleId))
                .ReturnsAsync((VehicleInspection?)null); // no result

            // Act
            var result = await _service.GetInspectionStatusAsync(vehicleId);

            // Assert
            Assert.Equal("Vehicle not found", result);
        }
    }
}