using Microsoft.AspNetCore.Mvc;
using DhofarAppApi.Model;
using DhofarAppApi.InterFaces;
using DhofarAppApi.Dtos.Complaint;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using DhofarAppWeb.Services;
using DhofarAppApi.Services;

namespace DhofarAppApi.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class ComplaintsController : ControllerBase
    {
        private readonly IComplaint _complaint;

        public ComplaintsController(IComplaint complaint)
        {
            _complaint = complaint;
        }


        [HttpPost("create")]
        public async Task<IActionResult> Create([FromForm] PostComplaintDTO complaint)
        {
            var createdComplaint = await _complaint.CreateComplaint(complaint);
            return Ok(createdComplaint);
        }


    }

}
