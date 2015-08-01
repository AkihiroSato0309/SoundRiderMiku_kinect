using UnityEngine;
using System.Collections;


public class PlayerStub : MonoBehaviour
{
	[SerializeField]
	private float speedZ = 10;
	private float speedX = 1;

	private SoundManager soundManager;


	void Start ()
	{
		this.soundManager = GameObject.FindWithTag ("SoundManager").GetComponent<SoundManager> ();
	}

	void Update()
	{
		Vector3 newPos = new Vector3 (this.transform.position.x, this.transform.position.y, this.soundManager.Time * this.speedZ);
		this.transform.position = newPos;

		if (Input.GetKeyDown (KeyCode.Space))
		{
			this.soundManager.PlaySE (SE.Foot);
		};

		if (Input.GetKey (KeyCode.A)) this.transform.Translate (-this.speedX, 0, 0);
		if (Input.GetKey (KeyCode.D)) this.transform.Translate (this.speedX, 0, 0);
	}
}
