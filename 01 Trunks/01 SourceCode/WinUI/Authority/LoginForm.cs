using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Core.Architecure;


namespace Core.Authority
{
    public partial class LoginForm : Form, IAuthorityDisplay
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            //创建新对象.
            RemotingServer remotingServer = WinApplication.GetInstance().RemotingServer; // new RemotingServer("tcp", "127.0.0.1:8545");
            IAuthorityService service = remotingServer.CreateRemotingInterface<IAuthorityService>("AuthorityService");

            UserLoginInfo  UserLoginInfo = new UserLoginInfo();

            UserLoginInfo.UserID = textBox1.Text ;
            UserLoginInfo.Password = textBox2.Text ;



            string guid = service.Login(UserLoginInfo);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //创建新对象.
            RemotingServer remotingServer = new RemotingServer("tcp", "127.0.0.1:8545");
            IAuthorityService service = remotingServer.CreateRemotingInterface<IAuthorityService>("AuthorityService");

            service.Logout("sdfsd");

        }

        #region IAuthorityDisplay 成员

        public void DisplayUserLoginInfo(UserLoginInfo userLoginInfo)
        {
            //显示。
        }


        public AuthorityControl AuthorityControl { get; set; }

        #endregion

        private void button3_Click(object sender, EventArgs e)
        {
             //SQLiteConnection conn  =   new SQLiteConnection("Data Source=testdb");
             //conn.Open();

             //SQLiteCommand cmd = conn.CreateCommand();
             //cmd.CommandText = "SELECT * FROM test";
             //SQLiteDataReader reader = cmd.ExecuteReader();

             //if (reader.HasRows)
             //{
             //    while (reader.Read())
             //    {
             //        Console.WriteLine("PK: " + reader.GetString(0));
             //        //Console.WriteLine("name: " + reader.GetString(1));
             //    }
             //} 


        }
    }
}
