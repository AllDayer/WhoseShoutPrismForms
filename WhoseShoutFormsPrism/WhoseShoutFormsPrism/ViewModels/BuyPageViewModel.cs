using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using Prism.Navigation;
using WhoseShoutFormsPrism.Models;

namespace WhoseShoutFormsPrism.ViewModels
{
    public class BuyPageViewModel : BaseViewModel
    {
        INavigationService m_NavigationService;
        private Shout m_Shout = new Shout();

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



        public async void OnBuyCommandExecuted()
        {
            m_Shout.PurchaseTimeUtc = DateTime.UtcNow;
            //Save sync item
            //Sync with server

            //Return
            m_NavigationService.GoBackAsync();
        }

        public override void OnNavigatedFrom(NavigationParameters parameters)
        {
        }

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            m_Shout = (Shout)parameters["model"];
            RaisePropertyChanged(nameof(GroupID));
            RaisePropertyChanged(nameof(ID));
        }

        public override void OnNavigatingTo(NavigationParameters parameters)
        {
        }
    }
}
