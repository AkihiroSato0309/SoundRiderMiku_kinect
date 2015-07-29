using UnityEngine;
using System.Collections;


[RequireComponent(typeof(LineRenderer))]
public class PlayingPosVisualizer : MonoBehaviour
{
	[SerializeField]
	private AudioSource audio;
	[SerializeField]
	private Camera targetCamera;

	private LineRenderer lineRenderer;


	/************************************************************************************//**
	開始.
		
	@return なし		
	****************************************************************************************/
	void Start ()
	{
		// LineRendererの取得と初期化.
		this.lineRenderer = this.GetComponent<LineRenderer> ();
		this.lineRenderer.SetVertexCount (2);
	}
	
	/************************************************************************************//**
	更新.
		
	@return なし		
	****************************************************************************************/
	void Update ()
	{
		const float LENGTH_BETWEEN_SECS = 3;
		const float START_POS_Y = 10;
		const float END_POS_Y = -10;

		float posX = this.transform.position.x + (this.audio.time * LENGTH_BETWEEN_SECS);
		float posZ = this.transform.position.z;
		var pos = new Vector3 (posX, START_POS_Y, posZ);
		this.lineRenderer.SetPosition (0, pos);

		pos.y = END_POS_Y;
		this.lineRenderer.SetPosition (1, pos);

		if (this.targetCamera != null)
		{
			pos.y = (END_POS_Y + START_POS_Y) / 2;
			pos.z = this.targetCamera.transform.position.z;
			this.targetCamera.transform.position = pos;
		}
	}
}
