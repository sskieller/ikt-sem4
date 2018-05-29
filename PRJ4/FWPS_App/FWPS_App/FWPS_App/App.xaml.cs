﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace FWPS_App
{
    /////////////////////////////////////////////////
    /// Initialisation of app
    /////////////////////////////////////////////////
    public partial class App : Application
	{
		public App ()
		{
			InitializeComponent();

			// MainPage = new FWPS_App.MainPage();
            MainPage = new NavigationPage(new HomePage());
        }

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
