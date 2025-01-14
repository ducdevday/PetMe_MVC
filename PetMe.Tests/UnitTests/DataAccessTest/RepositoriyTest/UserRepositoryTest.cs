using Microsoft.EntityFrameworkCore;
using PetMe.Data;
using PetMe.Data.Entities;
using PetMe.DataAccess.Repositories;

namespace PetMe.Tests.UnitTests.DataAccessTest.RepositoriyTest
{
    public class UserRepositoryTest : IDisposable
    {
        private readonly PetMeDbContext _context;

        public UserRepositoryTest()
        {
            // Use a unique name for the in-memory database for each test class run
            var options = new DbContextOptionsBuilder<PetMeDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Ensures a new in-memory DB for each test
                .Options;

            _context = new PetMeDbContext(options);
            _context.Database.EnsureCreated();
        }

        public void Dispose()
        {
            // Ensure the database is deleted after each test
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Fact]
        public async Task AddAsync_AddUserToDb_HaveRecord()
        {
            // Arrange
            var repository = new UserRepository(_context);
            var user = new User
            {
                Username = "testuser",
                Email = "test@example.com",
                PasswordHash = "hashedpassword",
                PhoneNumber = "1234567890",
                Address = "Test Address",
                DateOfBirth = DateTime.Now.AddYears(-25),
                IsActive = true,
                CreatedDate = DateTime.Now,
                ProfileImageUrl = "http://example.com/profile.jpg"
            };

            // Act
            await repository.AddAsync(user);

            // Assert
            var savedUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == "testuser");
            Assert.NotNull(savedUser);
            Assert.Equal("testuser", savedUser.Username);
        }

        [Fact]
        public async Task GetAllAsync_GetAllUser_ReturnAllUsers()
        {
            // Arrange
            var repository = new UserRepository(_context);

            var user1 = new User
            {
                Username = "user1",
                Email = "user1@example.com",
                PasswordHash = "hashedpassword1",
                PhoneNumber = "1234567891",
                Address = "Address 1",
                DateOfBirth = DateTime.Now.AddYears(-30),
                IsActive = true,
                CreatedDate = DateTime.Now,
                ProfileImageUrl = "http://example.com/user1.jpg"
            };

            var user2 = new User
            {
                Username = "user2",
                Email = "user2@example.com",
                PasswordHash = "hashedpassword2",
                PhoneNumber = "1234567892",
                Address = "Address 2",
                DateOfBirth = DateTime.Now.AddYears(-28),
                IsActive = true,
                CreatedDate = DateTime.Now,
                ProfileImageUrl = "http://example.com/user2.jpg"
            };

            await repository.AddAsync(user1);
            await repository.AddAsync(user2);

            // Act
            var users = await repository.GetAllAsync();

            // Assert
            Assert.NotNull(users);
            Assert.Equal(2, users.Count());  // Beklenen değer 2
        }

        [Fact]
        public async Task GetByIdAsync_GetNotExistUser_ReturnNull()
        {
            // Arrange
            var repository = new UserRepository(_context);

            // Act
            var foundUser = await repository.GetByIdAsync(-1); 

            // Assert
            Assert.Null(foundUser);
        }

        [Fact]
        public async Task UpdateAsync_UpdateUser_EffectRecord() {
            // Arrange
            var repository = new UserRepository(_context);
            var user = new User
            {
                Username = "testuser",
                Email = "test@example.com",
                PasswordHash = "hashedpassword",
                PhoneNumber = "1234567890",
                Address = "Test Address",
                DateOfBirth = DateTime.Now.AddYears(-25),
                IsActive = true,
                CreatedDate = DateTime.Now,
                ProfileImageUrl = "http://example.com/profile.jpg"
            };

            await repository.AddAsync(user);

            // Act
            user.Username = "updateduser";
            await repository.UpdateAsync(user);

            // Assert
            var updatedUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
            Assert.NotNull(updatedUser);
            Assert.Equal("updateduser", updatedUser.Username);
        }

    }
}
