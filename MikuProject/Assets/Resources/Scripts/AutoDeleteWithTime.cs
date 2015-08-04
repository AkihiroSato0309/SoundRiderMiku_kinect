using UnityEngine;
using System.Collections;

public class AutoDeleteWithTime : MonoBehaviour 
{
	public float deleteTime = 1.0f;

	// 更新処理
	void Update () 
	{
		deleteTime -= Time.deltaTime;
		if (deleteTime < 0.0f) 
		{
			Destroy(gameObject);
		}
	}
}