using Shared.BackendModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendServices.IServices
{
    public interface IMaterialsService
    {
        Task DeleteMaterialAsync(string materialId);
        Task<MaterialInterface> UpdateMaterialAsync(string materialId, MaterialInterface material);
        Task<MaterialInterface> GetMaterialAsync(string materialId);
        Task<MaterialInterface> CreateMaterialAsync(MaterialInterface material);
        Task<List<MaterialInterface>> GetMaterialsAsync();
    }
}
