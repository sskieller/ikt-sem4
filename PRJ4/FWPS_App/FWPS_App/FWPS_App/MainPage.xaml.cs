using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FWPS_App
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            ClimateControlButton.Clicked += ClimateControlButton_Clicked;
            LightButton.Clicked += LightButton_Clicked;
            PoombaButton.Clicked += PoombaButton_Clicked;
            HodoorButton.Clicked += HodoorButton_Clicked;

        }

        private void HodoorButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new HodoorPage());
        }

        private void PoombaButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new PoombaPage());
        }

        private void LightButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new LightPage());
        }

        private void ClimateControlButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ClimateControlPage());
        }
    }
}
