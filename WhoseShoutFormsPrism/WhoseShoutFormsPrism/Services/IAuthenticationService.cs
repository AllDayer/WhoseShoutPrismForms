using System;
using System.Threading.Tasks;
using WhoseShoutFormsPrism.ViewModels;
using Xamarin.Auth;

namespace WhoseShoutFormsPrism.Services
{
    public interface IAuthenticationService
    {
        Task<bool> SocialLogin(Account account);
        void RegisterFacebook(LoginPageViewModel loginViewModel);
        void Logout();
    }
}
