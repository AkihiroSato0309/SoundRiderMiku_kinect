using UnityEngine;
using System.Collections;

public class AutoDestory : MonoBehaviour {

	public float destoryTime = 4.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		destoryTime -= Time.deltaTime;
		if (destoryTime < 0.0f)
			Destroy (gameObject);
	}
}
