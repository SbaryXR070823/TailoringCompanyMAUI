using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using TailoringCompany.Services;

namespace TailoringCompany.ViewModels
{
    public partial class MapsPageViewModel : ObservableObject
    {
        private readonly INavigationService _navigationService;
        public ICommand BackCommand { get; }

        private ICommand _addPinCommand;
        public ICommand AddPinCommand
        {
            get => _addPinCommand;
            set => SetProperty(ref _addPinCommand, value);
        }

        [ObservableProperty]
        private ObservableCollection<PinData> _pins = new ObservableCollection<PinData>();

        public MapsPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            BackCommand = new Command(async () => await NavigateBackAsync());

            AddPinCommand = new Command(AddDefaultPin);
        }


        public void SetAddPinAction(Action<PinData> addPinAction)
        {
            AddPinCommand = new Command(() => addPinAction(new PinData()));
        }
        private void AddDefaultPin()
        {
            var newPin = new PinData
            {
                Latitude = 40.7128,
                Longitude = -74.0060,
                Label = $"Default pin at {DateTime.Now:T}"
            };

            Pins.Add(newPin);
            Debug.WriteLine($"Default pin added. Pin count: {Pins.Count}");
        }

        [RelayCommand]
        public void DeleteLastPin()
        {
            if (Pins.Any())
            {
                var pin = Pins.Last();
                Pins.Remove(pin);
                Debug.WriteLine($"DeleteLastPin executed. Removed: {pin.Label}. Pin count: {Pins.Count}");
            }
            else
            {
                Debug.WriteLine("DeleteLastPin: No pins to delete.");
            }
        }

        [RelayCommand]
        public void DeletePin(PinData pin)
        {
            try
            {
                if (pin != null && Pins.Contains(pin))
                {
                    Pins.Remove(pin);
                    Debug.WriteLine($"DeletePin executed. Removed: {pin.Label}. Pin count: {Pins.Count}");
                }
                else
                {
                    Debug.WriteLine("DeletePin: No pin specified or pin not found.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in DeletePin: {ex.Message}");
            }
        }

        [RelayCommand]
        public void EditLastPin()
        {
            if (Pins.Any())
            {
                var pin = Pins.Last();
                pin.Label = $"Edited at {DateTime.Now:T}";
                Debug.WriteLine($"EditLastPin executed. New label: {pin.Label}");

                OnPropertyChanged(nameof(Pins));
            }
            else
            {
                Debug.WriteLine("EditLastPin: No pins to edit.");
            }
        }

        [RelayCommand]
        public void EditPin(PinData pin)
        {
            try
            {
                if (pin != null && Pins.Contains(pin))
                {
                    pin.Label = $"Edited at {DateTime.Now:T}";
                    Debug.WriteLine($"EditPin executed. New label: {pin.Label}");

                    OnPropertyChanged(nameof(Pins));
                }
                else
                {
                    Debug.WriteLine("EditPin: No pin specified or pin not found.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in EditPin: {ex.Message}");
            }
        }


        private async Task NavigateBackAsync()
        {
            try
            {
                await _navigationService.NavigateToAsync("/LandingPage");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Navigation error: {ex.Message}");
            }
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

        [ObservableProperty]
        private DateTime _createdAt = DateTime.Now;

        public override string ToString()
        {
            return $"{Label} ({Latitude:F4}, {Longitude:F4})";
        }
    }
}