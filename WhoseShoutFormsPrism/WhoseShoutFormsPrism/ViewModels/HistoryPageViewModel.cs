using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using WhoseShoutWebService.Models;

namespace WhoseShoutFormsPrism.ViewModels
{
    public class HistoryPageViewModel : BaseViewModel
    {
        INavigationService m_NavigationService;
        private ShoutGroupDto m_ShoutGroup = new ShoutGroupDto();

        public ShoutGroupDto ShoutGroup
        {
            get
            {
                return m_ShoutGroup;
            }
            set
            {
                m_ShoutGroup = value;
                RaisePropertyChanged(nameof(ShoutGroup));
            }
        }

        public HistoryPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "History";
            m_NavigationService = navigationService;

        }

        public override void OnNavigatedFrom(NavigationParameters parameters)
        {
        }


        public override void OnNavigatedTo(NavigationParameters parameters)
        {

        }

        public override void OnNavigatingTo(NavigationParameters parameters)
        {
            ShoutGroup = (ShoutGroupDto)parameters["group"];
        }
    }
}
