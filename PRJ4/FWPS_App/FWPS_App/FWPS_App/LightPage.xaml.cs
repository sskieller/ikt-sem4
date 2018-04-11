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
	    public static string LightUri { get; set; } = "https://fwps.azurewebsites.net/api/Light/";

        //Timer timer;
        public LightPage ()
		{
			InitializeComponent ();
            OnButton.Clicked += OnButton_Clicked;
            OffButton.Clicked += OffButton_Clicked;
		    SignalRClient.Instance.OnLightChanged += LightChangedHandler;
		    //timer = new Timer();
		    //timer.Elapsed += (object s, ElapsedEventArgs e) => GetLightState();
		    //timer.AutoReset = true;
		    //timer.Interval = 1000;
		    //timer.Start();


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


	    public void LightChangedHandler(object obj, LightEventArgs e)
	    {
	        lightStateLabel.Text = e.State ? "Light is on" : "Light is off";
	    }


	    //protected void GetLightState()
     //   {

     //       HttpWebRequest request = (HttpWebRequest)WebRequest.Create(BaseUri + "Newest/");
     //       request.Method = "GET";
     //       request.KeepAlive = false;
     //       request.ContentType = "application/json";
     //       //request.Headers.Add("content-type", "application/json");
            
     //       try
     //       {
     //           HttpWebResponse response = (HttpWebResponse)  request.GetResponse();
     //           string myResponse = "";
     //           using (StreamReader sr = new StreamReader(response.GetResponseStream()))
     //           {
     //               myResponse = sr.ReadToEnd();
     //           }

     //           LightObject lightobject = JsonConvert.DeserializeObject<LightObject>(myResponse);

     //           if (lightobject.IsOn == true)
     //           {
     //               lightStateLabel.Text = "Light is on";

     //           }
     //           else if(lightobject.IsOn == false)
     //           {
     //               lightStateLabel.Text = "Light is off";
     //           }
     //           else
     //           {
     //               lightStateLabel.Text = "Something went much wrong";
     //           }
                
     //       }
     //       catch (WebException e)
     //       {
     //           Console.WriteLine(e.Message);
     //       }

     //   }



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