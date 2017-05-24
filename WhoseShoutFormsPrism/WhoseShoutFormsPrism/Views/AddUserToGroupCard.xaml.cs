using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhoseShoutFormsPrism.Services;
using WhoseShoutFormsPrism.ViewModels;
using WhoseShoutWebService.Models;
using Xamarin.Forms;

namespace WhoseShoutFormsPrism.Views
{
    public partial class AddUserToGroupCard : ContentView
    {
        //Should be a view model
        public ShoutGroupPageViewModel ShoutGroupVM { get; set; }
        public String BGColor { get; set; }
        public int Index { get; set; }
        //


        public AddUserToGroupCard()
        {
            InitializeComponent();
        }
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            this.Remove.Clicked += Remove_Clicked;
        }

        private void Remove_Clicked(object sender, EventArgs e)
        {
            ShoutGroupVM.RemoveUserCommand.Execute(Index);
        }
    }
}
