using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Caching.Expirations;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using System.Collections;

namespace JZT.Common.Cache

{
    /// <summary>
    /// Cache基类.
    /// </summary>
    public abstract class Cache : ICachable
    {
        #region 保护字段


        protected CacheManager cache; //缓存
        protected CacheItemPriority cacheItemPriority;
        protected ICacheItemRefreshAction refreshAction;
        protected ICacheItemExpiration expirationTime;

        #endregion

        #region 构造函数

        protected Cache()
        {
           
        }

        #endregion


        #region 实现接口ICachable

        public virtual object GetItem(string itemId)
        {
            return cache.GetData(itemId);
        }

        public virtual void AddItem(string itemId, object item)
        {
            cache.Add(itemId, item);
        }

        public virtual void RemoveItem(string itemId)
        {
            cache.Remove(itemId);
        }

        public virtual bool Contains(string itemId)
        {
            
            return cache.Contains(itemId);
        }
       
        public virtual void Flush()
        {
            cache.Flush();
        }
    
        #endregion

        
    }
}
