using System;
using System.Text;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Timers;


namespace FWPS_App
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LightPage : ContentPage
    {

        public LightPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            LightState();
            Timer();
            GetWakeUpAndSleepTimes();
            ReturnBtn.Clicked += ReturnBtn_Clicked;
            OnButton.Clicked += OnButton_Clicked;
            OffButton.Clicked += OffButton_Clicked;
            ShowStatisticsBtn.Clicked += ShowStatisticsBtn_Clicked;
            WakeUpAndSleepApplyBtn.Clicked += WakeUpAndSleepApplyBtn_Clicked;
        }

        private void ShowStatisticsBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ShowLightStatisticsPage());
        }
        private void ReturnBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private void GetWakeUpAndSleepTimes()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                string obj = HTTPRequestHandler.CreateGetRequest(LightUri + "newest/").ToString();

                var lightObject = JsonConvert.DeserializeObject<LightObject>(obj);

                hhWakeUpEditor.Text = lightObject.WakeUpTime.Hour.ToString();
                mmWakeUpEditor.Text = lightObject.WakeUpTime.Minute.ToString();
                hhSleepEditor.Text = lightObject.SleepTime.Hour.ToString();
                mmSleepEditor.Text = lightObject.SleepTime.Minute.ToString();
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


            string obj = HTTPRequestHandler.CreateGetRequest(LightUri + "newest/").ToString();

            var lightObject = JsonConvert.DeserializeObject<LightObject>(obj);

            LightObject lightWakeUpObject = new LightObject()
            {
                IsOn = lightObject.IsOn,
                Command = "UpdateTime",
                WakeUpTime = WakeUpParsedDate,
                SleepTime = SleepParsedDate
            };
            HTTPRequestHandler.CreateRequest(lightWakeUpObject, LightUri);
            WakeUpAndSleepApplyBtn.IsEnabled = true;
        }


        private void Timer()
        {
            Timer timer;
            timer = new Timer();
            timer.Elapsed += (object s, ElapsedEventArgs e) => LightState();
            timer.AutoReset = true;
            timer.Interval = 2000;
            timer.Start();
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
        }

        private void OffButton_Clicked(object sender, EventArgs e)
        {
            string obj = HTTPRequestHandler.CreateGetRequest(LightUri + "newest/").ToString();
            var lightStateObject = JsonConvert.DeserializeObject<LightObject>(obj);

            if(lightStateObject.IsOn == false)
            {
                DisplayAlert("Well tried", "Light is already turned off!", "OK");
            }
            else
            {
                OffButton.IsEnabled = false;

                LightObject lightObject = new LightObject()
                {
                    Command = "off",
                    IsRun = false,
                    IsOn = false
                };
                HTTPRequestHandler.CreateRequest(lightObject, LightUri);
                OffButton.IsEnabled = true;
            }
        }

        private void OnButton_Clicked(object sender, EventArgs e)
        {
            string obj = HTTPRequestHandler.CreateGetRequest(LightUri + "newest/").ToString();
            var lightStateObject = JsonConvert.DeserializeObject<LightObject>(obj);

            if (lightStateObject.IsOn == true)
            {
                DisplayAlert("Well tried", "Light is already turned on!", "OK");
            }
            else
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
        }

        public static string LightUri { get; set; } = "https://fwps.azurewebsites.net/api/Light/";

        public class LightObject
        {
            public string Command { get; set; }
            public bool IsRun { get; set; }
            public bool IsOn { get; set; }

            public DateTime SleepTime { get; set; }
            public DateTime WakeUpTime { get; set; }
            public DateTime CreatedDate { get; set; }
        }
    }
}