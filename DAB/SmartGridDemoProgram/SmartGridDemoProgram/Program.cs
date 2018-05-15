using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SmartGrid.Data;

namespace SmartGridDemoProgram
{
    class Program
    {
        static void Main(string[] args)
        {
            var prosumers = new List<Prosumer>();

            prosumers.Add(new Prosumer("House1", "House2"));
            prosumers.Add(new Prosumer("House2", "House3"));
            prosumers.Add(new Prosumer("House3", "House4"));
            prosumers.Add(new Prosumer("House4", "House5"));
            prosumers.Add(new Prosumer("House5", "House1"));


            var api = new WebApiConnector();

            api.PostItem("api/Prosumers", JsonConvert.SerializeObject(prosumers));

            var str = api.GetItem("api/Prosumers");

            Console.WriteLine(str);
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
