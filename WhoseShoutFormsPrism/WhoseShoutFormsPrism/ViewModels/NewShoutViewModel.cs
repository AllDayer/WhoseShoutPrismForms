using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using WhoseShoutFormsPrism.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WhoseShoutFormsPrism.ViewModels
{
    public class NewShoutViewModel : BaseViewModel
    {
        public NewShoutViewModel(INavigationService navigationService)
            : base(navigationService)
        {

            Title = "New Shout";

        }
    }
}
