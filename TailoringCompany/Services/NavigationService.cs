using Authentication.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TailoringCompany.Services;

namespace Authentication.Services;

public class NavigationService : INavigationService
{
    public async Task NavigateToAsync(string pageName)
    {
        await Shell.Current.GoToAsync(pageName);
    }

    public Task NavigateToAsync(string route, IDictionary<string, object> parameters = null)
    {
        return parameters != null
            ? Shell.Current.GoToAsync(route, parameters)
            : Shell.Current.GoToAsync(route);
    }

    public Task GoBackAsync()
    {
        return Shell.Current.GoToAsync("..");
    }
}
