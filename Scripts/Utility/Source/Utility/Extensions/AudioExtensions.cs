using System;
using System.Collections;
using UnityEngine;

namespace FK.Utility.Audio
{
    /// <summary>
    /// <para>Extension methods for everything Audio Related</para>
    ///
    /// v1.0 07/2018
    /// Written by Fabian Kober
    /// fabian-kober@gmx.net
    /// </summary>
    public static class AudioExtensions
    {
        #region VOLUME

        /// <summary>
        /// Fades the Volume of an Audio Source
        /// </summary>
        /// <param name="source">The Audio Source to fade</param>
        /// <param name="targetVolume">The volume to fade to</param>
        /// <param name="duration">Amount of time in seconds the Fade should take</param>
        /// <param name="progressMapping">A delegate function to map a value between 0 and 1 used as the progress for lerping that returns a new value between 0 and 1</param>
        /// <param name="finished">Delegate function that is called when the interpolation is finished</param>
        /// <returns></returns>
        private static IEnumerator FadeVolume(AudioSource source, float targetVolume, float duration, Func<float, float> progressMapping, Action finished)
        {
            float startVolume = source.volume;
            float clampedTargetVolume = Mathf.Clamp01(targetVolume);

            float progress = 0;
            while (progress < 1)
            {
                float mappedProgress = progressMapping?.Invoke(progress) ?? progress;
                source.volume = Mathf.Lerp(startVolume, clampedTargetVolume, mappedProgress);
                yield return null;
                progress += Time.deltaTime / duration;
            }

            source.volume = targetVolume;

            finished?.Invoke();
        }

        /// <summary>
        /// Fades the Volume of an Audio Source to a specified target volume
        /// </summary>
        /// <param name="source">The Audio Source to fade</param>
        /// <param name="host">MonoBehaviour to run the Coroutine on</param>
        /// <param name="targetVolume">The volume to fade to</param>
        /// <param name="duration">Amount of time in seconds the Fade should take</param>
        /// <param name="finished">Delegate function that is called when the interpolation is finished</param>
        /// <returns></returns>
        public static Coroutine FadeVolume(this AudioSource source, MonoBehaviour host, float targetVolume, float duration, Action finished = null)
        {
            return host.StartCoroutine(FadeVolume(source, targetVolume, duration, null, finished));
        }

        /// <summary>
        /// Fades the Volume of an Audio Source to a specified target volume
        /// </summary>
        /// <param name="source">The Audio Source to fade</param>
        /// <param name="host">MonoBehaviour to run the Coroutine on</param>
        /// <param name="targetVolume">The volume to fade to</param>
        /// <param name="duration">Amount of time in seconds the Fade should take</param>
        /// <param name="progressMapping">A delegate function to map a value between 0 and 1 used as the progress for lerping that returns a new value between 0 and 1</param>
        /// <param name="finished">Delegate function that is called when the interpolation is finished</param>
        /// <returns></returns>
        public static Coroutine FadeVolume(this AudioSource source, MonoBehaviour host, float targetVolume, float duration, Func<float, float> progressMapping, Action finished = null)
        {
            return host.StartCoroutine(FadeVolume(source, targetVolume, duration, progressMapping, finished));
        }

        /// <summary>
        /// Fades the Volume of an Audio Source completely in or out
        /// </summary>
        /// <param name="source">The Audio Source to fade</param>
        /// <param name="host">MonoBehaviour to run the Coroutine on</param>
        /// <param name="fadeIn">If true this fades the Volume to 1, else to 0</param>
        /// <param name="duration">Amount of time in seconds the Fade should take</param>
        /// <param name="finished">Delegate function that is called when the interpolation is finished</param>
        /// <returns></returns>
        public static Coroutine Fade(this AudioSource source, MonoBehaviour host, bool fadeIn, float duration, Action finished = null)
        {
            return host.StartCoroutine(FadeVolume(source, fadeIn ? 1 : 0, duration, null, finished));
        }

        /// <summary>
        /// Fades the Volume of an Audio Source completely in or out
        /// </summary>
        /// <param name="source">The Audio Source to fade</param>
        /// <param name="host">MonoBehaviour to run the Coroutine on</param>
        /// <param name="fadeIn">If true this fades the Volume to 1, else to 0</param>
        /// <param name="duration">Amount of time in seconds the Fade should take</param>
        /// <param name="progressMapping">A delegate function to map a value between 0 and 1 used as the progress for lerping that returns a new value between 0 and 1</param>
        /// <param name="finished">Delegate function that is called when the interpolation is finished</param>
        /// <returns></returns>
        public static Coroutine Fade(this AudioSource source, MonoBehaviour host, bool fadeIn, float duration, Func<float, float> progressMapping, Action finished = null)
        {
            return host.StartCoroutine(FadeVolume(source, fadeIn ? 1 : 0, duration, progressMapping, finished));
        }

        #endregion
    }
}