﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FWPS_App
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ClimateControlPage : ContentPage
	{
		public ClimateControlPage ()
		{
			InitializeComponent ();
            NavigationPage.SetHasNavigationBar(this, false);
            ReturnBtn.Clicked += ReturnBtn_Clicked;
        }

        private void ReturnBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}