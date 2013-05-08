using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Serialize;
using NHibernate;
using NHibernate.Cfg;
using Core.Common;
using System.Collections;
using System.Data;
using Core.Serialize.DB;
using System.Data.OleDb;
using Core.Architecure;


namespace Core.DB.NHibernate
{
    public class NHiberanteService :  IDisposable
    {

        public ISession Session { get; set; }

        public NHiberanteService(ISession session)
        {
            Session = session;
        }

        #region IObjectSerialize 成员

        public DataTable GetDataTable(string sql )
        {
            ISession session = Session;

            string connectString = string.Format(" {0} {1} {2}", @"Provider=SQLOLEDB;", "Password=gambol;", session.Connection.ConnectionString);

            OleDbConnection connect = new OleDbConnection(connectString);

            OleDbDataAdapter ada = new OleDbDataAdapter(sql, connect);

            DataTable table = new DataTable() ;
            ada.Fill(table);

            return  table;
        }

        public void Save(object obj)
        {
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

            Session.Save(obj);
        }

        public void Update(object obj)
        {
            //处理最后更新时间。
            IUpdateTime updateTime = obj as IUpdateTime;
            if (updateTime != null)
            {
                updateTime.LastModifyTime = DateTime.Now;
            }

            Session.Update(obj);
        }

        public void Delete(object obj)
        {
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

            Session.Save(obj);
        }

        public void RealDelete(object obj)
        {
            Session.Delete(obj);
        }

        public virtual IList GetObject( SqlScript sqlScript)
        {
            IList infos = null;

            try
            {
                IQuery query = Session.CreateQuery(sqlScript.Sql);

                if (sqlScript.ParamList != null)
                {
                    //为脚本设置参数。
                    foreach (SqlParam item in sqlScript.ParamList)
                    {
                        switch (item.ParamType.ToString())
                        {
                            //字符串
                            case "String":
                                query.SetAnsiString(item.ParamName, Convert.ToString(item.ParamValue));
                                break;
                            case "AnsiString":
                                query.SetAnsiString(item.ParamName, Convert.ToString(item.ParamValue));
                                break;
                            case "AnsiStringFixedLength":
                                query.SetAnsiString(item.ParamName, Convert.ToString(item.ParamValue));
                                break;
                            case "StringFixedLength":
                                query.SetAnsiString(item.ParamName, Convert.ToString(item.ParamValue));
                                break;
                            //布尔
                            case "Boolean":
                                query.SetBoolean(item.ParamName, Convert.ToBoolean(item.ParamValue));
                                break;
                            //整型
                            case "Byte":
                                query.SetByte(item.ParamName, Convert.ToByte(item.ParamValue));
                                break;
                            case "Int16":
                                query.SetInt16(item.ParamName, Convert.ToInt16(item.ParamValue));
                                break;
                            case "Int32":
                                query.SetInt32(item.ParamName, Convert.ToInt32(item.ParamValue));
                                break;
                            case "Int64":
                                query.SetInt64(item.ParamName, Convert.ToInt64(item.ParamValue));
                                break;
                            case "UInt16":
                                query.SetInt16(item.ParamName, Convert.ToInt16(item.ParamValue));
                                break;
                            case "UInt32":
                                query.SetInt32(item.ParamName, Convert.ToInt32(item.ParamValue));
                                break;
                            case "UInt64":
                                query.SetInt64(item.ParamName, Convert.ToInt64(item.ParamValue));
                                break;
                            //日期
                            case "Date":
                                query.SetDateTime(item.ParamName, Convert.ToDateTime(item.ParamValue));
                                break;
                            case "DateTime":
                                query.SetDateTime(item.ParamName, Convert.ToDateTime(item.ParamValue));
                                break;
                            case "DateTime2":
                                query.SetDateTime(item.ParamName, Convert.ToDateTime(item.ParamValue));
                                break;
                            case "DateTimeOffset":
                                break;
                            //浮点
                            case "Single":
                                query.SetSingle(item.ParamName, Convert.ToSingle(item.ParamValue));
                                break;
                            case "Double":
                                query.SetDouble(item.ParamName, Convert.ToDouble(item.ParamValue));
                                break;
                            case "Decimal":
                                query.SetDecimal(item.ParamName, Convert.ToDecimal(item.ParamValue));
                                break;
                            case "Currency":
                                query.SetDecimal(item.ParamName, Convert.ToDecimal(item.ParamValue));
                                break;
                            case "VarNumeric":
                                query.SetDecimal(item.ParamName, Convert.ToDecimal(item.ParamValue));
                                break;
                            //杂类
                            case "Guid":
                                break;
                            case "Object":
                                break;
                            default:
                                throw new Exception(string.Format("没有找到匹配参数 {0} ！", item.ParamType.ToString()));
                        }
                    }
                }

                infos = query.List();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return infos;
        }

        #endregion

        #region IDisposable 成员

        public void Dispose()
        {
            //sessionManager.Dispose();
        }

        #endregion
    }

}
