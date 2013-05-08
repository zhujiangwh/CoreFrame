using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Data;
using System.Reflection;

namespace JZT.Utility
{
    public class UtilList
    {
        public static DataTable ToDataTable(IList list)
        {
            if (list.Count > 0)
            {
                DataTable result = CreateTableSchema(list[0].GetType());
                return ToDataTable(result, list);
            }
            return null;
           
        }

        public static DataTable ToDataTable(DataTable result, IList list)
        {
            if (list!=null && list.Count > 0)
            {
              
                for (int i = 0; i < list.Count; i++)
                {
                    ToDataTable(result, list[i]);
                }
            }
            return null;
        }

        public static DataTable ToDataTable(DataTable dt, object obj)
        {
            PropertyInfo[] propertys = obj.GetType().GetProperties();
            ArrayList tempList = new ArrayList();
            foreach (PropertyInfo pi in propertys)
            {
                if (pi.PropertyType.IsValueType || pi.PropertyType == typeof(string) || pi.PropertyType.Name.IndexOf("Nullable") > -1)
                {
                    object objRow = pi.GetValue(obj, null);
                    tempList.Add(objRow);
                }
            }
            object[] array = tempList.ToArray();
            dt.LoadDataRow(array, true);
            return dt;
        }

        public static void ToDataSet(DataSet ds, object obj)
        {
            DataTable dt = ToDataTable(ds.Tables[0], obj);
            int i = 1;
            foreach (PropertyInfo pi in obj.GetType().GetProperties())
            {
                if (pi.PropertyType.Name.IndexOf("IList`1") > -1)
                {
                    if (pi.PropertyType.GetGenericArguments().Length > 0)
                    {
                        ToDataTable(ds.Tables[i], pi.GetValue(obj, null) as IList);
                        i++;
                    }
                }
            }
        }
        public static void ToDataSetByMultDt(DataSet ds, object obj)
        {
            DataTable dt = ToDataTable(ds.Tables[0], obj);
            int i = 1;
            foreach (PropertyInfo pi in obj.GetType().GetProperties())
            {
                if (pi.PropertyType.Name.IndexOf("IList`1") > -1)
                {
                   
                    if (pi.PropertyType.GetGenericArguments().Length > 0 )
                    {
                        ToDataTable(ds.Tables[i], pi.GetValue(obj, null) as IList);
                        i++;
                    }
                   
                }
            }
        }
        public static DataSet CreateDataSetSchema(Type t)
        {
            DataSet ds = new DataSet();
            ds.Tables.Add(CreateTableSchema(t));
            foreach (PropertyInfo pi in t.GetProperties())
            {
                // List 类型 添加字表
                if (pi.PropertyType.Name.IndexOf("IList`1") > -1)
                {
                    if (pi.PropertyType.GetGenericArguments().Length > 0)
                    {
                        ds.Tables.Add(CreateTableSchema(pi.PropertyType.GetGenericArguments()[0]));
                    }
                }
            }
            return ds;
        }
       
        public static DataTable CreateTableSchema(Type tp)
        {
            DataTable dt = new DataTable();
            dt.TableName = tp.Name;
            return CreateTableSchema(tp, dt);
        }

        public static DataTable CreateTableSchema(Type tp, DataTable dt)
        {
            PropertyInfo[] pis = tp.GetProperties();
            foreach (PropertyInfo pi in pis)
            {
                if (pi.PropertyType.Name.IndexOf("Nullable") > -1)
                {
                    DataColumn dc = new DataColumn(pi.Name, typeof(string));
                    dt.Columns.Add(dc);
                    continue;
                }
                if (pi.PropertyType.IsValueType || pi.PropertyType == typeof(string))
                {
                    DataColumn dc = new DataColumn(pi.Name, pi.PropertyType);
                    dt.Columns.Add(dc);
                }

            }
            return dt;
        }






        /// <summary>
        /// 枚举元素，统一执行某项操作
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.ComponentModel.Browsable(false)]
        public static void Foreach<T>(IEnumerable<T> input, Action<T> action)
        {
            if (input == null)
                throw new ArgumentNullException("input");
            if (action == null)
                throw new ArgumentNullException("action");
            foreach (T element in input)
            {
                action(element);
            }
        }

        /// <summary>
        /// 合并集合
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCode()]
        [System.Diagnostics.DebuggerStepThrough()]
        public static void AddRange<T>(ICollection<T> list, IEnumerable<T> elements)
        {
            foreach (T o in elements)
                list.Add(o);
        }


        /// <summary>
        /// 连接字符串
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCode()]
        [System.Diagnostics.DebuggerStepThrough()]
        public static string Join(string separator,IEnumerable source)
        {
            ArrayList arl = new ArrayList();
            foreach (object s in source)
            {
                if (s == null)
                {
                    throw new Exception("数据源中有null值");
                }
                arl.Add(s.ToString());
            }
            return string.Join(separator, ToArray<String>(arl));
        }

        /// <summary>
        /// 将数组变成只读集合
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCode()]
        [System.Diagnostics.DebuggerStepThrough()]
        public static ReadOnlyCollection<T> AsReadOnly<T>(T[] arr)
        {
            return Array.AsReadOnly(arr);
        }

        /// <summary>
        /// 把任意对象转换为数组
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCode()]
        [System.Diagnostics.DebuggerStepThrough()]
        public static T[] ToArray<T>(IEnumerable source)
        {
            ArrayList arl = new ArrayList();
            foreach (object obj in source)
            {
                if (!(obj is T))
                {
                    throw new Exception("数据源的内容不是" + typeof(T).FullName + "类型");
                }
                arl.Add(source);
            }
            return (T[])arl.ToArray(typeof(T));
        }

        /// <summary>
        /// 将可枚举的对象统一转换为某种数据类型
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCode()]
        [System.Diagnostics.DebuggerStepThrough()]
        public static List<T> Convert<T>(IEnumerable obj)
        {
            List<T> listEntity = new List<T>();
            foreach (T item in obj)
            {
                if (!(item is T))
                {
                    throw new Exception("数据源成员不是" + typeof(T).FullName + "类型");
                }
                listEntity.Add(item);
            }
            return listEntity;
        }

        /// <summary>
        /// 判断集合中是否存在某个元素
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.ComponentModel.Browsable(false)]
        public static bool Contains<T>(IEnumerable array, T obj)
        {
            foreach (T itm in array)
            {
                if (itm.Equals(obj))
                {
                    return true;
                }
            }
            return false;
        }

        public delegate void GroupInvokeHandlerWithKey(IEnumerable value);
        public delegate void GroupInvokeHandler(object key, IEnumerable value);

        /// <summary>
        /// 枚举某个数据源，对某个数据源进行分组，分组后的每一组对象都调用一次method方法
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCode()]
        [System.Diagnostics.DebuggerStepThrough()]
        public static void Group(object obj, string field, GroupInvokeHandler method)
        {
            if (obj == null)
            {
                throw new Exception("数据源obj不能为空");
            }
            if (method == null)
            {
                throw new Exception("method不能为空");
            }

            foreach (KeyValuePair<object, IEnumerable> pair in Group(obj, field))
            {
                method(pair.Key, pair.Value);
            }
        }

        /// <summary>
        /// 枚举某个数据源，对某个数据源进行分组，分组后的每一组对象都调用一次method方法
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCode()]
        [System.Diagnostics.DebuggerStepThrough()]
        public static void Group(object obj, string field, GroupInvokeHandlerWithKey method)
        {
            if (obj == null)
            {
                throw new Exception("数据源obj不能为空");
            }
            if (method == null)
            {
                throw new Exception("method不能为空");
            }

            foreach (KeyValuePair<object, IEnumerable> pair in Group(obj, field))
            {
                method(pair.Value);
            }
        }

        /// <summary>
        /// 枚举某个数据源，对某个数据源进行分组
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCode()]
        [System.Diagnostics.DebuggerStepThrough()]
        public static Dictionary<object, IEnumerable> Group(object obj, string field)
        {
            if (obj == null)
            {
                throw new Exception("数据源obj不能为空");
            }

            if (string.IsNullOrEmpty(field))
            {
                throw new Exception("分组字段名不能为空");
            }

            CurrencyManager cm = GetCurrencyManager(obj, string.Empty);
            if (cm == null)
            {
                throw new Exception("无法将数据源转换为枚举类型");
            }

            PropertyDescriptor pdKey = GetProperty(cm, field, false);
            if (pdKey == null)
            {
                throw new Exception("无法找到名称为" + field + "的字段");
            }

            Dictionary<object, IEnumerable> ret = new Dictionary<object, IEnumerable>();
            foreach (Object curObj in cm.List)
            {
                Object key = pdKey.GetValue(curObj);
                if (!ret.ContainsKey(key))
                {
                    ret.Add(key, new ArrayList());
                }
                ((ArrayList)ret[key]).Add(curObj);
            }
            return ret;
        }

        /// <summary>
        /// 枚举某个数据源，将制定制定分组，并将分组字段结果,与每组的行数
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        //[System.Diagnostics.DebuggerNonUserCode()]
        //[System.Diagnostics.DebuggerStepThrough()]
        public static Dictionary<string,int> GroupBy(object obj,string field)
        {
            if (obj == null)
            {
                throw new Exception("数据源obj不能为空");
            }

            if (string.IsNullOrEmpty(field))
            {
                throw new Exception("分组字段名不能为空");
            }

            CurrencyManager cm = GetCurrencyManager(obj, string.Empty);
            if (cm == null)
            {
                throw new Exception("无法将数据源转换为枚举类型");
            }
            string[] columns = field.Split(',');
            List<PropertyDescriptor> list = new List<PropertyDescriptor>();
            foreach (string te in columns)
            {
                PropertyDescriptor pdKey = GetProperty(cm, te, false);
                if (pdKey == null)
                {
                    throw new Exception("无法找到名称为" + field + "的字段");
                }
                list.Add(pdKey);
            }

            Dictionary<string, int> ret = new Dictionary<string, int>();
            foreach (Object curObj in cm.List)
            {
                string keys = "";
                foreach (PropertyDescriptor te in list)
                {
                    keys = keys +  System.Convert.ToString(te.GetValue(curObj)).Trim()+",";
                }
                keys =  keys.Remove(keys.Length - 1, 1);

                if (!ret.ContainsKey(keys))
                {
                    ret.Add(keys, 1);
                }
                else
                {
                    ret[keys] = ++ret[keys];
                }
            }
            return ret;
        }




        public static DataTable Select(object obj, string filed, string express)
        {
            if (obj == null)
            {
                throw new Exception("数据源obj不能为空");
            }

            if (string.IsNullOrEmpty(filed))
            {
                throw new Exception("分组字段名不能为空");
            }

            CurrencyManager cm = GetCurrencyManager(obj, string.Empty);
            if (cm == null)
            {
                throw new Exception("无法将数据源转换为枚举类型");
            }
            string[] columns = filed.Split(',');
            DataTable dt = new DataTable();
            List<PropertyDescriptor> list = new List<PropertyDescriptor>();
            foreach (string te in columns)
            {
                PropertyDescriptor pdKey = GetProperty(cm, te, false);
                dt.Columns.Add(te, pdKey.PropertyType);
                list.Add(pdKey);
            }
            foreach (Object curObj in cm.List)
            {
                DataRow row = dt.NewRow();
                foreach (PropertyDescriptor te in list)
                {
                    row[te.Name] = te.GetValue(curObj);
                }
                dt.Rows.Add(row);

            }
            if (!String.IsNullOrEmpty(express))
            {
                DataTable resoult = new DataTable();
                DataRow[] row = dt.Select(express);
                foreach (DataRow ro in row)
                {
                    resoult.ImportRow(ro);
                }
                return resoult;
            }
            return dt;  
        }

        public static List<string> SelectItem(object obj, string field)
        {
            if (obj == null)
            {
                throw new Exception("数据源obj不能为空");
            }

            if (string.IsNullOrEmpty(field))
            {
                throw new Exception("分组字段名不能为空");
            }

            CurrencyManager cm = GetCurrencyManager(obj, string.Empty);
            if (cm == null)
            {
                throw new Exception("无法将数据源转换为枚举类型");
            }
            string[] columns = field.Split(',');
            List<PropertyDescriptor> list = new List<PropertyDescriptor>();
            foreach (string te in columns)
            {
                PropertyDescriptor pdKey = GetProperty(cm, te, false);
                if (pdKey == null)
                {
                    throw new Exception("无法找到名称为" + field + "的字段");
                }
                list.Add(pdKey);
            }

            List<string> ret = new List<string>();
            foreach (Object curObj in cm.List)
            {
                string keys = "";
                foreach (PropertyDescriptor te in list)
                {
                    keys = keys + System.Convert.ToString(te.GetValue(curObj)).Trim() + ",";
                }
                keys = keys.Remove(keys.Length - 1, 1);

                ret.Add(keys);
            }
            return ret;
        }


        [System.Diagnostics.DebuggerNonUserCode()]
        [System.Diagnostics.DebuggerStepThrough()]
        public static PropertyDescriptor GetProperty(CurrencyManager cm, string fieldName, bool upper)
        {
            if (!upper)
            {
                return cm.GetItemProperties()[fieldName];
            }
            foreach (PropertyDescriptor pd in cm.GetItemProperties())
            {
                if (upper)
                {
                    if (pd.Name.Equals(fieldName, StringComparison.OrdinalIgnoreCase))
                    {
                        return pd;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 获取环境绑定变量<see cref="System.Windows.Forms.CurrencyManager"/>
        /// </summary>
        /// <param name="dataSource">数据源</param>
        /// <param name="dataMember">数据成员</param>
        /// <returns>返回CurrencyManager</returns>
        [System.Diagnostics.DebuggerNonUserCode()]
        [System.Diagnostics.DebuggerStepThrough()]
        public static CurrencyManager GetCurrencyManager(object dataSource, string dataMember)
        {
            try
            {
                BindingContext bc = new BindingContext();
                CurrencyManager cm = (CurrencyManager)bc[dataSource, dataMember];
                return cm;
            }
            catch
            {
                return null;
            }
        }
    }
}
