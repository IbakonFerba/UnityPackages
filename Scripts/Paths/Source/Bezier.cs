using UnityEngine;

namespace FK.Paths
{
    /// <summary>
    /// <para>Bezier Functions</para>
    /// <para>This class was created with the help of this tutorial: https://catlikecoding.com/unity/tutorials/curves-and-splines/</para>
    ///
    /// v1.0 08/2018
    /// Written by Fabian Kober
    /// fabian-kober@gmx.net
    /// </summary>
    public static class Bezier
    {
        // ######################## ENUMS & DELEGATES ######################## //
        /// <summary>
        /// The Modes for Bezier Tangents
        /// </summary>
        public enum BezierControlPointMode
        {
            /// <summary>
            /// No Constrains
            /// </summary>
            FREE,

            /// <summary>
            /// Alinged, but different lengths possible
            /// </summary>
            ALIGNED,

            /// <summary>
            /// Alinged and the same length
            /// </summary>
            MIRRORED
        }

        // ######################## FUNCTIONALITY ######################## //
        /// <summary>
        /// Get a Point along a quadratic Bezier curve
        /// </summary>
        /// <param name="p0">Start Point</param>
        /// <param name="p1">Control Point</param>
        /// <param name="p2">End Point</param>
        /// <param name="t">Position along the curve between 0 and 1</param>
        /// <returns></returns>
        public static Vector3 GetPoint(Vector3 p0, Vector3 p1, Vector3 p2, float t)
        {
            t = Mathf.Clamp01(t);
            float oneMinusT = 1f - t;

            return oneMinusT * oneMinusT * p0 +
                   2f * oneMinusT * t * p1 +
                   t * t * p2;
        }

        /// <summary>
        /// Get the first derivateive of a Point along a quadratic Bezier curve
        /// </summary>
        /// <param name="p0">Start Point</param>
        /// <param name="p1">Control Point</param>
        /// <param name="p2">End Point</param>
        /// <param name="t">Position along the curve between 0 and 1</param>
        /// <returns></returns>
        public static Vector3 GetFirstDerivative(Vector3 p0, Vector3 p1, Vector3 p2, float t)
        {
            return 2f * (1f - t) * (p1 - p0) +
                   2f * t * (p2 - p1);
        }


        /// <summary>
        /// Get a Point along a cubic Bezier curve
        /// </summary>
        /// <param name="p0">Start Point</param>
        /// <param name="p1">Control Point</param>
        /// <param name="p2">Control Point</param>
        /// <param name="p3">End Point</param>
        /// <param name="t">Position along the curve between 0 and 1</param>
        /// <returns></returns>
        public static Vector3 GetPoint(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
        {
            t = Mathf.Clamp01(t);
            float oneMinusT = 1f - t;
            return
                oneMinusT * oneMinusT * oneMinusT * p0 +
                3f * oneMinusT * oneMinusT * t * p1 +
                3f * oneMinusT * t * t * p2 +
                t * t * t * p3;
        }

        /// <summary>
        /// Get the first derivateive of a Point along a cubic Bezier curve
        /// </summary>
        /// <param name="p0">Start Point</param>
        /// <param name="p1">Control Point</param>
        /// <param name="p2">Control Point</param>
        /// <param name="p3">End Point</param>
        /// <param name="t">Position along the curve between 0 and 1</param>
        /// <returns></returns>
        public static Vector3 GetFirstDerivative(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
        {
            t = Mathf.Clamp01(t);
            float oneMinusT = 1f - t;
            return
                3f * oneMinusT * oneMinusT * (p1 - p0) +
                6f * oneMinusT * t * (p2 - p1) +
                3f * t * t * (p3 - p2);
        }
    }
}