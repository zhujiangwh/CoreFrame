using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Globalization;
using System.Data;
using System.Collections;

namespace JZT.Utility
{
    public class RowToEntity
    {
        public static object ToEntity(DataRow adaptedRow, Type entityType)
        {
            if (entityType == null || adaptedRow == null)
            {
                return null;
            }

            object entity = Activator.CreateInstance(entityType);
            CopyToEntity(entity, adaptedRow);

            return entity;
        }

        public static T ToEntity<T>(DataRow adaptedRow, T value) where T : new()
        {
            T item = new T();
            if (value == null || adaptedRow == null)
            {
                return item;
            }

            item = Activator.CreateInstance<T>();
            CopyToEntity(item, adaptedRow);

            return item;
        }

        public static Hashtable PropertyInfiCache = new Hashtable();

        public static PropertyInfo[] GetPropertyInfo(object entity )
        {
            string fullClassName = entity.GetType().FullName;
            PropertyInfo[] propertyInfoList = PropertyInfiCache[fullClassName] as PropertyInfo[]; 

            if (PropertyInfiCache[fullClassName] == null)
            {
                propertyInfoList = entity.GetType().GetProperties();
                PropertyInfiCache.Add(fullClassName, propertyInfoList);
            }
 

            return propertyInfoList;
        }

        public static Hashtable GetPropertyInfo1(object entity)
        {
            Hashtable hashProperty = new Hashtable();

            PropertyInfo[] propertyList = RowToEntity.GetPropertyInfo(entity);

            for (int i = 0; i < propertyList.Length; i++)
            {
                hashProperty.Add(propertyList[i].Name, propertyList[i]);
            }

            return hashProperty;
        }

        public static void CopyToEntity(object entity, DataRow adaptedRow)
        {
            if (entity == null || adaptedRow == null)
            {
                return;
            }
            PropertyInfo[] propertyInfos = GetPropertyInfo(entity);// entity.GetType().GetProperties();

            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                if (!CanSetPropertyValue(propertyInfo, adaptedRow))
                {
                    continue;
                }

                try
                {
                    if (adaptedRow[propertyInfo.Name] is DBNull)
                    {
                        propertyInfo.SetValue(entity, null, null);
                        continue;
                    }
                    SetPropertyValue(entity, adaptedRow, propertyInfo);
                }
                finally
                {

                }
            }
        }


        public static Hashtable CheckBolumn = new Hashtable();


        /// <summary>
        /// 检查数据行与对象是否对应。
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="adaptedRow"></param>
        /// <returns></returns>
        public static string CheckColumn(object entity, DataRow adaptedRow)
        {
            if (entity == null || adaptedRow == null)
            {
                return string.Empty;
            }

            //如果已经检查过就不需要再检查
            if (CheckBolumn.ContainsKey(entity.GetType().ToString()))
            {
                return string.Empty ;
            }


            string s = string.Empty;

            StringBuilder sb = new StringBuilder();
            PropertyInfo[] propertyInfos = GetPropertyInfo(entity);

            sb.Append(string.Format("正在检查类 {0} 。 {1}", entity.GetType().ToString(),Environment.NewLine));

            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                if (!adaptedRow.Table.Columns.Contains(propertyInfo.Name))
                {
                    sb.Append(string.Format(" 属性 {0} ， 没有数据列对应 。 {1}", propertyInfo.Name ,Environment.NewLine ));
                }
            }


            CheckBolumn.Add(entity.GetType().ToString() , sb.ToString());

            return sb.ToString() ;
        }

        private static bool CanSetPropertyValue(PropertyInfo propertyInfo, DataRow adaptedRow)
        {
            if (!propertyInfo.CanWrite)
            {
                return false;
            }

            if (!adaptedRow.Table.Columns.Contains(propertyInfo.Name))
            {
                return false;
            }

            return true;
        }

        public static void SetPropertyValue1(object entity, string value, PropertyInfo propertyInfo)
        {
            if (propertyInfo.PropertyType == typeof(DateTime?) || propertyInfo.PropertyType == typeof(DateTime))
            {
                DateTime date = DateTime.MaxValue;
                DateTime.TryParse(value, CultureInfo.CurrentCulture, DateTimeStyles.None, out date);

                propertyInfo.SetValue(entity, date, null);
            }
            else if (propertyInfo.PropertyType == typeof(int?) || propertyInfo.PropertyType == typeof(int))
            {
                int i = 0;
                if (!string.IsNullOrEmpty(value))
                {
                    i = Convert.ToInt32(value);
                }

                propertyInfo.SetValue(entity, i, null);
            }
            else if (propertyInfo.PropertyType == typeof(long?) || propertyInfo.PropertyType == typeof(long))
            {
                propertyInfo.SetValue(entity, Convert.ToInt64(value), null);
            }
            else if (propertyInfo.PropertyType == typeof(bool?) || propertyInfo.PropertyType == typeof(bool))
            {
                propertyInfo.SetValue(entity, Convert.ToBoolean(value), null);
            }
            else if (propertyInfo.PropertyType == typeof(decimal?) || propertyInfo.PropertyType == typeof(decimal))
            {
                decimal i = 0;

                if (!string.IsNullOrEmpty(value))
                {
                    i = Convert.ToDecimal(value);
                }

                propertyInfo.SetValue(entity, i, null);
            }
            else if (propertyInfo.PropertyType == typeof(string))
            {
                propertyInfo.SetValue(entity, value, null); //需要 取消 后置空格 ，否则系统报错，原因不明。
            }
            else
            {
                //处理枚举。 枚举值为字符串。
                propertyInfo.SetValue(entity, Enum.Parse(propertyInfo.PropertyType, value), null); //需要 取消 后置空格 ，否则系统报错，原因不明。
            }
        }


        private static void SetPropertyValue(object entity, DataRow adaptedRow, PropertyInfo propertyInfo)
        {
            if (propertyInfo.PropertyType == typeof(DateTime?) || propertyInfo.PropertyType == typeof(DateTime))
            {
                DateTime date = DateTime.MaxValue;
                DateTime.TryParse(adaptedRow[propertyInfo.Name].ToString(), CultureInfo.CurrentCulture, DateTimeStyles.None, out date);

                propertyInfo.SetValue(entity, date, null);
            }
            else if (propertyInfo.PropertyType == typeof(int?) || propertyInfo.PropertyType == typeof(int))
            {
                int i = 0;

                if (!string.IsNullOrEmpty(adaptedRow[propertyInfo.Name].ToString().Trim()))
                {
                    i = Convert.ToInt32(adaptedRow[propertyInfo.Name]);
                }

                propertyInfo.SetValue(entity,i , null);
            }
            else if (propertyInfo.PropertyType == typeof(long?) || propertyInfo.PropertyType == typeof(long))
            {
                propertyInfo.SetValue(entity, Convert.ToInt64(adaptedRow[propertyInfo.Name]), null);
            }
            else if (propertyInfo.PropertyType == typeof(bool?) || propertyInfo.PropertyType == typeof(bool))
            {
                propertyInfo.SetValue(entity, Convert.ToBoolean(adaptedRow[propertyInfo.Name]), null);
            }
            else if (propertyInfo.PropertyType == typeof(decimal?) || propertyInfo.PropertyType == typeof(decimal))
            {
                decimal  i = 0;

                if (!string.IsNullOrEmpty(adaptedRow[propertyInfo.Name].ToString().Trim()))
                {
                    i = Convert.ToDecimal(adaptedRow[propertyInfo.Name]);
                }

                propertyInfo.SetValue(entity, i, null);
            }
            else
            {
                string s = adaptedRow[propertyInfo.Name].ToString();
                propertyInfo.SetValue(entity, adaptedRow[propertyInfo.Name].ToString().Trim(), null); //需要 取消 后置空格 ，否则系统报错，原因不明。
            }


            //if (propertyInfo.PropertyType == typeof(DateTime))
            //{ return; }

            //object o = Convert.ChangeType( adaptedRow[propertyInfo.Name] , propertyInfo.PropertyType );

            //Type t = entity.GetType();

            //t.InvokeMember(propertyInfo.Name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetProperty, null, entity, new object[] {o });

        }
    }
}
