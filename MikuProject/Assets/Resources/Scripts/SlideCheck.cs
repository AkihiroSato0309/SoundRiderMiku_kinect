using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerCharacter))]
public class SlideCheck : MonoBehaviour {

	public float moveSpeed = 1.0f;

	private KinectManager manager;

	private Vector3 footRightPos;
	private Vector3 footLeftPos;

	private float depthDeff;

	private PlayerCharacter playerChara;

	// 初期化処理
	void Start () {
		// キネクトマネージャーの取得
		manager = KinectManager.Instance;

		// プレイヤーのスクリプトを取得
		playerChara = GetComponent<PlayerCharacter> ();
	}

	
	// Update is called once per frame
	void Update () {
		if (manager && manager.IsUserDetected ()) {
			long userId = manager.GetUserIdByIndex (0);  // manager.GetPrimaryUserID();

			// 右足の位置を取得　　
			footRightPos = manager.GetJointPosition (userId, (int)KinectInterop.JointType.FootRight);
			
			// 左足の位置を取得
			footLeftPos = manager.GetJointPosition(userId, (int)KinectInterop.JointType.FootLeft);

			// 差を計算
			depthDeff = footRightPos.z - footLeftPos.z;
		}
		print (depthDeff);
		playerChara.MoveSlide (depthDeff * moveSpeed);
	}
}
