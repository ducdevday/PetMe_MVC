using PetMe.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PetMe.Business.Services
{
    public interface IVnAddressService
    {
        Task<List<Province>> GetProvincesAsync();
        Task<List<District>> GetDisTrictsAsync(int provinceId);
    }

    public class VnAddressService : IVnAddressService
    {
        private readonly HttpClient _httpClient;

        public VnAddressService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Province>> GetProvincesAsync()
        {
            var response = await _httpClient.GetAsync("https://vapi.vnappmob.com/api/v2/province/");
            List<Province> provinces = [];
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                var jsonDocument = JsonDocument.Parse(content);
                var results = jsonDocument.RootElement.GetProperty("results").ToString();

                provinces = JsonSerializer.Deserialize<List<Province>>(results) ?? [];
            }
            return provinces;
        }
        
        public async Task<List<District>> GetDisTrictsAsync(int provinceId)
        {
            var response = await _httpClient.GetAsync($"https://vapi.vnappmob.com/api/v2/province/district/{provinceId}");
            List<District> districts = [];
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                var jsonDocument = JsonDocument.Parse(content);
                var results = jsonDocument.RootElement.GetProperty("results").ToString();

                districts = JsonSerializer.Deserialize<List<District>>(results) ?? [];
            }
            return districts;
        }


    }
}
