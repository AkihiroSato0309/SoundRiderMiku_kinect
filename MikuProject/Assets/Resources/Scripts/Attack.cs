using UnityEngine;
using System.Collections;

public class Attack : MonoBehaviour {

	[SerializeField]
	private float actionDistance = 0.4f;
	[SerializeField]
	private float actionHeight = 1.0f;
	[SerializeField]
	private float centerWidth = 0.4f;
	[SerializeField]
	private GameObject shockWave;
	[SerializeField]
	private GameObject shockWaveEffect;
	[SerializeField]
	private float wavePosZ = 10.0f;


	private KinectManager manager;
	private Vector3 handPos;
	private Vector3 shoulderPos;
	private float centerWidthHalf;

	private MyFlag rightAttackFlag;
	private MyFlag leftAttackFlag;
	private MyFlag centerAttackFlag;

	// プレイヤーからの衝撃波発生位置
	private Vector3 rightPos;
	private Vector3 leftPos;
	private Vector3 centerPos;

	// プレイヤーオブジェクト
	private GameObject player;

	// Use this for initialization
	void Start () 
	{
		// キネクトマネージャーの取得
		manager = KinectManager.Instance;

		// 中心域の半値を保存
		centerWidthHalf = centerWidth * 0.5f;

		// 位置の設定
		rightPos = new Vector3 (shockWave.transform.localScale.x, 0.0f, wavePosZ);
		leftPos = new Vector3 (-shockWave.transform.localScale.x, 0.0f, wavePosZ);
		centerPos = new Vector3 (0.0f, 0.0f, wavePosZ);

		// プレイヤーオブジェクトの取得
		player = GameObject.FindGameObjectWithTag ("Player");
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (manager && manager.IsUserDetected ()) 
		{
			CheckJointPosition();
		}
	}

	void CheckJointPosition()
	{
		long userId = manager.GetUserIdByIndex (0);  // manager.GetPrimaryUserID();

		// 手の位置を取得　　
		handPos = manager.GetJointPosition (userId, (int)KinectInterop.JointType.HandRight);
		SwapXZ (ref handPos);
		
		// 肩の位置を取得
		shoulderPos = manager.GetJointPosition (userId, (int)KinectInterop.JointType.ShoulderRight);
		SwapXZ (ref shoulderPos);

		// 差を計算
		Vector3 deffPos = handPos - shoulderPos;

		// 長さを取得
		float length = deffPos.magnitude;

		// 腕を前に突き出したときの処理 
		if (length > actionDistance && handPos.y > actionHeight) {
			// 右検知
			if (deffPos.x > centerWidthHalf) {
				if (rightAttackFlag.FlagChack (true)) {
					CreateShockWave (player.transform.position + rightPos);
				}
			}

			// 左検知
			if (deffPos.x < -centerWidthHalf) {
				if (leftAttackFlag.FlagChack (true)) {
					CreateShockWave (player.transform.position + leftPos);
				}
			}

			// 中央検知
			if (deffPos.x < centerWidthHalf && deffPos.x > -centerWidthHalf) {
				if (centerAttackFlag.FlagChack (true)) {
					CreateShockWave (player.transform.position + centerPos);
				}
			} 
		} 
		else 
		{
			leftAttackFlag.FlagChack (false);
			centerAttackFlag.FlagChack (false);
			rightAttackFlag.FlagChack (false);
		}
		//print (deffPos);
	}

	void SwapXZ(ref Vector3 pos)
	{
		float tmp;
		tmp = pos.x;
		pos.x = pos.z;
		pos.z = tmp;
	}

	void CreateShockWave(Vector3 pos)
	{
		Instantiate(shockWave, pos,Quaternion.identity);
		Instantiate (shockWaveEffect, pos, Quaternion.identity);

		// 音を鳴らす
		SoundManager.Inst.PlaySE (SE.Foot);
	}
}
