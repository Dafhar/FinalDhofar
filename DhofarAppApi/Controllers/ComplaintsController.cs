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
        public async Task<IActionResult> Create( PostComplaintDTO complaint)
        {
            var createdComplaint = await _complaint.CreateComplaint(complaint);
            return Ok(createdComplaint);
        }

        [HttpGet("GetMyAllComplaints")]
        public async Task<IActionResult> GetMyComplaints()
        {
            var complaints = await _complaint.GetMyComplaints();
            return Ok(complaints);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetCompliantById(int id)
        {
            var compliant = await _complaint.GetComplaintById(id);
            return Ok(compliant);
        }

    }

}
