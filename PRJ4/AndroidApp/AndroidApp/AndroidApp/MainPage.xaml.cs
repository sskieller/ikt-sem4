using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Net;
using System.IO;
using Newtonsoft.Json;

namespace AndroidApp
{
	public partial class MainPage : ContentPage
	{
        
        public MainPage()
		{
			InitializeComponent();
            ButtonClicked.Clicked += ButtonClick_Clicked;
        }

	    private void OnButtonOnClicked(object sender, EventArgs e)
        {

            LightObject light = new LightObject
            {
                Command = "on",
                IsRun = false
            };

            CreateLightRequest(light);


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



                HttpWebResponse response = (HttpWebResponse) request.GetResponse(); //Get response to make sure json object is sent
            }
            catch (Exception e)
            {

                label2.Text = e.Message;
                string message = e.Message;
                //await DisplayAlert("DisplayAlert", $"{message}", "OK");
            }
        }



    }
    public class LightObject
    {
        [JsonProperty]
        public string Command { get; set; }
        [JsonProperty]
        public bool IsRun { get; set; }


    }
}
