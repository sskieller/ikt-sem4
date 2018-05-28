using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FWPS_App
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PoombaPage : ContentPage
	{
		public PoombaPage ()
		{
			InitializeComponent ();
            Timer();
            NavigationPage.SetHasNavigationBar(this, false);
            ReturnBtn.Clicked += ReturnBtn_Clicked;
            OnButton.Clicked += OnButton_Clicked;
            OffButton.Clicked += OffButton_Clicked;
        }

        private void Timer()
        {
            Timer timer;
            timer = new Timer();
            timer.Elapsed += (object s, ElapsedEventArgs e) => PoombaState();
            timer.AutoReset = true;
            timer.Interval = 2000;
            timer.Start();
        }

        private void PoombaState()
        {

            string obj = HTTPRequestHandler.CreateGetRequest(PoombaUri + "newest/").ToString();
            var poombaObject = JsonConvert.DeserializeObject<PoombaObject>(obj);

            if (poombaObject.IsOn == true)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    poombaStateLabel.Text = "Poomba is ON";
                });
            }
            else if (poombaObject.IsOn == false)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    poombaStateLabel.Text = "Poomba is OFF";
                });
            }
            else
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    poombaStateLabel.Text = "Something went much wrong";
                });
            }
        }

        private void OffButton_Clicked(object sender, EventArgs e)
        {
            string obj = HTTPRequestHandler.CreateGetRequest(PoombaUri + "newest/").ToString();
            var poombaStateObject = JsonConvert.DeserializeObject<PoombaObject>(obj);

            if (poombaStateObject.IsOn == false)
            {
                DisplayAlert("Well tried", "Poomba is already turned off!", "OK");
            }
            else
            {
                OffButton.IsEnabled = false;

                PoombaObject poombaObject = new PoombaObject()
                {
                    Command = "off",
                    IsRun = false,
                    IsOn = false
                };
                HTTPRequestHandler.CreateRequest(poombaObject, PoombaUri);
                OffButton.IsEnabled = true;
            }
        }

        private void OnButton_Clicked(object sender, EventArgs e)
        {
            string obj = HTTPRequestHandler.CreateGetRequest(PoombaUri + "newest/").ToString();
            var poombaStateObject = JsonConvert.DeserializeObject<PoombaObject>(obj);

            if (poombaStateObject.IsOn == true)
            {
                DisplayAlert("Well tried", "Poomba is already turned on!", "OK");
            }
            else
            {
                OnButton.IsEnabled = false;
                PoombaObject poombaObject = new PoombaObject()
                {
                    Command = "on",
                    IsRun = false,
                    IsOn = true
                };
                HTTPRequestHandler.CreateRequest(poombaObject, PoombaUri);
                OnButton.IsEnabled = true;
            }
        }

        private void ReturnBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        public static string PoombaUri { get; set; } = "http://fwps.azurewebsites.net/api/poomba/";

        public class PoombaObject
        {
            public string Command { get; set; }
            public bool IsRun { get; set; }
            public bool IsOn { get; set; }
            public DateTime WakeUpTime { get; set; }
            public DateTime SleepTime { get; set; }
            public DateTime CleaningTime { get; set; }
        }
    }
}