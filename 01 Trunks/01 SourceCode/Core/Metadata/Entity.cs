using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Common;
using Core.Architecure;
using Core.Serialize.XML;
using System.Xml.Serialization;

namespace Core.Metadata
{
    [Serializable]
    public class BusiEntity : IGuidStringKey, IXmlSerialize
    {

        #region IGuidStringKey 成员

        public virtual string GuidString { get; set; }

        public virtual bool DeleteFlag { get; set; }

        #endregion

        /// <summary>
        /// 业务标识
        /// </summary>
        public virtual string EntityCode { get; set; }
        /// <summary>
        /// 实体类名称
        /// </summary>
        public virtual string EntityName { get; set; }

        public virtual string EntityCatalog { get; set; }

        /// <summary>
        /// 实体类描述
        /// </summary>
        public virtual string EntityScripe { get; set; }

        /// <summary>
        /// 所属模块
        /// </summary>
        public virtual string ModuleGuidString { get; set; }

        public virtual  ObjectDefine ClassDefine { get; set; }


        /// <summary>
        /// 表名
        /// </summary>
        //public virtual string TableName { get; set; }

        //public virtual string SuperClassName { get; set; }

        public virtual BusiDataItem this[string dataItemName]
        {
            get
            {
                BusiDataItem busiDataItem = null;
                foreach (BusiDataItem item in DataItemList)
                {
                    if (item.Name == dataItemName)
                    {
                        busiDataItem = item;
                        break;
                    }
                }

                return busiDataItem;
            }
        }


        [XmlIgnore]
        public virtual IList<BusiDataItem> DataItemList { get; set; }

        public virtual List<BusiDataItem> DataItemList_XML { get; set; }





 
        public BusiEntity()
        {
            DataItemList = new List<BusiDataItem>();
            DataItemList_XML = new List<BusiDataItem>();
            ClassDefine = new ObjectDefine();

            GuidString = Guid.NewGuid().ToString();
        }


        public virtual void NewDataItem()
        {
            BusiDataItem item = new BusiDataItem();

            DataItemList.Add(item);
        }


        #region IXmlSerialize 成员

        public virtual string GetFileName()
        {
            return EntityName;
        }

        public virtual string GetPath()
        {
            return string.Empty ;
        }

        public virtual void BeforeSerialize()
        {
            DataItemList_XML.Clear();
            DataItemList_XML.AddRange(DataItemList);
        }

        public virtual void AfterDeserialize()
        {
            foreach ( BusiDataItem item in DataItemList_XML)
            {
                DataItemList.Add(item);
            }
        }

        #endregion
    }
}
