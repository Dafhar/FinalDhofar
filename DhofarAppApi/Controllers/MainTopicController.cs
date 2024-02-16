using DhofarAppApi.Dtos.Main;
using DhofarAppApi.InterFaces;
using DhofarAppWeb.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DhofarAppApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MainTopicController : ControllerBase
    {
        private readonly IMain _subjectService;

        public MainTopicController(IMain subjectService)
        {
            _subjectService = subjectService;
        }

        [HttpGet("all")]
        public async Task<ActionResult<SubjectForMainDTO>> GetAll()
        {
            var result = await _subjectService.GetAll();
            if (result == null)
            {
                return NotFound();
            }
            return result;
        }
    }
}
