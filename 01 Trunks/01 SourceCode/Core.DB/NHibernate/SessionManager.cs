using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Cfg;
using NHibernate;

namespace Core.DB.NHibernate
{
    public class SessionManager : IDisposable
    {
        #region 单例 

        private static SessionManager sessionManager;

        private SessionManager()
        {
            try
            {
                Configuration cfg = new Configuration();

                cfg.Properties.Add("connection.driver_class", "NHibernate.Driver.SqlClientDriver");
                cfg.Properties.Add("connection.connection_string", "Data source=vmxp; Initial Catalog=Core; User Id=sa;Password=gambol;Min Pool Size=5;Max Pool Size=50");
                cfg.Properties.Add("dialect", "NHibernate.Dialect.MsSql2008Dialect");

                cfg.AddAssembly("HBM_SQL");
                _sessionFactory = cfg.BuildSessionFactory();
            }
            catch (Exception ex)
            {
                throw  ex;
            }
        }

        /// <summary>
        /// 取得一个 session 管理 实例。
        /// </summary>
        /// <returns>session 管理对象 </returns>
        public static SessionManager GetInstance()
        {
            if (sessionManager == null)
            {
                sessionManager = new SessionManager();
            }
            return sessionManager;
        }

        #endregion

        private ISessionFactory _sessionFactory;

        //public ISessionFactory GetSessionFactory()
        //{
        //    return _sessionFactory;
        //}


        /// <summary>
        /// 获取一个 session
        /// </summary>
        /// <returns></returns>
        public ISession GetSession()
        {
            DateTime dt = DateTime.Now;
            ISession session = _sessionFactory.OpenSession();

            //LocalDataStoreSlot slot = Context.GetNamedDataSlot("__SESSION__");

            //if (slot == null)
            //    Context.AllocateNamedDataSlot("__SESSION__");

            //Context.SetData(slot, session);

            return session;
        }

        /// <summary>
        /// 释放连接
        /// </summary>
        public void Dispose()
        {
            _sessionFactory.Dispose();
        }
    }


}
