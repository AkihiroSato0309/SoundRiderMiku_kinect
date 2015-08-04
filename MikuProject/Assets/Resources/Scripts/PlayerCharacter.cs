using UnityEngine;
using System.Collections;

public class PlayerCharacter : MonoBehaviour, IPlayerCharacter {

	[SerializeField]
	private float speed;
	[SerializeField]
	private float slideSpeed = 0.4f;
	[SerializeField]
	private float offsetX;
	[SerializeField, Range(0.0f, 1.0f)]
	private float turnSmooth = 0.01f;

	private float moveLimit;
	private float movePointX;
	private float rotatePointY;
	private SoundManager soundManager;

	/************************************************************************************//**
	初期化.
		
	@return なし		
	****************************************************************************************/
	void Start () 
	{
		moveLimit = WallManager.Inst.RoadWidth * 0.5f - offsetX;
		this.soundManager = GameObject.FindWithTag ("SoundManager").GetComponent<SoundManager> ();
	}
	
	/************************************************************************************//**
	更新.
	
	@return なし		
	****************************************************************************************/
	void Update ()
	{
		Move ();
		MoveLimitter ();

		if (Input.GetKey (KeyCode.RightArrow))
			MoveSlide (slideSpeed);
		if (Input.GetKey (KeyCode.LeftArrow))
			MoveSlide (-slideSpeed);
	}

	/************************************************************************************//**
	移動制限以内に抑える.
		
	@return なし		
	****************************************************************************************/
	void MoveLimitter ()
	{
		Vector3 pos = transform.position;
		pos.x = Mathf.Clamp (pos.x, -moveLimit, moveLimit);
		transform.position = pos;
		movePointX = pos.x = Mathf.Clamp (movePointX, -moveLimit, moveLimit);
	}

	/************************************************************************************//**
	曲の速さに合わせて移動.
		
	@return なし		
	****************************************************************************************/
	void Move()
	{
		// 横軸の差異を保存
		float deffX = this.transform.position.x;
		Vector3 newPos = new Vector3 (this.transform.position.x, this.transform.position.y, this.soundManager.Time * speed);

		// 補間をかける
		newPos.x = Mathf.Lerp (newPos.x, movePointX, turnSmooth);
		this.transform.position = newPos;

		// ボードを回転させる
		deffX -= newPos.x;
		deffX *= -1.0f;
		rotatePointY = 100.0f * deffX;
		Quaternion rot = Quaternion.identity * Quaternion.AngleAxis (rotatePointY, Vector3.up);
		transform.rotation = rot;
	}

	/************************************************************************************//**
	外部から横移動させる.

	@param 移動方向
	@return なし		
	****************************************************************************************/
	public void MoveSlide(float moveX)
	{
		movePointX = transform.position.x + moveX;
	}

	/************************************************************************************//**
	基底スピードの取得
		
	@return 基底スピード
	****************************************************************************************/
	public float GetSpeed()
	{
		return speed;
	}

}
