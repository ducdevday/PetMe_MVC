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
    public interface ICommentRepository {
        Task AddCommentAsync(Comment comment);
        Task<List<Comment>> GetCommentsByHelpRequestIdAsync(int helpRequestId);
        Task<Comment?> GetCommentByIdAsync(int id);
        Task UpdateCommentAsync(Comment comment);
        Task DeleteCommentAsync(int id);
    }
    public class CommentRepository : ICommentRepository
    {
        private readonly PetMeDbContext _context;

        public CommentRepository(PetMeDbContext context)
        {
            _context = context;
        }
        public async Task AddCommentAsync(Comment comment)
        {
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCommentAsync(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment != null)
            {
                _context.Comments.Remove(comment);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Comment?> GetCommentByIdAsync(int id)
        {
            return await _context.Comments
                .Include(c => c.User)
                .Include(c => c.Veterinarian)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<List<Comment>> GetCommentsByHelpRequestIdAsync(int helpRequestId)
        {
            return await _context.Comments
                .Include(c => c.User)
                .Include(c => c.Veterinarian)
                .Where(c => c.HelpRequestId == helpRequestId)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }

        public async Task UpdateCommentAsync(Comment comment)
        {
            _context.Comments.Update(comment);
            await _context.SaveChangesAsync();
        }
    }
}
