/***********************************************************************************************//**

@file ChangeBoxGenerator.cs
	
@author Keisuke Ohe
		
***************************************************************************************************/
		
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/***********************************************************************************************//**

チェンジボックス生成クラス.
	
***************************************************************************************************/
public class ChangeBoxGenerator : SingletonMonoBehaviour<ChangeBoxGenerator>
{
	// --------------- inspector ---------------
	[SerializeField]
	GameObject changeBoxPrefab;		// 生成するチェンジボックスオブジェクトのプレハブ.
	[SerializeField]
	float[] generationTimes;		// 各フェイズごとに生成する時間を設定する. -1の場合は, 生成しない.
	[SerializeField]
	float generationOffsetY;		// チェンジボックスに設定するY座標.
	[SerializeField]
	float generationOffsetZ;		// チェンジボックスに設定するZ座標.
	
	// --------------- private ---------------
	Transform playerTransform;
	FadeCamera fadeCamera;
	Queue<ChangeBox> boxes;
	float timer = 0;
	int timesIndex = 0;


	/************************************************************************************//**
	開始.
		
	@return なし		
	****************************************************************************************/
	void Awake()
	{
		this.playerTransform = GameObject.FindWithTag ("Player").transform;
		this.fadeCamera = GameObject.Find ("FadeCamera").GetComponent<FadeCamera> ();
		this.boxes = new Queue<ChangeBox> ();
	}
	
	/************************************************************************************//**
	更新.
		
	@return なし		
	****************************************************************************************/
	void Update()
	{
		float targetTime = this.generationTimes [timesIndex];
		if (targetTime < 0) return;

		this.timer += SoundManager.Inst.DeltaTime;

		if (this.timer > targetTime)
		{
			Generate ();
			this.generationTimes [timesIndex] = -1;
		}
	}

	/************************************************************************************//**
	次フェイズ移行. フェイズが移行する時に呼ばれる.
		
	@return なし		
	****************************************************************************************/
	public void MoveToNextPhase ()
	{
		// 既に最後のインデックスであれば, 何もしない.
		if (this.timesIndex == (this.generationTimes.Length - 1)) return;

		// タイマーをリセットし, インデックスを進める.
		++this.timesIndex;
		this.timer = 0;

		// チェンジボックスが1つでも存在するなら.
		if (this.boxes.Count != 0)
		{
			// ボックスを全て破壊する.
			for (int i = 0; i < this.boxes.Count; i++)
			{
				var box = this.boxes.Dequeue ();
				box.Destroy ();
			}
			// 画面全体にBGMパート切り替え時のエフェクトを掛ける.
			fadeCamera.FadeStart ();
		}
	}
	
	/************************************************************************************//**
	チェックボックスの生成.
		
	@return なし		
	****************************************************************************************/
	void Generate ()
	{
		// 座標算出.
		float x = Random.Range (0, WallManager.Inst.RoadWidth) - (WallManager.Inst.RoadWidth / 2);
		float y = generationOffsetY;
		float z = generationOffsetZ + playerTransform.position.z;
		var pos = new Vector3 (x, y, z);
		
		// インスタンス化と, インスタンスの初期化.
		var obj = Instantiate (this.changeBoxPrefab, pos, Quaternion.identity) as GameObject;
		obj.transform.parent = this.transform;
		var script = obj.GetComponent<ChangeBox> ();
		script.Init (this.playerTransform);
		this.boxes.Enqueue (script);
	}

}
