using Authentication.IServices;
using System.ComponentModel;
using System.Windows.Input;
using TailoringCompany.Services;
using System.Collections;
using Shared.Models;
using BackendServices.IServices;
using Shared.Enums;

namespace TailoringCompany.ViewModels;

public class RegisterViewModel : BaseViewModel, INotifyDataErrorInfo
{
    private readonly IAuthService _authService;
    private readonly INavigationService _navigationService;
    private readonly IUserService _userService;

    private string _email;
    private string _password;
    private string _confirmPassword;
    private string _displayName;
    private readonly Dictionary<string, List<string>> _errors = new();

    public string EmailError => _errors.ContainsKey(nameof(Email)) ? string.Join("\n", _errors[nameof(Email)]) : string.Empty;
    public bool HasEmailError => _errors.ContainsKey(nameof(Email));

    public string PasswordError => _errors.ContainsKey(nameof(Password)) ? string.Join("\n", _errors[nameof(Password)]) : string.Empty;
    public bool HasPasswordError => _errors.ContainsKey(nameof(Password));

    public string ConfirmPasswordError => _errors.ContainsKey(nameof(ConfirmPassword)) ? string.Join("\n", _errors[nameof(ConfirmPassword)]) : string.Empty;
    public bool HasConfirmPasswordError => _errors.ContainsKey(nameof(ConfirmPassword));

    public string DisplayNameError => _errors.ContainsKey(nameof(DisplayName)) ? string.Join("\n", _errors[nameof(DisplayName)]) : string.Empty;
    public bool HasDisplayNameError => _errors.ContainsKey(nameof(DisplayName));

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
                ValidateConfirmPassword(_confirmPassword); 
            }
        }
    }

    public string ConfirmPassword
    {
        get => _confirmPassword;
        set
        {
            if (SetProperty(ref _confirmPassword, value))
            {
                ValidateConfirmPassword(value);
            }
        }
    }

    public string DisplayName
    {
        get => _displayName;
        set
        {
            if (SetProperty(ref _displayName, value))
            {
                ValidateDisplayName(value);
            }
        }
    }

    public ICommand RegisterCommand { get; }
    public ICommand LoginInsteadCommand { get; }
    public ICommand ClearEmailCommand { get; }
    public ICommand ClearPasswordCommand { get; }
    public ICommand ClearConfirmPasswordCommand { get; }
    public ICommand ClearDisplayNameCommand { get; }
    public ICommand BackCommand { get; }

    public bool HasErrors => _errors.Any();

    public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

    public RegisterViewModel(IAuthService authService, INavigationService navigationService, IUserService userService)
    {
        _authService = authService;
        _navigationService = navigationService;
        _userService = userService;

        RegisterCommand = new Command(async () => await ExecuteRegisterCommand(),
            () => CanExecuteRegister());
        LoginInsteadCommand = new Command(async () => await NavigateToLogin());
        ClearEmailCommand = new Command(() => Email = string.Empty);
        ClearPasswordCommand = new Command(() => Password = string.Empty);
        ClearConfirmPasswordCommand = new Command(() => ConfirmPassword = string.Empty);
        ClearDisplayNameCommand = new Command(() => DisplayName = string.Empty);
        BackCommand = new Command(async () => await GoBack());
        PropertyChanged += (_, __) => ((Command)RegisterCommand).ChangeCanExecute();
    }

    private async Task GoBack()
    {
        await _navigationService.NavigateToAsync("///MainPage");
    }

    private async Task NavigateToLogin()
    {
        await _navigationService.NavigateToAsync("LoginPage");
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

    private void ValidateConfirmPassword(string confirmPassword)
    {
        var errorList = new List<string>();

        if (string.IsNullOrWhiteSpace(confirmPassword))
        {
            errorList.Add("Please confirm your password.");
        }
        else if (confirmPassword != Password)
        {
            errorList.Add("Passwords do not match.");
        }

        if (errorList.Count > 0)
        {
            _errors[nameof(ConfirmPassword)] = errorList;
        }
        else
        {
            _errors.Remove(nameof(ConfirmPassword));
        }

        RaiseErrorsChanged(nameof(ConfirmPassword));
    }

    private void ValidateDisplayName(string displayName)
    {
        var errorList = new List<string>();

        if (string.IsNullOrWhiteSpace(displayName))
        {
            errorList.Add("Display name is required.");
        }
        else if (displayName.Length < 3)
        {
            errorList.Add("Display name must be at least 3 characters long.");
        }

        if (errorList.Count > 0)
        {
            _errors[nameof(DisplayName)] = errorList;
        }
        else
        {
            _errors.Remove(nameof(DisplayName));
        }

        RaiseErrorsChanged(nameof(DisplayName));
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

    private async Task ExecuteRegisterCommand()
    {
        ValidateDisplayName(DisplayName);
        ValidateEmail(Email);
        ValidatePassword(Password);
        ValidateConfirmPassword(ConfirmPassword);

        if (HasErrors)
        {
            return;
        }

        try
        {
            IsBusy = true;

            var userInfo = await _authService.RegisterAsync(Email, Password, DisplayName);

            await Application.Current.MainPage.DisplayAlert("Success",
                "Registration successful! You are now logged in.", "OK");
            await _userService.AssignRoleAsync(Email, nameof(UserRolesEnum.User));
            await _navigationService.NavigateToAsync("HomePage");
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Error",
                $"Registration failed: {ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    private bool CanExecuteRegister()
    {
        return !string.IsNullOrWhiteSpace(Email) &&
               !string.IsNullOrWhiteSpace(Password) &&
               !string.IsNullOrWhiteSpace(ConfirmPassword) &&
               !string.IsNullOrWhiteSpace(DisplayName) &&
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
        OnPropertyChanged(nameof(ConfirmPasswordError));
        OnPropertyChanged(nameof(HasConfirmPasswordError));
        OnPropertyChanged(nameof(DisplayNameError));
        OnPropertyChanged(nameof(HasDisplayNameError));
        ((Command)RegisterCommand).ChangeCanExecute();
    }
}