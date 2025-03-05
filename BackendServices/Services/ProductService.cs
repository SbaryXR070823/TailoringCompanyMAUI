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
    public class ProductsService : IProductsService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl = $"{Shared.Constants.BackendUrl}/products";

        public ProductsService(HttpClient httpClient)
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

        public async Task<List<ProductInterface>> GetProductsAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<ProductInterface>>(_apiUrl);
        }

        public async Task<ProductInterface> CreateProductAsync(ProductInterface product)
        {
            var response = await _httpClient.PostAsJsonAsync(_apiUrl, product);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ProductInterface>();
        }

        public async Task<ProductInterface> GetProductAsync(string productId)
        {
            return await _httpClient.GetFromJsonAsync<ProductInterface>($"{_apiUrl}/{productId}");
        }

        public async Task<ProductInterface> UpdateProductAsync(string productId, ProductInterface product)
        {
            var response = await _httpClient.PutAsJsonAsync($"{_apiUrl}/{productId}", product);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ProductInterface>();
        }

        public async Task DeleteProductAsync(string productId)
        {
            var response = await _httpClient.DeleteAsync($"{_apiUrl}/{productId}");
            response.EnsureSuccessStatusCode();
        }
    }
}
