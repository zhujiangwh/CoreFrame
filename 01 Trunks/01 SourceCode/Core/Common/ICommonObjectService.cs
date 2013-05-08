using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Serialize.DB;
using System.Collections;

namespace Core.Common
{
    public interface ICommonObjectService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        object Create(object obj);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        void Update(object obj);

        /// <summary>
        /// 缺省删除均为逻辑删除.
        /// </summary>
        /// <param name="obj"></param>
        void Delete(object obj);

        IList GetObject(SqlScript sqlScript);

        /// <summary>
        /// 物理删除
        /// </summary>
        /// <param name="obj"></param>
        void RealDelete(object obj);

        bool SaveAllObject(IList objectList);

        IList GetAllObject();

    }
}
