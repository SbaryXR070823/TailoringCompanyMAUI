using BackendServices.IServices;
using Microsoft.Maui.Devices;
using Shared.Models;
using System.Net.Http.Json;
using System.Text.Json;


namespace BackendServices.Services;

public class UserService : IUserService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiUrl = $"{Shared.Constants.BackendUrl}"; 

    public UserService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        if (DeviceInfo.Platform == DevicePlatform.Android)
        {
            _apiUrl = "http://10.0.2.2:8000";
        }
        else
        {
            _apiUrl = $"{Shared.Constants.BackendUrl}";
        }

        Console.WriteLine($"API URL set to: {_apiUrl}");
    }

    public async Task<string> GetUserRoleAsync(string email)
    {
        try
        {
            var requestUrl = $"{_apiUrl}/user-role/{email}";
            Console.WriteLine($"Calling backend URL: {requestUrl}");
            var httpResponse = await _httpClient.GetAsync(requestUrl);
            Console.WriteLine($"Response status code: {httpResponse.StatusCode}");
            httpResponse.EnsureSuccessStatusCode();
            var responseString = await httpResponse.Content.ReadAsStringAsync();
            Console.WriteLine($"Backend response: {responseString}");
            try
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                var response = JsonSerializer.Deserialize<UserRoleResponse>(responseString, options);
                return response?.Role ?? null;
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"JSON exception: {ex.Message}");
                return responseString;
            }
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Error fetching user role for {email}: {ex.Message}");
            return null;
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"Error deserializing user role response for {email}: {ex.Message}");
            return null;
        }
    }

    public async Task<bool> AssignRoleAsync(string email, string role)
    {
        var body = new { email, role };
        var response = await _httpClient.PostAsJsonAsync($"{_apiUrl}/assign-role/", body);

        if (response.IsSuccessStatusCode)
        {
            return true; 
        }
        else
        {
            Console.WriteLine($"Error assigning role to {email}: {response.StatusCode}");
            return false; 
        }
    }
}
