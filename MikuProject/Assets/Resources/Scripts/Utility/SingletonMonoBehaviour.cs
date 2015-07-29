/***********************************************************************************************//**

@file SingletonMonoBehaviour.cs
	
@author Keisuke Ohe
		
***************************************************************************************************/

using UnityEngine;


/***********************************************************************************************//**

MonoBehaviour版シングルトン.

@attention	DontDestroyOnLoad()の呼び出し, あるいはオブジェクト削除時のinstの初期化を, サブクラス側で実装する必要がある.
			また, このクラスに実装されている Check関数 も, 必要であればサブクラス側で呼び出す.

***************************************************************************************************/
public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
	private static T inst;
	public static T Inst
	{
		get
		{
			if (inst == null)
			{
				inst = (T)FindObjectOfType(typeof(T));
				
				if (inst == null)
				{
					Debug.LogError (typeof(T) + " script is nothing");
				}
			}	
			return inst;
		}
	}

	/************************************************************************************//**
	インスタンスが複数存在した場合に削除する. 使用する場合は, サブクラスのAwakeで呼ぶことがオススメ.
	
	@return 既にインスタンスが存在しており, 削除が発生した場合はtrue.
	****************************************************************************************/
	protected bool MakeIntoSingleton ()
	{
		if(this != Inst)
		{
			Destroy (this);
			Debug.LogWarning (typeof(T) + " script must be single, so another instance has been deleted.");
			return true;
		}
		return false;
	}
}