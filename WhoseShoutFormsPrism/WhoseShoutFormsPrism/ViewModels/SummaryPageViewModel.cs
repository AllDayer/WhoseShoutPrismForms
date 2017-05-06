using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using WhoseShoutFormsPrism.Models;
using WhoseShoutFormsPrism.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WhoseShoutFormsPrism.ViewModels
{
    public class SummaryPageViewModel : BaseViewModel
    {
        public List<ShoutGroup> ShoutGroups { get; set; }

        IAuthenticationService _authenticationService { get; }
        public SummaryPageViewModel(INavigationService navigationService, IAuthenticationService authenticationService)
            : base(navigationService)
        {
            MockData();
            Title = "Summary";
            _authenticationService = authenticationService;
            LogoutCommand = new DelegateCommand(OnLogoutCommandExecuted);
            NewShoutCommand = new DelegateCommand(OnNewShoutCommandExecuted);
            BuyCommand = new DelegateCommand(OnBuyCommandExecuted);

            ShoutName =ShoutGroups[0].Name;
            User1 = ShoutGroups[0].Users[0].UserName;
            User2 = ShoutGroups[0].Users[1].UserName;
        }

        private void MockData()
        {
            ShoutGroups = new List<ShoutGroup>();
            ShoutGroups.Add(new ShoutGroup()
            {
                Name = "CoffeeTime!",
                Category = "Coffee",
                ID = new Guid("bf3641d1-a384-494d-a957-18f2aa42170c"),
                Users = new List<User>()
                {
                    new User()
                    {
                        ID = new Guid("d9c91004-3994-4bb4-a703-267904985126"),
                        UserName = "Tristan"
                    },
                    new User()
                    {
                        ID = new Guid("c9c9f88b-853b-46e5-a70a-fad212fab6b0"),
                        UserName = "Norman"
                    }
                },
                Shouts = new List<Shout>()
                {
                    new Shout()
                    {
                        ShoutGroupID = new Guid("bf3641d1-a384-494d-a957-18f2aa42170c"),
                        ID = new Guid("45def82e-2b68-47e1-98c6-7d34906b46f1"),
                        UserID = new Guid("d9c91004-3994-4bb4-a703-267904985126"),
                        Cost = 9.0f,
                        PurchaseTimeUtc = DateTime.UtcNow,
                    }
                }
            });
        }

        private string _message;
        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }

        private string m_ShoutName;
        public string ShoutName
        {
            get { return m_ShoutName; }
            set { SetProperty(ref m_ShoutName, value); }
        }

        private string m_User1;
        public string User1
        {
            get { return m_User1; }
            set { SetProperty(ref m_User1, value); }
        }

        private string m_User2;
        public string User2
        {
            get { return m_User2; }
            set { SetProperty(ref m_User2, value); }
        }

        public DelegateCommand LogoutCommand { get; }
        public DelegateCommand NewShoutCommand { get; }
        public DelegateCommand BuyCommand { get; }

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            Message = parameters.GetValue<string>("message");
        }

        public void OnLogoutCommandExecuted() =>
            _authenticationService.Logout();

        public async void OnNewShoutCommandExecuted()
        {
            await _navigationService.NavigateAsync("MainPage/NewShout");
        }

        public async void OnBuyCommandExecuted()
        {
            NavigationParameters nav = new NavigationParameters();
            nav.Add("model", new Shout() { ID = Guid.NewGuid(), ShoutGroupID = this.ShoutGroups[0].ID });
            await _navigationService.NavigateAsync("MainPage/BuyPage", nav);
        }
    }
}
