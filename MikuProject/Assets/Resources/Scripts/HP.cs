using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HP : MonoBehaviour {

	public Sprite[] hpGages;

	private Sprite currentGage;
	private Image image;
	private int currentHP;

	// Use this for initialization
	void Start () 
	{
		image = GetComponent<Image> ();

		image.sprite = hpGages[hpGages.Length - 1];

		// HPの初期化
		currentHP = hpGages.Length;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKeyDown (KeyCode.Space)) 
		{
			UpDown(-1);
		}
	}

	void UpDown(int num)
	{
		currentHP += num;

		if(currentHP <= 0)
		{
			gameObject.SetActive(false);
		}
		else
			image.sprite = hpGages[currentHP - 1];
	}
}
