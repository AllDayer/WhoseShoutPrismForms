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
        IAuthenticationService _authenticationService { get; }
        IPageDialogService _pageDialogService { get; }

        public LoginPageViewModel(INavigationService navigationService, IAuthenticationService authenticationService, IPageDialogService pageDialogService)
            : base(navigationService)
        {
            _authenticationService = authenticationService;
            _pageDialogService = pageDialogService;

            Title = "Login";

            LoginCommand = new DelegateCommand(OnLoginCommandExecuted, LoginCommandCanExecute)
                .ObservesProperty(() => UserName)
                .ObservesProperty(() => Password);

            OAuthCommand = new DelegateCommand(OnOAuthCommandExecuted);

            UserName = "a";
            Password = "a";
        }

        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set { SetProperty(ref _userName, value); }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set { SetProperty(ref _password, value); }
        }

        public DelegateCommand LoginCommand { get; }
        public DelegateCommand OAuthCommand { get; }

        private String TristanUserString = "d9c91004-3994-4bb4-a703-267904985126";
        private async void OnLoginCommandExecuted()
        {
            IsBusy = true;
            if (_authenticationService.Login(UserName, Password))
            {
                NavigationParameters nav = new NavigationParameters();

                var groups = await CurrentApp.Current.MainViewModel.ServiceApi.GetShoutGroups(TristanUserString);
                nav.Add("model", groups);

                await _navigationService.NavigateAsync("/MainPage/NavigationPage/SummaryPage", nav);
            }
            else
            {
                await _pageDialogService.DisplayAlertAsync("Wrong", "Hi", "");
            }
            IsBusy = false;
        }

        private async void OnOAuthCommandExecuted()
        {
            Account account = null;
            IEnumerable<Account> accounts = AccountStore.Create().FindAccountsForService("Facebook");
            if (accounts != null)
            {
                account = accounts.FirstOrDefault();
            }
            if (accounts == null)
            {
                _authenticationService.RegisterFacebook();
            }
            else
            {
                bool success = await _authenticationService.SocialLogin(account);
                if (Settings.Current.UserGuid == null || Settings.Current.UserGuid == Guid.Empty)
                {
                    ShoutUserDto userDto = new ShoutUserDto()
                    {
                        ShoutSocialID = Settings.Current.SocialUserID,
                        AuthType = Settings.Current.UserAuth,
                        UserName = Settings.Current.UserFirstName

                    };
                    var user = await CurrentApp.Current.MainViewModel.ServiceApi.GetShoutUserBySocial(userDto);

                    if (user != null)
                    {
                        Settings.Current.UserGuid = user.ID;
                    }
                    else
                    {
                        userDto.ID = Guid.NewGuid();
                        success = await CurrentApp.Current.MainViewModel.ServiceApi.NewShoutUser(userDto);
                        if (success)
                        {
                            Settings.Current.UserGuid = userDto.ID;
                        }
                    }
                }

                if (success)
                {
                    NavigationParameters nav = new NavigationParameters();

                    var groups = await CurrentApp.Current.MainViewModel.ServiceApi.GetShoutGroups(Settings.Current.UserGuid.ToString());
                    nav.Add("model", groups);

                    await _navigationService.NavigateAsync("/MainPage/NavigationPage/SummaryPage", nav);
                }
            }
        }

        private bool LoginCommandCanExecute() =>
            !string.IsNullOrWhiteSpace(UserName) && !string.IsNullOrWhiteSpace(Password) && IsNotBusy;
    }
}
