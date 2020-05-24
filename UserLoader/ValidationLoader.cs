using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Users;
using DotNetNuke.Security.Membership;
using System.Runtime.InteropServices;
using System.Web;
using GSS.AppServices.Model.Data;
using System.Configuration;
using UserLoader.Model;
using System.Net;
using System.IO;
using System.Xml;


namespace UserLoader
{
    public class ValidationLoader
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        

        public string[] VerifyUser(string username, string password, string tokenValue, int portalID, string portalName, string hostName)
        {
            logger.Info("validation started");
            string[] result = new string[2];
            string staffid = string.Empty;
            UserInfo userInfo = null;
            User user = null;
            try
            {
                user = User.Login(username, password);
                userInfo = UserController.GetUserByName(portalID, user.Username);
            }
            catch (Exception ex)
            {
                logger.Debug("Exception on User.Login");
                logger.Error(ex.StackTrace.ToString());
                //throw;
            }
            Employee employee;
            //string finacleID = "";
            if (userInfo == null)
            {
                //this.Navigate(user.Username, returnUrl);
                //Logger.Info("Hello123");
                result[0] = "You're not authorized to use this application.";
                result[1] = "";
                return result;
            }
            else
            {
                string finacleid = "";
                staffid = user.EmployeeNumber.ToString().Trim();
                string usernametrim = user.Email.Trim().ToLower().Replace("@ubagroup.com", "");
                ProcessManager.WorkItems.Util.AD_Data da = new ProcessManager.WorkItems.Util.AD_Data();
                da.ActiveDirectoryWrapper();
                da.PopulateUserDataStruct(da.GetUserDirectoryEntryDetails(usernametrim));
                ProcessManager.WorkItems.Util.AD_Data.ApplicationUserData user_data = da.userData;
                string countrycode = string.Empty;
                string solid = string.Empty;
                logger.Info("Usermail: " + user_data.personEmail);
                try
                {
                    countrycode = (user_data.personLocation.ToString().Substring(0, 3).ToString()).ToUpper();
                }
                catch (Exception)
                {
                    countrycode = "NGA";
                }
                try
                {
                    solid = (user_data.personLocation.ToString().Substring(4, 4).ToString()).ToUpper();
                }
                catch (Exception)
                {
                    solid = "0999";
                }
                getlivefinacleid(staffid, countrycode);
                

                try
                {
                    //Get user details by mail
                    DataAccess dataAccess = new DataAccess();
                    employee = dataAccess.GetEmployeeDetails(username);
                    finacleid = employee.FinacleID;
                    logger.Debug("Finacle ID:" + employee.FinacleID);
                }
                catch(Exception ex)
                {
                    logger.Error("User does not exist");
                    logger.Error("Data access:"+ex.ToString());
                }
                
                if ((finacleid.Trim() == string.Empty) || (finacleid.Trim() == null))
                {
                    result[0] = "Invalid Finacle User for " + staffid.Trim() + "!";
                    result[1] = "";
                    return result;
                }
                else
                {
                    var entrustReq = new EntrustRequest
                    {
                        requesterId = "",
                        requesterIp = "",
                        response = tokenValue,
                        userGroup = ConfigurationManager.AppSettings["Token_UserGroup"],
                        username = finacleid
                    };
                    EntrustRP entrust = new EntrustRP();
                    var entrust_response = entrust.TokenAuthenticate(entrustReq);
                    logger.Debug("Entrust value:" + entrust_response.isSuccessful);
                    if (entrust_response.isSuccessful == "true")
                    {
                        UserController.UserLogin(portalID, userInfo, portalName, hostName, false);
                        result[0] = "true";
                        result[1] = solid;
                    }                        
                    else
                    {
                        result[0] = "Invalid login attempt";
                        result[1] = "";
                    }
                    logger.Info("Result is {0} and {1}", result[0], result[1]);
                    return result;
                        
                }

            }
           
            //Session["SolID"] = solid;
            //return usernametrim+":"+solid+":"+countrycode;
            //return staffid;
        }

        public void MyMethod()
        {
            string url = "http://localhost:2018/EmployeeService.asmx";
            HttpWebRequest webreq = (HttpWebRequest)WebRequest.Create(url);
            webreq.ContentType = "text/xml; charset=utf-8";
            //webreq.Accept = "text/xml";
            webreq.Headers.Clear();
            webreq.Method = "POST";
            Encoding encode = Encoding.GetEncoding("utf-8");
            HttpWebResponse webres = null;
            webres = (HttpWebResponse)webreq.GetResponse();
            Stream reader = null;
            reader = webres.GetResponseStream();
            StreamReader sreader = new StreamReader(reader, encode, true);
            string result = sreader.ReadToEnd();
            logger.Info(result);
        }
        private string getlivefinacleid(string staffid, string countrycode)
        {
            string finacleid = string.Empty;
            FinacleService.FinacleValidation serv = new FinacleService.FinacleValidation();
            finacleid = serv.GetLIVEFinacleID(staffid, countrycode);
            logger.Info("Hello" + finacleid);
            return finacleid;
            
        }

    }
}
