using UnityEngine;
using System.Collections;

public class AttackStub : MonoBehaviour {

	public GameObject shockWave;
	public GameObject shockWaveEffect;
	public float wavePosZ = 10.0f;

	private GameObject player;

	private MyFlag rightAttackFlag;
	private MyFlag leftAttackFlag;
	private MyFlag centerAttackFlag;

	private Vector3 rightPos;
	private Vector3 leftPos;
	private Vector3 centerPos;

	// Use this for initialization
	void Start () 
	{
		rightPos = new Vector3 (shockWave.transform.localScale.x, 0.0f, wavePosZ);
		leftPos = new Vector3 (-shockWave.transform.localScale.x, 0.0f, wavePosZ);
		centerPos = new Vector3 (0.0f, 0.0f, wavePosZ);

		player = GameObject.FindGameObjectWithTag ("Player");
	}
	
	// Update is called once per frame
	void Update () 
	{


		if (Input.GetKey (KeyCode.RightArrow)) {
			if(rightAttackFlag.FlagChack(true))
			{
				CreateShockWave( player.transform.position + rightPos);
			}
		} else
			rightAttackFlag.FlagChack(false);

		if (Input.GetKey (KeyCode.LeftArrow)) {
			if(leftAttackFlag.FlagChack (true))
			{
				CreateShockWave( player.transform.position + leftPos);
			}
		} else
			leftAttackFlag.FlagChack (false);

		if (Input.GetKey (KeyCode.UpArrow)) {
			if(centerAttackFlag.FlagChack (true))
			{
				CreateShockWave( player.transform.position + centerPos);
			}
		} else
			centerAttackFlag.FlagChack (false);
	}

	void CreateShockWave(Vector3 pos)
	{
		Instantiate(shockWave, pos,Quaternion.identity);
		Instantiate (shockWaveEffect, pos, Quaternion.identity);
	}
}
