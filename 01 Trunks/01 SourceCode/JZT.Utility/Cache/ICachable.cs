using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;



namespace JZT.Common.Cache
{
    /// <summary>
    /// 实现Cache接口
    /// </summary>
    public interface ICachable
    {
        /// <summary>
        /// 从缓存中获取对象
        /// </summary>
        /// <param name="itemId">对象ｉｄ</param>
        /// <returns>对象</returns>
        object GetItem(string itemId);

        /// <summary>
        /// 把对象加入缓存
        /// </summary>
        /// <param name="itemId">对象ｉｄ</param>
        /// <param name="item">对象</param>
        void AddItem(string itemId, object item);

        /// <summary>
        /// 移出指定对象
        /// </summary>
        /// <param name="itemId">对象ｉｄ</param>
        void RemoveItem(string itemId);

        /// <summary>
        /// 是否存在某个对象
        /// </summary>
        /// <param name="itemId">对象的key</param>
        /// <returns>0不存在,1存在</returns>
        bool Contains(string itemId);

        /// <summary>
        /// 清空缓存
        /// </summary>
        void Flush();

      
    }
}
