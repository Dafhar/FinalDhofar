using DhofarAppApi.InterFaces;
using DhofarAppApi.Data;
using DhofarAppApi.Model;
using Microsoft.EntityFrameworkCore;

namespace DhofarAppApi.Services
{
    public class OnBoardScreenServices : IOnBoardScreen
    {
        private readonly AppDbContext _context;

        public OnBoardScreenServices(AppDbContext context) 
        {
            _context = context;
        }


        public async Task<List<OnBoardScreen>> GetAll()
        {
            var screenOne = await _context.OnBoardScreens.ToListAsync();
            if (screenOne == null)
            {
                return null;
            }
            else
            {
                return screenOne;
            }
        }

       
    }
}
