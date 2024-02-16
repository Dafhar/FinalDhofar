using DhofarAppApi.InterFaces;
using DhofarAppApi.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DhofarAppApi.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class VisitorController : ControllerBase
    {
        private readonly IVisitor _context;

        public VisitorController(IVisitor context)
        {
            _context = context;
        }

        [HttpPost("ContinueAsVisitor")]
        public async Task<IActionResult> ContinueAsVisitor()
        {
            string visitor = await _context.ContinueAsVisitor();
            return Ok(new { Vistor = visitor });
        }

        [HttpGet("VisitorStatistics")]
        public async Task<IActionResult> GetVisitorStatistics()
        {
            var (visitorsPerDay, totalVisitors) = await _context.GetVisitorStatistics();
            return Ok(new { VisitorsPerDay = visitorsPerDay, TotalVisitors = totalVisitors });
        }

        [HttpGet("VisitorCounter")]
        public async Task<IActionResult> GetVisitorCounter()
        {
            var counter = await _context.CountVisitors();

            return Ok(new { Counter = counter });
        }
    }
}
