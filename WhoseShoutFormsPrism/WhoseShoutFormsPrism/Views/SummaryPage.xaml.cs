using Prism.Events;
using WhoseShoutFormsPrism.Services;
using WhoseShoutFormsPrism.ViewModels;
using Xamarin.Forms;

namespace WhoseShoutFormsPrism.Views
{
    public partial class SummaryPage : ContentPage
    {
        private readonly IEventAggregator _ea;

        public SummaryPage(IEventAggregator eventAggregator)
        {
            InitializeComponent();
            _ea = eventAggregator;
            //_ea.GetEvent<GroupsLoadedEvent>().Subscribe(() => SetRepeater());
        }

        protected override void OnBindingContextChanged()
        {
            repeater.ParentVM = BindingContext;
            base.OnBindingContextChanged();
        }
        
        private void SetRepeater()
        {
            //
            //repeater.ItemsSource = ((SummaryPageViewModel)this.BindingContext).ShoutGroups;
        }

        protected override void OnDisappearing()
        {
            //_ea.GetEvent<GroupsLoadedEvent>().Unsubscribe(null);

            base.OnDisappearing();
        }
    }
}
