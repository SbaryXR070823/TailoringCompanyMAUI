using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TailoringCompany.Helpers;

public class UserInfoHelper
{
    public static UserInfo GetUserInfoFromPreferences()
    {
        var userInfo = new UserInfo
        {
            Email = Preferences.Get("UserEmail", string.Empty), 
            Name = Preferences.Get("UserName", string.Empty),  
            Role = Preferences.Get("UserRole", string.Empty),  
            UserId = Preferences.Get("UserId", string.Empty)   
        };

        return userInfo;
    }
}
