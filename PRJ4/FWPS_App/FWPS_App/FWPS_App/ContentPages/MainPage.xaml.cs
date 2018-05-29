using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;



namespace FWPS_App
{
    /////////////////////////////////////////////////
    /// Content Page which acts main page for the app 
    /////////////////////////////////////////////////
    public partial class MainPage : ContentPage
	{
        ContentPage lightPage, snapboxPage, hodoorPage, poombaPage, climateControlPage, lazyCurtainPage;

        /////////////////////////////////////////////////
        /// Initialisations 
        /////////////////////////////////////////////////
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
            LazyCurtainButton.Clicked += LazyCurtainButton_Clicked;
        }


        /////////////////////////////////////////////////
        /// Make pages
        /////////////////////////////////////////////////
        public void MakePages()
        {
            lightPage = new LightPage();
            snapboxPage = new SnapboxPage();
            hodoorPage = new HodoorPage();
            poombaPage = new PoombaPage();
            climateControlPage = new ClimateControlPage();
            lazyCurtainPage = new LazyCurtainPage();
        }

        #region PagesPushAsync

        /////////////////////////////////////////////////
        /// Button event handler that redirects to
        /// LightPage which shows content for MorningSun
        /////////////////////////////////////////////////
        private void LightButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(lightPage);
        }

        /////////////////////////////////////////////////
        /// Button event handler that redirects to
        /// SnapboxPage which shows content for 
        /// Snapbox
        /////////////////////////////////////////////////
        private void SnapboxButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(snapboxPage);
        }

        /////////////////////////////////////////////////
        /// Button event handler that redirects to
        /// LazyCurtainPage which shows content for 
        /// LazyCurtain
        /////////////////////////////////////////////////
        private void LazyCurtainButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(lazyCurtainPage);
        }

        /////////////////////////////////////////////////
        /// Button event handler that redirects to
        /// HodoorPage which shows content for 
        /// Hodoor
        /////////////////////////////////////////////////
        private void HodoorButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(hodoorPage);
        }

        /////////////////////////////////////////////////
        /// Button event handler that redirects to
        /// PoombaPage which shows content for 
        /// Poomba
        /////////////////////////////////////////////////
        private void PoombaButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(poombaPage);
        }

        /////////////////////////////////////////////////
        /// Button event handler that redirects to
        /// ClimateControlPage which shows content for 
        /// ClimateControl
        /////////////////////////////////////////////////
        private void ClimateControlButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(climateControlPage);
        }
        #endregion

        /////////////////////////////////////////////////
        /// Button event handler that redirects to
        /// HomePage which shows content for 
        /// homepage from where the user can login
        /////////////////////////////////////////////////
        private void LogoutButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

    }
}
