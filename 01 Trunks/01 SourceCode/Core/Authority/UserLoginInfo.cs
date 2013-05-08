using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Authority
{
    [Serializable]
    public class UserLoginInfo
    {
        public  string UserID { get; set; }

        public string Password { get; set; }

        public string IPAddress { get; set; }

        public string IdentifierCode { get; set; }
    }
}
