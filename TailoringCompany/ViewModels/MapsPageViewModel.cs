using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows.Input;
using TailoringCompany.Services;
namespace TailoringCompany.ViewModels
{
    public partial class MapsPageViewModel : ObservableObject
    {
        private readonly INavigationService _navigationService;
        public ICommand BackCommand { get; }

        public ICommand AddPinCommand { get; set; }

        [ObservableProperty]
        private ObservableCollection<PinData> _pins = new ObservableCollection<PinData>();

        public MapsPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            BackCommand = new Command(async () => await NavigateBack());
            AddPinCommand = new Command(AddPin);
        }

        public void AddPin()
        {
            var newPin = new PinData
            {
                Latitude = 40.7128,
                Longitude = -74.0060,
                Label = $"Pin at {DateTime.Now:T}"
            };
            Pins.Add(newPin);
            Console.WriteLine($"AddPin executed. Pin count: {Pins.Count}");
        }

        [RelayCommand]
        public void DeletePin(PinData pin)
        {
            if (pin == null && Pins.Any())
            {
                pin = Pins[0];
            }
            if (pin != null && Pins.Contains(pin))
            {
                Pins.Remove(pin);
                Console.WriteLine($"DeletePin executed. Removed: {pin.Label}. Pin count: {Pins.Count}");
            }
            else
            {
                Console.WriteLine("DeletePin: No pins to delete.");
            }
        }

        [RelayCommand]
        public void EditPin(PinData pin)
        {
            if (pin == null && Pins.Any())
            {
                pin = Pins[0];
            }
            if (pin != null && Pins.Contains(pin))
            {
                pin.Label = $"Edited at {DateTime.Now:T}";
                Console.WriteLine($"EditPin executed. New label: {pin.Label}");
            }
            else
            {
                Console.WriteLine("EditPin: No pins to edit.");
            }
        }

        private async Task NavigateBack()
        {
            await _navigationService.NavigateToAsync("/LandingPage");
        }
    }

    public partial class PinData : ObservableObject
    {
        [ObservableProperty]
        private double _latitude;
        [ObservableProperty]
        private double _longitude;
        [ObservableProperty]
        private string _label;
    }
}