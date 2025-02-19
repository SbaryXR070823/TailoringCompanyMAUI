using Shared.Models;

namespace Authentication.IServices;

public interface IAuthService
{
    Task<UserInfo> LoginAsync(string email, string password);
    Task<UserInfo> RegisterAsync(string email, string password, string displayName);
    Task LogoutAsync();
    Task<UserInfo> GetCurrentUserAsync();
    event EventHandler<UserInfo> AuthStateChanged;
}
