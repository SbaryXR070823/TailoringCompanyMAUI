using Shared.BackendModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendServices.IServices
{
    public interface IProductsService
    {
        Task DeleteProductAsync(string productId);
        Task<ProductInterface> UpdateProductAsync(string productId, ProductInterface product);
        Task<ProductInterface> GetProductAsync(string productId);
        Task<ProductInterface> CreateProductAsync(ProductInterface product);
        Task<List<ProductInterface>> GetProductsAsync();
    }
}
