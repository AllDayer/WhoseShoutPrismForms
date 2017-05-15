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
            this.VerticalOptions = LayoutOptions.StartAndExpand;
            m_ViewModels = new List<BaseViewModel>();
        }

        protected override View ViewFor(object vm, object parentVM)
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

        protected override View ViewFor(object vm, object parentVM)
        {
            return new ShoutSummaryGroupCard() { SummaryVM = (SummaryPageViewModel)parentVM, BindingContext = vm };
        }
    }

    public class RepeaterView<T> : StackLayout where T : class
    {
        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource), typeof(ObservableCollection<T>), typeof(RepeaterView<T>), new ObservableCollection<T>(), BindingMode.OneWay, null, ItemsChanged);
        public static readonly BindableProperty ParentVMProperty = BindableProperty.Create(nameof(ParentVM), typeof(object), typeof(object), null, BindingMode.OneWay);

        public RepeaterView()
        {
            Spacing = 0;
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

        protected virtual View ViewFor(object vm, object parent)
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

                foreach (T item in e.NewItems)
                {

                    var view = control.ViewFor(item, control.ParentVM);
                    if (view != null)
                    {
                        control.Children.Add(view);
                    }
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
            control.ItemsSource.CollectionChanged += control.ItemsSource_CollectionChanged;
            
            if (control == null)
            {
                throw new Exception(
                    "Invalid bindable object passed to RepeaterView::ItemsChanged expected a RepeaterView<T> received a "
                    + bindable.GetType().Name);
            }

            if (newValue != null)
            {
                foreach (var t in ((ObservableCollection<T>)newValue).Where(x => x != null))
                {
                    var view = control.ViewFor(t, control.ParentVM);
                    if (view != null)
                    {
                        control.Children.Add(view);
                    }
                }
            }
        }
    }
}