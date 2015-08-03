/***********************************************************************************************//**

@file AutoDestroyBehindPlayer.cs
	
@author Keisuke Ohe
		
***************************************************************************************************/

using UnityEngine;
using System.Collections;


/***********************************************************************************************//**

プレイヤーの背後に行った場合に, オブジェクトの透明度を上げるスクリプト.
	
***************************************************************************************************/
public class MakeTransparentBehindPlayer : MonoBehaviour
{
	// --------------- inspector ---------------
	[SerializeField]
	float distance = 1.0f;	// 透明度アップを行う基準となる距離.
	[SerializeField]
	float time = 1.0f;		// 透明度の目標値に至るまでの時間
	[SerializeField, Range(0, 255)]
	int targetAlpha = 64;	// 透明度の目標値
	
	// --------------- private ---------------
	Transform playerTransform;	// プレイヤーのTransform.
	Material material;			// マテリアル
	
	
	/************************************************************************************//**
	開始.
		
	@return なし		
	****************************************************************************************/
	void Start ()
	{
		// プレイヤーのTransformの取得
		this.playerTransform = GameObject.FindWithTag ("Player").transform;

		// マテリアルの取得
		this.material = this.GetComponent<Renderer>().material;
	}
	
	/************************************************************************************//**
	更新.
		
	@return なし		
	****************************************************************************************/
	void Update ()
	{
		// プレイヤーと壁とのZ軸上の距離を計算
		float distance = this.playerTransform.position.z - this.transform.position.z;
		
		// プレイヤーの背後に行っており, その距離が基準値を超えていた場合は, 削除する
		if (distance > this.distance)
		{
			this.StartCoroutine (this.MakeTransparent ());
		}
	}

	/************************************************************************************//**
	徐々に透明度を上昇させる.
	
	@return コルーチン特有の戻り値
	****************************************************************************************/
	IEnumerator MakeTransparent()
	{
		float from = this.material.color.a;	// 最初のアルファ値
		float to = 1.0f / this.targetAlpha;	// 目標とするアルファ値

		for (float elapsed = 0; elapsed < this.time; elapsed += Time.deltaTime)
		{
			float t = elapsed / this.time;
			float a = Mathf.Lerp (from, to, t);
			Color color = this.material.color;
			color.a = a;
			this.material.color = color;

			yield return null;
		}
	}
}
