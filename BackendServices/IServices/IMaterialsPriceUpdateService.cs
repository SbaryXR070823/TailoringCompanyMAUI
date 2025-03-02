using Shared.BackendModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendServices.IServices
{
    public interface IMaterialsPriceUpdateService
    {
        Task<List<MaterialsHistoryInterface>> GetMaterialPriceUpdatesByMaterialIdAsync(string materialId);
        Task DeleteMaterialsPriceUpdateAsync(string updateId);
        Task<MaterialsHistoryInterface> UpdateMaterialsPriceUpdateAsync(string updateId, MaterialsHistoryInterface priceUpdate);
        Task<MaterialsHistoryInterface> GetMaterialsPriceUpdateAsync(string updateId);
        Task<MaterialsHistoryInterface> CreateMaterialsPriceUpdateAsync(MaterialsHistoryInterface priceUpdate);
        Task<List<MaterialsHistoryInterface>> GetMaterialsPriceUpdatesAsync();
    }
}
