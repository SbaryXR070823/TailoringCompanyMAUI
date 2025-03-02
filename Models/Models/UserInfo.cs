using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models
{
    public class UserInfo
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string Role { get; set; } = string.Empty;
        public string UserId { get; set; }
    }

    public class UserRoleResponse
    {
        public string Role { get; set; }
    }
}
