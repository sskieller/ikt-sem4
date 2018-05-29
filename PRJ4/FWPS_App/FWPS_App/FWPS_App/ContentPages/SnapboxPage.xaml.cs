using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plugin.Toasts;
using Plugin.Toasts.Options;

namespace FWPS_App
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    /////////////////////////////////////////////////
    /// Content Page for Snapbox 
    /////////////////////////////////////////////////
    public partial class SnapboxPage : ContentPage
    {
        private bool _notificationSent;
        private bool _powerNotificationSent;

        /////////////////////////////////////////////////
        /// Initialisations 
        /////////////////////////////////////////////////
        public SnapboxPage()
        {
            InitializeComponent();
            Timer();
            NavigationPage.SetHasNavigationBar(this, false);
            ReturnBtn.Clicked += ReturnBtn_Clicked;
            ShowStatisticsBtn.Clicked += ShowStatisticsBtn_Clicked;
        }

        /////////////////////////////////////////////////
        /// Button event handler that redirects to
        /// statistics for Snapbox
        /////////////////////////////////////////////////
        private void ShowStatisticsBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ShowStatisticsPage());
        }

        /////////////////////////////////////////////////
        /// Timerfunction for polling every 2sec
        /////////////////////////////////////////////////
        private void Timer()
        {
            Timer timer;
            timer = new Timer();
            timer.Elapsed += (object s, ElapsedEventArgs e) => BatteryStatus();
            timer.Elapsed += (object s, ElapsedEventArgs e) => MailStatus();
            timer.AutoReset = true;
            timer.Interval = 2000;
            timer.Start();
        }

        /////////////////////////////////////////////////
        /// Button event handler that redirects to
        /// Mainpage which shows content for 
        /// mainpage
        /////////////////////////////////////////////////
        private void ReturnBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        /////////////////////////////////////////////////
        /// Loads current battery powerlevel to be 
        /// displayed in app. If powerlevel reach 15% 
        /// or lower make nofitication
        /////////////////////////////////////////////////
        private void BatteryStatus()
        {
            string obj = HTTPRequestHandler.CreateGetRequest(SnapboxUri + "newest/").ToString();

            var snapboxObject = JsonConvert.DeserializeObject<SnapboxObject>(obj);

            Device.BeginInvokeOnMainThread(() =>
                {
                    powerlevel.Text = snapboxObject.PowerLevel + "%";

                    if (int.Parse(snapboxObject.PowerLevel) <= 15 && _powerNotificationSent == false)
                    {
                        _powerNotificationSent = true;
                        // Make nofitication if powerlevel is less than 15%
                        var toastclass = new ToastClass();

                        toastclass.ShowToast(new NotificationOptions()
                        {
                            Title = "Low battery",
                            Description = "Your snapbox powerlevel has reached 15%!",
                            IsClickable = true,
                            WindowsOptions = new WindowsOptions() { LogoUri = "icon.png" },
                            ClearFromHistory = false,
                            AllowTapInNotificationCenter = false,
                            AndroidOptions = new AndroidOptions()
                            {
                                HexColor = "#F99D1C",
                                ForceOpenAppOnNotificationTap = true
                            }
                        });
                    }
                    else if (int.Parse(snapboxObject.PowerLevel) > 15)
                    {
                        _powerNotificationSent = false;
                    }
                });
        }

        /////////////////////////////////////////////////
        /// Loads mail status to be displayed in app. 
        /// If mail is recieved after Snapbox has been
        /// emptied make nofitication
        /////////////////////////////////////////////////
        private void MailStatus()
        {

            string obj = HTTPRequestHandler.CreateGetRequest(SnapboxUri + "newest/").ToString();

            var snapboxObject = JsonConvert.DeserializeObject<SnapboxObject>(obj);

            Device.BeginInvokeOnMainThread(() =>
            {
                if (snapboxObject.MailReceived == true)
                {
                    mailStatusLbl.Text = "You got mail";
                }
                if (snapboxObject.MailReceived == false)
                {
                    _notificationSent = false;
                    mailStatusLbl.Text = "Your snapbox is empty";
                }
            });

            if (snapboxObject.MailReceived == true && _notificationSent == false)
            {
                // Make nofitication if mail is received
                _notificationSent = true;

                var toastclass = new ToastClass();
                
                toastclass.ShowToast(new NotificationOptions()
                {
                    Title = "You got mail!",
                    Description = "Go to your snapbox to pick your snailmail",
                    IsClickable = true,
                    WindowsOptions = new WindowsOptions() { LogoUri = "icon.png" },
                    ClearFromHistory = false,
                    AllowTapInNotificationCenter = false,
                    AndroidOptions = new AndroidOptions()
                    {
                        HexColor = "#F99D1C",
                        ForceOpenAppOnNotificationTap = true
                    }
                });
            }
        }

        public static string SnapboxUri { get; set; } = "http://fwps.azurewebsites.net/api/snapbox/";

        /////////////////////////////////////////////////
        /// Snapbox object class
        /////////////////////////////////////////////////
        public class SnapboxObject
        {
            public string SnapBoxId { get; set; }
            public string PowerLevel { get; set; }
            public bool MailReceived { get; set; }
            public string ReceiverEmail { get; set; }
            public string Checksum { get; set; }
            public DateTime CreatedDate { get; set; }
        }
    }
}

