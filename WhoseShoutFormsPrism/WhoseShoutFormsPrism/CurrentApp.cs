using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhoseShoutFormsPrism.ViewModels;

namespace WhoseShoutFormsPrism
{
    public class CurrentApp
    {
        private static Lazy<CurrentApp> CurrentAppInstance = new Lazy<CurrentApp>(() => new CurrentApp());

        public static CurrentApp Current => CurrentAppInstance.Value;
        
        public MainViewModel MainViewModel { get; set; }

        private CurrentApp()
        {
            MainViewModel = new MainViewModel();
        }

        public static void Startup()
        {
            //Intelledox.Model.ILogging log = ServiceLocator.Current.GetInstance<Intelledox.Model.ILogging>();
            //log.Info(typeof(CurrentApp), "Application startup");
            //AppVersion = new Version(0, 0);
            //ProduceVersion = new Version(0, 0);

            //MessageHub = new TinyMessenger.TinyMessengerHub();

            //// Context
            //AppContext = new ViewModel.AppContext();
            //AppContext.Options = new Common.Options();
        }
    }
}