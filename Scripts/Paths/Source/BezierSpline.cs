using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace FK.Paths
{
    /// <summary>
    /// <para>A Path constructed from cubic bezier splines</para>
    /// <para>This class was created with the help of this tutorial: https://catlikecoding.com/unity/tutorials/curves-and-splines/</para>
    ///
    /// v1.1 09/2018
    /// Written by Fabian Kober
    /// fabian-kober@gmx.net
    /// </summary>
    public class BezierSpline : Path
    {
        // ######################## PROPERTIES ######################## //
        /// <summary>
        /// The number of individual curves that create the path
        /// </summary>
        public override int SegmentCount => (Points.Length - 1) / 3;

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
                {
                    _modes[_modes.Length - 1] = _modes[0];
                    SetControlPoint(0, Points[0]);
                }
            }
        }

        // ######################## PRIVATE VARS ######################## //
        /// <summary>
        /// The Control point modes for each bezier curve
        /// </summary>
        [SerializeField] private Bezier.BezierControlPointMode[] _modes;


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
                new Vector3(3, 0, 0),
                new Vector3(4, 0, 0),
            };

            _modes = new[]
            {
                Bezier.BezierControlPointMode.MIRRORED,
                Bezier.BezierControlPointMode.MIRRORED
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
            for (int i = 1; i < ControlPointCount; i += 3)
            {
                Vector3 p1 = transform.TransformPoint(GetControlPoint(i + 0));
                Vector3 p2 = transform.TransformPoint(GetControlPoint(i + 1));
                Vector3 p3 = transform.TransformPoint(GetControlPoint(i + 2));
                Handles.DrawBezier(p0, p3, p1, p2, new Color(1, 1, 1, 0.2f), null, 2f);
                p0 = p3;
            }
        }

        /// <summary>
        /// Creates a new Bezier Spline
        /// </summary>
        /// <param name="menuCommand"></param>
        [MenuItem("GameObject/Paths/Bezier Spline", false, 1)]
        private static void CreateBezierSpline(MenuCommand menuCommand)
        {
            // create the Object
            GameObject obj = new GameObject("Bezier Spline", typeof(BezierSpline));

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
            // create the target point and add the segment
            Vector3 point = Points[Points.Length - 1];
            AddSegment(new Vector3(point.x + 1, point.y, point.z), new Vector3(point.x + 2, point.y, point.z), new Vector3(point.x + 3, point.y, point.z));
        }

        /// <summary>
        /// Adds a Segment that ends at the provided Point (in local space)
        /// </summary>
        /// <param name="targetPoint"></param>
        public override void AddSegment(Vector3 targetPoint)
        {
            // calculate the distance between the lastz point and the target point and add the segment
            Vector3 delta = targetPoint - Points[Points.Length - 1];
            AddSegment(Points[Points.Length - 1] + delta / 2, targetPoint - delta / 2, targetPoint);
        }

        /// <summary>
        /// Adds a Segment that ends at the provided Point and has the provided control points (in local space)
        /// </summary>
        /// <param name="p0"></param>
        /// <param name="p1"></param>
        /// <param name="targetPoint"></param>
        public void AddSegment(Vector3 p0, Vector3 p1, Vector3 targetPoint)
        {
            // resize the array to fit the new spline and add the points
            Array.Resize(ref Points, Points.Length + 3);
            Points[Points.Length - 3] = p0;
            Points[Points.Length - 2] = p1;
            Points[Points.Length - 1] = targetPoint;

            // resize the array of the modes to fit the new spline and save the new mode
            Array.Resize(ref _modes, _modes.Length + 1);
            _modes[_modes.Length - 1] = _modes[_modes.Length - 2];

            // enforce the control point mode for the start of the new segment
            EnforceMode(Points.Length - 4);

            // make sure this is still a loop
            if (IsLoop)
            {
                Points[Points.Length - 1] = Points[0];
                _modes[_modes.Length - 1] = _modes[0];
                EnforceMode(0);
            }

            // presample if desired
            if (DoPresample)
                Presample();
        }

        /// <summary>
        /// Deletes the last Segment
        /// </summary>
        public override void RemoveSegment()
        {
            // resize the arrays
            Array.Resize(ref Points, Points.Length - 3);
            Array.Resize(ref _modes, _modes.Length - 1);

            // enforce the control point mode for the start of the new segment
            EnforceMode(Points.Length - 4);

            // make sure this is still a loop
            if (IsLoop)
            {
                Points[Points.Length - 1] = Points[0];
                _modes[_modes.Length - 1] = _modes[0];
                EnforceMode(0);
            }

            // presample if desired
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
                segmentStartPointIndex = Points.Length - 4;
            }
            else
            {
                t = Mathf.Clamp01(t) * SegmentCount;
                segmentStartPointIndex = (int) t;
                t -= segmentStartPointIndex;
                segmentStartPointIndex *= 3;
            }

            // return the position in world space
            return transform.TransformPoint(Bezier.GetPoint(Points[segmentStartPointIndex + 0], Points[segmentStartPointIndex + 1], Points[segmentStartPointIndex + 2],
                Points[segmentStartPointIndex + 3],
                t));
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
                segmentStartPointIndex = Points.Length - 4;
            }
            else
            {
                t = Mathf.Clamp01(t) * SegmentCount;
                segmentStartPointIndex = (int) t;
                t -= segmentStartPointIndex;
                segmentStartPointIndex *= 3;
            }

            // return the velocity in world space
            return transform.TransformPoint(Bezier.GetFirstDerivative(Points[segmentStartPointIndex + 0], Points[segmentStartPointIndex + 1], Points[segmentStartPointIndex + 2],
                       Points[segmentStartPointIndex + 3], t)) - transform.position;
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

        /// <summary>
        /// Makes sure that the Control Point modes on both sides of a segment point are the same
        /// </summary>
        /// <param name="index"></param>
        private void EnforceMode(int index)
        {
            // calculate the mode index and get the mode
            int modeIndex = (index + 1) / 3;
            Bezier.BezierControlPointMode mode = _modes[modeIndex];

            // if the mode is FREE or we are not a loop and this is the start or end point of the spline, we don't need to do anything
            if (mode == Bezier.BezierControlPointMode.FREE || !IsLoop && (modeIndex == 0 || modeIndex == _modes.Length - 1))
                return;

            // get the index of the middle point
            int middleIndex = modeIndex * 3;

            int fixedIndex, enforcedIndex;
            // if we are before the middle point, the point before that is the fixed point
            if (index <= middleIndex)
            {
                // set fixed index, considering looping
                fixedIndex = middleIndex - 1;
                if (fixedIndex < 0)
                {
                    fixedIndex = Points.Length - 2;
                }

                // set the enforced index to the point after the middle point, considering looping
                enforcedIndex = middleIndex + 1;
                if (enforcedIndex >= Points.Length)
                {
                    enforcedIndex = 1;
                }
            }
            else // if we are after the middle point, the point after that is the fixed point
            {
                // set fixed index, considering looping
                fixedIndex = middleIndex + 1;
                if (fixedIndex >= Points.Length)
                {
                    fixedIndex = 1;
                }

                // set the enforced index to the point after the middle point, considering looping
                enforcedIndex = middleIndex - 1;
                if (enforcedIndex < 0)
                {
                    enforcedIndex = Points.Length - 2;
                }
            }

            // get the middle point
            Vector3 middle = Points[middleIndex];
            // calculate the tangent that should be used
            Vector3 enforcedTangent = middle - Points[fixedIndex];

            // if we are in aligned mode, we need to calculate the length of the tangent
            if (mode == Bezier.BezierControlPointMode.ALIGNED)
            {
                enforcedTangent = enforcedTangent.normalized * Vector3.Distance(middle, Points[enforcedIndex]);
            }

            // set the enforced point
            Points[enforcedIndex] = middle + enforcedTangent;
        }

        // ######################## UTILITIES ######################## //
        /// <summary>
        /// Returns a segment point of the Path. This sets only points that the path is actually passing through (exluding Bezier Control Points for example).
        /// The Index is continuous (meaning that index 0 is the starting point, index 1 the next Segment point, etc)
        /// </summary>
        public override Vector3 GetSegmentPoint(int index)
        {
            // make sure the index exists
            if (index < 0 || index * 3 > Points.Length - 1)
            {
                Debug.LogError($"Point index {index} is out of Bounds!");
                return Vector3.zero;
            }

            // get the point
            return GetControlPoint(index * 3);
        }

        /// <summary>
        /// Set any point of the path (including Bezier Control Points)
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

            // if this point is a segment point (one that the curve runs through), we need to move its tangent with it
            if (index % 3 == 0)
            {
                // get the delta between the old point and the new point
                Vector3 delta = point - Points[index];

                // move the tangents considering looping
                if (IsLoop)
                {
                    // if this point is the start point of the path, we need to also update the last point to keep this a loop
                    if (index == 0)
                    {
                        Points[1] += delta;
                        Points[Points.Length - 2] += delta;
                        Points[Points.Length - 1] = point;
                    }
                    else if (index == Points.Length - 1) // if this point is the end point of the path, we need to also update the first point to keep this a loop
                    {
                        Points[0] = point;
                        Points[1] += delta;
                        Points[index - 1] += delta;
                    }
                    else // this is a point in the middle of the path, jsut update the tangent
                    {
                        Points[index - 1] += delta;
                        Points[index + 1] += delta;
                    }
                }
                else
                {
                    // update the control point before this point if one exists
                    if (index > 0)
                    {
                        Points[index - 1] += delta;
                    }

                    // update the control point after this point if one exists
                    if (index + 1 < Points.Length)
                    {
                        Points[index + 1] += delta;
                    }
                }
            }

            // save the point and enforce Control point modes
            Points[index] = point;
            EnforceMode(index);

            // presample if desired
            if (DoPresample)
                Presample();
        }

        /// <summary>
        /// Set a segment point of the Path. This sets only points that the path is actually passing through (excluding Bezier Control Points).
        /// The Index is continuous (meaning that index 0 is the starting point, index 1 the next Segment point, etc)
        /// </summary>
        /// <param name="index"></param>
        /// <param name="point"></param>
        /// <param name="space"></param>
        public override void SetSegmentPoint(int index, Vector3 point, Space space = Space.Self)
        {
            // make sure the index exists
            if (index < 0 || index * 3 > Points.Length - 1)
            {
                Debug.LogError($"Point index {index} is out of Bounds!");
                return;
            }

            // set the point
            SetControlPoint(index * 3, space == Space.World ? transform.InverseTransformPoint(point) : point);
        }

        /// <summary>
        /// Returns the Mode for the control point
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Bezier.BezierControlPointMode GetControlPointMode(int index)
        {
            return _modes[(index + 1) / 3];
        }

        /// <summary>
        /// sets the Mode of the control point
        /// </summary>
        /// <param name="index"></param>
        /// <param name="mode"></param>
        public void SetControlPointMode(int index, Bezier.BezierControlPointMode mode)
        {
            if (index < 0 || index > Points.Length - 1)
            {
                Debug.LogError($"Control Point index {index} is out of Bounds!");
                return;
            }

            // get the mode
            int modeIndex = (index + 1) / 3;
            _modes[modeIndex] = mode;

            // keep this a loop
            if (IsLoop)
            {
                if (modeIndex == 0)
                {
                    _modes[_modes.Length - 1] = mode;
                }
                else if (modeIndex == _modes.Length - 1)
                {
                    _modes[0] = mode;
                }
            }

            // enforce mode
            EnforceMode(index);

            // presample if desired
            if (DoPresample)
                Presample();
        }
    }
}