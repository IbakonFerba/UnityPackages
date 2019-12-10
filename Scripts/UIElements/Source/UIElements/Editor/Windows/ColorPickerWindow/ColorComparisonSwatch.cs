using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace FK.UIElements
{
    /// <summary>
    /// <para>A field that displays two colors next to each other and returns either one of them on click</para>
    ///
    /// v1.0 12/2019
    /// Written by Fabian Kober
    /// fabian-kober@gmx.net
    /// </summary>
    public class ColorComparisonSwatch : VisualElement
    {
        // ######################## STRUCTS & CLASSES ######################## //

        #region STRUCTS & CLASSES

        public new class UxmlFactory : UxmlFactory<ColorComparisonSwatch>
        {
        }

        #endregion


        // ######################## EVENTS ######################## //

        #region EVENTS

        public event Action<Color> OnColorClicked;

        #endregion


        // ######################## PROPERTIES ######################## //

        #region PROPERTIES

        public Color Color1
        {
            get => _color1;
            set
            {
                _color1 = value;
                _color1Element.style.backgroundColor = _color1;
            }
        }

        public Color Color2
        {
            get => _color2;
            set
            {
                _color2 = value;
                _color2Element.style.backgroundColor = _color2;
            }
        }

        #endregion


        // ######################## PRIVATE VARS ######################## //

        #region PRIVATE VARS

        private readonly VisualElement _color1Element;
        private readonly VisualElement _color2Element;

        private readonly TiledImage _background;

        private Color _color1;
        private Color _color2;

        #endregion


        // ######################## INITS ######################## //

        #region CONSTRUCTORS

        public ColorComparisonSwatch()
        {
            // load styles
            styleSheets.Add(Resources.Load<StyleSheet>("Styles"));
            AddToClassList(UssClasses.ROW);

            // add alpha background
            _background = TiledImage.CreateBackground(Resources.Load<Texture2D>("Images/AlphaCheckerboard"));
            Add(_background);

            // add color 1 element
            _color1Element = new VisualElement {name = "color1"};
            _color1Element.AddToClassList(UssClasses.FULL_FLEX);
            Add(_color1Element);

            // add color 2 element
            _color2Element = new VisualElement {name = "color2"};
            _color2Element.AddToClassList(UssClasses.FULL_FLEX);
            Add(_color2Element);


            // make colors clickable
            _color1Element.AddManipulator(new Clickable(() => OnColorClicked?.Invoke(Color1)));
            _color2Element.AddManipulator(new Clickable(() => OnColorClicked?.Invoke(Color2)));

            // add callbacks
            this.RegisterCallback<GeometryChangedEvent>(Init);
        }

        #endregion

        #region INITS

        private void Init(GeometryChangedEvent evt)
        {
            this.UnregisterCallback<GeometryChangedEvent>(Init);

            // set up background rounded corners
            _background.BorderTopLeftRadius = resolvedStyle.borderTopLeftRadius;
            _background.BorderTopRightRadius = resolvedStyle.borderTopRightRadius;
            _background.BorderBottomRightRadius = resolvedStyle.borderBottomRightRadius;
            _background.BorderBottomLeftRadius = resolvedStyle.borderBottomLeftRadius;
        }

        #endregion
    }
}