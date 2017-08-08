using System.Threading.Tasks;

using BarBot.Core.Model;

namespace BarBot.Core.Service.Login
{
    public interface ILoginService
    {
        Task<User> RegisterUser(string name, string emailAddress, string password);
        Task<bool> LoginUser(string emailAddress, string password);
        Task<bool> ForgotPassword(string emailAddress);
    }
}
