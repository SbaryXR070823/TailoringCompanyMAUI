using Mapsui;
using Mapsui.Layers;
using Mapsui.Projections;
using Mapsui.Styles;
using Mapsui.Tiling;
using Mapsui.UI.Maui;
using TailoringCompany.ViewModels;
using Map = Mapsui.Map;
using Microsoft.Maui.Controls;
using System;
using System.Diagnostics;

namespace TailoringCompany.Pages;

public partial class MapsPage : ContentPage
{
    private readonly MapsPageViewModel _viewModel;
    private WritableLayer _pinLayer;
    private bool _isInitialized = false;

    public MapsPage(MapsPageViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = viewModel;
        _viewModel.SetAddPinAction(AddPinAtCrosshair);
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (!_isInitialized)
        {
            InitializeMap();
            _isInitialized = true;
        }
    }

    private void InitializeMap()
    {
        try
        {
            var map = new Map();

            map.Layers.Add(OpenStreetMap.CreateTileLayer());

            _pinLayer = new WritableLayer { Name = "Pins" };
            map.Layers.Add(_pinLayer);

            mapControl.Map = map;

            mapControl.Map.Navigator.CenterOn(new MPoint(0, 0));
            mapControl.Map.Navigator.ZoomTo(10000);

            _viewModel.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(_viewModel.Pins))
                {
                    UpdatePins();
                }
            };

            _viewModel.Pins.CollectionChanged += (s, e) => UpdatePins();

            UpdatePins();

            Debug.WriteLine("Map initialized successfully");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error initializing map: {ex.Message}");
            DisplayAlert("Map Error", $"Failed to initialize map: {ex.Message}", "OK");
        }
    }

    private void AddPinAtCrosshair(PinData pin)
    {
        try
        {
            if (mapControl?.Map?.Navigator == null)
            {
                Debug.WriteLine("Map not fully initialized");
                return;
            }

            (double longitude, double latitude) = SphericalMercator.ToLonLat(mapControl.Map.Navigator.Viewport.CenterX, mapControl.Map.Navigator.Viewport.CenterY);

            pin.Latitude = latitude;
            pin.Longitude = longitude;
            pin.Label = $"Pin at {latitude:F4}, {longitude:F4}";

            _viewModel.Pins.Add(pin);

            Debug.WriteLine($"Added pin at crosshair: {latitude:F4}, {longitude:F4}");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error adding pin: {ex.Message}");
        }
    }

    private void UpdatePins()
    {
        try
        {
            if (_pinLayer == null || mapControl?.Map == null)
            {
                Debug.WriteLine("Cannot update pins: Map or pin layer not initialized");
                return;
            }

            _pinLayer.Clear();
            Debug.WriteLine($"UpdatePins called. Pin count: {_viewModel.Pins.Count}");

            foreach (var pin in _viewModel.Pins)
            {
                var point = SphericalMercator.FromLonLat(pin.Longitude, pin.Latitude);
                var feature = new PointFeature(point)
                {
                    Styles = new[] { CreatePinStyle() }
                };
                feature["Label"] = pin.Label;
                _pinLayer.Add(feature);
                Debug.WriteLine($"Added to layer: {pin.Label}");
            }

            _pinLayer.DataHasChanged();
            mapControl.Refresh(); 
            Debug.WriteLine("Layer updated and map refreshed.");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error updating pins: {ex.Message}");
        }
    }

    private IStyle CreatePinStyle()
    {
        return new SymbolStyle
        {
            SymbolScale = 0.7,
            SymbolType = SymbolType.Ellipse,
            Fill = new Mapsui.Styles.Brush { Color = Mapsui.Styles.Color.Red },
            Outline = new Mapsui.Styles.Pen { Color = Mapsui.Styles.Color.Black, Width = 2 }
        };
    }
}