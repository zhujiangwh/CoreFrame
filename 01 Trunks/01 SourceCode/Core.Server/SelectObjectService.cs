using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.UI;
using Core.Server;
using Core.Serialize.XML;
using NHibernate;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using Core.Architecure;
using Core.DB;

namespace Core
{
    public class SelectObjectService : RemotingService, ISelectObjectService
    {
        private XmlSerializeService xmlSerializeService;

        private string fullClassName = @"Core.UI.SelectObjectUIC";


        public SelectObjectService()
        {
            xmlSerializeService = new XmlSerializeService();
        }

        #region ISelectObjectService 成员

        public bool SaveSelectObjectUIC(SelectObjectUIC selectObjectUIC)
        {
            xmlSerializeService.SaveToFile(selectObjectUIC);

            return true;
        }

        public System.Data.DataTable GetDataTable(string name)
        {
            //根据 字段名 得到 文件名。
            SelectObjectUIC selectObjectUIC = LoadSelectObjectUIC(name);

            DBServer dbServer = WinApplication.GetInstance().DBServerManager.DBServerList[0];

            DataTable table = dbServer.GetDateTalbe(selectObjectUIC.SqlScript.Sql);


            return table;
        }



        public SelectObjectUIC LoadSelectObjectUIC(string name)
        {
            string filename = xmlSerializeService.GetFileName(fullClassName, name);
            return xmlSerializeService.LoadFormFile(fullClassName, filename) as SelectObjectUIC;
        }

        #endregion
    }
}
