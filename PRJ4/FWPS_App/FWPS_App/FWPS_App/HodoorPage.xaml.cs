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
	public partial class HodoorPage : ContentPage
	{
		public HodoorPage ()
		{
			InitializeComponent ();
            Timer();
            DoorStatus();
            NavigationPage.SetHasNavigationBar(this, false);
            ReturnBtn.Clicked += ReturnBtn_Clicked;
            LockBtn.Clicked += LockBtn_Clicked;
            UnlockBtn.Clicked += UnlockBtn_Clicked;
        }

        private void UnlockBtn_Clicked(object sender, EventArgs e)
        {
            string obj = HTTPRequestHandler.CreateGetRequest(HodoorUri + "newest/").ToString();
            var hodoorStateObject = JsonConvert.DeserializeObject<HodoorObject>(obj);

            if (hodoorStateObject.OpenStatus == true)
            {
                DisplayAlert("Well tried", "Hodoor is already open!", "OK");
            }
            else
            {
                UnlockBtn.IsEnabled = false;
                HodoorObject hodoorObject = new HodoorObject
                {
                    Command = "CmdUnlock",
                    IsRun = false,
                };
                HTTPRequestHandler.CreateRequest(hodoorObject, HodoorUri);
                UnlockBtn.IsEnabled = true;
            }
        }

        private void LockBtn_Clicked(object sender, EventArgs e)
        {
            string obj = HTTPRequestHandler.CreateGetRequest(HodoorUri + "newest/").ToString();
            var hodoorStateObject = JsonConvert.DeserializeObject<HodoorObject>(obj);

            if (hodoorStateObject.OpenStatus == false)
            {
                DisplayAlert("Well tried", "Hodoor is already closed!", "OK");
            }
            else
            {
                LockBtn.IsEnabled = false;
                HodoorObject hodoorObject = new HodoorObject
                {
                    Command = "CmdLock",
                    IsRun = false,
                };
                HTTPRequestHandler.CreateRequest(hodoorObject, HodoorUri);
                LockBtn.IsEnabled = true;
            }
        }

        private void Timer()
        {
            Timer timer;
            timer = new Timer();
            timer.Elapsed += (object s, ElapsedEventArgs e) => DoorStatus();
            timer.AutoReset = true;
            timer.Interval = 2000;
            timer.Start();
        }

        private void ReturnBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private void DoorStatus()
        {
            string obj = HTTPRequestHandler.CreateGetRequest(HodoorUri + "newest/").ToString();

            var hodoorObject = JsonConvert.DeserializeObject<HodoorObject>(obj);

            Device.BeginInvokeOnMainThread(() =>
            {
                if(hodoorObject.OpenStatus == false)
                {
                    doorStatus.Text = "Closed";
                }

                if(hodoorObject.OpenStatus == true)
                { 
                    doorStatus.Text = "Open";
                }
            });
        }

        public static string HodoorUri { get; set; } = "http://fwps.azurewebsites.net/api/hodoor/";

        public class HodoorObject
        {
            public string Command { get; set; }
            public bool OpenStatus { get; set; }
            public bool IsRun { get; set; }
        }
    }
}