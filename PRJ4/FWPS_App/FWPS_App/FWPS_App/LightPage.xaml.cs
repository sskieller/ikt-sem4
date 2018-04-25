using System;
using System.Text;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Timers;
using System.Threading;

namespace FWPS_App
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LightPage : ContentPage
	{
        System.Timers.Timer timer;
        public LightPage ()
		{
			InitializeComponent ();
            NavigationPage.SetHasNavigationBar(this, false);
            LightState();
            OnButton.Clicked += OnButton_Clicked;
            OffButton.Clicked += OffButton_Clicked;
            ReturnBtn.Clicked += ReturnBtn_Clicked;
            timer = new System.Timers.Timer();
            timer.Elapsed += (object s, ElapsedEventArgs e) => LightState();
            timer.AutoReset = true;
            timer.Interval = 2000;
            timer.Start();
        }

        private void ReturnBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private void LightState()
        {

            string obj = HTTPRequestHandler.CreateGetRequest(LightUri + "newest/").ToString();

            var lightObject = JsonConvert.DeserializeObject<LightObject>(obj);


            if (lightObject.IsOn == true)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    lightStateLabel.Text = "Light is ON";
                });
            }
            else if (lightObject.IsOn == false)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    lightStateLabel.Text = "Light is OFF";
                });
            }
            else
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    lightStateLabel.Text = "Something went much wrong";
                });
            }

            //Device.BeginInvokeOnMainThread(() =>
            //{
            //    mainlabel.Text = obj;
            //});
        }

        private void OffButton_Clicked(object sender, EventArgs e)
        {

                OnButton.IsEnabled = false;

                LightObject lightObject = new LightObject()
                {
                    Command = "off",
                    IsRun = false,
                    IsOn = false
                };
            HTTPRequestHandler.CreateRequest(lightObject, LightUri);
            OnButton.IsEnabled = true;
            
        }

        private void OnButton_Clicked(object sender, EventArgs e)
        {
            OnButton.IsEnabled = false;
            LightObject lightObject = new LightObject
            {
                Command = "on",
                IsRun = false,
                IsOn = true
            };
            HTTPRequestHandler.CreateRequest(lightObject, LightUri);
            OnButton.IsEnabled = true;
        }

        public static string LightUri { get; set; } = "https://fwps.azurewebsites.net/api/Light/";

        public class LightObject
        {
            public string Command { get; set; }
            public bool IsRun { get; set; }
            public bool IsOn { get; set; }

            public DateTime SleepTime { get; set; }
            public DateTime WakeUpTime { get; set; }
        }
    }
}   