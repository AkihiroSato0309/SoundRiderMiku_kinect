/***********************************************************************************************//**

@file .cs

@note	ファイル内でサブビート（Sub Beat）という造語を用いています.
		サブビートとは, ビートに合わせて, 等間隔に発生するタイミングのことを指します.
		サブビートの発生と同時に, SEが鳴り,  BeatNotifier.Notify() が呼ばれています.
		極端な話, 単純にビートのタイミングでSEを鳴らすとした場合, BPMが1の曲では1分に1回しかSEが鳴りません.
		このような事態を避けるために, サブビートという概念が生まれました.
		なお, 1ビートにおけるサブビートの数が1の場合, ビートとサブビートのタイミングは完全に一致します.
		元々BPMの高い曲では, 1ビートにおけるサブビートの数を1に設定すべきです.

@note	ファイル内でフェイズ（Phase）という用語を用いています.
		フェイズは, ゲーム内におけるシステムの1つであり, 端的に言えば, ステージの進行段階を示しています.
		フェイズは, チェンジボックスを破壊されるか, 一定時間が経過することで次の段階へと進みます.

@author Keisuke Ohe
		
***************************************************************************************************/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/***********************************************************************************************//**

サウンドマネージャクラス.
	
***************************************************************************************************/
public class SoundManager : SingletonMonoBehaviour<SoundManager>
{
	// --------------- inspector ---------------
	[SerializeField]
	AudioClip[] bgmParts;		// BGMのパーツ.
	[SerializeField]
	AudioClip cushionSE;		// BGMの遷移時に挟む繋ぎ音.
	[SerializeField]
	AudioClip hidingLoopSE;		// ループ時の繋ぎ目を隠す音.
	[SerializeField]
	int originalBPM = 135;		// 原曲のBPM (beat per minute).
	[SerializeField]
	float beatPerBar = 4;		// 1小節におけるビートの数.
	[SerializeField]
	float subBeatPerBeat = 4;	// 1ビートにおけるサブビートの数.
	[SerializeField]
	float playSpeedMin = 0.5f;	// 再生スピードの最小値.
	[SerializeField]
	float playSpeedMax = 1.5f;	// 再生スピードの最大値.

	// --------------- public property ---------------
	public float Time { get { return this.elapsedTime; } }					// 現在のBGMの再生時間（再生地点）.
	public float DeltaTime { get { return this.deltaTime; } }				// 前フレームからどれだけ再生時間が進んだか.
	public float OriginalBPS { get { return this.originalBPS; } }			// 曲の元のBPS (beat per second).
	public float SubBeatFreq { get { return (float)this.subBeatFreq; } }	// サブビートの発生頻度（単位：秒）.
	public float IntroLength { get { return bgmParts[0].length; } }			// イントロの長さ

	// --------------- private property ---------------
	private bool IsLastPhase { get {return this.currentPhase == (bgmParts.Length - 1);} }

	// --------------- private ---------------
	AudioSource currentAudio;				// 現在使用しているAudioSource.
	AudioSource anotherAudio;				// 現在未使用のAudioSource.
	AudioSource seAudio;					// SEを鳴らすために使うAudioSource.
	IRhythmCheck rhythmChecker;				// プレイヤーのリズムを取得するオブジェクト.
	float originalBPS;						// 曲の元のBPS (beat per second).
	double subBeatFreq;						// サブビートの頻度.
	double barFreq;							// 小節の頻度.
	double nextSubBeatTime = 0;				// 次のサブビートのタイミング.
	double nextBarStartTime = 0;			// 次の小節開始のタイミング.
	double lastBarStartTime = 0;			// 最後の小節（ループに入る前の小節）の開始タイミング.
	float prevPlayingTime = 0;				// 音声ファイルの前フレームにおける再生時間（再生地点）.
	float deltaTime = 0;					// 前フレームから今フレームまでにどれだけ再生時間が進んだか.
	float elapsedTime = 0;					// 合計の経過時間.
	Dictionary<SE, AudioClip> seClips;		// サウンドの実データ.
	Dictionary<SE, string> seClipPathes;	// サウンドファイルのパス.
	Queue<SE> waitingSounds;				// 再生待機中のサウンド.
	int currentPhase = 0;					// 現在のフェイズの段階.
	bool hasBeenOnSubBeat = false;			// サブビートが発生したタイミングかどうか.
	bool hasStartedBar = false;				// バーが開始されたタイミングかどうか.
	bool canChangeBGMSpeed = false;			// BGMの速度の変化を有効化するかどうか.

	
	/************************************************************************************//**
	開始.
		
	@return なし		
	****************************************************************************************/
	void Awake ()
	{
		// コンテナ生成.
		this.seClips = new Dictionary<SE, AudioClip> ();
		this.seClipPathes = new Dictionary<SE, string> ();
		this.waitingSounds = new Queue<SE> ();

		// enum SEとファイルパスを関連づける. SEを追加した場合は, この関数を編集する.
		this.RegisterSEs ();

		// オブジェクトの生成と取得.
		this.CreateAudioSources ();
		this.rhythmChecker = GameObject.Find ("RhythmCheck").GetComponent<IRhythmCheck> ();

		// 必要情報の計算.
		this.playSpeedMax = PlaySpeeds.GetNearSpeed (this.playSpeedMax);
		this.playSpeedMin = PlaySpeeds.GetNearSpeed (this.playSpeedMin);
		this.originalBPS = this.originalBPM / 60.0f;								// 1秒間のビート数 = 1分間のビート数 / 1分.
		this.subBeatFreq = 1 / (this.originalBPS * this.subBeatPerBeat);			// サブビートの頻度（秒） = 1秒 / 1秒間におけるサブビートの数.
		this.barFreq = this.SubBeatFreq * this.subBeatPerBeat * this.beatPerBar;	// 小節の頻度.
		this.CalcLastBarStartTime ();												// 最後の小節の開始時間を算出.

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
		// DeltaTime, ElapsedTimeの計算
		this.CalcDeltaAndElapsed ();
		
		// サブビートに関する処理
		this.ProcessRegardingSubBeat ();
		
		// 小節に関する処理
		this.ProcessRegardingBar ();
		
		// プレイヤーのテンポに合わせて再生速度を変更.
		this.ChangeBGMSpeedDependingOnPlayerTempo ();
		
		// 次フレームでDeltaTimeを計算するために, 現在の再生時間を保存.
		this.prevPlayingTime = this.currentAudio.time;
	}

	/************************************************************************************//**
	デバッグ表示.
		
	@return なし		
	****************************************************************************************/
	void OnGUI()
	{
		GUI.Label (new Rect(0, 20, 200, 50), "Pitch: " + this.currentAudio.pitch.ToString());
	}
	
	/************************************************************************************//**
	SE再生. 瞬時には再生されず, まずは再生待ちキューに格納され, BGMのビートに合わせてまとめて再生される.
		
	@param [in] se SEのID

	@return なし		
	****************************************************************************************/
	public void PlaySE (SE se)
	{
		this.LoadSEIfNecessary (se);
		this.waitingSounds.Enqueue (se);
	}

	/************************************************************************************//**
	フェイズを次の段階へと進める.
	
	@return なし		
	****************************************************************************************/
	public void MoveToNextPhase ()
	{
		// 既に最後のフェイズであれば何もしない.
		if (this.IsLastPhase) return;

		this.StartCoroutine (this.ChangeBGMPart ());
	}

	/************************************************************************************//**
	enum SEとファイルを関連づける. SEを追加した場合は, この関数を編集する.
		
	@return なし		
	****************************************************************************************/
	void RegisterSEs ()
	{
		// ファイルパスと列挙値の関連付け
		const string folderPath = @"Sounds/SE/";
		this.seClipPathes.Add (SE.Test, folderPath + "SampleSE");
		this.seClipPathes.Add (SE.Foot, folderPath + "Foot");
		this.seClipPathes.Add (SE.HitEnemy, folderPath + "HitEnemy");
		this.seClipPathes.Add (SE.DestroyEnemy, folderPath + "DestroyEnemy");
		this.seClipPathes.Add (SE.HitWall, folderPath + "HitWall");
		
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
	Audio Sourceの作成と設定.
		
	@return なし		
	****************************************************************************************/
	void CreateAudioSources ()
	{
		// 作成.
		this.currentAudio = this.gameObject.AddComponent<AudioSource> ();
		this.anotherAudio = this.gameObject.AddComponent<AudioSource> ();
		this.seAudio = this.gameObject.AddComponent<AudioSource> ();

		// 設定.
		this.currentAudio.clip = bgmParts [this.currentPhase];
		this.currentAudio.loop = true;
		this.anotherAudio.loop = true;
		this.currentAudio.spatialBlend = 0;
		this.anotherAudio.spatialBlend = 0;
		this.seAudio.spatialBlend = 0;

		// BGMの再生.
		this.currentAudio.Play ();
	}

	/************************************************************************************//**
	SEの実データ読込. 既に読み込まれている場合は, 何もしない.
			
	@return なし		
	****************************************************************************************/
	void LoadSEIfNecessary (SE se)
	{
		// 既にロードされているなら何もしない.
		if (this.seClips.ContainsKey (se)) return;

		// SEの実データを読み込む.
		var path = this.seClipPathes[se];
		var clip = Resources.Load (path) as AudioClip;
		this.seClips.Add (se, clip);
	}

	/************************************************************************************//**
	DeltaTime, ElapsedTimeの計算.
		
	@return なし		
	****************************************************************************************/
	void CalcDeltaAndElapsed ()
	{
		// DeltaTimeを計算.
		if (this.currentAudio.time < this.prevPlayingTime)	// ループした瞬間であれば.
		{
			float t_1 = this.currentAudio.clip.length - this.prevPlayingTime;
			float t_2 = this.currentAudio.time;
			this.deltaTime = t_1 + t_2;

			// 最後の小節の位置を再計算
			this.CalcLastBarStartTime ();
		}
		else	// ループした瞬間でなければ.
		{
			this.deltaTime = this.currentAudio.time - this.prevPlayingTime;
		}

		// ElapsedTime（総経過時間）を計算.
		this.elapsedTime += this.deltaTime;
	}

	/************************************************************************************//**
	最後の小節の開始時間を算出. Start時, BGMのパートが切り替わった時, ループ終了時に呼ばないといけない.
		
	@return なし		
	****************************************************************************************/
	void CalcLastBarStartTime ()
	{
		int barNum = (int)(this.currentAudio.clip.length / barFreq);
		this.lastBarStartTime = elapsedTime + ((barNum - 1) * this.barFreq);
	}

	/************************************************************************************//**
	サブビートに関係する処理.
		
	@return なし		
	****************************************************************************************/
	void ProcessRegardingSubBeat ()
	{
		// フラグを更新
		this.hasBeenOnSubBeat = (this.elapsedTime >= (float)this.nextSubBeatTime);

		// サブビートのタイミングであれば
		if (this.hasBeenOnSubBeat)
		{
			BeatNotifier.Notify ();					// サブビートの発生を通知.
			this.PlaySEsAcutually ();				// 再生待機キューに溜まっていたSEを全て鳴らす.
			this.nextSubBeatTime += SubBeatFreq;	// 次のSE再生タイミングを算出.
			//Debug.Log ("Sub Beat! (" + elapsedTime.ToString("F3") + " sec) " + "  誤差: " + (elapsedTime - nextSubBeatTime).ToString () + " sec");	// ログ出力.
		}
	}

	/************************************************************************************//**
	小節に関係する処理.
	
	@return なし		
	****************************************************************************************/
	void ProcessRegardingBar ()
	{
		// フラグの更新
		this.hasStartedBar = (this.elapsedTime > (float)this.nextBarStartTime);

		// 小節開始のタイミングであれば
		if (this.hasStartedBar)
		{
			// 次の小節開始タイミングを算出
			this.nextBarStartTime += barFreq;

			// 最後の小節の開始タイミングであれば, 
			if (this.elapsedTime > this.lastBarStartTime)
			{
				this.seAudio.PlayOneShot (this.cushionSE);	// ループ時の途切れを隠すためにSEを鳴らす.
				this.lastBarStartTime += 100;				// lastBarStartTimeが再計算されることを見越して, とりあえず大きな数値を入れておく.

				if (this.currentPhase == 0)
				{
					this.MoveToNextPhase ();
					this.Invoke ("FinishIntro", (float)this.barFreq);
				}
			}
		}
	}

	/************************************************************************************//**
	再生待ちSEの再生. 再生待ちキューに格納されたSEをまとめて再生する.
	
	@return なし		
	****************************************************************************************/
	void PlaySEsAcutually ()
	{
		// 再生待機中の全てのSEを鳴らす
		for (int i = 0; i < this.waitingSounds.Count; i++)
		{
			var index = waitingSounds.Dequeue ();
			this.seAudio.PlayOneShot (this.seClips[index]);
		}
	}
	
	/************************************************************************************//**
	BGMのパートを変更.
		
	@return コルーチンの戻り値
	****************************************************************************************/
	IEnumerator ChangeBGMPart ()
	{
		// 小節の開始になるまで待機
		while (!this.hasStartedBar)
		{
			yield return null;
		}

		// クッション音を鳴らす
		this.seAudio.PlayOneShot (cushionSE);
		yield return null;

		// 小節の開始になるまで待機
		while (!this.hasStartedBar)
		{
			yield return null;
		}

		// 現在のAudioを変更
		this.SwapClass (this.currentAudio, this.anotherAudio);
		this.currentAudio.clip = bgmParts [++this.currentPhase];
		this.currentAudio.time = 0;
		this.currentAudio.pitch = anotherAudio.pitch;

		// BGMのパートを切り替え
		anotherAudio.Stop ();
		this.currentAudio.Play ();
		prevPlayingTime = 0;

		// 最後の小節の位置を再計算
		this.CalcLastBarStartTime ();

		// 最後のフェイズであれば, アウトロー開始処理
		if (this.IsLastPhase)
		{
			this.StartOutro ();
		}

		// 他のオブジェクトに次フェイズへの移行を通知
		BGObjManager.Inst.MoveToNextPhase (this.currentPhase);
	}

	/************************************************************************************//**
	プレイヤーのテンポに合わせて再生速度を変更.
		
	@return なし		
	****************************************************************************************/
	void ChangeBGMSpeedDependingOnPlayerTempo ()
	{
		if (!this.canChangeBGMSpeed) return;

		float targetSpeed = this.rhythmChecker.GetBPM () / (float)this.originalBPM;
		targetSpeed = PlaySpeeds.GetNearSpeed (targetSpeed);
		//targetSpeed = this.ValueToMultipleWithRound (targetSpeed, 0.1f);

		float newPitch = Mathf.Lerp (this.currentAudio.pitch, targetSpeed, 0.01f);
		this.currentAudio.pitch = Mathf.Clamp (newPitch, this.playSpeedMin, this.playSpeedMax); 
	}

	/************************************************************************************//**
	倍数化関数（丸めVer.）. 渡された数値を指定された基準値の倍数になるように丸める.

	@param [in]	value	対象の値
	@param [in] divisor	基準値
		
	@return なし		
	****************************************************************************************/
	float ValueToMultipleWithRound (float value, float divisor)
	{
		float multiplier = value / divisor;
		multiplier = Mathf.Round (multiplier);
		return divisor * multiplier;
	}

	/************************************************************************************//**
	スワップ.
		
	@param [in] a スワップ対象
	@param [in] b スワップ対象
	
	@return なし	
	****************************************************************************************/
	void SwapClass<T>(T a, T b)
	{
		T temp;
		temp = a;
		a = b;
		b = temp;
	}
	
	/************************************************************************************//**
	イントロ終了時.
		
	@return なし		
	****************************************************************************************/
	void FinishIntro ()
	{
		this.canChangeBGMSpeed = true;
	}

	/************************************************************************************//**
	アウトロ開始時.
		
	@return なし		
	****************************************************************************************/
	void StartOutro ()
	{
		this.canChangeBGMSpeed = false;
		currentAudio.pitch = 1.0f;
		currentAudio.pitch = 1.0f;
	}
}
