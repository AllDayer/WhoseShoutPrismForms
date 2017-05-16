using System;
using WhoseShoutFormsPrism.Services;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using Xamarin.Auth;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WhoseShoutFormsPrism.Helpers;
using WhoseShoutWebService.Models;

namespace WhoseShoutFormsPrism.ViewModels
{
    public class LoginPageViewModel : BaseViewModel
    {
        IAuthenticationService m_AuthenticationService { get; }
        IPageDialogService _pageDialogService { get; }
        public DelegateCommand OAuthCommand { get; }

        //private String TristanUserString = "d9c91004-3994-4bb4-a703-267904985126";

        public LoginPageViewModel(INavigationService navigationService, IAuthenticationService authenticationService, IPageDialogService pageDialogService)
            : base(navigationService)
        {
            m_AuthenticationService = authenticationService;
            _pageDialogService = pageDialogService;

            Title = "Login";

            OAuthCommand = new DelegateCommand(OnOAuthCommandExecuted);
        }

        private bool m_IsLoggingIn;
        public bool IsLoggingIn
        {
            get { return m_IsLoggingIn; }
            set { SetProperty(ref m_IsLoggingIn, value); }
        }

        private async void OnOAuthCommandExecuted()
        {
            bool potentialFirstTimeExecuted = true;
            IsLoggingIn = true;
            Account account = null;
            IEnumerable<Account> accounts = AccountStore.Create().FindAccountsForService("Facebook");
            if (accounts != null)
            {
                account = accounts.FirstOrDefault();
                potentialFirstTimeExecuted = false;
            }
            if (account == null)
            {
                //Get auth token from Facebook
                m_AuthenticationService.RegisterFacebook();
            }


            bool success = await m_AuthenticationService.SocialLogin(account);



            if (success)
            {
                NavigationParameters nav = new NavigationParameters();

                var groups = await CurrentApp.Current.MainViewModel.ServiceApi.GetShoutGroups(Settings.Current.UserGuid.ToString());
                nav.Add("model", groups);

                await _navigationService.NavigateAsync("/MainPage/NavigationPage/SummaryPage", nav);
            }

            IsLoggingIn = false;
        }
        
    }
}
