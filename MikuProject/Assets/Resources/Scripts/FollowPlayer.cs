using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour {

	[SerializeField]
	private GameObject target;

	private float distance;
	private Vector3 initPos;


	// 初期化処理
	void Start () 
	{
		distance = transform.position.z * -1.0f;
		initPos = transform.position;
	}

	// 更新処理
	void LateUpdate()
	{
		Vector3 pos = target.transform.position;
		pos.z -= distance;
		pos.y = initPos.y;
		transform.position = pos;
	}
}
