/***********************************************************************************************//**

@file .cs

@note	ファイル内でサブビート（Sub Beat）という造語を用いています.
		サブビートとは, ビートに合わせて, 等間隔に発生するタイミングのことを指します.
		サブビートの発生と同時に, SEが鳴り,  BeatNotifier.Notify() が呼ばれています.
		極端な話, 単純にビートのタイミングでSEを鳴らすとした場合, BPMが1の曲では1分に1回しかSEが鳴りません.
		このような事態を避けるために, サブビートという概念が生まれました.
		なお, 1ビートにおけるサブビートの数が1の場合, ビートとサブビートのタイミングは完全に一致します.
		元々BPMの高い曲では, 1ビートにおけるサブビートの数を1に設定すべきです.

@author Keisuke Ohe
		
***************************************************************************************************/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/***********************************************************************************************//**

サウンドマネージャクラス.
	
***************************************************************************************************/
[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
	// --------------- inspector ---------------
	[SerializeField]
	private int originalBPM = 135;		// 原曲のBPM (beat per minute).
	[SerializeField]
	private float subBeatPerBeat = 4;	// 1ビートにおけるサブビートの数.
	[SerializeField]	
	private float startTime = 1.815f;	// 曲の開始時間.
	[SerializeField]
	private float playSpeedMin = 0.5f;	// 再生スピードの最小値
	[SerializeField]
	private float playSpeedMax = 1.5f;	// 再生スピードの最大値

	// --------------- private ---------------
	private AudioSource audio;						// 親オブジェクトにアタッチされたAudioSource.
	private IRhythmCheck rhythmChecker;				// プレイヤーのリズムを取得するオブジェクト
	private float originalBPS;						// 原曲のBPS (beat per second).
	private double subBeatFreq;						// サブビートの頻度.
	private double nextSubBeatTime = 0;				// 次のサブビートのタイミング
	private float prevPlayingTime = 0;				// 音声ファイルの前フレームにおける再生時間（再生地点）.
	private float deltaTime = 0;					// 前フレームから今フレームまでにどれだけ再生時間が進んだか.
	private Dictionary<SE, AudioClip> seClips;		// サウンドの実データ.
	private Dictionary<SE, string> seClipPathes;	// サウンドファイルのパス.
	private Queue<SE> waitingSounds;				// 再生待機中のサウンド.
	
	// --------------- property ---------------
	public float Time { get { return this.audio.time; } }
	public float DeltaTime { get { return this.deltaTime; } }
	public float OriginalBPS { get { return this.originalBPS; } }
	public float SubBeatFreq { get { return (float)this.subBeatFreq; } }


	/************************************************************************************//**
	コンストラクタ.
	****************************************************************************************/
	public SoundManager()
	{
		// コンテナ生成.
		this.seClips = new Dictionary<SE, AudioClip> ();
		this.seClipPathes = new Dictionary<SE, string> ();
		this.waitingSounds = new Queue<SE> ();

		// enum SEとファイルパスを関連づける.
		const string folderPath = @"Sounds/SE/";
		this.seClipPathes.Add (SE.Test, folderPath + "SampleSE");
		this.seClipPathes.Add (SE.Jump, folderPath + "Jumping");
		this.seClipPathes.Add (SE.Landing, folderPath + "Landing");
		this.seClipPathes.Add (SE.Trick, folderPath + "Trick");
		this.seClipPathes.Add (SE.Combo, folderPath + "Combo");
		this.seClipPathes.Add (SE.Star, folderPath + "Star");

		// 全てのenum SEの要素と, ファイルパスが関連づいているか, 確認.
		SE[] elements = System.Enum.GetValues(typeof(SE)) as SE[];
		foreach (var e in elements)
		{
			if (!this.seClipPathes.ContainsKey (e))
			{
				Debug.LogError ("enum SEの一部の要素とパスが関連付けられていないようなので, SoundManager.cs のコンストラクタを確認して下さい.");
				this.seClipPathes.Add (e, folderPath + "SampleSE");
			}
		}
	}

	/************************************************************************************//**
	開始. 上位のオブジェクトであるため, StartではなくAwakeで処理.
		
	@return なし		
	****************************************************************************************/
	void Awake ()
	{
		// オブジェクトの取得.
		this.audio = GetComponent<AudioSource>();
		this.rhythmChecker = GameObject.Find ("RhythmCheck").GetComponent<IRhythmCheck> ();

		// 必要情報の計算.
		this.originalBPS = this.originalBPM / 60.0f;						// 1秒間のビート数 = 1分間のビート数 / 1分
		this.subBeatFreq = 1 / (this.originalBPS * this.subBeatPerBeat);	// タイミングの頻度（秒） = 1秒 / 1秒間におけるタイミングの数

		// ログ出力.
		Debug.Log ("BPM: " + originalBPM.ToString ());
		Debug.Log ("BPS: " + originalBPS.ToString ());
		Debug.Log ("Beat Freq: " + (subBeatFreq * subBeatPerBeat).ToString ());
		Debug.Log ("Sub Beat Freq: " + subBeatFreq.ToString ());
	}
	
	/************************************************************************************//**
	更新.
		
	@return なし		
	****************************************************************************************/
	void Update ()
	{
		// 再生が終わっていれば何もしない.
		if (!this.audio.isPlaying) return;

		// DeltaTimeを計算.
		this.deltaTime = this.audio.time - this.prevPlayingTime;

		// 再生待機中のSEを実際に鳴らす.
		this.PlaySEsAcutually ();

		// プレイヤーのテンポに合わせて再生速度を変更.
		this.ChangeBGMSpeedDependingOnPlayerTempo ();

		// 次フレームでDeltaTimeを計算するために, 現在の再生時間を保存.
		this.prevPlayingTime = this.audio.time;
	}

	/************************************************************************************//**
	デバッグ表示.
		
	@return なし		
	****************************************************************************************/
	void OnGUI()
	{
		GUI.Label (new Rect(0, 20, 200, 50), "Pitch: " + this.audio.pitch.ToString());
	}

	/************************************************************************************//**
	SE再生. 瞬時には再生されず, まずは再生待ちキューに格納され, BGMのビートに合わせてまとめて再生される.
		
	@param [in] se SEのID

	@return なし		
	****************************************************************************************/
	public void PlaySE (SE se)
	{
		LoadSEIfNecessary (se);
		waitingSounds.Enqueue (se);
	}

	/************************************************************************************//**
	SEの実データ読込. 既に読み込まれている場合は, 何もしない.
			
	@return なし		
	****************************************************************************************/
	private void LoadSEIfNecessary (SE se)
	{
		// 既にロードされているなら何もしない.
		if (this.seClips.ContainsKey (se)) return;

		// SEの実データを読み込む.
		var path = this.seClipPathes[se];
		var clip = Resources.Load (path) as AudioClip;
		this.seClips.Add (se, clip);
	}

	/************************************************************************************//**
	再生待ちSEの再生. 再生待ちキューに格納されたSEをまとめて再生する.
	
	@return なし		
	****************************************************************************************/
	private void PlaySEsAcutually ()
	{
		float nextTime = (float)this.nextSubBeatTime;
		float realTime = this.audio.time - this.startTime;

		// 現在の再生時間が, 次のSE再生タイミング時間を超えているなら
		if (realTime > nextTime)
		{
			// ビートオブジェクトに通知
			BeatNotifier.Notify ();

			// 再生待機中の全てのSEを鳴らす
			for (int i = 0; i < this.waitingSounds.Count; i++)
			{
				var index = waitingSounds.Dequeue ();
				this.audio.PlayOneShot (this.seClips[index]);
			}

			// 次のSE再生タイミング時間を算出
			this.nextSubBeatTime += SubBeatFreq;

			// ログ出力
			//Debug.Log ("Sub Beat! (" + realTime.ToString("F3") + " sec) " + "  誤差: " + (realTime - nextTime).ToString () + " sec");
		}
	}

	/************************************************************************************//**
	プレイヤーのテンポに合わせて再生速度を変更.
		
	@return なし		
	****************************************************************************************/
	private void ChangeBGMSpeedDependingOnPlayerTempo ()
	{
		float targetSpeed = this.rhythmChecker.GetBPM () / (float)this.originalBPM;
		targetSpeed = this.ValueToMultipleWithRound (targetSpeed, 0.1f);

		float newPitch = Mathf.Lerp (this.audio.pitch, targetSpeed, 0.01f);
		this.audio.pitch = Mathf.Clamp (newPitch, this.playSpeedMin, this.playSpeedMax); 
	}

	/************************************************************************************//**
	倍数化関数（丸めVer.）. 渡された数値を指定された基準値の倍数になるように丸める.

	@param [in]	value	対象の値
	@param [in] divisor	基準値
		
	@return なし		
	****************************************************************************************/
	private float ValueToMultipleWithRound (float value, float divisor)
	{
		float multiplier = value / divisor;
		multiplier = Mathf.Round (multiplier);
		return divisor * multiplier;
	}	
}
