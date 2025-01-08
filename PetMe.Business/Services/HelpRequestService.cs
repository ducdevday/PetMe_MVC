﻿using PetMe.Core.Entities;
using PetMe.Core.Enums;
using PetMe.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetMe.Business.Services
{
    public interface IHelpRequestService
    {
        Task CreateHelpRequestAsync(HelpRequest helpRequest);
        Task<List<HelpRequest>> GetHelpRequestsAsync();
        Task<HelpRequest> GetHelpRequestByIdAsync(int id);
        Task UpdateHelpRequestAsync(HelpRequest helpRequest); // Yeni metot
        Task DeleteHelpRequestAsync(int id);
    }

    public class HelpRequestService : IHelpRequestService
    {
        private readonly IHelpRequestRepository _helpRequestRepository;

        public HelpRequestService(IHelpRequestRepository helpRequestRepository)
        {
            _helpRequestRepository = helpRequestRepository;
        }

        public async Task CreateHelpRequestAsync(HelpRequest helpRequest)
        {
            helpRequest.Status = HelpRequestStatus.Active;
            await _helpRequestRepository.CreateHelpRequestAsync(helpRequest);
        }

        public async Task<List<HelpRequest>> GetHelpRequestsAsync()
        {
            return await _helpRequestRepository.GetHelpRequestsAsync();
        }

        public async Task<HelpRequest> GetHelpRequestByIdAsync(int id)
        {
            var helpRequest = _helpRequestRepository.GetHelpRequestByIdAsync(id);
            if (helpRequest == null) { 
                throw new KeyNotFoundException();
            }
            return helpRequest;
        }

        public async Task<List<HelpRequest>> GetHelpRequestsByUserAsync(int userId)
        {
            return await _helpRequestRepository.GetHelpRequestsByUserAsync(userId);
        }

        public async Task UpdateHelpRequestAsync(HelpRequest helpRequest)
        {
            await _helpRequestRepository.UpdateHelpRequestAsync(helpRequest);
        }

        public async Task DeleteHelpRequestAsync(int id)
        {
            await _helpRequestRepository.DeleteHelpRequestAsync(id);
        }
    }
}
