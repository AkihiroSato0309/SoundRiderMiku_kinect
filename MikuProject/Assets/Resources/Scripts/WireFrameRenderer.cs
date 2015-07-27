/***********************************************************************************************//**

@file WireFrameRenderer.cs
	
@author Keisuke Ohe
		
***************************************************************************************************/

using UnityEngine;
using System.Collections;


/***********************************************************************************************//**

ワイヤーフレーム描画化スクリプト.

***************************************************************************************************/
[RequireComponent (typeof(MeshFilter))]
[RequireComponent (typeof(MeshRenderer))]
public class WireFrameRenderer : MonoBehaviour
{
	// --------------- inspector ---------------
	[SerializeField]
	private Color color = new Color(0.4f, 0.4f, 0.9f);

	// --------------- property ---------------
	public Color Color { set { this.meshRenderer.material.color = value; } }

	// --------------- private ---------------
	private MeshRenderer meshRenderer;


	/************************************************************************************//**
	開始.
		
	@return なし		
	****************************************************************************************/
	void Start ()
	{
		// 頂点インデックス情報はそのままに, 描画方法をLineStripに変更.
		MeshFilter meshFilter = GetComponent<MeshFilter> ();
		meshFilter.mesh.SetIndices (meshFilter.mesh.GetIndices(0), MeshTopology.LineStrip, 0);

		// 色を更新し, 影を無効化.
		this.meshRenderer = GetComponent<MeshRenderer> ();
		this.meshRenderer.material.color = this.color;
		this.meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
		this.meshRenderer.receiveShadows = false;
	}
}
