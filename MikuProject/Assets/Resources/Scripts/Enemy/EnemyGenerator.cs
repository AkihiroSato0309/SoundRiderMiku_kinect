/***********************************************************************************************//**

@file EnemyGenerator.cs
	
@author Keisuke Ohe
		
***************************************************************************************************/
		
using UnityEngine;
using System.Collections;


/***********************************************************************************************//**

敵生成クラス.
	
***************************************************************************************************/
public class EnemyGenerator : MonoBehaviour
{
	// --------------- inspector ---------------
	[SerializeField]
	GameObject enemyPrefab;				// 生成する背景装飾オブジェクトのプレハブ.
	[SerializeField]
	float distance = 100;				// プレイヤーがここに設定された距離を進む度に, 生成が行われる.
	[SerializeField]
	int instantiationPerGeneration = 1;	// 一度の生成でいくつインスタンスを作るか.
	[SerializeField]
	float generationOffsetY;			// チェンジボックスに設定するY座標.
	[SerializeField]
	float generationOffsetZ;			// チェンジボックスに設定するZ座標.
	
	// --------------- private ---------------
	Transform playerTransform;	// プレイヤーのTransform.
	float nextLine = 0;			// 次の生成発生地点のZ座標.
	
	
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
		
		// イントロ中は障害物が発生しないように, 生成発生地点をずらす.
		this.nextLine = 10 * SoundManager.Inst.IntroLength;
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
	}
	
	/************************************************************************************//**
	敵のインスタンス化.
		
	@return なし		
	****************************************************************************************/
	void InstantiateRandomly()
	{
		// 座標算出.
		float x = Random.Range (0, WallManager.Inst.RoadWidth) - (WallManager.Inst.RoadWidth / 2);
		float y = generationOffsetY;
		float z = this.nextLine + generationOffsetZ;
		var pos = new Vector3 (x, y, z);
		
		// インスタンス化と, インスタンスの初期化.
		var enemy = Instantiate (this.enemyPrefab, pos, Quaternion.identity) as GameObject;
		enemy.transform.parent = this.transform;
	}
}
