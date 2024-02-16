using DhofarAppApi.Data;
using DhofarAppApi.InterFaces;
using DhofarAppApi.Model;
using Microsoft.EntityFrameworkCore;

namespace DhofarAppApi.Services
{
    public class RatingSubjectServices : IRatingSubject
    {
        private readonly AppDbContext _db;
        public RatingSubjectServices(AppDbContext db)
        {
            _db = db;
        }
        public async Task<List<RatingSubject>> GetAll()
        {
            return await _db.RatingSubjects.ToListAsync();
        }
    }
}
