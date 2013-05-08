using System.Data ;

namespace JZT.Utility.LevelString
{
	/// <summary>
	/// 
	/// 
	/// 
	/// 版权所有: 版权所有(C) 2005，九州通集团
	/// 内容摘要: 操作层级串的功能接口。
	/// 完成日期：2005年7月28日
	/// 版    本：V1.0 
	/// 作    者：朱江
	///		
	/// 修改记录1: 
	/// 修改日期：2004年3月10日
	/// 版 本 号：V1.2
	/// 修 改 人：小张
	/// 修改内容：对方法……进行修改，修正故障BUG……。
	/// 修改记录2: 
	/// 修改日期：2004年3月20日
	/// 版 本 号：V1.3
	/// 修 改 人：小张
	/// 修改内容：对方法……进行进一步改进，修正故障……。
	/// </summary>
	public interface ILevelString
	{
		/// <summary>
		/// 创建一个新的层级串。
		/// </summary>
		/// <param name="levelString">传一个编辑好的层级串对象。</param>
		/// <returns>正确执行返回 0 ，异常时返回其他值 。</returns>
		int NewLevelString(LevelString levelString);

		/// <summary>
		/// 更新一个指定的层级串。
		/// </summary>
		/// <param name="levelString">传一个需要提交修改的层级串对象</param>
		/// <returns>正确执行返回 0 ，异常时返回其他值 。</returns>
		int UpdateLevleString(LevelString levelString);

		/// <summary>
		/// 给定一个对象名，得到一个层级串对象。
		/// </summary>
		/// <param name="levelStringName">传一个层级串名。</param>
		/// <param name="levelString">需要传回的层级串对象。</param>
		/// <returns>正确执行返回 0 ，异常时返回其他值 。</returns>
		int GetLevelStringByName(string levelStringName , out LevelString levelString );

		/// <summary>
		/// 给定一个主键值，得到一个层级串对象。
		/// </summary>
		/// <param name="levelStringName">传一个层级串名。</param>
		/// <param name="levelString">需要传回的层级串对象。</param>
		/// <returns>正确执行返回 0 ，异常时返回其他值 。</returns>
		int GetLevelStringByPrimaryKey(long primaryKey , out LevelString levelString );



		/// <summary>
		/// 给定一个层串对象的名字，删除这个层级串对象。
		/// </summary>
		/// <param name="levelStringName">层级串对象</param>
		/// <returns>正确执行返回 0 ，异常时返回其他值 。</returns>
		int DeleteLevelStringByName(string levelStringName);

		/// <summary>
		/// 给定一个层串对象的主键值，删除这个层级串对象。
		/// </summary>
		/// <param name="levelStringName">层级串对象</param>
		/// <returns>正确执行返回 0 ，异常时返回其他值 。</returns>
		int DeleteLevelStringByPrimaryKey(long primaryKey);



		/// <summary>
		/// 由于系统中的层级串的数量不会很大，所以可以取回所有的层级串。
		/// </summary>
		/// <param name="dataSet">得到一个包含所有层级串对象的数据集。</param>
		/// <returns>正确执行返回 0 ，异常时返回其他值 。</returns>
		int GetAllLevelString(out DataSet dataSet);

		/// <summary>
		/// 取回使用本层级串的对象，在对应数据库表中的最大子串。
		/// </summary>
		/// <returns>返回取得的当前最大子串，如没有或出现错误则返回空串。</returns>
		string GetCurrentMaxChildLevelString(LevelString levelString , string parentLevelString);


	}
}
