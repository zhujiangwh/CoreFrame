using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Core.UI;
using JZT.Utility;
using Core.Architecure;
using Core.Metadata;
using Core.Server;
using Core.Serialize.XML;

namespace TestWin
{


    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private ControlCreater controlCreater ;//= new ControlCreater();


        private void button1_Click(object sender, EventArgs e)
        {
            controlCreater = new ControlCreater();

            UIControlDefine c1 = new UIControlDefine();
            c1.Left = 10;
            c1.Top = 10;
            c1.Width = 50;
            c1.Heigth = 20;



            UIControlDefine c2 = new UIControlDefine();
            c2.Left = 10;
            c2.Top = 30;
            c2.Width = 200;
            c2.Heigth = 100;

 
            //测试一下代码。


            controlCreater.UIControlDefineList.Add(c1);
            controlCreater.UIControlDefineList.Add(c2);


            controlCreater.ParentControl = this.panel1;

            controlCreater.Create();

            //Type[] types = new Type[1]; ;
            //types[0] = typeof(ControlDefine);
            //types[1] = typeof(ObjectDefine);


            //StreamTool.SerializeXML( controlCreater, "sdf.xml");




            //StreamTool.SerializeXML(typeof(ControlCreater),types, controlCreater,"sdf.xml");



        }

        private void button2_Click(object sender, EventArgs e)
        {
            controlCreater.UIControlDefineList[0].SetText();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            controlCreater = StreamTool.DeserializeXml<ControlCreater>("sdf.xml");

            BusiDataItem dataItem = new BusiDataItem();
            dataItem.Caption = "Cation";
            dataItem.Name = "property";
            dataItem.ControlTypeName = "System.Windows.Forms.Label";

            BusiDataItem dataItem1 = new BusiDataItem();
            dataItem1.Caption = "标题";
            dataItem1.Name = "属性";
            dataItem1.ControlTypeName = "System.Windows.Forms.Button";


            //把数据项生成控件。
            controlCreater.UIControlDefineList.AddRange(
                controlCreater.CreateUIControlDefine(dataItem));
            controlCreater.UIControlDefineList.AddRange(
                controlCreater.CreateUIControlDefine(dataItem1));


            controlCreater.ParentControl = this.panel1;

            controlCreater.Create();


            


        }

        private void button4_Click(object sender, EventArgs e)
        {
            StreamTool.SerializeXML(controlCreater, "sdf.xml");

        }

        private void button5_Click(object sender, EventArgs e)
        {
            controlCreater.UIControlDefineList[0].Focus();

        }

        private void button6_Click(object sender, EventArgs e)
        {
            BusiDataItem dataItem = new BusiDataItem();
            dataItem.Caption = "Cation";

        }

        private void button7_Click(object sender, EventArgs e)
        {
            CommonObjectService service = new CommonObjectService(new XmlSerialize());

            service.Create(controlCreater);


        }
    }
}
