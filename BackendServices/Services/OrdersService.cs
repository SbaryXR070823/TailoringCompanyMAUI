using BackendServices.IServices;
using Shared.BackendModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace BackendServices.Services
{
    public class OrdersService : IOrdersService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl = $"{Shared.Constants.BackendUrl}/orders";

        public OrdersService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<OrderInterface>> GetOrdersAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<OrderInterface>>(_apiUrl);
        }

        public async Task<OrderInterface> CreateOrderAsync(OrderInterface order)
        {
            var response = await _httpClient.PostAsJsonAsync(_apiUrl, order);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<OrderInterface>();
        }

        public async Task<OrderInterface> GetOrderAsync(string orderId)
        {
            return await _httpClient.GetFromJsonAsync<OrderInterface>($"{_apiUrl}/{orderId}");
        }

        public async Task<OrderInterface> UpdateOrderAsync(string orderId, OrderInterface order)
        {
            var response = await _httpClient.PutAsJsonAsync($"{_apiUrl}/{orderId}", order);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<OrderInterface>();
        }

        public async Task DeleteOrderAsync(string orderId)
        {
            var response = await _httpClient.DeleteAsync($"{_apiUrl}/{orderId}");
            response.EnsureSuccessStatusCode();
        }

        public async Task<List<OrderInterface>> GetOrderForUserAsync(string userEmail)
        {
            return await _httpClient.GetFromJsonAsync<List<OrderInterface>>($"{_apiUrl}/by_user/{userEmail}");
        }
    }

}
