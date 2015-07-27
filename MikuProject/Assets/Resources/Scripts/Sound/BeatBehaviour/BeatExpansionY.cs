/***********************************************************************************************//**

@file BeatExpansionBox.cs
	
@author Keisuke Ohe
		
***************************************************************************************************/
		
using UnityEngine;
using System.Collections;


/***********************************************************************************************//**

ビートに応じて拡縮させるスクリプト.
		
***************************************************************************************************/
public class BeatExpansionY : BaseBeatBehaviour
{
	[SerializeField]
	private float maxScaleY = 2.0f;
	[SerializeField]
	private float minScaleY = 1.0f;
	[SerializeField]
	private float subBeatPerScaling = 1.0f;
	[SerializeField]
	private int subBeatPerExpansion = 2;

	public float MaxScaleY { set { this.maxScaleY = value; } }
	public float MinScaleY { set { this.minScaleY = value; } }
	public float SubBeatPerScaling { set { this.scalingTime = this.soundManager.SubBeatFreq * value; } }

	private Vector3 initialScale;
	private float scalingTime;
	private int subBeatCount = 0;


	/************************************************************************************//**
	開始.
		
	@return なし		
	****************************************************************************************/
	protected override void Start()
	{
		base.Start ();

		this.scalingTime = this.soundManager.SubBeatFreq * this.subBeatPerScaling;
		this.initialScale = this.transform.localScale;
	}
	
	/************************************************************************************//**
	通知受け取り. ビートに合わせて拡大する.
	
	@return なし		
	****************************************************************************************/
	public override void Notice()
	{
		if (this.subBeatCount == 0)
		{
			this.StartCoroutine (this.Expand ());
		}

		++this.subBeatCount;
		this.subBeatCount = subBeatCount % subBeatPerExpansion;
	}

	/************************************************************************************//**
	最大まで拡大し, 徐々に縮小.
		
	@return なし		
	****************************************************************************************/
	private IEnumerator Expand()
	{
		var maxScale = new Vector3 (this.initialScale.x, this.initialScale.y * this.maxScaleY, this.initialScale.z);
		var minScale = new Vector3 (this.initialScale.x, this.initialScale.y * this.minScaleY, this.initialScale.z);;

		// スケールを最大に設定
		this.transform.localScale = maxScale;
		yield return null;

		// スケールを徐々に小さくする
		for (float elapsed = 0; elapsed < this.scalingTime; elapsed += Time.deltaTime)
		{
			this.transform.localScale = Vector3.Slerp (maxScale, minScale, elapsed / this.scalingTime);
			yield return null;
		}
	}
}