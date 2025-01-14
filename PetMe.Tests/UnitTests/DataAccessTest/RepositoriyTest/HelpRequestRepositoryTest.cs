using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
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
    public class HelpRequestRepositoryTest : IDisposable
    {
        private readonly HelpRequestRepository _helpRequestRepository;
        private readonly PetMeDbContext _context;

        public HelpRequestRepositoryTest()
        {
            var options = new DbContextOptionsBuilder<PetMeDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            _context = new PetMeDbContext(options);
            _helpRequestRepository = new HelpRequestRepository(_context);

            // Ensure the database is clean before each test
            _context.Database.EnsureCreated();
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Fact]
        public async Task CreateHelpRequestAsync_ValidHelpRequest_CreateHelpRequestSuccessfully()
        {
            var helpRequest = new HelpRequest
            {
                Id = 1,
                Title = "Need help with cat",
                Description = "A stray cat is injured and needs medical attention.",
                CreatedAt = DateTime.UtcNow,
                UserId = 10,
                Location = "123 Cat Street",
            };

            // Act
            await _helpRequestRepository.CreateHelpRequestAsync(helpRequest);

            // Assert
            var inserted = await _context.HelpRequests.FindAsync(1);
            Assert.NotNull(inserted);
            Assert.Equal("Need help with cat", inserted.Title);
            Assert.Equal(10, inserted.UserId);
        }


        [Fact]
        public async Task UpdateHelpRequestAsync_ValidHelpRequest_UpdateHelpRequestSuccessfully()
        {
            // Arrange
            var hr = new HelpRequest
            {
                Id = 2,
                Title = "Initial Title",
                Description = "Initial Description",
                CreatedAt = DateTime.UtcNow,
                Location = "123 Cat Street",
            };
            _context.HelpRequests.Add(hr);
            await _context.SaveChangesAsync();

            // Act
            hr.Title = "Updated Title";
            hr.Description = "Updated Description";
            await _helpRequestRepository.UpdateHelpRequestAsync(hr);

            // Assert
            var updatedHr = await _context.HelpRequests.FindAsync(2);
            Assert.NotNull(updatedHr);
            Assert.Equal("Updated Title", updatedHr.Title);
            Assert.Equal("Updated Description", updatedHr.Description);
        }

        [Fact]
        public async Task DeleteHelpRequestAsync_ValidHelpRequest_DeleteHelpRequestSuccessfully()
        {
            // Arrange
            var hr = new HelpRequest
            {
                Id = 3,
                Title = "To be deleted",
                Description = "",
                Location = ""
            };
            _context.HelpRequests.Add(hr);
            await _context.SaveChangesAsync();

            // Act
            await _helpRequestRepository.DeleteHelpRequestAsync(hr.Id);

            // Assert
            var deleted = await _context.HelpRequests.FindAsync(3);
            Assert.Null(deleted);
        }
        //Task<List<HelpRequest>> GetHelpRequestsAsync();
        //Task<HelpRequest?> GetHelpRequestByIdAsync(int id);
        //Task<List<HelpRequest>> GetHelpRequestsByUserAsync(int userId);

        [Fact]
        public async Task GetHelpRequestByIdAsync_ValidIdHelpRequest_ReturnCorrectHeplRequest() {
            // Arrange
            // Arrange
            var helpRequest = new HelpRequest
            {
                Title = "Need help with cat",
                Description = "A stray cat is injured and needs medical attention.",
                CreatedAt = DateTime.UtcNow,
                UserId = 10,
                Location = "123 Cat Street",
            };

            _context.HelpRequests.Add(helpRequest);
            await _context.SaveChangesAsync();

            // Act
            var helpRequestById = await _helpRequestRepository.GetHelpRequestByIdAsync(helpRequest.Id);

            // Assert
            Assert.NotNull(helpRequestById);
            Assert.Equal(helpRequest.Id, helpRequestById.Id);
            Assert.Equal(helpRequest.Title, helpRequestById.Title);
            Assert.Equal(helpRequest.Description, helpRequestById.Description);
            Assert.Equal(helpRequest.UserId, helpRequestById.UserId);
            Assert.Equal(helpRequest.Location, helpRequestById.Location);
        }

        [Fact]
        public async Task GetHelpRequestsAsync_ReturnAllHelpRequest() {
            var helpRequest = new HelpRequest
            {
                Id = 1,
                Title = "Need help with cat",
                Description = "A stray cat is injured and needs medical attention.",
                CreatedAt = DateTime.UtcNow,
                UserId = 10,
                Location = "123 Cat Street",
            };

            _context.HelpRequests.Add(helpRequest);
            await _context.SaveChangesAsync();

            // Assert
            var helpRequests = await _helpRequestRepository.GetHelpRequestsAsync();
            Assert.NotNull(helpRequests);
        }

        [Fact]
        public async Task GetHelpRequestsByUserAsync() {
            var userId = 10;
            var helpRequest = new HelpRequest
            {
                Id = 1,
                Title = "Need help with cat",
                Description = "A stray cat is injured and needs medical attention.",
                CreatedAt = DateTime.UtcNow,
                UserId = 10,
                Location = "123 Cat Street",
            };

            _context.HelpRequests.Add(helpRequest);
            await _context.SaveChangesAsync();

            // Assert
            var helpRequests = await _helpRequestRepository.GetHelpRequestsByUserAsync(userId);
            Assert.NotNull(helpRequests);
            Assert.All<HelpRequest>(helpRequests, hr => Assert.Equal(userId, hr.UserId));
        }

    }
}
