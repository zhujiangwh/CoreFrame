using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Serialize.DB
{
    [Serializable]
    public class SqlScript
    {
        public virtual string Sql { get; set; }
        public virtual List<SqlParam> ParamList { get; set; }

        public SqlScript()
        {
            ParamList = new List<SqlParam>();
        }

        public SqlScript(string sql)
        {
            Sql = sql;
            ParamList = new List<SqlParam>();

        }
    }
}
