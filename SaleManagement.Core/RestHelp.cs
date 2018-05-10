using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace SaleManagement.Core
{
    public class RestHelp
    {

        public string QueryPostRestService(String methodUrl, string Pars)
        {

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(methodUrl);
            request.Method = "Post";
            request.ContentType = "application/json;charset=utf-8";
            request.Accept = "application/json;charset=utf-8";
            byte[] postdatabtyes = Encoding.UTF8.GetBytes(Pars);
            request.ContentLength = postdatabtyes.Length;
            Stream requeststream = request.GetRequestStream();
            requeststream.Write(postdatabtyes, 0, postdatabtyes.Length);
            requeststream.Close();
            string resp;
            try
            {
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                    resp = sr.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                return "";
            }

            return resp;
        }

        private string ReadJsonResponse(WebResponse response)
        {
            StringBuilder sbSource = new StringBuilder();
            if (response != null)
            {
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                sbSource = new StringBuilder(sr.ReadToEnd());
            }

            return sbSource.ToString();
        }

        private void SetWebRequest(HttpWebRequest request)
        {
            request.Credentials = CredentialCache.DefaultCredentials;
            request.Timeout = 10000;
        }

        private void WriteRequestData(HttpWebRequest request, byte[] data)
        {
            request.ContentLength = data.Length;
            Stream writer = request.GetRequestStream();
            writer.Write(data, 0, data.Length);
            writer.Close();
        }

        private String ParsToString(Hashtable Pars)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string k in Pars.Keys)
            {
                if (sb.Length > 0)
                {
                    sb.Append("&");
                }
                sb.Append(HttpUtility.UrlEncode(k) + "=" + HttpUtility.UrlEncode(Pars[k].ToString()));
            }
            return sb.ToString();
        }

        private byte[] EncodePars(Hashtable Pars)
        {
            return Encoding.UTF8.GetBytes(ParsToString(Pars));
        }
    }
}
