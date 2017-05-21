using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhoseShoutFormsPrism.ViewModels;
using WhoseShoutFormsPrism.Views;
using WhoseShoutWebService.Models;
using Xamarin.Forms;

namespace WhoseShoutFormsPrism.Controls
{
    public class UserRepeaterView : RepeaterView<ShoutUserDto>
    {
        private List<BaseViewModel> m_ViewModels;

        public UserRepeaterView()
        {
            this.HorizontalOptions = LayoutOptions.FillAndExpand;
            this.VerticalOptions = LayoutOptions.Start;
            m_ViewModels = new List<BaseViewModel>();
        }

        protected override View ViewFor(object vm, object parentVM, string bgColour)
        {
            return new AddUserToGroupCard() { ShoutGroupVM = (NewShoutGroupPageViewModel)parentVM, BindingContext = vm };
        }
    }

    public class GroupRepeaterView : RepeaterView<ShoutGroupDto>
    {
        private List<BaseViewModel> m_ViewModels;

        public GroupRepeaterView()
        {
            this.HorizontalOptions = LayoutOptions.FillAndExpand;
            this.VerticalOptions = LayoutOptions.StartAndExpand;
            m_ViewModels = new List<BaseViewModel>();
        }

        protected override View ViewFor(object vm, object parentVM, string bgColour)
        {
            return new ShoutSummaryGroupCard() { SummaryVM = (SummaryPageViewModel)parentVM, BGColour = bgColour, BindingContext = vm };
        }
    }

    public class RepeaterView<T> : StackLayout where T : class
    {
        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource), typeof(ObservableCollection<T>), typeof(RepeaterView<T>), new ObservableCollection<T>(), BindingMode.OneWay, null, ItemsChanged);
        public static readonly BindableProperty ParentVMProperty = BindableProperty.Create(nameof(ParentVM), typeof(object), typeof(object), null, BindingMode.OneWay);

        public RepeaterView()
        {
            Spacing = 10;
        }

        public ObservableCollection<T> ItemsSource
        {
            get { return (ObservableCollection<T>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public Object ParentVM
        {
            get { return GetValue(ParentVMProperty); }
            set { SetValue(ParentVMProperty, value); }
        }

        protected virtual View ViewFor(object vm, object parent, string bgColour)
        {
            return null;
        }

        void ItemsSource_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                this.Children.RemoveAt(e.OldStartingIndex);
                this.UpdateChildrenLayout();
                this.InvalidateLayout();
            }

            if (e.NewItems != null)
            {
                var control = this as RepeaterView<T>;
                int i = 0;
                foreach (T item in e.NewItems)
                {

                    var view = control.ViewFor(item, control.ParentVM, GetColour(i));
                    if (view != null)
                    {
                        control.Children.Add(view);
                    }
                    i++;
                }

                this.UpdateChildrenLayout();
                this.InvalidateLayout();
            }
        }

        private static void ItemsChanged(
            BindableObject bindable,
            object oldValue,
            object newValue)
        {

            var control = bindable as RepeaterView<T>;
            control.Children.Clear();
            control.ItemsSource.CollectionChanged += control.ItemsSource_CollectionChanged;

            if (control == null)
            {
                throw new Exception(
                    "Invalid bindable object passed to RepeaterView::ItemsChanged expected a RepeaterView<T> received a "
                    + bindable.GetType().Name);
            }

            if (newValue != null)
            {
                int i = 0;
                foreach (var t in ((ObservableCollection<T>)newValue).Where(x => x != null))
                {

                    var view = control.ViewFor(t, control.ParentVM, GetColour(i));
                    if (view != null)
                    {
                        //view.BackgroundColor = Color.FromHex("#2980b9");
                        //view.BackgroundColor = Color.Red;
                        control.Children.Add(view);
                    }
                    i++;
                }
            }
        }

        private static string GetColour(int i)
        {
            Random r = new Random();
            //string colour = "#000000";
            //if (i % 2 == 0)
            //{
            //    colour = "#279371";
            //}
            //else
            //{
            //    colour = "#434cba";
            //}

            List<string> Colour = new List<string>() {
                "#c62828",//red
                "#ad1457",//pink
                "#6a1b9a",//purple
                "#4527a0",//deep purple
                "#283593",//indigo
                "#1565c0",//blue
                "#0277bd",//l blue
                "#00838f",//cyyan
                "#00695c",//teal
                "#2e7d32",//green
                "#558b2f",//l green
                "#9e9d24",//yello
                "#f9a825",//lime
                "#ff8f00",//amber
                "#ef6c00",//orange
                "#d84315",//deep orange
                "#4e342e",//Brown
                "#424242",//Grey
                "#37474f",//BlueGrey
            };


            return Colour[r.Next(19)];
        }
    }
}