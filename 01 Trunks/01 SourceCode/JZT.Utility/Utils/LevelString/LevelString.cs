using System ;
using System.Collections ;

namespace JZT.Utility.LevelString
{
	/// <author>朱江</author>
	/// <version>0.1</version>
	/// <stereotype>
	/// </stereotype>
	/// <since>2005-7-6</since>
	/// <persistent/>
	public class LevelString
	{
		/// <directed>True</directed>
		/// <link>aggregationByValue</link>
		/// <associates>JZT.Utility.LevelString.LevelStringSection</associates>
		/// <supplierCardinality>0..*</supplierCardinality>
		/// <stereotype>用于保存 层次段 对象</stereotype>
		public ArrayList levelStringSectionLst;


		/// <postconditions>由 Singleton 指定的 构造方法。</postconditions>
		public LevelString(params int[] sections)
		{
			//实体对象对应的数据库表名。
			//TableName = "app_LevelString" ;

			//初始化列表。
			levelStringSectionLst = new ArrayList() ;

			//形成各段。
			AddSections(sections) ;
		}
	
		#region 用于增加各层级段的段信息。

		/// <output>得到一个新的 LevelStringSection 对象。</output>
		/// <postconditions>本方法作为 LevelStringSection 的工厂方法。</postconditions>
		public LevelStringSection NewLevelStringSection()
		{
			//创建一个新的层级串段 。
			return  new LevelStringSection() ;
		}

		/// <input>将指定层级段加入到列表中。</input>
		public void AddLevelStringSection(LevelStringSection levelStringSection)
		{
			levelStringSectionLst.Add(levelStringSection) ;
		}

		/// <input>给定层组段段长，生成层级段列表。</input>
		public void AddSections(params int[] sections)
		{
			if (sections == null)
			{
				return  ;
			}

			for ( int i = 0 ; i < sections.Length ; i++ ) 
			{
				//创建节段对象。
				LevelStringSection levelStringSection = NewLevelStringSection() ;
				levelStringSection.Length             = sections[i] ;
				//加入到列表中。
				AddLevelStringSection(levelStringSection) ;
			}
		}

		#endregion 

		/// <associates>
		/// </associates>
		/// <directed>False</directed>
		/// <input>给定自身的层级串。</input>
		/// <output>得到其父串。</output>
		public string GetParentString(string levelString)
		{
			//截取前后的空格。
			levelString = levelString.Trim() ;
			//首先得到本层组字符串的段度（多少段）。
			int sectionCount = GetSectionCount(levelString) ;
			//截去最后一段，本级的层组字符串，得到本级的父串。
			//得到最后一串的长度。
			int lastSectionLen = ( levelStringSectionLst[sectionCount - 1] as LevelStringSection ).Length ;
			//得到父串的长度。
			int lastSectionBeginPos = levelString.Length - lastSectionLen ;
			//返回父串。
			return levelString.Substring(0 , lastSectionBeginPos) ;
		}



		/// <input>给定父结点的层级串和最大子结点的层级串</input>
		/// <output>得到新的层级串。</output>
		/// //本方法有隐含的缺陷，当超过本级最大时它没有作出超出范围的判断，需要修正。
		public string GetNewChildLevelStringByChildLevel( string LastChildLevel)
		{


			//取得父串字符串。
			string ParentLevelString = this.GetParentString(LastChildLevel) ;


			//首先得到本层组字符串的段度（多少段）。
			int sectionCount = GetSectionCount(LastChildLevel) ;

			//得到最后一串的串值 。
			string lastSection = GetSectionString(LastChildLevel,sectionCount) ;
			//转换为整型。
			int currentValue = Convert.ToInt32(lastSection);
			//将最后一位加 1 .
			currentValue++ ;
			//得到本串的段长。
			int sectionLen = LastChildLevel.Trim().Length - ParentLevelString.Trim().Length ;
			//形成本级的层级串段 。
			string lastSectionStr = string.Format(("{0:D" + sectionLen.ToString() + "}"),currentValue ) ;

			return  ParentLevelString + lastSectionStr ;

		}

		/// <summary>
		/// 取回使用本层级串的对象，在对应数据库表中的最大子串。
		/// </summary>
		/// <returns>返回取得的子串，如没有或出现错误则返回空串。</returns>
		public string GetCurrentMaxChildLevelString( string parentLevelString)
		{
			//ILevelString levelString = new LevelStringSQLServer() ;
            return null;//levelString.GetCurrentMaxChildLevelString(this , parentLevelString);
		}


        /// <summary>
        /// 取新的子对象层级
        /// </summary>
        /// <param name="parentLevelString">子对象层级串。</param>
        /// <returns></returns>
		public string GetNewChildLevelString(string parentLevelString) 
		{
			string CurrentMaxChildLevelString = GetCurrentMaxChildLevelString(parentLevelString) ;
			return GetNewChildLevelStringByChildLevel(CurrentMaxChildLevelString) ;
		}


		/// <input>给定层级串及需要得到的层级段。</input>
		/// <output>得到指定的层级段字符串。</output>
		public string GetSectionString(string levelString , int section)
		{
			int beginPos = GetSectionBegin(section) ;
			int len = ( levelStringSectionLst[section - 1] as LevelStringSection ).Length ;
			return levelString.Substring(beginPos , len ) ;
		}


		#region 层级串级数和字符串长度相关的方法。

		/// <summary>
		/// 给定一个字符串，得到它的层级数，如果发生错误返回 -1 。
		/// </summary>
		/// <param name="LevelString">给定字符串</param>
		/// <returns></returns>
		public int GetSectionCount(string LevelString)
		{
			int len = LevelString.Length ;

			int retLevelCount = -1 ;
			for ( int i = 0 ; i < levelStringSectionLst.Count ; i++ )
			{
				len = len - ( levelStringSectionLst[i] as LevelStringSection ).Length ;
				if ( len == 0 )
				{
					retLevelCount = i ;
					break ;
				}
			}

			//层级的数量 要加1 ，因为i 的起点是0 
			return ++retLevelCount ;
		}

		/// <summary>
		/// 给定一个段长度（第几级），得到指定段的长度。
		/// </summary>
		/// <param name="sectionCount">段的长度</param>
		/// <returns>返回该级段的字符长度，如果出现问题则返回 0 .</returns>
		public int GetLevelStringLength(int sectionCount)
		{
			if ((sectionCount > 0) && (sectionCount <= this.levelStringSectionLst.Count ))
			{
				int len = 0 ;
				for ( int i = 1 ; i <= sectionCount ; i++)
				{
					len += (levelStringSectionLst[i - 1] as LevelStringSection).Length ;
				}
				return len ;
			}
			else
			{
				return -1 ;
			}
			
		}


		/// <summary>
		/// 取得段开始长度。
		/// 如果出现错误，返回 -1 .
		/// </summary>
		/// <param name="section"></param>
		/// <returns></returns>
		public int GetSectionBegin(int section)
		{
			//如果给定的长度超过字符段的长度，则给出错误代码并退出。
			if (( section > levelStringSectionLst.Count ) || (section < 0 ))
			{
				return -1 ;
			}

			int beginLen = 0 ;
			for ( int i = 1 ; i < section ; i++ )
			{
				beginLen += ( levelStringSectionLst[ i - 1 ] as LevelStringSection ).Length ;
			}

			return beginLen ;
		}

		/// <summary>
		/// 取得段结束长度。
		/// 如果出现错误 ，返回 -1 .
		/// </summary>
		/// <param name="section"></param>
		/// <returns></returns>
		public int GetSectionEnd(int section)
		{
			//如果给定的长度超过字符段的长度，则给出错误代码并退出。
			if (( section > levelStringSectionLst.Count - 1 ) || (section < 0 ))
			{
				return -1 ;
			}
			
			return GetSectionBegin(section) +( levelStringSectionLst[section] as LevelStringSection ).Length ;
		}
		#endregion


		#region 属性定义区。

		private string _levelTableName ;
		/// <summary>
		/// 层级串对应保存的表名。
		/// </summary>
		public string LevelTableName
		{
			get
			{
				return _levelTableName ;
			}
			set
			{
				_levelTableName = value ;
			}
		}

		private string _levelFieldName ;
		/// <summary>
		/// 本层级串对应的保存的字段名。
		/// </summary>
		public string LevelFieldName
		{
			get
			{
				return _levelFieldName ;
			}
			set
			{
				_levelFieldName = value ;
			}
		}

		private string _levelStringSections ;
		/// <summary>
		/// 层级串的段数量和每段段长的字符数组。
		/// 例：“2，3，4”，表示一个三段的段长，第一段长为2 第二段长为3 第三段长为 4.
		/// </summary>
		public string LevelStringSections
		{
			get
			{
				return _levelStringSections;
			}
			set
			{
				_levelStringSections = value ;
			}
		}


		private string _lvelStringName ;
		/// <summary>
		/// 层级串的名字。
		/// </summary>
		public string LevelStringName
		{
			get
			{
				return _lvelStringName;
			}
			set
			{
				_lvelStringName = value ;
			}
		}

		private string _description ;
		/// <summary>
		/// 层级串简述。
		/// </summary>
		public string Description
		{
			get
			{
				return _description ;
			}
			set
			{
				_description = value ;
			}
		}

		#endregion
	
	}

}
