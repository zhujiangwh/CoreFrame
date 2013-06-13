using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Architecure;
using NHibernate;
using System.Data;
using System.Collections;
using NHibernate.Cfg;
using Core.DB;

namespace Core.Server
{
    public class ArchitectureService : RemotingService, IArchitectureService
    {
        #region IArchitectureService 成员

        //===================软件系统===================================

        public SoftSystem GetSoftSystem(string guidString)
        {
            DBServer server = WinApplication.GetInstance().DBServerManager.DBServerList[0];
            NHService = server.GetNHiberanteService() ;

            SoftSystem softSystem = null;
            using (ISession session = NHService.Session)
            {
                SoftSystemBO service = new SoftSystemBO(NHService);
                softSystem = service.GetSoftSystem(guidString);
            }
            return softSystem;// softSystem;
        }

        public DataTable GetSoftSystem()
        {
            //SoftSystemBO service = new SoftSystemBO(NHService);
            return null; // service.GetSoftSystem();
        }


        //===================模块系统===================================


        //===================软件组件系统===================================

        public IList GetSystemComponent(string moduleGuidString)
        {
            //SystemComponentBO bo = new SystemComponentBO(NHService);
            return null;// bo.GetSystemComponent(moduleGuidString);
        }
        #endregion


        public DB.NHibernate.NHiberanteService NHService { get; set; }
    }
}
