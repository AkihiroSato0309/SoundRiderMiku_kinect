/***********************************************************************************************//**

@file BeatNotifier.cs
	
@author Keisuke Ohe
		
***************************************************************************************************/
		
using System.Collections.Generic;


/***********************************************************************************************//**

ビート通知者.
	
***************************************************************************************************/
public static class BeatNotifier
{
	private static LinkedList<IBeatObserver> observers = new LinkedList<IBeatObserver> ();


	public static void Attach(IBeatObserver observer)
	{
		observers.AddLast (observer);
	}

	public static void Detach(IBeatObserver observer)
	{
		observers.Remove (observer);
	}

	public static void Notify()
	{
		foreach (var observer in observers)
		{
			observer.Notice ();
		}
	}
}