using System;
using System.Threading.Tasks;
using Xamarin.Auth;

namespace WhoseShoutFormsPrism.Services
{
    public interface IAuthenticationService
    {
        Task<bool> SocialLogin(Account account);
        void RegisterFacebook();
        void Logout();
    }
}
