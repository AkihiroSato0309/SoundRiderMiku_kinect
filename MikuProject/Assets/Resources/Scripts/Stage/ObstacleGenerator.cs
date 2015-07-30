/***********************************************************************************************//**

@file ObstacleGenerator.cs
	
@author Keisuke Ohe
		
***************************************************************************************************/
		
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/***********************************************************************************************//**

障害物生成クラス.
	
***************************************************************************************************/
public class ObstacleGenerator : MonoBehaviour
{
	// --------------- inspector ---------------
	[SerializeField]
	GameObject obstaclePrefab;		// 生成する背景装飾オブジェクトのプレハブ.
	[SerializeField]
	float generationPerSec;			// 一秒に何回生成するか
	[SerializeField]
	int instantiationPerGeneration;	// 一度の生成でいくつインスタンスを作るか
	[SerializeField]
	int initialGenerationNum;		// Startで何度生成を行うか
	[SerializeField]
	float generationDepth;			// 生成時の奥行きの範囲
	
	// --------------- private ---------------
	Transform playerTransform;	// プレイヤーのTransform
	Timer timer;				// タイマー
	int generationCount = 0;	// 生成した回数
	
	
	/************************************************************************************//**
	開始.
		
	@return なし		
	****************************************************************************************/
	void Start()
	{
		// generationPerSecはゼロであってはいけない（ゼロ除算が発生するため）
		if (this.generationPerSec == 0)
		{
			this.generationPerSec = -1;
			Debug.LogError ("RandomGenerator.generationPerSec must not be 0.0f.");
		}
		
		// プレイヤーのTransformの取得.
		this.playerTransform = GameObject.FindWithTag ("Player").transform;
		
		// タイマーの初期化
		this.timer = new Timer (1 / this.generationPerSec, this.Generate);

		// 初期生成
		for (int i = 0; i < this.initialGenerationNum; i++)
		{
			this.Generate ();
		}
	}
	
	/************************************************************************************//**
	更新.
		
	@return なし		
	****************************************************************************************/
	void Update()
	{
		this.timer.Update ();
	}

	/************************************************************************************//**
	障害物の生成.
		
	@return なし		
	****************************************************************************************/
	void Generate ()
	{
		for (int i = 0; i < this.instantiationPerGeneration; i++)
		{
			this.InstantiateRandomly ();
		}

		++this.generationCount;
	}

	/************************************************************************************//**
	障害物のインスタンス化.
		
	@return なし		
	****************************************************************************************/
	void InstantiateRandomly()
	{
		// 座標算出
		float offsetZ = this.generationCount * this.generationDepth;
		float x = Random.Range (0, WallManager.Inst.RoadWidth) - (WallManager.Inst.RoadWidth / 2);
		float y = playerTransform.position.y;
		float z = Random.Range (0, this.generationDepth) + offsetZ;
		var pos = new Vector3 (x, y, z);

		// インスタンス化と, インスタンスの初期化
		var obstacle = Instantiate (obstaclePrefab, pos, Quaternion.identity) as GameObject;
		obstacle.AddComponent<AutoDestroyBehindPlayer> ();
		obstacle.transform.parent = this.transform;
	}
}
