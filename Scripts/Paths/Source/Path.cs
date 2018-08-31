using System.Collections.Generic;
using UnityEngine;

namespace FK.Paths
{
    /// <summary>
    /// <para>The Base of a Path. It has akk methods that are needed to traverse the path.</para>
    /// <para>This class was created with the help of these tutorials:</para>
    /// <para>https://catlikecoding.com/unity/tutorials/curves-and-splines/</para>
    /// <para>https://unity3d.com/de/learn/tutorials/topics/scripting/creating-spline-tool</para>
    /// 
    /// v1.0 08/2018
    /// Written by Fabian Kober
    /// fabian-kober@gmx.net
    /// </summary>
    public abstract class Path : MonoBehaviour
    {
        // ######################## PROPERTIES ######################## //
        /// <summary>
        /// The amount of Segment control points (points that are actual positions the Path runs through)
        /// </summary>
        public int SegmentPointCount => SegmentCount + 1;

        /// <summary>
        /// The amount of actual points controlling the Path (including Bezier control points for example)
        /// </summary>
        public int ControlPointCount => Points.Length;

        /// <summary>
        /// If true, we should automatically presample the path if anything changes
        /// </summary>
        protected bool DoPresample
        {
            get
            {
#if UNITY_EDITOR
                // only return true in Playmode if we are in the editor
                return Application.isPlaying && AutoPresample;
#endif

                return AutoPresample;
            }
        }


        /// <summary>
        /// The number of individual curves that create the path
        /// </summary>
        public abstract int SegmentCount { get; }

        /// <summary>
        /// Is this Path a Loop?
        /// </summary>
        public abstract bool IsLoop { get; set; }

        // ######################## PUBLIC VARS ######################## //
        /// <summary>
        /// Should we automatically presample the path if anything changes? This is only for setting the value, to check please use DoPresample
        /// </summary>
        public bool AutoPresample = false;


        // ######################## PROTECTED VARS ######################## //
        /// <summary>
        /// All the Points that make up the path
        /// </summary>
        [SerializeField] protected Vector3[] Points;

        /// <summary>
        /// Backing for IsLoop Property
        /// </summary>
        [SerializeField] protected bool Loop;

        /// <summary>
        /// The Points of the Presampled Path that have Regular distances and allow a linear progression along the path
        /// </summary>
        [SerializeField] protected Vector3[] LinearPoints;


        // ######################## PRIVATE VARS ######################## //
        /// <summary>
        /// Should we presample the path on awake?
        /// </summary>
        [SerializeField] private bool _presampleOnAwake = false;

        /// <summary>
        /// The step size relative to the curve Lenght to find the next Presampeled Point with a regular distance.
        /// The smaller this value is, the more accurate the presampeled curve will be, but the longer the calculation takes
        /// </summary>
        [SerializeField] private float _presampleSearchStepSize = 0.00001f;

        /// <summary>
        /// Resolution of the Presampeled Curve.
        /// The smaller this value is, the more accurate the presampeled curve will be, but the longer the calculation takes
        /// </summary>
        [SerializeField] private int _presampleResolution = 1024;


        // ######################## UNITY EVENT FUNCTIONS ######################## //
        protected virtual void Awake()
        {
            if (_presampleOnAwake)
                Presample();
        }

#if UNITY_EDITOR
        /// <summary>
        /// Displays the Presampeled Points if the Curve is selected
        /// </summary>
        protected virtual void OnDrawGizmosSelected()
        {
            if (LinearPoints == null)
                return;
            foreach (Vector3 point in LinearPoints)
            {
                Gizmos.DrawSphere(transform.TransformPoint(point), 0.1f);
            }
        }
#endif

        // ######################## INITS ######################## //
        /// <summary>
        /// Calculates Points on the Path with a constant distance from each other. These points can then be used to get a uniform point on the Path and traverse it linearly
        /// </summary>
        public void Presample()
        {
            // get the total length of the path
            float length = GetLength(_presampleSearchStepSize);

            // create a list for the linear points
            List<Vector3> linearPoints = new List<Vector3>();

            // calculate the distance the points should have from each other
            float stepDist = length / _presampleResolution;

            // traverse the path to calculate the points, start at position 0
            Vector3 start = transform.InverseTransformPoint(GetPosition(0f));
            float t = 0f;
            // as long as t is less than 1, we are not finished
            while (t <= 1f)
            {
                // get the current position
                Vector3 current = transform.InverseTransformPoint(GetPosition(t));
                // check if the distance between the previous linear point and the current position is less than the desired distance. If it is, we need to march on
                while ((current - start).magnitude < stepDist && t <= 1f)
                {
                    // go one step further and sample the point
                    t += _presampleSearchStepSize;
                    current = transform.InverseTransformPoint(GetPosition(t));
                }

                // we found the next point, save it and set it at the next start point
                start = current;
                linearPoints.Add(current);
            }

            // save the points
            LinearPoints = linearPoints.ToArray();
        }

        // ######################## FUNCTIONALITY ######################## //
        /// <summary>
        /// Returns a position along the path that is linear to the value of t. This results in a constant speed when traversing the path
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public Vector3 GetUniformPosition(float t)
        {
            if (LinearPoints == null || (LinearPoints != null && LinearPoints.Length == 0))
            {
                Debug.LogWarning($"The path \"{gameObject.name}\" was not presampled yet. We will Presample now, but note that this can take a bit of time. You should make sure you know when Presampling is happening!");
                Presample();
            }
            
            // calculate the sections of the resampeled path
            int sections = LinearPoints.Length - (IsLoop ? 0 : 1);

            // calculate the index of the current point
            int i = Mathf.Min(Mathf.FloorToInt(t * sections), sections - 1);
            int count = LinearPoints.Length;
            if (i < 0)
                i += count;

            // calculate a t between 0 and 1
            t = t * (float) sections - (float) i;

            // return the Position
            Vector3 p0 = LinearPoints[i % count];
            Vector3 p1 = LinearPoints[(i + 1) % count];
            return transform.TransformPoint(Vector3.Lerp(p0, p1, t));
        }

        /// <summary>
        /// Returns the Direction of the path at the current uniform position
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public Vector3 GetUniformDirection(float t)
        {
            Vector3 p0 = GetUniformPosition(t - 0.001f);
            Vector3 p1 = GetUniformPosition(t + 0.001f);
            return (p1 - p0).normalized;
        }

        /// <summary>
        /// Returns the total length of the Path
        /// </summary>
        /// <param name="stepSize">The steps size for calculating the length. The smaller this value is the more accurate the length will be, but the more time the calculation takes</param>
        /// <returns></returns>
        public float GetLength(float stepSize = 0.001f)
        {
            float length = 0;
            Vector3 p0 = GetPosition(0);
            for (float t = 0f; t < 1f; t += stepSize)
            {
                Vector3 p1 = GetPosition(t);
                length += (p1 - p0).magnitude;
                p0 = p1;
            }

            return length;
        }

        /// <summary>
        /// Adds a default Segment to the Path
        /// </summary>
        public abstract void AddSegment();

        /// <summary>
        /// Adds a Segment that ends at the provided Point (in local space)
        /// </summary>
        /// <param name="targetPoint"></param>
        public abstract void AddSegment(Vector3 targetPoint);

        /// <summary>
        /// Deletes the last Segment
        /// </summary>
        public abstract void RemoveSegment();

        /// <summary>
        /// Returns a position along the path. Note that using this function, movement along the path will appear faster on longer segments because t does not scale linearly to the path, but to each path segment
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public abstract Vector3 GetPosition(float t);

        /// <summary>
        /// Returns a Velocity along the path. Note that with this function, t does not scale linearly to the path, but to each path segment
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public abstract Vector3 GetVelocity(float t);

        /// <summary>
        /// Returns a Direction along the path. Note that with this function, t does not scale linearly to the path, but to each path segment
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public abstract Vector3 GetDirection(float t);


        // ######################## UTILITIES ######################## //
        public Vector3 GetControlPoint(int index)
        {
            return Points[index];
        }

        /// <summary>
        /// Set any point of the path (including Bezier Control Points for example)
        /// </summary>
        /// <param name="index"></param>
        /// <param name="point"></param>
        /// <param name="space"></param>
        public abstract void SetControlPoint(int index, Vector3 point, Space space = Space.Self);

        /// <summary>
        /// Set a segment point of the Path. This sets only points that the path is actually passing through (exluding Bezier Control Points for example).
        /// The Index is continuous (meaning that index 0 is the starting point, index 1 the next Segment point, etc)
        /// </summary>
        /// <param name="index"></param>
        /// <param name="point"></param>
        /// <param name="space"></param>
        public abstract void SetSegmentPoint(int index, Vector3 point, Space space = Space.Self);
    }
}