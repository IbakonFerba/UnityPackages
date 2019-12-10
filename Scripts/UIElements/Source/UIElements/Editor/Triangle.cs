using UnityEngine;
using UnityEngine.UIElements;

namespace FK.UIElements
{
    /// <summary>
    /// <para>A UI Elements Triangle that can be rotated around an origin point</para>
    ///
    /// v1.0 12/2019
    /// Written by Fabian Kober
    /// fabian-kober@gmx.net
    /// </summary>
    public class Triangle : ImmediateModeElement
    {
        // ######################## PROPERTIES ######################## //

        #region PROPERTIES

        public Vector2 Point0
        {
            get => _points[0];
            set
            {
                _points[0] = value;
                CalulcateBounds();
            }
        }

        public Color Color0
        {
            get => _colors[0];
            set
            {
                _colors[0] = value;
                MarkDirtyRepaint();
            }
        }

        public Vector2 Point1
        {
            get => _points[1];
            set
            {
                _points[1] = value;
                CalulcateBounds();
            }
        }

        public Color Color1
        {
            get => _colors[1];
            set
            {
                _colors[1] = value;
                MarkDirtyRepaint();
            }
        }

        public Vector2 Point2
        {
            get => _points[2];
            set
            {
                _points[2] = value;
                CalulcateBounds();
            }
        }

        public Color Color2
        {
            get => _colors[2];
            set
            {
                _colors[2] = value;
                MarkDirtyRepaint();
            }
        }

        public float Angle
        {
            get => Mathf.Rad2Deg * _angle;
            set
            {
                _angle = Mathf.Deg2Rad * value;
                CalulcateBounds();
            }
        }

        public Vector2 Origin
        {
            get => _origin;
            set
            {
                _origin = value;
                CalulcateBounds();
            }
        }

        private static Material Material
        {
            get
            {
                if (!_material)
                {
                    Shader shader = Shader.Find("Hidden/Internal-GUITextureClip");
                    _material = new Material(shader);
                    _material.hideFlags = HideFlags.HideAndDontSave;
                    _material.SetInt("_SrcBlend", (int) UnityEngine.Rendering.BlendMode.SrcAlpha);
                    _material.SetInt("_DstBlend", (int) UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                    _material.SetInt("_Cull", (int) UnityEngine.Rendering.CullMode.Off);
                    _material.SetInt("_ZWrite", 0);
                }

                return _material;
            }
        }

        #endregion


        // ######################## PRIVATE VARS ######################## //

        #region PRIVATE VARS

        private static Material _material;

        private readonly Vector2[] _points = new Vector2[3];
        private readonly Vector2[] _localPoints = new Vector2[3];
        private readonly Color[] _colors = new Color[3];

        private float _angle;
        private Vector2 _origin;

        #endregion


        // ######################## INITS ######################## //

        #region CONSTRUCTORS

        public Triangle()
        {
            RegisterCallback<GeometryChangedEvent>(OnGeometryChanged);
        }

        #endregion


        // ######################## FUNCTIONALITY ######################## //

        #region FUNCTIONALITY

        private void OnGeometryChanged(GeometryChangedEvent evt)
        {
            // calculate local pouints
            _localPoints[0] = parent.ChangeCoordinatesTo(this, Point0);
            _localPoints[1] = parent.ChangeCoordinatesTo(this, Point1);
            _localPoints[2] = parent.ChangeCoordinatesTo(this, Point2);

            // rotate around origin
            float sin = Mathf.Sin(_angle);
            float cos = Mathf.Cos(_angle);
            Vector2 localOrigin = parent.ChangeCoordinatesTo(this, _origin);

            for (int i = 0; i < _points.Length; ++i)
            {
                _localPoints[i] = _localPoints[i] - localOrigin;
                _localPoints[i] = new Vector2(_localPoints[i].x * cos - _localPoints[i].y * sin, _localPoints[i].x * sin + _localPoints[i].y * cos) + localOrigin;
            }
        }

        protected override void ImmediateRepaint()
        {
            Material.SetPass(0);


            GL.Begin(GL.TRIANGLES);

            GL.Color(Color0);
            GL.Vertex3(_localPoints[0].x, _localPoints[0].y, 0);

            GL.Color(Color1);
            GL.Vertex3(_localPoints[1].x, _localPoints[1].y, 0);

            GL.Color(Color2);
            GL.Vertex3(_localPoints[2].x, _localPoints[2].y, 0);
            GL.End();
        }

        private void CalulcateBounds()
        {
            float sin = Mathf.Sin(_angle);
            float cos = Mathf.Cos(_angle);

            Vector2[] tempPoints = new Vector2[_points.Length];
            for (int i = 0; i < _points.Length; ++i)
            {
                tempPoints[i] = _points[i] - _origin;
                tempPoints[i] = new Vector2(tempPoints[i].x * cos - tempPoints[i].y * sin, tempPoints[i].x * sin + tempPoints[i].y * cos) + _origin;
            }

            Vector2 min = Vector2.Min(tempPoints[0], Vector2.Min(tempPoints[1], tempPoints[2]));
            Vector2 max = Vector2.Max(tempPoints[0], Vector2.Max(tempPoints[1], tempPoints[2]));

            // calculate bounds
            style.left = min.x;
            style.top = min.y;
            style.width = max.x - min.x;
            style.height = max.y - min.y;

            MarkDirtyRepaint();
        }

        public override bool ContainsPoint(Vector2 localPoint)
        {
            Vector3 barycentric = GetBarycentricCoordinates(localPoint);
            return barycentric.x > 0 && barycentric.y > 0 && barycentric.z > 0;
        }

        #endregion


        // ######################## UTILITIES ######################## //

        #region UTILITIES

        public Vector3 GetBarycentricCoordinates(Vector2 p)
        {
            Vector2 v0 = _localPoints[1] - _localPoints[0];
            Vector2 v1 = _localPoints[2] - _localPoints[0];
            Vector2 v2 = p - _localPoints[0];

            float den = v0.x * v1.y - v1.x * v0.y;
            Vector3 barycentric = new Vector3();
            barycentric.y = (v2.x * v1.y - v1.x * v2.y) / den;
            barycentric.z = (v0.x * v2.y - v2.x * v0.y) / den;
            barycentric.x = 1.0f - barycentric.y - barycentric.z;

            return barycentric;
        }

        public Vector2 GetCartesianFromBarycentric(Vector3 barycentric)
        {
            return barycentric.x * _localPoints[0] + barycentric.y * _localPoints[1] + barycentric.z * _localPoints[2];
        }

        #endregion
    }
}