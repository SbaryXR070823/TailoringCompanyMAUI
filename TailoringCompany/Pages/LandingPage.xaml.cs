using TailoringCompany.ViewModels;

namespace TailoringCompany.Pages;

public partial class LandingPage : ContentPage
{
	public LandingPage(LandingPageViewModel landingPageViewModel)
	{
        InitializeComponent();
        BindingContext = landingPageViewModel;
    }
}