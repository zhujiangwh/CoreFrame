using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Architecure;

namespace Core.Authority
{
    public class AuthorityControl
    {
        public IAuthorityDisplay AuthorityDisplay { get; set; }

        public RemotingServer remotingServer {get;set;}
        public IAuthorityService service { get; set; }

        public string LogionKey { get; set; }


        public AuthorityControl()
        {
            remotingServer = new RemotingServer("tcp", "127.0.0.1:8545");
            service = remotingServer.CreateRemotingInterface<IAuthorityService>("AuthorityService");
        }

        public void CreateForm()
        {
            AuthorityDisplay = null;
            AuthorityDisplay.AuthorityControl = this;
        }


        public bool Login()
        {
            bool flag = false;
            // 获取历史登录信息


            //初始化登录信息。
            UserLoginInfo userLoginInfo = new UserLoginInfo();


            //显示登录界面 。
            if (AuthorityDisplay != null)
            {
                AuthorityDisplay.DisplayUserLoginInfo(userLoginInfo);
            }


            return flag;

        }

        public void  Login(UserLoginInfo userLoginInfo) 
        {
            LogionKey = service.Login(userLoginInfo);

            if (string.IsNullOrEmpty(LogionKey))
            { }
            else
            { }
 
        }
    }
}
