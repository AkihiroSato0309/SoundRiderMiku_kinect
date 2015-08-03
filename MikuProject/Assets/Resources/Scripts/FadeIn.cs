using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeIn : MonoBehaviour {

	[SerializeField]
	private float fadeTime = 2.0f;
	private Image image;
	private float time = 0.0f;

	// 初期化処理
	void Start () 
	{
		image = GetComponent<Image> ();
	}
	
	// 更新処理
	void Update () 
	{
		time += Time.deltaTime;

		// フェード処理
		Color color = image.color;
		float a = 1.0f - time / fadeTime;
		color.a = a;
		image.color = color;
		print (color.a);
	}
}
