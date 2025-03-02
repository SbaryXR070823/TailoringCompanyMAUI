using Shared.BackendModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendServices.IServices
{
    public interface IOrdersService
    {
        Task<List<OrderInterface>> GetOrderForUserAsync(string userEmail);
        Task DeleteOrderAsync(string orderId);
        Task<OrderInterface> UpdateOrderAsync(string orderId, OrderInterface order);
        Task<OrderInterface> GetOrderAsync(string orderId);
        Task<OrderInterface> CreateOrderAsync(OrderInterface order);
        Task<List<OrderInterface>> GetOrdersAsync();
    }
}
