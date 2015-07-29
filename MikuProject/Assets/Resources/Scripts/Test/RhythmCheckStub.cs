using UnityEngine;

public class RhythmCheckStub : MonoBehaviour, IRhythmCheck
{
	[SerializeField]
	private int playerBPM;

	public int GetBPM ()
	{
		return this.playerBPM;
	}
}


