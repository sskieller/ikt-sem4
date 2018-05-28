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
            GetWakeUpAndSleepTimes();
            NavigationPage.SetHasNavigationBar(this, false);
            ReturnBtn.Clicked += ReturnBtn_Clicked;
            OnButton.Clicked += OnButton_Clicked;
            OffButton.Clicked += OffButton_Clicked;
            WakeUpAndSleepApplyBtn.Clicked += WakeUpAndSleepApplyBtn_Clicked;
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
        private void GetWakeUpAndSleepTimes()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                string obj = HTTPRequestHandler.CreateGetRequest(PoombaUri + "newest/").ToString();
                var poombaObject = JsonConvert.DeserializeObject<PoombaObject>(obj);

                hhWakeUpEditor.Text = poombaObject.WakeUpTime.Hour.ToString();
                mmWakeUpEditor.Text = poombaObject.WakeUpTime.Minute.ToString();
                hhSleepEditor.Text = poombaObject.SleepTime.Hour.ToString();
                mmSleepEditor.Text = poombaObject.SleepTime.Minute.ToString();
            });
        }

        private void WakeUpAndSleepApplyBtn_Clicked(object sender, EventArgs e)
        {
            WakeUpAndSleepApplyBtn.IsEnabled = false;
            // The four editor fiels has to hold integer values
            if (!int.TryParse((hhWakeUpEditor.Text), out int something))
            {
                DisplayAlert("Wrong input", "Field(s) either empty or wrong input type. Please try again", "OK");
                hhWakeUpEditor.Text = "";
                WakeUpAndSleepApplyBtn.IsEnabled = true;
                return;
            }

            if (!int.TryParse((mmWakeUpEditor.Text), out int something2))
            {
                DisplayAlert("Wrong input", "Field(s) either empty or wrong input type. Please try again", "OK");
                mmWakeUpEditor.Text = "";
                WakeUpAndSleepApplyBtn.IsEnabled = true;
                return;
            }

            if (!int.TryParse((hhSleepEditor.Text), out int something3))
            {
                DisplayAlert("Wrong input", "Field(s) either empty or wrong input type. Please try again", "OK");
                hhSleepEditor.Text = "";
                WakeUpAndSleepApplyBtn.IsEnabled = true;
                return;
            }

            if (!int.TryParse((mmSleepEditor.Text), out int something4))
            {
                DisplayAlert("Wrong input", "Field(s) either empty or wrong input type. Please try again", "OK");
                mmSleepEditor.Text = "";
                WakeUpAndSleepApplyBtn.IsEnabled = true;
                return;
            }


            var hhWakeUpInput = Double.Parse(hhWakeUpEditor.Text);
            var mmWakeUpInput = Double.Parse(mmWakeUpEditor.Text);
            var hhSleepInput = Double.Parse(hhSleepEditor.Text);
            var mmSleepInput = Double.Parse(mmSleepEditor.Text);

            // Check if inputs are within ranges
            if (hhWakeUpInput < 0 || hhWakeUpInput > 23)
            {
                DisplayAlert("Wrong input", "Input(s) out of range. Please try again", "OK");
                hhWakeUpEditor.Text = "";
                WakeUpAndSleepApplyBtn.IsEnabled = true;
                return;
            }

            if (mmWakeUpInput < 0 || mmWakeUpInput > 59)
            {
                DisplayAlert("Wrong input", "Input(s) out of range. Please try again", "OK");
                mmWakeUpEditor.Text = "";
                WakeUpAndSleepApplyBtn.IsEnabled = true;
                return;
            }

            if (hhSleepInput < 0 || hhSleepInput > 23)
            {
                DisplayAlert("Wrong input", "Input(s) out of range. Please try again", "OK");
                hhSleepEditor.Text = "";
                WakeUpAndSleepApplyBtn.IsEnabled = true;
                return;
            }

            if (mmSleepInput < 0 || mmSleepInput > 59)
            {
                DisplayAlert("Wrong input", "Input(s) out of range. Please try again", "OK");
                mmSleepEditor.Text = "";
                WakeUpAndSleepApplyBtn.IsEnabled = true;
                return;
            }


            DateTime WakeUpParsedDate = new DateTime();
            WakeUpParsedDate = DateTime.Today;
            WakeUpParsedDate = WakeUpParsedDate.AddHours(hhWakeUpInput);
            WakeUpParsedDate = WakeUpParsedDate.AddMinutes(mmWakeUpInput);

            DateTime SleepParsedDate = new DateTime();
            SleepParsedDate = DateTime.Today;
            SleepParsedDate = SleepParsedDate.AddHours(hhSleepInput);
            SleepParsedDate = SleepParsedDate.AddMinutes(mmSleepInput);


            string obj = HTTPRequestHandler.CreateGetRequest(PoombaUri + "newest/").ToString();
            var poombaObject = JsonConvert.DeserializeObject<PoombaObject>(obj);

            PoombaObject poombaWakeUpObject = new PoombaObject()
            {
                IsOn = poombaObject.IsOn,
                Command = "UpdateTime",
                WakeUpTime = WakeUpParsedDate,
                SleepTime = SleepParsedDate
            };
            HTTPRequestHandler.CreateRequest(poombaWakeUpObject, PoombaUri);
            WakeUpAndSleepApplyBtn.IsEnabled = true;
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