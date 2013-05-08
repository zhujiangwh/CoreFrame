using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Core.Architecure;
using System.Reflection;
using System.Xml.Serialization;
using Core.Metadata;

namespace Core.UI
{
    public class UIControlDefine
    {

        [XmlAttribute]
        public virtual string PropertyName { get; set; }

        [XmlAttribute]
        public virtual string Name { get; set; }
        [XmlAttribute]
        public virtual string Text { get; set; }



        [XmlAttribute]
        public virtual string ControlName { get; set; }

        [XmlAttribute]
        public virtual string ControlTypeName { get; set; }


        [XmlAttribute]
        public virtual int Top { get; set; }
        [XmlAttribute]
        public virtual int Left { get; set; }
        [XmlAttribute]
        public virtual int Width { get; set; }
        [XmlAttribute]
        public virtual int Heigth { get; set; }


        //public virtual ObjectDefine ControlType { get; set; }



        [XmlIgnore]
        public virtual Control RelateControl { get; set; }

        

        public UIControlDefine()
        {
            //ControlType = new ObjectDefine();
            ControlName = "";
            ControlTypeName = "";
            Text = "";
            Name = "";
            PropertyName = "";

            Width = 75;
            Heigth = 20;
        }

        public virtual void SetText()
        {
            RelateControl.Text = Text;

            Left += 1;
            RelateControl.Left += 1;
        }

        public void Focus()
        {
            RelateControl.Focus();
        }

    }

    public class ControlCreater
    {
        [XmlIgnore]
        public Control ParentControl { get; set; }

        public List<UIControlDefine> UIControlDefineList { get; set; }

        public ControlCreater()
        {
            UIControlDefineList = new List<UIControlDefine>();
        }


        private Assembly GetAssembly(string assemblyName)
        {
            Assembly assembly = null;

            Assembly[] assemblys = AppDomain.CurrentDomain.GetAssemblies();

            for (int i = 0; i < assemblys.Length; i++)
            {
                if (assemblys[i].FullName.IndexOf(assemblyName) == 0)
                {
                    assembly = assemblys[i];
                    break;
                }
            }

            return assembly;
        }

        public void Create()
        {
            foreach (UIControlDefine item in UIControlDefineList)
            {
                CreateControl(item);
            }
        }
        public virtual List<UIControlDefine> CreateUIControlDefine(BusiDataItem dataItem )
        {
            List<UIControlDefine> list = new List<UIControlDefine>();

            //创建 lable 
            UIControlDefine UIControlDefine1 = new UIControlDefine();
            UIControlDefine1.Text = dataItem.Caption;
            UIControlDefine1.ControlTypeName = "System.Windows.Forms.Label";

            UIControlDefine1.Top = 1;
            UIControlDefine1.Left = 1;

            UIControlDefine1.PropertyName = dataItem.Name;



            //创建  编辑 。
            UIControlDefine UIControlDefine2 = new UIControlDefine();
            UIControlDefine2.Text = dataItem.Caption;
            UIControlDefine2.ControlTypeName = dataItem.ControlTypeName;

            UIControlDefine2.Top = 1;
            UIControlDefine2.Left = 100;

            UIControlDefine2.PropertyName = dataItem.Name;


            list.Add(UIControlDefine1);
            list.Add(UIControlDefine2);

            return list;
        }



        public void CreateControl(UIControlDefine item)
        {
            ControlDefine controlA = ControlService.GetInstance()[item.ControlTypeName];

            //查找 控件 是否已经创建 。

            Assembly assembly = GetAssembly(controlA.ControlType.AssemblyName);
            Control control = assembly.CreateInstance(controlA.ControlType.FullClassName) as Control;


            item.RelateControl = control;

            control.Text = item.Text;

            ParentControl.Controls.Add(control);

            control.Location = new System.Drawing.Point(item.Left, item.Top);
            control.Size = new System.Drawing.Size(item.Width, item.Heigth);
        }



    }
}
