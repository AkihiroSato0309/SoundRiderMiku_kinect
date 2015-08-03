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
	GameObject explosionFXPrefab;	// チェックボックスが破壊された時に発生するエフェクト.
	[SerializeField]
	SE hitSE;						// ヒット時のSE.
	[SerializeField]
	SE DestroySE;					// 破壊時のSE.

	// --------------- private ---------------
	Transform playerTransform;


	/************************************************************************************//**
	開始.
		
	@return なし		
	****************************************************************************************/
	void Start ()
	{
		this.playerTransform = GameObject.FindWithTag ("Player").transform;
	}

	/************************************************************************************//**
	更新.
	
	@return なし		
	****************************************************************************************/
	void Update ()
	{
		// プレイヤーと一定の距離を保つように座標を更新.
		float targetZ = this.playerTransform.position.z + this.distanceToPlayer;
		var pos = this.transform.position;
		pos.z = Mathf.Lerp (pos.z, targetZ, 0.1f);
		this.transform.position = pos;
	}


	/************************************************************************************//**
	衝突開始時. 
		
	@param [in] col 衝突情報.
	
	@return なし		
	****************************************************************************************/
	public void OnCollisionEnter (Collision col)
	{
		if (col.gameObject.tag == "Bullet")
		{
			if (--this.hp == 0)
			{
				SoundManager.Inst.MoveToNextPhase();
				this.Destroy ();
			}
			else
			{
				SoundManager.Inst.PlaySE (this.hitSE);
			}
		}
	}

	/************************************************************************************//**
	破壊.
		
	@return なし		
	****************************************************************************************/
	void Destroy()
	{
		// エフェクト生成.
		var fx = Instantiate (this.explosionFXPrefab);
		fx.transform.position = this.transform.position;

		// SEを鳴らす.
		SoundManager.Inst.PlaySE (this.DestroySE);

		// ボックスを削除.
		this.Destroy (this.gameObject);
	}
}

