using NLog;
using System;
using System.Configuration;
using System.IO;
using System.Xml.Serialization;
using UserLoader.Model;

namespace UserLoader
{
    public class EntrustRP
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public AuthenticationResponce TokenAuthenticate(EntrustRequest _param)
        {
            try
            {
                logger.Debug("Entrust validation started");
                string Url = ConfigurationManager.AppSettings["Token_ENDPOINT"].ToString();
                bool Token_Islive = Convert.ToBoolean(ConfigurationManager.AppSettings["Token_Islive"]);
                string sb = TokenRequest.TokenRequestPayload; // File.ReadAllText(HttpContext.Current.Server.MapPath("~/XML/TokenAuthRequest.xml"));
                sb = string.Format(sb, _param.response, _param.userGroup, _param.username, _param.requesterId, _param.requesterIp);

                string authentication = ConfigurationManager.AppSettings["upm_Authentication"].ToString();
                AuthenticationResponce result = XmlHelpers.XmlProcessor(Url, sb, Token_Islive, authentication);
                logger.Debug("AuthenticationService::TokenAuthenticate::Responce:: " + result.isSuccessful);
                //using (StringReader stringreader = new StringReader(XmlHelpers.XmlGetPayload(result)))
                //{
                //    var serializer = new XmlSerializer(typeof(AuthenticationResponce));
                //    var s = (AuthenticationResponce)serializer.Deserialize(stringreader);
                //    return s;
                //}
                
                return result;
                

            }
            catch (Exception ex)
            {
                logger.Error(ex.StackTrace.ToString());
                throw new Exception(ex.Message + "\n" + ex.StackTrace);
            }
        }
    }
}
