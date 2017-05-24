using DryIoc;
using Prism.DryIoc;
using WhoseShoutFormsPrism.Services;
using WhoseShoutFormsPrism.Views;
using Xamarin.Forms;

namespace WhoseShoutFormsPrism
{
    public partial class App : PrismApplication
    {
        public App(IPlatformInitializer initializer = null) : base(initializer) { }

        protected override void OnInitialized()
        {
            InitializeComponent();

            NavigationService.NavigateAsync("NavigationPage/LoginPage");
        }

        protected override void RegisterTypes()
        {
            Container.RegisterTypeForNavigation<NavigationPage>();
            Container.RegisterTypeForNavigation<MainPage>();
            Container.RegisterTypeForNavigation<LoginPage>();
            Container.RegisterTypeForNavigation<SummaryPage>();
            Container.RegisterTypeForNavigation<BuyPage>();
            Container.RegisterTypeForNavigation<ShoutGroupPage>();

            Container.Register<IAuthenticationService, AuthenticationService>(Reuse.Singleton);

        }
    }
}
