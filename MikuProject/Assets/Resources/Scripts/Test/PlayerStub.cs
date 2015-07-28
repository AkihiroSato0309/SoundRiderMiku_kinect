using UnityEngine;
using System.Collections;

public class PlayerStub : MonoBehaviour
{
	[SerializeField]
	private float speed = 1;
	[SerializeField]
	private int bpm = 135;

	public int BPM { get { return this.bpm; } }

	private SoundManager soundManager;
	private Vector3 initialPos;


	void Start ()
	{
		this.soundManager = GameObject.FindWithTag ("SoundManager").GetComponent<SoundManager> ();
		this.initialPos = this.transform.position;
	}

	void Update()
	{
		Vector3 newPos = new Vector3 (initialPos.x, initialPos.y, this.soundManager.Time * speed);
		//this.transform.position = newPos;

		if (Input.GetKeyDown (KeyCode.Space))
		{
			this.soundManager.PlaySE (SE.Jump);
		};
	}
}
