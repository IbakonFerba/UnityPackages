using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace FK.Utility.UI
{
    /// <summary>
    /// Extension Methods for UI Elements
    /// 
    /// v1.2 06/2018
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
        public static Coroutine Fade(this CanvasGroup group, MonoBehaviour host, bool FadeIn, float duration, CoroutineCallback finished = null)
        {
            return host.StartCoroutine(FadeCanvasGroup(group, FadeIn, duration, finished));
        }

        /// <summary>
        /// Fades an Image in or out
        /// </summary>
        /// <param name="image">The image to Fade</param>
        /// <param name="host">The MonoBehavoiur to run the Coroutine on</param>
        /// <param name="FadeIn">Fade In?</param>
        /// <param name="duration">Amount of seconds the fading should take</param>
        /// <param name="finished">Callback for when the Fading is finished</param>
        /// <returns></returns>
        public static Coroutine Fade(this Image image, MonoBehaviour host, bool FadeIn, float duration, CoroutineCallback finished = null)
        {
            // get start and target color by taking the image color and setting its alpha value to 0 or 1
            Color start = image.color;
            start.a = FadeIn ? 0 : 1;
            Color target = image.color;
            target.a = FadeIn ? 1 : 0;

            // set start color and start fading by using a Color Lerp
            image.color = start;
            return host.StartCoroutine(LerpColor(image, target, duration, finished));
        }

        /// <summary>
        /// Fades a Text in or out
        /// </summary>
        /// <param name="text">The text to fade</param>
        /// <param name="host">The MonoBehavoiur to run the Coroutine on</param>
        /// <param name="FadeIn">Fade In?</param>
        /// <param name="duration">Amount of seconds the fading should take</param>
        /// <param name="finished">Callback for when the Fading is finished</param>
        /// <returns></returns>
        public static Coroutine Fade(this Text text, MonoBehaviour host, bool FadeIn, float duration, CoroutineCallback finished = null)
        {
            // get start and target color by taking the text color and setting its alpha value to 0 or 1
            Color start = text.color;
            start.a = FadeIn ? 0 : 1;
            Color target = text.color;
            target.a = FadeIn ? 1 : 0;

            // set start color and start fading by using a Color Lerp
            text.color = start;
            return host.StartCoroutine(LerpColor(text, target, duration, finished));
        }
        #endregion

        #region COLOR
        /// <summary>
        /// Lerps the Color of an Image
        /// </summary>
        /// <param name="image">The image to set the Color to</param>
        /// <param name="target">The Color to lerp to</param>
        /// <param name="duration">Amount of seconds the lerp should take</param>
        /// <param name="finished">Callback for when the Fading is finished</param>
        /// <returns></returns>
        private static IEnumerator LerpColor(Image image, Color target, float duration, CoroutineCallback finished)
        {
            Color start = image.color;

            float progress = 0;
            while (progress < 1)
            {
                image.color = Color.Lerp(start, target, progress);
                yield return null;
                progress += Time.deltaTime / duration;
            }

            image.color = target;

            if (finished != null)
                finished();
        }

        /// <summary>
        /// Lerps the Color of a Text
        /// </summary>
        /// <param name="text">The text to set the Color to</param>
        /// <param name="target">The Color to lerp to</param>
        /// <param name="duration">Amount of seconds the lerp should take</param>
        /// <param name="finished">Callback for when the Fading is finished</param>
        /// <returns></returns>
        private static IEnumerator LerpColor(Text text, Color target, float duration, CoroutineCallback finished)
        {
            Color start = text.color;

            float progress = 0;
            while (progress < 1)
            {
                text.color = Color.Lerp(start, target, progress);
                yield return null;
                progress += Time.deltaTime / duration;
            }

            text.color = target;

            if (finished != null)
                finished();
        }

        /// <summary>
        /// Lerps the Color of the Image in the given Duration
        /// </summary>
        /// <param name="image">The image to set the Color to</param>
        /// <param name="host">Mono Behaviour to run the Coroutine on</param>
        /// <param name="target">The Color to lerp to</param>
        /// <param name="duration">Amount of seconds the lerp should take</param>
        /// <param name="finished">Callback for when the Fading is finished</param>
        /// <returns></returns>
        public static Coroutine LerpColor(this Image image, MonoBehaviour host, Color target, float duration, CoroutineCallback finished = null)
        {
            return host.StartCoroutine(LerpColor(image, target, duration, finished));
        }

        /// <summary>
        /// Lerps the Color of a Text in the given Duration
        /// </summary>
        /// <param name="image">The text to set the Color to</param>
        /// <param name="host">Mono Behaviour to run the Coroutine on</param>
        /// <param name="target">The Color to lerp to</param>
        /// <param name="duration">Amount of seconds the lerp should take</param>
        /// <param name="finished">Callback for when the Fading is finished</param>
        /// <returns></returns>
        public static Coroutine LerpColor(this Text text, MonoBehaviour host, Color target, float duration, CoroutineCallback finished = null)
        {
            return host.StartCoroutine(LerpColor(text, target, duration, finished));
        }
        #endregion
    }
}
