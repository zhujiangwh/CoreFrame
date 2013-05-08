using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Cfg;
using NHibernate;
using Core.DB.NHibernate;
using Core.Architecure;

namespace Core.Authority
{
    public class AuthorityBLL : IAuthorityService
    {
        private AuthorityBO authorityBO = new AuthorityBO();
        #region IAuthorityService 成员

        public string Login(UserLoginInfo userLoginInfo)
        {
            //获取该单据的单据定义。
            Function function = new Function();

            function.OnExecute += new EventHandler(function_OnExecute);


            return string.Empty;


        }

        void function_OnExecute(object sender, EventArgs e)
        {
            authorityBO.Login(null);
        }

        public void Logout(string loginKey)
        {
            Function function1 = new Function();

            function1.OnExecute += new EventHandler(function1_OnExecute);
        }

        void function1_OnExecute(object sender, EventArgs e)
        {
            authorityBO.Logout("");
        }

        #endregion
    }

    public class AuthorityBO : IAuthorityService
    {
        #region IAuthorityService 成员

        public string Login(UserLoginInfo userLoginInfo)
        {
            //验证用户登录 ，并返回登录Key给到用户客户端 。

            //验证识别码。

            //验证用户是否存在。


            //验证用户密码是否匹配。

            NHiberanteService nhiberanteService1 = WinApplication.GetInstance().DBServerManager.DBServerList[1].GetNHiberanteService();


            //创建用户登录信息。
            OnlineUser onlineUser = new OnlineUser(userLoginInfo.UserID);
            onlineUser.Login();
            onlineUser.Logout();




            RunLog runLog = onlineUser.CreateRunLog();
            runLog.EndExe();

            
            //保存用户信息到数据库中。

             using (ISession session = nhiberanteService1.Session)
            {
                using (ITransaction trans = session.BeginTransaction())
                {
                    try
                    {
                        //保存对象, 自动 处理 主键 值。
                        //如果有 guidstring ,则也在里面赋值 。
                        nhiberanteService1.Save(onlineUser);
                        nhiberanteService1.Save(runLog);

                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        throw ex;
                    }
                }
            }

            return onlineUser.LoginKey;
        }

        public void Logout(string loginKey)
        {
            //取得在线信息。
            OnlineUser onlineUser = null;

            if (onlineUser != null)
            {
                onlineUser.Logout();

                //保存用户信息以数据库。
            }
            else
            {
                //抛出异常。
            }


        }

        #endregion
    }
}
