using UnityEngine;
using System.Collections;

public class TargetControllerStub : MonoBehaviour, ITargetController {

	private Vector2 targetPos = Vector2.zero;

	// Use this for initialization
	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () 
	{
		MoveTmp ();
	}


	public Vector2 GetTargetPosition()
	{
		return targetPos;
	}

	void MoveTmp()
	{
		float x = Input.GetAxisRaw ("Horizontal") * 10.0f;
		float y = Input.GetAxisRaw ("Vertical") * 10.0f;

		targetPos.x += x;
		targetPos.y += y;
	}
}
