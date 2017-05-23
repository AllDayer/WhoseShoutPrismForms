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
    public partial class GridItemTemplate : ContentView
    {

        public GridItemTemplate()
        {
            InitializeComponent();
        }

        public GridItemTemplate(object item)
        {
            InitializeComponent();
            BindingContext = item;
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
        }
        
    }
}
