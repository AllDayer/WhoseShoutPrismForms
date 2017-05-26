using Prism.Events;
using WhoseShoutFormsPrism.Services;
using WhoseShoutFormsPrism.ViewModels;
using Xamarin.Forms;

namespace WhoseShoutFormsPrism.Views
{
    public partial class ShoutGroupPage : ContentPage
    {
        private readonly IEventAggregator _ea;

        public ShoutGroupPage(IEventAggregator eventAggregator)
        {
            InitializeComponent();
            _ea = eventAggregator;

            //_ea.GetEvent<UserAddedToGroupEvent>().Subscribe(() => SetRepeater());
        }

        protected override void OnBindingContextChanged()
        {
            repeater.ParentVM = BindingContext;
            base.OnBindingContextChanged();
            GridColours.IsVisible = false;
            GridColours.Opacity = 0;

            //var tapGestureRecognizer = new TapGestureRecognizer
            //{
            //    Command = ((ShoutGroupPageViewModel)BindingContext).ClickCommand,
            //    CommandParameter = "123",
            //    NumberOfTapsRequired = 1,
            //};

            //Hello.GestureRecognizers.Add(tapGestureRecognizer);
            if (BindingContext != null)
            {
                ((ShoutGroupPageViewModel)BindingContext).PropertyChanged += ShoutGroupPage_PropertyChanged;
            }
        }

        private void ShoutGroupPage_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ShowColours")
            {
                if (((ShoutGroupPageViewModel)sender).ShowColours)
                {
                    GridColours.IsVisible = true;
                    GridColours.FadeTo(1, 300, Easing.CubicIn);
                }
                else
                {
                    var animation = new Animation(v => GridColours.Opacity = v, 1, 0);
                    animation.Commit(this, "FadeColours", 16, 250, Easing.CubicOut, (v,c) => this.GridColours.IsVisible = false);
                    //GridColours.FadeTo(0, 300, Easing.CubicOut);
                    //GridColours.IsVisible = false;
                }
            }
        }

        private void SetRepeater()
        {
            repeater.ParentVM = BindingContext;
            //repeater.ItemsSource = ((ShoutGroupPageViewModel)BindingContext).UsersInGroup;
        }

        protected override void OnDisappearing()
        {
            _ea.GetEvent<UserAddedToGroupEvent>().Unsubscribe(null);

            base.OnDisappearing();
        }
    }
}
