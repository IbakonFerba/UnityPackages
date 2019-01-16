using System.Collections;
using UnityEngine;
using System;

namespace FK.Utility
{
    /// <summary>
    /// <para>Extension Methods for Vector and Quaternion</para>
    ///
    /// v2.1 01/2019
    /// Written by Fabian Kober
    /// fabian-kober@gmx.net
    /// </summary>
    public static class TransformStructsExtensions
    {
        #region VECTOR

        /// <summary>
        /// Lerps one Vector2 to another
        /// </summary>
        /// <param name="start">The start Vector</param>
        /// <param name="target">The target Vector</param>
        /// <param name="duration">Duration of the lerp in seconds</param>
        /// <param name="returnAction">Function to get the new Vector</param>
        /// <param name="finished">Callback for when the Lerping is finished</param>
        /// <param name="progressMapping">Function for mapping the progress, takes one float argument between 0 and 1 and should return a float between 0 and 1</param>
        /// <returns></returns>
        private static IEnumerator LerpVector(Vector2 start, Vector2 target, float duration, Action<Vector2> returnAction, Action finished, Func<float, float> progressMapping)
        {
            float progress = 0;
            while (progress < 1)
            {
                float mappedProgress = progressMapping?.Invoke(progress) ?? progress;
                returnAction(Vector2.LerpUnclamped(start, target, mappedProgress));
                yield return null;
                progress += Time.deltaTime / duration;
            }

            returnAction(target);
            finished?.Invoke();
        }

        /// <summary>
        /// Lerps one Vector3 to another
        /// </summary>
        /// <param name="start">The start Vector</param>
        /// <param name="target">The target Vector</param>
        /// <param name="duration">Duration of the lerp in seconds</param>
        /// <param name="returnAction">Function to get the new Vector</param>
        /// <param name="finished">Callback for when the Lerping is finished</param>
        /// <param name="progressMapping">Function for mapping the progress, takes one float argument between 0 and 1 and should return a float between 0 and 1</param>
        /// <returns></returns>
        private static IEnumerator LerpVector(Vector3 start, Vector3 target, float duration, Action<Vector3> returnAction, Action finished, Func<float, float> progressMapping)
        {
            float progress = 0;
            while (progress < 1)
            {
                float mappedProgress = progressMapping?.Invoke(progress) ?? progress;
                returnAction(Vector3.LerpUnclamped(start, target, mappedProgress));
                yield return null;
                progress += Time.deltaTime / duration;
            }

            returnAction(target);
            finished?.Invoke();
        }

        /// <summary>
        /// Lerps one Vector4 to another
        /// </summary>
        /// <param name="start">The start Vector</param>
        /// <param name="target">The target Vector</param>
        /// <param name="duration">Duration of the lerp in seconds</param>
        /// <param name="returnAction">Function to get the new Vector</param>
        /// <param name="finished">Callback for when the Lerping is finished</param>
        /// <param name="progressMapping">Function for mapping the progress, takes one float argument between 0 and 1 and should return a float between 0 and 1</param>
        /// <returns></returns>
        private static IEnumerator LerpVector(Vector4 start, Vector4 target, float duration, Action<Vector4> returnAction, Action finished, Func<float, float> progressMapping)
        {
            float progress = 0;
            while (progress < 1)
            {
                float mappedProgress = progressMapping?.Invoke(progress) ?? progress;
                returnAction(Vector4.LerpUnclamped(start, target, mappedProgress));
                yield return null;
                progress += Time.deltaTime / duration;
            }

            returnAction(target);
            finished?.Invoke();
        }

        /// <summary>
        /// Lerps one Vector2 to another
        /// </summary>
        /// <param name="vector">The Vector that should be lerped</param>
        /// <param name="target">The target Vector</param>
        /// <param name="duration">Duration of the lerp in seconds</param>
        /// <param name="returnAction">Function to get the new Vector</param>
        /// <param name="finished">Callback for when the Lerping is finished</param>
        /// <returns></returns>
        public static Coroutine Lerp(this Vector2 vector, Vector2 target, float duration, Action<Vector2> returnAction, Action finished = null)
        {
            return CoroutineHost.Instance.StartCoroutine(LerpVector(vector, target, duration, returnAction, finished, null));
        }

        /// <summary>
        /// Lerps one Vector2 to another
        /// </summary>
        /// <param name="vector">The Vector that should be lerped</param>
        /// <param name="target">The target Vector</param>
        /// <param name="duration">Duration of the lerp in seconds</param>
        /// <param name="returnAction">Function to get the new Vector</param>
        /// <param name="finished">Callback for when the Lerping is finished</param>
        /// <param name="progressMapping">Function for mapping the progress, takes one float argument between 0 and 1 and should return a float between 0 and 1</param>
        /// <returns></returns>
        public static Coroutine Lerp(this Vector2 vector, Vector2 target, float duration, Action<Vector2> returnAction, Func<float, float> progressMapping, Action finished = null)
        {
            return CoroutineHost.Instance.StartCoroutine(LerpVector(vector, target, duration, returnAction, finished, progressMapping));
        }

        /// <summary>
        /// Lerps one Vector3 to another
        /// </summary>
        /// <param name="vector">The Vector that should be lerped</param>
        /// <param name="target">The target Vector</param>
        /// <param name="duration">Duration of the lerp in seconds</param>
        /// <param name="returnAction">Function to get the new Vector</param>
        /// <param name="finished">Callback for when the Lerping is finished</param>
        /// <returns></returns>
        public static Coroutine Lerp(this Vector3 vector, Vector3 target, float duration, Action<Vector3> returnAction, Action finished = null)
        {
            return CoroutineHost.Instance.StartCoroutine(LerpVector(vector, target, duration, returnAction, finished, null));
        }

        /// <summary>
        /// Lerps one Vector2 to another
        /// </summary>
        /// <param name="vector">The Vector that should be lerped</param>
        /// <param name="target">The target Vector</param>
        /// <param name="duration">Duration of the lerp in seconds</param>
        /// <param name="returnAction">Function to get the new Vector</param>
        /// <param name="finished">Callback for when the Lerping is finished</param>
        /// <param name="progressMapping">Function for mapping the progress, takes one float argument between 0 and 1 and should return a float between 0 and 1</param>
        /// <returns></returns>
        public static Coroutine Lerp(this Vector3 vector, Vector3 target, float duration, Action<Vector3> returnAction, Func<float, float> progressMapping, Action finished = null)
        {
            return CoroutineHost.Instance.StartCoroutine(LerpVector(vector, target, duration, returnAction, finished, progressMapping));
        }

        /// <summary>
        /// Lerps one Vector4 to another
        /// </summary>
        /// <param name="vector">The Vector that should be lerped</param>
        /// <param name="target">The target Vector</param>
        /// <param name="duration">Duration of the lerp in seconds</param>
        /// <param name="returnAction">Function to get the new Vector</param>
        /// <param name="finished">Callback for when the Lerping is finished</param>
        /// <returns></returns>
        public static Coroutine Lerp(this Vector4 vector, Vector4 target, float duration, Action<Vector4> returnAction, Action finished = null)
        {
            return CoroutineHost.Instance.StartCoroutine(LerpVector(vector, target, duration, returnAction, finished, null));
        }

        /// <summary>
        /// Lerps one Vector4 to another
        /// </summary>
        /// <param name="vector">The Vector that should be lerped</param>
        /// <param name="target">The target Vector</param>
        /// <param name="duration">Duration of the lerp in seconds</param>
        /// <param name="returnAction">Function to get the new Vector</param>
        /// <param name="finished">Callback for when the Lerping is finished</param>
        /// <param name="progressMapping">Function for mapping the progress, takes one float argument between 0 and 1 and should return a float between 0 and 1</param>
        /// <returns></returns>
        public static Coroutine Lerp(this Vector4 vector, Vector4 target, float duration, Action<Vector4> returnAction, Func<float, float> progressMapping, Action finished = null)
        {
            return CoroutineHost.Instance.StartCoroutine(LerpVector(vector, target, duration, returnAction, finished, progressMapping));
        }

        #endregion
    }
}