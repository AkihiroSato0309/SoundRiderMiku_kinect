using System;
using UnityEngine;

public class Timer
{
	public event Action Tick;
	public float Interval { get; set; }

	float t = 0;


	public Timer (float interval, Action func = null)
	{
		this.Interval = interval;
		this.Tick += func;
	}

	public void Update ()
	{
		this.t += Time.deltaTime;
		if (this.t > this.Interval)
		{
			if (this.Tick != null) this.Tick();
			t = 0;
		}
	}
}

