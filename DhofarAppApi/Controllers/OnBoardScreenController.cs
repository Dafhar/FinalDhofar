using DhofarAppApi.InterFaces;
using DhofarAppApi.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DhofarAppApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OnBoardScreenController : ControllerBase
    {

        private readonly IOnBoardScreen _context;

        public OnBoardScreenController(IOnBoardScreen context)
        {
            _context = context;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<List<OnBoardScreen>>> GetAll()
        {
            var screenOne = await _context.GetAll();

            return screenOne == null ? NotFound() : screenOne;
        }

        
    }
}
