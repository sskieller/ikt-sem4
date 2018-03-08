using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Net;
using System.IO;
using Newtonsoft.Json;
namespace AndroidApp
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
            ClimateControlButton.Clicked += ClimateControlButton_Clicked;
            LightButton.Clicked += LightButton_Clicked;
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
