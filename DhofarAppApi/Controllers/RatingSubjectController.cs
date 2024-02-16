using DhofarAppApi.InterFaces;
using DhofarAppApi.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DhofarAppApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingSubjectController : ControllerBase
    {
        private readonly IRatingSubject _ratingSubject;
        public RatingSubjectController(IRatingSubject ratingSubject)
        {
            _ratingSubject = ratingSubject;
        }

        [HttpGet]
        public async Task<ActionResult<RatingSubject>> GetAll()
        {
          var rating = await _ratingSubject.GetAll();
            return Ok(rating);
        }
    }
}
