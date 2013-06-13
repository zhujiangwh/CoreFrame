using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Xml.Serialization;
using Core.Architecure;
using Core.DB.NHibernate;
using NHibernate;
using NHibernate.Cfg;

namespace Core.DB
{
    public enum DataBaseType
    {
        SQLServer, Oracle , Sqlite
    }

    [Serializable]
    public class NHiberanteConfig
    {
        public NHiberanteConfig(DBServer dbServer)
        {
            Initial(dbServer);
        }

        public void Initial(DBServer dbServer)
        {
            Configuration cfg = new Configuration();

            try
            {
                cfg.Properties.Add("connection.driver_class", dbServer.Nh_connection_driver_class);
                cfg.Properties.Add("connection.connection_string", dbServer.GetNHConnection());
                cfg.Properties.Add("dialect", dbServer.Nh_dialect);
                cfg.Properties.Add("show_sql", dbServer.Nh_Show_sql);
                cfg.Properties.Add("adonet.batch_size", dbServer.Nh_adonet_batchsize);
                cfg.Properties.Add("use_outer_join", "true");
                cfg.Properties.Add("command_timeout", dbServer.Nh_command_timeout);
                cfg.Properties.Add("query.substitutions", dbServer.Nh_query_substitutions);

                foreach (string item in dbServer.HBMAssemblyList)
                {
                    cfg.AddAssembly(item);
                }

                _sessionFactory = cfg.BuildSessionFactory();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private ISessionFactory _sessionFactory;


        /// <summary>
        /// 获取一个 session
        /// </summary>
        /// <returns></returns>
        public ISession GetSession()
        {
            DateTime dt = DateTime.Now;
            ISession session = _sessionFactory.OpenSession();
            return session;
        }
    }

    [Serializable]
    public class DBServer : Server
    {
        [Category("02 数据库服务器")]
        [Description("数据库类型")]
        [XmlAttribute]
        public virtual DataBaseType DataBaseType { get; set; }

        [Category("02 数据库服务器")]
        [Description("连接串 。 [Provider=SQLOLEDB;database=jointown;Server=192.168.88.16;uid=jzterp;pwd=jzterp;]")]
        [XmlAttribute]
        public string ConnectString { get; set; }


        [XmlAttribute]
        public virtual string Database { get; set; }
        [XmlAttribute]
        public virtual string UserID { get; set; }
        [XmlAttribute]
        public virtual string Password { get; set; }
        [XmlAttribute]
        public virtual int MinPoolSize { get; set; }
        [XmlAttribute]
        public virtual int MaxPoolSize { get; set; }

        [XmlAttribute]
        public virtual string Nh_connection_driver_class { get; set; }
        [XmlAttribute]
        public virtual string Nh_connection_connection_string { get; set; }
        [XmlAttribute]
        public virtual string Nh_dialect { get; set; }
        [XmlAttribute]
        public virtual string Nh_Show_sql { get; set; }
        [XmlAttribute]
        public virtual string Nh_adonet_batchsize { get; set; }
        [XmlAttribute]
        public virtual string Nh_command_timeout { get; set; }
        [XmlAttribute]
        public virtual string Nh_query_substitutions { get; set; }




        public List<string> HBMAssemblyList { get; set; }

        private NHiberanteConfig nhiberanteConfig;

        public DBServer()
        {
            HBMAssemblyList = new List<string>();
        }



        public NHiberanteService GetNHiberanteService()
        {
            if (nhiberanteConfig == null)
            {
                nhiberanteConfig = new NHiberanteConfig(this);
            }

            return new NHiberanteService(nhiberanteConfig.GetSession());
        }






        public virtual string GetNHConnection()
        {
            string connection = string.Empty;

            bool flag = true;

            string nhConnection = string.Empty;

            switch (DataBaseType)
            {
                case DataBaseType.SQLServer:
                    connection = @"Data source={0}; Initial Catalog ={1};User Id={2};Password={3};Min Pool Size={4};Max Pool Size={5}";
                    nhConnection = string.Format(connection, IPAddress, Database, UserID, Password, MinPoolSize, MaxPoolSize);
                    break;
                case DataBaseType.Oracle:
                    break;
                case DataBaseType.Sqlite:
                    connection = @"Data Source={0}";
                    nhConnection = string.Format(connection, Database);
                    break;
                default:
                    break;
            }

            return nhConnection;
        }

        public virtual string GetAdoConnection()
        {
            string connection = string.Empty;

            string con = string.Empty;

            switch (DataBaseType)
            {
                case DataBaseType.SQLServer:
                    connection = @" Provider=SQLOLEDB;Data source={0}; Initial Catalog={1}; User Id={2};Password={3};Min Pool Size=5;Max Pool Size=50";
                    con = string.Format(connection, IPAddress, Database, UserID, Password, MinPoolSize, MaxPoolSize);
                    break;
                case DataBaseType.Oracle:
                    connection = @"Data source={0}/{1};User Id={2};Password={3};Min Pool Size={4};Max Pool Size={5}";
                    con = string.Format(connection, IPAddress, Database, UserID, Password, MinPoolSize, MaxPoolSize);
                    break;
                case DataBaseType.Sqlite:
                    break;
                default:
                    break;
            }

            return con; ;
        }



        public IDbConnection GetConnect()
        {
            return new OleDbConnection(ConnectString);
        }



        public void Test()
        {
            using (OleDbConnection connect = new OleDbConnection(ConnectString))
            {
                connect.Open();
                OleDbCommand command = new OleDbCommand("SELECT GETDATE()", connect);
                object obj = command.ExecuteScalar();
            }

        }

        public int ExecuteNonQuery(string sql)
        {
            int count = 0;
            using (OleDbConnection connect = new OleDbConnection(ConnectString))
            {
                connect.Open();
                OleDbCommand command = new OleDbCommand(sql, connect);
                count = command.ExecuteNonQuery();
            }

            return count;

        }

        public DataTable GetDateTalbe(string sql)
        {
            return GetDataSet(sql).Tables[0];
        }

        public DataSet GetDataSet(string sql)
        {
            DataSet dataSet = new DataSet();

            List<string> list = GetSqlList(sql);

            return GetDataSet(list);

            //using (OleDbConnection connect = new OleDbConnection(ConnectString))
            //{
            //    connect.Open();
            //    OleDbDataAdapter command = new  OleDbDataAdapter(sql,connect);
            //    command.Fill(dataSet);
            //}

            //return dataSet;
        }

        public DataSet GetDataSet(List<string> sqlList)
        {
            DataSet dataSet = new DataSet();

            using (OleDbConnection connect = new OleDbConnection(GetAdoConnection()))
            {
                connect.Open();

                foreach (string sql in sqlList)
                {
                    OleDbDataAdapter command = new OleDbDataAdapter(sql, connect);
                    DataTable table = new DataTable();
                    command.Fill(table);

                    dataSet.Tables.Add(table);
                }
            }

            return dataSet;
        }

        protected List<string> GetSqlList(string sqlScripe)
        {
            List<string> SQLList = new List<string>();

            string[] s = sqlScripe.Split(';');

            foreach (string item in s)
            {
                if (!string.IsNullOrEmpty(item.Trim()))
                {
                    SQLList.Add(item);
                }
            }

            return SQLList;

        }

        public override string GetText()
        {
            return string.Format("[{0}:{1}]  ", Name, ConnectString);
        }

    }

    [Serializable]
    public class DBServerManager
    {
        public List<DBServer> DBServerList { get; set; }

        public virtual string DefaultDBServerName { get; set; }

        [XmlIgnore]
        public DBServer DefaultDBServer
        {
            get
            {
                return this[DefaultDBServerName];
            }
        }

        public DBServerManager()
        {
            DBServerList = new List<DBServer>();
        }

        public DBServer this[string dbServerName]
        {
            get
            {
                return DBServerList.Find(delegate(DBServer c3) { return c3.Name == dbServerName; });
            }
        }


    }

}
