using Authentication.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TailoringCompany.Helpers;
using TailoringCompany.Services;

namespace TailoringCompany.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly IAuthService _authService;
        private readonly INavigationService _navigationService;

        public ICommand LoginCommand { get; }
        public ICommand RegisterCommand { get; }

        public MainViewModel(IAuthService authService, INavigationService navigationService)
        {
            _authService = authService;
            _navigationService = navigationService;
            var user = UserInfoHelper.GetUserInfoFromPreferences();
            if (!string.IsNullOrEmpty(user.UserId))
            {
                _navigationService.NavigateToAsync("LandingPage");
            }
            LoginCommand = new Command(async () => await OnLoginClicked());
            RegisterCommand = new Command(async () => await OnRegisterClicked());
        }

        private async Task OnLoginClicked()
        {
            await _navigationService.NavigateToAsync("LoginPage");
        }

        private async Task OnRegisterClicked()
        {
            await _navigationService.NavigateToAsync("RegisterPage");
        }
    }
}
