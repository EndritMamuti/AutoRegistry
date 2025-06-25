using AutoRegistry.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace AutoRegistry.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehicleController : ControllerBase
    {
        private readonly VehicleInspectionService _inspectionService;

        public VehicleController(VehicleInspectionService inspectionService)
        {
            _inspectionService = inspectionService;
        }

        /// <summary>
        /// Get the latest inspection status of a vehicle.
        /// </summary>
        /// <param name="id">Vehicle ID (GUID)</param>
        /// <returns>
        /// 200 OK with status message if found.  
        /// 404 Not Found if the vehicle does not exist.
        /// </returns>
        [HttpGet("{id}/inspection-status")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(string), 404)]
        public async Task<IActionResult> GetInspectionStatus(Guid id)
        {
            var status = await _inspectionService.GetInspectionStatusAsync(id);

            if (status == "Vehicle not found")
                return NotFound(status);

            return Ok(status);
        }
    }
}