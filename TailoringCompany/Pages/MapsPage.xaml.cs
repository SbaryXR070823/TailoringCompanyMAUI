using Mapsui;
using Mapsui.Layers;
using Mapsui.Projections;
using Mapsui.Styles;
using Mapsui.Tiling;
using Mapsui.UI.Maui;
using TailoringCompany.ViewModels;
using Map = Mapsui.Map;

namespace TailoringCompany.Pages;

public partial class MapsPage : ContentPage
{
    private readonly MapsPageViewModel _viewModel;
    private WritableLayer _pinLayer;

    public MapsPage(MapsPageViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = viewModel;

        var map = new Map();
        map.Layers.Add(OpenStreetMap.CreateTileLayer());

        _pinLayer = new WritableLayer { Name = "Pins" };
        map.Layers.Add(_pinLayer);

        mapControl.Map = map;
        mapControl.Map.Navigator.CenterOn(new MPoint(0, 0));
        mapControl.Map.Navigator.ZoomTo(10000);

        var tapGesture = new TapGestureRecognizer();
        tapGesture.Tapped += MapControl_Tapped;
        mapControl.GestureRecognizers.Add(tapGesture);

        _viewModel.PropertyChanged += (s, e) =>
        {
            if (e.PropertyName == nameof(_viewModel.Pins))
            {
                UpdatePins();
            }
        };
        _viewModel.Pins.CollectionChanged += (s, e) => UpdatePins();
        UpdatePins(); 
    }

    private void MapControl_Tapped(object sender, EventArgs e)
    {
        if (sender is MapControl mapControl)
        {
            var tapEventArgs = e as TappedEventArgs;
            if (tapEventArgs != null)
            {
                var screenPosition = tapEventArgs.GetPosition(mapControl);
                if (screenPosition.HasValue)
                {
                    var mapInfo = mapControl.GetMapInfo(
                        new Mapsui.Manipulations.ScreenPosition(screenPosition.Value.X, screenPosition.Value.Y),
                        mapControl.Map.Layers
                    );
                    if (mapInfo?.WorldPosition != null)
                    {
                        (double longitude, double latitude) = SphericalMercator.ToLonLat(mapInfo.WorldPosition.X, mapInfo.WorldPosition.Y);
                        _viewModel.Pins.Add(new PinData
                        {
                            Latitude = latitude,
                            Longitude = longitude,
                            Label = $"Tapped at {latitude:F2}, {longitude:F2}"
                        });
                    }
                }
            }
        }
    }
    private void UpdatePins()
    {
        _pinLayer.Clear();
        Console.WriteLine($"UpdatePins called. Pin count: {_viewModel.Pins.Count}");
        foreach (var pin in _viewModel.Pins)
        {
            var point = SphericalMercator.FromLonLat(pin.Longitude, pin.Latitude);
            var feature = new PointFeature(point)
            {
                Styles = new[] { CreatePinStyle() }
            };
            feature["Label"] = pin.Label;
            _pinLayer.Add(feature);
            Console.WriteLine($"Added to layer: {pin.Label}");
        }
        _pinLayer.DataHasChanged();
        mapControl.Refresh(); // Force map redraw
        Console.WriteLine("Layer updated and map refreshed.");
    }

    private IStyle CreatePinStyle()
    {
        return new SymbolStyle
        {
            SymbolScale = 0.5,
            SymbolType = SymbolType.Ellipse,
            Fill = new Mapsui.Styles.Brush { Color = Mapsui.Styles.Color.Red },
            Outline = new Mapsui.Styles.Pen { Color = Mapsui.Styles.Color.Black, Width = 2 }
        };
    }
}