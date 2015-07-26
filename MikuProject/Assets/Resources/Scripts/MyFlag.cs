using System.Collections;

// 切り替わった瞬間のみtrueを返すフラグ関数
public struct MyFlag{

	private bool inFlag;

	public bool FlagChack(bool f)
	{
		bool result = false;
		if (f == true && inFlag != f) 
		{
			result = true;
			inFlag = true;
		} 
		else if (f == false) 
		{
			inFlag = false;
		}

		return result;
	}

}
