using Mapsui;
using Mapsui.Tiling;
using Mapsui.Tiling.Layers;
using TailoringCompany.ViewModels;
using Map = Mapsui.Map;

namespace TailoringCompany.Pages;

public partial class MapsPage : ContentPage
{
    public MapsPage(MapsPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;

        var map = new Map();

        var osmLayer = OpenStreetMap.CreateTileLayer();
        map.Layers.Add(osmLayer);

        mapControl.Map = map;

        mapControl.Map.Navigator.CenterOn(new MPoint(0, 0));

        mapControl.Map.Navigator.ZoomTo(10000); 
    }
}