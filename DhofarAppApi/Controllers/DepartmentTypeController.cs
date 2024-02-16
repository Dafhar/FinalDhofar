using DhofarAppApi.Dtos.DepartmentType;
using DhofarAppApi.InterFaces;
using DhofarAppApi.Model;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]/[Action]")]
public class DepartmentTypeController : ControllerBase
{
    private readonly IDepartmentType _departmentTypeService;

    public DepartmentTypeController(IDepartmentType departmentTypeService)
    {
        _departmentTypeService = departmentTypeService;
    }

    [HttpPost]
    public async Task<ActionResult<DepartmentTypeDTO>> CreateDepartmentType(DepartmentTypeDTO departmentType)
    {
        var createdDepartmentType = await _departmentTypeService.CreateDepartmentType(departmentType);
        return Ok(createdDepartmentType);
    }

    [HttpGet]
    public async Task<ActionResult<List<DepartmentTypeDTO>>> GetAllDepartmentTypes()
    {
        var departmentTypes = await _departmentTypeService.GetAllDepartmentTypes();
        return Ok(departmentTypes);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<DepartmentTypeDTO>> GetDepartmentTypeById(int id)
    {
        var departmentType = await _departmentTypeService.GetDepartmentTypeById(id);
        if (departmentType == null)
        {
            var errorResponse = new { errors = new { message = "Not found" } };
            return BadRequest(errorResponse);
        }

        return Ok(departmentType);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<DepartmentTypeDTO>> UpdateDepartmentType(int id, DepartmentTypeDTO updatedDepartmentType)
    {
        var departmentType = await _departmentTypeService.UpdateDepartmentType(id, updatedDepartmentType);
        if (departmentType == null)
        {
            var errorResponse = new { errors = new { message = "Not found" } };
            return BadRequest(errorResponse);
        }

        return Ok(departmentType);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDepartmentType(int id)
    {
        var result = await _departmentTypeService.DeleteDepartmentType(id);
        if (!result)
        {
            var errorResponse = new { errors = new { message = "Not found" } };
            return BadRequest(errorResponse);
        }

        return NoContent(); // Successfully deleted
    }
}
