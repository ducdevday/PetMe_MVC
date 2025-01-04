using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetMe.Data.Repositories
{
    public interface IAdminRepository
    {
        Task<bool> IsUserAdminAsync(int userId);
    }

    public class AdminRepository : IAdminRepository
    {
        private readonly PetMeDbContext _context;

        public AdminRepository(PetMeDbContext context)
        {
            _context = context;
        }

        public Task<bool> IsUserAdminAsync(int userId)
        {
            return Task.FromResult(_context.Admins.Any(x => x.Id == userId));
        }
    }
}
