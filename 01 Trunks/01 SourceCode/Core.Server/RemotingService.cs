using System;
using Core.DB.NHibernate;
using Core.DB;
using Core.Architecure;

namespace Core.Server
{
    public class RemotingService : MarshalByRefObject
    {
        public RemotingService()
        {
            //DBServer server = WinApplication.GetInstance().DBServerManager.DBServerList[0];
            //NHService = server.GetNHiberanteService() ;
        }

        //public NHiberanteService NHService { get; set; }

    }
}
