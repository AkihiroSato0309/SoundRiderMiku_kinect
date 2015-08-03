using UnityEngine;
using System.Collections;

public class HandCheck : MonoBehaviour {
	
	public float checkRangeShoulderToHand = 0.45f;
	public float checkUnderDistance = 0.4f;

	public GameObject showHandObject = null;
	public GameObject showShoulderObject = null;
	public GameObject showShoulder2HandVecObject = null;

	private KinectManager manager;

	private Vector3 handPos;
	private Vector3 shoulderPos;

	private Vector3 shoulder2HandVec;

	private bool isHandUp;

	public Vector3 Shoulder2HandVec
	{
		get{return shoulder2HandVec;}
	}

	public bool IsHandUp
	{
		get{return isHandUp;}
	}

	// 初期化処理
	void Start () 
	{
		// キネクトマネージャーの取得
		manager = KinectManager.Instance;
	}

	// 更新処理
	void Update () {
		if (manager && manager.IsUserDetected ()) {
			long userId = manager.GetUserIdByIndex (0);  // manager.GetPrimaryUserID();

			// 手の位置を取得　　
			handPos = manager.GetJointPosition (userId, (int)KinectInterop.JointType.HandRight);
			if(showHandObject != null)showHandObject.transform.position = handPos;

			// 肩の位置を取得
			shoulderPos = manager.GetJointPosition(userId, (int)KinectInterop.JointType.ShoulderRight);
			if(showShoulderObject != null)showShoulderObject.transform.position = shoulderPos;

			// 手と肩の位置の差異で判定を行う
			if((handPos - shoulderPos).magnitude > checkRangeShoulderToHand &&
			   handPos.y > (shoulderPos.y - checkUnderDistance))
			{
				//print ("肩");
				shoulder2HandVec = handPos - shoulderPos;

				float tmp = shoulder2HandVec.z;
				shoulder2HandVec.z = shoulder2HandVec.x;
				shoulder2HandVec.x = tmp;
				shoulder2HandVec.Normalize();

				if(showShoulder2HandVecObject != null)showShoulder2HandVecObject.transform.LookAt(showShoulder2HandVecObject.transform.position + shoulder2HandVec);
				isHandUp = true;
			}
			else
			{
				isHandUp = false;
			}
		}
	}

	/*
	void OnGUI()
	{
		if (manager && manager.IsUserDetected ()) {
			long userId = manager.GetUserIdByIndex (0);
			GUI.Label (new Rect (0, 10, 100, 30), manager.GetJointPosition (userId, (int)KinectInterop.JointType.ShoulderRight).ToString());
			GUI.Label (new Rect (0, 20, 100, 30), manager.GetJointPosition (userId, (int)KinectInterop.JointType.HandRight).ToString());
		}
	}
	*/
}
