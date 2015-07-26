using UnityEngine;
using System.Collections;

public class ShotBullet : MonoBehaviour {

	public GameObject bullet;
	public float shotInterval = 0.5f;

	private GameObject handCheckObject;
	private HandCheck handCheck;
	private float shotCount = 0.0f;

	void Start()
	{
		handCheckObject = GameObject.Find ("HandCheck");
		handCheck = handCheckObject.GetComponent<HandCheck> ();
	}

	void Update()
	{
		if (handCheck.IsHandUp == true) 
		{
			shotCount -=  Time.deltaTime;
			if(shotCount < 0.0f)
			{
				Shot (handCheck.Shoulder2HandVec);
				shotCount = shotInterval;
			}
		}
	}

	public void Shot(Vector3 shotVec)
	{
		GameObject tmp = Instantiate (bullet, transform.position, Quaternion.identity) as GameObject;
		tmp.GetComponent<Bullet> ().SetMoveVec (shotVec);
	}
}
