using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Serialize
{
    /// <summary>
    /// 对象序列化的接口.
    /// </summary>
    public interface IObjectSerialize
    {
        void Save(object obj);

        void Update(object obj);

        /// <summary>
        /// 缺省删除均为逻辑删除.
        /// </summary>
        /// <param name="obj"></param>
        void Delete(object obj);

        T GetObject<T>(string Key);

        /// <summary>
        /// 物理删除
        /// </summary>
        /// <param name="obj"></param>
        void RealDelete(object obj);
    }
}
