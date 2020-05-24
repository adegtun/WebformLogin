using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;
using UserLoader.Model;

namespace UserLoader
{
    public static class XmlHelpers
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private static AuthenticationResponce ValidResponce()
        {
            AuthenticationResponce authresponce = new AuthenticationResponce();
            authresponce.isSuccessful = "true";
            authresponce.response = "Successful";
            return authresponce;
        }

        private static AuthenticationResponce InValidResponce()
        {
            AuthenticationResponce authresponce = new AuthenticationResponce();
            authresponce.isSuccessful = "false";
            authresponce.response = "Please provide basic authentication";
            return authresponce;
        }

        public static AuthenticationResponce XmlProcessor(string Uri, string xml, bool Token_Islive, string authentication)
        {
            try
            {
                //don't check the value of Token_Islive. Always use token for all validation.
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Uri);
                byte[] bytes;
                bytes = System.Text.Encoding.ASCII.GetBytes(xml);
                request.ContentType = "text/xml; encoding='utf-8'";
                request.ContentLength = bytes.Length;
                request.Method = "POST";

                request.UseDefaultCredentials = false;
                request.Proxy = new WebProxy();
                //string auth = "Basic R1VQQVk6d2F0ZXJmYWxs";
                //logger.Debug("Auth:" + auth);
                //  request.Headers.Add("Authorization", ConfigurationManager.AppSettings["Token_Authorization"].ToString());
                request.Headers.Add("Authorization", authentication);

                //Token_Islive = false; //For Testing only 
                Token_Islive = true;
                if (!Token_Islive)
                {
                    //On Test
                    return ValidResponce();
                }
                else //on Production
                {
                    Stream requestStream = request.GetRequestStream();
                    requestStream.Write(bytes, 0, bytes.Length);
                    requestStream.Close();
                    HttpWebResponse response;
                    response = (HttpWebResponse)request.GetResponse();
                    //Stream responseStream = null;
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        //try
                        //{
                        //    responseStream = response.GetResponseStream();
                        //    var esb_result = new StreamReader(responseStream).ReadToEnd();
                        //    logger.Debug("Entrust Response1:: " + esb_result);
                        //    return esb_result;
                        //}
                        //catch(Exception ex)
                        //{
                        //    logger.Error("Error reading stream result");
                        //}
                        //finally
                        //{
                        //    if (responseStream != null)
                        //    {
                        //        responseStream.Close();
                        //        responseStream = null;
                        //    }
                        //}
                        AuthenticationResponce authenticationResponce = DecodeResponse(response);
                        return authenticationResponce;
                    }
                }
                
            }
            catch (Exception ex)
            {
                logger.Debug("xmlProcessor:: ex: " + ex.Message + " StackTrace" + ex.StackTrace);
                return InValidResponce();
            }
            return null;
        }


        public static string XmlGetPayload(string xmlResult)
        {
            logger.Debug("XmlGetPayload:  " + xmlResult);
            //string newResult = WebUtility.HtmlDecode(xmlResult);
            string newResult = xmlResult;
            string retVal = newResult.Substring(newResult.IndexOf("<return>") + 8, newResult.IndexOf("</return>") - (newResult.IndexOf("<return>") + 8));
            return "<?xml version=\"1.0\" encoding=\"utf-8\"?><AuthenticationResponce>" + retVal + "</AuthenticationResponce>";
        }

        private static AuthenticationResponce DecodeResponse(HttpWebResponse response)
        {
            AuthenticationResponce ar = new AuthenticationResponce();
            try
            {
                using (XmlTextReader xtReader = new XmlTextReader(response.GetResponseStream()))
                {
                    while (xtReader.Read())
                    {
                        if (xtReader.IsStartElement())
                        {
                            switch (xtReader.Name)
                            {
                                case "response":
                                    ar.response = xtReader.ReadString();
                                    break;
                                case "isSuccessful":
                                    ar.isSuccessful = xtReader.ReadString();
                                    break;
                                case "errorResponseCode":
                                    ar.errorResponseCode = xtReader.ReadString();
                                    break;
                                case "errorResponseMessage":
                                    ar.errorResponseMessage = xtReader.ReadString();
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return ar;
        }
    }
}
