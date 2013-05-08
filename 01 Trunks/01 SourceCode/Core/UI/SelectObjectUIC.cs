using System;
using System.Data;
using System.Xml.Serialization;
using Core.Architecure;
using Core.Metadata;
using Core.Serialize.DB;

namespace Core.UI
{
    public interface ISelectObject
    {
        /// <summary>
        /// 创建 参数 控件。
        /// </summary>
        /// <param name="sqlScript"></param>
        void CreateParamsUI(SqlScript sqlScript);

        /// <summary>
        /// 显示表格。
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        DataRow Display(DataTable table);

        /// <summary>
        /// 显示参数控件。
        /// </summary>
        /// <param name="sqlScript"></param>
        void DisplayParamList(SqlScript sqlScript);

        void ConfigForm(SelectObjectUIC selectObjectUIC);

    }

    [Serializable]
    public class SelectObjectUIC : UIinteractive
    {
        [XmlIgnore]
        public virtual ISelectObject SelectObject { get; set; }

        public virtual ObjectDefine SelectObjectDefine { get; set; }

        public virtual SqlScript SqlScript { get; set; }


        [XmlIgnore]
        public virtual DataTable DataSource { get; set; }

        public virtual string Name { get; set; }

        //行为
        public virtual bool MultiSelect { get; set; }

        public virtual bool IsSingleRowAutoReturn { get; set; }

        //界面 
        public virtual string Title { get; set; }

        public virtual int FormHeight { get; set; }

        public virtual int FormWidth { get; set; }

        public virtual bool IsDisplayToolbar { get; set; }

        public virtual EntityDisplayer DisplayerDefine { get; set; }



        public SelectObjectUIC()
        {
            SqlScript = new SqlScript(" ");

            SelectObjectDefine = new ObjectDefine("", "");
        }

        public virtual void Initial()
        {
            CreateSelectObjectUI();
        }

        public virtual void CreateSelectObjectUI()
        {
            SelectObject = SelectObjectDefine.CreateObject() as ISelectObject;

            //创建 界面 上的参数编辑控件。
            SelectObject.CreateParamsUI(SqlScript);

        }

        public virtual DataTable GetDataTable()
        {
            RemotingServer remotingServer = WinApplication.GetInstance().RemotingServer;
            ISelectObjectService selectObjectService = remotingServer.CreateRemotingInterface<ISelectObjectService>();

            return selectObjectService.GetDataTable(Name);
        }



        public virtual DataRow GetSelectedRow()
        {
            DataRow row = null;

            //创建窗口。
            CreateSelectObjectUI();

            SelectObject.ConfigForm(this);



            //显示数据。
            if (SelectObject != null)
            {
                //显示并初始化参数。
                SelectObject.DisplayParamList(SqlScript);


                //获取数据集。
                DataSource = GetDataTable();

                //构造控件显示。
                if (DisplayerDefine == null)
                {
                    DisplayerDefine = new EntityDisplayer(DataSource);
                }


                DisplayerDefine.CloumnDisplayerListXML.AddRange(DisplayerDefine.CloumnDisplayerList);



                //显示数据
                row = SelectObject.Display(DataSource);
            }


            //界面操作。


            //返回选择的数据。



            return row;
        }

        #region xml 序列化。

        public override string GetFileName()
        {
            return Name;
        }

        public override void AfterDeserialize()
        {
            base.AfterDeserialize();

            DisplayerDefine.AfterDeserialize();
        }

        public override void BeforeSerialize()
        {
            base.BeforeSerialize();

            DisplayerDefine.BeforeSerialize();
        }

        #endregion
    }
}
