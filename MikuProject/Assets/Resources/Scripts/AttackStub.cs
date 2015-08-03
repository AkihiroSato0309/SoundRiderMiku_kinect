using UnityEngine;
using System.Collections;

public class AttackStub : MonoBehaviour {

	public GameObject shockWave;
	private float wavePosZ = 15.0f;

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
		GameObject obj;

		if (Input.GetKey (KeyCode.RightArrow)) {
			if(rightAttackFlag.FlagChack(true))
			{
				obj = Instantiate(shockWave, player.transform.position + rightPos,Quaternion.identity) as GameObject;
				obj.transform.SetParent(player.transform);
			}
		} else
			rightAttackFlag.FlagChack(false);

		if (Input.GetKey (KeyCode.LeftArrow)) {
			if(leftAttackFlag.FlagChack (true))
			{
				obj = Instantiate(shockWave, player.transform.position + leftPos,Quaternion.identity) as GameObject;
				obj.transform.SetParent(player.transform);
			}
		} else
			leftAttackFlag.FlagChack (false);

		if (Input.GetKey (KeyCode.UpArrow)) {
			if(centerAttackFlag.FlagChack (true))
			{
				obj = Instantiate(shockWave, player.transform.position + centerPos,Quaternion.identity) as GameObject;
				obj.transform.SetParent(player.transform);
			}
		} else
			centerAttackFlag.FlagChack (false);
	}
}
