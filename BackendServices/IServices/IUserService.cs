using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendServices.IServices
{
    public interface IUserService
    {
        Task<bool> AssignRoleAsync(string email, string role);
        Task<string> GetUserRoleAsync(string email);
    }
}
