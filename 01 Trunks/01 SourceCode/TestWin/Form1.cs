using System;
using System.Collections;
using System.Windows.Forms;
using Core.Architecure;
using Core.Common;
using Core.Metadata;
using Core.Serialize.XML;
using Core.Server;
using Core.UI;
using JZT.Utility;
using System.Reflection;

namespace TestWin
{


    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private ControlCreater controlCreater;//= new ControlCreater();


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
            //controlCreater = StreamTool.DeserializeXml<ControlCreater>("sdf.xml");

            //BusiDataItem dataItem = new BusiDataItem();
            //dataItem.Caption = "Cation";
            //dataItem.Name = "property";
            //dataItem.ControlTypeName = "System.Windows.Forms.Label";

            //BusiDataItem dataItem1 = new BusiDataItem();
            //dataItem1.Caption = "标题";
            //dataItem1.Name = "属性";
            //dataItem1.ControlTypeName = "System.Windows.Forms.Button";


            ////把数据项生成控件。
            //controlCreater.UIControlDefineList.AddRange(
            //    controlCreater.CreateUIControlDefine(dataItem));
            //controlCreater.UIControlDefineList.AddRange(
            //    controlCreater.CreateUIControlDefine(dataItem1));


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
            ICommonObjectService service = CommonObjectCreater.CreateCommonObjectService();
            service.Create(controlCreater);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            ICommonObjectService service = CommonObjectCreater.CreateCommonObjectService();
            controlCreater = service.GetObject<ControlCreater>("formde");
        }

        private void button9_Click(object sender, EventArgs e)
        {
            ICommonObjectService service = CommonObjectCreater.CreateCommonObjectService();

            XmlSerializeDefineManager ma =  XmlSerializeDefineManager.GetInstance();
            service.Create(ma);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            XmlSerializeDefineManager ma = XmlSerializeDefineManager.GetInstance();

            //ICommonObjectService service = CommonObjectCreater.CreateCommonObjectService();

            //XmlSerializeDefineManager k = service.GetObject<XmlSerializeDefineManager>("对象序列化配置");
        }

        private void button11_Click(object sender, EventArgs e)
        {
            ICommonObjectService service = CommonObjectCreater.CreateCommonObjectService();

            BusiObjectPool pool = BusiObjectPool.GetInstance();
            BusiObjectDefine b = new BusiObjectDefine();
            b.FullClassName = "classname";
            b.BusiObjectDefi = new ObjectDefine("assembly", "name");

            pool.BusiObjectDefineList.Add(b);



            service.Create(pool);


        }

        private void button12_Click(object sender, EventArgs e)
        {
            ICommonObjectService service = CommonObjectCreater.CreateCommonObjectService();


        }

        private void button13_Click(object sender, EventArgs e)
        {
            EditAllObjectUIC uic = new EditAllObjectUIC();

            uic.EditAllObjectDisplayUIDefine.AssemblyName = "WinUI";
            uic.EditAllObjectDisplayUIDefine.FullClassName = "Core.UI.EditAllObjectForm";

            uic.ObjectTypeDefine.AssemblyName = "Core";
            uic.ObjectTypeDefine.FullClassName = //"Core.UI.SelectObjectUIC";
            //    "Core.Serialize.XML.XmlSerializeDefine";
            //"Core.Authority.Function";
            "Core.Architecure.ObjectDefine";

            uic.service = CommonObjectCreater.CreateCommonObjectService();

            uic.Initial();


            DispalyForm(uic.EditAllObjectDisplayUI as Form);

        }

        private void DispalyForm(Form form)
        {
            form.Show();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            XmlSerializeDefine xmlSerializeDefine = new XmlSerializeDefine("Core.Serialize.XML.XmlSerializeDefine", "XXX", "");

            //IList<XmlSerializeDefine> list = new List();

            ArrayList l = new ArrayList(); 



            l.Add(new XmlSerializeDefine("1", "1", "1"));
            l.Add(new XmlSerializeDefine("2", "2", "2"));
            l.Add(new XmlSerializeDefine("3", "3", "3"));



            //xmlSerializeDefine.SaveAllObject(l);


        }

        private void button15_Click(object sender, EventArgs e)
        {
            XmlSerializeDefine xmlSerializeDefine = new XmlSerializeDefine("Core.Architecure.ObjectDefine", "XXX", "");

            IList list = xmlSerializeDefine.GetAllObject();

            xmlSerializeDefine.SaveAllObject(list);


        }

        private void button16_Click(object sender, EventArgs e)
        {
            AppDomain app = AppDomain.CurrentDomain;

            Assembly asm = Assembly.Load("Core");//.LoadFrom("Core.dll");

            Type[] s = asm.GetTypes();

            ArrayList arrayList  = new ArrayList();

            foreach ( Type t in s)
            {
                ObjectDefine objectDefine = new ObjectDefine(t);

                arrayList.Add(objectDefine);


            }


            


        }
    }
}
