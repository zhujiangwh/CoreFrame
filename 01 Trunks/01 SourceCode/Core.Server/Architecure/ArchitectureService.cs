using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Architecure;
using NHibernate;
using System.Data;
using System.Collections;
using NHibernate.Cfg;

namespace Core.Server
{
    public class ArchitectureService : RemotingService, IArchitectureService
    {
        #region IArchitectureService 成员

        //===================软件系统===================================

        public SoftSystem GetSoftSystem(string guidString)
        {
            SoftSystem softSystem = null;
            //using (ISession session = NHService.Session)
            {
                //SoftSystemBO service = new SoftSystemBO(NHService);
                //softSystem = service.GetSoftSystem(guidString);
            }
            return null;// softSystem;
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

    }
}
