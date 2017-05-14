using System;
using System.Collections.Generic;
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

    public class GroupRepeaterView  : RepeaterView<ShoutGroupDto>
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
        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource), typeof(IEnumerable<T>), typeof(RepeaterView<T>), Enumerable.Empty<T>(), BindingMode.OneWay, null, ItemsChanged);
        public static readonly BindableProperty ParentVMProperty = BindableProperty.Create(nameof(ParentVM), typeof(object), typeof(object), null, BindingMode.OneWay);

        public RepeaterView()
        {
            Spacing = 0;
        }

        public IEnumerable<T> ItemsSource
        {
            get { return (IEnumerable<T>)GetValue(ItemsSourceProperty); }
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

        private static void ItemsChanged(
            BindableObject bindable,
            object oldValue,
            object newValue)
        {
            var control = bindable as RepeaterView<T>;
            if (control == null)
            {
                throw new Exception(
                    "Invalid bindable object passed to RepeaterView::ItemsChanged expected a RepeaterView<T> received a "
                    + bindable.GetType().Name);
            }

            if (newValue != null)
            {
                foreach (var t in ((IEnumerable<T>)newValue).Where(x => x != null))
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