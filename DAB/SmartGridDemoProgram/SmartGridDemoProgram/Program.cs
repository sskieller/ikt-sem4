using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SmartGridDemoProgram
{
    class Program
    {

        static void Main(string[] args)
        {
            var prosumers = new List<Prosumer>();

            // 33 Houses
            for (int i = 0; i < 33; i++)
            {
                prosumers.Add(ProsumerFactory.Create("House"));
            }

            // 12 Business's
            for (int i = 0; i < 12; i++)
            {
                prosumers.Add(ProsumerFactory.Create("Business"));
            }
            // Setting Starters.
            prosumers.Find(p => p.Name == "House1").PreferedBuyer = "House33";
            prosumers.Find(p => p.Name == "Business1").PreferedBuyer = "Business12";
            

            var api = new WebApiConnector();

            api.PostItem("api/Prosumers", JsonConvert.SerializeObject(prosumers));
            
            Console.WriteLine("Press 'q' to quit -- 'u' to Update all -- 't' to view transaction");
            
            while (true)
            {
                var key = Console.Read();

                if(key == 'q') Environment.Exit(0);
                else if (key == 'u')
                {
                    Console.WriteLine("Updating...");
                    foreach(var pro in prosumers)
                        pro.Update();

                    api.PostItem("api/Prosumers", JsonConvert.SerializeObject(prosumers));
                    Console.WriteLine("Done updating!");
                }
            }
        }
    }

    public class WebApiConnector
    {
        //private static HTTPRequestHandler _instance;

        public static string BaseUri { get; set; } = "http://localhost:51202/";

        public WebApiConnector()
        {
        }
        public string GetItem(string uri)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(BaseUri + uri);
            request.Method = "GET";
            request.KeepAlive = false;

            //REMEMBER ERROR HANDLING CODE IN CASE OBJECT DOES NOT EXIST

            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                string myResponse = "";
                using (System.IO.StreamReader sr = new System.IO.StreamReader(response.GetResponseStream()))
                {
                    myResponse = sr.ReadToEnd();
                }

                return myResponse;
            }
            catch (WebException e)
            {
                Console.WriteLine(e.Response);
                return null;
            }
        }

        public string PostItem(string uri, string json)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(BaseUri + uri);
            request.Method = "POST";
            request.ContentType = "application/json";

            //REMEMBER ERROR HANDLING CODE IN CASE OBJECT DOES NOT EXIST

            try
            {

                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(request.GetRequestStream()))
                {
                    sw.Write(json);
                    sw.Flush();
                    sw.Close();
                }

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                

                string myResponse = "";

                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    myResponse = sr.ReadToEnd();
                }
                return myResponse;
            }
            catch (WebException e)
            {
                Console.WriteLine(e.Response);
                return null;
            }
        }
    }
}
