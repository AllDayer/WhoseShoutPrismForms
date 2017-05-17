﻿using Prism.Commands;
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
using WhoseShoutFormsPrism.Helpers;

namespace WhoseShoutFormsPrism.ViewModels
{
    public class SummaryPageViewModel : BaseViewModel
    {
        IAuthenticationService _authenticationService { get; }
        IEventAggregator m_EventAggregator { get; }
        public DelegateCommand LogoutCommand { get; }
        public DelegateCommand NewShoutCommand { get; }
        public DelegateCommand BuyCommand { get; }

        private bool m_NoGroups;
        public bool NoGroups
        {
            get { return m_NoGroups; }
            set { SetProperty(ref m_NoGroups, value); }
        }
        
        private ShoutGroupDto m_ShoutGroupDto = new ShoutGroupDto();
        public ShoutGroupDto ShoutGroupDto
        {
            get { return m_ShoutGroupDto; }
            set { SetProperty(ref m_ShoutGroupDto, value); }
        }
        public ObservableCollection<ShoutGroupDto> ShoutGroups
        {
            get
            {
                return Settings.Current.ShoutGroups;
            }
        }


        public List<ShoutDto> ShoutsForGroup { get; set; }

        public SummaryPageViewModel(INavigationService navigationService, IAuthenticationService authenticationService, IEventAggregator eventAggregator)
            : base(navigationService)
        {
            Title = "Summary";
            _authenticationService = authenticationService;
            m_EventAggregator = eventAggregator;
            LogoutCommand = new DelegateCommand(OnLogoutCommandExecuted);
            NewShoutCommand = new DelegateCommand(OnNewShoutCommandExecuted);
        }

        public override void OnNavigatingTo(NavigationParameters parameters)
        {
            base.OnNavigatingTo(parameters);
            LoadData();
            m_EventAggregator.GetEvent<GroupsLoadedEvent>().Publish();
        }

        private String TristanUserString = "d9c91004-3994-4bb4-a703-267904985126";

        public async void LoadData()
        {
            //ShoutGroupDto = ShoutGroups.First();
            ShoutGroupDto = Settings.Current.ShoutGroups?.FirstOrDefault();
            foreach (ShoutGroupDto sg in Settings.Current.ShoutGroups)
            {
                //Move to a better call
                ShoutsForGroup = await LoadShoutsForGroup(ShoutGroupDto.ID.ToString());
                NoGroups = false;
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
