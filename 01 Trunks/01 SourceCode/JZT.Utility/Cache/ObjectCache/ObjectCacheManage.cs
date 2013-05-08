using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;



namespace JZT.Common.Cache
{
    #region 加载程序集通用类

    public class ObjectCacheManage
    {

        private static Dictionary<string,Assembly> m_assList = new Dictionary<string,Assembly>();

        private static Dictionary<string, object> objCache = new Dictionary<string, object>();

      
        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="assemblyName">程序集</param>
        /// <param name="className">类名</param>
        /// <param name="isCache">是否将创建对象添加到缓存中</param>
        /// <returns></returns>
        public static Object CreateObject(string assemblyName,string className,bool isCache)
        {


            return CreateObject(assemblyName, className, isCache, null);

           
        }
        public static Object CreateObject(string assemblyName, string className, bool isCache, object[] parameters)
        {
            if (objCache.ContainsKey(assemblyName + "." + className))
                return objCache[assemblyName + "." + className];
            else
            {
                object obj = CreateInstance(assemblyName, className, parameters);
                if (isCache)
                {
                    objCache.Add(assemblyName + "." + className, obj);
                }
                return obj;
            }
        }

       
        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="assemblyName">程序集名</param>
        /// <param name="className">类名</param>
        /// <returns></returns>
        public static Object CreateObject(string assemblyName, string className)
        {
            return CreateObject(assemblyName, className, false);
        }

        public static Object CreateObject(string assemblyName, string className, object[] parameters)
        {
            return CreateObject(assemblyName, className, false,parameters);
        }


       /// <summary>
       /// 创建对象实例
       /// </summary>
       /// <param name="assemblyName">程序集名</param>
       /// <param name="className">class名</param>
       /// <param name="parameters">参数列表</param>
       /// <returns></returns>
        private static object CreateInstance(string assemblyName, string className,object[] parameters)
        {
            Assembly asm = GetAssembly(assemblyName);
            if (asm == null)
            {
                throw new Exception(string.Format("没有找到 {0}  动态链接库 。 ", assemblyName));
            }
            object obj = null;
            try
            {
                if (asm != null)
                {
                    Type tp = asm.GetType(className);
                    

                    if (tp != null)
                    {
                        if (parameters != null)
                        {
                            ConstructorInfo constructor = tp.GetConstructor(GetTypes(parameters));
                            if (constructor != null)
                            {
                                obj = constructor.Invoke(parameters);
                            }
                        }
                        else
                        {
                            try
                            {
                                obj = asm.CreateInstance(className);
                            }
                            catch ( Exception ex )
                            {
                                throw new Exception(string.Format(@" [{0}][{1}] 创建失败 ！\n '{2}'",assemblyName, className , ex.Message.ToString()),ex);
                            }
                         }
                        return obj;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return obj;
        }

        public static Assembly GetAssembly(string assemblyName)
        {

            Assembly asm = null;
            try
            {
                if (assemblyName != null && assemblyName.Length > 0)
                {

                    if (m_assList.ContainsKey(assemblyName))
                    {
                        asm = m_assList[assemblyName];
                    }
                    else
                    {

                        string path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, assemblyName);
                        try
                        {
                            asm = Assembly.Load(assemblyName);
                        }
                        catch
                        {
                            asm = null;
                        }
                        if (asm == null)
                        {
                            try
                            {
                                asm = Assembly.LoadFile(assemblyName);
                            }
                            catch
                            {
                                asm = null;
                            }
                        }
                        if (asm == null)
                        {
                            try
                            {
                                asm = Assembly.LoadFrom(assemblyName);
                            }
                            catch
                            {
                                asm = null;
                            }
                        }
                        //if (asm != null)
                        //{
                        //    m_assList.Add(assemblyName, asm);
                        //}
                    }

                }
            }
            catch
            {
                throw new Exception("未找到的程序集");
            }
            return asm;  
        }

        /// <summary>
        /// 获得参数类型列表
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private static Type[] GetTypes(object[] parameters)
        {
            Type[] types;

            if (parameters == null || parameters.Length == 0)
                types = Type.EmptyTypes;
            else
            {
                types = new Type[parameters.Length];
                for (int i = 0; i < parameters.Length; i++)
                    types[i] = parameters[i].GetType();
            }

            return types;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <param name="className"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private static object CreateInstance(string assemblyName, string className)
        {
            return CreateInstance(assemblyName, className, null);
        }


        /// <summary>
        /// 获取对象所有简单属性列表,IList对象为  主对象.子对象
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Dictionary<string, object> GetSimpleProperty(object obj)
        {
            Type tp = obj.GetType();
            Dictionary<string, object> dict = new Dictionary<string, object>();
            foreach (PropertyInfo pi in tp.GetProperties())
            {
                 dict.Add(string.Format("{0}_{1}",tp.Name,pi.Name), pi.GetValue(obj, null));
            }
            return dict;
        }


     

    }

    #endregion


}
