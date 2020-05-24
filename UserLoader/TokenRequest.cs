using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UserLoader
{
    public static class TokenRequest
    {
        public static string TokenRequestPayload
        {
            get
            {
                return "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:ws=\"http://ws.waei.uba.com/\">" +
                      "<soapenv:Header/>" +
                      "<soapenv:Body>" +
                        "<ws:authenticateToken>" +
                          "<!--Optional:-->" +
                          "<request>" +
                            "<!--Optional:-->" +
                            "<response>{0}</response>" +
                            "<!--Optional:-->" +
                            "<userGroup>{1}</userGroup>" +
                            "<!--Optional:-->" +
                            "<username>{2}</username>" +
                            "<!--Optional:-->" +
                            "<requesterId>{3}</requesterId>" +
                            "<!--Optional:-->" +
                            "<requesterIp>{4}</requesterIp>" +
                          "</request>" +
                        "</ws:authenticateToken>" +
                      "</soapenv:Body>" +
                    "</soapenv:Envelope>";
            }
        }

    }
}
