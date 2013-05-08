using System ;
using System.Collections ;

namespace JZT.Utility.LevelString
{
	/// <author>�콭</author>
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
		/// <stereotype>���ڱ��� ��ζ� ����</stereotype>
		public ArrayList levelStringSectionLst;


		/// <postconditions>�� Singleton ָ���� ���췽����</postconditions>
		public LevelString(params int[] sections)
		{
			//ʵ������Ӧ�����ݿ������
			//TableName = "app_LevelString" ;

			//��ʼ���б�
			levelStringSectionLst = new ArrayList() ;

			//�γɸ��Ρ�
			AddSections(sections) ;
		}
	
		#region �������Ӹ��㼶�εĶ���Ϣ��

		/// <output>�õ�һ���µ� LevelStringSection ����</output>
		/// <postconditions>��������Ϊ LevelStringSection �Ĺ���������</postconditions>
		public LevelStringSection NewLevelStringSection()
		{
			//����һ���µĲ㼶���� ��
			return  new LevelStringSection() ;
		}

		/// <input>��ָ���㼶�μ��뵽�б��С�</input>
		public void AddLevelStringSection(LevelStringSection levelStringSection)
		{
			levelStringSectionLst.Add(levelStringSection) ;
		}

		/// <input>��������ζγ������ɲ㼶���б�</input>
		public void AddSections(params int[] sections)
		{
			if (sections == null)
			{
				return  ;
			}

			for ( int i = 0 ; i < sections.Length ; i++ ) 
			{
				//�����ڶζ���
				LevelStringSection levelStringSection = NewLevelStringSection() ;
				levelStringSection.Length             = sections[i] ;
				//���뵽�б��С�
				AddLevelStringSection(levelStringSection) ;
			}
		}

		#endregion 

		/// <associates>
		/// </associates>
		/// <directed>False</directed>
		/// <input>��������Ĳ㼶����</input>
		/// <output>�õ��丸����</output>
		public string GetParentString(string levelString)
		{
			//��ȡǰ��Ŀո�
			levelString = levelString.Trim() ;
			//���ȵõ��������ַ����Ķζȣ����ٶΣ���
			int sectionCount = GetSectionCount(levelString) ;
			//��ȥ���һ�Σ������Ĳ����ַ������õ������ĸ�����
			//�õ����һ���ĳ��ȡ�
			int lastSectionLen = ( levelStringSectionLst[sectionCount - 1] as LevelStringSection ).Length ;
			//�õ������ĳ��ȡ�
			int lastSectionBeginPos = levelString.Length - lastSectionLen ;
			//���ظ�����
			return levelString.Substring(0 , lastSectionBeginPos) ;
		}



		/// <input>���������Ĳ㼶��������ӽ��Ĳ㼶��</input>
		/// <output>�õ��µĲ㼶����</output>
		/// //��������������ȱ�ݣ��������������ʱ��û������������Χ���жϣ���Ҫ������
		public string GetNewChildLevelStringByChildLevel( string LastChildLevel)
		{


			//ȡ�ø����ַ�����
			string ParentLevelString = this.GetParentString(LastChildLevel) ;


			//���ȵõ��������ַ����Ķζȣ����ٶΣ���
			int sectionCount = GetSectionCount(LastChildLevel) ;

			//�õ����һ���Ĵ�ֵ ��
			string lastSection = GetSectionString(LastChildLevel,sectionCount) ;
			//ת��Ϊ���͡�
			int currentValue = Convert.ToInt32(lastSection);
			//�����һλ�� 1 .
			currentValue++ ;
			//�õ������Ķγ���
			int sectionLen = LastChildLevel.Trim().Length - ParentLevelString.Trim().Length ;
			//�γɱ����Ĳ㼶���� ��
			string lastSectionStr = string.Format(("{0:D" + sectionLen.ToString() + "}"),currentValue ) ;

			return  ParentLevelString + lastSectionStr ;

		}

		/// <summary>
		/// ȡ��ʹ�ñ��㼶���Ķ����ڶ�Ӧ���ݿ���е�����Ӵ���
		/// </summary>
		/// <returns>����ȡ�õ��Ӵ�����û�л���ִ����򷵻ؿմ���</returns>
		public string GetCurrentMaxChildLevelString( string parentLevelString)
		{
			//ILevelString levelString = new LevelStringSQLServer() ;
            return null;//levelString.GetCurrentMaxChildLevelString(this , parentLevelString);
		}


        /// <summary>
        /// ȡ�µ��Ӷ���㼶
        /// </summary>
        /// <param name="parentLevelString">�Ӷ���㼶����</param>
        /// <returns></returns>
		public string GetNewChildLevelString(string parentLevelString) 
		{
			string CurrentMaxChildLevelString = GetCurrentMaxChildLevelString(parentLevelString) ;
			return GetNewChildLevelStringByChildLevel(CurrentMaxChildLevelString) ;
		}


		/// <input>�����㼶������Ҫ�õ��Ĳ㼶�Ρ�</input>
		/// <output>�õ�ָ���Ĳ㼶���ַ�����</output>
		public string GetSectionString(string levelString , int section)
		{
			int beginPos = GetSectionBegin(section) ;
			int len = ( levelStringSectionLst[section - 1] as LevelStringSection ).Length ;
			return levelString.Substring(beginPos , len ) ;
		}


		#region �㼶���������ַ���������صķ�����

		/// <summary>
		/// ����һ���ַ������õ����Ĳ㼶��������������󷵻� -1 ��
		/// </summary>
		/// <param name="LevelString">�����ַ���</param>
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

			//�㼶������ Ҫ��1 ����Ϊi �������0 
			return ++retLevelCount ;
		}

		/// <summary>
		/// ����һ���γ��ȣ��ڼ��������õ�ָ���εĳ��ȡ�
		/// </summary>
		/// <param name="sectionCount">�εĳ���</param>
		/// <returns>���ظü��ε��ַ����ȣ�������������򷵻� 0 .</returns>
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
		/// ȡ�öο�ʼ���ȡ�
		/// ������ִ��󣬷��� -1 .
		/// </summary>
		/// <param name="section"></param>
		/// <returns></returns>
		public int GetSectionBegin(int section)
		{
			//��������ĳ��ȳ����ַ��εĳ��ȣ������������벢�˳���
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
		/// ȡ�öν������ȡ�
		/// ������ִ��� ������ -1 .
		/// </summary>
		/// <param name="section"></param>
		/// <returns></returns>
		public int GetSectionEnd(int section)
		{
			//��������ĳ��ȳ����ַ��εĳ��ȣ������������벢�˳���
			if (( section > levelStringSectionLst.Count - 1 ) || (section < 0 ))
			{
				return -1 ;
			}
			
			return GetSectionBegin(section) +( levelStringSectionLst[section] as LevelStringSection ).Length ;
		}
		#endregion


		#region ���Զ�������

		private string _levelTableName ;
		/// <summary>
		/// �㼶����Ӧ����ı�����
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
		/// ���㼶����Ӧ�ı�����ֶ�����
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
		/// �㼶���Ķ�������ÿ�ζγ����ַ����顣
		/// ������2��3��4������ʾһ�����εĶγ�����һ�γ�Ϊ2 �ڶ��γ�Ϊ3 �����γ�Ϊ 4.
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
		/// �㼶�������֡�
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
		/// �㼶��������
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
