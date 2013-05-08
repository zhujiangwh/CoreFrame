using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Authority
{
    public interface IAuthorityDisplay
    {
        AuthorityControl AuthorityControl { get; set; }
        void DisplayUserLoginInfo(UserLoginInfo userLoginInfo);
    }
}
