using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using WhoseShoutFormsPrism.Droid.Renderers;
using WhoseShoutFormsPrism.Controls;

[assembly: ExportRenderer(typeof(CustomImageButton), typeof(CustomImageButtonRenderer))]
namespace WhoseShoutFormsPrism.Droid.Renderers
{
    class CustomImageButtonRenderer : ButtonRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Button> e)
        {
            base.OnElementChanged(e);
        }
    }
}