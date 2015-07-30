using UnityEngine;
using System.Collections;


public class RandomGenerator : MonoBehaviour
{
	[SerializeField]
	GameObject obj;
	[SerializeField]
	float generationPerSec;
	[SerializeField]
	int instantiationPerGeneration;
	[SerializeField]
	Vector3 minXYZ;
	[SerializeField]
	Vector3 maxXYZ;

	Timer timer;


	// Use this for initialization
	void Start ()
	{
		if (this.generationPerSec == 0)
		{
			this.generationPerSec = -1;
			Debug.LogError ("RandomGenerator.generationPerSec must not be 0.0f.");
		}

		this.timer = new TimerWithSoundManager (1 / this.generationPerSec, this.Generate);
	}
	
	// Update is called once per frame
	void Update ()
	{
		this.timer.Update ();
	}

	void Generate()
	{
		if (this.obj == null) return;

		for (int i = 0; i < this.instantiationPerGeneration; i++)
		{
			float x = Random.Range (this.minXYZ.x, this.maxXYZ.x);
			float y = Random.Range (this.minXYZ.y, this.maxXYZ.y);
			float z = Random.Range (this.minXYZ.z, this.maxXYZ.z);
			Vector3 pos = this.transform.position + new Vector3(x, y, z);

			var newObj = this.Instantiate (obj, pos, Quaternion.identity) as GameObject;
			newObj.transform.parent = this.transform;
		}
	}
}
