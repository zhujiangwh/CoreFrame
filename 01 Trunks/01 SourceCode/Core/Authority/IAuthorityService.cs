using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Authority
{
    public interface IAuthorityService
    {
        string Login(UserLoginInfo userLoginInfo);

        void Logout(string loginKey);
    }
}
