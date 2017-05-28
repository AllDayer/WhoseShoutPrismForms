using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhoseShoutFormsPrism.Helpers;
using WhoseShoutFormsPrism.Services;
using WhoseShoutFormsPrism.ViewModels;
using WhoseShoutWebService.Models;
using Xamarin.Forms;

namespace WhoseShoutFormsPrism.Views
{
    public partial class ShoutSummaryGroupCard : ContentView
    {
        public SummaryPageViewModel SummaryVM { get; set; }
        public String BGColour { get; set; }
        
        public ShoutSummaryGroupCard()
        {
            InitializeComponent();
        }
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            card.BackgroundColor = Color.FromHex(BGColour);
            var tap = new TapGestureRecognizer();
            tap.Tapped += (s, e) =>
            {
                var args = new BuyRoundArgs() { Group = (ShoutGroupDto)this.BindingContext };
                SummaryVM.OnBuyCommandExecuted(args);
            };
            card.GestureRecognizers.Add(tap);
            //WebImage.Source = Settings.Current.AvatarUrl;
            //circle2.Source = Settings.Current.AvatarUrl;

            Random r = new Random();
            if (r.Next(2) % 2 == 0)
            {
                categoryImage.Source = "ic_food_croissant_white_48dp.png";
            }
            //BuyRound.Clicked += BuyRound_Clicked;
        }

        private void BuyRound_Clicked(object sender, EventArgs e)
        {
        }
    }
}
