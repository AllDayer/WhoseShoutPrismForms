using Prism.Commands;
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

        public ObservableCollection<ShoutUserDto> UsersForShout = new ObservableCollection<ShoutUserDto>();

        public BuyPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            m_NavigationService = navigationService;
            BuyCommand = new DelegateCommand(OnBuyCommandExecuted);
        }

        public DelegateCommand BuyCommand { get; }

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

        private ShoutGroupDto m_Group;

        public async void OnBuyCommandExecuted()
        {
            m_Shout.PurchaseTimeUtc = DateTime.UtcNow;
            m_Shout.ShoutUserID = UserDto.ID;
            m_Shout.ShoutGroupID = new Guid(GroupID);
            m_Shout.Cost = (float.Parse(Cost));

            //Save sync item
            //Sync with server
            await CurrentApp.Current.MainViewModel.ServiceApi.NewShout(m_Shout);

            //Return
            m_NavigationService.GoBackAsync();
        }

        public override void OnNavigatedFrom(NavigationParameters parameters)
        {
        }

        
        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            m_Shout = (ShoutDto)parameters["model"];
            RaisePropertyChanged(nameof(GroupID));
            RaisePropertyChanged(nameof(ID));

            foreach (var u in ((List<ShoutUserDto>)parameters["users"]))
            {
                UsersForShout.Add(u);
            }
            RaisePropertyChanged("UserName");
        }

        public override void OnNavigatingTo(NavigationParameters parameters)
        {

        }
    }
}
