/***********************************************************************************************//**

@file ColorFluctuationOnBeat.cs
	
@author Keisuke Ohe
		
***************************************************************************************************/

using UnityEngine;
using System.Collections;


/***********************************************************************************************//**

ビートに応じて Material.color の値を formからto, toからfrom という形で交互に変化させるスクリプト.

***************************************************************************************************/
public class ColorFluctuationOnBeat : BaseBeatBehaviour
{
	[SerializeField]
	int freq_subBeat = 2;				// サブビート何回ごとに色の変化を開始するか
	[SerializeField]
	float forwardTime_subBeat = 1.0f;	// formからtoへ色が変化する時間は, サブビート何回分の時間に相当するか
	[SerializeField]
	float backTime_subBeat = 1.0f;		// toからfromへ色が変化する時間は, サブビート何回分の時間に相当するか
	[SerializeField]
	Color from;							// Material.color に設定する値1
	[SerializeField]
	Color to;							// Material.color に設定する値2

	delegate float Interpolation(float t);
	Material material;
	int subBeatCount = 0;	// サブビートの発生回数を数える
	bool isUp = false;		// trueでfrom->to, falseでto->from


	/************************************************************************************//**
	開始.
		
	@return なし		
	****************************************************************************************/
	protected override void Start()
	{
		base.Start ();

		this.material = this.GetComponent<Renderer> ().material;
		this.material.color = from;
	}
	
	/************************************************************************************//**
	通知受け取り. ビートに合わせて色を変更する.
		
	@return なし		
	****************************************************************************************/
	public override void Notice()
	{
		if ((++this.subBeatCount % this.freq_subBeat) == 1)
		{
			this.StartCoroutine (this.Fluctuate ());
		}
	}
	
	/************************************************************************************//**
	material.color を current から target の値に変更していく.
		
	@return なし		
	****************************************************************************************/
	private IEnumerator Fluctuate()
	{
		// 色を from -> to で徐々に変化
		float time = this.forwardTime_subBeat * SoundManager.Inst.SubBeatFreq;
		for (float elapsed = 0; elapsed < time; elapsed += Time.deltaTime)
		{
			float t = elapsed / time;
			t = Cube (t);
			this.material.color = Color.Lerp (this.from, this.to, t);
			yield return null;
		}

		// 色を to -> from で徐々に変化
		time = this.backTime_subBeat * SoundManager.Inst.SubBeatFreq;
		for (float elapsed = 0; elapsed < time; elapsed += Time.deltaTime)
		{
			float t = elapsed / time;
			t = CubeRevert (t);
			this.material.color = Color.Lerp (this.from, this.to, t);
			yield return null;
		}
	}
	
	private float Cube(float t)
	{
		return t * t;
	}

	private float CubeRevert(float t)
	{
		return 1.0f - (t * t);
	}

	/*
	private IEnumerator Fluctuate()
	{
		// フラグを反転し, 色の変化にかける時間を算出
		float time;
		Interpolation interpolation;
		
		if (this.isUp = !this.isUp)
		{
			time = this.forwardTime_subBeat * SoundManager.Inst.SubBeatFreq;
			interpolation = this.Cube;
		}
		else
		{
			time = this.backTime_subBeat * SoundManager.Inst.SubBeatFreq;
			interpolation = this.CubeRevert;
		}
		
		// 色を徐々に変更する
		for (float elapsed = 0; elapsed < time; elapsed += Time.deltaTime)
		{
			float t = elapsed / time;
			t = interpolation (t);
			this.material.color = Color.Lerp (this.from, this.to, t);
			yield return null;
		}
	}
	*/
}