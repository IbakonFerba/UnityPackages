using UnityEngine;

namespace FK.Utility
{
    /// <summary>
    /// <para>An oriented bounding box that can be used to check volumes for points</para>
    ///
    /// v1.1 10/2019
    /// Written by Fabian Kober
    /// fabian-kober@gmx.net
    /// </summary>
    public struct OBB
    {
        // ######################## PROPERTIES ######################## //
        public Quaternion Rotation => _transformMatrix.rotation;
        public Vector3 Center => _center;

        /// <summary>
        /// The total size of the box. This is always twice as large as the extends
        /// </summary>
        public Vector3 Size => _size;

        /// <summary>
        /// The extends of the box. This is always half the size of the size
        /// </summary>
        public Vector3 Extends => _extends;


        // ######################## PRIVATE VARS ######################## //
        private Vector3 _center;
        private Vector3 _size;
        private Vector3 _extends;

        private Matrix4x4 _transformMatrix;
        private Vector3[] _worldPoints;


        // ######################## INITS ######################## //
        public OBB(Vector3 center, Vector3 size, Quaternion rotation, bool calculateWorldPoints = false)
        {
            _center = center;
            _size = size;
            _extends = size * 0.5f;
            _transformMatrix = Matrix4x4.TRS(center, rotation, Vector3.one);

            if (calculateWorldPoints)
            {
                _worldPoints = new Vector3[8];
                CalculateWorldPoints();
            }
            else
            {
                _worldPoints = null;
            }
        }


        // ######################## FUNCTIONALITY ######################## //
        /// <summary>
        /// Is the point contained inside the box?
        /// </summary>
        /// <param name="worldPoint"></param>
        /// <returns></returns>
        public bool Contains(Vector3 worldPoint)
        {
            Vector3 localPoint = _transformMatrix.inverse.MultiplyPoint3x4(worldPoint);
            return localPoint.x >= -_extends.x && localPoint.x <= _extends.x && localPoint.y >= -_extends.y && localPoint.y <= _extends.y && localPoint.z >= -_extends.z && localPoint.z <= _extends.z;
        }

        private void CalculateWorldPoints()
        {
            if (_worldPoints == null)
                _worldPoints = new Vector3[8];

            _worldPoints[0] = _transformMatrix.MultiplyPoint3x4(new Vector3(-_extends.x, -_extends.y, -_extends.z));
            _worldPoints[1] = _transformMatrix.MultiplyPoint3x4(new Vector3(_extends.x, -_extends.y, -_extends.z));
            _worldPoints[2] = _transformMatrix.MultiplyPoint3x4(new Vector3(_extends.x, -_extends.y, _extends.z));
            _worldPoints[3] = _transformMatrix.MultiplyPoint3x4(new Vector3(-_extends.x, -_extends.y, _extends.z));
            _worldPoints[4] = _transformMatrix.MultiplyPoint3x4(new Vector3(-_extends.x, _extends.y, -_extends.z));
            _worldPoints[5] = _transformMatrix.MultiplyPoint3x4(new Vector3(_extends.x, _extends.y, -_extends.z));
            _worldPoints[6] = _transformMatrix.MultiplyPoint3x4(new Vector3(_extends.x, _extends.y, _extends.z));
            _worldPoints[7] = _transformMatrix.MultiplyPoint3x4(new Vector3(-_extends.x, _extends.y, _extends.z));
        }


        // ######################## GETTER ######################## //
        /// <summary>
        /// Returns the 8 corner points of the OBB in world space. These are generated on demand, so calling this the first time will calculate them and all subsequent changes to the OBB will recalculate them from that point on
        /// </summary>
        /// <returns></returns>
        public Vector3[] GetWorldPoints()
        {
            if (_worldPoints == null)
                CalculateWorldPoints();

            return _worldPoints;
        }


        // ######################## SETTER ######################## //
        /// <summary>
        /// Sets the transform values of the OBB. Might recalculate the world points
        /// </summary>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        public void SetTransform(Vector3 position, Quaternion rotation)
        {
            _center = position;
            _transformMatrix.SetTRS(position, rotation, Vector3.one);

            if (_worldPoints != null)
                CalculateWorldPoints();
        }

        /// <summary>
        /// Sets the position of the OBB. Might recalculate the world points
        /// </summary>
        /// <param name="position"></param>
        public void SetPosition(Vector3 position)
        {
            SetTransform(position, Rotation);
        }

        /// <summary>
        /// Sets the rotation of the OBB. Might recalculate the world points
        /// </summary>
        /// <param name="rotation"></param>
        public void SetRotation(Quaternion rotation)
        {
            SetTransform(_center, rotation);
        }

        /// <summary>
        /// Sets the size of the OBB. Might recalculate the world points
        /// </summary>
        /// <param name="size"></param>
        public void SetSize(Vector3 size)
        {
            _size = size;
            _extends = size * 0.5f;

            if (_worldPoints != null)
                CalculateWorldPoints();
        }


        // ######################## UTILITIES ######################## //

        #region EDITOR
        
        /// <summary>
        /// Draws the OBB. Only available in the Editor
        /// </summary>
        /// <param name="color"></param>
        /// <param name="duration"></param>
        public void DebugDraw(Color color, float duration = 1)
        {
#if UNITY_EDITOR
            if (_worldPoints == null)
                CalculateWorldPoints();

            Debug.DrawLine(_worldPoints[0], _worldPoints[1], color, duration);
            Debug.DrawLine(_worldPoints[1], _worldPoints[2], color, duration);
            Debug.DrawLine(_worldPoints[2], _worldPoints[3], color, duration);
            Debug.DrawLine(_worldPoints[3], _worldPoints[0], color, duration);
            Debug.DrawLine(_worldPoints[4], _worldPoints[5], color, duration);
            Debug.DrawLine(_worldPoints[5], _worldPoints[6], color, duration);
            Debug.DrawLine(_worldPoints[6], _worldPoints[7], color, duration);
            Debug.DrawLine(_worldPoints[7], _worldPoints[4], color, duration);
            Debug.DrawLine(_worldPoints[0], _worldPoints[4], color, duration);
            Debug.DrawLine(_worldPoints[1], _worldPoints[5], color, duration);
            Debug.DrawLine(_worldPoints[2], _worldPoints[6], color, duration);
            Debug.DrawLine(_worldPoints[3], _worldPoints[7], color, duration);
#endif
        }

        /// <summary>
        /// Draws the OBB in white. Only available in the Editor
        /// </summary>
        public void DebugDraw()
        {
            DebugDraw(Color.white);
        }

        #endregion
    }
}