using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Data;
using System.Xml.Serialization;
using Core.Common;

namespace Core.Metadata
{

    [Serializable]
    public class DataItemDisplayer
    {
        [XmlAttribute]
        public virtual string ColumnName { get; set; }
        [XmlAttribute]
        public virtual string DisplayLable { get; set; }
        [XmlAttribute]
        public virtual string ColumnType { get; set; }


        [XmlAttribute]
        public virtual bool IsReadOnly { get; set; }
        [XmlAttribute]
        public virtual bool IsVisible { get; set; }
        [XmlAttribute]
        public virtual int VisibleIndex { get; set; }

        [XmlAttribute]
        public virtual bool IsUnique { get; set; }
        [XmlAttribute]
        public virtual bool IsDeleted { get; set; }
        [XmlAttribute]
        public virtual string DisplayFormat { get; set; }

        [XmlAttribute]
        public virtual bool BestFitWidth { get; set; }
        [XmlAttribute]
        public virtual int MinDisplayWidth { get; set; }
        [XmlAttribute]
        public virtual int MaxDisplayWidth { get; set; }
        [XmlAttribute]
        public virtual int DisplayWidth { get; set; }
        [XmlAttribute]
        public virtual int Length { get; set; }


        [XmlAttribute]
        public virtual string FontName { get; set; }
        [XmlAttribute]
        public virtual int FontSize { get; set; }
        [XmlIgnore]
        public virtual Color FontColor { get; set; }
        [XmlAttribute]
        public virtual bool FontBold { get; set; }
        [XmlAttribute]
        public virtual bool FontItalic { get; set; }
        [XmlAttribute]
        public virtual bool FontOutline { get; set; }
        [XmlAttribute]
        public virtual bool FontShadow { get; set; }
        [XmlAttribute]
        public virtual bool FontStrikethrough { get; set; }
        [XmlIgnore]
        public virtual Color BackGroundColor { get; set; }


        public DataItemDisplayer()
        {
            //删除状态为 false;
            IsDeleted = false;
        }

        public DataItemDisplayer(DataColumn dataColumn)
        {
            //删除状态为 false;
            IsDeleted = false;
            IsVisible = true;


            ColumnName = dataColumn.ColumnName;
            DisplayLable = dataColumn.Caption;

            Length = dataColumn.MaxLength;

            VisibleIndex = dataColumn.Ordinal;
            IsReadOnly = dataColumn.ReadOnly;

            IsUnique = dataColumn.Unique;
            DisplayWidth = 100;
        }


    }


    [Serializable]
    public class EntityDisplayer : BaseObject
    {



        //public override void SaveToXMLField()
        //{
        //    base.SaveToXMLField();

        //    if (CloumnDisplayerList.Count > 0)
        //    {
        //        if (CloumnDisplayerListXML == null)
        //        {
        //            CloumnDisplayerListXML = new List<ColumnDisplayer>();
        //        }

        //        CloumnDisplayerListXML.Clear();
        //        foreach (ColumnDisplayer item in CloumnDisplayerList)
        //        {
        //            CloumnDisplayerListXML.Add(item);
        //        }
        //    }

        //}

        //public override void LoadFromXMLField()
        //{
        //    base.LoadFromXMLField();

        //    if (CloumnDisplayerListXML.Count > 0)
        //    {
        //        if (CloumnDisplayerList == null)
        //        {
        //            CloumnDisplayerList = new List<ColumnDisplayer>();
        //        }

        //        CloumnDisplayerList.Clear();
        //        foreach (ColumnDisplayer item in CloumnDisplayerListXML)
        //        {
        //            //这一步很关键，否则nhibernate 保存到数据库中的时候 父子的关系 无法创建 。
        //            item.TableDisplayer = this;
        //            CloumnDisplayerList.Add(item);
        //        }

        //        CloumnDisplayerListXML.Clear();
        //    }

        //}

        //public override string GetPKSQLByCode()
        //{
        //    return string.Format(" select pk,version from tb_sys_TableDisplayer where GuidString = '{0}' ", GuidString);
        //}


        #region 属性

        public virtual string TableName { get; set; }

        public virtual bool Editable { get; set; }
        public virtual bool ColumnAutoWidth { get; set; }
        public virtual bool ShowGroupPanel { get; set; }
        public virtual bool AllowCellMerge { get; set; }
        public virtual string GroupPanelText { get; set; }
        public virtual bool AllowSort { get; set; }
        public virtual bool AllowFilter { get; set; }
        public virtual bool AllowGroup { get; set; }
        public virtual bool AllowColumnMoving { get; set; }
        public virtual bool EnableAppearanceFocusedCell { get; set; }
        public virtual bool MultiSelect { get; set; }
        public virtual bool RowSelect { get; set; }


        [XmlIgnore]
        public virtual IList<DataItemDisplayer> CloumnDisplayerList { get; set; }

        public virtual List<DataItemDisplayer> CloumnDisplayerListXML { get; set; }

        #endregion


        public EntityDisplayer()
        {
            CloumnDisplayerList = new List<DataItemDisplayer>();
            CloumnDisplayerListXML = new List<DataItemDisplayer>();
        }

        public EntityDisplayer(DataTable dataTable)
        {
            CloumnDisplayerList = new List<DataItemDisplayer>();
            CloumnDisplayerListXML = new List<DataItemDisplayer>();

            //设置参数
            TableName = dataTable.TableName;

            //初始化数据列对象。
            foreach (DataColumn item in dataTable.Columns)
            {
                DataItemDisplayer columnDisplayer = new DataItemDisplayer(item);
                //columnDisplayer.TableDisplayer = this;

                CloumnDisplayerList.Add(columnDisplayer);
            }
        }


        public virtual string GetColumnNameList()
        {
            StringBuilder columnNameList = new StringBuilder();

            foreach (DataItemDisplayer item in CloumnDisplayerList)
            {
                columnNameList.Append(string.Format("'{0}' ", item.ColumnName));

                if (CloumnDisplayerList.IndexOf(item) < CloumnDisplayerList.Count - 1)
                {
                    columnNameList.Append(" , ");
                }
            }

            return columnNameList.ToString();
        }

        public virtual void SetColumnCaption(DataTable table)
        {
            foreach (DataItemDisplayer item in CloumnDisplayerList)
            {
                item.DisplayLable = GetColumnCaption(item.ColumnName, table);
            }
        }

        private string GetColumnCaption(string colunmName, DataTable table)
        {
            string caption = string.Empty;

            foreach (DataRow item in table.Rows)
            {
                if (item["columnName"].ToString().ToUpper() == colunmName.ToUpper())
                {
                    caption = item["caption"].ToString();
                    break;
                }
            }

            return caption;
        }

        /// <summary>
        /// Table 结构是否和显示相匹配。
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public virtual bool IsMatch(DataTable table)
        {
            //数量是否一致。
            bool flag = table.Columns.Count == CloumnDisplayerList.Count;

            if (flag)
            {
                //比较列。
                foreach (DataColumn col in table.Columns)
                {
                    bool findFlag = GetColumnDisplayer(col.ColumnName) != null;

                    //如果有一个没找到，就退出循环。
                    if (!findFlag)
                    {
                        flag = false;
                        break;
                    }
                }
            }

            return flag;
        }

        /// <summary>
        /// 根据列名取得列控制对象。
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual DataItemDisplayer GetColumnDisplayer(string name)
        {
            DataItemDisplayer column = null;

            foreach (DataItemDisplayer item in CloumnDisplayerList)
            {
                //比较列名。
                if (name.ToUpper() == item.ColumnName.ToUpper())
                {
                    column = item;
                    continue;
                }
            }

            return column;
        }

        /// <summary>
        /// 用新的结构刷新。
        /// </summary>
        /// <param name="table"></param>
        public virtual void Merge(DataTable table)
        {
            //首先全部设为 被删除状态。
            foreach (DataItemDisplayer item in CloumnDisplayerList)
            {
                item.IsDeleted = true;
            }

            //foreach (DataColumn item in table.Columns)
            //{
            //    //如果不存在，则新创建一列，差且加到列表。
            //    DataItemDisplayer col = GetColumnDisplayer(item.ColumnName);
            //    col.IsDeleted = false; //设删除状态为 false;

            //    if (col == null)
            //    {
            //        //初始创建时，删除状态为 false;
            //        DataItemDisplayer columnDisplayer = new ColumnDisplayer(item);
            //        columnDisplayer.TableDisplayer = this;
            //        CloumnDisplayerList.Add(columnDisplayer);
            //    }
            //}
        }


        public virtual void ResetCloumnDisplayerListVisibleIndex()
        {
            int i = 0;
            int j = 0;
            foreach (DataItemDisplayer item in CloumnDisplayerList)
            {
                //item.LineID = i++;

                if (item.IsVisible)
                {
                    item.VisibleIndex = j++;
                }
                else
                {
                    item.VisibleIndex = -1;
                }
            }
        }


        #region IXmlSerialize 成员

        public override void BeforeSerialize()
        {
            if (CloumnDisplayerList.Count > 0)
            {
                if (CloumnDisplayerListXML == null)
                {
                    CloumnDisplayerListXML = new List<DataItemDisplayer>();
                }

                CloumnDisplayerListXML.Clear();
                foreach (DataItemDisplayer item in CloumnDisplayerList)
                {
                    CloumnDisplayerListXML.Add(item);
                }
            }
        }

        public override void AfterDeserialize()
        {
            base.AfterDeserialize();
            if (CloumnDisplayerListXML.Count > 0)
            {
                if (CloumnDisplayerList == null)
                {
                    CloumnDisplayerList = new List<DataItemDisplayer>();
                }

                CloumnDisplayerList.Clear();
                foreach (DataItemDisplayer item in CloumnDisplayerListXML)
                {
                    //这一步很关键，否则nhibernate 保存到数据库中的时候 父子的关系 无法创建 。
                    //item.TableDisplayer = this;
                    CloumnDisplayerList.Add(item);
                }

                CloumnDisplayerListXML.Clear();
            }
        }

        #endregion



    }



}
