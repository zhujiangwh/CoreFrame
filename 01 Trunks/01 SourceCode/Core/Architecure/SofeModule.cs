using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Common;
using Core.UI;

namespace Core.Architecure
{
    [Serializable]
    public class BaseSoftModule : IUpdateTime, IGuidStringKey
    {
        public BaseSoftModule()
        {
            SubMouduleList = new List<BaseSoftModule>();
        }

        public virtual BaseSoftModule ParentModule { get; set; }
        public virtual List<BaseSoftModule> SubMouduleList { get; set; }

        public virtual string GuidString { get; set; }
        public virtual string SoftName { get; set; }
        public virtual string Caption { get; set; }



        public virtual int Version { get; set; }
        public virtual DateTime CreateTime { get; set; }
        public virtual DateTime LastModifyTime { get; set; }
        public virtual bool DeleteFlag { get; set; }


        public virtual string NameSpaceForClass { get; set; }  //类的命名空间。
        public virtual string NameSpaceForDB { get; set; }     //数据表的前缀。

        public virtual string Author { get; set; }
        public virtual string SoftVersion { get; set; }

    }

    [Serializable]
    public class SoftModule : BaseSoftModule, IDisplayUIObject
    {
        public virtual string SoftSystemGuidString { get; set; }

        public virtual string ParentModuleGuidString { get; set; }

        public virtual List<SoftComponent> ComponentList { get; set; }

        public SoftModule()
        {
            ComponentList = new List<SoftComponent>();
        }


        public virtual SoftModule CreateSubModule()
        {
            SoftModule module = new SoftModule();
            module.ParentModule = this;
            module.ParentModuleGuidString = GuidString;
            module.SoftSystemGuidString = SoftSystemGuidString;

            return module;
        }



        #region IDisplayUIObject 成员

        public virtual  UIDisplayObject Convert()
        {
            UIDisplayObject uiObject = new UIDisplayObject();
            uiObject.Name = this.SoftName;
            uiObject.Title = this.SoftName;
            uiObject.LevelString = "01";
            uiObject.RealObject = this;

            return uiObject;
        }

        public virtual void Load(UIDisplayObject uiDisplayObject)
        {
        }

        #endregion
    }

    [Serializable]
    public class SoftSystem : BaseSoftModule, IDisplayUIObject
    {
        #region IDisplayUIObject 成员

        public virtual UIDisplayObject Convert()
        {
            UIDisplayObject uiObject = new UIDisplayObject();
            uiObject.Name = this.SoftName;
            uiObject.Title = this.SoftName;
            uiObject.LevelString = "01";
            uiObject.RealObject = this;


            UIDisplayObject module = new UIDisplayObject();
            module.Parent = uiObject;
            module.Name = "模块";
            module.Title = "模块";
            module.LevelString = "0101";
            uiObject.List.Add(module);

            foreach (BaseSoftModule item in SubMouduleList)
            {

                UIDisplayObject module11 = (item as SoftModule).Convert();// new UIDisplayObject();
                module11.Parent = uiObject;
                //module11.Name = "模块";
                //module11.Title = "模块";
                module11.LevelString = "010101";
                uiObject.List.Add(module11);

            }



            UIDisplayObject module1 = new UIDisplayObject();
            module1.Parent = uiObject;
            module1.Name = "安全";
            module1.Title = "安全";
            module1.LevelString = "0102";
            uiObject.List.Add(module1);




            return uiObject;
        }

        public virtual void Load(UIDisplayObject uiDisplayObject)
        {
        }

        #endregion

        public virtual SoftModule CreateSubModule()
        {
            SoftModule module = new SoftModule();
            module.ParentModuleGuidString = string.Empty;
            module.SoftSystemGuidString = GuidString;
            module.SoftName = "模块三";


            return module;
        }
    }
}
