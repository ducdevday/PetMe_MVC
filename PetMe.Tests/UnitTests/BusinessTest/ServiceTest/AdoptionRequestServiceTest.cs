using Microsoft.EntityFrameworkCore;
using Moq;
using PetMe.Business.Services;
using PetMe.Data.Entities;
using PetMe.Data.Enums;
using PetMe.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetMe.Tests.UnitTests.BusinessTest
{
    public class AdoptionRequestServiceTest
    {
        private readonly Mock<IAdoptionRequestRepository> _mockRepository;
        private readonly Mock<IAdoptionRepository> _mockAdoptionRepository;
        private readonly AdoptionRequestService _service;

        public AdoptionRequestServiceTest()
        {
            _mockRepository = new Mock<IAdoptionRequestRepository>();
            _mockAdoptionRepository = new Mock<IAdoptionRepository>();
            _service = new AdoptionRequestService(_mockRepository.Object, _mockAdoptionRepository.Object);
        }

        [Fact]
        public async Task CreateAdoptionRequestAsync_ValidAdopRequest_CreateSuccessfully()
        {
            // Arrange
            var adoptionRequest = new AdoptionRequest
            {
                Id = 1,
                PetId = 1,
                UserId = 1,
                Status = AdoptionStatus.Pending,
                RequestDate = DateTime.UtcNow,
            };

            _mockAdoptionRepository.Setup(r => r.GetAdoptionByPetIdAsync(1)).ReturnsAsync((Adoption?)null);
            _mockRepository.Setup(r => r.AddAsync(adoptionRequest)).Returns(Task.CompletedTask);
            // Act
            await _service.CreateAdoptionRequestAsync(adoptionRequest);
        }

        [Fact]
        public async Task UpdateAdoptionRequestAsync_ValidChanges_UpdateSuccessfully()
        {

            var updatedAdoptionRequest = new AdoptionRequest
            {
                Id = 1,
                PetId = 2,
                UserId = 1,
                Status = AdoptionStatus.Pending,
                RequestDate = DateTime.UtcNow,
            };

            _mockRepository.Setup(r => r.UpdateAsync(updatedAdoptionRequest)).Returns(Task.CompletedTask);

            // Act
            await _service.UpdateAdoptionRequestAsync(updatedAdoptionRequest);

            _mockRepository.Verify(r => r.UpdateAsync(It.Is<AdoptionRequest>(a => a.PetId == 2)), Times.Once);
        }

        [Fact]
        public async Task GetAdoptionRequestByIdAsync_ValidAdoptionRequestId_ReturnAdoptionRequest() {
            var adoptionRequest = new AdoptionRequest
            {
                Id = 1,
                PetId = 1,
                UserId = 1,
                Status = AdoptionStatus.Pending,
                RequestDate = DateTime.UtcNow,
            };

            _mockRepository.Setup(r => r.GetByIdAsync(adoptionRequest.Id)).ReturnsAsync(adoptionRequest);

            await _service.GetAdoptionRequestByIdAsync(adoptionRequest.Id);

            _mockRepository.Verify(r => r.GetByIdAsync(It.Is<int>(id => id == adoptionRequest.Id)), Times.Once);
        }

        [Fact]
        public async Task GetAdoptionRequestsByPetIdAsync_ValidPetId_ReturnCorrentAdoptionRequest() {
            var adoptionRequest = new AdoptionRequest
            {
                Id = 1,
                PetId = 1,
                UserId = 1,
                Status = AdoptionStatus.Pending,
                RequestDate = DateTime.UtcNow,
            };
            var adoptionRequest2 = new AdoptionRequest
            {
                Id = 2,
                PetId = 1,
                UserId = 2,
                Status = AdoptionStatus.Pending,
                RequestDate = DateTime.UtcNow,
            };
            var adoptionRequest3 = new AdoptionRequest
            {
                Id = 3,
                PetId = 2,
                UserId = 2,
                Status = AdoptionStatus.Pending,
                RequestDate = DateTime.UtcNow,
            };

            _mockRepository.Setup(r => r.GetAdoptionRequestsByPetIdAsync(1)).ReturnsAsync(new List<AdoptionRequest>() { adoptionRequest, adoptionRequest2 });

            var result = await _service.GetPendingRequestsByPetIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetAdoptionRequestsByPetIdAsync_InValidPetId_ReturnEmpty()
        {
            var adoptionRequest = new AdoptionRequest
            {
                Id = 1,
                PetId = 1,
                UserId = 1,
                Status = AdoptionStatus.Pending,
                RequestDate = DateTime.UtcNow,
            };
            var adoptionRequest2 = new AdoptionRequest
            {
                Id = 2,
                PetId = 1,
                UserId = 2,
                Status = AdoptionStatus.Pending,
                RequestDate = DateTime.UtcNow,
            };
            var adoptionRequest3 = new AdoptionRequest
            {
                Id = 3,
                PetId = 2,
                UserId = 2,
                Status = AdoptionStatus.Pending,
                RequestDate = DateTime.UtcNow,
            };

            _mockRepository.Setup(r => r.GetAdoptionRequestsByPetIdAsync(-1)).ReturnsAsync(new List<AdoptionRequest>());

            var result = await _service.GetPendingRequestsByPetIdAsync(-1);

            Assert.Empty(result);
        }

        [Fact]
        public async Task GetPendingRequestsByPetIdAsync_ValidPetId_ReturnCorrentRequestList() {
            var adoptionRequest = new AdoptionRequest
            {
                Id = 1,
                PetId = 1,
                UserId = 1,
                Status = AdoptionStatus.Pending,
                RequestDate = DateTime.UtcNow,
            };
            var adoptionRequest2 = new AdoptionRequest
            {
                Id = 2,
                PetId = 1,
                UserId = 2,
                Status = AdoptionStatus.Pending,
                RequestDate = DateTime.UtcNow,
            };
            var adoptionRequest3 = new AdoptionRequest
            {
                Id = 3,
                PetId = 2,
                UserId = 2,
                Status = AdoptionStatus.Pending,
                RequestDate = DateTime.UtcNow,
            };

            _mockRepository.Setup(r => r.GetAdoptionRequestsByPetIdAsync(1)).ReturnsAsync(new List<AdoptionRequest>() { adoptionRequest, adoptionRequest2 });

            var result = await _service.GetPendingRequestsByPetIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetAdoptionRequestByUserAndPetAsync_ValidUserIdAndPetId_ReturnCorrentAdoptRequest() {
            var adoptionRequest = new AdoptionRequest
            {
                Id = 1,
                PetId = 1,
                UserId = 1,
                Status = AdoptionStatus.Pending,
                RequestDate = DateTime.UtcNow,
            };
            var adoptionRequest2 = new AdoptionRequest
            {
                Id = 2,
                PetId = 1,
                UserId = 2,
                Status = AdoptionStatus.Pending,
                RequestDate = DateTime.UtcNow,
            };
            var adoptionRequest3 = new AdoptionRequest
            {
                Id = 3,
                PetId = 2,
                UserId = 2,
                Status = AdoptionStatus.Pending,
                RequestDate = DateTime.UtcNow,
            };

            _mockRepository.Setup(r => r.GetAdoptionRequestByUserAndPetAsync(1,1)).ReturnsAsync(adoptionRequest);

            var result = await _service.GetAdoptionRequestByUserAndPetAsync(1,1);

            Assert.NotNull(result);
            Assert.Equal(adoptionRequest.PetId, result.PetId);
            Assert.Equal(adoptionRequest.UserId, result.UserId);
        }

    }
}
