using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using PetMe.Data;
using PetMe.Data.Entities;
using PetMe.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetMe.Tests.UnitTests.DataAccessTest.RepositoriyTest
{
    public class CommentRepositoryTests : IDisposable
    {
        private readonly CommentRepository _commentRepository;
        private readonly PetMeDbContext _context;

        public CommentRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<PetMeDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            _context = new PetMeDbContext(options);
            _commentRepository = new CommentRepository(_context);

            // Ensure the database is clean before each test
            _context.Database.EnsureCreated();
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Fact]
        public async Task AddCommentAsync_ValidComment_AddsCommentSuccessfully () {
            var newComment = new Comment
            {
                Id = 1,
                HelpRequestId = 10,
                UserId = 100,
                Content = "Test comment",
                CreatedAt = DateTime.UtcNow
            };

            // Act
            await _commentRepository.AddCommentAsync(newComment);

            // Assert
            var insertedComment = await _context.Comments.FindAsync(newComment.Id);
            Assert.NotNull(insertedComment);
            Assert.Equal(10, insertedComment.HelpRequestId);
            Assert.Equal(100, insertedComment.UserId);
            Assert.Equal("Test comment", insertedComment.Content);
        }

        [Fact]
        public async Task UpdateCommentAsync_ValidComment_UpdatesCommentSuccessfully() {
            var comment = new Comment
            {
                Id = 10,
                HelpRequestId = 99,
                UserId = 999,
                Content = "Original comment",
                CreatedAt = DateTime.UtcNow
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            // Act
            comment.Content = "Updated comment";
            await _commentRepository.UpdateCommentAsync(comment);

            // Assert
            var updated = await _context.Comments.FindAsync(10);
            Assert.NotNull(updated);
            Assert.Equal("Updated comment", updated.Content);
        }

        [Fact]
        public async Task DeleteCommentAsync_ValidId_DeletesCommentSuccessfully() {
            var comment = new Comment
            {
                Id = 7,
                HelpRequestId = 88,
                UserId = 888,
                Content = "Comment to delete",
                CreatedAt = DateTime.UtcNow
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            // Act
            await _commentRepository.DeleteCommentAsync(comment.Id);

            // Assert
            var deletedComment = await _context.Comments.FindAsync(comment.Id);
            Assert.Null(deletedComment);
        }

        [Fact]
        public async Task GetCommentsByHelpRequestIdAsync_ValidHelpRequestId_ReturnsListOfComments() {
            var helpRequestId = 99;
            var comment = new Comment
            {
                Id = 10,
                HelpRequestId = helpRequestId,
                UserId = 999,
                Content = "Original comment",
                CreatedAt = DateTime.UtcNow
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            // Assert
            var comments = await _commentRepository.GetCommentsByHelpRequestIdAsync(helpRequestId);
            Assert.NotNull(comments);
        }

        [Fact]
        public async Task GetCommentByIdAsync_ValidId_ReturnsCorrectComment() {
            var commentId = 99;
            var comment = new Comment
            {
                Id = commentId,
                HelpRequestId = 99,
                UserId = 999,
                Content = "Original comment",
                CreatedAt = DateTime.UtcNow
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            // Assert
            var commentById = await _commentRepository.GetCommentByIdAsync(commentId);
            Assert.NotNull(commentById);
            Assert.Equal(commentId, commentById.Id);
        }

    }
}
