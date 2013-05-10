using System;
using System.Collections;
using Core.Common;
using Core.Serialize;
using Core.Serialize.DB;
using Core.Serialize.XML;
using JZT.Utility;

namespace Core.Server
{

    public class CommonObjectService : RemotingService, ICommonObjectService
    {
        private CommonObjectStorage CommonObjectStorage { get; set; }

        public CommonObjectService()
        {
            CommonObjectStorage = new CommonObjectStorage();
        }


        #region ICommonObjectService 成员

        public object Create(object obj)
        {
            return CommonObjectStorage.Create(obj);
        }

        public void Update(object obj)
        {
            CommonObjectStorage.Update(obj);
        }

        public void Delete(object obj)
        {
            CommonObjectStorage.Delete(obj);
        }

        public T GetObject<T>(string key)
        {
            return CommonObjectStorage.GetObject<T>(key);
        }

        public IList GetObject(SqlScript sqlScript)
        {
            return CommonObjectStorage.GetObject(sqlScript);
        }

        public void RealDelete(object obj)
        {
            CommonObjectStorage.RealDelete(obj);
        }

        public bool SaveAllObject(IList objectList)
        {
            return CommonObjectStorage.SaveAllObject(objectList);
        }

        public IList GetAllObject()
        {
            return CommonObjectStorage.GetAllObject();
        }

        #endregion
    }

    public class CommonObjectStorage : ICommonObjectService
    {

        public IObjectSerialize objectSerialize { get; set; }

        public CommonObjectStorage()
        {
            //根据配置，来确定使用哪一种序列器。
            objectSerialize = new XmlSerialize();
        }

        public CommonObjectStorage(IObjectSerialize ser)
        {
            objectSerialize = ser;
        }


        private Bo GetBO(object obj)
        {
            //根据传入的对象，来确定BO .

            string fullClassName = obj.GetType().FullName;
            BusiObjectPool pool = BusiObjectPool.GetInstance();

            BusiObjectDefine busiObjectDefine = pool[fullClassName];

            Bo bo = null;
            if (busiObjectDefine == null)
            {
                bo = new Bo(objectSerialize);
            }
            else
            {
                bo = busiObjectDefine.BusiObjectDefi.CreateObject() as Bo;
                bo.objectSerialize = objectSerialize;
            }

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

        public T GetObject<T>(string key)
        {
            Bo bo = new Bo(objectSerialize);



            //Bo bo = GetBO(obj);


            return bo.GetObject<T>(key);
        }


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



}
