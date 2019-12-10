using System;
using UnityEngine;
using UnityEngine.UIElements;
using ColorUtility = FK.Utility.ColorUtility;

namespace FK.UIElements
{
    /// <summary>
    /// <para>A Quad that displays saturation and luminance values for a given hue. Meant for use in a color wheel</para>
    ///
    /// v1.0 12/2019
    /// Written by Fabian Kober
    /// fabian-kober@gmx.net
    /// </summary>
    public class SaturationLuminanceQuad : ImmediateModeElement
    {
        // ######################## EVENTS ######################## //

        #region EVENTS

        /// <summary>
        /// x is Saturation, y is Luminance
        /// </summary>
        public Action<Vector2> OnValueChanged;

        #endregion


        // ######################## PROPERTIES ######################## //

        #region PROPERTIES

        /// <summary>
        /// [0, 360]
        /// </summary>
        public int Hue
        {
            get => _hue;
            set => _hue = Mathf.Clamp(value, 0, 360);
        }

        /// <summary>
        /// [0, 1]
        /// </summary>
        public float Saturation
        {
            get => _saturationLuminance.x;
            set
            {
                _saturationLuminance.x = Mathf.Clamp01(value);
                _handle.style.left = _saturationLuminance.x * layout.width - _handle.layout.width * 0.5f;
            }
        }

        /// <summary>
        /// [0, 1]
        /// </summary>
        public float Luminance
        {
            get => _saturationLuminance.y;
            set
            {
                _saturationLuminance.y = Mathf.Clamp01(value);
                _handle.style.top = (1 - _saturationLuminance.y) * layout.height - _handle.layout.height * 0.5f;
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

        private readonly VisualElement _handle;

        private int _hue;

        /// <summary>
        /// x is Saturation, y is Luminance
        /// </summary>
        private Vector2 _saturationLuminance;

        #endregion


        // ######################## INITS ######################## //

        #region CONSTRUCTORS

        public SaturationLuminanceQuad()
        {
            // add handle
            _handle = new VisualElement {name = "handle"};
            _handle.style.backgroundImage = Resources.Load<Texture2D>("Images/ColorWheelHandle");
            _handle.style.position = Position.Absolute;
            _handle.style.width = 20;
            _handle.style.height = _handle.style.width;
            Add(_handle);

            // register callbacks
            RegisterCallback<GeometryChangedEvent>(UpdatePositions);

            // add handle manipulator
            DragManipulator handleManipulator = new DragManipulator(new ManipulatorActivationFilter {button = MouseButton.LeftMouse}, true, _handle, delta => HandleUpdate());
            handleManipulator.OnActivated += evt => HandleUpdate();
            this.AddManipulator(handleManipulator);
        }

        #endregion


        // ######################## FUNCTIONALITY ######################## //

        #region FUNCTIONALITY

        protected override void ImmediateRepaint()
        {
            Material.SetPass(0);

            // raw 4 quads to render the gradient
            GL.Begin(GL.QUADS);

            // top left quad
            GL.Color(ColorUtility.HSL(Hue, 50, 100));
            GL.Vertex3(layout.width * 0.5f, 0, 0);

            GL.Color(ColorUtility.HSL(Hue, 0, 100));
            GL.Vertex3(0, 0, 0);

            GL.Color(ColorUtility.HSL(Hue, 0, 50));
            GL.Vertex3(0, layout.height * 0.5f, 0);

            GL.Color(ColorUtility.HSL(Hue, 50, 50));
            GL.Vertex3(layout.width * 0.5f, layout.height * 0.5f, 0);


            // top right quad
            GL.Color(ColorUtility.HSL(Hue, 100, 100));
            GL.Vertex3(layout.width, 0, 0);

            GL.Color(ColorUtility.HSL(Hue, 50, 100));
            GL.Vertex3(layout.width * 0.5f, 0, 0);

            GL.Color(ColorUtility.HSL(Hue, 50, 50));
            GL.Vertex3(layout.width * 0.5f, layout.height * 0.5f, 0);

            GL.Color(ColorUtility.HSL(Hue, 100, 50));
            GL.Vertex3(layout.width, layout.height * 0.5f, 0);


            // bottom left quad
            GL.Color(ColorUtility.HSL(Hue, 50, 50));
            GL.Vertex3(layout.width * 0.5f, layout.height * 0.5f, 0);

            GL.Color(ColorUtility.HSL(Hue, 0, 50));
            GL.Vertex3(0, layout.height * 0.5f, 0);

            GL.Color(ColorUtility.HSL(Hue, 0, 0));
            GL.Vertex3(0, layout.height, 0);

            GL.Color(ColorUtility.HSL(Hue, 50, 0));
            GL.Vertex3(layout.width * 0.5f, layout.height, 0);


            // bottom right quad
            GL.Color(ColorUtility.HSL(Hue, 100, 50));
            GL.Vertex3(layout.width, layout.height * 0.5f, 0);

            GL.Color(ColorUtility.HSL(Hue, 50, 50));
            GL.Vertex3(layout.width * 0.5f, layout.height * 0.5f, 0);

            GL.Color(ColorUtility.HSL(Hue, 50, 0));
            GL.Vertex3(layout.width * 0.5f, layout.height, 0);

            GL.Color(ColorUtility.HSL(Hue, 100, 0));
            GL.Vertex3(layout.width, layout.height, 0);

            GL.End();
        }

        private void UpdatePositions(GeometryChangedEvent evt)
        {
            Saturation = _saturationLuminance.x;
            Luminance = _saturationLuminance.y;
        }

        private void HandleUpdate()
        {
            // clamp the andle to inside the quad
            Vector2 position = new Vector2(_handle.style.left.value.value + _handle.layout.width * 0.5f, _handle.style.top.value.value + _handle.layout.height * 0.5f);
            if (position.x < 0)
                position.x = 0;

            if (position.x > layout.width)
                position.x = layout.width;

            if (position.y < 0)
                position.y = 0;

            if (position.y > layout.width)
                position.y = layout.height;

            _handle.style.left = position.x - _handle.layout.width * 0.5f;
            _handle.style.top = position.y - _handle.layout.height * 0.5f;

            // calculate saturation and luminance values
            _saturationLuminance = new Vector2(position.x / layout.width, (layout.height - position.y) / layout.height);
            OnValueChanged?.Invoke(_saturationLuminance);
        }

        #endregion
    }
}