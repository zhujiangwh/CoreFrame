using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Server;

namespace Core.Authority
{
    public class AuthorityService :RemotingService, IAuthorityService
    {

        private  AuthorityBO authorityBO = new AuthorityBO();

        #region IAuthorityService 成员

        public string Login(UserLoginInfo userLoginInfo)
        {
            return authorityBO.Login(userLoginInfo);
        }

        public void Logout(string loginKey)
        {
            authorityBO.Logout(loginKey);
        }

        #endregion
    }
}
