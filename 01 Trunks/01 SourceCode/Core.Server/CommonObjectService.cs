﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Common;
using NHibernate;
using Core.DB.NHibernate;
using Core.Serialize.DB;
using System.Collections;
using System.Data;
using Core.Serialize.XML;
using JZT.Utility;
using Core.Serialize;

namespace Core.Server
{



    public class CommonObjectService : RemotingService, ICommonObjectService
    {

        public IObjectSerialize objectSerialize { get; set; }

        public CommonObjectService()
        {
            objectSerialize = new XmlSerialize();
        }

        private Bo GetBO(object obj)
        {
            Bo bo = new Bo();
            bo.objectSerialize = objectSerialize;

            return bo;
        }

        private ICommonBusiObject GetRealBusiObject(object obj)
        {
            ICommonBusiObject commonBusiObject = null;

            if (obj != null)
            {
                ObjectConfigManager manager = new ObjectConfigManager();

                string fullName = obj.GetType().FullName;

                ObjectConfig objectConfig = manager.GetObjectConfig(fullName);

                if (objectConfig != null)
                {

                    commonBusiObject = objectConfig.CreateCommonBusiObject();

                    BusiObject busiObject = commonBusiObject as BusiObject;
                    if (busiObject != null)
                    {
                        //busiObject.SetNHiberanteService(NHService);
                    }
                }
            }
            else
            {
                //触发异常等。
            }

            return commonBusiObject;
        }

        #region ICommonObjectService 成员

        public object Create(object obj)
        {
            //获取扩展对象
            ICommonBusiObject commonBusiObject = GetRealBusiObject(obj);

            Bo bo = GetBO(obj);

            //using (ISession session = NHService.Session)
            {
                // using (ITransaction trans = session.BeginTransaction())
                {
                    try
                    {

                        bo.Create(obj);

                        //trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        //trans.Rollback();
                        throw ex;
                    }
                }
            }

            return "";
        }

        public void Update(object obj)
        {
            //获取扩展对象
            ICommonBusiObject commonBusiObject = GetRealBusiObject(obj);

            Bo bo = GetBO(obj);


            //using (ISession session = NHService.Session)
            {
                //using (ITransaction trans = session.BeginTransaction())
                {
                    try
                    {

                        bo.Update(obj);

                        //trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        //trans.Rollback();
                        throw ex;
                    }
                }
            }
        }

        public void Delete(object obj)
        {
            //获取扩展对象
            ICommonBusiObject commonBusiObject = GetRealBusiObject(obj);

            Bo bo = GetBO(obj);


            //using (ISession session = NHService.Session)
            {
                // using (ITransaction trans = session.BeginTransaction())
                {
                    try
                    {

                        bo.Delete(obj);
                        //trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        //trans.Rollback();
                        throw ex;
                    }
                }
            }
        }

        public IList GetObject(SqlScript sqlScript)
        {
            IList list = null;
            //using (ISession session = NHService.Session)
            {
                //list = NHService.GetObject(sqlScript);
            }

            return list;
        }

        public void RealDelete(object obj)
        {
            Bo bo = GetBO(obj);

            bo.RealDelete(obj);

            //objectSerialize.RealDelete(obj);

        }

        #endregion

        #region ICommonObjectService 成员


        public bool SaveAllObject(IList objectList)
        {
            //XmlSerializeService service = new XmlSerializeService();

            //service.XmlSerializeDefineManager.LoadXmlSerializeDefineList(objectList);

            //service.SaveXmlSerializeDefineManager();

            ArrayList list = new ArrayList();
            list.AddRange(objectList);

            ;

            if (objectList.Count > 1)
            {
                //t = objectList[0].GetType();

            }

            Type type = typeof(ArrayList);
            Type[] sk = new Type[1];
            sk[0] = objectList[0].GetType();

            StreamTool.SerializeXML(type, sk, list, "sdfsd.xml");

            return true;
        }

        public IList GetAllObject()
        {
            Type type = typeof(ArrayList);// Type.GetType("Core.Serialize.XML.XmlSerializeDefine");
            Type[] sk = new Type[1];
            sk[0] = typeof(XmlSerializeDefine);


            IList l = StreamTool.DeserializeXml(type, sk, "sdfsd.xml") as IList;

            return l;


            //IList list = new ArrayList();

            //XmlSerializeService service = new XmlSerializeService();


            //return service.XmlSerializeDefineManager.XmlSerializeDefineList;
        }

        #endregion
    }


    public class Bo : ICommonObjectService, ICommonBusiObject
    {
        public IObjectSerialize objectSerialize { get; set; }

        #region ICommonObjectService 成员

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

        public bool SaveAllObject(IList objectList)
        {
            throw new NotImplementedException();
        }

        public IList GetAllObject()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region ICommonBusiObject 成员

        public void OnBeforeSave(object obj)
        {
        }

        public void OnAfterSave(object obj)
        {
        }

        public void OnBeforeUpdate(object obj)
        {
        }

        public void OnAfterUpdate(object obj)
        {
         }

        public void OnBeforeDelete(object obj)
        {
        }

        public void OnAfterDelete(object obj)
        {
        }

        #endregion
    }
}