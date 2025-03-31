using Mapsui;
using Mapsui.Layers;
using Mapsui.Tiling;
using TailoringCompany.ViewModels;
using System.Diagnostics;
using System.Collections.Specialized;
using Mapsui.Projections;
using Mapsui.Styles;
using Color = Mapsui.Styles.Color;
using Font = Mapsui.Styles.Font;

namespace TailoringCompany.Pages;

public partial class MapsPage : ContentPage
{
    private readonly MapsPageViewModel _viewModel;
    private MemoryLayer _pinLayer;
    private bool _isInitialized = false;

    public MapsPage(MapsPageViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (!_isInitialized)
        {
            InitializeMap();
            _isInitialized = true;
        }

        _viewModel.Pins.CollectionChanged += OnPinsCollectionChanged;

        UpdatePins();

        _viewModel.SetAddPinAction(AddPinAtCrosshair);
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _viewModel.Pins.CollectionChanged -= OnPinsCollectionChanged;
        if (mapControl?.Map?.Navigator != null)
        {
            mapControl.Map.Navigator.ViewportChanged -= MapControl_ViewportChanged;
        }
    }

    private void OnPinsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        UpdatePins();
    }

    private void InitializeMap()
    {
        try
        {
            var map = new Mapsui.Map();

            map.Layers.Add(OpenStreetMap.CreateTileLayer());

            _pinLayer = new MemoryLayer { Name = "PinLayer" };
            map.Layers.Add(_pinLayer);

            mapControl.Map = map;

            map.Home = n =>
            {
                n.CenterOn(new Mapsui.MPoint(0, 0));
                n.ZoomTo(5);
            };
            map.Navigator.CenterOn(new Mapsui.MPoint(0, 0));
            map.Navigator.ZoomTo(5);

            mapControl.Info += MapControl_Info;

            mapControl.Map.Navigator.ViewportChanged += MapControl_ViewportChanged;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error initializing map: {ex}");
        }
    }

    private void MapControl_ViewportChanged(object sender, EventArgs e)
    {
        try
        {
            if (mapControl?.Map?.Navigator != null)
            {
                var center = mapControl.Map.Navigator.Viewport.CenterX != 0 && mapControl.Map.Navigator.Viewport.CenterY != 0
                    ? SphericalMercator.ToLonLat(
                        mapControl.Map.Navigator.Viewport.CenterX,
                        mapControl.Map.Navigator.Viewport.CenterY)
                    : (0, 0);

                _viewModel.CurrentLongitude = center.Item1;
                _viewModel.CurrentLatitude = center.Item2;
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error updating coordinates: {ex}");
        }
    }

    private void MapControl_Info(object sender, MapInfoEventArgs e)
    {
        try
        {
            if (e.MapInfo?.Feature == null) return;

            if (e.MapInfo.Feature is PointFeature pointFeature)
            {
                if (e.MapInfo.Feature["PinId"] is string pinId)
                {
                    var pin = _viewModel.Pins.FirstOrDefault(p => p.Id == pinId);
                    if (pin != null)
                    {
                        MainThread.BeginInvokeOnMainThread(() => {
                            _viewModel.SelectPin(pin);
                        });
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error handling map info event: {ex}");
        }
    }

    private void AddPinAtCrosshair(PinData pin)
    {
        try
        {
            if (mapControl?.Map?.Navigator?.Viewport == null) return;

            var viewport = mapControl.Map.Navigator.Viewport;
            var sphericalMercatorCoordinate = new MPoint(viewport.CenterX, viewport.CenterY);

            var wgs84Coordinate = SphericalMercator.ToLonLat(sphericalMercatorCoordinate);

            pin.Longitude = wgs84Coordinate.X;
            pin.Latitude = wgs84Coordinate.Y;

            if (!_viewModel.Pins.Contains(pin))
            {
                _viewModel.Pins.Add(pin);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error adding pin: {ex}");
        }
    }

    private void UpdatePins()
    {
        try
        {
            if (_pinLayer == null || mapControl?.Map == null) return;

            var features = new List<Mapsui.IFeature>();

            foreach (var pin in _viewModel.Pins)
            {
                try
                {
                    var lonlat = new MPoint(pin.Longitude, pin.Latitude);
                    var sphericalMercator = SphericalMercator.FromLonLat(lonlat.X, lonlat.Y);

                    var pointGeometry = new MPoint(sphericalMercator.x, sphericalMercator.y);

                    var feature = new PointFeature(sphericalMercator.x, sphericalMercator.y);

                    feature["PinId"] = pin.Id;
                    feature["Name"] = string.IsNullOrEmpty(pin.Name) ? pin.Label : pin.Name;

                    var symbolStyle = new SymbolStyle
                    {
                        SymbolScale = 0.8,
                        Fill = new Mapsui.Styles.Brush(Color.Red),
                        Outline = new Pen(Color.Black, 2)
                    };
                    var labelStyle = new LabelStyle
                    {
                        Text = string.IsNullOrEmpty(pin.Name) ? pin.Label : pin.Name,
                        Font = new Font { Size = 14 },
                        BackColor = new Mapsui.Styles.Brush(Color.White),
                        ForeColor = Color.Black,
                        HorizontalAlignment = LabelStyle.HorizontalAlignmentEnum.Center
                    };

                    feature.Styles = new List<IStyle> { symbolStyle, labelStyle };

                    features.Add(feature);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error creating pin feature: {ex}");
                }
            }

            _pinLayer.Features = features;

            mapControl.Refresh();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error updating pins: {ex}");
        }
    }
}

