using System;
using System.IO;
using System.Net;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HttpClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

			LightControl control = new LightControl();

	        while (true)
	        {
		        LightObject light = control.GetRequest("Next").Result;
		        if (light != null)
		        {
			        Console.WriteLine("Light command to execute: \"" +  light.Command + "\" with id: " + light.Id);

			        control.HandleLightCommand(light);
					
					
		        }
				Thread.Sleep(500);
			}




			
        }


	
	}

	public class LightControl
	{
		public static string BaseUri { get; set; } = "https://fwps.azurewebsites.net/api/Light/";
		public async Task<LightObject> GetRequest(string uriAppend)
		{
			
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(BaseUri + uriAppend);
			request.Method = "Get";
			request.KeepAlive = false;
			request.ContentType = "application/json";
			request.Headers.Add("Content-Type", "application/json");

			//REMEMBER ERROR HANDLING CODE IN CASE OBJECT DOES NOT EXIST

			try
			{
				HttpWebResponse response =  (HttpWebResponse) await request.GetResponseAsync();
				string myResponse = "";
				using (System.IO.StreamReader sr = new System.IO.StreamReader(response.GetResponseStream()))
				{
					myResponse = sr.ReadToEnd();
				}

				return JsonConvert.DeserializeObject<LightObject>(myResponse);
			}
			catch (WebException e)
			{
				return null;
			}

		}



		public void HandleLightCommand(LightObject light)
		{
			switch (light.Command)
			{
				case "on":
					HandleOnCommand(light);
					break;
				case "off":
					HandleOffCommand(light);
					break;
				default:
					HandleUnknownCommand(light);
					break;
			}

			UpdateLightRequest(light);
			
		}

		protected virtual void HandleUnknownCommand(LightObject light)
		{
			Console.WriteLine("Handler: Unknown command");
			light.IsRun = true;
		}

		protected virtual void HandleOnCommand(LightObject light)
		{
			Console.WriteLine("Handler: Light on");
			light.IsRun = true;
		}

		protected virtual void HandleOffCommand(LightObject light)
		{
			Console.WriteLine("Handler: Light off");
			light.IsRun = true;
		}

		protected virtual async void UpdateLightRequest(LightObject light)
		{
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(BaseUri + light.Id);
			request.Method = "Put";
			request.KeepAlive = false;
			request.ContentType = "application/json";
			request.Headers.Add("Content-Type", "application/json");

			string json = JsonConvert.SerializeObject(light); //Serialize json object to string
			request.ContentLength = json.Length; //Get length of json
			Stream stream = request.GetRequestStream(); //Create stream

			stream.Write(Encoding.UTF8.GetBytes(json), 0, json.Length); //Write PUT request


			
			HttpWebResponse response = (HttpWebResponse) await request.GetResponseAsync(); //Get response to make sure json object is sent




			
		}


	}

	public class LightObject
	{
		public string Command { get; set; }
		public bool IsRun { get; set; }
		public DateTime CreatedDate { get; set; }
		public DateTime LastModifiedDate { get; set; }
		public int Id { get; set; }

	}



}
