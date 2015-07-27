using UnityEngine;
using System.Collections;

public class SoundTest : MonoBehaviour {

	[SerializeField]
	private AudioSource audio;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Space)) {
			audio.time = 0.0f;
			audio.Play();
			print ("test");
		}
	}
}
