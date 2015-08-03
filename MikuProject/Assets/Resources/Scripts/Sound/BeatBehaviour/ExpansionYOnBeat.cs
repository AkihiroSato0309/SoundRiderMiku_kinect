/***********************************************************************************************//**

@file BeatExpansionBox.cs
	
@author Keisuke Ohe
		
***************************************************************************************************/
		
using UnityEngine;
using System.Collections;


/***********************************************************************************************//**

ビートに応じて拡縮させるスクリプト.

***************************************************************************************************/
public class ExpansionYOnBeat : BaseBeatBehaviour
{
	[SerializeField]
	float maxScaleY = 2.0f;
	[SerializeField]
	float minScaleY = 1.0f;
	[SerializeField]
	int freq_subBeat = 2;
	[SerializeField]
	float time_subBeat = 1.0f;

	int subBeatCount = 0;
	

	/************************************************************************************//**
	通知受け取り. ビートに合わせて拡大する.
	
	@return なし		
	****************************************************************************************/
	public override void Notice()
	{
		if ((++this.subBeatCount % this.freq_subBeat) == 1)
		{
			this.StartCoroutine (this.Expand ());
		}
	}

	/************************************************************************************//**
	瞬時に最大まで拡大し, 徐々に元の大きさまで縮小.
		
	@return なし		
	****************************************************************************************/
	private IEnumerator Expand()
	{
		// 新しいスケール値を定義
		float x = this.transform.localScale.x;
		float y = this.maxScaleY;
		float z = this.transform.localScale.z;

		// 最大の大きさに設定
		this.transform.localScale = new Vector3 (x, y, z);

		// スケールに掛ける時間
		float time = this.time_subBeat * SoundManager.Inst.SubBeatFreq;

		// スケールを徐々に小さくする
		for (float elapsed = 0; elapsed < time; elapsed += Time.deltaTime)
		{
			float t = elapsed / time;
			float c = t * t;
			y = Mathf.Lerp (this.maxScaleY, this.minScaleY, c);
			this.transform.localScale = new Vector3 (x, y, z);

			yield return null;
		}
	}
}