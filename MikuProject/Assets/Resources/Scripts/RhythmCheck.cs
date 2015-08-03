using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RhythmCheck : MonoBehaviour, IRhythmCheck{
	
	private KinectManager manager;

	// リズム計測に使用する変数
	private Queue<float> rhythmTimes;
	private float rhythmAvarage;
	private float currentBPM;
	private float lastRhythmTime;
	private float timer = 0;

	// 平均法による屈伸判定
	private Queue<float> checkObjectPosY;
	private float checkObjectAvaragePosY;
	private MyFlag underYFlag;

	// 前情報との差異を用いる屈伸判定
	private float lastCheckObjectPosY;


	public GameObject checkObject;
	public KinectInterop.JointType checkBodyType = KinectInterop.JointType.Head;		// どの位置でリズムをとるか

	public int rhythmCalcNum = 2;	// リズムの平均値を求める際に使用するタイムの個数
	public float initBPM = 135.0f;

	public int checkObjectPosYNum = 60;

	public float deffPosY = 0.003f;	// どれほどの下降幅をリズムとして検知するのか

	public float deadVal = 0.01f;
	

	// 開始処理
	void Start () {
		// キネクトマネージャーの取得
		manager = KinectManager.Instance;

		// リズムカウントの配列を初期化
		rhythmTimes = new Queue<float>(rhythmCalcNum);

		float initRhythm = 60.0f / initBPM;
		for (int i = 0; i < rhythmTimes.Count; i++) {
			rhythmTimes.Enqueue(initRhythm);
		}

		checkObjectPosY = new Queue<float>(checkObjectPosYNum);
		// チェックオブジェクトの高さ検出に関する初期化

		// 初期位置で配列を埋める
		long userId = manager.GetUserIdByIndex (0);
		Vector3 pos = manager.GetJointPosition (userId, (int)checkBodyType);
	}
	
	// 更新処理
	void Update () {
		// タイマーを増やす
		timer += Time.deltaTime;

		// リズムの検出
		if (manager && manager.IsUserDetected ()) {
			long userId = manager.GetUserIdByIndex (0);  // manager.GetPrimaryUserID();

			Vector3 pos = manager.GetJointPosition (userId, (int)checkBodyType);

			// 平均法によるリズム検出
			/*
			checkObjectPosY.Enqueue(pos.y);
			if(checkObjectPosY.Count > checkObjectPosYNum)checkObjectPosY.Dequeue();

			float sum = 0.0f;
			foreach(float posY in checkObjectPosY)
			{
				sum += posY;
			}
			checkObjectAvaragePosY = sum / checkObjectPosYNum;

			// 平均より下だった場合true
			bool underCheck = (checkObjectAvaragePosY > pos.y)?true:false;

			print (underYFlag.FlagChack(underCheck));
			*/

			// 差異を用いたリズム検出
			bool result;
			float def = lastCheckObjectPosY - pos.y;
			if(def > deffPosY)
			{
				result = underYFlag.FlagChack(true);
				if(result == true)
				{
					print(result);

					// 差異の算出
					float deffTime = timer - lastRhythmTime;
					lastRhythmTime = timer;

					// 差異タイムを記録
					rhythmTimes.Enqueue(deffTime);
					if(rhythmTimes.Count > rhythmCalcNum) rhythmTimes.Dequeue();

					// 平均時間を計算
					float timeSum = 0.0f;
					foreach(float time in rhythmTimes)
					{
						timeSum += time;
					}
					rhythmAvarage = timeSum / (float)rhythmTimes.Count;

					// BPMに変換
					currentBPM = 60.0f / rhythmAvarage;
				}

			}

			// 上昇の検知時にフラグを戻す
			if(def < -deffPosY)
			{
				underYFlag.FlagChack(false);
			}

			lastCheckObjectPosY = pos.y;

		}

	}

	void CalcRhythm()
	{

	}

	public int GetBPM()
	{
		return (int)currentBPM;
	}

	void OnGUI()
	{
		//GUI.Label (new Rect (0, 0, 100, 30), rhythmAvarage.ToString());
	}
}
