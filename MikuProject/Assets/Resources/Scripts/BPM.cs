using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BPM : MonoBehaviour {

	public Sprite[] numSprites;

	private Image[] numImages;
	private int bpm = 0;

	private IRhythmCheck rhythmChecker;

	// Use this for initialization
	void Start () 
	{
		numImages = new Image[3];
		numImages [0] = GameObject.Find ("Num1").GetComponent<Image>();
		numImages [1] = GameObject.Find ("Num2").GetComponent<Image>();
		numImages [2] = GameObject.Find ("Num3").GetComponent<Image>();

		this.rhythmChecker = GameObject.Find ("RhythmCheck").GetComponent<IRhythmCheck> ();
	}
	
	// Update is called once per frame
	void Update () 
	{

		bpm = this.rhythmChecker.GetBPM ();
		numImages[2].sprite = numSprites[bpm % 10];
		numImages[1].sprite = numSprites [(bpm / 10) % 10];
		numImages[0].sprite = numSprites[bpm / 100];

	}
}
