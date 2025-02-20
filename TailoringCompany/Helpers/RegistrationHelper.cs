using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TailoringCompany.Helpers;

public static class RegistrationHelper
{
    public static void RegisterPagesAndViewModels(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        // Register Pages
        var pageTypes = assembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(ContentPage)) && t.Namespace.Contains("TailoringCompany"));

        foreach (var pageType in pageTypes)
        {
            services.AddTransient(pageType);

            // Register route using the class name without namespace
            string routeName = pageType.Name;
            Routing.RegisterRoute(routeName, pageType);
        }

        // Register ViewModels
        var viewModelTypes = assembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && t.Name.EndsWith("ViewModel") && t.Namespace.Contains("TailoringCompany"));

        foreach (var vmType in viewModelTypes)
        {
            services.AddTransient(vmType);
        }
    }
}
