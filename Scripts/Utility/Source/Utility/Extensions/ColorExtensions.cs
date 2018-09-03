using System.Collections;
using UnityEngine;
using System;
using FK.Utility.Fading;
using UnityEngine.UI;

namespace FK.Utility
{
    /// <summary>
    /// <para>Extension Methods Concerning Colors of different types</para>
    ///
    /// v3.0 09/2018
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
        /// <param name="target">The target color</param>
        /// <param name="duration">Duration of the lerp in seconds</param>
        /// <param name="returnAction">Function to get the new color</param>
        /// <param name="finished">Callback for when the Lerping is finished</param>
        /// <returns></returns>
        public static Coroutine Interpolate(this Color color, Color target, float duration, Action<Color> returnAction, Action finished = null)
        {
            return CoroutineHost.Instance.StartCoroutine(LerpColor(color, target, duration, returnAction, finished, null));
        }

        /// <summary>
        /// Lerps the color to the target
        /// </summary>
        /// <param name="color">The start color</param>
        /// <param name="target">The target color</param>
        /// <param name="duration">Duration of the lerp in seconds</param>
        /// <param name="returnAction">Function to get the new color</param>
        /// <param name="progressMapping">Function for mapping the progress, takes one float argument between 0 and 1 and should return a float between 0 and 1</param>
        /// <param name="finished">Callback for when the Lerping is finished</param>
        /// <returns></returns>
        public static Coroutine Interpolate(this Color color, Color target, float duration, Action<Color> returnAction, Func<float, float> progressMapping, Action finished = null)
        {
            return CoroutineHost.Instance.StartCoroutine(LerpColor(color, target, duration, returnAction, finished, progressMapping));
        }

        #region GRAPHIC

        /// <summary>
        /// Lerps the Color of the Graphic in the given Duration
        /// </summary>
        /// <param name="graphic">The Graphic to set the Color to</param>
        /// <param name="target">The Color to lerp to</param>
        /// <param name="duration">Amount of seconds the lerp should take</param>
        /// <param name="finished">Callback for when the Fading is finished</param>
        /// <returns></returns>
        public static Coroutine LerpColor(this Graphic graphic, Color target, float duration, Action finished = null)
        {
            return CoroutineHost.Instance.StartCoroutine(FadingMethods.LerpColor(graphic, target, duration, false, finished, null));
        }

        /// <summary>
        /// Lerps the Color of the Graphic in the given Duration
        /// </summary>
        /// <param name="graphic">The Graphic to set the Color to</param>
        /// <param name="target">The Color to lerp to</param>
        /// <param name="duration">Amount of seconds the lerp should take</param>
        /// <param name="progressMapping">Function for mapping the progress, takes one float argument between 0 and 1 and should return a float between 0 and 1</param>
        /// <param name="finished">Callback for when the Fading is finished</param>
        /// <returns></returns>
        public static Coroutine LerpColor(this Graphic graphic, Color target, float duration, Func<float, float> progressMapping, Action finished = null)
        {
            return CoroutineHost.Instance.StartCoroutine(FadingMethods.LerpColor(graphic, target, duration, false, finished, progressMapping));
        }

        #endregion

        #region SPRITE

        /// <summary>
        /// Lerps the Color of the Sprite in the given Duration
        /// </summary>
        /// <param name="sprite">The Sprite to set the Color to</param>
        /// <param name="target">The Color to lerp to</param>
        /// <param name="duration">Amount of seconds the lerp should take</param>
        /// <param name="finished">Callback for when the Fading is finished</param>
        /// <returns></returns>
        public static Coroutine LerpColor(this SpriteRenderer sprite, Color target, float duration, Action finished = null)
        {
            return CoroutineHost.Instance.StartCoroutine(FadingMethods.LerpColor(sprite, target, duration, false, finished, null));
        }

        /// <summary>
        /// Lerps the Color of the Sprite in the given Duration
        /// </summary>
        /// <param name="sprite">The Sprite to set the Color to</param>
        /// <param name="target">The Color to lerp to</param>
        /// <param name="duration">Amount of seconds the lerp should take</param>
        /// <param name="progressMapping">Function for mapping the progress, takes one float argument between 0 and 1 and should return a float between 0 and 1</param>
        /// <param name="finished">Callback for when the Fading is finished</param>
        /// <returns></returns>
        public static Coroutine LerpColor(this SpriteRenderer sprite, Color target, float duration, Func<float, float> progressMapping, Action finished = null)
        {
            return CoroutineHost.Instance.StartCoroutine(FadingMethods.LerpColor(sprite, target, duration, false, finished, progressMapping));
        }

        #endregion

        #region MATERIAL

        /// <summary>
        /// Lerps the Color of the Material in the given Duration
        /// </summary>
        /// <param name="material">The Material to set the Color to</param>
        /// <param name="target">The Color to lerp to</param>
        /// <param name="duration">Amount of seconds the lerp should take</param>
        /// <param name="finished">Callback for when the Fading is finished</param>
        /// <returns></returns>
        public static Coroutine LerpColor(this Material material, Color target, float duration, Action finished = null)
        {
            return CoroutineHost.Instance.StartCoroutine(FadingMethods.LerpColor(material, target, duration, finished, null));
        }

        /// <summary>
        /// Lerps the Color of the Material in the given Duration
        /// </summary>
        /// <param name="material">The Material to set the Color to</param>
        /// <param name="target">The Color to lerp to</param>
        /// <param name="duration">Amount of seconds the lerp should take</param>
        /// <param name="progressMapping">Function for mapping the progress, takes one float argument between 0 and 1 and should return a float between 0 and 1</param>
        /// <param name="finished">Callback for when the Fading is finished</param>
        /// <returns></returns>
        public static Coroutine LerpColor(this Material material, Color target, float duration, Func<float, float> progressMapping, Action finished = null)
        {
            return CoroutineHost.Instance.StartCoroutine(FadingMethods.LerpColor(material, target, duration, finished, progressMapping));
        }

        #endregion

        #region RENDERER

        /// <summary>
        /// Lerps the Color of the Renderer in the given Duration
        /// </summary>
        /// <param name="rend">The Renderer to set the Color to</param>
        /// <param name="target">The Color to lerp to</param>
        /// <param name="duration">Amount of seconds the lerp should take</param>
        /// <param name="finished">Callback for when the Fading is finished</param>
        /// <returns></returns>
        public static Coroutine LerpColor(this Renderer rend, Color target, float duration, Action finished = null)
        {
            Color[] targets = new Color[rend.materials.Length];
            for (int i = 0; i < targets.Length; ++i)
            {
                targets[i] = target;
            }

            return CoroutineHost.Instance.StartCoroutine(FadingMethods.LerpColor(rend, targets, duration, false, finished, null));
        }

        /// <summary>
        /// Lerps the Color of the Renderer in the given Duration
        /// </summary>
        /// <param name="rend">The Renderer to set the Color to</param>
        /// <param name="target">The Color to lerp to</param>
        /// <param name="duration">Amount of seconds the lerp should take</param>
        /// <param name="progressMapping">Function for mapping the progress, takes one float argument between 0 and 1 and should return a float between 0 and 1</param>
        /// <param name="finished">Callback for when the Fading is finished</param>
        /// <returns></returns>
        public static Coroutine LerpColor(this Renderer rend, Color target, float duration, Func<float, float> progressMapping, Action finished = null)
        {
            Color[] targets = new Color[rend.materials.Length];
            for (int i = 0; i < targets.Length; ++i)
            {
                targets[i] = target;
            }

            return CoroutineHost.Instance.StartCoroutine(FadingMethods.LerpColor(rend, targets, duration, false, finished, progressMapping));
        }

        #endregion
    }
}