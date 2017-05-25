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
using Xamarin.Forms;

namespace WhoseShoutFormsPrism.ViewModels
{
    public class ShoutGroupPageViewModel : BaseViewModel
    {
        INavigationService m_NavigationService;
        IEventAggregator m_EventAggregator;

        public DelegateCommand CreateGroupCommand { get; }
        public DelegateCommand AddUserToGroupCommand { get; }
        public DelegateCommand CancelCommand { get; }
        public DelegateCommand ClickColour { get; }
        public DelegateCommand<int?> RemoveUserCommand { get; }
        public Command<object> ClickCommand { get; }
        public bool ShowColours { get; set; }

        public ObservableCollection<ShoutUserDto> UsersInGroup { get; set; } = new ObservableCollection<ShoutUserDto>();
        public ShoutGroupDto Group { get; set; }
        public ShoutDto ShoutFromEdit { get; set; }
        public String ShoutName { get; set; }
        public ObservableCollection<String> Colours { get; set; } = new ObservableCollection<String>();

        public ShoutGroupPageViewModel(INavigationService navigationService, IEventAggregator eventAggregator) : base(navigationService)
        {
            m_NavigationService = navigationService;
            m_EventAggregator = eventAggregator;
            CreateGroupCommand = new DelegateCommand(OnCreateGroupCommand);
            AddUserToGroupCommand = new DelegateCommand(OnAddUserToGroupCommand);
            CancelCommand = new DelegateCommand(OnCancelCommand);
            ClickCommand = new Command<object>(OnClickCommand);
            ClickColour = new DelegateCommand(OnClickColour);
            RemoveUserCommand = new DelegateCommand<int?>(OnRemoveUserCommand);
            UsersInGroup.Add(new ShoutUserDto());
            Colours = MyColours;
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

            //Settings.Current.GroupColourDictionary.Add(Group.ID, SelectedColour);

            NavigationParameters nav = new NavigationParameters();

            var groups = await CurrentApp.Current.MainViewModel.ServiceApi.GetShoutGroups(Settings.Current.UserGuid.ToString());
            nav.Add("model", groups);
            await _navigationService.NavigateAsync("/NavigationPage/SummaryPage", nav);
        }

        public async void OnCancelCommand()
        {
            //Show Dialog
            if (Group != null)
            {
                NavigationParameters nav = new NavigationParameters();
                nav.Add("group", Group);
                nav.Add("model", ShoutFromEdit);
                bool result = await _navigationService.GoBackAsync(nav);
            }
        }

        private string m_SelectedColour = "#d84315";
        public string SelectedColour
        {
            get
            {
                return m_SelectedColour;
            }
            set
            {
                m_SelectedColour = value;
                RaisePropertyChanged(nameof(SelectedColour));
            }
        }

        void OnClickCommand(object s)
        {
            SelectedColour = MyColours[(int)s];
            OnClickColour();
        }

        public void OnClickColour()
        {
            ShowColours = !ShowColours;
            RaisePropertyChanged(nameof(ShowColours));
        }

        public void OnRemoveUserCommand(int? index)
        {
            ShowColours = true;
            if (index.HasValue)
            {
                UsersInGroup.RemoveAt(index.Value);
            }
        }

        public override void OnNavigatedFrom(NavigationParameters parameters)
        {
        }


        public override void OnNavigatedTo(NavigationParameters parameters)
        {
        }

        public override void OnNavigatingTo(NavigationParameters parameters)
        {

            if (parameters["group"] != null)
            {
                this.Group = (ShoutGroupDto)parameters["group"];
                RaisePropertyChanged(nameof(Group));
                ShoutName = Group.Name;
                RaisePropertyChanged(nameof(ShoutName));
                //UsersInGroup = new ObservableCollection<ShoutUserDto>(Group.Users);
                UsersInGroup.Clear();
                foreach (var u in Group.Users)
                {
                    UsersInGroup.Add(u);
                }
            }
            if(parameters["shout"] != null)
            {
                ShoutFromEdit = (ShoutDto)parameters["shout"];
            }
        }



        public ObservableCollection<string> MyColours = new ObservableCollection<string>() {
                "#c62828",//red
                "#ad1457",//pink
                "#6a1b9a",//purple
                "#4527a0",//deep purple
                "#283593",//indigo
                "#1565c0",//blue
                "#0277bd",//l blue
                "#00838f",//cyyan
                "#00695c",//teal
                "#2e7d32",//green
                "#558b2f",//l green
                "#9e9d24",//yello
                "#f9a825",//lime
                "#ff8f00",//amber
                "#ef6c00",//orange
                "#d84315",//deep orange
                "#4e342e",//Brown
                "#424242",//Grey
                "#37474f",//BlueGrey
            };

    }
}
