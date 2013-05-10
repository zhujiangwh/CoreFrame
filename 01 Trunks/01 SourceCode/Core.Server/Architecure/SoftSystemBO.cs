using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Architecure;
using Core.Serialize.DB;
using System.Data;
using Core.DB.NHibernate;
using System.Collections;
using Core.Common;

namespace Core.Server
{
    public class SoftSystemBO : BusiObject
    {
        public SoftSystemBO()
        { }

        public SoftSystemBO(NHiberanteService nhiberanteService)
        {
            NHService = nhiberanteService;
        }



        public DataTable GetSoftSystem()
        {
            return NHService.GetDataTable(" select * from Core_SoftSystem ");
        }

        public SoftSystem GetSoftSystem(string guidString)
        {

            string hql = " from SoftSystem where GuidString = :GuidString ";
            SqlScript sqlScript = new SqlScript(hql);
            sqlScript.ParamList.Add(new SqlParam("GuidString", DbType.String, guidString));

            IList list =  NHService.GetObject(sqlScript);

            SoftSystem softSystem = null;
            if (list.Count == 1)
            {
                softSystem = list[0] as SoftSystem;

                //SoftModuleBO service = new SoftModuleBO(NHService);
                //IList moduleList = service.GetSoftSystem(softSystem.GuidString);

                //foreach (object obj in moduleList)
                //{
                //    softSystem.SubMouduleList.Add(obj as SoftModule);
                //}
            }


            return softSystem;
        }
    }
}
