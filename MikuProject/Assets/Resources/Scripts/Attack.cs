using UnityEngine;
using System.Collections;

public class Attack : MonoBehaviour {

	[SerializeField]
	private float actionDistance = 0.4f;
	[SerializeField]
	private float actionHeight = 1.0f;
	[SerializeField]
	private float centerWidth = 0.4f;

	private KinectManager manager;
	private Vector3 handPos;
	private Vector3 shoulderPos;
	private float centerWidthHalf;

	private MyFlag rightAttackFlag;
	private MyFlag leftAttackFlag;
	private MyFlag centerAttackFlag;

	// Use this for initialization
	void Start () 
	{
		// キネクトマネージャーの取得
		manager = KinectManager.Instance;

		// 中心域の半値を保存
		centerWidthHalf = centerWidth * 0.5f;
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
		if (length > actionDistance && handPos.y > actionHeight) 
		{
			if(deffPos.x < -centerWidthHalf)
			{
				print ("left");
			}
			else if(deffPos.x > centerWidthHalf)
			{
				print ("right");
			}
			else
			{
				print ("center");
			}
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
}
