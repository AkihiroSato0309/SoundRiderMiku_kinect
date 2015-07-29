/***********************************************************************************************//**

@file WallManager.cs
	
@author Keisuke Ohe
		
***************************************************************************************************/
		
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/***********************************************************************************************//**

背景装飾オブジェクト管理クラス. 壁を生成し, 常にプレイヤーの左右に壁が来るように座標を管理する.
	
***************************************************************************************************/
public class BGObjGenerator : MonoBehaviour
{
	// --------------- inspector ---------------
	[SerializeField]
	GameObject[] objPrefabs;	// 生成する背景装飾オブジェクトのプレハブ.
	[SerializeField]
	float[] phaseAdvanceTimes;	// フェイズをいつインクリメントするか.
	[SerializeField]
	float generationPerSec;
	[SerializeField]
	Vector3 minTrans;
	[SerializeField]
	Vector3 maxTrans;
	[SerializeField]
	Vector3 minRot;
	[SerializeField]
	Vector3 maxRot;

	// --------------- private ---------------
	int phase = 0;				// 現在のフェイズ（ステージの進行段階）.
	Timer timer;				// タイマー
	Transform playerTransform;	// プレイヤーのTransform


	/************************************************************************************//**
	開始.
	
	@return なし		
	****************************************************************************************/
	void Start()
	{
		// generationPerSecのゼロチェック
		if (this.generationPerSec == 0)
		{
			this.generationPerSec = -1;
			Debug.LogError ("RandomGenerator.generationPerSec must not be 0.0f.");
		}

		// プレイヤーのTransformの取得.
		this.playerTransform = GameObject.FindWithTag ("Player").transform;

		// タイマーの初期化
		this.timer = new Timer (1 / this.generationPerSec, this.InstantiateRandomly);
	}

	/************************************************************************************//**
	更新.
		
	@return なし		
	****************************************************************************************/
	void Update()
	{
		this.AdvancePhaseIfNecessary ();
		this.timer.Update ();
	}

	void AdvancePhaseIfNecessary()
	{
		if (this.phase == this.phaseAdvanceTimes.Length) return;

		if (this.phaseAdvanceTimes [this.phase] > SoundManager.Inst.Time)
		{
			++this.phase;
		}
	}

	void InstantiateRandomly()
	{
		for (int i = 0; i < (this.phase + 1); i++)
		{
			var newPos = this.playerTransform.position + this.RondomVec (this.minTrans, this.maxTrans);
			var newRot = Quaternion.Euler (this.RondomVec (this.minRot, this.maxRot));

			var newObj = Instantiate (objPrefabs[i], newPos, newRot) as GameObject;
			newObj.AddComponent<AutoDestroyBehindPlayer> ();
			newObj.transform.parent = this.transform;
		}
	}

	Vector3 RondomVec (Vector3 min, Vector3 max)
	{
		float x = Random.Range (min.x, max.x);
		float y = Random.Range (min.y, max.y);
		float z = Random.Range (min.z, max.z);
		return new Vector3 (x, y, z);
	}
}
