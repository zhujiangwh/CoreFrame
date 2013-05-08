using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Metadata
{
    [Serializable]
    public enum BusinessType
    {
        String   = 0,
        Int      = 1,
        Decimal  = 2,
        Money    = 3,
        Datetime = 4,
        Bool     = 5,
        Text     = 6,
        Image    = 7
        


        //Int16 = 0,
        //Int32 = 1,
        //Int64 = 2,
        //Single = 3,
        //Double = 4,
        //Decimal = 5,
        //String = 6,
        //Datetime = 7,
        //Bool = 8,
        //Text = 9,
        //Image = 10,
        //Unknown = 99
    }

    public class TypeInfo
    {
        public TypeInfo()
        {
            BusiType = BusinessType.String;
            Name = string.Empty;
            IsDefault = true;
        }

        public TypeInfo(BusinessType busiType, string name, bool isDefault)
        {
            BusiType = busiType;
            Name = name;
            IsDefault = isDefault;
        }


        public BusinessType BusiType { get; set; }

        public string Name { get; set; }

        public bool IsDefault { get; set; }
    }

 
    public class TypeConvert
    {
        public virtual List<TypeInfo> TypeInfoList { get; set; }

        public TypeConvert()
        {

        }

        public List<TypeInfo> GetTypeInfoList(BusinessType businessType)
        {
            List<TypeInfo> list = new List<TypeInfo>();

            foreach (TypeInfo item in TypeInfoList)
            {
                if (item.BusiType == businessType)
                {
                    list.Add(item);
                }
            }

            return list;
        }

        public TypeInfo GetDefaultTypeInfo(BusinessType businessType)
        {
            TypeInfo typeInfo = null;

            foreach (TypeInfo item in GetTypeInfoList(businessType))
            {
                if (item.IsDefault)
                {
                    typeInfo = item;
                    break;
                }
            }

            return typeInfo;
        }


    }

    public class CSharpTypeConvert : TypeConvert
    {
        public CSharpTypeConvert()
        {
            TypeInfoList = new List<TypeInfo>();

            TypeInfoList.Add(new TypeInfo(BusinessType.Int, "Int16", false));
            TypeInfoList.Add(new TypeInfo(BusinessType.Int, "Int32", true));
            TypeInfoList.Add(new TypeInfo(BusinessType.Int, "Int64", false));
            TypeInfoList.Add(new TypeInfo(BusinessType.Decimal, "Single", false));
            TypeInfoList.Add(new TypeInfo(BusinessType.Decimal, "Double", true));
            TypeInfoList.Add(new TypeInfo(BusinessType.Money, "Decimal", true));
            TypeInfoList.Add(new TypeInfo(BusinessType.String, "string", true));
            TypeInfoList.Add(new TypeInfo(BusinessType.Datetime, "DateTime", true));
            TypeInfoList.Add(new TypeInfo(BusinessType.Bool, "bool", true));
            TypeInfoList.Add(new TypeInfo(BusinessType.Text, "string", true));
            TypeInfoList.Add(new TypeInfo(BusinessType.Image, "Byte[]", true));

        }
    }

    public class SQLTypeConvert : TypeConvert
    {
        public SQLTypeConvert()
        {
            TypeInfoList = new List<TypeInfo>();

            TypeInfoList.Add(new TypeInfo(BusinessType.Int, "smallint", false));
            TypeInfoList.Add(new TypeInfo(BusinessType.Int, "int", true));
            TypeInfoList.Add(new TypeInfo(BusinessType.Int, "Int64", false));
            TypeInfoList.Add(new TypeInfo(BusinessType.Decimal, "decimal", false));
            TypeInfoList.Add(new TypeInfo(BusinessType.Decimal, "numeric", true));
            TypeInfoList.Add(new TypeInfo(BusinessType.Money, "smallmoney", false));
            TypeInfoList.Add(new TypeInfo(BusinessType.Money, "money", true));
            TypeInfoList.Add(new TypeInfo(BusinessType.String, "char", false));
            TypeInfoList.Add(new TypeInfo(BusinessType.String, "varchar", false));
            TypeInfoList.Add(new TypeInfo(BusinessType.String, "nchar", true));


            TypeInfoList.Add(new TypeInfo(BusinessType.Datetime, "datetime", true));
            TypeInfoList.Add(new TypeInfo(BusinessType.Bool, "bit", true));
            TypeInfoList.Add(new TypeInfo(BusinessType.Text, "text", true));
            TypeInfoList.Add(new TypeInfo(BusinessType.Text, "ntext", false));

            TypeInfoList.Add(new TypeInfo(BusinessType.Image, "varbinary", true));
            TypeInfoList.Add(new TypeInfo(BusinessType.Image, "image", false));

        }
    }

    public class OracleTypeConvert : TypeConvert
    {
        public OracleTypeConvert()
        {
            TypeInfoList = new List<TypeInfo>();

            TypeInfoList.Add(new TypeInfo(BusinessType.Int, "NUMBER(11)", true));
            TypeInfoList.Add(new TypeInfo(BusinessType.Int, "NUMBER(11)", true));
            TypeInfoList.Add(new TypeInfo(BusinessType.Int, "NUMBER(19)", true));
            TypeInfoList.Add(new TypeInfo(BusinessType.Decimal, "NUMBER({0},{1})", true));
            TypeInfoList.Add(new TypeInfo(BusinessType.Decimal, "NUMBER({0},{1})", true));
            TypeInfoList.Add(new TypeInfo(BusinessType.Money, "NUMBER({0},{1})", true));
            TypeInfoList.Add(new TypeInfo(BusinessType.String, "VARCHAR2({0})", true));
            TypeInfoList.Add(new TypeInfo(BusinessType.String, "NVARCHAR2({0})", false));
            TypeInfoList.Add(new TypeInfo(BusinessType.Datetime, "Date", true));
            TypeInfoList.Add(new TypeInfo(BusinessType.Bool, "NUMBER(1)", true));
            TypeInfoList.Add(new TypeInfo(BusinessType.Text, "Clob", true));
            TypeInfoList.Add(new TypeInfo(BusinessType.Image, "Blob", true));
        }
    }

}
