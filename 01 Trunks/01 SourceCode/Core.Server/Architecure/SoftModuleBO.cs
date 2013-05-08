using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Core.Serialize.DB;
using Core.DB.NHibernate;
using System.Data;
using Core.Common;

namespace Core.Server
{
    public class SoftModuleBO:BusiObject
    {
        public SoftModuleBO()
        { }

        public SoftModuleBO(NHiberanteService nhiberanteService)
        {
            NHService = nhiberanteService;
        }

  

        public IList GetSoftSystem(string guidString)
        {
            string hql = " from SoftModule where SoftSystemGuidString = :SoftSystemGuidString ";
            SqlScript sqlScript = new SqlScript(hql);
            sqlScript.ParamList.Add(new SqlParam("SoftSystemGuidString", DbType.String, guidString));

            return NHService.GetObject(sqlScript);
        } 

        


    }
}
