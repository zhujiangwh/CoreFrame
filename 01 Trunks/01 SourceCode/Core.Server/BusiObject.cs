using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DB.NHibernate;
using Core.Common;
using Core.Serialize;
using Core.Serialize.DB;
using System.Collections;

namespace Core.Server
{

    public class Bo : ICommonObjectService, ICommonBusiObject
    {
        public IObjectSerialize objectSerialize { get; set; }

        /// <summary>
        /// 序列化类型定义，由本操作方法进行序列化定义，由它自己决定 是否会处理。
        /// </summary>
        public SerType SerType { get; set; }

        public Bo(IObjectSerialize ser)
        {
            objectSerialize = ser;
        }

        public Bo()
        {

        }


        #region ICommonObjectService 成员


        public T GetObject<T>(string key)
        {


            return objectSerialize.GetObject<T>(key);

        }


        public object Create(object obj)
        {
            OnBeforeSave(obj);

            //处理最后更新时间。
            IUpdateTime updateTime = obj as IUpdateTime;
            if (updateTime != null)
            {
                updateTime.CreateTime = DateTime.Now;
                updateTime.LastModifyTime = DateTime.Now;
            }

            //处理GuidString接口
            IGuidStringKey guidString = obj as IGuidStringKey;
            if (guidString != null && string.IsNullOrEmpty(guidString.GuidString))
            {
                guidString.GuidString = Guid.NewGuid().ToString();
            }

            //保存对象, 自动 处理 主键 值。
            //如果有 guidstring ,则也在里面赋值 。
            objectSerialize.Save(obj);

            OnAfterSave(obj);
            return "";
        }

        public void Update(object obj)
        {
            OnBeforeUpdate(obj);
            //处理最后更新时间。
            IUpdateTime updateTime = obj as IUpdateTime;
            if (updateTime != null)
            {
                updateTime.LastModifyTime = DateTime.Now;
            }

            objectSerialize.Update(obj);

            OnAfterUpdate(obj);
        }

        public void Delete(object obj)
        {
            OnBeforeDelete(obj);

            //处理最后更新时间。
            IUpdateTime updateTime = obj as IUpdateTime;
            if (updateTime != null)
            {
                updateTime.LastModifyTime = DateTime.Now;
            }

            //处理逻辑删除。
            IDelteteFlag delteteFlag = obj as IDelteteFlag;
            if (delteteFlag != null)
            {
                delteteFlag.DeleteFlag = false;
            }
            else
            {
                throw new Exception("未实现 IDelteteFlag 接口。");
            }


            //保存对象, 自动 处理 主键 值。
            //如果有 guidstring ,则也在里面赋值 。
            objectSerialize.Delete(obj);

            OnAfterDelete(obj);
        }

        public IList GetObject(SqlScript sqlScript)
        {
            throw new NotImplementedException();
        }

        public void RealDelete(object obj)
        {
        }

        public bool SaveAllObject(string fullClassName,IList objectList)
        {
            return objectSerialize.SaveAllObject(fullClassName,objectList);
            //throw new NotImplementedException();
        }

        public IList GetAllObject()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region ICommonBusiObject 成员

        public virtual void OnBeforeSave(object obj)
        {
        }

        public virtual void OnAfterSave(object obj)
        {
        }

        public virtual void OnBeforeUpdate(object obj)
        {
        }

        public virtual void OnAfterUpdate(object obj)
        {
        }

        public virtual void OnBeforeDelete(object obj)
        {
        }

        public virtual void OnAfterDelete(object obj)
        {
        }

        #endregion

        #region ICommonObjectService 成员


        public IList GetAllObject(string fullClassName)
        {
            return objectSerialize.GetAllObject(fullClassName);

        }

        public IList<T> GetAllObject<T>()
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public class SampleBo : Bo
    {

        public override void OnBeforeSave(object obj)
        {
            base.OnBeforeSave(obj);
        }

        public override void OnAfterSave(object obj)
        {
            base.OnAfterSave(obj);
        }

    }


    public class BusiObject : ICommonBusiObject
    {
        /// <summary>
        /// 业务对象中的 NH 服务类 不由自己创建 ，由对外的接口服务类创建。
        /// </summary>
        public NHiberanteService NHService { get; set; }

        public void SetNHiberanteService(NHiberanteService nhService)
        {
            NHService = nhService;
        }


        #region ICommonBusiObject 成员

        public virtual void OnBeforeSave(object obj)
        {
        }

        public virtual void OnAfterSave(object obj)
        {
        }

        public virtual void OnBeforeUpdate(object obj)
        {
        }

        public virtual void OnAfterUpdate(object obj)
        {
        }

        public virtual void OnBeforeDelete(object obj)
        {
        }

        public virtual void OnAfterDelete(object obj)
        {
        }

        #endregion
    }
}
