using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Slide : MonoBehaviour {

	[SerializeField]
	[Multiline]private string[] texts;

	private Text text;
	private int currentNum = 0;

	// 初期化処理
	void Start () 
	{
		text = GetComponent<Text> ();
		text.text = texts [currentNum];
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

	// スライドを進める
	public void Next()
	{
		currentNum++;

		// 配列外アクセスの防止
		if (currentNum >= texts.Length) 
		{
			return;
		}

		text.text =  texts[currentNum];
	}
}
