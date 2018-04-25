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
            timer.Interval = 3000;
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
                    lightStateLabel.Text = "Light is on";
                });
            }
            else if (lightObject.IsOn == false)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    lightStateLabel.Text = "Light is off";
                });
            }
            else
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    lightStateLabel.Text = "Something went much wrong";
                });
            }

            Device.BeginInvokeOnMainThread(() =>
            {
                mainlabel.Text = obj;
            });
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
        //protected virtual void CreateLightRequest(LightObject light)
        //{
        //    try
        //    {
        //        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(BaseUri);
        //        request.Method = "POST";
        //        request.ContentType = "application/json";
        //        //request.Headers.Add("content-type", "application/json");
        //        ///string json = "{\"command\":\"on\"}";

        //        string json = JsonConvert.SerializeObject(light); //Serialize json object to string
        //        request.ContentLength = json.Length; //Get length of json
        //        Stream stream = request.GetRequestStream(); //Create stream

        //        mainlabel.Text = "Reached Write + " + json;
        //        stream.Write(Encoding.UTF8.GetBytes(json), 0, json.Length); //Write PUT request

        //        HttpWebResponse response = (HttpWebResponse)request.GetResponse(); //Get response to make sure json object is sent
        //    }
        //    catch (Exception e)
        //    {
        //        label2.Text = e.Message;
        //        string message = e.Message;
        //        //await DisplayAlert("DisplayAlert", $"{message}", "OK");
        //    }


        //}

        //protected void GetLightState()
        //{

        //    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(BaseUri + "Newest/");
        //    request.Method = "GET";
        //    request.KeepAlive = false;
        //    request.ContentType = "application/json";
        //    //request.Headers.Add("content-type", "application/json");
            
        //    try
        //    {
        //        HttpWebResponse response = (HttpWebResponse)  request.GetResponse();
        //        string myResponse = "";
        //        using (StreamReader sr = new StreamReader(response.GetResponseStream()))
        //        {
        //            myResponse = sr.ReadToEnd();
        //        }

        //        LightObject lightobject = JsonConvert.DeserializeObject<LightObject>(myResponse);

        //        if (lightobject.IsOn == true)
        //        {
        //            lightStateLabel.Text = "Light is on";

        //        }
        //        else if(lightobject.IsOn == false)
        //        {
        //            lightStateLabel.Text = "Light is off";
        //        }
        //        else
        //        {
        //            lightStateLabel.Text = "Something went much wrong";
        //        }
                
        //    }
        //    catch (WebException e)
        //    {
        //        Console.WriteLine(e.Message);
        //    }

        //}



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