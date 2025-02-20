using Authentication.IServices;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Options;
using Shared.Models;
using System;
using System.Threading.Tasks;

namespace Authentication.Services;

public class FirebaseAuthService : IAuthService
{
    private readonly FirebaseAuth _auth;
    private readonly FirebaseSettings _settings;
    private UserInfo _currentUser;

    public event EventHandler<UserInfo> AuthStateChanged;

    public FirebaseAuthService(IOptions<FirebaseSettings> settings)
    {
        _settings = settings.Value;

        var serviceAccountKeyJson = System.Text.Json.JsonSerializer.Serialize(_settings.ServiceAccountKey);

        var credential = GoogleCredential.FromJson(serviceAccountKeyJson);

        if (FirebaseApp.DefaultInstance == null)
        {
            FirebaseApp.Create(new AppOptions
            {
                Credential = credential,
                ProjectId = _settings.ProjectId
            });
        }

        _auth = FirebaseAuth.DefaultInstance;
        if (_auth == null)
        {
            throw new InvalidOperationException("FirebaseAuth.DefaultInstance is null after initialization.");
        }
    }

    public async Task<UserInfo> LoginAsync(string email, string password)
    {
        try
        {
            var userRecord = await _auth.GetUserByEmailAsync(email);

            var userInfo = new UserInfo
            {
                Email = userRecord.Email,
                Name = userRecord.DisplayName,
                UserId = userRecord.Uid
            };

            _currentUser = userInfo;
            AuthStateChanged?.Invoke(this, userInfo);

            return userInfo;
        }
        catch (FirebaseAuthException ex)
        {
            throw new Exception("Authentication failed", ex);
        }
    }

    public async Task<UserInfo> RegisterAsync(string email, string password, string displayName)
    {
        try
        {
            var userArgs = new UserRecordArgs()
            {
                Email = email,
                EmailVerified = false,
                Password = password,
                DisplayName = displayName,
                Disabled = false,
            };

            var userRecord = await _auth.CreateUserAsync(userArgs);

            var userInfo = new UserInfo
            {
                Email = userRecord.Email,
                Name = userRecord.DisplayName,
                UserId = userRecord.Uid
            };

            _currentUser = userInfo;
            AuthStateChanged?.Invoke(this, userInfo);

            return userInfo;
        }
        catch (FirebaseAuthException ex)
        {
            throw new Exception("Registration failed", ex);
        }
    }

    public Task LogoutAsync()
    {
        _currentUser = null;
        AuthStateChanged?.Invoke(this, null);
        return Task.CompletedTask;
    }

    public Task<UserInfo> GetCurrentUserAsync()
    {
        return Task.FromResult(_currentUser);
    }
}