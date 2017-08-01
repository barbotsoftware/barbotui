using System;
using System.Threading.Tasks;

using BarBot.Core.Model;

namespace BarBot.Core.Service.Rest
{
    public interface IRestService
    {
        Task<User> RegisterUser(string name, string email, string password);
        Task<bool> LoginUser(string email, string password);
    }
}
