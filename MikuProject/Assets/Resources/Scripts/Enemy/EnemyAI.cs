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

	public GameObject explosionParticle;

	private Vector3 basePos;
	private Vector3 prevPos;
	private Action updateFunc;
	private float wateDistance;

	private Animator controller;

	// Use this for initialization
	void Start () 
	{
		controller = GetComponent<Animator> ();
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
		transform.Translate (0.0f, 0.0f, -speed, Space.World);
		
		Vector3 currentPos = transform.position;
		basePos = Camera.main.transform.position;
		float worldWatePosZ = basePos.z + waitSpace;
		if (currentPos.z < worldWatePosZ) 
		{
			currentPos.z = worldWatePosZ;
			transform.position = currentPos;
			wateDistance = waitSpace;
			updateFunc = WaitUpdate;
		}
	}

	void WaitUpdate()
	{
		// プレイヤーと並走する
		FollowPlayer ();

		// 時間を減らす
		waitTime -= Time.deltaTime;
		if (waitTime < 0.0f)
			updateFunc = RemoveUpdate;
	}

	void RemoveUpdate()
	{
		// 移動
		transform.Translate (0.0f, 0.0f, -speed, Space.World);

		controller = GetComponent<Animator> ();
	}

	// 並走処理
	void FollowPlayer()
	{
		// プレイヤーと並走する
		Vector3 basePos = Camera.main.transform.position;
		Vector3 currentPos = transform.position;
		currentPos.z = basePos.z + wateDistance;
		transform.position = currentPos;
	}

	void OnTriggerEnter(Collider col)
	{
		print ("unti");
		// 弾に当たった時の処理
		if (col.gameObject.tag == "Bullet") 
		{
			// 音を鳴らす
			SoundManager.Inst.PlaySE (SE.HitEnemy);

			// 停止させる
			updateFunc = FollowPlayer;

			// 並走距離（カメラとの距離）を設定
			wateDistance = transform.position.z - Camera.main.transform.position.z;

			controller.SetTrigger("isDeath");
		}
		
	}
	
	public void Death()
	{
		SoundManager.Inst.PlaySE (SE.DestroyEnemy);
		GameObject.Instantiate (explosionParticle, transform.position, Quaternion.identity);
		Destroy (gameObject);
	}
}
