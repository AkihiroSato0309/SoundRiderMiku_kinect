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
	GameObject obstaclePrefab;			// 生成する背景装飾オブジェクトのプレハブ.
	[SerializeField]
	float distance = 50;				// プレイヤーがここに設定された距離を進む度に, 生成が行われる.
	[SerializeField]
	int instantiationPerGeneration = 5;	// 一度の生成でいくつインスタンスを作るか.
	
	// --------------- private ---------------
	const int initialNum = 5;	// Startで何度生成を行うか.
	Transform playerTransform;	// プレイヤーのTransform.
	float nextLine = 0;			// 次の生成発生地点のZ座標
	float posOffsetZ = 0;		// 生成するインスタンスのZ座標に対するオフセット.
	
	
	/************************************************************************************//**
	開始.
		
	@return なし		
	****************************************************************************************/
	void Start()
	{
		// プレイヤー情報の取得.
		var player = GameObject.FindWithTag ("Player");
		//var playerScript = player.GetComponent<PlayerStub> ();
		this.playerTransform = player.transform;

		// イントロ中は障害物が発生しないように, 生成位置オフセットと生成発生地点をずらす.
		this.posOffsetZ = 10 * SoundManager.Inst.IntroLength;
		this.nextLine = this.posOffsetZ;

		// 初期生成.
		for (int i = 0; i < initialNum; i++)
		{
			this.Generate ();
		}
		this.nextLine -= (this.distance * initialNum);	// 初期生成でずれた分を修正
	}
	
	/************************************************************************************//**
	更新.
		
	@return なし		
	****************************************************************************************/
	void Update()
	{
		if (this.playerTransform.position.z > this.nextLine)
		{
			//Debug.Log ("Generate (" + this.nextLine.ToString() +")");
			Generate ();
		}
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
		this.nextLine += this.distance;
		this.posOffsetZ += this.distance;
	}

	/************************************************************************************//**
	障害物のインスタンス化.
		
	@return なし		
	****************************************************************************************/
	void InstantiateRandomly()
	{
		// 座標算出.
		float x = Random.Range (0, WallManager.Inst.RoadWidth) - (WallManager.Inst.RoadWidth / 2);
		float y = playerTransform.position.y;
		float z = Random.Range (0, this.distance) + this.posOffsetZ;
		var pos = new Vector3 (x, y, z);

		// インスタンス化と, インスタンスの初期化.
		var obstacle = Instantiate (this.obstaclePrefab, pos, Quaternion.identity) as GameObject;
		obstacle.transform.parent = this.transform;
	}
}
