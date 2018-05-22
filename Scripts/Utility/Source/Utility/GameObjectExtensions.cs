using UnityEngine;
using System.Collections;

namespace FK.Utility
{
    /// <summary>
    /// Callback for a finished Coroutine
    /// </summary>
    public delegate void CoroutineCallback();

    /// <summary>
    /// Extension Methods for GameObject
    /// </summary>
    public static class GameObjectExtensions
    {
        /// <summary>
        /// Sets the Layer of a GameObject and all of its Children
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="layer"></param>
        public static void SetLayerRecursively(this GameObject gameObject, int layer)
        {
            gameObject.layer = layer;
            foreach(Transform child in gameObject.transform)
            {
                child.gameObject.SetLayerRecursively(layer);
            }
        }
    }

    /// <summary>
    /// Extension Methods for Transform and RectTransform
    /// </summary>
    public static class TransformExtensions
    {
        #region INTERPOLATION
        public delegate float DelProgressMappingFunction(float progress);


        /// <summary>
        /// Interpolates Position, Rotation and Scale of the given transform
        /// </summary>
        /// <param name="trans">The transform to manipulate</param>
        /// <param name="targetPos">The Position to interpolate to</param>
        /// <param name="targetRot">The Rotation to interpolate to</param>
        /// <param name="targetScale">The Scale to interpolate to</param>
        /// <param name="duration">The amount of time in seconds the interpolation should take</param>
        /// <param name="space">Operate in world or local space</param>
        /// <param name="progressMapping">A delegate function to map a value between 0 and 1 used as the progress for lerping that returns a new value between 0 and 1</param>
        /// <param name="finished">Delegate function that is called when the interpolation is finished</param>
        /// <returns></returns>
        private static IEnumerator InterpolateTransform(Transform trans, Vector3 targetPos, Quaternion targetRot, Vector3 targetScale, float duration, Space space, DelProgressMappingFunction progressMapping, CoroutineCallback finished)
        {
            // set start values
            Vector3 startPos;
            Quaternion startRot;
            Vector3 startScale = trans.localScale;

            if (space == Space.Self)
            {
                startPos = trans.localPosition;
                startRot = trans.localRotation;
            } else
            {
                startPos = trans.position;
                startRot = trans.rotation;
            }


            float progress = 0;
            while (progress < 1)
            {
                float mappedProgress = progressMapping != null ? Mathf.Clamp01(progressMapping(progress)) : progress;

                // lerp position if start and target are different
                if(targetPos != startPos)
                {
                    Vector3 lerpedPos = Vector3.Lerp(startPos, targetPos, mappedProgress);

                    if (space == Space.Self)
                    {
                        trans.localPosition = lerpedPos;
                    }
                    else
                    {
                        trans.position = lerpedPos;
                    }
                }

                // lerp rotation if start and target are different
                if (targetRot != startRot)
                {
                    Quaternion lerpedRot = Quaternion.Lerp(startRot, targetRot, mappedProgress);

                    if (space == Space.Self)
                    {
                        trans.localRotation = lerpedRot;
                    }
                    else
                    {
                        trans.rotation = lerpedRot;
                    }
                }

                // lerp scale if start and target are different
                if (targetScale != startScale)
                {
                    Vector3 lerpedScale = Vector3.Lerp(startScale, targetScale, mappedProgress);

                    trans.localScale = lerpedScale;
                }

                yield return null;
                progress += Time.deltaTime / duration;
            }

            // set final values
            trans.localScale = targetScale;

            if (space == Space.Self)
            {
                trans.localPosition = targetPos;
                trans.localRotation = targetRot;
            }
            else
            {
                trans.position = targetPos;
                trans.rotation = targetRot;
            }

            if (finished != null)
                finished();
        }



        /// <summary>
        /// Interpolates Position, Rotation and Scale of the given transform to the provided Transform
        /// </summary>
        /// <param name="transform">The Transform to manipulate</param>
        /// <param name="host">MonoBehaviour to run the Coroutine on</param>
        /// <param name="target">The Transform to interpolate to</param>
        /// <param name="duration">The amount of time in seconds the interpolation should take</param>
        /// <param name="space">Operate in world or local space</param>
        /// <param name="progressMapping">A delegate function to map a value between 0 and 1 used as the progress for lerping that returns a new value between 0 and 1</param>
        /// <returns></returns>
        public static Coroutine Interpolate(this Transform transform, MonoBehaviour host, Transform target, float duration, Space space, DelProgressMappingFunction progressMapping = null)
        {
            return host.StartCoroutine(InterpolateTransform(transform, space == Space.Self ? target.localPosition : target.position, space == Space.Self ? target.localRotation : target.rotation, target.localScale, duration, space, progressMapping, null));
        }

        /// <summary>
        /// Interpolates Position, Rotation and Scale of the given transform to the provided Transform
        /// </summary>
        /// <param name="transform">The Transform to manipulate</param>
        /// <param name="host">MonoBehaviour to run the Coroutine on</param>
        /// <param name="target">The Transform to interpolate to</param>
        /// <param name="duration">The amount of time in seconds the interpolation should take</param>
        /// <param name="space">Operate in world or local space</param>
        /// <param name="finished">Delegate function that is called when the interpolation is finished</param>
        /// <param name="progressMapping">A delegate function to map a value between 0 and 1 used as the progress for lerping that returns a new value between 0 and 1</param>
        /// <returns></returns>
        public static Coroutine Interpolate(this Transform transform, MonoBehaviour host, Transform target, float duration, Space space, CoroutineCallback finished, DelProgressMappingFunction progressMapping = null)
        {
            return host.StartCoroutine(InterpolateTransform(transform, space == Space.Self ? target.localPosition : target.position, space == Space.Self ? target.localRotation : target.rotation, target.localScale, duration, space, progressMapping, finished));
        }

        /// <summary>
        /// Interpolates Position, Rotation and Scale of the given transform to the provided Values
        /// </summary>
        /// <param name="transform">The Transform to manipulate</param>
        /// <param name="host">MonoBehaviour to run the Coroutine on</param>
        /// <param name="position">Position to interpolate to</param>
        /// <param name="rotation">Rotation to interpolate to</param>
        /// <param name="scale">Scale to interpolate to</param>
        /// <param name="duration">The amount of time in seconds the interpolation should take</param>
        /// <param name="space">Operate in world or local space</param>
        /// <param name="progressMapping">A delegate function to map a value between 0 and 1 used as the progress for lerping that returns a new value between 0 and 1</param>
        /// <returns></returns>
        public static Coroutine Interpolate(this Transform transform, MonoBehaviour host, Vector3 position, Quaternion rotation, Vector3 scale, float duration, Space space, DelProgressMappingFunction progressMapping = null)
        {
            return host.StartCoroutine(InterpolateTransform(transform, position, rotation, scale, duration, space, progressMapping, null));
        }

        /// <summary>
        /// Interpolates Position, Rotation and Scale of the given transform to the provided Values
        /// </summary>
        /// <param name="transform">The Transform to manipulate</param>
        /// <param name="host">MonoBehaviour to run the Coroutine on</param>
        /// <param name="position">Position to interpolate to</param>
        /// <param name="rotation">Rotation to interpolate to</param>
        /// <param name="scale">Scale to interpolate to</param>
        /// <param name="duration">The amount of time in seconds the interpolation should take</param>
        /// <param name="space">Operate in world or local space</param>
        /// <param name="finished">Delegate function that is called when the interpolation is finished</param>
        /// <param name="progressMapping">A delegate function to map a value between 0 and 1 used as the progress for lerping that returns a new value between 0 and 1</param>
        /// <returns></returns>
        public static Coroutine Interpolate(this Transform transform, MonoBehaviour host, Vector3 position, Quaternion rotation, Vector3 scale, float duration, Space space, CoroutineCallback finished, DelProgressMappingFunction progressMapping = null)
        {
            return host.StartCoroutine(InterpolateTransform(transform, position, rotation, scale, duration, space, progressMapping, finished));
        }

        /// <summary>
        /// Interpolates Position and Rotation of the given transform to the provided Values
        /// </summary>
        /// <param name="transform">The Transform to manipulate</param>
        /// <param name="host">MonoBehaviour to run the Coroutine on</param>
        /// <param name="position">Position to interpolate to</param>
        /// <param name="rotation">Rotation to interpolate to</param>
        /// <param name="duration">The amount of time in seconds the interpolation should take</param>
        /// <param name="space">Operate in world or local space</param>
        /// <param name="progressMapping">A delegate function to map a value between 0 and 1 used as the progress for lerping that returns a new value between 0 and 1</param>
        /// <returns></returns>
        public static Coroutine Interpolate(this Transform transform, MonoBehaviour host, Vector3 position, Quaternion rotation, float duration, Space space, DelProgressMappingFunction progressMapping = null)
        {
            return host.StartCoroutine(InterpolateTransform(transform, position, rotation, transform.localScale, duration, space, progressMapping, null));
        }

        /// <summary>
        /// Interpolates Position and Rotation of the given transform to the provided Values
        /// </summary>
        /// <param name="transform">The Transform to manipulate</param>
        /// <param name="host">MonoBehaviour to run the Coroutine on</param>
        /// <param name="position">Position to interpolate to</param>
        /// <param name="rotation">Rotation to interpolate to</param>
        /// <param name="duration">The amount of time in seconds the interpolation should take</param>
        /// <param name="space">Operate in world or local space</param>
        /// <param name="finished">Delegate function that is called when the interpolation is finished</param>
        /// <param name="progressMapping">A delegate function to map a value between 0 and 1 used as the progress for lerping that returns a new value between 0 and 1</param>
        /// <returns></returns>
        public static Coroutine Interpolate(this Transform transform, MonoBehaviour host, Vector3 position, Quaternion rotation, float duration, Space space, CoroutineCallback finished, DelProgressMappingFunction progressMapping = null)
        {
            return host.StartCoroutine(InterpolateTransform(transform, position, rotation, transform.localScale, duration, space, progressMapping, finished));
        }

        /// <summary>
        /// Interpolates Position and Scale of the given transform to the provided Values
        /// </summary>
        /// <param name="transform">The Transform to manipulate</param>
        /// <param name="host">MonoBehaviour to run the Coroutine on</param>
        /// <param name="position">Position to interpolate to</param>
        /// <param name="scale">Scale to interpolate to</param>
        /// <param name="duration">The amount of time in seconds the interpolation should take</param>
        /// <param name="space">Operate in world or local space</param>
        /// <param name="progressMapping">A delegate function to map a value between 0 and 1 used as the progress for lerping that returns a new value between 0 and 1</param>
        /// <returns></returns>
        public static Coroutine Interpolate(this Transform transform, MonoBehaviour host, Vector3 position, Vector3 scale, float duration, Space space, DelProgressMappingFunction progressMapping = null)
        {
            return host.StartCoroutine(InterpolateTransform(transform, position, transform.rotation, scale, duration, space, progressMapping, null));
        }

        /// <summary>
        /// Interpolates Position and Scale of the given transform to the provided Values
        /// </summary>
        /// <param name="transform">The Transform to manipulate</param>
        /// <param name="host">MonoBehaviour to run the Coroutine on</param>
        /// <param name="position">Position to interpolate to</param>
        /// <param name="scale">Scale to interpolate to</param>
        /// <param name="duration">The amount of time in seconds the interpolation should take</param>
        /// <param name="space">Operate in world or local space</param>
        /// <param name="finished">Delegate function that is called when the interpolation is finished</param>
        /// <param name="progressMapping">A delegate function to map a value between 0 and 1 used as the progress for lerping that returns a new value between 0 and 1</param>
        /// <returns></returns>
        public static Coroutine Interpolate(this Transform transform, MonoBehaviour host, Vector3 position, Vector3 scale, float duration, Space space, CoroutineCallback finished, DelProgressMappingFunction progressMapping = null)
        {
            return host.StartCoroutine(InterpolateTransform(transform, position, transform.rotation, scale, duration, space, progressMapping, finished));
        }

        /// <summary>
        /// Interpolates Rotation and Scale of the given transform to the provided Values
        /// </summary>
        /// <param name="transform">The Transform to manipulate</param>
        /// <param name="host">MonoBehaviour to run the Coroutine on</param>
        /// <param name="rotation">Rotation to interpolate to</param>
        /// <param name="scale">Scale to interpolate to</param>
        /// <param name="duration">The amount of time in seconds the interpolation should take</param>
        /// <param name="space">Operate in world or local space</param>
        /// <param name="progressMapping">A delegate function to map a value between 0 and 1 used as the progress for lerping that returns a new value between 0 and 1</param>
        /// <returns></returns>
        public static Coroutine Interpolate(this Transform transform, MonoBehaviour host, Quaternion rotation, Vector3 scale, float duration, Space space, DelProgressMappingFunction progressMapping = null)
        {
            return host.StartCoroutine(InterpolateTransform(transform, transform.position, rotation, scale, duration, space, progressMapping, null));
        }

        /// <summary>
        /// Interpolates Rotation and Scale of the given transform to the provided Values
        /// </summary>
        /// <param name="transform">The Transform to manipulate</param>
        /// <param name="host">MonoBehaviour to run the Coroutine on</param>
        /// <param name="rotation">Rotation to interpolate to</param>
        /// <param name="scale">Scale to interpolate to</param>
        /// <param name="duration">The amount of time in seconds the interpolation should take</param>
        /// <param name="space">Operate in world or local space</param>
        /// <param name="finished">Delegate function that is called when the interpolation is finished</param>
        /// <param name="progressMapping">A delegate function to map a value between 0 and 1 used as the progress for lerping that returns a new value between 0 and 1</param>
        /// <returns></returns>
        public static Coroutine Interpolate(this Transform transform, MonoBehaviour host, Quaternion rotation, Vector3 scale, float duration, Space space, CoroutineCallback finished, DelProgressMappingFunction progressMapping = null)
        {
            return host.StartCoroutine(InterpolateTransform(transform, transform.position, rotation, scale, duration, space, progressMapping, finished));
        }

        /// <summary>
        /// Interpolates Positionof the given transform to the provided Position
        /// </summary>
        /// <param name="transform">The Transform to manipulate</param>
        /// <param name="host">MonoBehaviour to run the Coroutine on</param>
        /// <param name="position">Position to interpolate to</param>
        /// <param name="duration">The amount of time in seconds the interpolation should take</param>
        /// <param name="space">Operate in world or local space</param>
        /// <param name="progressMapping">A delegate function to map a value between 0 and 1 used as the progress for lerping that returns a new value between 0 and 1</param>
        /// <returns></returns>
        public static Coroutine Interpolate(this Transform transform, MonoBehaviour host, Vector3 position, float duration, Space space, DelProgressMappingFunction progressMapping = null)
        {
            return host.StartCoroutine(InterpolateTransform(transform, position, transform.rotation, transform.localScale, duration, space, progressMapping, null));
        }

        /// <summary>
        /// Interpolates Positionof the given transform to the provided Position
        /// </summary>
        /// <param name="transform">The Transform to manipulate</param>
        /// <param name="host">MonoBehaviour to run the Coroutine on</param>
        /// <param name="position">Position to interpolate to</param>
        /// <param name="duration">The amount of time in seconds the interpolation should take</param>
        /// <param name="space">Operate in world or local space</param>
        /// <param name="finished">Delegate function that is called when the interpolation is finished</param>
        /// <param name="progressMapping">A delegate function to map a value between 0 and 1 used as the progress for lerping that returns a new value between 0 and 1</param>
        /// <returns></returns>
        public static Coroutine Interpolate(this Transform transform, MonoBehaviour host, Vector3 position, float duration, Space space, CoroutineCallback finished, DelProgressMappingFunction progressMapping = null)
        {
            return host.StartCoroutine(InterpolateTransform(transform, position, transform.rotation, transform.localScale, duration, space, progressMapping, finished));
        }


        /// <summary>
        /// Interpolates Rotation of the given transform to the provided Rotation
        /// </summary>
        /// <param name="transform">The Transform to manipulate</param>
        /// <param name="host">MonoBehaviour to run the Coroutine on</param>
        /// <param name="rotation">Rotation to interpolate to</param>
        /// <param name="duration">The amount of time in seconds the interpolation should take</param>
        /// <param name="space">Operate in world or local space</param>
        /// <param name="progressMapping">A delegate function to map a value between 0 and 1 used as the progress for lerping that returns a new value between 0 and 1</param>
        /// <returns></returns>
        public static Coroutine Interpolate(this Transform transform, MonoBehaviour host, Quaternion rotation, float duration, Space space, DelProgressMappingFunction progressMapping = null)
        {
            return host.StartCoroutine(InterpolateTransform(transform, transform.position, rotation, transform.localScale, duration, space, progressMapping, null));
        }

        /// <summary>
        /// Interpolates Rotation of the given transform to the provided Rotation
        /// </summary>
        /// <param name="transform">The Transform to manipulate</param>
        /// <param name="host">MonoBehaviour to run the Coroutine on</param>
        /// <param name="rotation">Rotation to interpolate to</param>
        /// <param name="duration">The amount of time in seconds the interpolation should take</param>
        /// <param name="space">Operate in world or local space</param>
        /// <param name="finished">Delegate function that is called when the interpolation is finished</param>
        /// <param name="progressMapping">A delegate function to map a value between 0 and 1 used as the progress for lerping that returns a new value between 0 and 1</param>
        /// <returns></returns>
        public static Coroutine Interpolate(this Transform transform, MonoBehaviour host, Quaternion rotation, float duration, Space space, CoroutineCallback finished, DelProgressMappingFunction progressMapping = null)
        {
            return host.StartCoroutine(InterpolateTransform(transform, transform.position, rotation, transform.localScale, duration, space, progressMapping, finished));
        }

        /// <summary>
        /// Interpolates Positionof the given transform to the provided Position
        /// </summary>
        /// <param name="transform">The Transform to manipulate</param>
        /// <param name="host">MonoBehaviour to run the Coroutine on</param>
        /// <param name="scale">Scale to interpolate to</param>
        /// <param name="duration">The amount of time in seconds the interpolation should take</param>
        /// <param name="progressMapping">A delegate function to map a value between 0 and 1 used as the progress for lerping that returns a new value between 0 and 1</param>
        /// <returns></returns>
        public static Coroutine Interpolate(this Transform transform, MonoBehaviour host, Vector3 scale, float duration, DelProgressMappingFunction progressMapping = null)
        {
            return host.StartCoroutine(InterpolateTransform(transform, transform.position, transform.rotation, scale, duration, Space.Self, progressMapping, null));
        }

        /// <summary>
        /// Interpolates Positionof the given transform to the provided Position
        /// </summary>
        /// <param name="transform">The Transform to manipulate</param>
        /// <param name="host">MonoBehaviour to run the Coroutine on</param>
        /// <param name="scale">Scale to interpolate to</param>
        /// <param name="duration">The amount of time in seconds the interpolation should take</param>
        /// <param name="finished">Delegate function that is called when the interpolation is finished</param>
        /// <param name="progressMapping">A delegate function to map a value between 0 and 1 used as the progress for lerping that returns a new value between 0 and 1</param>
        /// <returns></returns>
        public static Coroutine Interpolate(this Transform transform, MonoBehaviour host, Vector3 scale, float duration, CoroutineCallback finished, DelProgressMappingFunction progressMapping = null)
        {
            return host.StartCoroutine(InterpolateTransform(transform, transform.position, transform.rotation, scale, duration, Space.Self, progressMapping, finished));
        }


        #endregion


        #region RECT_TRANSFORM
        /// <summary>
        /// Sets width and Height of a Rect Transform
        /// </summary>
        /// <param name="rectTransform"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public static void SetAnchoredSize(this RectTransform rectTransform, float width, float height)
        {
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
        }

        #endregion


        #region CHILDREN
        /// <summary>
        /// Finds a Child anywhere in the hierarchy below
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Transform FindDeepChild(this Transform transform, string name)
        {
            Transform result = transform.Find(name);

            if (result != null)
                return result;

            foreach(Transform child in transform)
            {
                result = child.FindDeepChild(name);

                if (result != null)
                    return result;
            }

            return null;
        }
        #endregion
    }
}
