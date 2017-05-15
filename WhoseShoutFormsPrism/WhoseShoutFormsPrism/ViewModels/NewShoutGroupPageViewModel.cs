using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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

        public ObservableCollection<ShoutUserDto> UsersInGroup { get; set; } = new ObservableCollection<ShoutUserDto>();
        public ShoutGroupDto Group { get; set; }
        public String ShoutName { get; set; }

        public NewShoutGroupPageViewModel(INavigationService navigationService, IEventAggregator eventAggregator) : base(navigationService)
        {
            m_NavigationService = navigationService;
            m_EventAggregator = eventAggregator;
            CreateGroupCommand = new DelegateCommand(OnCreateGroupCommand);
            AddUserToGroupCommand = new DelegateCommand(OnAddUserToGroupCommand);
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
                Name = ShoutName,
                Users = UsersInGroup.ToList()
            };

            await CurrentApp.Current.MainViewModel.ServiceApi.CreateGroupCommand(Group);
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
