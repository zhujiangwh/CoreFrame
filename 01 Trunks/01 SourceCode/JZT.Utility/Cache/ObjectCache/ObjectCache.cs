using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using Microsoft.Practices.EnterpriseLibrary.Caching.Expirations;

namespace JZT.Common.Cache
{
    #region 加载程序集缓存管理
    internal class ObjectCache : JZT.Common.Cache.Cache
    {
        public const string CACHE_NAME = "Object Cache";

        public static readonly ObjectCache ObjectCacheInstance = new ObjectCache();


        private ObjectCache()
        {
            cache = CacheFactory.GetCacheManager(CACHE_NAME);
            cacheItemPriority = CacheItemPriority.Normal;
            //expirationTime = new SlidingTime(TimeSpan.FromMinutes(1));

        }

        public override void AddItem(string itemId, object item)
        {
            cache.Add(itemId, item);
            //   cache.Add(itemId, item, cacheItemPriority, refreshAction, expirationTime);
        }
    }
    #endregion
    
}
