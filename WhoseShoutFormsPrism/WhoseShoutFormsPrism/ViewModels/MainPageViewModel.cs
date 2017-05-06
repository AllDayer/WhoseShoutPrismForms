﻿using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using WhoseShoutFormsPrism.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WhoseShoutFormsPrism.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        public MainPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = "Home Page";
            Settings.Current.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == nameof(Settings.UserName))
                    UserName = Settings.Current.UserName;
            };

            NavigateCommand = new DelegateCommand<string>(OnNavigateCommandExecuted);
        }

        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set { SetProperty(ref _userName, value); }
        }

        public DelegateCommand<string> NavigateCommand { get; }

        private async void OnNavigateCommandExecuted(string path)
        {
            await _navigationService.NavigateAsync(path);
        }

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            UserName = Settings.Current.UserName;
        }
    }
}
