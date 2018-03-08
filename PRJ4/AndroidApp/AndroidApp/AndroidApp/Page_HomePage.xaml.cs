using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FWPS
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Page_HomePage : ContentPage
	{
		public Page_HomePage()
		{
			InitializeComponent();
            ClimateControlButton.Clicked += ClimateControlButton_Clicked;
            LightButton.Clicked += LightButton_Clicked;
            PoombaButton.Clicked += PoombaButton_Clicked;
            HodoorButton.Clicked += HodoorButton_Clicked;
        }

        private void HodoorButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Page_Hodoor());
        }

        private void PoombaButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Page_Poomba());
        }

        private void LightButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Page_Light());
        }

        private void ClimateControlButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Page_Climate());
        }
    }
}