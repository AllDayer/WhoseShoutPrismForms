using System;
using Prism.Navigation;
using WhoseShoutFormsPrism.Helpers;
using Xamarin.Auth;
using System.Threading.Tasks;

namespace WhoseShoutFormsPrism.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        INavigationService _navigationService { get; }

        public AuthenticationService(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public bool Login(string username, string password)
        {
            if (password.Equals("a", StringComparison.OrdinalIgnoreCase))
            {
                Settings.Current.UserName = username;
                return true;
            }

            return false;
        }

        public async Task<bool> SocialLogin(Account account)
        {
            bool success = await GetFacebook(account);
            if (success)
            {

            }
            return success;
        }

        #region Facebook
        async Task<bool> GetFacebook(Account account)
        {
            try
            {
                var request = new OAuth2Request("GET", new Uri("https://graph.facebook.com/me?fields=email,first_name,last_name,gender,picture"), null, account);
                var response = await request.GetResponseAsync();
                var fbUser = Newtonsoft.Json.Linq.JObject.Parse(response.GetResponseText());

                //var email = fbUser["email"].ToString().Replace("\"", "");
                var name = fbUser["first_name"].ToString().Replace("\"", "");
                var id = fbUser["id"].ToString().Replace("\"", "");
                var email = fbUser["email"].ToString().Replace("\"", "");

                Settings.Current.UserFirstName = name;
                Settings.Current.SocialUserID = id;
                Settings.Current.UserEmail = email;

            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public void RegisterFacebook()
        {
            var authenticator = new OAuth2Authenticator(
                                            clientId: "842344439275386",
                                            scope: "email",
                                            authorizeUrl: new Uri("https://m.facebook.com/dialog/oauth/"),
                                            redirectUrl: new Uri("http://www.facebook.com/connect/login_success.html"));

            authenticator.Completed += OnAuthCompleted;
            authenticator.Error += OnAuthError;

            var presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();
            presenter.Login(authenticator);
        }

        void OnAuthCompleted(object sender, AuthenticatorCompletedEventArgs e)
        {
            var authenticator = sender as OAuth2Authenticator;

            if (authenticator != null)
            {
                authenticator.Completed -= OnAuthCompleted;
                authenticator.Error -= OnAuthError;
            }

            if (e.IsAuthenticated)
            {
                var accessToken = e.Account.Properties["access_token"].ToString();
                AccountStore.Create().Save(e.Account, "Facebook");
            }
        }

        void OnAuthError(object sender, AuthenticatorErrorEventArgs e)
        {
            var authenticator = sender as OAuth2Authenticator;

            if (authenticator != null)
            {
                authenticator.Completed -= OnAuthCompleted;
                authenticator.Error -= OnAuthError;
            }

            //Debug.WriteLine("Authentication error: " + e.Message);
        }
        #endregion

        public void Logout()
        {
            Settings.Current.UserName = string.Empty;
            _navigationService.NavigateAsync("/Login");
        }
    }
}
