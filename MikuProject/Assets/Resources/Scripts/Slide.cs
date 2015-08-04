using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class Slide : BaseBeatBehaviour
{
	[SerializeField]
	[Multiline]private string[] texts;
	[SerializeField]
	float titleSlideStayTime_SubBeat = 64;
	[SerializeField]
	float normalSlideStayTime_SubBeat = 16;

	private Text text;
	private int currentNum = 0;
	private int subBeatCount;
	private bool canSlide;


	// 初期化処理
	void Start () 
	{
		base.Start ();

		text = GetComponent<Text> ();
		text.text = texts [currentNum];

		if (titleSlideStayTime_SubBeat < 1) titleSlideStayTime_SubBeat = 1;
		if (titleSlideStayTime_SubBeat < 1) titleSlideStayTime_SubBeat = 1;
	}
	
	// 更新処理
	void Update () 
	{
		// 暫定テストコード
		if (Input.GetKeyDown (KeyCode.N)) 
		{
			Next();
		}
	}

	public override void Notice ()
	{
		if (!canSlide) return;

		if (currentNum == 0)
		{
			if (++subBeatCount == titleSlideStayTime_SubBeat)
			{
				Next ();
				subBeatCount = 0;
			}
		}
		else
		{
			if (++subBeatCount == normalSlideStayTime_SubBeat)
			{
				Next ();
				subBeatCount = 0;
			}
		}
	}

	public void StartSlide ()
	{
		this.canSlide = true;
		this.gameObject.SetActive (true);
	}

	// スライドを進める
	public void Next()
	{
		currentNum++;

		// 配列外アクセスの防止
		if (currentNum == texts.Length) 
		{
			return;
		}

		text.text =  texts[currentNum];
	}
}
