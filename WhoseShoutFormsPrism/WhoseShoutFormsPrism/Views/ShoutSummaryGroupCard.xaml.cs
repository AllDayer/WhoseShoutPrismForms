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
    public partial class ShoutSummaryGroupCard : ContentView
    {
        public SummaryPageViewModel SummaryVM { get; set; }
        
        public ShoutSummaryGroupCard()
        {
            InitializeComponent();
        }
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            var tap = new TapGestureRecognizer();
            tap.Tapped += (s, e) =>
            {
                var args = new BuyRoundArgs() { Group = (ShoutGroupDto)this.BindingContext };
                SummaryVM.OnBuyCommandExecuted(args);
            };
            circle.GestureRecognizers.Add(tap);
            //BuyRound.Clicked += BuyRound_Clicked;
        }

        private void BuyRound_Clicked(object sender, EventArgs e)
        {
        }
    }
}
