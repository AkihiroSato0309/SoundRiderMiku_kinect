/***********************************************************************************************//**

@file BGObjManager.cs
	
@author Keisuke Ohe
		
***************************************************************************************************/
		
using UnityEngine;
using System.Collections;


/***********************************************************************************************//**

背景装飾オブジェクト管理クラス.
背景装飾オブジェクトを生成し, それらが常に適切な位置に表示されるよう, 座標を調整する.
	
***************************************************************************************************/
public class BGObjManager : SingletonMonoBehaviour<BGObjManager>
{
	// --------------- inspector ---------------
	[SerializeField]
	GameObject bgObjPrefab;			// 背景装飾オブジェクトのプレハブ.
	[SerializeField]
	float bgObjDepth = 50;			// 背景装飾オブジェクトの1つ分の奥行き
	[SerializeField]
	float warpStartDistance = 50;	// ワープを開始する基準となる距離（プレイヤーの背後からどれだけ離れたかでワープを判定する）
	[SerializeField]
	int instNum;					// 使い回すインスタンスの数（多いほど遠方まで背景オブジェクトが設置される）

	// --------------- private ---------------
	Transform playerTransform;	// プレイヤーのTransform
	BGObj[] bgObjs;				// 生成した背景装飾オブジェクトのスクリプト
	int backmostIndex = 0;		// 最後方にある背景装飾オブジェクトを指すインデックス


	/************************************************************************************//**
	開始.
	
	@return なし		
	****************************************************************************************/
	void Awake()
	{
		// シングルトンになるように削除を行う.
		if (base.MakeIntoSingleton ()) return;
		
		// プレイヤーのTransformの取得. ワープ判定に使う.
		this.playerTransform = GameObject.FindWithTag ("Player").transform;

		// 生成した背景装飾オブジェクトのスクリプトを格納するためのコレクションを生成
		this.bgObjs = new BGObj[this.instNum];

		// インスタンスを生成
		this.CreateInsts ();
	}

	/************************************************************************************//**
	更新.
		
	@return なし		
	****************************************************************************************/
	void Update()
	{
		this.WarpBGObjIfNecessary ();
	}

	/************************************************************************************//**
	フェイズを次の段階へと進んだ時に呼ばれる. 背景装飾オブジェクトの同名関数を呼ぶ.
		
	@param [in] nextPhase 次のフェイズの番号（段階を示す値）
			
	@return なし		
	****************************************************************************************/
	public void MoveToNextPhase (int nextPhase)
	{
		foreach (var bgObj in this.bgObjs)
		{
			bgObj.MoveToNextPhase (nextPhase);
		}
	}

	/************************************************************************************//**
	背景装飾オブジェクトのインスタンスを生成.
		
	@return なし		
	****************************************************************************************/
	void CreateInsts()
	{
		// インスタンスを生成.
		for (int i = 0; i < this.instNum; i++)
		{
			// 座標算出
			Vector3 pos = this.transform.position;
			pos.z = i * bgObjDepth;

			// 左の壁.
			GameObject inst = Instantiate (this.bgObjPrefab, pos, Quaternion.identity) as GameObject;
			bgObjs[i] = inst.GetComponent<BGObj> ();
			inst.transform.parent = this.transform;
		}
	}

	/************************************************************************************//**
	最後方にある背景装飾オブジェクトのワープ.
		
	@return なし		
	****************************************************************************************/
	void WarpBGObjIfNecessary()
	{
		// 最後方の背景装飾オブジェクト.
		var script = this.bgObjs[this.backmostIndex];
		var inst = script.gameObject;
			
		// もしオブジェクトがプレイヤーの後方に行っていたら, 前方へワープさせる.
		float distance = this.playerTransform.position.z - inst.transform.position.z;
		if (distance > this.warpStartDistance)
		{
			// 座標を更新.
			Vector3 pos = inst.transform.position;
			pos.z = pos.z + (this.bgObjDepth * this.instNum);
			inst.transform.position = pos;
			
			// 最後方のオブジェクトを指すインデックスを更新.
			this.backmostIndex = ++this.backmostIndex % this.instNum;
		}
	}
}
