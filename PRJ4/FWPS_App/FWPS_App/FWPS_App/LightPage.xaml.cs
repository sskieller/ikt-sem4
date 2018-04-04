using System;
using System.Text;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Net;
using System.IO;
using Newtonsoft.Json;

namespace FWPS_App
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LightPage : ContentPage
	{
		public LightPage ()
		{
			InitializeComponent ();
            OnButton.Clicked += OnButton_Clicked;
            OffButton.Clicked += OffButton_Clicked;
        }

        private void OffButton_Clicked(object sender, EventArgs e)
        {
            LightObject light = new LightObject()
            {
                Command = "off",
                IsRun = false
            };
            CreateLightRequest(light);
        }

        private void OnButton_Clicked(object sender, EventArgs e)
        {
            LightObject lightObject = new LightObject
            {
                Command = "on",
                IsRun = false
            };
            CreateLightRequest(lightObject);
        }

        public static string BaseUri { get; set; } = "https://fwps.azurewebsites.net/api/Light/";
        protected virtual void CreateLightRequest(LightObject light)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(BaseUri);
                request.Method = "POST";
                request.ContentType = "application/json";
                //request.Headers.Add("content-type", "application/json");
                ///string json = "{\"command\":\"on\"}";

                string json = JsonConvert.SerializeObject(light); //Serialize json object to string
                request.ContentLength = json.Length; //Get length of json
                Stream stream = request.GetRequestStream(); //Create stream

                mainlabel.Text = "Reached Write + " + json;
                stream.Write(Encoding.UTF8.GetBytes(json), 0, json.Length); //Write PUT request

                HttpWebResponse response = (HttpWebResponse)request.GetResponse(); //Get response to make sure json object is sent
            }
            catch (Exception e)
            {
                label2.Text = e.Message;
                string message = e.Message;
                //await DisplayAlert("DisplayAlert", $"{message}", "OK");
            }
        }


        //protected virtual void GetLightState()
        //{

        //    //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(BaseUri);
        //    //request.Method = "GET";
        //    //request.ContentType = "application/json";
        //}
        public class LightObject
        {
            public string Command { get; set; }
            public bool IsRun { get; set; }
        }
    }
}