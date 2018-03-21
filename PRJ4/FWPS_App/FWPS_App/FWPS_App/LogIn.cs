using System;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FWPS_App
{
    internal interface IVerifyCredentials
    {
        bool VerifyUsernameExists(string usrname);
        bool VerifyPassword(string usrname, string pskword);
    }

    internal interface IHasher
    {
        string Hash(string stringToBeHashed);
    }

    internal class Sha256Hasher : IHasher
    {
        private readonly SHA256 _sha256;

        internal Sha256Hasher()
        {
            _sha256 = SHA256.Create();
        }

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

    internal class VerifyLogin : IVerifyCredentials
    {
        private static string _baseUri;

        internal VerifyLogin(string baseUri)
        {
            _baseUri = baseUri;
        }

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

        public bool VerifyPassword(string usrname, string pskword)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_baseUri + usrname + ';' + pskword);
                request.Method = "GET";

                HttpWebResponse response = (HttpWebResponse)request.GetResponse(); // Get response to check the returnvalue

                Console.WriteLine("Status code returned: {0}", response.StatusCode);
                if (response.StatusCode == HttpStatusCode.OK)
                    Console.WriteLine("IT GUD BRO");
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
    }

    public class LogIn
    {
        private readonly IVerifyCredentials _verify;

        private readonly IHasher _hasher;


        public LogIn(string apiUrl)
        {
            _verify = new VerifyLogin(apiUrl);
            _hasher = new Sha256Hasher();
        }


        public async Task<bool> Login(string usrname, string pskword)
        {
            usrname = usrname?.Trim();

            pskword = pskword?.Trim();

            if (!_verify.VerifyPassword(usrname, _hasher.Hash(pskword)))
                return false;

            return true;

        }
    }
}