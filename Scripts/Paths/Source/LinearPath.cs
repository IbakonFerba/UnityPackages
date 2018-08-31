using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace FK.Paths
{
    /// <summary>
    /// <para>A Linear Path that has sharp turns between linear segments</para>
    ///
    /// v1.0 08/2018
    /// Written by Fabian Kober
    /// fabian-kober@gmx.net
    /// </summary>
    public class LinearPath : Path
    {
        // ######################## PROPERTIES ######################## //
        /// <summary>
        /// The number of individual curves that create the path
        /// </summary>
        public override int SegmentCount => Points.Length - 1;

        /// <summary>
        /// Is this Path a Loop?
        /// </summary>
        public override bool IsLoop
        {
            get { return Loop; }
            set
            {
                Loop = value;

                // make sure this Path is actually a loop
                if (Loop)
                    SetControlPoint(0, Points[0]);
            }
        }


        // ######################## UNITY EVENT FUNCTIONS ######################## //
        /// <summary>
        /// Resets the Path
        /// </summary>
        public void Reset()
        {
            Points = new[]
            {
                new Vector3(1, 0, 0),
                new Vector3(2, 0, 0),
            };
            Presample();
        }

#if UNITY_EDITOR
        /// <summary>
        /// Draws the Path as a transparent Line
        /// </summary>
        private void OnDrawGizmos()
        {
            Vector3 p0 = transform.TransformPoint(GetControlPoint(0));
            for (int i = 1; i < ControlPointCount; ++i)
            {
                Vector3 p1 = transform.TransformPoint(GetControlPoint(i + 0));
                Handles.color = new Color(1, 1, 1, 0.2f);
                Handles.DrawLine(p0, p1);
                p0 = p1;
            }
        }

        /// <summary>
        /// Creates a new Linear path
        /// </summary>
        /// <param name="menuCommand"></param>
        [MenuItem("GameObject/Paths/Linear Path", false, 1)]
        private static void CreateBezierSpline(MenuCommand menuCommand)
        {
            // create the Object
            GameObject obj = new GameObject("Linear Path", typeof(LinearPath));

            // Ensure it gets reparented if this was a context click (otherwise does nothing)
            GameObjectUtility.SetParentAndAlign(obj, menuCommand.context as GameObject);
            // Register the creation in the undo system
            Undo.RegisterCreatedObjectUndo(obj, "Create " + obj.name);
            Selection.activeObject = obj;
        }
#endif

        // ######################## FUNCTIONALITY ######################## //
        /// <summary>
        /// Adds a default Segment to the Path
        /// </summary>
        public override void AddSegment()
        {
            // create a target point and add the segment
            Vector3 point = Points[Points.Length - 1];
            AddSegment(new Vector3(point.x + 1, point.y, point.z));
        }

        /// <summary>
        /// Adds a Segment that ends at the provided Point (in local space)
        /// </summary>
        /// <param name="targetPoint"></param>
        public override void AddSegment(Vector3 targetPoint)
        {
            // resize the Points Array to fit the new path
            Array.Resize(ref Points, Points.Length + 1);

            // set the point
            Points[Points.Length - 1] = targetPoint;

            // make sure the path still loops
            if (IsLoop)
                Points[Points.Length - 1] = Points[0];

            // Presample if desired
            if (DoPresample)
                Presample();
        }

        /// <summary>
        /// Deletes the last Segment
        /// </summary>
        public override void RemoveSegment()
        {
            // resize the Points Array to fit the new path
            Array.Resize(ref Points, Points.Length - 1);

            // make sure the path still loops
            if (IsLoop)
                Points[Points.Length - 1] = Points[0];

            // Presample if desired
            if (DoPresample)
                Presample();
        }

        /// <summary>
        /// Returns a position along the path. Note that using this function, movement along the path will appear faster on longer segments because t does not scale linearly to the path, but to each path segment
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public override Vector3 GetPosition(float t)
        {
            // calculate the start index of the segment
            int segmentStartPointIndex;
            if (t >= 1f)
            {
                t = 1f;
                segmentStartPointIndex = Points.Length - 2;
            }
            else
            {
                t = Mathf.Clamp01(t) * SegmentCount;
                segmentStartPointIndex = (int) t;
                t -= segmentStartPointIndex;
            }

            // return the position in world space
            return transform.TransformPoint(Vector3.Lerp(Points[segmentStartPointIndex], Points[segmentStartPointIndex + 1], t));
        }

        /// <summary>
        /// Returns a Velocity along the path. Note that with this function, t does not scale linearly to the path, but to each path segment
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public override Vector3 GetVelocity(float t)
        {
            // calculate the start index of the segment
            int segmentStartPointIndex;
            if (t >= 1f)
            {
                t = 1f;
                segmentStartPointIndex = Points.Length - 2;
            }
            else
            {
                t = Mathf.Clamp01(t) * SegmentCount;
                segmentStartPointIndex = (int) t;
                t -= segmentStartPointIndex;
            }

            // return the Velocity in world space
            return transform.TransformPoint(Points[segmentStartPointIndex + 1] - Points[segmentStartPointIndex]) - transform.position;
        }

        /// <summary>
        /// Returns a Direction along the path. Note that with this function, t does not scale linearly to the path, but to each path segment
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public override Vector3 GetDirection(float t)
        {
            // return the direction in world space
            return GetVelocity(t).normalized;
        }


        // ######################## UTILITIES ######################## //
        /// <summary>
        /// Set any point of the path
        /// </summary>
        /// <param name="index"></param>
        /// <param name="point"></param>
        /// <param name="space"></param>
        public override void SetControlPoint(int index, Vector3 point, Space space = Space.Self)
        {
            // make sure the index exists
            if (index < 0 || index > Points.Length - 1)
            {
                Debug.LogError($"Control Point index {index} is out of Bounds!");
                return;
            }

            // if the point is a worldspace point we need to Transform it first
            if (space == Space.World)
                point = transform.InverseTransformPoint(point);

            // make sure we still are a loop
            if (IsLoop)
            {
                if (index == 0)
                    Points[Points.Length - 1] = point;
                else if (index == Points.Length - 1)
                    Points[0] = point;
            }

            // update the point
            Points[index] = point;

            // presample if desired
            if (DoPresample)
                Presample();
        }

        /// <summary>
        /// Set a segment point of the Path. This sets only points that the path is actually passing through.
        /// The Index is continuous (meaning that index 0 is the starting point, index 1 the next Segment point, etc)
        /// </summary>
        /// <param name="index"></param>
        /// <param name="point"></param>
        /// <param name="space"></param>
        public override void SetSegmentPoint(int index, Vector3 point, Space space = Space.Self)
        {
            SetControlPoint(index, point, space);
        }
    }
}