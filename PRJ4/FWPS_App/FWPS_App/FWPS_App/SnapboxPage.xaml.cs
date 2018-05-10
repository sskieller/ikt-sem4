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
    public partial class SnapboxPage : ContentPage
    {
        public SnapboxPage()
        {
            InitializeComponent();
            Timer();
            BatteryStatus();
            NavigationPage.SetHasNavigationBar(this, false);
            ReturnBtn.Clicked += ReturnBtn_Clicked;
        }

        private void Timer()
        {
            Timer timer;
            timer = new Timer();
            timer.Elapsed += (object s, ElapsedEventArgs e) => BatteryStatus();
            timer.AutoReset = true;
            timer.Interval = 2000;
            timer.Start();
        }

        private void ReturnBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private void BatteryStatus()
        {
            string obj = HTTPRequestHandler.CreateGetRequest(SnapboxUri + "newest/").ToString();

            var snapboxObject = JsonConvert.DeserializeObject<SnapboxObject>(obj);

            Device.BeginInvokeOnMainThread(() =>
                {
                    powerlevel.Text = snapboxObject.PowerLevel + "%";
                });
        }

        public static string SnapboxUri { get; set; } = "http://fwps.azurewebsites.net/api/snapbox/";

        public class SnapboxObject
        {
            public string SnapBoxId { get; set; }
            public string PowerLevel { get; set; }
            public bool MailReceived { get; set; }
            public string ReceiverEmail { get; set; }
            public string Checksum { get; set; }
        }
    }
}

    // Check om der er kommet mail. Gives nofikation såfremt

