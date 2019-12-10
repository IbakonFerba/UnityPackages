using FK.Utility;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using ColorUtility = FK.Utility.ColorUtility;

namespace FK.UIElements
{
    /// <summary>
    /// Interface for objects that can be bound to the color picker window
    /// </summary>
    public interface IColorPickerBindable
    {
        void SetColor(Color color);
    }

    /// <summary>
    /// <para>A custom color picker window supporting HSLA and RGBA colors</para>
    ///
    /// v1.0 12/2019
    /// Written by Fabian Kober
    /// fabian-kober@gmx.net
    /// </summary>
    public class ColorPickerWindow : EditorWindow
    {
        // ######################## ENUMS & DELEGATES ######################## //

        #region ENUMS & DELEGATES

        /// <summary>
        /// All available modes for the sliders
        /// </summary>
        private enum SliderMode
        {
            HSL,
            RGB
        }

        #endregion


        // ######################## EVENTS ######################## //

        #region EVENTS

        public static event System.Action<Color> OnColorChanged;

        #endregion


        // ######################## PRIVATE VARS ######################## //

        #region PRIVATE VARS

        /// <summary>
        /// The original color when the window was opened
        /// </summary>
        private static Color _originalColor;

        /// <summary>
        /// The latest selected color
        /// </summary>
        private static Color _currentColor;

        private SliderMode _mode;

        private ColorComparisonSwatch _colorComparisonSwatch;
        private ColorWheel _hlsWheel;
        private VisualElement _sliderContainer;

        /// <summary>
        /// {R, G, B} in RGB mode and {H, S, L} in HSL mode
        /// </summary>
        private readonly ColorSlider[] _colorSliders = new ColorSlider[3];

        private ColorSlider _alphaSlider;
        private TextField _hexField;

        private int _prevHue;
        private int _prevSat;
        private int _prevLum;

        #endregion


        // ######################## INITS ######################## //

        #region INITS

        /// <summary>
        /// Opens the color window
        /// </summary>
        /// <param name="originalColor">Initial color that should be remembered and shown in the Color Swatch</param>
        /// <param name="colorChangedCallback">Callback that is invoked when the color changes</param>
        /// <param name="showAlpha">If TRUE an alpha slider is displayed</param>
        public static void Open(Color originalColor, System.Action<Color> colorChangedCallback, bool showAlpha)
        {
            OnColorChanged = colorChangedCallback;
            _originalColor = originalColor;

            // open the window
            ColorPickerWindow wnd = GetWindow<ColorPickerWindow>();
            wnd.titleContent = new GUIContent("Color Picker");

            wnd.OnOpen(showAlpha);
        }

        /// <summary>
        /// Opens the color window
        /// </summary>
        /// <param name="originalColor">Initial color that should be remembered and shown in the Color Swatch</param>
        /// <param name="boundObject">An object that should be bound to the window</param>
        /// <param name="showAlpha">If TRUE an alpha slider is displayed</param>
        public static void Open(Color originalColor, IColorPickerBindable boundObject, bool showAlpha)
        {
            Open(originalColor, boundObject.SetColor, showAlpha);
        }

        /// <summary>
        /// Called after the window was openend
        /// </summary>
        /// <param name="showAlpha">If TRUE an alpha slider is displayed</param>
        private void OnOpen(bool showAlpha)
        {
            // show or hide the alphab slider
            if (showAlpha && _sliderContainer.hierarchy.IndexOf(_alphaSlider) == -1)
                _sliderContainer.Add(_alphaSlider);
            else if (!showAlpha && _sliderContainer.hierarchy.IndexOf(_alphaSlider) != -1)
                _alphaSlider.RemoveFromHierarchy();

            // set original color in swatch
            _colorComparisonSwatch.Color1 = _originalColor;

            // set the original color as the current color
            SetColor(_originalColor);
        }

        #endregion


        // ######################## UNITY EVENT FUNCTIONS ######################## //

        #region UNITY EVENT FUNCTIONS

        public void OnEnable()
        {
            // show as an utility window with a fixed size
            ShowUtility();
            minSize = new Vector2(300, 600);
            maxSize = minSize;

            // Import UXML
            VisualTreeAsset visualTreeAsset = Resources.Load<VisualTreeAsset>("ColorPickerWindow_layout");
            visualTreeAsset.CloneTree(rootVisualElement);

            // load styles
            rootVisualElement.styleSheets.Add(Resources.Load<StyleSheet>("ColorPickerWindow_styles"));
            rootVisualElement.styleSheets.Add(Resources.Load<StyleSheet>("Styles"));

            // get elements
            EnumField modeField = rootVisualElement.Q<EnumField>("modeField");
            _colorComparisonSwatch = rootVisualElement.Q<ColorComparisonSwatch>();
            _hlsWheel = rootVisualElement.Q<ColorWheel>();
            _sliderContainer = rootVisualElement.Q("sliderContainer");
            _colorSliders[0] = rootVisualElement.Q<ColorSlider>("colorSlider1");
            _colorSliders[1] = rootVisualElement.Q<ColorSlider>("colorSlider2");
            _colorSliders[2] = rootVisualElement.Q<ColorSlider>("colorSlider3");
            _hexField = rootVisualElement.Q<TextField>("hexField");

            // add alpha slider
            _alphaSlider = new ColorSlider();
            _alphaSlider.AddToClassList("color-slider");
            _alphaSlider.label = "A";
            _alphaSlider.lowValue = 0;
            _alphaSlider.highValue = 1;

            // register callbacks
            _colorComparisonSwatch.OnColorClicked += SetColor;
            _hlsWheel.OnColorChanged += UpdateColorFromHslWheel;
            _colorSliders[0].RegisterValueChangedCallback(UpdateColorFromSlider0);
            _colorSliders[1].RegisterValueChangedCallback(UpdateColorFromSlider1);
            _colorSliders[2].RegisterValueChangedCallback(UpdateColorFromSlider2);
            _alphaSlider.RegisterValueChangedCallback(UpdateAlpha);
            _hexField.RegisterCallback<FocusOutEvent>(UpdateColorFromHex);

            // initialize with hsl mode
            SetSliderMode(SliderMode.HSL);
            UpdateSliders();

            // initialize the mode field with the current mode and register to its callback
            modeField.Init(_mode);
            modeField.RegisterValueChangedCallback(evt => SetSliderMode((SliderMode) evt.newValue));
        }

        private void OnDisable()
        {
            // unregister any listeners
            OnColorChanged = null;
        }

        #endregion


        // ######################## FUNCTIONALITY ######################## //

        #region FUNCTIONALITY

        #region EVENT_HANDLING

        /// <summary>
        /// Called when the hsl wheel updates
        /// </summary>
        /// <param name="newColor"></param>
        private void UpdateColorFromHslWheel(Color newColor)
        {
            UpdatePrevColor();

            // make sure alpha is kept and set the color
            newColor.a = _currentColor.a;
            _currentColor = newColor;

            // update display
            UpdateColorComparisionSwatch();
            UpdateSliders();
            UpdateHexColor();

            OnColorChanged?.Invoke(_currentColor);
        }

        /// <summary>
        /// Called when the hex field updates
        /// </summary>
        /// <param name="evt"></param>
        private void UpdateColorFromHex(FocusOutEvent evt)
        {
            UpdatePrevColor();

            // make sure the color string starts with a #
            string colorString = _hexField.value;
            if (!colorString.StartsWith("#"))
                colorString = colorString.Insert(0, "#");

            // if the string is not valid, reset to the previous color
            if (!UnityEngine.ColorUtility.TryParseHtmlString(colorString, out Color colorFromHex))
            {
                _hexField.SetValueWithoutNotify(UnityEngine.ColorUtility.ToHtmlStringRGB(_currentColor));
                return;
            }

            // set color
            _currentColor = colorFromHex;

            // update display
            UpdateColorComparisionSwatch();
            UpdateHslWheel();
            UpdateSliders();

            OnColorChanged?.Invoke(_currentColor);
        }


        /// <summary>
        /// Called When slider 0 updates (H or R)
        /// </summary>
        /// <param name="value"></param>
        private void UpdateColorFromSlider0(ChangeEvent<float> value)
        {
            UpdatePrevColor();
            switch (_mode)
            {
                case SliderMode.HSL:
                    // calculate color
                    _currentColor = ColorUtility.HSL((int) value.newValue, _prevSat, _prevLum, _currentColor.a);

                    // update other sliders
                    int hue = GetHue();
                    int sat = GetSat();
                    int lum = GetLum();
                    UpdateSaturationSlider(hue, sat, lum);
                    UpdateLuminanceSlider(hue, sat, lum);
                    break;
                case SliderMode.RGB:
                    // calculate color
                    _currentColor = ColorUtility.RGB((int) value.newValue, (int) (_currentColor.g * 255), (int) (_currentColor.b * 255), (int) (_currentColor.a * 255));

                    // update other sliders
                    UpdateGreenSlider();
                    UpdateBlueSlider();
                    break;
            }

            DoCommonUpdateFromSliders();
        }

        /// <summary>
        /// Called When slider 1 updates (S or G)
        /// </summary>
        /// <param name="value"></param>
        private void UpdateColorFromSlider1(ChangeEvent<float> value)
        {
            UpdatePrevColor();
            switch (_mode)
            {
                case SliderMode.HSL:
                    // calculate color
                    _currentColor = ColorUtility.HSL(_prevHue, (int) value.newValue, _prevLum, _currentColor.a);

                    // update other sliders
                    UpdateLuminanceSlider(GetHue(), GetSat(), GetLum());
                    break;
                case SliderMode.RGB:
                    // calculate color
                    _currentColor = ColorUtility.RGB((int) (_currentColor.r * 255), (int) value.newValue, (int) (_currentColor.b * 255), (int) (_currentColor.a * 255));

                    // update other sliders
                    UpdateRedSlider();
                    UpdateBlueSlider();
                    break;
            }

            DoCommonUpdateFromSliders();
        }

        /// <summary>
        /// Called When slider 0 updates (L or B)
        /// </summary>
        /// <param name="value"></param>
        private void UpdateColorFromSlider2(ChangeEvent<float> value)
        {
            UpdatePrevColor();
            switch (_mode)
            {
                case SliderMode.HSL:
                    // calculate color
                    _currentColor = ColorUtility.HSL(_prevHue, _prevSat, (int) value.newValue, _currentColor.a);

                    // update other sliders
                    UpdateSaturationSlider(GetHue(), GetSat(), GetLum());
                    break;
                case SliderMode.RGB:
                    // calculate color
                    _currentColor = ColorUtility.RGB((int) (_currentColor.r * 255), (int) (_currentColor.g * 255), (int) value.newValue, (int) (_currentColor.a * 255));

                    // update other sliders
                    UpdateRedSlider();
                    UpdateGreenSlider();
                    break;
            }

            DoCommonUpdateFromSliders();
        }

        /// <summary>
        /// Called when the alpha slider updates
        /// </summary>
        /// <param name="evt"></param>
        private void UpdateAlpha(ChangeEvent<float> evt)
        {
            _currentColor.a = evt.newValue;

            // update display
            UpdateColorComparisionSwatch();

            OnColorChanged?.Invoke(_currentColor);
        }

        #endregion

        /// <summary>
        /// Sets the current color and updates all displays
        /// </summary>
        /// <param name="newColor"></param>
        public void SetColor(Color newColor)
        {
            UpdatePrevColor();

            _currentColor = newColor;

            // update display
            UpdateColorComparisionSwatch();
            UpdateHslWheel();
            UpdateSliders();
            UpdateHexColor();

            OnColorChanged?.Invoke(_currentColor);
        }

        /// <summary>
        /// Switches the mode the the requested one and changes the sliders accordingly
        /// </summary>
        /// <param name="mode"></param>
        private void SetSliderMode(SliderMode mode)
        {
            Debug.Log(mode);
            _mode = mode;

            switch (mode)
            {
                case SliderMode.HSL:
                    // Hue slider
                    _colorSliders[0].label = "H";
                    _colorSliders[0].SetGradientColors(new Color(1, 0, 0), new Color(1, 1, 0), new Color(0, 1, 0), new Color(0, 1, 1), new Color(0, 0, 1), new Color(1, 0, 1), new Color(1, 0, 0));
                    _colorSliders[0].lowValue = 0;
                    _colorSliders[0].highValue = 360;
                    _colorSliders[0].value = GetSat();

                    // saturation slider
                    _colorSliders[1].label = "S";
                    _colorSliders[1].lowValue = 0;
                    _colorSliders[1].highValue = 100;
                    _colorSliders[1].value = GetSat();

                    // luminance slider
                    _colorSliders[2].label = "L";
                    _colorSliders[2].lowValue = 0;
                    _colorSliders[2].highValue = 100;
                    _colorSliders[2].value = GetLum();
                    break;
                case SliderMode.RGB:
                    // red slider
                    _colorSliders[0].label = "R";
                    _colorSliders[0].lowValue = 0;
                    _colorSliders[0].highValue = 255;

                    // green slider
                    _colorSliders[1].label = "G";
                    _colorSliders[1].lowValue = 0;
                    _colorSliders[1].highValue = 255;

                    // blue slider
                    _colorSliders[2].label = "B";
                    _colorSliders[2].lowValue = 0;
                    _colorSliders[2].highValue = 255;
                    break;
            }

            UpdateSliders();
        }

        /// <summary>
        /// Updates the prev values for HSL
        /// </summary>
        private void UpdatePrevColor()
        {
            _prevHue = GetHue();
            _prevSat = GetSat();
            _prevLum = GetLum();
        }

        #region DISPLAY_UPDATE

        /// <summary>
        /// All the common updating after a slider changed
        /// </summary>
        private void DoCommonUpdateFromSliders()
        {
            // update display
            UpdateColorComparisionSwatch();
            UpdateHslWheel();
            UpdateAlphaSlider();
            UpdateHexColor();

            OnColorChanged?.Invoke(_currentColor);
        }

        /// <summary>
        /// Updates the displayed current color in the color comparision swatch
        /// </summary>
        private void UpdateColorComparisionSwatch()
        {
            _colorComparisonSwatch.Color2 = _currentColor;
        }

        /// <summary>
        /// Updates the display for all sliders
        /// </summary>
        private void UpdateSliders()
        {
            switch (_mode)
            {
                case SliderMode.HSL:
                    int hue = GetHue();
                    int sat = GetSat();
                    int lum = GetLum();

                    UpdateHueSlider(hue);
                    UpdateSaturationSlider(hue, sat, lum);
                    UpdateLuminanceSlider(hue, sat, lum);
                    break;
                case SliderMode.RGB:

                    UpdateRedSlider();
                    UpdateGreenSlider();
                    UpdateBlueSlider();
                    break;
            }

            UpdateAlphaSlider();
        }

        /// <summary>
        /// Updates the color displayed in the Hsl wheel
        /// </summary>
        private void UpdateHslWheel()
        {
            _hlsWheel.Color = _currentColor;
        }

        /// <summary>
        /// Updates the displayed hex color
        /// </summary>
        private void UpdateHexColor()
        {
            _hexField.SetValueWithoutNotify(UnityEngine.ColorUtility.ToHtmlStringRGB(_currentColor));
        }

        private void UpdateHueSlider(int hue)
        {
            _colorSliders[0].value = hue;
        }

        private void UpdateSaturationSlider(int hue, int sat, int lum)
        {
            _colorSliders[1].value = sat;
            _colorSliders[1].SetGradientColors(ColorUtility.HSL(hue, 0, lum), ColorUtility.HSL(hue, 100, lum));
        }

        private void UpdateLuminanceSlider(int hue, int sat, int lum)
        {
            _colorSliders[2].value = lum;
            _colorSliders[2].SetGradientColors(ColorUtility.HSL(hue, sat, 0), ColorUtility.HSL(hue, sat, 50), ColorUtility.HSL(hue, sat, 100));
        }

        private void UpdateRedSlider()
        {
            _colorSliders[0].value = _currentColor.r * 255;
            _colorSliders[0].SetGradientColors(new Color(0, _currentColor.g, _currentColor.b), new Color(1, _currentColor.g, _currentColor.b));
        }

        private void UpdateGreenSlider()
        {
            _colorSliders[1].value = _currentColor.g * 255;
            _colorSliders[1].SetGradientColors(new Color(_currentColor.r, 0, _currentColor.b), new Color(_currentColor.r, 1, _currentColor.b));
        }

        private void UpdateBlueSlider()
        {
            _colorSliders[2].value = _currentColor.b * 255;
            _colorSliders[2].SetGradientColors(new Color(_currentColor.r, _currentColor.g, 0), new Color(_currentColor.r, _currentColor.g, 1));
        }

        private void UpdateAlphaSlider()
        {
            _alphaSlider.value = _currentColor.a;
            _alphaSlider.SetGradientColors(new Color(_currentColor.r, _currentColor.g, _currentColor.b, 0), new Color(_currentColor.r, _currentColor.g, _currentColor.b, 1));
        }

        #endregion

        #endregion


        // ######################## UTILITIES ######################## //

        #region UTILITIES

        /// <summary>
        /// Returns the last valid hue. If we have saturation and luminance values that don't have a specific hue, this returns the last specific one
        /// </summary>
        /// <returns></returns>
        private int GetHue()
        {
            int sat = _currentColor.Saturation();
            int lum = _currentColor.Luminance();
            if (lum <= 0 || lum >= 100 || sat <= 0)
                return _prevHue;

            return _currentColor.Hue();
        }

        /// <summary>
        /// Returns the last valid saturation. If we have a luminance value that doesn't have a specific saturation, this returns the last specific one
        /// </summary>
        /// <returns></returns>
        private int GetSat()
        {
            int lum = _currentColor.Luminance();
            if (lum <= 0 || lum >= 100)
                return _prevSat;

            return _currentColor.Saturation();
        }

        /// <summary>
        /// Returns the luminance of the current color
        /// </summary>
        /// <returns></returns>
        private int GetLum()
        {
            return _currentColor.Luminance();
        }

        #endregion
    }
}