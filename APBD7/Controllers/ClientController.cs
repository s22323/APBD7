using APBD7.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APBD7.Controllers
{
    [Route("api/clients")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IDbService dbService;

        public ClientController(IDbService dbService)
        {
            this.dbService = dbService;
        }

        [HttpDelete("{idClient}")]
        public async Task<IActionResult> DeleteClient([FromRoute] int idClient)
        {
            return Ok(dbService.DeleteClient(idClient));
        }
    }
}
