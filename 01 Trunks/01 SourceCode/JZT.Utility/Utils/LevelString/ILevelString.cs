using System.Data ;

namespace JZT.Utility.LevelString
{
	/// <summary>
	/// 
	/// 
	/// 
	/// ��Ȩ����: ��Ȩ����(C) 2005������ͨ����
	/// ����ժҪ: �����㼶���Ĺ��ܽӿڡ�
	/// ������ڣ�2005��7��28��
	/// ��    ����V1.0 
	/// ��    �ߣ��콭
	///		
	/// �޸ļ�¼1: 
	/// �޸����ڣ�2004��3��10��
	/// �� �� �ţ�V1.2
	/// �� �� �ˣ�С��
	/// �޸����ݣ��Է������������޸ģ���������BUG������
	/// �޸ļ�¼2: 
	/// �޸����ڣ�2004��3��20��
	/// �� �� �ţ�V1.3
	/// �� �� �ˣ�С��
	/// �޸����ݣ��Է����������н�һ���Ľ����������ϡ�����
	/// </summary>
	public interface ILevelString
	{
		/// <summary>
		/// ����һ���µĲ㼶����
		/// </summary>
		/// <param name="levelString">��һ���༭�õĲ㼶������</param>
		/// <returns>��ȷִ�з��� 0 ���쳣ʱ��������ֵ ��</returns>
		int NewLevelString(LevelString levelString);

		/// <summary>
		/// ����һ��ָ���Ĳ㼶����
		/// </summary>
		/// <param name="levelString">��һ����Ҫ�ύ�޸ĵĲ㼶������</param>
		/// <returns>��ȷִ�з��� 0 ���쳣ʱ��������ֵ ��</returns>
		int UpdateLevleString(LevelString levelString);

		/// <summary>
		/// ����һ�����������õ�һ���㼶������
		/// </summary>
		/// <param name="levelStringName">��һ���㼶������</param>
		/// <param name="levelString">��Ҫ���صĲ㼶������</param>
		/// <returns>��ȷִ�з��� 0 ���쳣ʱ��������ֵ ��</returns>
		int GetLevelStringByName(string levelStringName , out LevelString levelString );

		/// <summary>
		/// ����һ������ֵ���õ�һ���㼶������
		/// </summary>
		/// <param name="levelStringName">��һ���㼶������</param>
		/// <param name="levelString">��Ҫ���صĲ㼶������</param>
		/// <returns>��ȷִ�з��� 0 ���쳣ʱ��������ֵ ��</returns>
		int GetLevelStringByPrimaryKey(long primaryKey , out LevelString levelString );



		/// <summary>
		/// ����һ���㴮��������֣�ɾ������㼶������
		/// </summary>
		/// <param name="levelStringName">�㼶������</param>
		/// <returns>��ȷִ�з��� 0 ���쳣ʱ��������ֵ ��</returns>
		int DeleteLevelStringByName(string levelStringName);

		/// <summary>
		/// ����һ���㴮���������ֵ��ɾ������㼶������
		/// </summary>
		/// <param name="levelStringName">�㼶������</param>
		/// <returns>��ȷִ�з��� 0 ���쳣ʱ��������ֵ ��</returns>
		int DeleteLevelStringByPrimaryKey(long primaryKey);



		/// <summary>
		/// ����ϵͳ�еĲ㼶������������ܴ����Կ���ȡ�����еĲ㼶����
		/// </summary>
		/// <param name="dataSet">�õ�һ���������в㼶����������ݼ���</param>
		/// <returns>��ȷִ�з��� 0 ���쳣ʱ��������ֵ ��</returns>
		int GetAllLevelString(out DataSet dataSet);

		/// <summary>
		/// ȡ��ʹ�ñ��㼶���Ķ����ڶ�Ӧ���ݿ���е�����Ӵ���
		/// </summary>
		/// <returns>����ȡ�õĵ�ǰ����Ӵ�����û�л���ִ����򷵻ؿմ���</returns>
		string GetCurrentMaxChildLevelString(LevelString levelString , string parentLevelString);


	}
}
