using DhofarAppApi.Dtos.SubjectType;
using DhofarAppApi.InterFaces;
using DhofarAppApi.Model;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DhofarAppApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[Action]")]
    public class SubjectTypesController : ControllerBase
    {
        private readonly ISubjectType _subjectTypeService;

        public SubjectTypesController(ISubjectType subjectTypeService)
        {
            _subjectTypeService = subjectTypeService;
        }

        [HttpGet]
        public async Task<ActionResult<List<GetSubjectTypeDTO>>> GetAllSubjectTypes()
        {
            var subjectTypes = await _subjectTypeService.GetAllSubjectTypes();
            return Ok(subjectTypes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetSubjectTypeDTO>> GetSubjectTypeById(int id)
        {
            var subjectType = await _subjectTypeService.GetSubjectTypeById(id);

            if (subjectType == null)
            {
                var errorResponse = new { errors = new { message = "Not found" } };
                return BadRequest(errorResponse);

            }

            return Ok(subjectType);
        }

        [HttpPost]
        public async Task<ActionResult<SubjectType>> CreateSubjectType(PostSubjectTypeDTO postSubjectTypeDto)
        {
            var subjectType = await _subjectTypeService.CreateSubjectType(postSubjectTypeDto);
            return CreatedAtAction(nameof(GetSubjectTypeById), new { id = subjectType.Id }, subjectType);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSubjectType(int id, PostSubjectTypeDTO postSubjectTypeDto)
        {
            var updatedSubjectType = await _subjectTypeService.UpdateSubjectType(id, postSubjectTypeDto);

            if (updatedSubjectType == null)
            {
                var errorResponse = new { errors = new { message = "Not found" } };
                return BadRequest(errorResponse);

            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubjectType(int id)
        {
            var deleted = await _subjectTypeService.DeleteSubjectType(id);

            if (!deleted)
            {
                var errorResponse = new { errors = new { message = "Not found" } };
                return BadRequest(errorResponse);

            }

            return NoContent();
        }
    }
}
