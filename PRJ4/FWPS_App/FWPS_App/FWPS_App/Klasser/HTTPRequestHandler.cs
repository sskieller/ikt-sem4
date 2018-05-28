using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FWPS_App
{
    static class HTTPRequestHandler
    {
        public static void CreateRequest(object obj, string BaseUri)
        {
            Task.Factory.StartNew(() => _CreatePostRequest(obj, BaseUri));
        }

        public static object CreateGetRequest(string BaseUri)
        {
            return _GetObject(BaseUri);
        }

        private static Task _CreatePostRequest(object obj, string uri)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                request.Method = "POST";
                request.ContentType = "application/json";

                string json = JsonConvert.SerializeObject(obj); //Serialize json object to string
                request.ContentLength = json.Length; //Get length of json
                Stream stream = request.GetRequestStream(); //Create stream
                
                stream.Write(Encoding.UTF8.GetBytes(json), 0, json.Length); //Write PUT request

                HttpWebResponse response = (HttpWebResponse)request.GetResponse(); //Get response to make sure json object is sent
            }
            catch (Exception e)
            {
                string message = e.Message;
                return Task.CompletedTask;
            }

            return Task.CompletedTask;
        }


        private static string _GetObject(string uri)
        {
                string myResponse = string.Empty;

                try
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                    request.Method = "GET";
                    request.KeepAlive = false;
                    request.ContentType = "application/json";
                    //request.Headers.Add("content-type", "application/json");

                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                    using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                    {
                        myResponse = sr.ReadToEnd();
                    }
                }

                catch (WebException e)
                {
                    string message = e.Message;

                    return myResponse;
                }

                return myResponse;
        }
    }
}

