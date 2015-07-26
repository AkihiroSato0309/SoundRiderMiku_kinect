using UnityEngine;
using System.Collections;

public class SlideCheck : MonoBehaviour {

	public float moveSpeed = 0.1f;

	private KinectManager manager;

	private Vector3 footRightPos;
	private Vector3 footLeftPos;

	private float depthDeff;

	// 初期化処理
	void Start () {
		// キネクトマネージャーの取得
		manager = KinectManager.Instance;
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

		transform.Translate (depthDeff * moveSpeed, 0.0f, 0.0f);
	}
}
