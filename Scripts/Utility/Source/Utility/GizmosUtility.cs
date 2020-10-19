using UnityEngine;

namespace FK
{
    namespace Utility
    {
        /// <summary>
        /// <para>Utility functions for Gizmos</para>
        ///
        /// v1.0 10/2020
        /// Written by Fabian Kober
        /// fabian-kober@gmx.net
        /// </summary>
        public class GizmosUtility
        {
            /// <summary>
            /// Draws a rect around a center point, defined by a normal vector and dimensions
            /// </summary>
            /// <param name="center"></param>
            /// <param name="normal"></param>
            /// <param name="dimensions"></param>
            /// <param name="color"></param>
            public static void DrawRect(Vector3 center, Vector3 normal, Vector2 dimensions, Color color)
            {
                Vector3 vert1 = new Vector3(-dimensions.x * 0.5f, 0.0f, -dimensions.y * 0.5f);
                Vector3 vert2 = new Vector3(-dimensions.x * 0.5f, 0.0f, dimensions.y * 0.5f);
                Vector3 vert3 = new Vector3(dimensions.x * 0.5f, 0.0f, dimensions.y * 0.5f);
                Vector3 vert4 = new Vector3(dimensions.x * 0.5f, 0.0f, -dimensions.y * 0.5f);

                Matrix4x4 restoreMatrix = Gizmos.matrix;
                Gizmos.matrix = Gizmos.matrix * Matrix4x4.TRS(center, Quaternion.FromToRotation(Vector3.up, normal), Vector3.one);
                Gizmos.color = color;
                Gizmos.DrawLine(vert1, vert2);
                Gizmos.DrawLine(vert2, vert3);
                Gizmos.DrawLine(vert3, vert4);
                Gizmos.DrawLine(vert4, vert1);
                Gizmos.matrix = restoreMatrix;
            }

            /// <summary>
            /// Draws a rect with the provided vertices
            /// </summary>
            /// <param name="vert1"></param>
            /// <param name="vert2"></param>
            /// <param name="vert3"></param>
            /// <param name="vert4"></param>
            /// <param name="color"></param>
            public static void DrawRect(Vector3 vert1, Vector3 vert2, Vector3 vert3, Vector3 vert4, Color color)
            {
                Gizmos.color = color;
                Gizmos.DrawLine(vert1, vert2);
                Gizmos.DrawLine(vert2, vert3);
                Gizmos.DrawLine(vert3, vert4);
                Gizmos.DrawLine(vert4, vert1);
            }

            /// <summary>
            /// Draws an arch around the provided center point with the provided normal starting at startAngle, covering spanAngle
            /// </summary>
            /// <param name="center"></param>
            /// <param name="normal"></param>
            /// <param name="radius"></param>
            /// <param name="startAngle">Start angle in DEGREE</param>
            /// <param name="spanAngle">Span Angle in DEGREE</param>
            /// <param name="subdivisions"></param>
            /// <param name="color"></param>
            public static void DrawArch(Vector3 center, Vector3 normal, float radius, float startAngle, float spanAngle, int subdivisions, Color color)
            {
                float radStartAngle = Mathf.Deg2Rad * startAngle;
                float radSpanAngle = Mathf.Deg2Rad * spanAngle;
                float angleStep = radSpanAngle / subdivisions;

                Matrix4x4 restoreMatrix = Gizmos.matrix;
                Gizmos.matrix = Gizmos.matrix * Matrix4x4.TRS(center, Quaternion.FromToRotation(Vector3.up, normal), Vector3.one);
                Gizmos.color = color;

                Vector3 nextVert = new Vector3(radius * Mathf.Cos(radStartAngle), 0.0f, radius * Mathf.Sin(radStartAngle));
                for (int i = 0; i < subdivisions; ++i)
                {
                    Vector3 currentVert = nextVert;
                    float nextAngle = radStartAngle + angleStep * (i + 1);
                    nextVert.x = radius * Mathf.Cos(nextAngle);
                    nextVert.z = radius * Mathf.Sin(nextAngle);
                    Gizmos.DrawLine(currentVert, nextVert);
                }

                Gizmos.matrix = restoreMatrix;
            }

            /// <summary>
            /// Draws a rounded box
            /// </summary>
            /// <param name="center"></param>
            /// <param name="dimensions"></param>
            /// <param name="cornerRadius"></param>
            /// <param name="color"></param>
            public static void DrawRoundedBox(Vector3 center, Vector3 dimensions, float cornerRadius, Color color)
            {
                Vector3 halfDimensions = dimensions * 0.5f;
                float doubleCornerRadius = cornerRadius * 2.0f;
                Vector3 innerDimensions = new Vector3(dimensions.x - doubleCornerRadius, dimensions.y - doubleCornerRadius, dimensions.z - doubleCornerRadius);
                Vector3 halfInnerDimensions = innerDimensions * 0.5f;

                DrawRect(Vector3.right * halfDimensions.x, Vector3.right, new Vector2(innerDimensions.y, innerDimensions.z), color);
                DrawRect(Vector3.up * halfDimensions.y, Vector3.up, new Vector2(innerDimensions.x, innerDimensions.z), color);
                DrawRect(Vector3.forward * halfDimensions.z, Vector3.forward, new Vector2(innerDimensions.x, innerDimensions.y), color);
                DrawRect(-Vector3.right * halfDimensions.x, -Vector3.right, new Vector2(innerDimensions.y, innerDimensions.z), color);
                DrawRect(-Vector3.up * halfDimensions.y, -Vector3.up, new Vector2(innerDimensions.x, innerDimensions.z), color);
                DrawRect(-Vector3.forward * halfDimensions.z, -Vector3.forward, new Vector2(innerDimensions.x, innerDimensions.y), color);

                {
                    DrawArch(halfInnerDimensions, Vector3.forward, cornerRadius, 0.0f, -90.0f, 10, color);
                    DrawArch(halfInnerDimensions, Vector3.up, cornerRadius, 0.0f, 90.0f, 10, color);
                    DrawArch(halfInnerDimensions, Vector3.right, cornerRadius, 90.0f, 90.0f, 10, color);
                }
                {
                    Vector3 archCenter = new Vector3(halfInnerDimensions.x, halfInnerDimensions.y, -halfInnerDimensions.z);
                    DrawArch(archCenter, Vector3.forward, cornerRadius, 0.0f, -90.0f, 10, color);
                    DrawArch(archCenter, Vector3.up, cornerRadius, 0.0f, -90.0f, 10, color);
                    DrawArch(archCenter, Vector3.right, cornerRadius, -90.0f, -90.0f, 10, color);
                }
                {
                    DrawArch(-halfInnerDimensions, Vector3.forward, cornerRadius, 90.0f, 90.0f, 10, color);
                    DrawArch(-halfInnerDimensions, Vector3.up, cornerRadius, -90.0f, -90.0f, 10, color);
                    DrawArch(-halfInnerDimensions, Vector3.right, cornerRadius, 0.0f, -90.0f, 10, color);
                }
                {
                    Vector3 archCenter = new Vector3(-halfInnerDimensions.x, -halfInnerDimensions.y, halfInnerDimensions.z);
                    DrawArch(archCenter, Vector3.forward, cornerRadius, 90.0f, 90.0f, 10, color);
                    DrawArch(archCenter, Vector3.up, cornerRadius, 90.0f, 90.0f, 10, color);
                    DrawArch(archCenter, Vector3.right, cornerRadius, 0.0f, 90.0f, 10, color);
                }
                {
                    Vector3 archCenter = new Vector3(-halfInnerDimensions.x, halfInnerDimensions.y, halfInnerDimensions.z);
                    DrawArch(archCenter, Vector3.forward, cornerRadius, -90.0f, -90.0f, 10, color);
                    DrawArch(archCenter, Vector3.up, cornerRadius, 90.0f, 90.0f, 10, color);
                    DrawArch(archCenter, Vector3.right, cornerRadius, 90.0f, 90.0f, 10, color);
                }
                {
                    Vector3 archCenter = new Vector3(-halfInnerDimensions.x, halfInnerDimensions.y, -halfInnerDimensions.z);
                    DrawArch(archCenter, Vector3.forward, cornerRadius, -90.0f, -90.0f, 10, color);
                    DrawArch(archCenter, Vector3.up, cornerRadius, -90.0f, -90.0f, 10, color);
                    DrawArch(archCenter, Vector3.right, cornerRadius, -90.0f, -90.0f, 10, color);
                }
                {
                    Vector3 archCenter = new Vector3(halfInnerDimensions.x, -halfInnerDimensions.y, halfInnerDimensions.z);
                    DrawArch(archCenter, Vector3.forward, cornerRadius, 0.0f, 90.0f, 10, color);
                    DrawArch(archCenter, Vector3.up, cornerRadius, 0.0f, 90.0f, 10, color);
                    DrawArch(archCenter, Vector3.right, cornerRadius, 0.0f, 90.0f, 10, color);

                }
                {
                    Vector3 archCenter = new Vector3(halfInnerDimensions.x, -halfInnerDimensions.y, -halfInnerDimensions.z);
                    DrawArch(archCenter, Vector3.forward, cornerRadius, 0.0f, 90.0f, 10, color);
                    DrawArch(archCenter, Vector3.up, cornerRadius, 0.0f, -90.0f, 10, color);
                    DrawArch(archCenter, Vector3.right, cornerRadius, 0.0f, -90.0f, 10, color);
                }
            }
        }
    }
}

