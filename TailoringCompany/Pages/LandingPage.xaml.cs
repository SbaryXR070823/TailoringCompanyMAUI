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

    private async void OnLogoutClicked(object sender, EventArgs e)
    {
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