using UnityEngine;
using System.Collections;

public class ObjectPositionTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
		foreach (Transform child in transform) 
		{
			child.position = new Vector3 (0.0f, 4.0f, 0.0f) + transform.position;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
