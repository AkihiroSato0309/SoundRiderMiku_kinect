using UnityEngine;
using System.Collections;

public class KinectSenserInitializer{

	private KinectManager manager;

	private Rect foregroundGuiRect;
	private Rect foregroundImgRect;
	
	private int depthImageWidth;
	private int depthImageHeight;

	private static KinectSenserInitializer instance = null;

	public Rect ForegroundImgRect{
		get{return foregroundImgRect;}
	}

	public static KinectSenserInitializer Instance{
		get{
			if(instance == null)
				instance = new KinectSenserInitializer();
			return instance;
		}
	}

	// Use this for initialization
	public void SencerInitialize () {
		// キネクトマネージャーの取得
		manager = KinectManager.Instance;

		if (manager && manager.IsInitialized ()) {
			KinectInterop.SensorData sensorData = manager.GetSensorData ();
		
			if (sensorData != null && sensorData.sensorInterface != null) {
				// get depth image size
				depthImageWidth = sensorData.depthImageWidth;
				depthImageHeight = sensorData.depthImageHeight;
			
				// calculate the foreground rectangles
				Rect cameraRect = Camera.main.pixelRect;
				float rectHeight = cameraRect.height;
				float rectWidth = cameraRect.width;
			
				if (rectWidth > rectHeight)
					rectWidth = rectHeight * depthImageWidth / depthImageHeight;
				else
					rectHeight = rectWidth * depthImageHeight / depthImageWidth;
			
				float foregroundOfsX = (cameraRect.width - rectWidth) / 2;
				float foregroundOfsY = (cameraRect.height - rectHeight) / 2;
				foregroundImgRect = new Rect (foregroundOfsX, foregroundOfsY, rectWidth, rectHeight);
				foregroundGuiRect = new Rect (foregroundOfsX, cameraRect.height - foregroundOfsY, rectWidth, -rectHeight);
			}
		}
	}
}
