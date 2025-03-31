using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mapsui;
using Mapsui.Projections;
using Mapsui.UI.Maui;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Mapsui.Providers; 
using Mapsui.Nts;
using System.Windows.Input;
using TailoringCompany.Services;

namespace TailoringCompany.ViewModels
{
    public partial class MapsPageViewModel : ObservableObject
    {
        private readonly INavigationService _navigationService;
        private readonly IPinStorageService _pinStorageService;
        public ICommand BackCommand { get; }

        private ICommand _addPinCommand;
        public ICommand AddPinCommand
        {
            get => _addPinCommand;
            set => SetProperty(ref _addPinCommand, value);
        }

        [ObservableProperty]
        private ObservableCollection<PinData> _pins = new ObservableCollection<PinData>();

        [ObservableProperty]
        private PinData _selectedPin;

        [ObservableProperty]
        private bool _isAddingPin;

        [ObservableProperty]
        private bool _isEditingPin;

        [ObservableProperty]
        private bool _showPinOverlay;

        [ObservableProperty]
        private double _currentLatitude;

        [ObservableProperty]
        private double _currentLongitude;

        public MapsPageViewModel(INavigationService navigationService, IPinStorageService pinStorageService)
        {
            _navigationService = navigationService;
            _pinStorageService = pinStorageService;
            BackCommand = new Command(async () => await NavigateBackAsync());
            LoadPinsAsync();
        }

        private async void LoadPinsAsync()
        {
            try
            {
                var savedPins = await _pinStorageService.GetPinsAsync();
                Pins.Clear();
                foreach (var pin in savedPins)
                {
                    Pins.Add(pin);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading pins: {ex.Message}");
            }
        }

        public void SetAddPinAction(Action<PinData> addPinAction)
        {
            AddPinCommand = new Command(() => 
            {
                IsAddingPin = true;
                ShowPinOverlay = true;
                SelectedPin = new PinData
                {
                    Latitude = CurrentLatitude,
                    Longitude = CurrentLongitude
                };
            });
        }

        [RelayCommand]
        private async Task SavePin()
        {
            if (SelectedPin == null) return;
            if (IsAddingPin)
            {
                SelectedPin.CreatedAt = DateTime.Now;
            }
            SelectedPin.UpdatedAt = DateTime.Now;
            if (string.IsNullOrEmpty(SelectedPin.Name))
            {
                if (string.IsNullOrEmpty(SelectedPin.Label))
                {
                    SelectedPin.Label = $"Pin at {DateTime.Now:g}";
                }
                SelectedPin.Name = SelectedPin.Label;
            }
            else if (string.IsNullOrEmpty(SelectedPin.Label))
            {
                SelectedPin.Label = SelectedPin.Name;
            }
            await _pinStorageService.SavePinAsync(SelectedPin);
            if (IsAddingPin && !Pins.Contains(SelectedPin))
            {
                Pins.Add(SelectedPin);
            }
            else
            {
                int index = Pins.IndexOf(SelectedPin);
                if (index >= 0)
                {
                    Pins.Remove(SelectedPin);
                    Pins.Add(SelectedPin);
                }
            }
            IsAddingPin = false;
            IsEditingPin = false;
            ShowPinOverlay = false;
            SelectedPin = null;
        }

        [RelayCommand]
        private async Task CancelPinEdit()
        {
            IsAddingPin = false;
            IsEditingPin = false;
            ShowPinOverlay = false;
            SelectedPin = null;
        }

        [RelayCommand]
        public void SelectPin(PinData pin)
        {
            if (pin != null)
            {
                SelectedPin = pin;
                IsEditingPin = true;
                ShowPinOverlay = true;
            }
        }

        [RelayCommand]
        public async Task DeleteCurrentPin()
        {
            if (SelectedPin != null)
            {
                await _pinStorageService.DeletePinAsync(SelectedPin.Id);
                Pins.Remove(SelectedPin);
                
                IsAddingPin = false;
                IsEditingPin = false;
                ShowPinOverlay = false;
                SelectedPin = null;
            }
        }

        [RelayCommand]
        public void ShowAddPinOverlay()
        {
            IsAddingPin = true;
            IsEditingPin = false;
            ShowPinOverlay = true;
            SelectedPin = new PinData
            {
                Latitude = CurrentLatitude,
                Longitude = CurrentLongitude,
                Label = $"New Pin"
            };
        }

        public void UpdateCurrentCoordinates(MapControl mapControl)
        {
            try
            {
                if (mapControl?.Map?.Navigator?.Viewport != null)
                {
                    var viewport = mapControl.Map.Navigator.Viewport;
                    
                    var centerX = viewport.CenterX;
                    var centerY = viewport.CenterY;
                    var sphericalMercatorCoordinate = new MPoint(centerX, centerY);
                    
                    var wgs84Coordinate = SphericalMercator.ToLonLat(sphericalMercatorCoordinate);
                    
                    CurrentLongitude = wgs84Coordinate.X;
                    CurrentLatitude = wgs84Coordinate.Y;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error updating coordinates: {ex.Message}");
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
        private string _id = Guid.NewGuid().ToString();

        [ObservableProperty]
        private double _latitude;

        [ObservableProperty]
        private double _longitude;

        [ObservableProperty]
        private string _label;

        [ObservableProperty]
        private string _name;

        [ObservableProperty]
        private string _description;

        [ObservableProperty]
        private DateTime _createdAt = DateTime.Now;

        [ObservableProperty]
        private DateTime _updatedAt = DateTime.Now;

        public override string ToString()
        {
            return $"{Name ?? Label} ({Latitude:F4}, {Longitude:F4})";
        }
    }
}