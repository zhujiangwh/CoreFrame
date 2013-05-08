using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Common;
using System.Xml.Serialization;

namespace Core.Metadata
{

    [Serializable]
    public class BaseDataItem : IGuidStringKey
    {
        public BaseDataItem()
        {
            GuidString = Guid.NewGuid().ToString();
        }

        [XmlAttribute]
        public virtual string Name { get; set; }


        /// <summary>
        /// 中文名称
        /// </summary>
        //[Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        //[Category("元数据")]
        //[Description("中文名称")]

        [XmlAttribute]
        public virtual string Caption { get; set; }

        [XmlAttribute]
        public virtual BusinessType BusiType { get; set; }

        /// <summary>
        /// 物理长度
        /// </summary>
        //[Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        //[Category("元数据")]
        //[Description("物理长度")]
        [XmlAttribute]
        public virtual int Length { get; set; }

        [XmlAttribute]
        public virtual int NumScale { get; set; }

        [XmlAttribute]
        public virtual bool AllowNull { get; set; }









        /// <summary>
        /// 最大值
        /// </summary>
        //[Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        //[Category("校验")]
        //[Description("最大值")]

        [XmlAttribute]
        public virtual int Maximum { get; set; }



        /// <summary>
        /// 最小值
        /// </summary>
        //[Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        //[Category("校验")]
        //[Description("最小值")]

        [XmlAttribute]
        public virtual int Minimum { get; set; }



        /// <summary>
        /// 精度
        /// </summary>
        //[Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        //[Category("元数据")]
        //[Description("精度")]



        /// <summary>
        /// 可否为空
        /// </summary>
        //       [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]

        [XmlAttribute]
        public virtual string Nullable { get; set; }



        /// <summary>
        /// 数据类型
        /// </summary>
        //[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        //[Category("元数据")]
        //[Description("数据类型")]

        [XmlAttribute]
        public virtual string DataTypeSetting { get; set; }



        /// <summary>
        /// 掩码串
        /// </summary>
        //[Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        //[Category("校验"), DefaultValue("")]
        //[Description("掩码串")]

        [XmlAttribute]
        public virtual string MaskString { get; set; }

        /// <summary>
        /// 掩码类型
        /// </summary>
        //[Category("校验")]
        //[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]

        [XmlAttribute]
        public virtual string MaskType { get; set; }


        /// <summary>
        /// 正则串
        /// </summary>
        //[Category("校验")]
        //[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        [XmlAttribute]
        public virtual string Regular { get; set; }



        /// <summary>
        /// 默认值
        /// </summary>
        //[Category("元数据")]
        //[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]

        [XmlAttribute]
        public virtual string DefaultValue { get; set; }



        #region IGuidStringKey 成员

        [XmlAttribute]
        public virtual string GuidString { get; set; }

        [XmlAttribute]
        public virtual  bool DeleteFlag { get; set; }

        #endregion
    }

    [Serializable]
    public class BusiDataItem : BaseDataItem 
    {
        [XmlIgnore]
        public virtual Domain Domain { get; set; }

        [XmlIgnore]
        public virtual BusiEntity BusiEntity { get; set; }

        public virtual string ControlTypeName { get; set; }

    }
}
