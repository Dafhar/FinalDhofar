using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DhofarAppApi.Model;
using DhofarAppApi.InterFaces;
using DhofarAppApi.Dtos.ReplyComment;

namespace DhofarAppApi.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class CommentRepliesController : ControllerBase
    {
        private readonly IReplyComment _context;

        public CommentRepliesController(IReplyComment context)
        {
            _context = context;
        }
        // ToDo: Tested 

        // GET: api/CommentReplies/{commentSubjectId}
        [HttpGet("{commentSubjectId}")]
        public async Task<ActionResult<IEnumerable<GetReplyCommentDTO>>> GetCommentReplies(int commentSubjectId)
        {
            var replies = await _context.GetCommentReplies(commentSubjectId);
            if (replies == null || !replies.Any())
                return NotFound(); // return 404 if no replies found
            return Ok(replies);
        }
        // ToDo: Tested 

        // GET: api/CommentReplies/getReplyCommentById/{id}
        [HttpGet("getReplyCommentById/{id}")]
        public async Task<ActionResult<GetReplyCommentDTO>> GetCommentReplyById(int id)
        {
            var commentReply = await _context.GetCommentRepliesById(id);
            if (commentReply == null)
                return NotFound(); // return 404 if reply not found
            return Ok(commentReply);
        }
        // ToDo: Tested 

        [HttpPut("EditReplyComment/{id}")]
        public async Task<ActionResult<GetReplyCommentDTO>> PutCommentReplies( PostReplyCommentDTO commentReplies, int id)
        {
            var updatedReply = await _context.EditReplyComment(commentReplies, id);
            if (updatedReply == null)
                return NotFound(); // return 404 if reply not found
            return Ok(updatedReply);
        }
        // ToDo: Tested 

        // POST: api/CommentReplies/{commentSubjectId}
        [HttpPost("{commentSubjectId}")]
        [ProducesResponseType(typeof(CommentSubject), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<ActionResult<GetReplyCommentDTO>> PostCommentReplies(  PostReplyCommentDTO commentReplies, int commentSubjectId)
        {
            var createdReply = await _context.AddReply(commentReplies, commentSubjectId);
            if (createdReply == null)
                return NotFound(); // return 404 if reply not created
            return Ok(createdReply);
        }

        // ToDo: Tested 

        // DELETE: api/CommentReplies/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCommentReplies(int id)
        {
            var deletedReply = await _context.DeleteReplyComment(id);
            if (deletedReply == null)
                return NotFound(); // return 404 if reply not found
            return Ok(deletedReply);
        }
    }
}
