using UnityEngine;
using System.Collections;
using System.IO;

[RequireComponent(typeof(AudioSource))]
public class StageDataGenerator : MonoBehaviour
{
	public const string FolderPath = @"Resources/Data/StageData/";	// 出力フォルダのパス
	public const string FileEX = @".csv";							// 出力ファイルの拡張子
	public const int VertexPerSec = 64;							// 1秒分の波形データから, いくつの頂点データを作成するか
	
	[SerializeField]
	private string outputFileName = "001";

	private AudioSource audio;			// 親オブジェクトにアタッチされたAudioSource
	private AudioClip clip;				// 音声ファイル
	private int samplingRate = 44100;	// 音声ファイルのサンプリングレート
	private float length = 0;			// 音声ファイルの長さ
	private float[] data;				// 音声ファイルのデータを格納するための配列
	private int time = 0;				// 現在処理している位置
	private int vertexToSample;			// 1頂点の作成に, いくつのサンプルを使用するか


	/************************************************************************************//**
	開始.
		
	@return なし		
	****************************************************************************************/
	void Start ()
	{
		this.audio = GetComponent<AudioSource>();
		this.clip = this.audio.clip;
		this.samplingRate = this.clip.frequency;
		this.length = this.clip.length;
		this.data = new float[this.samplingRate];
		this.vertexToSample = this.samplingRate / VertexPerSec;
		
		Debug.Log ("Sampling Rate is " + this.samplingRate.ToString() + "Hz");
		Debug.Log ("BGM Length is " + this.length.ToString("F2") + "秒");

		this.StartCoroutine (this.GenerateData ());
	}
	
	/************************************************************************************//**
	更新.
			
	@return なし		
	****************************************************************************************/
	IEnumerator GenerateData ()
	{
		yield return null;

		Debug.Log ("Start the Generation.");

		while (this.time < this.length)
		{
			// データの取得
			int offset = this.time * this.samplingRate;
			this.clip.GetData (this.data, offset);

			// データを加工して出力
			this.OutputVolumeDataAsStageData();

			// 処理位置を進める
			this.time += 1;

			// 1フレーム待つ
			yield return null;
		}

		Debug.Log ("Finish the Generation.");
	}

	/************************************************************************************//**
	ステージデータとして音声の波形をそのまま出力.
	波形データを飛び飛びに抽出しており, 多くの波形データは無視されている. 

	@note 移動平均法
		
	@return なし		
	****************************************************************************************/
	private void OutputWaveDataAsStageData()
	{
		// ファイルを開く.
		string filepath = Application.dataPath + @"/" + FolderPath + this.outputFileName + FileEX;
		var fi = new FileInfo(filepath);
		using (StreamWriter sw = fi.AppendText())
		{
			// 1秒分の音声データを解析して頂点データを作成し, 出力.
			for (int i = 0; i < VertexPerSec; ++i)
			{
				int index = i * vertexToSample;
				float posY = this.data[index];

				// 1頂点データをファイルに書き込む.
				sw.Write (posY.ToString ("F3") + ",");
			}
			sw.Write (sw.NewLine);	// 1行分のデータを出力したタイミングで改行を入れる.
		}
	}

	/************************************************************************************//**
	ステージデータとして「音量」を出力.
	音量は「特定の区間の波形の位相を2乗したものの平均」と考えて計算している.
	
	@return なし		
	****************************************************************************************/
	private void OutputVolumeDataAsStageData()
	{
		// ファイルを開く.
		string filepath = Application.dataPath + @"/" + FolderPath + this.outputFileName + FileEX;
		var fi = new FileInfo (filepath);
		using (StreamWriter sw = fi.AppendText())
		{
			// 1秒分の音声データを解析して頂点データを作成し, 出力.
			for (int i = 0; i < VertexPerSec; ++i)
			{
				float posY = 0;
				int topIndex = i * vertexToSample;

				// 各サンプルデータから1頂点のデータを作成.
				for (int j = topIndex; j < (topIndex + vertexToSample); j++)
				{
					posY += (this.data [j] * this.data [j]);
				}
				posY /= VertexPerSec;
				
				// 1頂点データをファイルに書き込む.
				sw.Write (posY.ToString ("F3") + ",");
			}
			sw.Write (sw.NewLine);	// 1行分のデータを出力したタイミングで改行を入れる.
		}
	}

	/************************************************************************************//**
	コンソールにテスト出力. 単なるメモなので気にしなくていい.

	@return なし	
	****************************************************************************************/
	private void TestOutputOnConsole()
	{
		int offset = (int)(this.audio.time * this.samplingRate);
		
		float[] tempData = new float[10];
		
		this.audio.GetOutputData (tempData, 0);
		Debug.Log (tempData[0].ToString() + ", " + tempData[1].ToString() + ", " + tempData[2].ToString() + ", " + 
		           tempData[3].ToString() + ", " + tempData[4].ToString() + ", " + tempData[5].ToString() + ", " + 
		           tempData[6].ToString() + ", " + tempData[7].ToString() + ", " + tempData[8].ToString() + ", " +
		           tempData[9].ToString() + ", ");
		
		this.clip.GetData (tempData, offset);
		Debug.Log (tempData[0].ToString() + ", " + tempData[1].ToString() + ", " + tempData[2].ToString() + ", " + 
		           tempData[3].ToString() + ", " + tempData[4].ToString() + ", " + tempData[5].ToString() + ", " + 
		           tempData[6].ToString() + ", " + tempData[7].ToString() + ", " + tempData[8].ToString() + ", " +
		           tempData[9].ToString() + ", ");
	}
}
