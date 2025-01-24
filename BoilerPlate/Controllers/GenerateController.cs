﻿using BoilerPlate.Models;
using BoilerPlate.Models.DTO;
using BoilerPlate.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BoilerPlate.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenerateController : ControllerBase
    {
        private readonly GenerateTablesService _tablesService;
        private Response _objResponse; 
        public GenerateController(GenerateTablesService tablesService)
        {
            _tablesService = tablesService;
            _objResponse = new Response();
        }
        [HttpPost, Route("GenerateTable/{tableName}")]
        public IActionResult  CreateQuery([FromBody]List<DTOTableDefinition> lstDTOTableDefinitions, string tableName)
        {
            return Ok(JsonConvert.SerializeObject(_tablesService.GenerateTableQuery(lstDTOTableDefinitions, tableName)));
        }
    }
}
