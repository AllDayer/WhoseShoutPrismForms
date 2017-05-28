using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhoseShoutFormsPrism.Helpers;
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

        protected override View ViewFor(object vm, object parentVM, string bgColour, int i)
        {
            return new AddUserToGroupCard() { ShoutGroupVM = (ShoutGroupPageViewModel)parentVM, BindingContext = vm, Index = i };
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

        protected override View ViewFor(object vm, object parentVM, string bgColour, int i)
        {
            var colour = bgColour;
            if (CurrentApp.Current.MainViewModel.GroupColourDictionary.ContainsKey(((ShoutGroupDto)vm).ID))
            {
                colour = CurrentApp.Current.MainViewModel.GroupColourDictionary[((ShoutGroupDto)vm).ID];
            }
            return new ShoutSummaryGroupCard() { SummaryVM = (SummaryPageViewModel)parentVM, BGColour = colour, BindingContext = vm };
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

        protected virtual View ViewFor(object vm, object parent, string bgColour, int index)
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
                    var view = control.ViewFor(item, control.ParentVM, CurrentApp.Current.MainViewModel.RandomColour(), i);
                    if (view != null)
                    {
                        view.Opacity = 0;
                        control.Children.Add(view);

                        view.FadeTo(1, 300, Easing.CubicIn);
                    }
                    i++;
                }

                this.UpdateChildrenLayout();
                this.InvalidateLayout();
            }

            if(sender is ObservableCollection<T>)
            {
                if(((ObservableCollection<T>)sender).Count == 0)
                {
                    var control = this as RepeaterView<T>;
                    control.Children.Clear();
                }
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

                    var view = control.ViewFor(t, control.ParentVM, CurrentApp.Current.MainViewModel.RandomColour(), i);
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
    }
}