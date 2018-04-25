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
        ContentPage lightPage, snapboxPage, hodoorPage, poombaPage, climateControlPage;

        public MainPage()
		{
			InitializeComponent();
            MakePages();
            NavigationPage.SetHasNavigationBar(this, false);
            ClimateControlButton.Clicked += ClimateControlButton_Clicked;
            LightButton.Clicked += LightButton_Clicked;
            PoombaButton.Clicked += PoombaButton_Clicked;
            HodoorButton.Clicked += HodoorButton_Clicked;
            LogoutButton.Clicked += LogoutButton_Clicked;
            SnapboxButton.Clicked += SnapboxButton_Clicked;

        }

        public void MakePages()
        {
            lightPage = new LightPage();
            snapboxPage = new SnapboxPage();
            hodoorPage = new HodoorPage();
            poombaPage = new PoombaPage();
            climateControlPage = new ClimateControlPage();
        }

        #region PagesPushAsync
        private void LightButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(lightPage);
        }

        private void SnapboxButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(snapboxPage);
        }

        private void HodoorButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(hodoorPage);
        }

        private void PoombaButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(poombaPage);
        }

        private void ClimateControlButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(climateControlPage);
        }
        #endregion

        private void LogoutButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

    }
}
