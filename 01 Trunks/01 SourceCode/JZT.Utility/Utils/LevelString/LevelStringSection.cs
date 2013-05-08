namespace JZT.Utility.LevelString
{
/// <author>朱江</author>
/// <version>0.1</version>
/// <since>2005.7.6</since>
public class LevelStringSection
	{

	int _length  ;
	/// <postconditions>本层级串的长度。</postconditions>
	public int Length
	{
		get
		{
			return _length ;
		}
		set
		{
			_length = value ;
		}
	}
}

}
