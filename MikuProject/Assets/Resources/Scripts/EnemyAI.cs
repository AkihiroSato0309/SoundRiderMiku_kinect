using UnityEngine;
using System.Collections;
using System;

public class EnemyAI : MonoBehaviour {

	[SerializeField]
	private float waitTime = 3.0f;
	[SerializeField]
	private float waitSpace = 10.0f;
	[SerializeField]
	private float speed = 0.3f;

	private Vector3 basePos;
	private Vector3 prevPos;
	private Action updateFunc;

	// Use this for initialization
	void Start () 
	{
		updateFunc = InterUpdate;
	}
	
	// Update is called once per frame
	void Update () 
	{
		updateFunc ();
	}

	void InterUpdate()
	{
		// 移動
		transform.Translate (0.0f, 0.0f, -speed);
		
		Vector3 currentPos = transform.position;
		basePos = Camera.main.transform.position;
		float worldWatePosZ = basePos.z + waitSpace;
		if (currentPos.z < worldWatePosZ) 
		{
			currentPos.z = worldWatePosZ;
			transform.position = currentPos;
			updateFunc = WaitUpdate;
		}
	}

	void WaitUpdate()
	{
		Vector3 basePos = Camera.main.transform.position;
		//Vector3 basePos = GameObject.FindWithTag("Player").transform.position;
		Vector3 currentPos = transform.position;
		currentPos.z = basePos.z + waitSpace;
		// プレイヤーと並走する
		transform.position = currentPos;

		// 時間を減らす
		waitTime -= Time.deltaTime;
		if (waitTime < 0.0f)
			updateFunc = RemoveUpdate;
	}

	void RemoveUpdate()
	{
		// 移動
		transform.Translate (0.0f, 0.0f, -speed);
	}
}
