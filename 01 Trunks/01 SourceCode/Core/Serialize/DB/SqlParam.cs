using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml.Serialization;


namespace Core.Serialize.DB
{
    [Serializable]
    public class SqlParam
    {
        [XmlAttribute]
        public virtual string ParamName { get; set; }

        [XmlAttribute]
        public virtual DbType ParamType { get; set; }

        [XmlAttribute]
        public virtual string DisplayLable { get; set; }

        [XmlIgnore]
        public virtual object ParamValue { get; set; }

        [XmlIgnore]
        public virtual object DefaultValue { get; set; }

        public SqlParam()
        {
        }


        public SqlParam(string paramName, DbType paramType, object paramValue)
        {
            ParamName = paramName;
            ParamType = paramType;
            ParamValue = paramValue;
        }

    }
}
