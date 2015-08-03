/***********************************************************************************************//**

@file PlaySpeeds.cs
	
@author Keisuke Ohe
		
***************************************************************************************************/

using System;


/***********************************************************************************************//**

再生速度クラス. キリのいい再生速度の値を保持する静的クラス.
	
***************************************************************************************************/
public static class PlaySpeeds
{
	//! 要素数
	const int Num = 25;

	//! 再生速度
	public static readonly float[] speeds = new float[Num];


	/************************************************************************************//**
	コンストラクタ.
		
	@return なし		
	****************************************************************************************/
	static PlaySpeeds ()
	{
		for (int i = 0; i < Num; i++)
		{
			int pitch = i - (Num / 2);
			speeds[i] = (float)Math.Pow (2, ((double)pitch / 12));
		}
	}

	/************************************************************************************//**
	基準となる速度値に最も近いキリのいい速度値を返す.
		
	@param [in] speed 基準となる速度値.
			
	@return 基準となる速度値に最も近いキリのいい速度値
	****************************************************************************************/
	public static float GetNearSpeed(float speed)
	{
		int mid = Num / 2;
		int index = mid;
		float min = 100;

		if (speed > speeds[mid])
		{
			for (int i = mid; i < Num; i++)
			{
				GetMinValueAndIndex (speed, i, ref min, ref index);
			}
		}
		else
		{
			for (int i = 0; i <= mid; i++)
			{
				GetMinValueAndIndex (speed, i, ref min, ref index);
			}
		}

		return speeds[index];
	}

	/************************************************************************************//**
	GetNearSpeed関数のヘルパー.
		
	@param [in]		origin	GetNearSpeed関数に渡された引数.
	@param [in]		i		GetNearSpeed内のループ制御変数.
	@param [in,out]	min 	現状の誤差最小値.
	@param [out]	index	現状の誤差最小値を導き出せるインデックス.

	@return なし.
	****************************************************************************************/
	static void GetMinValueAndIndex(float origin, int i, ref float min, ref int index)
	{
		float sub = Math.Abs(origin - speeds[i]);
		if (sub < min)
		{
			min = sub;
			index = i;
		}
	}
}

