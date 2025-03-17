using System.ComponentModel;
using System.Runtime.CompilerServices;
using TailoringCompany.Helpers;
using TailoringCompany.ViewModels;

namespace TailoringCompany.Pages;

public partial class LandingPage : ContentPage
{
    public LandingPage(LandingPageViewModel landingPageViewModel)
	{
        InitializeComponent();
        BindingContext = landingPageViewModel;
        var user = UserInfoHelper.GetUserInfoFromPreferences();
    }

    private void OnMenuButtonClicked(object sender, EventArgs e)
    {
        FlyoutMenu.IsVisible = true;
    }

    private void OnBackgroundTapped(object sender, EventArgs e)
    {
        FlyoutMenu.IsVisible = false;
    }

    private void OnMapsClicked(object sender, EventArgs e)
    {
        FlyoutMenu.IsVisible = false;

        if (BindingContext is LandingPageViewModel viewModel)
        {
            viewModel.NavigateToMapsPage();
        }
    }


    private async void OnLogoutClicked(object sender, EventArgs e)
    {
        FlyoutMenu.IsVisible = false;
        bool confirmed = await DisplayAlert(
            "Logout",
            "Are you sure you want to logout?",
            "Yes",
            "No");
        if (confirmed)
        {
            UserInfoHelper.RemoveUserInfoFromPreferences();
            await Shell.Current.GoToAsync("///MainPage");
        }
    }
}