using UnityEngine;
using System.Collections;

public class DepthImageViewer : MonoBehaviour 
{
	// The index of the used player. Default is 0 (first player).
	public int playerIndex = 0;
	
	// the KinectManager instance
	private KinectManager manager;

	// the foreground texture
	private Texture2D foregroundTex;
	
	// rectangle taken by the foreground texture (in pixels)
	private Rect foregroundGuiRect;
	private Rect foregroundImgRect;

	// game objects to contain the joint colliders
	//private GameObject[] jointColliders = null;
	private GameObject handRight;
	private GameObject handLeft;
	private GameObject kneeRight;
	private GameObject kneeLeft;
	private GameObject head;
	private int numColliders = 0;

	private int depthImageWidth;
	private int depthImageHeight;

	void Start () 
	{
		manager = KinectManager.Instance;

		if(manager && manager.IsInitialized())
		{
			KinectInterop.SensorData sensorData = manager.GetSensorData();

			if(sensorData != null && sensorData.sensorInterface != null)
			{
				// get depth image size
				depthImageWidth = sensorData.depthImageWidth;
				depthImageHeight = sensorData.depthImageHeight;

				// calculate the foreground rectangles
				Rect cameraRect = Camera.main.pixelRect;
				float rectHeight = cameraRect.height;
				float rectWidth = cameraRect.width;
				
				if(rectWidth > rectHeight)
					rectWidth = rectHeight * depthImageWidth / depthImageHeight;
				else
					rectHeight = rectWidth * depthImageHeight / depthImageWidth;
				
				float foregroundOfsX = (cameraRect.width - rectWidth) / 2;
				float foregroundOfsY = (cameraRect.height - rectHeight) / 2;
				foregroundImgRect = new Rect(foregroundOfsX, foregroundOfsY, rectWidth, rectHeight);
				foregroundGuiRect = new Rect(foregroundOfsX, cameraRect.height - foregroundOfsY, rectWidth, -rectHeight);
				
				// create joint colliders
				numColliders = sensorData.jointCount;
				//jointColliders = new GameObject[numColliders];
				/*
				for(int i = 0; i < numColliders; i++)
				{
					string sColObjectName = ((KinectInterop.JointType)i).ToString() + "Collider";
					jointColliders[i] = new GameObject(sColObjectName);
					jointColliders[i].transform.parent = transform;
					
					SphereCollider collider = jointColliders[i].AddComponent<SphereCollider>();
					collider.radius = 0.2f;
				}
				*/

				handRight = new GameObject("handRight");
				handRight.transform.parent = transform;
				handRight.AddComponent<SphereCollider>().radius = 0.2f;

				handLeft = new GameObject("handLeft");
				handLeft.transform.parent = transform;
				handLeft.AddComponent<SphereCollider>().radius = 0.2f;

				kneeRight = new GameObject("kneeRight");
				kneeRight.transform.parent = transform;
				kneeRight.AddComponent<SphereCollider>().radius = 0.2f;

				kneeLeft = new GameObject("kneeLeft");
				kneeLeft.transform.parent = transform;
				kneeLeft.AddComponent<SphereCollider>().radius = 0.2f;

				head = new GameObject("head");
				head.transform.parent = transform;
				head.AddComponent<SphereCollider>().radius = 0.2f;

			}
		}

	}
	
	void Update () 
	{
		// get the users texture
		if(manager && manager.IsInitialized())
		{
			foregroundTex = manager.GetUsersLblTex();
		}

		if(manager && manager.IsUserDetected())
		{
			long userId = manager.GetUserIdByIndex(playerIndex);  // manager.GetPrimaryUserID();

			// 衝突範囲の更新
			/*
			for(int i = 0; i < numColliders; i++)
			{
				if(manager.IsJointTracked(userId, i))
				{
					Vector3 posCollider = manager.GetJointPosDepthOverlay(userId, i, Camera.main, foregroundImgRect);
					jointColliders[i].transform.position = posCollider;
				}
			}
			*/
			Vector3 posCollider = manager.GetJointPosDepthOverlay(userId, (int)KinectInterop.JointType.HandRight, Camera.main, foregroundImgRect);
			handRight.transform.position = posCollider;

			posCollider = manager.GetJointPosDepthOverlay(userId, (int)KinectInterop.JointType.HandLeft, Camera.main, foregroundImgRect);
			handLeft.transform.position = posCollider;

			posCollider = manager.GetJointPosDepthOverlay(userId, (int)KinectInterop.JointType.KneeRight, Camera.main, foregroundImgRect);
			kneeRight.transform.position = posCollider;

			posCollider = manager.GetJointPosDepthOverlay(userId, (int)KinectInterop.JointType.KneeLeft, Camera.main, foregroundImgRect);
			kneeLeft.transform.position = posCollider;

			posCollider = manager.GetJointPosDepthOverlay(userId, (int)KinectInterop.JointType.Head, Camera.main, foregroundImgRect);
			head.transform.position = posCollider;
		}

	}

	void OnGUI()
	{
		if(foregroundTex)
		{
			GUI.DrawTexture(foregroundGuiRect, foregroundTex);
		}
	}

}
