using UnityEngine;
using System.Collections;

public class kinectManagerCustom : KinectManager {
	void Awake(){
		base.Awake ();
		KinectSenserInitializer.Instance.SencerInitialize ();
	}
}
