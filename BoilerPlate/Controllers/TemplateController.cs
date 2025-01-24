using BoilerPlate.Models;
using BoilerPlate.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BoilerPlate.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TemplateController : ControllerBase
    {
        private readonly TemplatesServices _templatesService;
        Response _objResponse; 

        public TemplateController(TemplatesServices templatesServices)
        {
            _templatesService  = templatesServices;
            _objResponse= new Response();
        }
        [HttpGet("GetTemplate/{fileName}")]
        public IActionResult GetTemplate(string fileName)
        {
           _objResponse =  _templatesService.GetTemplate(fileName);
    
            return Ok(JsonConvert.SerializeObject(_objResponse));
        }
    }
}
