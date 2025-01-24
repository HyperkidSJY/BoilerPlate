using BoilerPlate.Models;
using BoilerPlate.Models.DTO;
using BoilerPlate.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BoilerPlate.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenerateController : ControllerBase
    {
        private readonly GenerateTablesService _tablesService;
        private Response _objResponse; 
        public GenerateController()
        {
            _tablesService = new GenerateTablesService();
            _objResponse = new Response();
        }
        [HttpPost, Route("GenerateTable")]
        public IActionResult  CreateQuery([FromBody]List<DTOTableDefinition> lstDTOTableDefinitions, [FromQuery]string tableName)
        {
            _objResponse = _tablesService.GenerateTableQuery(lstDTOTableDefinitions , tableName);
            _tablesService.GenerateDTO(lstDTOTableDefinitions, tableName);
            return Ok(_objResponse);
        }
    }
}
