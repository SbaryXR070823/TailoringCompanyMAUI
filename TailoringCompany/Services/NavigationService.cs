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
}
