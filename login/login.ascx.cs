using System;
using System.Configuration;
using System.Runtime.InteropServices;
using System.Web;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Users;
using DotNetNuke.Security.Membership;
using GSS.AppServices.Model.Data;
using UserLoader;

namespace GSS.AppServices.WebUI.Security
{
    public partial class Login : PortalModuleBase
    {
        private const string USER_NAME_COOKIE = "ACB06F465EBB41ef941370FB6CADCBBD";

        /* protected void Page_Load(object sender, EventArgs e)
        {
             try
            {
                if (this.IsPostBack == false)
                {
                    HttpCookie usernameCookie = this.Request.Cookies[USER_NAME_COOKIE];

                    if (usernameCookie != null)
                    {
                        this.userNameTextBox.Text = usernameCookie.Value;
                    }
                }
            }
            catch (Exception)
            {
                throw;
             }
        }*/

        private string UserName
        {
            get
            {
                string username = this.userNameTextBox.Text.Trim().ToLower();
                return username;
            }
        }

        private string Domain
        {
            get
            {
                string domain = ConfigurationManager.AppSettings["domain"];
                return domain.ToLower();
            }
        }

        private string DomainExtension
        {
            get
            {
                string extension = ConfigurationManager.AppSettings["domain_extension"];
                return extension.ToLower();
            }
        }

        private string SqlServerName
        {
            get
            {
                string sqlServerName = ConfigurationManager.AppSettings["sql_server_name"];
                return sqlServerName;
            }
        }

        protected void loginButton_Click(object sender, EventArgs e)
        {
            try
            {
                ValidationLoader validationLoader = new ValidationLoader();
                string returnUrl = this.Request.QueryString["returnUrl"];
                this.messageLabel.Text = "";
                string username = this.userNameTextBox.Text.Trim();
                //var password = this.passwordTextBox.Text.Trim();
                string msg = validationLoader.VerifyUser(username, this.passwordTextBox.Text.Trim(), 1);
				password = "";
            }
            catch (Exception ex)
            {
                this.messageLabel.Text = ex.Message;
            }
        }

        private void Navigate(string username, string url)
        {
            HttpCookie cookie = new HttpCookie(USER_NAME_COOKIE, username);
            this.Response.Cookies.Add(cookie);

            if (String.IsNullOrEmpty(url) == true)
            {
                url = TabManager.GetUrl(this.PortalSettings.HomeTabId);
            }
            this.Response.Redirect(url);
        }
    }
}