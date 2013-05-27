using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Core.Metadata.CSharpDesign
{
    [Serializable]
    public class CSharpClassDefine
    {
        public virtual string NameSpace { get; set; }

        public virtual bool IsSerializable { get; set; }

        public virtual string ClassName { get; set; }

        public virtual List<ClassPropertyDefine> PropertyDefineList { get; set; }

        
        public virtual ClassPropertyDefine this[string dataItemName]
        {
            get
            {
                return PropertyDefineList.Find(delegate(ClassPropertyDefine c3) { return c3.DataItemName == dataItemName; });
            }
        }




        public CSharpClassDefine()
        {

        }


        public CSharpClassDefine(BusiEntity busiEntity)
        {
            PropertyDefineList = new List<ClassPropertyDefine>();

            ClassName = busiEntity.EntityName;

            NameSpace = " Core.Meta";

            foreach (BusiDataItem item in busiEntity.DataItemList)
            {
                PropertyDefineList.Add(new ClassPropertyDefine(item));
            }
        }

        public virtual string GenUsingNameSpace()
        {
            string s =
@"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
";
            return s;
        }

        public virtual string GenNameSpace()
        {
            return NameSpace;
        }

        public virtual string GetClass()
        {
            string s =
@"
  {0}
  {1} class {2}
  {{
    //属性定义。
{3}

    //构造函数
{4}
    
    //

  }}

";

            string Serializable = IsSerializable ? "[Serializable]" : string.Empty;

            string classStr = string.Format(s, Serializable, "public", ClassName, GetProperty(), GenConstruct());


            return classStr;

        }


        public virtual string GetProperty()
        {
            StringBuilder sb = new StringBuilder();

            int i = 0;
            foreach (ClassPropertyDefine item in PropertyDefineList)
            {
                sb.AppendLine(item.GenCode());

                i++;
                if (i < PropertyDefineList.Count)
                {
                    sb.AppendLine();
                }
            }

            return sb.ToString();
        }

        public virtual string GenConstruct()
        {
            string s =
@"    public {0}()
    {{
    }}

";
            return string.Format(s, ClassName);
        }

        public virtual string GenCode()
        {
            string s =
@"
{0}

namespace {1}
{{
{2}
}}

";

            StringBuilder sb = new StringBuilder();
            //引用命名空间
            sb.AppendLine(string.Format(s, GenUsingNameSpace(), GenNameSpace(), GetClass()));

            return sb.ToString(); ;
        }
    }

    [Serializable]
    public class ClassPropertyDefine
    {
        [XmlAttribute]
        public virtual string DataItemName { get; set; }
        [XmlAttribute]
        public virtual string PropertyName 
        { get; set; }
        [XmlAttribute]
        public virtual string DataType { get; set; }
        [XmlAttribute]
        public virtual string Caption { get; set; }
        [XmlAttribute]
        public virtual bool IsXML { get; set; }

        [XmlIgnore]
        public virtual string Cripe { get; set; }



        public ClassPropertyDefine()
        { }

        public ClassPropertyDefine(BaseDataItem busiDataItem)
        {
            CSharpTypeConvert typeConvert = new CSharpTypeConvert();
            DataType = typeConvert.GetDefaultTypeInfo(busiDataItem.BusiType).Name;

            DataItemName = busiDataItem.Name;
            PropertyName = busiDataItem.Name;
            Caption = busiDataItem.Caption;

            IsXML = true;

        }


        public virtual string GetXmlDefing()
        {
            if (IsXML)
            {
            }

            return "    [XmlAttribute]";
        }


        public virtual string GenCode()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(GetXmlDefing());

            string s =
@"    public virtual {0} {1} {{get; set;}}  // {2}";

            sb.Append(string.Format(s, DataType, PropertyName, Caption));


            return sb.ToString();
        }
    }
}
