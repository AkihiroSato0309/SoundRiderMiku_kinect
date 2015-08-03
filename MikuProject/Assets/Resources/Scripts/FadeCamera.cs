using UnityEngine;
using System.Collections;

public class FadeCamera : MonoBehaviour {

	private ParticleSystem[] particles;

	// 開始処理
	void Start () 
	{
		particles = new ParticleSystem[transform.childCount];
		int i = 0;
		foreach (Transform child in transform) 
		{
			particles[i] = child.gameObject.GetComponent<ParticleSystem>();
			i++;
		}
	}
	
	// 更新処理
	void Update () 
	{
		if (Input.GetKeyDown (KeyCode.Space)) 
		{
			FadeStart();
		}
	}
	
	// フェードを開始する
	public void FadeStart()
	{
		foreach (ParticleSystem p in particles) 
		{
			p.Play();
		}
	}
}
