using DhofarAppApi.Dtos.Comment;
using DhofarAppApi.Dtos.Subject;
using DhofarAppApi.InterFaces;
using DhofarAppApi.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DhofarAppApi.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class CommentSubjectController : ControllerBase
    {
        private readonly ICommentSubject _context;

        public CommentSubjectController(ICommentSubject context)
        {
            _context = context;
        }

        // ToDo: Tested 

        [HttpPost("{subjectId}")]
        [ProducesResponseType(typeof(GetSubjectCommentDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetSubjectCommentDTO>> AddComment( int subjectId, PostCommentDTO postCommentDTO)
        {
            var commentSubject = await _context.CreateCommentSubject(postCommentDTO, subjectId);
            return Ok(commentSubject);
        }

        [HttpGet("{subjectId}")]
        public async Task<ActionResult<IEnumerable<GetSubjectCommentDTO>>> GetSubjectComments(int subjectId)
        {
            var comments = await _context.GetAllCommentForSubjectId(subjectId);

            return Ok(comments);
        }

        [HttpPut("/VoteUp/{commentId}")]
        public async Task<ActionResult> VoteUp(int commentId)
        {
            await _context.VoteUpComment(commentId);
            return Ok();

        }

        [HttpPut("/VoteDown/{commentId}")]
        public async Task<ActionResult> VoteDown(int commentId)
        {
            await _context.VoteDownComment(commentId);
            return Ok();
        }

        [HttpPut("RemoveVoteUp/{commentId}")]
        public async Task<IActionResult> RemoveVoteUpComment(int commentId)
        {
            

            // Call the service to remove the upvote
            await _context.RemoveVoteUpComment(commentId);

            // Return a successful response
            return Ok();
        }

        [HttpPut("RemoveVoteDown/{commentId}")]
        public async Task<IActionResult> RemoveVoteDownComment(int commentId)
        {
            
            // Call the service to remove the downvote .
            await _context.RemoveVoteDownComment(commentId);

            // Return a successful response
            return Ok();

        }
        
        [HttpPut("EditComment/{commentId}")]
        [ProducesResponseType(typeof(GetSubjectCommentDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetSubjectCommentDTO>> EditComment( int commentId,PostCommentDTO commentDTO)
        {
           
            // Call the service to edit the comment
            var editedComment = await _context.EditComment(commentId,commentDTO);

            if (editedComment != null)
            {
                return Ok(editedComment);
            }
            else
            {

                var errorResponse = new { errors = new { message = "You don't have the premissions to edit this comment or something Error" } };
                return BadRequest(errorResponse);
            }
        }

        [HttpDelete("DeleteComment/{commentId}")]
        public async Task<IActionResult> DeleteComment(int commentId)
        {
            // Call the service to delete the comment
            await _context.DeleteComment(commentId);

            return NoContent(); // 204 No Content
        }
    }
}
