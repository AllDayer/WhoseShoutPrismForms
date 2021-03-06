﻿using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using Prism.Navigation;
using WhoseShoutFormsPrism.Models;
using WhoseShoutWebService.Models;
using System.Collections.ObjectModel;

namespace WhoseShoutFormsPrism.ViewModels
{
    public class BuyPageViewModel : BaseViewModel
    {
        INavigationService m_NavigationService;
        private ShoutDto m_Shout = new ShoutDto();
        private ShoutGroupDto m_ShoutGroup = new ShoutGroupDto();

        public DelegateCommand BuyCommand { get; }
        public DelegateCommand CancelCommand { get; }
        public DelegateCommand EditGroupCommand { get; }
        public DelegateCommand HistoryCommand { get; }

        public ObservableCollection<ShoutUserDto> UsersForShout { get; set; }

        public BuyPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "Record";
            m_NavigationService = navigationService;
            BuyCommand = new DelegateCommand(OnBuyCommand, BuyCommandCanExecute).ObservesProperty(() => UserDto);
            CancelCommand = new DelegateCommand(OnCancelCommand);
            EditGroupCommand = new DelegateCommand(OnEditGroupCommand);
            HistoryCommand = new DelegateCommand(OnHistoryCommand);
            UsersForShout = new ObservableCollection<ShoutUserDto>();
        }


        public String ID
        {
            get
            {
                return m_Shout.ID.ToString();
            }
            set
            {
                m_Shout.ID = new Guid(value);
                RaisePropertyChanged(nameof(ID));
            }
        }

        public String GroupID
        {
            get
            {
                return m_Shout.ShoutGroupID.ToString();
            }
        }


        public String ShoutTitle
        {
            get
            {
                return m_Shout.ShoutGroupName;
            }
        }

        public bool TrackCost
        {
            get
            {
                return m_ShoutGroup.TrackCost;
            }
        }

        public String Cost
        {
            get
            {
                return m_Shout.Cost.ToString();
            }
            set
            {
                float cost = 0;
                float.TryParse(value, out cost);
                m_Shout.Cost = cost;
                RaisePropertyChanged(nameof(Cost));
            }
        }

        private string m_UserName;
        public string UserName
        {
            get
            {
                return m_UserName;
            }
            set
            {
                m_UserName = value;
                if (UsersForShout != null && UsersForShout.Count > 0)
                {
                    UserDto = UsersForShout.FirstOrDefault(x => x.UserName.StartsWith(value, StringComparison.OrdinalIgnoreCase));
                }
                RaisePropertyChanged(nameof(UserName));
            }
        }

        private ShoutUserDto m_UserDto;
        public ShoutUserDto UserDto
        {
            get
            {
                return m_UserDto;
            }
            set
            {
                m_UserDto = value;
                RaisePropertyChanged(nameof(UserDto));
            }
        }

        int m_SelectedIndex = -1;
        public int SelectedIndex
        {
            get
            {
                return m_SelectedIndex;
            }
            set
            {
                if (m_SelectedIndex != value && value >= 0)
                {
                    m_SelectedIndex = value;
                    RaisePropertyChanged(nameof(SelectedIndex));
                    UserDto = UsersForShout[m_SelectedIndex];
                }
            }
        }

        private bool BuyCommandCanExecute() => UserDto != null;

        public async void OnBuyCommand()
        {
            m_Shout.PurchaseTimeUtc = DateTime.UtcNow;
            m_Shout.ShoutUserID = UserDto.ID;
            m_Shout.ShoutGroupID = new Guid(GroupID);
            m_Shout.Cost = (float.Parse(Cost));

            //Save sync item
            //Sync with server
            await CurrentApp.Current.MainViewModel.ServiceApi.NewShout(m_Shout);
            
            //await _navigationService.NavigateAsync("SummaryPage");
            await m_NavigationService.GoBackAsync();
        }

        public async void OnCancelCommand()
        {
            //Are you sure
            //await _navigationService.NavigateAsync("SummaryPage");
            await m_NavigationService.GoBackAsync();

        }

        public async void OnEditGroupCommand()
        {
            NavigationParameters nav = new NavigationParameters();
            nav.Add("group", m_ShoutGroup);
            nav.Add("shout", m_Shout);
            nav.Add("edit", true);
            await _navigationService.NavigateAsync("ShoutGroupPage", nav);
        }

        public async void OnHistoryCommand()
        {
            NavigationParameters nav = new NavigationParameters();
            nav.Add("group", m_ShoutGroup);
            await _navigationService.NavigateAsync("HistoryPage", nav);
        }

        public override void OnNavigatedFrom(NavigationParameters parameters)
        {
        }

        
        public override void OnNavigatedTo(NavigationParameters parameters)
        {

        }

        public override void OnNavigatingTo(NavigationParameters parameters)
        {
            m_Shout = (ShoutDto)parameters["model"];
            RaisePropertyChanged(nameof(GroupID));
            RaisePropertyChanged(nameof(ID));
            RaisePropertyChanged(nameof(ShoutTitle));
            RaisePropertyChanged(nameof(Cost));

            m_ShoutGroup = (ShoutGroupDto)parameters["group"];
            RaisePropertyChanged(nameof(TrackCost));

            foreach (var u in m_ShoutGroup.Users)
            {
                UsersForShout.Add(u);
            }

            if(m_Shout.ShoutUserID != Guid.Empty)
            {
                SelectedIndex = UsersForShout.IndexOf(UsersForShout.FirstOrDefault(x => x.ID == m_Shout.ShoutUserID));
            }

            RaisePropertyChanged("UserName");
        }
    }
}
