using BackendServices.IServices;
using Microsoft.VisualBasic;
using Shared.BackendModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace BackendServices.Services
{
    public class MaterialsPriceUpdateService : IMaterialsPriceUpdateService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl = $"{Shared.Constants.BackendUrl}/materials_price_updates";

        public MaterialsPriceUpdateService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<MaterialsHistoryInterface>> GetMaterialsPriceUpdatesAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<MaterialsHistoryInterface>>(_apiUrl);
        }

        public async Task<MaterialsHistoryInterface> CreateMaterialsPriceUpdateAsync(MaterialsHistoryInterface priceUpdate)
        {
            var response = await _httpClient.PostAsJsonAsync(_apiUrl, priceUpdate);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<MaterialsHistoryInterface>();
        }

        public async Task<MaterialsHistoryInterface> GetMaterialsPriceUpdateAsync(string updateId)
        {
            return await _httpClient.GetFromJsonAsync<MaterialsHistoryInterface>($"{_apiUrl}/{updateId}");
        }

        public async Task<MaterialsHistoryInterface> UpdateMaterialsPriceUpdateAsync(string updateId, MaterialsHistoryInterface priceUpdate)
        {
            var response = await _httpClient.PutAsJsonAsync($"{_apiUrl}/{updateId}", priceUpdate);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<MaterialsHistoryInterface>();
        }

        public async Task DeleteMaterialsPriceUpdateAsync(string updateId)
        {
            var response = await _httpClient.DeleteAsync($"{_apiUrl}/{updateId}");
            response.EnsureSuccessStatusCode();
        }

        public async Task<List<MaterialsHistoryInterface>> GetMaterialPriceUpdatesByMaterialIdAsync(string materialId)
        {
            return await _httpClient.GetFromJsonAsync<List<MaterialsHistoryInterface>>($"{_apiUrl}/materials_prices_updated_for_material_id/{materialId}");
        }
    }

}
