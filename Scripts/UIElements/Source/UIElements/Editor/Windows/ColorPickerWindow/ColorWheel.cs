using System;
using UnityEngine;
using UnityEngine.UIElements;
using FK.Utility;
using ColorUtility = FK.Utility.ColorUtility;

namespace FK.UIElements
{
    /// <summary>
    /// <para>A HSL Color Wheel</para>
    ///
    /// v1.0 12/2019
    /// Written by Fabian Kober
    /// fabian-kober@gmx.net
    /// </summary>
    public class ColorWheel : VisualElement
    {
        // ######################## STRUCTS & CLASSES ######################## //

        #region STRUCTS & CLASSES

        public new class UxmlFactory : UxmlFactory<ColorWheel>
        {
        }

        #endregion


        // ######################## EVENTS ######################## //

        #region EVENTS

        public event Action<Color> OnColorChanged;

        #endregion


        // ######################## PROPERTIES ######################## //

        #region PROPERTIES

        public Color Color
        {
            get => _color;
            set
            {
                _color = value;

                // Get HSL from color
                int hue = _color.Hue();
                int sat = _color.Saturation();
                _luminance = _color.Luminance();

                // if the luminance is 0 or 100, the saturation could be anything. For continuity, use the last saturation
                if (!(_luminance <= 0 || _luminance >= 100))
                    _saturation = sat;

                // if the saturation is 0 or the luminance is 0 or 100, the hue could be anything. For continuity, use the last hue
                if (!(_saturation <= 0.0f || _luminance <= 0.0f || _luminance >= 100f))
                    _hue = hue;

                DisplayCurrentColor();
            }
        }

        #endregion


        // ######################## PRIVATE VARS ######################## //

        #region PRIVATE VARS

        #region USS PROPERTIES

        private static readonly CustomStyleProperty<int> _size_property = new CustomStyleProperty<int>("--size");

        #endregion

        private VisualElement _hueWheelHandle;
        private SaturationLuminanceQuad _saturationLuminanceQuad;

        private Color _color;
        private int _hue;
        private float _saturation;
        private float _luminance;

        private float _hueWheelRadius;
        private float _handleSize;
        private float _innerWheelRadius;

        #endregion


        // ######################## INITS ######################## //

        #region CONSTRUCTORS

        public ColorWheel()
        {
            Init();
        }

        public ColorWheel(Action<Color> onColorChangedCallback)
        {
            OnColorChanged += onColorChangedCallback;
            Init();
        }

        #endregion

        #region INITS

        private void Init()
        {
            // add hue wheel
            VisualElement hueWheel = new VisualElement {name = "hueWheel"};
            hueWheel.style.backgroundImage = Resources.Load<Texture2D>("Images/HueWheel");
            hueWheel.style.position = Position.Absolute;
            hueWheel.style.top = 0;
            hueWheel.style.bottom = 0;
            hueWheel.style.left = 0;
            hueWheel.style.right = 0;
            Add(hueWheel);

            // add hue wheel handle
            _hueWheelHandle = new VisualElement {name = "handle"};
            _hueWheelHandle.style.backgroundImage = Resources.Load<Texture2D>("Images/ColorWheelHandle");
            _hueWheelHandle.style.position = Position.Absolute;
            hueWheel.Add(_hueWheelHandle);

            // add quad for saturation and luminance
            _saturationLuminanceQuad = new SaturationLuminanceQuad();
            _saturationLuminanceQuad.OnValueChanged += OnSaturationLuminanceChanged;
            Add(_saturationLuminanceQuad);

            // register callbacks
            RegisterCallback<CustomStyleResolvedEvent>(OnCustomStyleResolved);
            RegisterCallback<GeometryChangedEvent>(InitialUpdate);

            // add hue wheel manipulator
            DragManipulator hueWheelManipulator = new DragManipulator(new ManipulatorActivationFilter {button = MouseButton.LeftMouse}, true, _hueWheelHandle, delta => OnHueManipulatorChanged());
            hueWheelManipulator.OnActivated += evt => OnHueManipulatorChanged();
            this.AddManipulator(hueWheelManipulator);
        }

        private void InitialUpdate(GeometryChangedEvent evt)
        {
            UnregisterCallback<GeometryChangedEvent>(InitialUpdate);
            style.height = layout.width;

            CalculcateSizes(layout.width);
            DisplayCurrentColor();
        }

        private void CalculcateSizes(float hueWheelSize, float innerRadiusOffset = 10)
        {
            _hueWheelRadius = hueWheelSize * 0.5f;
            // the handle should be as thick as the hue wheel. In the image used, the thickness of the wheel is exactly 9.375% of the total width of the image
            _handleSize = hueWheelSize * 0.09375f;
            _innerWheelRadius = _hueWheelRadius - _handleSize - innerRadiusOffset;

            _hueWheelHandle.style.width = _handleSize;
            _hueWheelHandle.style.height = _handleSize;


            // the saturation luminance quad should be centered and fill out the inner radius
            Vector2 center = new Vector2(_hueWheelRadius, _hueWheelRadius);
            Vector2 min = center + new Vector2((_innerWheelRadius) * Mathf.Cos(Mathf.Deg2Rad * -135), (_innerWheelRadius) * Mathf.Sin(Mathf.Deg2Rad * -135));
            Vector2 max = center + new Vector2((_innerWheelRadius) * Mathf.Cos(Mathf.Deg2Rad * 45), (_innerWheelRadius) * Mathf.Sin(Mathf.Deg2Rad * 45));
            _saturationLuminanceQuad.style.left = min.x;
            _saturationLuminanceQuad.style.top = min.y;
            _saturationLuminanceQuad.style.width = max.x - min.x;
            _saturationLuminanceQuad.style.height = max.y - min.y;
        }

        #endregion


        // ######################## FUNCTIONALITY ######################## //

        #region FUNCTIONALITY

        #region EVENT_HANDLING

        private void OnCustomStyleResolved(CustomStyleResolvedEvent evt)
        {
            ICustomStyle styles = evt.customStyle;

            if (styles.TryGetValue(_size_property, out int value))
            {
                style.width = value;
                style.height = value;
                CalculcateSizes(value);
            }
        }

        private void OnSaturationLuminanceChanged(Vector2 saturationLuminance)
        {
            _saturation = saturationLuminance.x * 100f;
            _luminance = saturationLuminance.y * 100f;

            UpdateColor();
        }

        private void OnHueManipulatorChanged()
        {
            _hue = GetHueFromHandle();
            PositionHueWheelHandle(_hue);

            _saturationLuminanceQuad.Hue = _hue;

            UpdateColor();
        }

        #endregion

        /// <summary>
        /// calculates the current color from the HSL values and notifies everyone that it changed
        /// </summary>
        private void UpdateColor()
        {
            _color = ColorUtility.HSL(Mathf.RoundToInt(_hue), Mathf.RoundToInt(_saturation), Mathf.RoundToInt(_luminance));
            OnColorChanged?.Invoke(_color);
        }

        /// <summary>
        /// Returns the hue from the position of the hue wheel handle
        /// </summary>
        /// <returns></returns>
        private int GetHueFromHandle()
        {
            Vector2 center = new Vector2(_hueWheelRadius, _hueWheelRadius);

            // calculate the position of the handle center
            Vector2 centerPosition = new Vector2(_hueWheelHandle.style.left.value.value + _hueWheelHandle.layout.width * 0.5f,
                _hueWheelHandle.style.top.value.value + _hueWheelHandle.layout.height * 0.5f);

            // translate the position to the origin so its angle can be calculated
            centerPosition -= center;

            // get the angle and transform it into degrees
            float angle = Mathf.Atan2(centerPosition.y, centerPosition.x);
            angle *= Mathf.Rad2Deg;

            // we need to reverse the angle to get our hue and make sure it is not less than 0
            angle *= -1;
            if (angle < 0)
                angle += 360;

            // return the hue
            return Mathf.RoundToInt(angle);
        }

        /// <summary>
        /// Positions the hue wheel handle on the hue wheel from a given hue
        /// </summary>
        /// <param name="hue"></param>
        private void PositionHueWheelHandle(float hue)
        {
            // we need radiands
            hue *= Mathf.Deg2Rad;

            // polar to cartesian conversion
            Vector2 cartesianPosition = new Vector2((_hueWheelRadius - _handleSize * 0.5f) * Mathf.Cos(-hue), (_hueWheelRadius - _handleSize * 0.5f) * Mathf.Sin(-hue));

            // translate rotation origin to wheel center
            Vector2 center = new Vector2(_hueWheelRadius, _hueWheelRadius);
            cartesianPosition += center;

            // set position
            _hueWheelHandle.style.left = cartesianPosition.x - _handleSize * 0.5f;
            _hueWheelHandle.style.top = cartesianPosition.y - _handleSize * 0.5f;
        }

        /// <summary>
        /// Sets hue wheel and saturationLuminanceQuad to display the current values
        /// </summary>
        private void DisplayCurrentColor()
        {
            PositionHueWheelHandle(_hue);
            _saturationLuminanceQuad.Hue = _hue;
            _saturationLuminanceQuad.Saturation = _saturation / 100f;
            _saturationLuminanceQuad.Luminance = _luminance / 100f;
        }

        public override bool ContainsPoint(Vector2 localPoint)
        {
            Vector2 center = new Vector2(_hueWheelRadius, _hueWheelRadius);
            float dist = Vector2.Distance(localPoint, center);
            return dist <= _hueWheelRadius && dist >= _innerWheelRadius;
        }

        #endregion
    }
}