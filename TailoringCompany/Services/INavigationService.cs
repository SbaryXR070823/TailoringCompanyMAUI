using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TailoringCompany.Services;
public interface INavigationService
{
    public Task NavigateToAsync(string pageName);
    public Task NavigateToAsync(string route, IDictionary<string, object> parameters = null);
    public Task GoBackAsync();
}
