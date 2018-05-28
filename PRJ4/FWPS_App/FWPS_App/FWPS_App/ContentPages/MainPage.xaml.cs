/**
 * @file MainPage.xaml.cs
 * @author 
 * @date 25 may 2018
 * @brief This file initiates Content Pages and which through button event handlers can be navigated to
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;



namespace FWPS_App
{
    /// <summary>
    /// Content Page which acts main page for the app 
    /// </summary>
    public partial class MainPage : ContentPage
	{
        /// <summary>
        /// Pages that can be reached through touch events
        /// </summary>
        ContentPage lightPage, snapboxPage, hodoorPage, poombaPage, climateControlPage;

        /// <summary>
        /// Initialisations
        /// </summary>
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


        /// <summary>
        /// Assignation of Pages
        /// </summary>
        public void MakePages()
        {
            lightPage = new LightPage();
            snapboxPage = new SnapboxPage();
            hodoorPage = new HodoorPage();
            poombaPage = new PoombaPage();
            climateControlPage = new ClimateControlPage();
        }

        #region PagesPushAsync
        /// <summary>
        /// Button event handler that redirects to LightPage which shows content for MorningSun 
        /// </summary>
        private void LightButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(lightPage);
        }


        /// <summary>
        /// Button event handler that redirects to LightPage which shows content for MorningSun 
        /// </summary>
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
