using DhofarAppApi.InterFaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DhofarAppApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {

        private readonly ISearch _context;

        public SearchController(ISearch context)
        {
            _context = context;
        }



        [HttpGet("search")]
        public async Task<IActionResult> SearchSubject(string subjectName)
        {

            var subjectRrsults = await _context.SearchForSubject(subjectName);
            if (subjectRrsults == null)
                return NotFound();
            else
                return Ok(subjectRrsults);

        }

    }
}
