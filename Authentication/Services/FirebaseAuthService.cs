using Authentication.IServices;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Options;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        // Initialize Firebase Admin SDK if not already initialized
        if (FirebaseApp.DefaultInstance == null)
        {
            FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile("path-to-your-service-account.json"),
                ProjectId = _settings.ProjectId
            });
        }

        _auth = FirebaseAuth.DefaultInstance;
    }

    public async Task<UserInfo> LoginAsync(string email, string password)
    {
        try
        {
            // Use Firebase Admin SDK to verify the user
            var userRecord = await _auth.GetUserByEmailAsync(email);

            // In a real implementation, you would validate the password
            // For now, we're just checking if the user exists

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
