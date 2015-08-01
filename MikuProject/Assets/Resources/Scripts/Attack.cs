using UnityEngine;
using System.Collections;

public class Attack : MonoBehaviour {

	private KinectManager manager;
	private Vector3 handPos;
	private Vector3 shoulderPos;

	// Use this for initialization
	void Start () 
	{
		// キネクトマネージャーの取得
		manager = KinectManager.Instance;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (manager && manager.IsUserDetected ()) 
		{
			long userId = manager.GetUserIdByIndex (0);  // manager.GetPrimaryUserID();
			
			// 手の位置を取得　　
			handPos = manager.GetJointPosition (userId, (int)KinectInterop.JointType.HandRight);
			
			// 肩の位置を取得
			shoulderPos = manager.GetJointPosition (userId, (int)KinectInterop.JointType.ShoulderRight);
		}
	}
}
