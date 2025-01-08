using PetMe.Core.Entities;
using PetMe.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetMe.Business.Services
{
    public interface ICommentService {
        Task AddCommentAsync(Comment comment);
        Task<List<Comment>> GetCommentsByHelpRequestIdAsync(int helpRequestId);
        Task<Comment> GetCommentByIdAsync(int id);
        Task UpdateCommentAsync(Comment comment);
        Task DeleteCommentAsync(int id);
    }
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;

        public CommentService(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public async Task AddCommentAsync(Comment comment)
        {
            await _commentRepository.AddCommentAsync(comment);
        }

        public async Task<List<Comment>> GetCommentsByHelpRequestIdAsync(int helpRequestId)
        {
            return await _commentRepository.GetCommentsByHelpRequestIdAsync(helpRequestId);
        }

        public async Task<Comment> GetCommentByIdAsync(int id)
        {
            var comment = await _commentRepository.GetCommentByIdAsync(id);
            if (comment == null) { 
                throw new KeyNotFoundException();
            }
            return comment;
        }

        public async Task UpdateCommentAsync(Comment comment)
        {
            await _commentRepository.UpdateCommentAsync(comment);
        }

        public async Task DeleteCommentAsync(int id)
        {
            await _commentRepository.DeleteCommentAsync(id);
        }
    }
}
