using DhofarAppApi.Dtos.GeneralSubject;
using DhofarAppApi.Dtos.SubjectType;
using DhofarAppApi.InterFaces;
using DhofarAppApi.Model;
using DhofarAppWeb.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DhofarAppApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[Action]")]
    public class GeneralSubjectTypesController : ControllerBase
    {
        private readonly IGeneralSubject _generalSubjectType;

        public GeneralSubjectTypesController(IGeneralSubject generalSubjectType)
        {
            _generalSubjectType = generalSubjectType;
        }

        [HttpGet]
        public async Task<ActionResult<List<GetGeneralSubjectTypeToAllDTO>>> GetAllSubjectTypes()
        {
            var subjectTypes = await _generalSubjectType.GetAllSubjectTypesForAll();
            return Ok(subjectTypes);
        }

        [HttpGet("AllGeneralSbujectsTypesForAdmin")]
        public async Task<ActionResult<List<GetGeneralSubjectTypeDTO>>> GetAllSubjectTypesForAdmins()
        {
            var subjectTypes = await _generalSubjectType.GetAllSubjectTypesForAdmin();
            return Ok(subjectTypes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetSinlgeGeneralSubjectTypeDTO>> GetSubjectTypeById(int id)
        {
            var subjectType = await _generalSubjectType.GetSubjectTypeById(id);

            if (subjectType == null)
            {
                var errorResponse = new { errors = new { message = "Not found" } };
                return BadRequest(errorResponse);

            }

            return Ok(subjectType);
        }

        [HttpPost]
        public async Task<ActionResult<GeneralSubjectsType>> CreateSubjectType(PostGeneralSubjectTypeDTO postSubjectTypeDto)
        {
            var subjectType = await _generalSubjectType.CreateSubjectType(postSubjectTypeDto);
            return CreatedAtAction(nameof(GetSubjectTypeById), new { id = subjectType.Id }, subjectType);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSubjectType(int id, PostGeneralSubjectTypeDTO postSubjectTypeDto)
        {
            var updatedSubjectType = await _generalSubjectType.UpdateSubjectType(id, postSubjectTypeDto);

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
            var deleted = await _generalSubjectType.DeleteSubjectType(id);

            if (!deleted)
            {
                var errorResponse = new { errors = new { message = "Not found" } };
                return BadRequest(errorResponse);

            }

            return NoContent();
        }
    }
}
