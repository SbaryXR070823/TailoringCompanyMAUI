using BackendServices.IServices;
using BackendServices.Services;
using Microsoft.Maui.Platform;
using Shared.BackendModels;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using TailoringCompany.Helpers;
using TailoringCompany.Services;
using TailoringCompany.ViewModels;

namespace TailoringCompany.ViewModels;

public class LandingPageViewModel : BaseViewModel
{
    private readonly IOrdersService _ordersService;
    private readonly INavigationService _navigationService;
    public ICommand ShowDescriptionCommand { get; }
    private ObservableCollection<OrderInterface> _orders;

    public ObservableCollection<OrderInterface> Orders
    {
        get => _orders;
        set
        {
            _orders = value;
            OnPropertyChanged();
        }
    }

    public LandingPageViewModel(IOrdersService ordersService, INavigationService navigationService)
    {
        _ordersService = ordersService;
        _navigationService = navigationService;
        ShowDescriptionCommand = new Command<string>(ShowDescription);
        LoadOrders();
    }

    private async void ShowDescription(string description)
    {
        await Application.Current.MainPage.DisplayAlert("Order Description", description, "Close");
    }
    
    public async Task NavigateToMapsPage()
    {
        await _navigationService.NavigateToAsync("MapsPage");
    }
    private async void LoadOrders()
    {
        var user = UserInfoHelper.GetUserInfoFromPreferences();
        var orders = await _ordersService.GetOrderForUserAsync(user.Email);
        Orders = new ObservableCollection<OrderInterface>(orders);
    }
}

