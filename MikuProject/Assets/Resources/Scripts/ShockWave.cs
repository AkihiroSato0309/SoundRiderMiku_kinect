using UnityEngine;
using System.Collections;

public class ShockWave : MonoBehaviour {

	private float lifeTime = 0.5f;

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		lifeTime -= Time.deltaTime;
		if (lifeTime < 0.0f) 
		{
			Destroy(gameObject);
		}
	}


}
