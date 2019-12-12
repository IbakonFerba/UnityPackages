using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace FK.UIElements
{
    /// <summary>
    /// <para>A slider meant to be used as a channel slider for a color selection</para>
    ///
    /// v1.0 12/2019
    /// Written by Fabian Kober
    /// fabian-kober@gmx.net
    /// </summary>
    public class ColorSlider : Slider
    {
        // ######################## STRUCTS & CLASSES ######################## //

        #region STRUCTS & CLASSES

        public new class UxmlFactory : UxmlFactory<ColorSlider>
        {
        }

        #endregion


        // ######################## PRIVATE VARS ######################## //

        #region PRIVATE VARS

        private readonly Texture2D _gradientTexture;

        #endregion


        // ######################## INITS ######################## //

        #region CONSTRUCTORS

        public ColorSlider()
        {
            // load styles
            styleSheets.Add(Resources.Load<StyleSheet>("ColorSlider_styles"));
            styleSheets.Add(Resources.Load<StyleSheet>("Styles"));

            // create and initialize gradient texture
            _gradientTexture = new Texture2D(2, 1, TextureFormat.RGBA32, false, true)
            {
                hideFlags = HideFlags.HideAndDontSave,
                wrapMode = TextureWrapMode.Clamp,
                filterMode = FilterMode.Bilinear,
                alphaIsTransparency = true
            };

            _gradientTexture.SetPixel(0, 0, Color.clear);
            _gradientTexture.SetPixel(1, 0, Color.clear);
            _gradientTexture.Apply();


            // set up alpha background
            TiledImage background = TiledImage.CreateBackground(Resources.Load<Texture2D>("Images/AlphaCheckerboard"));
            background.AddToClassList("unity-base-slider--horizontal");
            background.AddToClassList("unity-base-slider__tracker");
            background.style.backgroundColor = Color.clear;

            // set as child of the first element in the hierarchy of the slider
            IEnumerator<VisualElement> children = Children().GetEnumerator();
            children.MoveNext();
            children.Current.Add(background);
            children.Dispose();

            background.SendToBack();

            // add a field for the value to be displayed
            IntegerField valueField = new IntegerField();
            valueField.AddToClassList(UssClasses.FILL_HEIGHT);
            valueField.AddToClassList("color-slider-value-field");
            Add(valueField);

            // set up the background of the slider (unity calls this the tracker). This is what holds the gradient texture
            VisualElement tracker = this.Q("unity-tracker");
            tracker.AddToClassList(UssClasses.FULL);
            tracker.AddToClassList(UssClasses.NO_SLICE);
            tracker.style.backgroundImage = _gradientTexture;


            // set up the handle
            VisualElement handle = this.Q("unity-dragger");
            handle.AddToClassList("color-slider-handle");
            handle.AddToClassList(UssClasses.FILL_HEIGHT);
            handle.style.position = Position.Absolute;

#if UNITY_2019_3_OR_NEWER
            // remove handle border
            VisualElement handleBorder = this.Q("unity-dragger-border");
            handleBorder.visible = false;
#endif


            // set up the label
            labelElement.AddToClassList("color-slider-label");
            labelElement.AddToClassList(UssClasses.FILL_HEIGHT);

            // register callbacks
            this.RegisterValueChangedCallback(evt => valueField.SetValueWithoutNotify((int) evt.newValue));
            valueField.RegisterValueChangedCallback(evt =>
            {
                float clampedValue = Mathf.Clamp(evt.newValue, lowValue, highValue);
                valueField.SetValueWithoutNotify((int) clampedValue);
                value = clampedValue;
            });
        }

        #endregion


        // ######################## FUNCTIONALITY ######################## //

        #region FUNCTIONALITY

        /// <summary>
        /// Set the colors of the displayed gradient from left to right
        /// </summary>
        /// <param name="colors"></param>
        public void SetGradientColors(params Color[] colors)
        {
            _gradientTexture.Resize(colors.Length, 1);

            for (int i = 0; i < colors.Length; ++i)
            {
                _gradientTexture.SetPixel(i, 0, colors[i]);
            }

            _gradientTexture.Apply();
        }

        #endregion
    }
}