using UnityEngine;
using System.Collections;


public class PlayerStub : MonoBehaviour
{
	[SerializeField]
	private float speed = 1;

	private SoundManager soundManager;


	void Start ()
	{
		this.soundManager = GameObject.FindWithTag ("SoundManager").GetComponent<SoundManager> ();
	}

	void Update()
	{
		Vector3 newPos = new Vector3 (this.transform.position.x, this.transform.position.y, this.soundManager.Time * speed);
		this.transform.position = newPos;

		if (Input.GetKeyDown (KeyCode.Space))
		{
			this.soundManager.PlaySE (SE.Foot);
		};
	}
}
