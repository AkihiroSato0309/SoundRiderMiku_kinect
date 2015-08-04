using UnityEngine;
using System.Collections;


public class ChangeBox : MonoBehaviour
{
	// --------------- inspector ---------------
	[SerializeField]
	int hp = 3;						// HP.
	[SerializeField]
	float distanceToPlayer = 10;	// プレイヤーとチェックボックス間で保たれる距離.
	[SerializeField]
	Vector3 initialRot;				// 初期のtransform.rotation
	[SerializeField]
	Vector3 rotSpd;					// 毎フレームにおける回転速度
	[SerializeField]
	float rotCoefficientOnDying;	// HPが0になった時に回転速度に掛ける係数
	[SerializeField]
	SE hitSE;						// ヒット時のSE.
	[SerializeField]
	SE DestroySE;					// 破壊時のSE.

	// --------------- private ---------------
	Transform playerTransform;
	float rotCoefficient = 1;


	/************************************************************************************//**
	開始.
		
	@return なし	.
	****************************************************************************************/
	void Start ()
	{
		this.transform.rotation = Quaternion.Euler (initialRot);
	}

	/************************************************************************************//**
	更新.
	
	@return なし	.
	****************************************************************************************/
	void Update ()
	{
		// プレイヤーと一定の距離を保つように座標を更新.
		float targetZ = this.playerTransform.position.z + this.distanceToPlayer;
		var pos = this.transform.position;
		pos.z = Mathf.Lerp (pos.z, targetZ, 0.1f);
		this.transform.position = pos;

		// 回転する
		this.transform.Rotate (this.rotSpd * this.rotCoefficient);
	}
	
	/************************************************************************************//**
	衝突開始時. 
		
	@param [in] col 衝突情報.
	
	@return なし	.
	****************************************************************************************/
	void OnCollisionEnter (Collision col)
	{
		if (col.gameObject.tag == "Bullet")
		{
			if (--this.hp == 0)
			{
				SoundManager.Inst.MoveToNextPhase();
				this.rotCoefficient = this.rotCoefficientOnDying;
			}
			else
			{
				SoundManager.Inst.PlaySE (this.hitSE);
			}
		}
	}

	/************************************************************************************//**
	初期化.

	@param [in]	playerTransform	PlayerのTransform.
	@param [in]	fadeCamera		FadeCameraのスクリプト.
		
	@return なし.
	****************************************************************************************/
	public void Init (Transform playerTransform)
	{
		this.playerTransform = playerTransform;
	}

	/************************************************************************************//**
	破壊.
		
	@return なし	.
	****************************************************************************************/
	public void Destroy()
	{
		// SEを鳴らす.
		//SoundManager.Inst.PlaySE (this.DestroySE);

		// ボックスを削除.
		this.Destroy (this.gameObject);
	}
}

