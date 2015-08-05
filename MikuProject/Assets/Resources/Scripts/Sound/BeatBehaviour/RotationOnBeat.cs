/***********************************************************************************************//**

@file RotationOnBeat.cs
	
@author Keisuke Ohe
		
***************************************************************************************************/
		
using UnityEngine;
using System.Collections;


/***********************************************************************************************//**

ビートに応じて回転させるスクリプト.
	
***************************************************************************************************/
public class RotationOnBeat : BaseBeatBehaviour
{
	// --------------- inspector ---------------
	[SerializeField]
	Vector3 rotation;
	[SerializeField]
	int freq_subBeat = 2;
	[SerializeField]
	float time_subBeat = 1.0f;

	// --------------- private ---------------
	int subBeatCount = 0;
	bool canRotate = true;
	
	
	/************************************************************************************//**
	通知受け取り. ビートに合わせて回転する.
		
	@return なし		
	****************************************************************************************/
	public override void Notice()
	{
		if ((++this.subBeatCount % this.freq_subBeat) == 1)
		{
			if (this.canRotate) this.StartCoroutine (this.Rotate ());
		}
	}
	
	/************************************************************************************//**
	瞬時に最大まで拡大し, 徐々に元の大きさまで縮小.
		
	@return なし		
	****************************************************************************************/
	private IEnumerator Rotate()
	{
		this.canRotate = false;

		// 新しい回転値を定義.
		var from = this.transform.eulerAngles;
		var to = from + rotation;

		// 回転に掛ける時間.
		float time = this.time_subBeat * SoundManager.Inst.SubBeatFreq;
		
		// 時間を掛けて回転させる.
		for (float elapsed = 0; elapsed < time; elapsed += Time.deltaTime)
		{
			float t = elapsed / time;
			float c = t * t * t * t;
			this.transform.eulerAngles = Vector3.Lerp (from, to, c);

			yield return null;
		}
		yield return null;
		this.transform.eulerAngles = to;

		this.canRotate = true;
	}
}