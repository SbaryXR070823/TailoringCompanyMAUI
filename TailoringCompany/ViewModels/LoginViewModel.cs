using Authentication.IServices;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using TailoringCompany.Services;
using System.Collections;
using BackendServices.IServices;

namespace TailoringCompany.ViewModels;

public class LoginViewModel : BaseViewModel, INotifyDataErrorInfo
{
    private readonly IAuthService _authService;
    private readonly INavigationService _navigationService;
    private readonly IUserService _userService;

    private string _email;
    private string _password;
    private readonly Dictionary<string, List<string>> _errors = new();
    public string EmailError => _errors.ContainsKey(nameof(Email)) ? string.Join("\n", _errors[nameof(Email)]) : string.Empty;
    public bool HasEmailError => _errors.ContainsKey(nameof(Email));

    public string PasswordError => _errors.ContainsKey(nameof(Password)) ? string.Join("\n", _errors[nameof(Password)]) : string.Empty;
    public bool HasPasswordError => _errors.ContainsKey(nameof(Password));
    public string Email
    {
        get => _email;
        set
        {
            if (SetProperty(ref _email, value))
            {
                ValidateEmail(value);
            }
        }
    }

    public string Password
    {
        get => _password;
        set
        {
            if (SetProperty(ref _password, value))
            {
                ValidatePassword(value);
            }
        }
    }

    public ICommand LoginCommand { get; }
    public ICommand ForgotPasswordCommand { get; }
    public ICommand ClearEmailCommand { get; }
    public ICommand RegisterCommand { get; }
    public ICommand ClearPasswordCommand { get; }
    public ICommand BackCommand { get; }

    public bool HasErrors => _errors.Any();

    public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

    public LoginViewModel(IAuthService authService, INavigationService navigationService, IUserService userService)
    {
        _authService = authService;
        _navigationService = navigationService;
        _userService = userService;

        LoginCommand = new Command(async () => await ExecuteLoginCommand(),
            () => CanExecuteLogin());
        RegisterCommand = new Command(async () => await NavigateToRegister());
        ForgotPasswordCommand = new Command(async () => await ExecuteForgotPasswordCommand());
        ClearEmailCommand = new Command(() => Email = string.Empty);
        ClearPasswordCommand = new Command(() => Password = string.Empty);
        BackCommand = new Command(async () => await GoBack());
        PropertyChanged += (_, __) => ((Command)LoginCommand).ChangeCanExecute();
    }

    private async Task GoBack()
    {
        await _navigationService.NavigateToAsync("///MainPage");
    }

    private async Task NavigateToRegister()
    {
        await _navigationService.NavigateToAsync("RegisterPage");
    }

    private void ValidateEmail(string email)
    {
        var errorList = new List<string>();

        if (string.IsNullOrWhiteSpace(email))
        {
            errorList.Add("Email is required.");
        }
        else if (!IsValidEmail(email))
        {
            errorList.Add("Please enter a valid email address.");
        }

        if (errorList.Count > 0)
        {
            _errors[nameof(Email)] = errorList;
        }
        else
        {
            _errors.Remove(nameof(Email)); 
        }

        RaiseErrorsChanged(nameof(Email));
    }

    private void ValidatePassword(string password)
    {
        var errorList = new List<string>();

        if (string.IsNullOrWhiteSpace(password))
        {
            errorList.Add("Password is required.");
        }
        else if (password.Length < 6)
        {
            errorList.Add("Password must be at least 6 characters long.");
        }

        if (errorList.Count > 0)
        {
            _errors[nameof(Password)] = errorList;
        }
        else
        {
            _errors.Remove(nameof(Password));
        }

        RaiseErrorsChanged(nameof(Password));
    }


    private bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    private async Task ExecuteLoginCommand()
    {
        ValidateEmail(Email);
        ValidatePassword(Password);

        if (HasErrors)
        {
            return;
        }

        try
        {
            IsBusy = true;
            var user = await _authService.LoginAsync(Email, Password);
            var role = await _userService.GetUserRoleAsync(Email);
            await _navigationService.NavigateToAsync("HomePage");
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Error",
                $"Login failed: {ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task ExecuteForgotPasswordCommand()
    {
        await Application.Current.MainPage.DisplayAlert("Info",
            "Forgot password feature coming soon!", "OK");
    }

    private bool CanExecuteLogin()
    {
        return !string.IsNullOrWhiteSpace(Email) &&
               !string.IsNullOrWhiteSpace(Password) &&
               !IsBusy &&
               !HasErrors;
    }

    public IEnumerable GetErrors(string propertyName)
    {
        if (string.IsNullOrEmpty(propertyName) || !_errors.ContainsKey(propertyName))
            return null;
        return _errors[propertyName];
    }

    private void RaiseErrorsChanged(string propertyName)
    {
        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        OnPropertyChanged(nameof(HasErrors));
        OnPropertyChanged(nameof(EmailError));
        OnPropertyChanged(nameof(HasEmailError));
        OnPropertyChanged(nameof(PasswordError));
        OnPropertyChanged(nameof(HasPasswordError));
        ((Command)LoginCommand).ChangeCanExecute();
    }
}