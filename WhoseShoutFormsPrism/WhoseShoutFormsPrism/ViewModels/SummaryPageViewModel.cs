using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using WhoseShoutFormsPrism.Models;
using WhoseShoutFormsPrism.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using WhoseShoutWebService.Models;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace WhoseShoutFormsPrism.ViewModels
{
    public class SummaryPageViewModel : BaseViewModel
    {
        public List<ShoutGroup> ShoutGroups { get; set; }

        public List<ShoutDto> ShoutsForGroup { get; set; }

        IAuthenticationService _authenticationService { get; }

        private String TristanUserString = "d9c91004-3994-4bb4-a703-267904985126";

        private ShoutUserDto m_WhoseShout;
        public ShoutUserDto WhoseShout
        {
            get { return m_WhoseShout; }
            set { SetProperty(ref m_WhoseShout, value); }
        }

        //private void MockData()
        //{
        //    ShoutGroups = new List<ShoutGroup>();
        //    ShoutGroups.Add(new ShoutGroup()
        //    {
        //        Name = "CoffeeTime!",
        //        Category = "Coffee",
        //        ID = new Guid("bf3641d1-a384-494d-a957-18f2aa42170c"),
        //        ShoutUsers = new List<ShoutUser>()
        //        {
        //            new ShoutUser()
        //            {
        //                ID = new Guid("d9c91004-3994-4bb4-a703-267904985126"),
        //                UserName = "Tristan"
        //            },
        //            new ShoutUser()
        //            {
        //                ID = new Guid("c9c9f88b-853b-46e5-a70a-fad212fab6b0"),
        //                UserName = "Norman"
        //            }
        //        },
        //        Shouts = new List<Shout>()
        //        {
        //            new Shout()
        //            {
        //                ShoutGroupID = new Guid("bf3641d1-a384-494d-a957-18f2aa42170c"),
        //                ID = new Guid("45def82e-2b68-47e1-98c6-7d34906b46f1"),
        //                ShoutUserID = new Guid("d9c91004-3994-4bb4-a703-267904985126"),
        //                Cost = 9.0f,
        //                PurchaseTimeUtc = DateTime.UtcNow,
        //            }
        //        }
        //    });
        //}

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

            LoadData();
        }

        private ShoutGroupDto m_ShoutGroupDto;
        public ShoutGroupDto ShoutGroupDto
        {
            get { return m_ShoutGroupDto; }
            set { SetProperty(ref m_ShoutGroupDto, value); }
        }

        public SummaryPageViewModel(INavigationService navigationService, IAuthenticationService authenticationService)
            : base(navigationService)
        {
            Title = "Summary";
            _authenticationService = authenticationService;
            LogoutCommand = new DelegateCommand(OnLogoutCommandExecuted);
            NewShoutCommand = new DelegateCommand(OnNewShoutCommandExecuted);
            BuyCommand = new DelegateCommand(OnBuyCommandExecuted);

            LoadData();
            //ShoutName = ShoutGroups[0].Name;
            //User1 = ShoutGroups[0].ShoutUsers[0].UserName;
            //User2 = ShoutGroups[0].ShoutUsers[1].UserName;
        }

        public override void OnNavigatingTo(NavigationParameters parameters)
        {
            base.OnNavigatingTo(parameters);
        }

        public async void LoadData()
        {
            var groups = await LoadGroups(TristanUserString);
            ShoutsForGroup = await LoadShoutsForGroup(ShoutGroupDto.ID.ToString());
            CalculateWhoseShout(ShoutGroupDto.ID);
        }

        private void CalculateWhoseShout(Guid shoutGuid)
        {
            int max = 0;
            int min = int.MaxValue;
            int lowestShoutCount = int.MaxValue;

            ShoutUserDto whoseShout = null;
            List<ShoutUserDto> tiedShout = new List<ShoutUserDto>();

            List<Guid> userGuids = new List<Guid>();
            foreach (var u in ShoutGroupDto.Users)
            {
                var shoutCount = ShoutsForGroup.Count(x => x.ShoutUserID == u.ID);
                if (shoutCount < lowestShoutCount)
                {
                    whoseShout = u;
                    lowestShoutCount = shoutCount;
                    tiedShout.Clear();
                }
                else if (shoutCount == lowestShoutCount)
                {
                    tiedShout.Add(u);
                }
            }

            if (tiedShout.Count > 0)
            {
                tiedShout.Add(whoseShout);
                WhoseShout = (from x in ShoutsForGroup
                              join tied in tiedShout on x.ShoutUserID equals tied.ID
                              orderby x.PurchaseTimeUtc ascending
                              select tied).FirstOrDefault();
            } 
            else
            {
                WhoseShout = whoseShout;
            }
        }

        public async Task<List<ShoutGroupDto>> LoadGroups(String userID)
        {
            var groups = await CurrentApp.Current.MainViewModel.ServiceApi.GetShoutGroups(userID);
            ShoutGroupDto = groups.First();
            return groups;
        }

        public async Task<List<ShoutDto>> LoadShoutsForGroup(String groupID)
        {
            return await CurrentApp.Current.MainViewModel.ServiceApi.GetShoutsForGroup(groupID);
        }

        public void OnLogoutCommandExecuted() => _authenticationService.Logout();

        public async void OnNewShoutCommandExecuted()
        {
            await _navigationService.NavigateAsync("MainPage/NewShout");
        }

        public async void OnBuyCommandExecuted()
        {
            NavigationParameters nav = new NavigationParameters();
            nav.Add("model", new ShoutDto() { ID = Guid.NewGuid(), ShoutGroupID = ShoutGroupDto.ID });
            nav.Add("users", ShoutGroupDto.Users);
            await _navigationService.NavigateAsync("MainPage/BuyPage", nav);
        }
    }
}
