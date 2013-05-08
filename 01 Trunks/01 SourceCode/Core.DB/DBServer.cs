using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Architecure;
using System.ComponentModel;
using System.Data.OleDb;
using System.Data;
using NHibernate;
using NHibernate.Cfg;
using System.Xml.Serialization;

namespace Core.DB
{
    [Serializable]
    public class NHiberanteConfig
    { 
        public NHiberanteConfig()
        {
            HBMAssemblyList = new List<string>();

            connection_driver_class = "NHibernate.Driver.SqlClientDriver";
            connection_connection_string ="Data source=192.168.79.1; Initial Catalog=Core; User Id=sa;Password=gambol;Min Pool Size=5;Max Pool Size=50";
            dialect ="NHibernate.Dialect.MsSql2008Dialect";



            HBMAssemblyList.Add("HBM_SQL");

            Initial();

        }

        public virtual string connection_driver_class {get;set;}

        public virtual string connection_connection_string {get;set;}

        public virtual string dialect {get;set;}

        public List<string> HBMAssemblyList {get;set;}


        public void Initial()
        {
            Configuration cfg = new Configuration();

            try
            {

                cfg.Properties.Add("connection.driver_class", connection_driver_class);
                cfg.Properties.Add("connection.connection_string", connection_connection_string);
                cfg.Properties.Add("dialect", dialect);
                cfg.Properties.Add("show_sql", "true");
                cfg.Properties.Add("adonet.batch_size", "100");
                cfg.Properties.Add("use_outer_join", "true");
                cfg.Properties.Add("command_timeout", "10");
                cfg.Properties.Add("query.substitutions", "true 1, false 0, yes 'Y', no 'N'");


                foreach (string item in HBMAssemblyList)
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

            //LocalDataStoreSlot slot = Context.GetNamedDataSlot("__SESSION__");

            //if (slot == null)
            //    Context.AllocateNamedDataSlot("__SESSION__");

            //Context.SetData(slot, session);

            return session;
        }






    }

    [Serializable]
    public class DBServer : Server
    {
        [Category("02 数据库服务器")]
        [Description("数据库类型")]
        [XmlAttribute]
        public string DBType { get; set; }

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

        //        cfg.Properties.Add("show_sql", "true");
        //cfg.Properties.Add("adonet.batch_size", "100");
        //cfg.Properties.Add("use_outer_join", "true");
        //cfg.Properties.Add("command_timeout", "10");
        //cfg.Properties.Add("query.substitutions", "true 1, false 0, yes 'Y', no 'N'");


        public List<string> HBMAssemblyList { get; set; }

        [XmlIgnore]
        public NHiberanteConfig NHiberanteConfig { get; set; }

        public DBServer()
        {
            NHiberanteConfig = new NHiberanteConfig();

            Name = "名字";
            Code = "代码";
            Text = "说明";
            IPAddress = "127.0.0.1";
            Database = "vmxp";
            UserID = "sa";
            Password = "gambol";
            MinPoolSize = 5;
            MaxPoolSize = 50;

            Nh_connection_driver_class = "NHibernate.Driver.SqlClientDriver";
            Nh_dialect = "NHibernate.Dialect.MsSql2008Dialect";
            Nh_Show_sql = "true";
            Nh_adonet_batchsize = "100";
            Nh_command_timeout = "10";
            Nh_query_substitutions = "true 1, false 0, yes 'Y', no 'N'";

        }




        public virtual string GetNHConnection()
        {
            string connection = string.Empty;

            bool flag = true;

            if (flag)
            {
                connection = @"Data source={0}/{1};User Id={2};Password={3};Min Pool Size={4};Max Pool Size={5}";
            }
            else
            {
                connection = @"Data source={0}; Initial Catalog={1}; User Id={2};Password={3};Min Pool Size=5;Max Pool Size=50";
            }

            return string.Format(connection, IPAddress, Database, UserID, Password, MinPoolSize, MaxPoolSize);
        }

        public virtual string GetAdoConnection(string dbtype)
        {
            string connection = string.Empty;

            bool flag = true;

            if (flag)
            {
                connection = @"Data source={0}/{1};User Id={2};Password={3};Min Pool Size={4};Max Pool Size={5}";
            }
            else
            {
                connection = @" Provider=SQLOLEDB;Data source={0}; Initial Catalog={1}; User Id={2};Password={3};Min Pool Size=5;Max Pool Size=50";
            }

            return string.Format(connection, IPAddress, Database, UserID, Password, MinPoolSize, MaxPoolSize);
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

            using (OleDbConnection connect = new OleDbConnection(ConnectString))
            {
                connect.Open();

                foreach (string sql in sqlList)
                {

                    OleDbDataAdapter command = new OleDbDataAdapter(sql, connect);
                    DataTable table = new DataTable();
                    command.Fill(table);

                    dataSet.Tables.Add(table);
                }
                //command.Fill(dataSet);
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


    public class DBServerL
    {
        private static DBServerL instance;

        public static DBServerL GetInstance()
        {
            if (instance == null)
            {
                instance = new DBServerL();
            }
            return instance;
        }

        public List<DBServer> DBServerList { get; set; }

        public DBServer DefaultDBServer { get; set; }

        private DBServerL()
        {
            DBServerList = new List<DBServer>();
            DefaultDBServer = new DBServer();
        }
 
    }

}
