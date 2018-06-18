using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

namespace FK.Utility.UI
{
    /// <summary>
    /// <para>Extension Methods for UI Elements</para>
    /// 
    /// v2.0 06/2018
    /// Written by Fabian Kober
    /// fabian-kober@gmx.net
    /// </summary>
    public static class UIExtensions
    {
        #region FADING
        /// <summary>
        /// Fades a Canvas Group in or out
        /// </summary>
        /// <param name="group">The Group to fade</param>
        /// <param name="FadeIn">Fade In?</param>
        /// <param name="duration">Amount of seconds the fading should take</param>
        /// <param name="finished">Callback for when the Fading is finished</param>
        /// <param name="progressMapping">Function for mapping the progress, takes one float argument between 0 and 1 and should return a float between 0 and 1</param>
        /// <returns></returns>
        private static IEnumerator FadeCanvasGroup(CanvasGroup group, bool FadeIn, float duration, Action finished, Func<float, float> progressMapping)
        {
            float progress = 0;
            while (progress < 1)
            {
                float mappedProgress = progressMapping != null ? Mathf.Clamp01(progressMapping(progress)) : progress;
                group.alpha = FadeIn ? mappedProgress : 1 - mappedProgress;
                yield return null;
                progress += Time.deltaTime / duration;
            }

            group.alpha = FadeIn ? 1 : 0;

            if (finished != null)
                finished();
        }

        /// <summary>
        /// Fades a Canvas Group in or out
        /// </summary>
        /// <param name="group">The Group to fade</param>
        /// <param name="host">The MonoBehavoiur to run the Coroutine on</param>
        /// <param name="FadeIn">Fade In?</param>
        /// <param name="duration">Amount of seconds the fading should take</param>
        /// <param name="finished">Callback for when the Fading is finished</param>
        public static Coroutine Fade(this CanvasGroup group, MonoBehaviour host, bool FadeIn, float duration, Action finished = null)
        {
            return host.StartCoroutine(FadeCanvasGroup(group, FadeIn, duration, finished, null));
        }

        /// <summary>
        /// Fades a Canvas Group in or out
        /// </summary>
        /// <param name="group">The Group to fade</param>
        /// <param name="host">The MonoBehavoiur to run the Coroutine on</param>
        /// <param name="FadeIn">Fade In?</param>
        /// <param name="duration">Amount of seconds the fading should take</param>
        /// <param name="progressMapping">Function for mapping the progress, takes one float argument between 0 and 1 and should return a float between 0 and 1</param>
        /// <param name="finished">Callback for when the Fading is finished</param>
        /// <returns></returns>
        public static Coroutine Fade(this CanvasGroup group, MonoBehaviour host, bool FadeIn, float duration, Func<float, float> progressMapping, Action finished = null)
        {
            return host.StartCoroutine(FadeCanvasGroup(group, FadeIn, duration, finished, progressMapping));
        }

        /// <summary>
        /// Fades a graphic in or out
        /// </summary>
        /// <param name="graphic">The graphic to fade</param>
        /// <param name="host">The MonoBehavoiur to run the Coroutine on</param>
        /// <param name="FadeIn">Fade In?</param>
        /// <param name="duration">Amount of seconds the fading should take</param>
        /// <param name="finished">Callback for when the Fading is finished</param>
        /// <returns></returns>
        public static Coroutine Fade(this Graphic graphic, MonoBehaviour host, bool FadeIn, float duration, Action finished = null)
        {
            Color start = graphic.color;
            start.a = FadeIn ? 0 : 1;
            Color target = graphic.color;
            target.a = FadeIn ? 1 : 0;

            return host.StartCoroutine(LerpColor(graphic, target, duration, finished, null));
        }

        /// <summary>
        /// Fades a graphic in or out
        /// </summary>
        /// <param name="graphic">The graphic to fade</param>
        /// <param name="host">The MonoBehavoiur to run the Coroutine on</param>
        /// <param name="FadeIn">Fade In?</param>
        /// <param name="duration">Amount of seconds the fading should take</param>
        /// <param name="progressMapping">Function for mapping the progress, takes one float argument between 0 and 1 and should return a float between 0 and 1</param>
        /// <param name="finished">Callback for when the Fading is finished</param>
        /// <returns></returns>
        public static Coroutine Fade(this Graphic graphic, MonoBehaviour host, bool FadeIn, float duration, Func<float, float> progressMapping, Action finished = null)
        {
            Color start = graphic.color;
            start.a = FadeIn ? 0 : 1;
            Color target = graphic.color;
            target.a = FadeIn ? 1 : 0;

            return host.StartCoroutine(LerpColor(graphic, target, duration, finished, progressMapping));
        }
        #endregion

        #region COLOR
        /// <summary>
        /// Lerps the Color of an Image
        /// </summary>
        /// <param name="graphic">The graphic to set the Color to</param>
        /// <param name="target">The Color to lerp to</param>
        /// <param name="duration">Amount of seconds the lerp should take</param>
        /// <param name="finished">Callback for when the Fading is finished</param>
        /// <param name="progressMapping">Function for mapping the progress, takes one float argument between 0 and 1 and should return a float between 0 and 1</param>
        /// <returns></returns>
        private static IEnumerator LerpColor(Graphic graphic, Color target, float duration, Action finished, Func<float, float> progressMapping)
        {
            Color start = graphic.color;

            float progress = 0;
            while (progress < 1)
            {
                float mappedProgress = progressMapping != null ? Mathf.Clamp01(progressMapping(progress)) : progress;
                graphic.color = Color.Lerp(start, target, progress);
                yield return null;
                progress += Time.deltaTime / duration;
            }

            graphic.color = target;

            if (finished != null)
                finished();
        }

        /// <summary>
        /// Lerps the Color of the Image in the given Duration
        /// </summary>
        /// <param name="graphic">The image to set the Color to</param>
        /// <param name="host">Mono Behaviour to run the Coroutine on</param>
        /// <param name="target">The Color to lerp to</param>
        /// <param name="duration">Amount of seconds the lerp should take</param>
        /// <param name="finished">Callback for when the Fading is finished</param>
        /// <returns></returns>
        public static Coroutine LerpColor(this Graphic graphic, MonoBehaviour host, Color target, float duration, Action finished = null)
        {
            return host.StartCoroutine(LerpColor(graphic, target, duration, finished, null));
        }

        /// <summary>
        /// Lerps the Color of the Image in the given Duration
        /// </summary>
        /// <param name="graphic">The image to set the Color to</param>
        /// <param name="host">Mono Behaviour to run the Coroutine on</param>
        /// <param name="target">The Color to lerp to</param>
        /// <param name="duration">Amount of seconds the lerp should take</param>
        /// <param name="progressMapping">Function for mapping the progress, takes one float argument between 0 and 1 and should return a float between 0 and 1</param>
        /// <param name="finished">Callback for when the Fading is finished</param>
        /// <returns></returns>
        public static Coroutine LerpColor(this Graphic graphic, MonoBehaviour host, Color target, float duration, Func<float, float> progressMapping, Action finished = null)
        {
            return host.StartCoroutine(LerpColor(graphic, target, duration, finished, progressMapping));
        }
        #endregion
    }
}
