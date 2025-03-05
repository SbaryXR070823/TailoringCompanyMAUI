using BackendServices.IServices;
using Microsoft.Maui.Devices;
using Shared.BackendModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace BackendServices.Services
{
    public class MaterialsService : IMaterialsService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl = $"{Shared.Constants.BackendUrl}/materials";

        public MaterialsService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            if (DeviceInfo.Platform == DevicePlatform.Android)
            {
                _apiUrl = "http://10.0.2.2:8000";
            }
            else
            {
                _apiUrl = $"{Shared.Constants.BackendUrl}";
            }

            Console.WriteLine($"API URL set to: {_apiUrl}");
        }

        public async Task<List<MaterialInterface>> GetMaterialsAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<MaterialInterface>>(_apiUrl);
        }

        public async Task<MaterialInterface> CreateMaterialAsync(MaterialInterface material)
        {
            var response = await _httpClient.PostAsJsonAsync(_apiUrl, material);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<MaterialInterface>();
        }

        public async Task<MaterialInterface> GetMaterialAsync(string materialId)
        {
            return await _httpClient.GetFromJsonAsync<MaterialInterface>($"{_apiUrl}/{materialId}");
        }

        public async Task<MaterialInterface> UpdateMaterialAsync(string materialId, MaterialInterface material)
        {
            var response = await _httpClient.PutAsJsonAsync($"{_apiUrl}/{materialId}", material);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<MaterialInterface>();
        }

        public async Task DeleteMaterialAsync(string materialId)
        {
            var response = await _httpClient.DeleteAsync($"{_apiUrl}/{materialId}");
            response.EnsureSuccessStatusCode();
        }
    }
}
