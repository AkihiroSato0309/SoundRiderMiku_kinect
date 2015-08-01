using UnityEngine;
using System.Collections;

public class BulletTarget : MonoBehaviour {

	[SerializeField]
	private float radius;

	private float screenRadius;
	private RectTransform rectTransform;
	private ITargetController targetController;
	private Camera mainCamera;

	// 初期化処理
	void Start () 
	{
		rectTransform = GetComponent<RectTransform> ();
		targetController = GameObject.Find("TargetController").GetComponent<ITargetController>();
		mainCamera = Camera.main;
		screenRadius = radius * (Screen.width / 1920.0f); 
	}
	
	// 更新処理
	void Update () 
	{
		MoveTarget (targetController.GetTargetPosition());

		GameObject[] enemys = GameObject.FindGameObjectsWithTag ("Enemy");
		foreach (GameObject enemy in enemys) 
		{
			Vector2 pos = mainCamera.WorldToScreenPoint(enemy.transform.position);
			float length = (pos - (Vector2)rectTransform.position).magnitude;
			if(length < screenRadius)
			{
				print("hit");
			}
		}
	}

	// ターゲット移動
	void MoveTarget(Vector2 pos)
	{
		rectTransform.anchoredPosition = pos;
	}

}
