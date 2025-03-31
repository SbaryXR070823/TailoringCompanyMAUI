using TailoringCompany.ViewModels;

namespace TailoringCompany.Services;

public interface IPinStorageService
{
    Task<IEnumerable<PinData>> GetPinsAsync();
    Task SavePinAsync(PinData pin);
    Task DeletePinAsync(string pinId);
}