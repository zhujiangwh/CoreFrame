using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Common;

namespace Core.Server
{
    /// <summary>
    /// 序列化类型 ， xml json 等。
    /// </summary>
    public enum SerType
    {
        Xml, NHibernate, Json
    }


    /// <summary>
    /// 功能服务接口创建 。 它只处理是本地方法还是采用远程接口。
    /// </summary>
    public class CommonObjectCreater
    {
        public static ICommonObjectService CreateCommonObjectService()
        {
            return new CommonObjectStorage();
        }

        public static ICommonObjectService CreateCommonObjectService(SerType serType)
        {
            ICommonObjectService service = CreateCommonObjectService();
            return new CommonObjectStorage();
        }


    }
}
