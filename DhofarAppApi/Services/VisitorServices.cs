using DhofarAppApi.InterFaces;
using DhofarAppApi.Data;
using DhofarAppApi.Model;
using Microsoft.EntityFrameworkCore;

namespace DhofarAppApi.Services
{
    public class VisitorServices : IVisitor
    {
        private readonly AppDbContext _context;

        public VisitorServices(AppDbContext context)
        {
            _context = context;
        }
        public async Task<string> ContinueAsVisitor()
        {
            // Generate a random number as the visitor's name
            Random rnd = new Random();
            string visitorName = "Visitor " + rnd.Next(100000, 999999).ToString(); // Change the range as needed


            // Create Visitor record
            var visitor = new Visitor
            {
                Name =  visitorName,
                JoinedDate = DateTime.UtcNow,
            };

            // Save visitor record to database
            await _context.Visitors.AddAsync(visitor);
            await _context.SaveChangesAsync();

            // Return the visitor's name
            return visitorName;
        }

        public async Task<int> CountVisitors()
        {
            var visitorCount = await _context.Visitors.CountAsync();

            return visitorCount;
        }

        public async Task<(List<object>, int)> GetVisitorStatistics()
        {
            var visitorsPerDay = await _context.Visitors
           .GroupBy(v => v.JoinedDate.Date)
           .Select(g => new { Date = g.Key, Count = g.Count() })
           .ToListAsync();

            // Example: Get total number of visitors
            var totalVisitors = await _context.Visitors.CountAsync();

            return (visitorsPerDay.Cast<object>().ToList(), totalVisitors);
        }
    }
}
