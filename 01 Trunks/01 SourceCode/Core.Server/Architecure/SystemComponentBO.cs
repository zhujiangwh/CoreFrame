using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Server;
using Core.Serialize.DB;
using System.Data;
using System.Collections;
using Core.DB.NHibernate;
using Core.Common;

namespace Core.Architecure
{
    public class SystemComponentBO : BusiObject
    {
        public SystemComponentBO()
        { }

        public SystemComponentBO(NHiberanteService nhiberanteService)
        {
            NHService = nhiberanteService;
        }


        public IList GetSystemComponent(string moduleGuidString)
        {
            string hql = " from SoftComponent where ModuleGuidString = :moduleGuidString ";
            SqlScript sqlScript = new SqlScript(hql);
            sqlScript.ParamList.Add(new SqlParam("SoftSystemGuidString", DbType.String, moduleGuidString));

            return NHService.GetObject(sqlScript);
        }

    }
}
