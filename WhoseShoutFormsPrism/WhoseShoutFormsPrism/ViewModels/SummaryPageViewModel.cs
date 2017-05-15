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
using System.Collections.ObjectModel;
using Prism.Events;

namespace WhoseShoutFormsPrism.ViewModels
{
    public class SummaryPageViewModel : BaseViewModel
    {
        public ObservableCollection<ShoutGroupDto> ShoutGroups { get; set; }

        public List<ShoutDto> ShoutsForGroup { get; set; }

        IAuthenticationService _authenticationService { get; }
        IEventAggregator m_EventAggregator { get; }


        private ShoutUserDto m_WhoseShout;
        public ShoutUserDto WhoseShout
        {
            get { return m_WhoseShout; }
            set { SetProperty(ref m_WhoseShout, value); }
        }

        private string _message;
        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }
        

        public DelegateCommand LogoutCommand { get; }
        public DelegateCommand NewShoutCommand { get; }
        public DelegateCommand BuyCommand { get; }
        

        private ShoutGroupDto m_ShoutGroupDto = new ShoutGroupDto();
        public ShoutGroupDto ShoutGroupDto
        {
            get { return m_ShoutGroupDto; }
            set { SetProperty(ref m_ShoutGroupDto, value); }
        }

        public SummaryPageViewModel(INavigationService navigationService, IAuthenticationService authenticationService, IEventAggregator eventAggregator)
            : base(navigationService)
        {
            Title = "Summary";
            _authenticationService = authenticationService;
            m_EventAggregator = eventAggregator;
            LogoutCommand = new DelegateCommand(OnLogoutCommandExecuted);
            NewShoutCommand = new DelegateCommand(OnNewShoutCommandExecuted);
            //BuyCommand = new DelegateCommand(OnBuyCommandExecuted);
            //LoadData();
            //ShoutName = ShoutGroups[0].Name;
            //User1 = ShoutGroups[0].ShoutUsers[0].UserName;
            //User2 = ShoutGroups[0].ShoutUsers[1].UserName;
        }

        public override void OnNavigatingTo(NavigationParameters parameters)
        {
            base.OnNavigatingTo(parameters);

            Message = parameters.GetValue<string>("message");
            var groups = (List<ShoutGroupDto>)parameters["model"];
            ShoutGroups = new ObservableCollection<ShoutGroupDto>(groups);
            LoadData();
            m_EventAggregator.GetEvent<GroupsLoadedEvent>().Publish();
        }

        private String TristanUserString = "d9c91004-3994-4bb4-a703-267904985126";

        public async void LoadData()
        {
            //ShoutGroupDto = ShoutGroups.First();
            ShoutGroupDto = ShoutGroups?.FirstOrDefault();
            foreach (ShoutGroupDto sg in ShoutGroups)
            {
                //Move to a better call
                ShoutsForGroup = await LoadShoutsForGroup(ShoutGroupDto.ID.ToString());
                CalculateWhoseShout(ShoutGroupDto.ID);
            }
        }

        private void CalculateWhoseShout(Guid shoutGuid)
        {
            try
            {
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
            catch (Exception)
            {
            }
        }


        public async Task<List<ShoutDto>> LoadShoutsForGroup(String groupID)
        {
            var shouts = await CurrentApp.Current.MainViewModel.ServiceApi.GetShoutsForGroup(groupID);
            if (shouts != null)
            {
                return shouts;
            }
            return new List<ShoutDto>();
        }

        public void OnLogoutCommandExecuted() => _authenticationService.Logout();

        public async void OnNewShoutCommandExecuted()
        {
            await _navigationService.NavigateAsync("MainPage/NewShoutGroupPage");
        }

        public async void OnBuyCommandExecuted(BuyRoundArgs e)
        {
            NavigationParameters nav = new NavigationParameters();
            nav.Add("model", new ShoutDto() { ID = Guid.NewGuid(), ShoutGroupID = e.Group.ID });
            nav.Add("users", ShoutGroupDto.Users);
            await _navigationService.NavigateAsync("MainPage/BuyPage", nav);
        }
    }
}
