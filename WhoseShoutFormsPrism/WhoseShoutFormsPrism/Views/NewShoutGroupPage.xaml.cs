using Prism.Events;
using WhoseShoutFormsPrism.Services;
using WhoseShoutFormsPrism.ViewModels;
using Xamarin.Forms;

namespace WhoseShoutFormsPrism.Views
{
    public partial class NewShoutGroupPage : ContentPage
    {
        private readonly IEventAggregator _ea;

        public NewShoutGroupPage(IEventAggregator eventAggregator)
        {
            InitializeComponent();
            _ea = eventAggregator;
            //_ea.GetEvent<UserAddedToGroupEvent>().Subscribe(() => SetRepeater());
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
        }

        private void SetRepeater()
        {
            repeater.ParentVM = BindingContext;
            //repeater.ItemsSource = ((NewShoutGroupPageViewModel)BindingContext).UsersInGroup;
        }

        protected override void OnDisappearing()
        {
            _ea.GetEvent<UserAddedToGroupEvent>().Unsubscribe(null);

            base.OnDisappearing();
        }
    }
}
