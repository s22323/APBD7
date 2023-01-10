using APBD7.DTOs.Requests;
using APBD7.Services;
using Microsoft.AspNetCore.Mvc;

namespace APBD7.Controllers
{
    [ApiController]
    [Route("api/trips")]
    public class TripController : ControllerBase
    {
        private readonly IDbService dbService;

        public TripController(IDbService dbService)
        {
            this.dbService = dbService;
        }

        [HttpGet]   
        public async Task<IActionResult> GetTrips()
        {
            return Ok(dbService.GetTrips());
        }

        [HttpPost("{idTrip}/clients")]
        public async Task<IActionResult> AssignClientToTrip([FromRoute] int idTrip, [FromBody] AssignClientToTripDTO client)
        {
            return Ok(dbService.AssignClientToTrip(idTrip, client));
        }
    }
}