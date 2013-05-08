using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Metadata.CSharpDesign;
using Core.Properties;
using Core.Metadata.SQLDesign;
using System.Xml.Serialization;

namespace Core.Metadata.HBMDesign
{
    [Serializable]
    public class HBMBlockDefine
    {
        public virtual string Assembly { get; set; }

        public virtual string NameSpace { get; set; }

        public virtual List<HBMClassDefine> HBMClassDefineList { get; set; }


        public HBMBlockDefine()
        {
            HBMClassDefineList = new List<HBMClassDefine>();
        }

        public virtual string GenCode()
        {
            StringBuilder sb = new StringBuilder();
            foreach (HBMClassDefine item in HBMClassDefineList)
            {
                sb.Append(item.GenCode());
            }

            return string.Format(Resources.HBMOracle, Assembly, NameSpace, sb.ToString());
        }

    }


    [Serializable]
    public class HBMClassDefine
    {
        public virtual HBMIDDefine IDDefine { get; set; }

        public virtual bool IsID { get; set; }
        public virtual string ClassName { get; set; }
        public virtual string TableName { get; set; }

        public virtual List<HBMPropertyDefine> PropertyDefineList { get; set; }

        public HBMClassDefine(BusiEntity busiEntity ,CSharpClassDefine classDefine, TableDefine tableDefine)
        {


            IDDefine = new HBMIDDefine();
            PropertyDefineList = new List<HBMPropertyDefine>();

            ClassName = classDefine.NameSpace + "." + classDefine.ClassName;
            TableName = tableDefine.TableName;

            foreach (BusiDataItem item in busiEntity.DataItemList)
            {
                PropertyDefineList.Add(new HBMPropertyDefine(classDefine[item.Name], tableDefine[item.Name]));
            }


        }


        public virtual string GenVersion()
        {
            return "    <version name= \"Version\"  column=\"Version\" /> ";
        }


        public virtual string GenCode()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(IDDefine.GenCode());

            sb.AppendLine(GenVersion());

            foreach (HBMPropertyDefine item in PropertyDefineList)
            {
                sb.AppendLine(item.GenCode());
            }

            return string.Format(Resources.HBMClass, ClassName, TableName, sb.ToString());
        }

    }

    [Serializable]
    public class HBMPropertyDefine
    {
        [XmlAttribute]
        public virtual bool IsEnable { get; set; }
        [XmlAttribute]
        public virtual string PropertyName { get; set; }
        [XmlAttribute]
        public virtual string FieldName { get; set; }
        [XmlAttribute]
        public virtual string Caption { get; set; }

        public HBMPropertyDefine(ClassPropertyDefine propertyDefine, FieldDefine fieldDefine)
        {
            PropertyName = propertyDefine.PropertyName;
            Caption = propertyDefine.Caption;

            FieldName = fieldDefine.FieldName;

        }


        public virtual string GenCode()
        {
            return string.Format(Resources.HBMOracleCol, PropertyName, FieldName, Caption);
        }

    }

    [Serializable]
    public class HBMIDDefine
    {

        public virtual string GenCode()
        {
            string s =
                @" 
    <id name=     unsaved-value=0 >
    </id>
";
            return s;
        }


    }
}
