using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using NLog;

namespace UserLoader
{
    public static class Utility
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public static string Base64Encode(string plainText)
        {
            logger.Debug("Plain text:" + plainText);
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        public static string XmlGetPayload(string xmlResult)
        {
            if (xmlResult == null) return null;
            //string newResult = WebUtility.HtmlDecode(xmlResult);
            string newResult = xmlResult;
            newResult = newResult.Replace("<![CDATA[", "");
            newResult = newResult.Replace("]]>", "");
            string retVal = newResult.Substring(newResult.IndexOf("<return>") + 8, newResult.IndexOf("</return>") - (newResult.IndexOf("<return>") + 8));
            return retVal;
        }

        public static string XmlProcessor(string Uri, string xml)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Uri);
                byte[] bytes;
                bytes = System.Text.Encoding.ASCII.GetBytes(xml);
                request.ContentType = "text/xml; encoding='utf-8'";
                request.ContentLength = bytes.Length;
                request.Method = "POST";

                // request.Headers.Add("Authorization", ConfigurationManager.AppSettings["Authorization"].ToString());

                request.Proxy = new WebProxy();
                request.UseDefaultCredentials = false;

                Stream requestStream = request.GetRequestStream();
                requestStream.Write(bytes, 0, bytes.Length);
                requestStream.Close();
                HttpWebResponse response;
                response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Stream responseStream = response.GetResponseStream();
                    return new StreamReader(responseStream).ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "\n" + ex.StackTrace);
            }
            return null;
        }

        public static string GetBaseURL()
        {
            var url = ConfigurationManager.AppSettings["baseURL"];
            return url;
        }
    }
}
