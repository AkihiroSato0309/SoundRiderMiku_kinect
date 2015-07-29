/***********************************************************************************************//**

@file WallManager.cs
	
@author Keisuke Ohe
		
***************************************************************************************************/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class WallManager : SingletonMonoBehaviour<WallManager>
{
	// --------------- inspector ---------------
	[SerializeField]
	GameObject wallPrefab;
	[SerializeField]
	int num;
	[SerializeField]
	Vector3 offset = new Vector3 (10, 0, 10);
	[SerializeField]
	float warpStartDistanceZ;

	// --------------- property ---------------
	public float RoadWidth { get { return offset.x * 2; } }

	// --------------- private ---------------
	List<GameObject> leftWalls;
	List<GameObject> rightWalls;
	int backmostWallIndex = 0;
	Transform playerTransform;


	/************************************************************************************//**
	開始.
		
	@return なし		
	****************************************************************************************/
	void Awake ()
	{
		// シングルトンになるように削除を行う
		if (base.MakeIntoSingleton ()) return;

		// プレイヤーの取得
		this.playerTransform = GameObject.FindWithTag ("Player").transform;

		// 壁生成準備
		this.leftWalls = new List<GameObject> (this.num);
		this.rightWalls = new List<GameObject> (this.num);

		// 壁を生成
		for (int i = 0; i < this.num; i++)
		{
			GameObject wall = Instantiate (this.wallPrefab);
			wall.transform.position = new Vector3 (-this.offset.x, this.offset.y, offset.z * i);
			wall.transform.parent = this.transform;
			this.leftWalls.Add (wall);

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
		GameObject leftWall = this.leftWalls[this.backmostWallIndex];
		GameObject rightWall = this.rightWalls[this.backmostWallIndex];
		float distance = this.playerTransform.position.z - leftWall.transform.position.z;

		if (distance > this.warpStartDistanceZ)
		{
			float x = leftWall.transform.position.x;
			float y = leftWall.transform.position.y;
			float z = leftWall.transform.position.z + this.offset.z * this.num;
			leftWall.transform.position = new Vector3 (x, y, z);
			rightWall.transform.position = new Vector3 (-x, y, z);
			this.backmostWallIndex = ++this.backmostWallIndex % this.num;
		}
	}
}
