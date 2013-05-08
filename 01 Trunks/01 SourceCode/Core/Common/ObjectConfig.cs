using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Core.Architecure;

namespace Core.Common
{
  
    /// <summary>
    /// 
    /// </summary>
    public interface ICommonBusiObject
    {

        void OnBeforeSave(object obj);

        void OnAfterSave(object obj);

        void OnBeforeUpdate(object obj);

        void OnAfterUpdate(object obj);

        void OnBeforeDelete(object obj);

        void OnAfterDelete(object obj);

    }

    [Serializable]
    public class ObjectConfig
    {
        public ObjectConfig()
        { }

        public ObjectConfig(string fullClassName, string serverObjectAssemblyName ,string serverObjectFullClassName)
        {
            FullClassName = fullClassName;
            ServerObjectAssemblyName = serverObjectAssemblyName;
            ServerObjectFullCalssName = serverObjectFullClassName;
        }

        public virtual string FullClassName { get; set; }
        public virtual string ServerObjectAssemblyName { get; set; }
        public virtual string ServerObjectFullCalssName { get; set; }

        public virtual ICommonBusiObject CreateCommonBusiObject()
        {
            return new ObjectDefine(ServerObjectAssemblyName, ServerObjectFullCalssName).CreateObject() as ICommonBusiObject;
        }

    }

    public class ObjectConfigManager
    {
        public Hashtable ObjectConfigList { get; set; }

        public ObjectConfigManager()
        {
            ObjectConfigList = new Hashtable();

            ObjectConfigList.Add("Core.Architecure.SoftSystem",new ObjectConfig("Core.Architecure.SoftSystem", "Core.Server", "Core.Server.SoftSystemBO"));
            ObjectConfigList.Add("Core.Architecure.SoftModule", new ObjectConfig("Core.Architecure.SoftModule", "Core.Server", "Core.Server.SoftModuleBO"));
            ObjectConfigList.Add("Core.Architecure.SoftComponent", new ObjectConfig("Core.Architecure.SoftComponent", "Core.Server", "Core.Architecure.SystemComponentBO"));
        }

        public ObjectConfig GetObjectConfig(string fullClassName)
        {
            return ObjectConfigList[fullClassName] as ObjectConfig;
        }
    }
}
