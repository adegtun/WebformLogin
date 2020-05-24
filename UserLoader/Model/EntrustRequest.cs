using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UserLoader.Model
{
    public class EntrustRequest
    {
        public string requesterId { get; set; }
        public string requesterIp { get; set; }
        public string response { get; set; }
        public string userGroup { get; set; }
        public string username { get; set; }
    }

    public class AuthenticationResponce
    {
        public string isSuccessful { get; set; }
        public string response { get; set; }
        public string errorResponseMessage { get; set; }
        public string errorResponseCode { get; set; }

    }
}
