using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FWPS_App
{
    /////////////////////////////////////////////////
    /// Interface to implement a tool to verify
    /// credentials.
    /////////////////////////////////////////////////
    internal interface IVerifyCredentials
    {
        bool VerifyUsernameExists(string usrname);
        bool VerifyPassword(string usrname, string pskword);
    }

    /////////////////////////////////////////////////
    /// Interface to implement a hashing algorithm
    /////////////////////////////////////////////////
    internal interface IHasher
    {
        string Hash(string stringToBeHashed);
    }

    /////////////////////////////////////////////////
    /// Sha256 implementation of the IHaster interface
    /////////////////////////////////////////////////
    internal class Sha256Hasher : IHasher
    {
        private readonly SHA256 _sha256;

        /////////////////////////////////////////////////
        /// 
        /////////////////////////////////////////////////
        internal Sha256Hasher()
        {
            _sha256 = SHA256.Create();
        }

        /////////////////////////////////////////////////
        /// Computates the SHA2 - 256 hash of
        /// stringToBeHashed
        /////////////////////////////////////////////////
        public string Hash(string stringToBeHashed)
        {
            var data = _sha256.ComputeHash(Encoding.UTF8.GetBytes(stringToBeHashed));

            StringBuilder sb = new StringBuilder();

            foreach (var t in data)
            {
                sb.Append(t.ToString("x2"));
            }

            return sb.ToString();
        }
    }

    /////////////////////////////////////////////////
    /// 
    /////////////////////////////////////////////////
    internal class VerifyLogin : IVerifyCredentials
    {
        private static string _baseUri;

        /////////////////////////////////////////////////
        /// 
        /////////////////////////////////////////////////
        internal VerifyLogin(string baseUri)
        {
            _baseUri = baseUri;
        }

        /////////////////////////////////////////////////
        /// Currently not used. Checks if the username
        /// exists in the database
        /////////////////////////////////////////////////
        public bool VerifyUsernameExists(string usrname)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_baseUri + usrname);
                request.Method = "GET";

                HttpWebResponse response = (HttpWebResponse)request.GetResponse(); //Get response to make sure json object is sent

                Console.WriteLine("Status code returned: {0}", response.StatusCode);
                return true;
            }
            catch (Exception e)
            {
                string message = e.Message;
                Console.WriteLine("DisplayAlert {0}, OK", 1);
                Console.WriteLine(message);
                return false;
            }
        }

        /////////////////////////////////////////////////
        /// Validates and verifys the given username and
        /// password.
        /// TODO: implement a login token.
        /////////////////////////////////////////////////
        public bool VerifyPassword(string usrname, string pskword)
        {
            if (usrname == string.Empty || pskword == string.Empty) return false;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_baseUri + usrname + ';' + pskword);
                request.Method = "GET";

                HttpWebResponse response = (HttpWebResponse)request.GetResponse(); // Get response to check the returnvalue

                string myResponse;

                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    myResponse = sr.ReadToEnd();
                }

                return response.StatusCode == HttpStatusCode.OK && myResponse == "LoginOk";
            }
            catch (Exception e)
            {
                string message = e.Message;
                Console.WriteLine("DisplayAlert {0}, OK", 1);
                Console.WriteLine(message);
                return false;
            }
        }
    }

    /////////////////////////////////////////////////
    /// Class to wrap the login process. 
    /////////////////////////////////////////////////
    public class LogIn
    {
        private readonly IVerifyCredentials _verify;

        private readonly IHasher _hasher;

        /////////////////////////////////////////////////
        /// Sets up constructor. Takes a base URI to
        /// route the login requests.
        /////////////////////////////////////////////////
        public LogIn(string apiUrl)
        {
            _verify = new VerifyLogin(apiUrl);
            _hasher = new Sha256Hasher();
        }

        /////////////////////////////////////////////////
        /// Tries to login. Will hash the given password
        /// with the _hasher interface. 
        /////////////////////////////////////////////////
        public bool Login(string usrname, string pskword)
        {
            usrname = usrname?.Trim();

            pskword = pskword?.Trim();
            
            if (!_verify.VerifyPassword(usrname, _hasher.Hash(pskword)))
                return false;

            return true;

        }
    }
}