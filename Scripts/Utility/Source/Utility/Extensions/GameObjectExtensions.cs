using UnityEngine;
using System.Collections;
using FK.Utility.Binary;
using System;

namespace FK.Utility
{
    /// <summary>
    /// <para>Extension Methods for GameObject</para>
    /// 
    /// v1.0 06/2018
    /// Written by Fabian Kober
    /// fabian-kober@gmx.net
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
            foreach (Transform child in gameObject.transform)
            {
                child.gameObject.SetLayerRecursively(layer);
            }
        }
    }

    /// <summary>
    /// <para>Extension Methods for Transform and RectTransform</para>
    /// 
    /// v2.1 09/2018
    /// Written by Fabian Kober
    /// fabian-kober@gmx.net
    /// </summary>
    public static class TransformExtensions
    {
        #region INTERPOLATION

        /// <summary>
        /// A container for Position, rotation and scale with a bitmask that indicates which of these values to use
        /// </summary>
        private struct InterpolationTransform
        {
            public Vector3 position;
            public Quaternion rotation;
            public Vector3 scale;

            /// <summary>
            /// A Bitmask for which of the transform values to use. Counted from the lest significant bit, bit 0 is position, bit 1 is rotation and bit 2 is scale
            /// </summary>
            public byte valuesToUse;
        }


        /// <summary>
        /// Interpolates Position, Rotation and Scale of the given transform
        /// </summary>
        /// <param name="trans">The transform to manipulate</param>
        /// <param name="targetTrans">Container for target position, rotation and scale, containing info which of these values to use</param>
        /// <param name="duration">The amount of time in seconds the interpolation should take</param>
        /// <param name="space">Operate in world or local space</param>
        /// <param name="progressMapping">A delegate function to map a value between 0 and 1 used as the progress for lerping that returns a new value between 0 and 1</param>
        /// <param name="finished">Delegate function that is called when the interpolation is finished</param>
        /// <returns></returns>
        private static IEnumerator InterpolateTransform(Transform trans, InterpolationTransform targetTrans, float duration, Space space, Func<float, float> progressMapping, Action finished)
        {
            //Create neede variables
            Vector3 startPos = space == Space.Self ? trans.localPosition : trans.position;
            Quaternion startRot = space == Space.Self ? trans.localRotation : trans.rotation;
            ;
            Vector3 startScale = trans.localScale;
            ;

            // fins out which values we should interpolate
            bool usePos = targetTrans.valuesToUse.GetBitValue(0);
            bool useRot = targetTrans.valuesToUse.GetBitValue(1);
            bool useScale = targetTrans.valuesToUse.GetBitValue(2);


            float progress = 0;
            while (progress < 1)
            {
                // map progress
                float mappedProgress = progressMapping != null ? Mathf.Clamp01(progressMapping(progress)) : progress;

                // interpolate position if needed
                if (usePos)
                {
                    Vector3 lerpedPos = Vector3.Lerp(startPos, targetTrans.position, mappedProgress);

                    if (space == Space.Self)
                    {
                        trans.localPosition = lerpedPos;
                    }
                    else
                    {
                        trans.position = lerpedPos;
                    }
                }


                // interpolate rotation if needed
                if (useRot)
                {
                    Quaternion lerpedRot = Quaternion.Lerp(startRot, targetTrans.rotation, mappedProgress);

                    if (space == Space.Self)
                    {
                        trans.localRotation = lerpedRot;
                    }
                    else
                    {
                        trans.rotation = lerpedRot;
                    }
                }


                // interpolate scale if needed
                if (useScale)
                {
                    Vector3 lerpedScale = Vector3.Lerp(startScale, targetTrans.scale, mappedProgress);

                    trans.localScale = lerpedScale;
                }

                yield return null;
                progress += Time.deltaTime / duration;
            }

            // set final values
            if (usePos)
            {
                if (space == Space.Self)
                {
                    trans.localPosition = targetTrans.position;
                }
                else
                {
                    trans.position = targetTrans.position;
                }
            }

            if (useRot)
            {
                if (space == Space.Self)
                {
                    trans.localRotation = targetTrans.rotation;
                }
                else
                {
                    trans.rotation = targetTrans.rotation;
                }
            }

            if (useScale)
            {
                trans.localScale = targetTrans.scale;
            }

            // call delegate
            if (finished != null)
                finished();
        }

        /// <summary>
        /// Stops all Interpolations on the Transform
        /// </summary>
        /// <param name="transform"></param>
        public static void StopAllInterpolations(this Transform transform)
        {
            CoroutineHost.StopAllTrackedCoroutines(transform, "Interpolation");
        }

        /// <summary>
        /// Stops the provided Interpolation on the Transform
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="routine"></param>
        public static void StopInterpolation(this Transform transform, Coroutine routine)
        {
            CoroutineHost.StopTrackedCoroutine(routine, "Interpolation");
        }
        
        /// <summary>
        /// Interpolates Position, Rotation and Scale of the given transform to the provided Transform
        /// </summary>
        /// <param name="transform">The Transform to manipulate</param>
        /// <param name="targetTransform">The Transform to interpolate to</param>
        /// <param name="duration">The amount of time in seconds the interpolation should take</param>
        /// <param name="space">Operate in world or local space</param>
        /// <param name="progressMapping">A delegate function to map a value between 0 and 1 used as the progress for lerping that returns a new value between 0 and 1</param>
        /// <returns></returns>
        public static Coroutine Interpolate(this Transform transform, Transform targetTransform, float duration, Space space, Func<float, float> progressMapping = null)
        {
            InterpolationTransform target = new InterpolationTransform();
            if (space == Space.Self)
            {
                target.position = targetTransform.localPosition;
                target.rotation = targetTransform.localRotation;
            }
            else
            {
                target.position = targetTransform.position;
                target.rotation = targetTransform.rotation;
            }

            target.scale = targetTransform.localScale;

            target.valuesToUse = 7; // 00000111

            return CoroutineHost.StartTrackedCoroutine(InterpolateTransform(transform, target, duration, space, progressMapping, null), transform, "Interpolation");
        }

        /// <summary>
        /// Interpolates Position, Rotation and Scale of the given transform to the provided Transform
        /// </summary>
        /// <param name="transform">The Transform to manipulate</param>
        /// <param name="targetTransform">The Transform to interpolate to</param>
        /// <param name="duration">The amount of time in seconds the interpolation should take</param>
        /// <param name="space">Operate in world or local space</param>
        /// <param name="finished">Delegate function that is called when the interpolation is finished</param>
        /// <param name="progressMapping">A delegate function to map a value between 0 and 1 used as the progress for lerping that returns a new value between 0 and 1</param>
        /// <returns></returns>
        public static Coroutine Interpolate(this Transform transform, Transform targetTransform, float duration, Space space, Action finished, Func<float, float> progressMapping = null)
        {
            InterpolationTransform target = new InterpolationTransform();
            if (space == Space.Self)
            {
                target.position = targetTransform.localPosition;
                target.rotation = targetTransform.localRotation;
            }
            else
            {
                target.position = targetTransform.position;
                target.rotation = targetTransform.rotation;
            }

            target.scale = targetTransform.localScale;

            target.valuesToUse = 7; // 00000111

            return CoroutineHost.StartTrackedCoroutine(InterpolateTransform(transform, target, duration, space, progressMapping, finished), transform, "Interpolation");
        }

        /// <summary>
        /// Interpolates Position, Rotation and Scale of the given transform to the provided Values
        /// </summary>
        /// <param name="transform">The Transform to manipulate</param>
        /// <param name="position">Position to interpolate to</param>
        /// <param name="rotation">Rotation to interpolate to</param>
        /// <param name="scale">Scale to interpolate to</param>
        /// <param name="duration">The amount of time in seconds the interpolation should take</param>
        /// <param name="space">Operate in world or local space</param>
        /// <param name="progressMapping">A delegate function to map a value between 0 and 1 used as the progress for lerping that returns a new value between 0 and 1</param>
        /// <returns></returns>
        public static Coroutine Interpolate(this Transform transform, Vector3 position, Quaternion rotation, Vector3 scale, float duration, Space space, Func<float, float> progressMapping = null)
        {
            InterpolationTransform target = new InterpolationTransform();

            target.position = position;
            target.rotation = rotation;
            target.scale = scale;

            target.valuesToUse = 7; // 00000111

            return CoroutineHost.StartTrackedCoroutine(InterpolateTransform(transform, target, duration, space, progressMapping, null), transform, "Interpolation");
        }

        /// <summary>
        /// Interpolates Position, Rotation and Scale of the given transform to the provided Values
        /// </summary>
        /// <param name="transform">The Transform to manipulate</param>
        /// <param name="position">Position to interpolate to</param>
        /// <param name="rotation">Rotation to interpolate to</param>
        /// <param name="scale">Scale to interpolate to</param>
        /// <param name="duration">The amount of time in seconds the interpolation should take</param>
        /// <param name="space">Operate in world or local space</param>
        /// <param name="finished">Delegate function that is called when the interpolation is finished</param>
        /// <param name="progressMapping">A delegate function to map a value between 0 and 1 used as the progress for lerping that returns a new value between 0 and 1</param>
        /// <returns></returns>
        public static Coroutine Interpolate(this Transform transform, Vector3 position, Quaternion rotation, Vector3 scale, float duration, Space space, Action finished,
            Func<float, float> progressMapping = null)
        {
            InterpolationTransform target = new InterpolationTransform();

            target.position = position;
            target.rotation = rotation;
            target.scale = scale;

            target.valuesToUse = 7; // 00000111

            return CoroutineHost.StartTrackedCoroutine(InterpolateTransform(transform, target, duration, space, progressMapping, finished), transform, "Interpolation");
        }

        /// <summary>
        /// Interpolates Position and Rotation of the given transform to the provided Values
        /// </summary>
        /// <param name="transform">The Transform to manipulate</param>
        /// <param name="position">Position to interpolate to</param>
        /// <param name="rotation">Rotation to interpolate to</param>
        /// <param name="duration">The amount of time in seconds the interpolation should take</param>
        /// <param name="space">Operate in world or local space</param>
        /// <param name="progressMapping">A delegate function to map a value between 0 and 1 used as the progress for lerping that returns a new value between 0 and 1</param>
        /// <returns></returns>
        public static Coroutine Interpolate(this Transform transform, Vector3 position, Quaternion rotation, float duration, Space space, Func<float, float> progressMapping = null)
        {
            InterpolationTransform target = new InterpolationTransform();

            target.position = position;
            target.rotation = rotation;

            target.valuesToUse = 3; // 00000011

            return CoroutineHost.StartTrackedCoroutine(InterpolateTransform(transform, target, duration, space, progressMapping, null), transform, "Interpolation");
        }

        /// <summary>
        /// Interpolates Position and Rotation of the given transform to the provided Values
        /// </summary>
        /// <param name="transform">The Transform to manipulate</param>
        /// <param name="position">Position to interpolate to</param>
        /// <param name="rotation">Rotation to interpolate to</param>
        /// <param name="duration">The amount of time in seconds the interpolation should take</param>
        /// <param name="space">Operate in world or local space</param>
        /// <param name="finished">Delegate function that is called when the interpolation is finished</param>
        /// <param name="progressMapping">A delegate function to map a value between 0 and 1 used as the progress for lerping that returns a new value between 0 and 1</param>
        /// <returns></returns>
        public static Coroutine Interpolate(this Transform transform, Vector3 position, Quaternion rotation, float duration, Space space, Action finished, Func<float, float> progressMapping = null)
        {
            InterpolationTransform target = new InterpolationTransform();

            target.position = position;
            target.rotation = rotation;

            target.valuesToUse = 3; // 00000011

            return CoroutineHost.StartTrackedCoroutine(InterpolateTransform(transform, target, duration, space, progressMapping, finished), transform, "Interpolation");
        }

        /// <summary>
        /// Interpolates Position and Scale of the given transform to the provided Values
        /// </summary>
        /// <param name="transform">The Transform to manipulate</param>
        /// <param name="position">Position to interpolate to</param>
        /// <param name="scale">Scale to interpolate to</param>
        /// <param name="duration">The amount of time in seconds the interpolation should take</param>
        /// <param name="space">Operate in world or local space</param>
        /// <param name="progressMapping">A delegate function to map a value between 0 and 1 used as the progress for lerping that returns a new value between 0 and 1</param>
        /// <returns></returns>
        public static Coroutine Interpolate(this Transform transform, Vector3 position, Vector3 scale, float duration, Space space, Func<float, float> progressMapping = null)
        {
            InterpolationTransform target = new InterpolationTransform();

            target.position = position;
            target.scale = scale;

            target.valuesToUse = 5; // 00000101

            return CoroutineHost.StartTrackedCoroutine(InterpolateTransform(transform, target, duration, space, progressMapping, null), transform, "Interpolation");
        }

        /// <summary>
        /// Interpolates Position and Scale of the given transform to the provided Values
        /// </summary>
        /// <param name="transform">The Transform to manipulate</param>
        /// <param name="position">Position to interpolate to</param>
        /// <param name="scale">Scale to interpolate to</param>
        /// <param name="duration">The amount of time in seconds the interpolation should take</param>
        /// <param name="space">Operate in world or local space</param>
        /// <param name="finished">Delegate function that is called when the interpolation is finished</param>
        /// <param name="progressMapping">A delegate function to map a value between 0 and 1 used as the progress for lerping that returns a new value between 0 and 1</param>
        /// <returns></returns>
        public static Coroutine Interpolate(this Transform transform, Vector3 position, Vector3 scale, float duration, Space space, Action finished, Func<float, float> progressMapping = null)
        {
            InterpolationTransform target = new InterpolationTransform();

            target.position = position;
            target.scale = scale;

            target.valuesToUse = 5; // 00000101

            return CoroutineHost.StartTrackedCoroutine(InterpolateTransform(transform, target, duration, space, progressMapping, finished), transform, "Interpolation");
        }

        /// <summary>
        /// Interpolates Rotation and Scale of the given transform to the provided Values
        /// </summary>
        /// <param name="transform">The Transform to manipulate</param>
        /// <param name="rotation">Rotation to interpolate to</param>
        /// <param name="scale">Scale to interpolate to</param>
        /// <param name="duration">The amount of time in seconds the interpolation should take</param>
        /// <param name="space">Operate in world or local space</param>
        /// <param name="progressMapping">A delegate function to map a value between 0 and 1 used as the progress for lerping that returns a new value between 0 and 1</param>
        /// <returns></returns>
        public static Coroutine Interpolate(this Transform transform, Quaternion rotation, Vector3 scale, float duration, Space space, Func<float, float> progressMapping = null)
        {
            InterpolationTransform target = new InterpolationTransform();

            target.rotation = rotation;
            target.scale = scale;

            target.valuesToUse = 6; // 00000110

            return CoroutineHost.StartTrackedCoroutine(InterpolateTransform(transform, target, duration, space, progressMapping, null), transform, "Interpolation");
        }

        /// <summary>
        /// Interpolates Rotation and Scale of the given transform to the provided Values
        /// </summary>
        /// <param name="transform">The Transform to manipulate</param>
        /// <param name="rotation">Rotation to interpolate to</param>
        /// <param name="scale">Scale to interpolate to</param>
        /// <param name="duration">The amount of time in seconds the interpolation should take</param>
        /// <param name="space">Operate in world or local space</param>
        /// <param name="finished">Delegate function that is called when the interpolation is finished</param>
        /// <param name="progressMapping">A delegate function to map a value between 0 and 1 used as the progress for lerping that returns a new value between 0 and 1</param>
        /// <returns></returns>
        public static Coroutine Interpolate(this Transform transform, Quaternion rotation, Vector3 scale, float duration, Space space, Action finished, Func<float, float> progressMapping = null)
        {
            InterpolationTransform target = new InterpolationTransform();

            target.rotation = rotation;
            target.scale = scale;

            target.valuesToUse = 6; // 00000110

            return CoroutineHost.StartTrackedCoroutine(InterpolateTransform(transform, target, duration, space, progressMapping, finished), transform, "Interpolation");
        }

        /// <summary>
        /// Interpolates Positionof the given transform to the provided Position
        /// </summary>
        /// <param name="transform">The Transform to manipulate</param>
        /// <param name="position">Position to interpolate to</param>
        /// <param name="duration">The amount of time in seconds the interpolation should take</param>
        /// <param name="space">Operate in world or local space</param>
        /// <param name="progressMapping">A delegate function to map a value between 0 and 1 used as the progress for lerping that returns a new value between 0 and 1</param>
        /// <returns></returns>
        public static Coroutine Interpolate(this Transform transform, Vector3 position, float duration, Space space, Func<float, float> progressMapping = null)
        {
            InterpolationTransform target = new InterpolationTransform();

            target.position = position;

            target.valuesToUse = 1; // 00000001

            return CoroutineHost.StartTrackedCoroutine(InterpolateTransform(transform, target, duration, space, progressMapping, null), transform, "Interpolation");
        }

        /// <summary>
        /// Interpolates Positionof the given transform to the provided Position
        /// </summary>
        /// <param name="transform">The Transform to manipulate</param>
        /// <param name="position">Position to interpolate to</param>
        /// <param name="duration">The amount of time in seconds the interpolation should take</param>
        /// <param name="space">Operate in world or local space</param>
        /// <param name="finished">Delegate function that is called when the interpolation is finished</param>
        /// <param name="progressMapping">A delegate function to map a value between 0 and 1 used as the progress for lerping that returns a new value between 0 and 1</param>
        /// <returns></returns>
        public static Coroutine Interpolate(this Transform transform, Vector3 position, float duration, Space space, Action finished, Func<float, float> progressMapping = null)
        {
            InterpolationTransform target = new InterpolationTransform();

            target.position = position;

            target.valuesToUse = 1; // 00000001

            return CoroutineHost.StartTrackedCoroutine(InterpolateTransform(transform, target, duration, space, progressMapping, finished), transform, "Interpolation");
        }


        /// <summary>
        /// Interpolates Rotation of the given transform to the provided Rotation
        /// </summary>
        /// <param name="transform">The Transform to manipulate</param>
        /// <param name="rotation">Rotation to interpolate to</param>
        /// <param name="duration">The amount of time in seconds the interpolation should take</param>
        /// <param name="space">Operate in world or local space</param>
        /// <param name="progressMapping">A delegate function to map a value between 0 and 1 used as the progress for lerping that returns a new value between 0 and 1</param>
        /// <returns></returns>
        public static Coroutine Interpolate(this Transform transform, Quaternion rotation, float duration, Space space, Func<float, float> progressMapping = null)
        {
            InterpolationTransform target = new InterpolationTransform();

            target.rotation = rotation;

            target.valuesToUse = 2; // 00000010

            return CoroutineHost.StartTrackedCoroutine(InterpolateTransform(transform, target, duration, space, progressMapping, null), transform, "Interpolation");
        }

        /// <summary>
        /// Interpolates Rotation of the given transform to the provided Rotation
        /// </summary>
        /// <param name="transform">The Transform to manipulate</param>
        /// <param name="rotation">Rotation to interpolate to</param>
        /// <param name="duration">The amount of time in seconds the interpolation should take</param>
        /// <param name="space">Operate in world or local space</param>
        /// <param name="finished">Delegate function that is called when the interpolation is finished</param>
        /// <param name="progressMapping">A delegate function to map a value between 0 and 1 used as the progress for lerping that returns a new value between 0 and 1</param>
        /// <returns></returns>
        public static Coroutine Interpolate(this Transform transform, Quaternion rotation, float duration, Space space, Action finished, Func<float, float> progressMapping = null)
        {
            InterpolationTransform target = new InterpolationTransform();

            target.rotation = rotation;

            target.valuesToUse = 2; // 00000010

            return CoroutineHost.StartTrackedCoroutine(InterpolateTransform(transform, target, duration, space, progressMapping, finished), transform, "Interpolation");
        }

        /// <summary>
        /// Interpolates Positionof the given transform to the provided Position
        /// </summary>
        /// <param name="transform">The Transform to manipulate</param>
        /// <param name="scale">Scale to interpolate to</param>
        /// <param name="duration">The amount of time in seconds the interpolation should take</param>
        /// <param name="progressMapping">A delegate function to map a value between 0 and 1 used as the progress for lerping that returns a new value between 0 and 1</param>
        /// <returns></returns>
        public static Coroutine Interpolate(this Transform transform, Vector3 scale, float duration, Func<float, float> progressMapping = null)
        {
            InterpolationTransform target = new InterpolationTransform();

            target.scale = scale;

            target.valuesToUse = 4; // 00000100

            return CoroutineHost.StartTrackedCoroutine(InterpolateTransform(transform, target, duration, Space.Self, progressMapping, null), transform, "Interpolation");
        }

        /// <summary>
        /// Interpolates Positionof the given transform to the provided Position
        /// </summary>
        /// <param name="transform">The Transform to manipulate</param>
        /// <param name="scale">Scale to interpolate to</param>
        /// <param name="duration">The amount of time in seconds the interpolation should take</param>
        /// <param name="finished">Delegate function that is called when the interpolation is finished</param>
        /// <param name="progressMapping">A delegate function to map a value between 0 and 1 used as the progress for lerping that returns a new value between 0 and 1</param>
        /// <returns></returns>
        public static Coroutine Interpolate(this Transform transform, Vector3 scale, float duration, Action finished, Func<float, float> progressMapping = null)
        {
            InterpolationTransform target = new InterpolationTransform();

            target.scale = scale;

            target.valuesToUse = 4; // 00000100

            return CoroutineHost.StartTrackedCoroutine(InterpolateTransform(transform, target, duration, Space.Self, progressMapping, finished), transform, "Interpolation");
        }

        #endregion


        #region RECT_TRANSFORM

        /// <summary>
        /// Sets width and height of a Rect Transform
        /// </summary>
        /// <param name="rectTransform"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public static void SetAnchoredSize(this RectTransform rectTransform, float width, float height)
        {
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
        }

        /// <summary>
        /// Sets width and height of a Rect Transform
        /// </summary>
        /// <param name="rectTransform"></param>
        /// <param name="newSizeDelta"></param>
        public static void SetAnchoredSize(this RectTransform rectTransform, Vector2 newSizeDelta)
        {
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, newSizeDelta.x);
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, newSizeDelta.y);
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

            foreach (Transform child in transform)
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