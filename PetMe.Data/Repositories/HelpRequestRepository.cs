using Microsoft.EntityFrameworkCore;
using PetMe.Core.Entities;
using PetMe.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetMe.DataAccess.Repositories
{

    public interface IHelpRequestRepository {
        Task CreateHelpRequestAsync(HelpRequest helpRequest);
        Task<List<HelpRequest>> GetHelpRequestsAsync();
        Task<HelpRequest?> GetHelpRequestByIdAsync(int id);
        Task<List<HelpRequest>> GetHelpRequestsByUserAsync(int userId);
        Task UpdateHelpRequestAsync(HelpRequest helpRequest); 
        Task DeleteHelpRequestAsync(int id);
    }

    public class HelpRequestRepository : IHelpRequestRepository
    {
        private readonly PetMeDbContext _context;

        public HelpRequestRepository(PetMeDbContext context)
        {
            _context = context;
        }

        public async Task CreateHelpRequestAsync(HelpRequest helpRequest)
        {
            await _context.HelpRequests.AddAsync(helpRequest);
            await _context.SaveChangesAsync();
        }

        public async Task<List<HelpRequest>> GetHelpRequestsAsync()
        {
            return await _context.HelpRequests
                .Include(hr => hr.User) 
                .OrderByDescending(hr => hr.CreatedAt) 
                .ToListAsync();
        }

        public async Task<HelpRequest?> GetHelpRequestByIdAsync(int id)
        {
            return await _context.HelpRequests
                .Include(hr => hr.User)
                .FirstOrDefaultAsync(hr => hr.Id == id);
        }

        public async Task<List<HelpRequest>> GetHelpRequestsByUserAsync(int userId)
        {
            return await _context.HelpRequests
                .Where(hr => hr.UserId == userId)
                .Include(hr => hr.User) 
                .OrderByDescending(hr => hr.CreatedAt)
                .ToListAsync();
        }

        public async Task UpdateHelpRequestAsync(HelpRequest helpRequest)
        {
            var existingRequest = await _context.HelpRequests.FindAsync(helpRequest.Id);
            if (existingRequest != null)
            {
                _context.Entry(existingRequest).CurrentValues.SetValues(helpRequest);
                await _context.SaveChangesAsync();
            }
        }
        
        public async Task DeleteHelpRequestAsync(int id)
        {
            var helpRequest = await _context.HelpRequests.FindAsync(id);
            if (helpRequest != null)
            {
                _context.HelpRequests.Remove(helpRequest);
                await _context.SaveChangesAsync();
            }
        }
    }
}
