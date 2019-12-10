using System;
using UnityEngine;
using UnityEngine.UIElements;


namespace FK.UIElements
{
    /// <summary>
    /// <para>A Button that opens a color picker window</para>
    ///
    /// v1.0 12/2019
    /// Written by Fabian Kober
    /// fabian-kober@gmx.net
    /// </summary>
    public class ColorButton : BaseField<Color>, IColorPickerBindable
    {
        // ######################## STRUCTS & CLASSES ######################## //

        #region STRUCTS & CLASSES

        public new class UxmlFactory : UxmlFactory<ColorButton, UxmlTraits>
        {
        }

        public new class UxmlTraits : BaseFieldTraits<Color, UxmlColorAttributeDescription>
        {
            UxmlBoolAttributeDescription _showColorAsBackgroundColor = new UxmlBoolAttributeDescription {name = "show-color-as-background", defaultValue = true};
            UxmlBoolAttributeDescription _showColorAsBackgroundImageTint = new UxmlBoolAttributeDescription {name = "show-color-as-background-image-tint", defaultValue = true};

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);

                ((ColorButton) ve)._showColorAsBackgroundColor = _showColorAsBackgroundColor.GetValueFromBag(bag, cc);
                ((ColorButton) ve)._showColorAsBackgroundImageTintColor = _showColorAsBackgroundImageTint.GetValueFromBag(bag, cc);
            }
        }

        #endregion


        // ######################## PROPERTIES ######################## //

        #region PROPERTIES

        public Clickable clickable
        {
            get { return _clickable; }
            set
            {
                if (_clickable != null && _clickable.target == this)
                {
                    this.RemoveManipulator(_clickable);
                }

                _clickable = value;

                if (_clickable != null)
                {
                    this.AddManipulator(_clickable);
                }
            }
        }

        public override Color value
        {
            get => base.value;
            set
            {
                base.value = value;
                OnValueChanged(value);
            }
        }

        public bool ShowColorAsBackgroundColor
        {
            get => _showColorAsBackgroundColor;
            set
            {
                _showColorAsBackgroundColor = value;
                OnValueChanged(this.value);
            }
        }

        public bool ShowColorAsBackgroundImageTintColor
        {
            get => _showColorAsBackgroundImageTintColor;
            set
            {
                _showColorAsBackgroundImageTintColor = value;
                OnValueChanged(this.value);
            }
        }

        #endregion


        // ######################## PRIVATE VARS ######################## //

        #region PRIVATE VARS

        private Clickable _clickable;

        private bool _showColorAsBackgroundColor = true;
        private bool _showColorAsBackgroundImageTintColor = true;

        #endregion


        // ######################## INITS ######################## //

        #region CONSTRUCTORS

        public ColorButton() : base(null, null)
        {
            clickable = new Clickable((Action) null);
            clickable.clicked += OnClicked;
        }

        #endregion


        // ######################## FUNCTIONALITY ######################## //

        #region FUNCTIONALITY

        private void OnValueChanged(Color newValue)
        {
            if (_showColorAsBackgroundColor)
                style.backgroundColor = newValue;
            if (_showColorAsBackgroundImageTintColor)
                style.unityBackgroundImageTintColor = newValue;
        }

        private void OnClicked()
        {
            ColorPickerWindow.Open(value, this, false);
        }

        public void SetColor(Color color)
        {
            value = color;
        }

        #endregion
    }
}