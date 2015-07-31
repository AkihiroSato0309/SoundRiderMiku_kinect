using UnityEngine;
using System.Collections;

public class PlayerTarget : MonoBehaviour {

	// 初期化処理
	void Start () 
	{
	
	}
	
	// 更新処理
	void Update () 
	{


	}

	void MoveTmp()
	{
		float x = Input.GetAxisRaw ("Horizontal") * 10.0f;
		float y = Input.GetAxisRaw ("Vertical") * 10.0f;
	}
}
