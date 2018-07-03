﻿using System.Collections;
using UnityEngine;
using System;

namespace FK.Utility
{
	/// <summary>
	/// <para>Color Extension Methods</para>
	///
	/// v1.0 06/2018
	/// Written by Fabian Kober
	/// fabian-kober@gmx.net
	/// </summary>
	public static class ColorExtensions
	{
		/// <summary>
		/// Lerps One Color to another one
		/// </summary>
		/// <param name="start">The start color</param>
		/// <param name="target">The target color</param>
		/// <param name="duration">Duration of the lerp in seconds</param>
		/// <param name="returnAction">Function to get the new color</param>
		/// <param name="finished">Callback for when the Lerping is finished</param>
		/// <param name="progressMapping">Function for mapping the progress, takes one float argument between 0 and 1 and should return a float between 0 and 1</param>
		/// <returns></returns>
		private static IEnumerator LerpColor(Color start, Color target, float duration, Action<Color> returnAction, Action finished, Func<float, float> progressMapping)
		{
			float progress = 0;
			while (progress < 1)
			{
				float mappedProgress = progressMapping != null ? Mathf.Clamp01(progressMapping(progress)) : progress;
				returnAction(Color.Lerp(start, target, mappedProgress));
				yield return null;
				progress += Time.deltaTime / duration;
			}

			returnAction(target);

			if (finished != null)
				finished();
		}

		/// <summary>
		/// Lerps the color to the target
		/// </summary>
		/// <param name="color">The start color</param>
		/// <param name="host">Monobehaviour to run the Coroutine on</param>
		/// <param name="target">The target color</param>
		/// <param name="duration">Duration of the lerp in seconds</param>
		/// <param name="returnAction">Function to get the new color</param>
		/// <param name="finished">Callback for when the Lerping is finished</param>
		/// <returns></returns>
		public static Coroutine Interpolate(this Color color, MonoBehaviour host, Color target, float duration, Action<Color> returnAction, Action finished = null)
		{
			return host.StartCoroutine(LerpColor(color, target, duration, returnAction, finished, null));
		}
		
		/// <summary>
		/// Lerps the color to the target
		/// </summary>
		/// <param name="color">The start color</param>
		/// <param name="host">Monobehaviour to run the Coroutine on</param>
		/// <param name="target">The target color</param>
		/// <param name="duration">Duration of the lerp in seconds</param>
		/// <param name="returnAction">Function to get the new color</param>
		/// <param name="progressMapping">Function for mapping the progress, takes one float argument between 0 and 1 and should return a float between 0 and 1</param>
		/// <param name="finished">Callback for when the Lerping is finished</param>
		/// <returns></returns>
		public static Coroutine Interpolate(this Color color, MonoBehaviour host, Color target, float duration, Action<Color> returnAction, Func<float, float> progressMapping, Action finished = null)
		{
			return host.StartCoroutine(LerpColor(color, target, duration, returnAction, finished, progressMapping));
		}
	}
}
