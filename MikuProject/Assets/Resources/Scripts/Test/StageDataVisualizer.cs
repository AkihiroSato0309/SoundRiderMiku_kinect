using UnityEngine;
using System.Collections;
using System.IO;


public class StageDataVisualizer : MonoBehaviour
{
	[SerializeField]
	private string fileName;
	[SerializeField]
	private int sec;

	/************************************************************************************//**
	開始.
		
	@return なし		
	****************************************************************************************/
	void Start() 
	{
		this.TestRender ();
	}

	void TestRender ()
	{
		// 定数.
		const int VERTEX_PER_SEC = StageDataGenerator.VertexPerSec;		// 1秒間分の頂点データ数..
		const float LENGTH_PER_SEC = 3;										// 1秒間分の頂点群が専有するX軸距離
		const float LENGTH_PER_VERTEX = LENGTH_PER_SEC / VERTEX_PER_SEC;	// 頂点間のX軸距離

		// LineRendererの取得と初期化.
		var lineRenderer = this.GetComponent<LineRenderer> ();
		lineRenderer.enabled = true;
		lineRenderer.SetVertexCount (this.sec * VERTEX_PER_SEC);

		// ファイルを開く
		string filepath = Application.dataPath + @"/" + StageDataGenerator.FolderPath + this.fileName + StageDataGenerator.FileEX;
		using (StreamReader sr = new StreamReader (filepath, System.Text.Encoding.UTF8))
		{
			// 初期座標の決定
			Vector3 pos = this.transform.position;

			// 指定された秒数分だけ繰り返す
			for (int i = 0; i < this.sec; i++)
			{
				if (sr.EndOfStream) break;

				// 1秒間分のデータを読み込む（1行 = 1秒間分のデータ）
				string line = sr.ReadLine ();
				string[] data = line.Split (',');

				// 1秒間分の頂点データを処理する
				for (int j = 0; j < VERTEX_PER_SEC; j++)
				{
					pos.y = float.Parse (data [j]);							// 頂点データ（頂点のY座標を示す数字列）を取得してY座標を更新
					lineRenderer.SetPosition (i * VERTEX_PER_SEC + j, pos);	// LineRendererに頂点情報にセット
					pos.x += LENGTH_PER_VERTEX;								// 次の頂点用にX座標を更新
				}
			}
		}
	}
}
