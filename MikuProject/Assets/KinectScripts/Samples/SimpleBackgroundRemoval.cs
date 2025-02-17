﻿using UnityEngine;
using System.Collections;

public class SimpleBackgroundRemoval : MonoBehaviour 
{
	// whether to display the foreground texture on the screen or not
	public bool displayForeground = true;


	// the foreground texture
	private Texture2D foregroundTex;
	
	// rectangle taken by the foreground texture (in pixels)
	private Rect foregroundRect;

	// the Kinect manager
	private KinectManager manager;
	

	void Start () 
	{
		manager = KinectManager.Instance;

		if(manager && manager.IsInitialized())
		{
			Rect cameraRect = Camera.main.pixelRect;
			float rectHeight = cameraRect.height;
			float rectWidth = cameraRect.width;

			KinectInterop.SensorData sensorData = manager.GetSensorData();

			if(sensorData != null && sensorData.sensorInterface != null)
			{
				if(rectWidth > rectHeight)
					rectWidth = rectHeight * sensorData.depthImageWidth / sensorData.depthImageHeight;
				else
					rectHeight = rectWidth * sensorData.depthImageHeight / sensorData.depthImageWidth;
				
				foregroundRect = new Rect((cameraRect.width - rectWidth) / 2, cameraRect.height - (cameraRect.height - rectHeight) / 2, rectWidth, -rectHeight);
			}
		}
	}
	
	void Update () 
	{
		if(manager && manager.IsInitialized())
		{
			foregroundTex = manager.GetUsersLblTex();
		}
	}

	void OnGUI()
	{
		if(displayForeground && foregroundTex)
		{
			GUI.DrawTexture(foregroundRect, foregroundTex);
		}
	}
	
}
