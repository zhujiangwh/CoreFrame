using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Core.Common
{
    public class LevelStringSection
    {

        /// <postconditions>本层级串的长度。</postconditions>
        public int Length { get; set; }
    }

    [Serializable]
    public class LevelString //: JZT.Common.LevelString_Info
    {
        
		#region Member Variables
		
		protected string _levelStringName = string.Empty;
        protected string _levelStringSections = string.Empty;
        protected string _levelTableName = string.Empty;
        protected string _levelFieldName = string.Empty;

		#endregion



        public LevelString() 
        {
            //初始化列表。
            levelStringSectionLst = new ArrayList();
        }


		#region Public Properties


		public virtual string LevelStringName
		{
			get { return _levelStringName; }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for LevelStringName", value, value.ToString());
				_levelStringName = value;
			}
		}

		public virtual string LevelStringSections
		{
			get { return _levelStringSections; }
			set
			{
				if ( value != null && value.Length > 20)
					throw new ArgumentOutOfRangeException("Invalid value for LevelStringSections", value, value.ToString());
				_levelStringSections = value;
			}
		}

		public virtual string LevelTableName
		{
			get { return _levelTableName; }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for LevelTableName", value, value.ToString());
				_levelTableName = value;
			}
		}

		public virtual string LevelFieldName
		{
			get { return _levelFieldName; }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for LevelFieldName", value, value.ToString());
				_levelFieldName = value;
			}
		}

		#endregion
		


        ///// <directed>True</directed>
        ///// <link>aggregationByValue</link>
        ///// <associates>Jointown.Utility.LevelString.LevelStringSection</associates>
        ///// <supplierCardinality>0..*</supplierCardinality>
        ///// <stereotype>用于保存 层次段 对象</stereotype>
        private ArrayList levelStringSectionLst;


        //public virtual void AddSections(params int[] sections)
        //{
        //    //实体对象对应的数据库表名。
        //    //TableName = "app_LevelString" ;


        //    //形成各段。
        //    if (sections == null)
        //    {
        //        return;
        //    }

        //    for (int i = 0; i < sections.Length; i++)
        //    {
        //        //创建节段对象。
        //        LevelStringSection levelStringSection = NewLevelStringSection();
        //        levelStringSection.Length = sections[i];
        //        //加入到列表中。
        //        AddLevelStringSection(levelStringSection);
        //    }


        //}



        ///// <postconditions>由 Singleton 指定的 构造方法。</postconditions>
        //public virtual LevelString(params int[] sections)
        //{
        //    //实体对象对应的数据库表名。
        //    //TableName = "app_LevelString" ;

        //    //初始化列表。
        //    levelStringSectionLst = new ArrayList() ;

        //    //形成各段。
        //    AddSections(sections) ;
        //}

        #region 用于增加各层级段的段信息。

        /// <output>得到一个新的 LevelStringSection 对象。</output>
        /// <postconditions>本方法作为 LevelStringSection 的工厂方法。</postconditions>
        public virtual LevelStringSection NewLevelStringSection()
        {
            //创建一个新的层级串段 。
            return new LevelStringSection();
        }

        /// <input>将指定层级段加入到列表中。</input>
        public virtual void AddLevelStringSection(LevelStringSection levelStringSection)
        {
            levelStringSectionLst.Add(levelStringSection);
        }

        /// <input>给定层组段段长，生成层级段列表。</input>
        public virtual void AddSections(params int[] sections)
        {
            if (sections == null)
            {
                return;
            }

            for (int i = 0; i < sections.Length; i++)
            {
                //创建节段对象。
                LevelStringSection levelStringSection = NewLevelStringSection();
                levelStringSection.Length = sections[i];
                //加入到列表中。
                AddLevelStringSection(levelStringSection);
            }
        }

        #endregion

        /// <associates>
        /// </associates>
        /// <directed>False</directed>
        /// <input>给定自身的层级串。</input>
        /// <output>得到其父串。</output>
        public virtual string GetParentString(string levelString)
        {
            //截取前后的空格。
            levelString = levelString.Trim();
            //首先得到本层组字符串的段度（多少段）。
            int sectionCount = GetSectionCount(levelString);
            //截去最后一段，本级的层组字符串，得到本级的父串。
            //得到最后一串的长度。
            int lastSectionLen = (levelStringSectionLst[sectionCount - 1] as LevelStringSection).Length;
            //得到父串的长度。
            int lastSectionBeginPos = levelString.Length - lastSectionLen;
            //返回父串。
            return levelString.Substring(0, lastSectionBeginPos);
        }



        /// <input>给定父结点的层级串和最大子结点的层级串</input>
        /// <output>得到新的层级串。</output>
        /// //本方法有隐含的缺陷，当超过本级最大时它没有作出超出范围的判断，需要修正。
        public virtual string GetNewChildLevelStringByChildLevel(string ParentLevelString, string LastChildLevel)
        {
            int currentValue;
            //取得父串字符串。

            if (LastChildLevel.CompareTo(string.Empty) == 0)
            {
                currentValue = 1;
            }
            else
            {
                //首先得到本层组字符串的段度（多少段）。
                int sectionCount = GetSectionCount(LastChildLevel);

                //得到最后一串的串值 。
                string lastSection = GetSectionString(LastChildLevel, sectionCount);
                //转换为整型。
                currentValue = Convert.ToInt32(lastSection);
                //将最后一位加 1 .
                currentValue++;
            }
            //得到本串的段长。-- 这一处代码也需要调整，应该直接使用 定义的结构。
            int parentSectionCount = GetSectionCount(ParentLevelString);

            int sectionLen = GetLevelStringLength(parentSectionCount + 1) - GetLevelStringLength(parentSectionCount);
            //形成本级的层级串段 。
            string lastSectionStr = string.Format(("{0:D" + sectionLen.ToString() + "}"), currentValue);

            return ParentLevelString + lastSectionStr;
        }

        /// <summary>
        /// 取回使用本层级串的对象，在对应数据库表中的最大子串。
        /// </summary>
        /// <returns>返回取得的子串，如没有或出现错误则返回空串。</returns>
        public virtual string GetCurrentMaxChildLevelString(string parentLevelString)
        {
            //ILevelString levelString = new LevelStringSQLServer() ;
            return null;//levelString.GetCurrentMaxChildLevelString(this , parentLevelString);
        }

        public virtual string GetNewChildLevelString(string parentLevelString)
        {
            string CurrentMaxChildLevelString = GetCurrentMaxChildLevelString(parentLevelString);
            return GetNewChildLevelStringByChildLevel(parentLevelString, CurrentMaxChildLevelString);
        }


        /// <input>给定层级串及需要得到的层级段。</input>
        /// <output>得到指定的层级段字符串。</output>
        public virtual  string GetSectionString(string levelString, int section)
        {
            int beginPos = GetSectionBegin(section);
            int len = (levelStringSectionLst[section - 1] as LevelStringSection).Length;
            return levelString.Substring(beginPos, len);
        }


        #region 层级串级数和字符串长度相关的方法。

        /// <summary>
        /// 给定一个字符串，得到它的层级数，如果发生错误返回 -1 。
        /// </summary>
        /// <param name="LevelString">给定字符串</param>
        /// <returns></returns>
        public virtual int GetSectionCount(string LevelString)
        {
            int len = LevelString.Length;

            int retLevelCount = -1;
            for (int i = 0; i < levelStringSectionLst.Count; i++)
            {
                len = len - (levelStringSectionLst[i] as LevelStringSection).Length;
                if (len == 0)
                {
                    retLevelCount = i;
                    break;
                }
            }

            //层级的数量 要加1 ，因为i 的起点是0 
            return ++retLevelCount;
        }

        /// <summary>
        /// 给定一个段长度（第几级），得到指定段的长度。
        /// </summary>
        /// <param name="sectionCount">段的长度</param>
        /// <returns>返回该级段的字符长度，如果出现问题则返回 0 .</returns>
        public virtual int GetLevelStringLength(int sectionCount)
        {
                int len = 0;

            if ((sectionCount > 0) && (sectionCount <= this.levelStringSectionLst.Count))
            {
                for (int i = 1; i <= sectionCount; i++)
                {
                    len += (levelStringSectionLst[i - 1] as LevelStringSection).Length;
                }
            }
            //else
            //{
            //    return -1;
            //}

            return len;

        }


        /// <summary>
        /// 取得段开始长度。
        /// 如果出现错误，返回 -1 .
        /// </summary>
        /// <param name="section"></param>
        /// <returns></returns>
        public virtual int GetSectionBegin(int section)
        {
            //如果给定的长度超过字符段的长度，则给出错误代码并退出。
            if ((section > levelStringSectionLst.Count) || (section < 0))
            {
                return -1;
            }

            int beginLen = 0;
            for (int i = 1; i < section; i++)
            {
                beginLen += (levelStringSectionLst[i - 1] as LevelStringSection).Length;
            }

            return beginLen;
        }

        /// <summary>
        /// 取得段结束长度。
        /// 如果出现错误 ，返回 -1 .
        /// </summary>
        /// <param name="section"></param>
        /// <returns></returns>
        public virtual int GetSectionEnd(int section)
        {
            //如果给定的长度超过字符段的长度，则给出错误代码并退出。
            if ((section > levelStringSectionLst.Count - 1) || (section < 0))
            {
                return -1;
            }

            return GetSectionBegin(section) + (levelStringSectionLst[section] as LevelStringSection).Length;
        }
        #endregion




    }
}
