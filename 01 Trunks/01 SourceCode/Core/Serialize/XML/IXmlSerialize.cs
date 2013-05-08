using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Serialize.XML
{
    public interface IXmlSerialize
    {
        string GetFileName();

        string GetPath();

        /// <summary>
        /// 处理流对象。部分字段作为不直接保存到数据中，而保存在流字段中所做的转换。
        /// </summary>
        void BeforeSerialize();


        /// <summary>
        /// 处理流对象。部分字段作为不直接保存到数据中，而保存在流字段中所做的转换。
        /// </summary>
        void AfterDeserialize();
    }
}
