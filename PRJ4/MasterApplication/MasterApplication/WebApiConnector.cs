using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using MasterApplication.Models;
using Newtonsoft.Json;

namespace MasterApplication
{
    /////////////////////////////////////////////////
    /// Handles the REST calls made to the Web Api.
    /////////////////////////////////////////////////
    public class WebApiConnector
    {
	    public static string BaseUri { get; set; } = "https://fwps.azurewebsites.net/api/"; //!< BaseURI for the WebApi

		public WebApiConnector()
	    {
	    }

        /////////////////////////////////////////////////
        /// Gets an item from the WebApi
        /////////////////////////////////////////////////
        public string GetItem(string uri)
	    {
		    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(BaseUri + uri);
		    request.Method = "Get";
		    request.KeepAlive = false;
		    request.ContentType = "application/json";
		    request.Headers.Add("Content-Type", "application/json");

		    //REMEMBER ERROR HANDLING CODE IN CASE OBJECT DOES NOT EXIST

		    try
		    {
			    HttpWebResponse response = (HttpWebResponse) request.GetResponse();
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

        /////////////////////////////////////////////////
        /// Post an item to the WebApi
        /////////////////////////////////////////////////
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
