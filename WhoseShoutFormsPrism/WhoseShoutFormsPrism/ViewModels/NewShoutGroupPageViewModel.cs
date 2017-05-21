using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using WhoseShoutFormsPrism.Helpers;
using WhoseShoutFormsPrism.Models;
using WhoseShoutFormsPrism.Services;
using WhoseShoutWebService.Models;

namespace WhoseShoutFormsPrism.ViewModels
{
    public class NewShoutGroupPageViewModel : BaseViewModel
    {
        INavigationService m_NavigationService;
        IEventAggregator m_EventAggregator;

        public DelegateCommand CreateGroupCommand { get; }
        public DelegateCommand AddUserToGroupCommand { get; }
        public DelegateCommand CancelCommand { get; }

        public ObservableCollection<ShoutUserDto> UsersInGroup { get; set; } = new ObservableCollection<ShoutUserDto>();
        public ShoutGroupDto Group { get; set; }
        public String ShoutName { get; set; }

        public NewShoutGroupPageViewModel(INavigationService navigationService, IEventAggregator eventAggregator) : base(navigationService)
        {
            m_NavigationService = navigationService;
            m_EventAggregator = eventAggregator;
            CreateGroupCommand = new DelegateCommand(OnCreateGroupCommand);
            AddUserToGroupCommand = new DelegateCommand(OnAddUserToGroupCommand);
            CancelCommand = new DelegateCommand(OnCancelCommand);
            UsersInGroup.Add(new ShoutUserDto());
        }

        public void OnAddUserToGroupCommand()
        {
            UsersInGroup.Add(new ShoutUserDto());
            RaisePropertyChanged(nameof(UsersInGroup));
        }

        public async void OnCreateGroupCommand()
        {
            Group = new ShoutGroupDto()
            {
                ID = Guid.NewGuid(),
                Name = ShoutName,
                Users = UsersInGroup.ToList()
            };

            Group.Users.Add(new ShoutUserDto() { ID = Settings.Current.UserGuid });

            await CurrentApp.Current.MainViewModel.ServiceApi.CreateGroupCommand(Group);

            NavigationParameters nav = new NavigationParameters();

            var groups = await CurrentApp.Current.MainViewModel.ServiceApi.GetShoutGroups(Settings.Current.UserGuid.ToString());
            nav.Add("model", groups);
            await _navigationService.NavigateAsync("/MainPage/NavigationPage/SummaryPage", nav);
        }

        public async void OnCancelCommand()
        {
            //Show Dialog
        }

        public override void OnNavigatedFrom(NavigationParameters parameters)
        {
        }


        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            
        }

        public override void OnNavigatingTo(NavigationParameters parameters)
        {

        }
    }
}
