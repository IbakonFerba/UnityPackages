using UnityEngine;
using UnityEngine.UIElements;

namespace FK.UIElements
{
    /// <summary>
    /// <para>A straight line Visual Element</para>
    ///
    /// v1.2 12/2019
    /// Written by Fabian Kober
    /// fabian-kober@gmx.net
    /// </summary>
    public class Line : ImmediateModeElement
    {
        // ######################## STRUCTS & CLASSES ######################## //

        #region STRUCTS & CLASSES

        public new class UxmlFactory : UxmlFactory<Line, UxmlTraits>
        {
        }

        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            private readonly UxmlFloatAttributeDescription _startX = new UxmlFloatAttributeDescription {name = "startX"};
            private readonly UxmlFloatAttributeDescription _startY = new UxmlFloatAttributeDescription {name = "startY"};
            private readonly UxmlFloatAttributeDescription _endX = new UxmlFloatAttributeDescription {name = "endX"};
            private readonly UxmlFloatAttributeDescription _endY = new UxmlFloatAttributeDescription {name = "endY"};

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);

                Line line = (Line) ve;
                line._start = new Vector2(_startX.GetValueFromBag(bag, cc), _startY.GetValueFromBag(bag, cc));
                line._end = new Vector2(_endX.GetValueFromBag(bag, cc), _endY.GetValueFromBag(bag, cc));

                line.UpdateBounds();
            }
        }

        #endregion


        // ######################## PROPERTIES ######################## //

        #region PROPERTIES

        public Vector2 Start
        {
            get => _start;
            set
            {
                _start = value;
                UpdateBounds();
            }
        }

        public Vector2 End
        {
            get => _end;
            set
            {
                _end = value;
                UpdateBounds();
            }
        }

        public int Width
        {
            get => _width;
            set
            {
                _width = value;
                UpdateBounds();
            }
        }

        public Color StartColor
        {
            get => _startColor;
            set
            {
                _startColor = value;
                MarkDirtyRepaint();
            }
        }

        public Color EndColor
        {
            get => _endColor;
            set
            {
                _endColor = value;
                MarkDirtyRepaint();
            }
        }

        public Color AddColor
        {
            get => _addColor;
            set
            {
                _addColor = value;
                MarkDirtyRepaint();
            }
        }

        public float Opacity
        {
            get => _opacity;
            set
            {
                _opacity = Mathf.Clamp01(value);
                MarkDirtyRepaint();
            }
        }
        
        public float SoftEdgeFraction
        {
            get => _softEdgeFraction;
            set
            {
                _softEdgeFraction = Mathf.Clamp01(value);
                MarkDirtyRepaint();
            }
        }

        private static Material LineMaterial
        {
            get
            {
                if (!_lineMaterial)
                {
                    Shader shader = Shader.Find("Hidden/Internal-GUITextureClip");
                    _lineMaterial = new Material(shader);
                    _lineMaterial.hideFlags = HideFlags.HideAndDontSave;
                    _lineMaterial.SetInt("_SrcBlend", (int) UnityEngine.Rendering.BlendMode.SrcAlpha);
                    _lineMaterial.SetInt("_DstBlend", (int) UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                    _lineMaterial.SetInt("_Cull", (int) UnityEngine.Rendering.CullMode.Off);
                    _lineMaterial.SetInt("_ZWrite", 0);
                }

                return _lineMaterial;
            }
        }

        #endregion


        // ######################## PRIVATE VARS ######################## //

        #region PRIVATE VARS

        #region USS_PROPERTIES

        private static readonly CustomStyleProperty<int> _line_width_property = new CustomStyleProperty<int>("--line-width");
        private static readonly CustomStyleProperty<Color> _line_start_color_property = new CustomStyleProperty<Color>("--line-start-color");
        private static readonly CustomStyleProperty<Color> _line_end_color_property = new CustomStyleProperty<Color>("--line-end-color");
        private static readonly CustomStyleProperty<Color> _line_add_color_property = new CustomStyleProperty<Color>("--line-add-color");
        private static readonly CustomStyleProperty<float> _line_opacity_property = new CustomStyleProperty<float>("--line-opacity");
        private static readonly CustomStyleProperty<float> _line_soft_edge_fraction = new CustomStyleProperty<float>("--line-soft-edge-fraction");

        #endregion

        #region DEFAULT_VALUES

        private const int DEFAULT_LINE_WIDTH = 4;
        private static readonly Color _default_color = new Color(220 / 255.0f, 220 / 255.0f, 220 / 255.0f, 1);

        #endregion

        private Vector2 _start;
        private Vector2 _end;

        private int _width;

        private Color _startColor;
        private Color _endColor;
        private Color _addColor;

        private float _opacity = 1;
        private float _softEdgeFraction = 0.3f;

        private static Material _lineMaterial;

        #endregion


        // ######################## INITS ######################## //
        public Line()
        {
            // load styles
            styleSheets.Add(Resources.Load<StyleSheet>("Styles"));
            AddToClassList(UssClasses.FLOAT);

            // set default values
            _startColor = _default_color;
            _endColor = _default_color;
            _width = DEFAULT_LINE_WIDTH;

            RegisterCallback<CustomStyleResolvedEvent>(OnCustomStyleResolved);

            UpdateBounds();
        }


        // ######################## FUNCTIONALITY ######################## //

        #region FUNCTIONALITY

        private void OnCustomStyleResolved(CustomStyleResolvedEvent evt)
        {
            ICustomStyle styles = evt.customStyle;

            if (styles.TryGetValue(Line._line_width_property, out int value))
                _width = value;
            if (styles.TryGetValue(Line._line_start_color_property, out Color startColor))
                _startColor = startColor;
            if (styles.TryGetValue(Line._line_end_color_property, out Color endColor))
                _endColor = endColor;
            if (styles.TryGetValue(Line._line_add_color_property, out Color tintColor))
                AddColor = tintColor;
            if (styles.TryGetValue(Line._line_opacity_property, out float opacity))
                Opacity = opacity;
            if (styles.TryGetValue(Line._line_soft_edge_fraction, out float softEdgeFraction))
                SoftEdgeFraction = softEdgeFraction;

            UpdateBounds();
        }

        private void UpdateBounds()
        {
            Vector2 min = Vector2.Min(_start, _end);
            Vector2 max = Vector2.Max(_start, _end);

            // calculate bounds
            style.left = min.x;
            style.top = min.y;
            style.width = max.x - min.x;
            style.height = max.y - min.y;

            MarkDirtyRepaint();
        }

        protected override void ImmediateRepaint()
        {
            LineMaterial.SetPass(0);

            Vector2 min = Vector2.Min(_start, _end);

            // calculate a perpendicular vector with the length of half the width. That will be the offset of the quad vertices that make up the line
            Vector2 relativeStart = _start - min;
            Vector2 relativeEnd = _end - min;
            Vector2 direction = (relativeStart - relativeEnd).normalized;
            Vector2 normal = new Vector2(direction.y, direction.x).normalized;
            Vector2 vertexOffset = 0.5f * _width * normal;
            Vector2 edgeFractionVector = vertexOffset * _softEdgeFraction;
            vertexOffset -= edgeFractionVector;

            Color startColor = new Color(Mathf.Clamp01(_startColor.r+_addColor.r),Mathf.Clamp01(_startColor.g+_addColor.g),Mathf.Clamp01(_startColor.b+_addColor.b), _opacity);
            Color transparentStart = startColor;
            transparentStart.a = 0;
            Color endColor = new Color(Mathf.Clamp01(_endColor.r+_addColor.r),Mathf.Clamp01(_endColor.g+_addColor.g),Mathf.Clamp01(_endColor.b+_addColor.b), _opacity);
            Color transparentEnd = startColor;
            transparentEnd.a = 0;


            // draw a quad to render the line
            GL.Begin(GL.QUADS);
            
            // draw a soft edge
            GL.Color(transparentStart);
            GL.Vertex3(relativeStart.x - vertexOffset.x-edgeFractionVector.x, relativeStart.y + vertexOffset.y+edgeFractionVector.y, 0);
            GL.Color(startColor);
            GL.Vertex3(relativeStart.x - vertexOffset.x, relativeStart.y + vertexOffset.y, 0);
            GL.Color(endColor);
            GL.Vertex3(relativeEnd.x - vertexOffset.x, relativeEnd.y + vertexOffset.y, 0);
            GL.Color(transparentEnd);
            GL.Vertex3(relativeEnd.x - vertexOffset.x-edgeFractionVector.x, relativeEnd.y + vertexOffset.y+edgeFractionVector.y, 0);
            
            // draw the solid line
            GL.Color(startColor);
            GL.Vertex3(relativeStart.x + vertexOffset.x, relativeStart.y - vertexOffset.y, 0);
            GL.Vertex3(relativeStart.x - vertexOffset.x, relativeStart.y + vertexOffset.y, 0);
            GL.Color(endColor);
            GL.Vertex3(relativeEnd.x - vertexOffset.x, relativeEnd.y + vertexOffset.y, 0);
            GL.Vertex3(relativeEnd.x + vertexOffset.x, relativeEnd.y - vertexOffset.y, 0);
            
            // draw the other soft edge
            GL.Color(transparentStart);
            GL.Vertex3(relativeStart.x + vertexOffset.x+edgeFractionVector.x, relativeStart.y - vertexOffset.y-edgeFractionVector.y, 0);
            GL.Color(startColor);
            GL.Vertex3(relativeStart.x + vertexOffset.x, relativeStart.y - vertexOffset.y, 0);
            GL.Color(endColor);
            GL.Vertex3(relativeEnd.x + vertexOffset.x, relativeEnd.y - vertexOffset.y, 0);
            GL.Color(transparentEnd);
            GL.Vertex3(relativeEnd.x + vertexOffset.x+edgeFractionVector.x, relativeEnd.y - vertexOffset.y-edgeFractionVector.y, 0);
            GL.End();
        }

        public override bool ContainsPoint(Vector2 localPoint)
        {
            Vector2 point = this.ChangeCoordinatesTo(parent, localPoint);
            float distanceFromLine = Mathf.Abs(point.x * (_end.y - _start.y) - point.y * (_end.x - _start.x) + _end.x * _start.y - _end.y * _start.x) /
                                     Mathf.Sqrt(Mathf.Pow(_end.y - _start.y, 2) + Mathf.Pow(_end.x - _start.x, 2));
            return distanceFromLine <= _width * 0.5f;
        }

        #endregion
    }
}