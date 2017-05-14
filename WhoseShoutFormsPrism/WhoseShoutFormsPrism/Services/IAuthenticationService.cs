using System;
using System.Threading.Tasks;
using Xamarin.Auth;

namespace WhoseShoutFormsPrism.Services
{
    public interface IAuthenticationService
    {
        bool Login(string username, string password);
        Task<bool> SocialLogin(Account account);
        void RegisterFacebook();

        void Logout();
    }
}
