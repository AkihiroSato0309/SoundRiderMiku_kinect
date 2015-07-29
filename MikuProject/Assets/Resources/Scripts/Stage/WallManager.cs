/***********************************************************************************************//**

@file WallManager.cs
	
@author Keisuke Ohe
		
***************************************************************************************************/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/***********************************************************************************************//**

壁管理クラス. 壁を生成し, 常にプレイヤーの左右に壁が来るように座標を管理する.
	
***************************************************************************************************/
public class WallManager : SingletonMonoBehaviour<WallManager>
{
	// --------------- inspector ---------------
	[SerializeField]
	GameObject wallPrefab;						// 生成したい壁のプレハブ
	[SerializeField]
	int num;									// 使い回す壁の数（多いほど遠方まで壁が設置される）
	[SerializeField]
	Vector3 offset = new Vector3 (10, 0, 10);	// 初期配置・ワープ時のオフセット. xは道幅の1/2, yは0, zは壁のz方向の長さ（奥行き）を入れるといい.
	[SerializeField]
	float warpStartDistance;					// ワープを開始する基準となる距離（プレイヤーの背後からどれだけ離れたかでワープを判定する）

	// --------------- property ---------------
	public float RoadWidth { get { return offset.x * 2; } }		// 形成される道幅 = 左壁と右壁間におけるX方向距離

	// --------------- private ---------------
	List<GameObject> leftWalls;		// 左の壁
	List<GameObject> rightWalls;	// 右の壁
	int backmostWallIndex = 0;		// 最後方にある壁を指すインデックス
	Transform playerTransform;		// プレイヤーのTransform


	/************************************************************************************//**
	開始.
		
	@return なし		
	****************************************************************************************/
	void Awake ()
	{
		// シングルトンになるように削除を行う.
		if (base.MakeIntoSingleton ()) return;

		// プレイヤーのTransformの取得. ワープ判定に使う.
		this.playerTransform = GameObject.FindWithTag ("Player").transform;

		// 壁生成準備.
		this.leftWalls = new List<GameObject> (this.num);
		this.rightWalls = new List<GameObject> (this.num);

		// 壁を生成.
		for (int i = 0; i < this.num; i++)
		{
			// 左の壁.
			GameObject wall = Instantiate (this.wallPrefab);
			wall.transform.position = new Vector3 (-this.offset.x, this.offset.y, offset.z * i);
			wall.transform.parent = this.transform;
			this.leftWalls.Add (wall);

			// 右の壁.
			wall = Instantiate (this.wallPrefab);
			wall.transform.position = new Vector3 (this.offset.x, this.offset.y, offset.z * i);
			wall.transform.parent = this.transform;
			this.rightWalls.Add (wall);
		}
	}

	/************************************************************************************//**
	更新.
		
	@return なし		
	****************************************************************************************/
	void Update ()
	{
		// 最後方の左右の壁を取得.
		GameObject leftWall = this.leftWalls[this.backmostWallIndex];
		GameObject rightWall = this.rightWalls[this.backmostWallIndex];

		// もし壁がプレイヤーの後方に行っていたら, 前方へワープさせる.
		float distance = this.playerTransform.position.z - leftWall.transform.position.z;
		if (distance > this.warpStartDistance)
		{
			// 左右の壁の座標を更新.
			float x = this.offset.x;
			float y = this.offset.y;
			float z = leftWall.transform.position.z + this.offset.z * this.num;
			leftWall.transform.position = new Vector3 (x, y, z);
			rightWall.transform.position = new Vector3 (-x, y, z);

			// 最後方の壁を指すインデックスを更新.
			this.backmostWallIndex = ++this.backmostWallIndex % this.num;
		}
	}
}
