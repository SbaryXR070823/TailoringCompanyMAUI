using System.Windows.Input;
using TailoringCompany.Services;
using TailoringCompany.ViewModels;

namespace TailoringCompany.ViewModels
{
    public class MapsPageViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;

        public ICommand BackCommand { get; }

        public MapsPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

            BackCommand = new Command(async () => await NavigateBack());
        }

        private async Task NavigateBack()
        {
            await _navigationService.NavigateToAsync("/LandingPage");
        }
    }
}