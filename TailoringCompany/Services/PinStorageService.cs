using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TailoringCompany.ViewModels;

namespace TailoringCompany.Services;

public class PinStorageService : IPinStorageService
{
    private readonly List<PinData> _pins = new();

    public Task<IEnumerable<PinData>> GetPinsAsync()
    {
        return Task.FromResult(_pins.AsEnumerable());
    }

    public Task SavePinAsync(PinData pin)
    {
        var existingPin = _pins.FirstOrDefault(p => p.Id == pin.Id);
        if (existingPin != null)
        {
            _pins.Remove(existingPin);
        }
        _pins.Add(pin);
        return Task.CompletedTask;
    }

    public Task DeletePinAsync(string pinId)
    {
        var pin = _pins.FirstOrDefault(p => p.Id == pinId);
        if (pin != null)
        {
            _pins.Remove(pin);
        }
        return Task.CompletedTask;
    }
}