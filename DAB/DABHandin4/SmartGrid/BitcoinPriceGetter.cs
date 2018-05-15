using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using Newtonsoft.Json.Linq;

namespace SmartGrid
{
    public static class BitcoinPriceGetter
    {
        public static float GetPrice()
        {
            var webapi = new WebApiConnector();


            var item = webapi.GetItem();

            //JArray arr = JArray.Parse(item);
            JObject obj = JObject.Parse(item);

            var value = float.Parse(obj["bpi"]["USD"]["rate_float"].ToString());

            return value * (float) (Math.Pow(10, -5) * 3.59);
            //Console.WriteLine(value);
        }
        public class WebApiConnector
        {
            //private static HTTPRequestHandler _instance;

            public static string BaseUri { get; set; } = "https://api.coindesk.com/v1/bpi/currentprice.json";

            public WebApiConnector()
            {
            }
            public string GetItem()
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(BaseUri);
                request.Method = "Get";
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
        }
    }
}