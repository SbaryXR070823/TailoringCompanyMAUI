using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Shared.BackendModels;
using BackendServices.Services;
using TailoringCompany.ViewModels;
using BackendServices.IServices;
using TailoringCompany.Helpers;

namespace TailoringCompany.ViewModels;

public class LandingPageViewModel : BaseViewModel
{
    private readonly IOrdersService _ordersService;
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

    public LandingPageViewModel(IOrdersService ordersService)
    {
        _ordersService = ordersService;
        LoadOrders();
    }

    private async void LoadOrders()
    {
        var user = UserInfoHelper.GetUserInfoFromPreferences();
        var orders = await _ordersService.GetOrderForUserAsync(user.Email);
        Orders = new ObservableCollection<OrderInterface>(orders);
    }
}

