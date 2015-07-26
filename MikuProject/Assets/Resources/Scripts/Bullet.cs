using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	public float moveSpeed;

	private Vector3 moveVec;
	private Vector3 move;

	public void SetMoveVec(Vector3 vec)
	{
		moveVec = vec.normalized;
		move = moveVec * moveSpeed;
	}

	// Update is called once per frame
	void Update () 
	{
		transform.Translate (move);
	}

	void OnCollisionEnter(Collision col)
	{
		print ("hit");
	}
}
