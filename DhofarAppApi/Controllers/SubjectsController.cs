using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DhofarAppApi.Model;
using DhofarAppApi.InterFaces;
using DhofarAppApi.Dtos.Subject;
using DhofarAppApi.Dtos.Complaint;
using Microsoft.AspNetCore.Authorization;
using DhofarAppApi.Dtos.SubjectFiles;
using SendGrid.Helpers.Mail;
using Microsoft.AspNetCore.Http.HttpResults;

namespace DhofarAppApi.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class SubjectsController : ControllerBase
    {
        private readonly ISubject _db;


        public SubjectsController(ISubject context)
        {
            _db = context;
            
        }

        // GET: api/Subjects
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetSubjectDTO>>> GetAllSubjects()
        {
            var subjects =  await _db.GetAllSubjects();
            return subjects;
        }

        // GET: api/Subjects/5
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<GetSubjectDTO>> GetSubjectById(int id)
        {
          
            // Retrieve the subject details
            var subject = await _db.GetSubjectById(id);

            // Increment the visitor count using the service
            _db.IncrementVisitorCounter(id);


            if (subject == null)
            {
                return NotFound();
            }
            else
            {
                int visitorCounter = _db.getVisitorCounter(id);

                subject.VisitorCounter = visitorCounter;
                return subject;
            }
        }

        [HttpGet("ByUser")]
        public async Task<ActionResult<IEnumerable<GetSubjectDTO>>> GetAllSubjectByUser()
        {

            var subjectsForUser = await _db.GetSubjectByUserId();
            if (subjectsForUser.Count > 0)
            {
                return Ok(subjectsForUser);

            }
            else
            {
                return NotFound();
            }
        }

        // PUT: api/Subjects/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("Edit/{id}")]
        public async Task<IActionResult> PutSubject(int id, EditSubjectDTO subject)
        {
           var subjectToEdit = await _db.EditSubject(id,  subject);
            if (subjectToEdit == null)
            {
                var errorResponse = new { errors = new { message = subjectToEdit } };
                return BadRequest(errorResponse);
            }
            else
            {
                return Ok(subjectToEdit);
            }
        }

        // POST: api/Subjects
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754

        [HttpPost("Create")]
        [Authorize]
        [ProducesResponseType(typeof(GetSubjectFilesDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetSubjectDTO>> PostSubject(PostSubjectDTO subject)
        {
            var newSubject = await _db.CreateSubject(subject);

            return newSubject;
        }

        // DELETE: api/Subjects/5
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteSubject(int id)
        {
           var subject = await _db.DeleteSubject(id);
            if (subject == null)
            {
                var errorResponse = new { errors = new { message = "Not found" } };
                return BadRequest(errorResponse);
            }
            return Ok(subject);
        }

        [HttpPost("Like/{id}")]
        [Authorize]
        public async Task<IActionResult> LikeSubject(int id)
        {
            var result = await _db.Like(id);
            
            
                return Ok(result);
            
        }

        [HttpPost("Dislike/{id}")]
        [Authorize]
        public async Task<IActionResult> DislikeSubject(int id)
        {
                var result = await _db.Dislike(id);
            
            
                return Ok(result);
            
        }
        [Authorize]
        [HttpPost("AddToFavorite/{id}")]
        public async Task<IActionResult> AddSubjectToFavorite(int id)
        {
            await _db.AddSubjectToFavorite(id);

            return Ok(); // or return any appropriate response
        }

        [Authorize]
        [HttpPost("RemoveFromFavorite/{id}")]
        public async Task<IActionResult> RemoveSubjectFromFavorite(int id)
        {
            await _db.RemoveSubjectFromFavorite(id);

            return Ok(); // or return any appropriate response
        }


        [Authorize]
        [HttpGet("GetAllFavoriteSubjects")]
        public async Task<ActionResult<List<GetSubjectDTO>>> GetAllFavoriteSubjects()
        {
            var favSubjects = await _db.GetFavoriteSubjects();

            return favSubjects;
        }

        [Authorize]
        [HttpPost("VoteForAoption")]
        public async Task<IActionResult> VoteForPollOption([FromBody] VoteDTO voteDto)
        {
            if (voteDto == null || voteDto.SubjectId <= 0 || voteDto.PollId <= 0 || voteDto.PollOptionId <= 0)
            {
                var errorResponse = new { errors = new { message = "Invalid vote information" } };
                return BadRequest(errorResponse);

            }

            var result = await _db.VoteForPollOption(voteDto.SubjectId, voteDto.PollId, voteDto.PollOptionId);

            if (result)
            {
                return Ok("Vote successful");
            }
            else
            {
                var errorResponse = new { errors = new { message = "Unable to vote" } };
                return BadRequest(errorResponse);

            }

        }

        [Authorize]
        [HttpGet("Polls/{subjectId}")]
        public async Task<ActionResult<List<PollDTO>>> GetPollBySubject(int subjectId)
        {
            var polls = await _db.GetPollBySubject(subjectId);

            if (polls != null)
            {
                return polls;
            }
            else
            {
                var errorResponse = new { errors = new { message = "Not found" } };
                return BadRequest(errorResponse);
            }
        }

        [Authorize]
        [HttpDelete("DeleteVote")]
        public async Task<IActionResult> DeleteVote([FromBody] VoteDTO voteDto)
        {

            if (voteDto == null || voteDto.SubjectId <= 0 || voteDto.PollId <= 0 || voteDto.PollOptionId <= 0)
            {
                var errorResponse = new { errors = new { message = "Invalid vote information" } };
                return BadRequest(errorResponse);
            }

            var result = await _db.DeleteVote(voteDto.SubjectId, voteDto.PollId, voteDto.PollOptionId);

            if (result)
            {
                return Ok("Vote deleted successfully");
            }
            else
            {
                var errorResponse = new { errors = new { message = "Unable to delete vote" } };
                return BadRequest(errorResponse);
            }
        }

        [HttpGet("MostInterActiveSubject")]
        public async Task<ActionResult<Subject>> MostInterAciveSubject()
        {
            var most = await _db.GetTheMostSubjectInteract();

            if (most != null)
                return Ok(most);
            else
            {
                var errorResponse = new { errors = new { message = "Not found" } };
                return BadRequest(errorResponse);
            }
        }




        [HttpGet("GetSubjectCount")]
        public async Task<ActionResult<int>> GetSubjectCount()
        {
            var counter = await _db.GetCountOfSubjects();

            return counter > 0 ? Ok(counter) : NotFound();
        }




    }
}
