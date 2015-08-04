using UnityEngine;
using System.Collections;


public class PlayerStub : MonoBehaviour, IPlayerCharacter
{
	[SerializeField]
	private float speedX = 0.5f;
	[SerializeField]
	private float speedZ = 10;

	
	void Update()
	{
		Vector3 newPos = new Vector3 (this.transform.position.x, this.transform.position.y, SoundManager.Inst.Time * this.speedZ);
		this.transform.position = newPos;

		if (Input.GetKeyDown (KeyCode.Space))
		{
			SoundManager.Inst.MoveToNextPhase ();
		};

		if (Input.GetKey (KeyCode.A)) this.transform.Translate (-this.speedX, 0, 0);
		if (Input.GetKey (KeyCode.D)) this.transform.Translate (this.speedX, 0, 0);
	}

	public float GetSpeed ()
	{
		return this.speedZ;
	}
}
