using UnityEngine;
using System.Collections;

public class TargetController : MonoBehaviour, ITargetController {

	[SerializeField]
	private float virtualWallDistance = 1000.0f;

	private HandCheck handCheck;
	private Vector2 targetPos;

	// Use this for initialization
	void Start () 
	{
		handCheck = GameObject.Find("HandCheck").GetComponent<HandCheck>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		Vector2 pos;
		Vector3 shoulderVec = handCheck.Shoulder2HandVec;

		targetPos = (Vector2)((virtualWallDistance / shoulderVec.z) * shoulderVec);

	}

	public Vector2 GetTargetPosition()
	{
		//print (targetPos);
		return targetPos;
	}
}
