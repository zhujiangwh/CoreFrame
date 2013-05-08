using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Core.Properties;

namespace Core.Metadata.SQLDesign
{
    [Serializable]
    public class SQLBlockDefine
    {
        public virtual List<TableDefine> TableDefineList { get; set; }

        public virtual string   GenCode()
        {
            StringBuilder sb = new StringBuilder();
            foreach (TableDefine item in TableDefineList)
            {
                sb.AppendLine(item.GenCode());
            }

            return string.Format(Resources.SQLBlock, sb.ToString());
        }

        public SQLBlockDefine()
        {
            TableDefineList = new List<TableDefine>();
        }
    }

    [Serializable]
    public class TableDefine
    {
        public virtual string TableName { get; set; }

        public virtual  FieldDefine this[string dataItemName]
        {
            get
            {
                return FieldDefineList.Find(delegate(FieldDefine c3) { return c3.DataItemName == dataItemName; });
            }
        }


        public virtual List<FieldDefine> FieldDefineList { get; set; }

        public virtual List<IndexDefine> IndexDefineList { get; set; }

        public TableDefine(BusiEntity busiEntity)
        {
            FieldDefineList = new List<FieldDefine>();
            IndexDefineList = new List<IndexDefine>();

            TableName = busiEntity.EntityName;

            foreach (BusiDataItem item in busiEntity.DataItemList)
            {
                FieldDefineList.Add(new FieldDefine(item));
            }
        }

        public virtual string GenCode()
        {
            StringBuilder sb = new StringBuilder();
            foreach (FieldDefine item in FieldDefineList)
            {
                sb.AppendLine(item.GenCode());
            }

            return string.Format(Resources.SQLTable,TableName, sb.ToString() ,""); 
        }
    }

    public class IndexDefine
    {
        public virtual string FieldName { get; set; }

        public IndexDefine()
        {
 
        }

        public virtual string GenCode()
        {
            return " table";
        }


    }


    [Serializable]
    public class FieldDefine
    {
        [XmlAttribute]
        public virtual string DataItemName { get; set; }
        [XmlAttribute]
        public virtual string FieldName { get; set; }
        [XmlAttribute]
        public virtual string DataType { get; set; }
        [XmlAttribute]
        public virtual string Caption { get; set; }
        [XmlAttribute]
        public virtual string Default { get; set; }

        [XmlAttribute]
        public virtual int Length { get; set; }
        [XmlAttribute]
        public virtual int NumScale { get; set; }
        [XmlAttribute]
        public virtual bool AllowNull { get; set; }


        public FieldDefine(BusiDataItem busiDataItem)
        {
            SQLTypeConvert sqlTypeConvert = new SQLTypeConvert();

            DataItemName = busiDataItem.Name;
            FieldName = busiDataItem.Name;

            DataType = sqlTypeConvert.GetDefaultTypeInfo(busiDataItem.BusiType).Name;

            Caption = busiDataItem.Caption;

            Length = busiDataItem.Length;
            NumScale = busiDataItem.NumScale;
            AllowNull = busiDataItem.AllowNull;
        }

        public virtual string GenCode()
        {
            //   	 GuidString nvarchar(36) NOT NULL,  --Guid

            string nullstring = AllowNull ? " null " : " not null";

            string s = @" {0} {1} {2} , -- {3}";
            return string.Format(s,FieldName,DataType,nullstring,Caption);
        }


    }


}
