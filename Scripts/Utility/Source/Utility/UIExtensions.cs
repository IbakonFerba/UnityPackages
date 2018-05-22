using UnityEngine;
using System.Collections;


namespace FK.Utility.UI
{
    /// <summary>
    /// Extension Methods for UI Elements
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
        /// <returns></returns>
        private static IEnumerator FadeCanvasGroup(CanvasGroup group, bool FadeIn, float duration, CoroutineCallback finished)
        {
            float progress = 0;

            while (progress < 1)
            {
                group.alpha = FadeIn ? progress : 1 - progress;
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
        public static void Fade(this CanvasGroup group, MonoBehaviour host, bool FadeIn, float duration, CoroutineCallback finished = null)
        {
            host.StartCoroutine(FadeCanvasGroup(group, FadeIn, duration, finished));
        }
        #endregion
    }
}
