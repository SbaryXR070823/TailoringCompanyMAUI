using BackendServices.IServices;
using BackendServices.Services;
using Shared.BackendModels;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using TailoringCompany.Helpers;
using TailoringCompany.ViewModels;

namespace TailoringCompany.ViewModels;

public class LandingPageViewModel : BaseViewModel
{
    private readonly IOrdersService _ordersService;
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

    public LandingPageViewModel(IOrdersService ordersService)
    {
        _ordersService = ordersService;
        ShowDescriptionCommand = new Command<string>(ShowDescription);
        LoadOrders();
    }

    private async void ShowDescription(string description)
    {
        await Application.Current.MainPage.DisplayAlert("Order Description", description, "Close");
    }

    private async void LoadOrders()
    {
        var user = UserInfoHelper.GetUserInfoFromPreferences();
        var orders = await _ordersService.GetOrderForUserAsync(user.Email);
        Orders = new ObservableCollection<OrderInterface>(orders);
    }
}

