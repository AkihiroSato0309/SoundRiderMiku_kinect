/***********************************************************************************************//**

@file BGObj.cs
	
@author Keisuke Ohe
		
***************************************************************************************************/
		
using UnityEngine;
using System.Collections;


/***********************************************************************************************//**

背景装飾オブジェクトクラス.
	
***************************************************************************************************/
public class BGObj : MonoBehaviour
{
	GameObject[] parts;


	void Start ()
	{
		const int partNum = 5;
		const string partName = "Act_";

		this.parts = new GameObject[partNum];

		for (int i = 0; i < partNum; i++)
		{
			this.parts[i] = this.transform.FindChild (partName + i.ToString()).gameObject;
		}
	}

	/************************************************************************************//**
	フェイズが次の段階に進んだ時に呼ばれる. 見た目が派手になる.
		
	@param [in] nextPhase 次のフェイズの番号（段階を示す値）
			
	@return なし		
	****************************************************************************************/
	public void MoveToNextPhase (int nextPhase)
	{
		if (nextPhase >= parts.Length) return;

		parts [nextPhase].SetActive (true);
	}
}