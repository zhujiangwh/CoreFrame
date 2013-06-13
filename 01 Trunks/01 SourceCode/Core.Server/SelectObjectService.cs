using System.Data;
using Core.Architecure;
using Core.DB;
using Core.Serialize.XML;
using Core.Server;
using Core.UI;

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
