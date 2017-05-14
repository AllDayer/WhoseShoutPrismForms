using System;

using Xamarin.Forms;

namespace WhoseShoutFormsPrism.Controls
{
    public class CardView : Frame
    {
        public CardView()
        {
            Padding = 10;
            Margin = 10;
            //if (Device.OS == TargetPlatform.iOS)
            {
                HasShadow = true;
                OutlineColor = Color.Transparent;
                BackgroundColor = Color.Transparent;
            }
        }
    }
}
