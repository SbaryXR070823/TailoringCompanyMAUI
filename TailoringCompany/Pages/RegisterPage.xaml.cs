using TailoringCompany.ViewModels;
namespace TailoringCompany.Pages;
public partial class RegisterPage : ContentPage
{
    public RegisterPage(RegisterViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}