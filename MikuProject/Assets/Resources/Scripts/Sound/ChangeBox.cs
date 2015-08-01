using UnityEngine;
using System.Collections;


public class ChangeBox : MonoBehaviour
{
	[SerializeField]
	int hp = 3;
	[SerializeField]
	GameObject explosionFXPrefab;


	public void OnCollision (Collision col)
	{
		if (col.gameObject.tag == "Bullet")
		{
			if (--this.hp == 0)
			{
				//SoundManager.Inst.ToString();
				this.Destroy ();
			}
		}
	}

	void Destroy()
	{
		// エフェクト生成
		var fx = Instantiate (this.explosionFXPrefab);
		fx.transform.position = this.transform.position;

		// ボックスを削除
		this.Destroy (this.gameObject);
	}
}

