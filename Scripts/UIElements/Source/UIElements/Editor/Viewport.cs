using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Cursor = UnityEngine.UIElements.Cursor;

namespace FK.UIElements
{
    /// <summary>
    /// <para>A 2d Viewport that can pan and zoom</para>
    ///
    /// v1.1 11/2019
    /// Written by Fabian Kober
    /// fabian-kober@gmx.net
    /// </summary>
    public class Viewport : VisualElement
    {
        // ######################## STRUCTS & CLASSES ######################## //

        #region STRUCTS & CLASSES

        public new class UxmlFactory : UxmlFactory<Viewport, UxmlTraits>
        {
        }

        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            private readonly UxmlFloatAttributeDescription _zoomSpeed = new UxmlFloatAttributeDescription {name = "zoomSpeed", defaultValue = 0.1f};
            private readonly UxmlFloatAttributeDescription _minZoomValue = new UxmlFloatAttributeDescription {name = "minZoom", defaultValue = 0.1f};
            private readonly UxmlFloatAttributeDescription _maxZoomValue = new UxmlFloatAttributeDescription {name = "maxZoom", defaultValue = 5f};

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                Viewport viewport = (Viewport) ve;
                viewport._zoomSpeed = _zoomSpeed.GetValueFromBag(bag, cc);
                viewport._minZoomValue = _minZoomValue.GetValueFromBag(bag, cc);
                viewport._maxZoomValue = _maxZoomValue.GetValueFromBag(bag, cc);

                if (viewport._minZoomValue > viewport._maxZoomValue)
                {
                    Debug.LogWarning($"{AssetDatabase.GetAssetPath(cc.visualTreeAsset)}: Viewports minZoom is bigger than maxZoom!", cc.visualTreeAsset);
                    viewport._minZoomValue = viewport._maxZoomValue;
                }

                viewport.UpdateZoom(0);
            }
        }

        #endregion


        // ######################## EVENTS ######################## //
        public event Action OnUpdated;


        // ######################## PROPERTIES ######################## //

        #region PROPERTIES

        public override VisualElement contentContainer => _contentContainer;

        public Vector2 Position => _contentContainer.layout.position;
        public Vector2 ZoomOrigin => _origin.layout.position;
        public float ZoomValue => _origin.transform.scale.x;

        #endregion


        // ######################## PRIVATE VARS ######################## //

        #region PRIVATE VARS

        #region USS_PROPERTIES

        private static readonly CustomStyleProperty<Texture2D> _background_image_property = new CustomStyleProperty<Texture2D>("--viewport-background-image");

        #endregion

        private readonly VisualElement _origin;
        private readonly VisualElement _contentContainer;

        private TiledImage _backgroundImage;

        #region ZOOM

        private float _zoomSpeed = 0.1f;
        private float _minZoomValue = 0.1f;
        private float _maxZoomValue = 5f;

        #endregion

        #endregion


        // ######################## INITS ######################## //
        public Viewport()
        {
            // load styles
            styleSheets.Add(Resources.Load<StyleSheet>("Styles"));
            AddToClassList(UssClasses.FULL);

            // add origin
            _origin = new VisualElement {name = "origin"};
            hierarchy.Add(_origin);

            // add content container
            _contentContainer = new VisualElement {name = "content"};
            _origin.Add(_contentContainer);

            // add manipulators
            this.AddManipulator(new DragManipulator(new ManipulatorActivationFilter {button = MouseButton.MiddleMouse}, false, _contentContainer, OnDrag));

            // register callbacks
            RegisterCallback<WheelEvent>(OnMouseWheel);
            RegisterCallback<CustomStyleResolvedEvent>(OnCustomStyleResolved);
        }


        // ######################## FUNCTIONALITY ######################## //

        #region FUNCTIONALITY

        #region EVENT HANDLING

        private void OnMouseWheel(WheelEvent evt)
        {
            // do nothing if the mouse is captured
            if (MouseCaptureController.IsMouseCaptured())
                return;

            // update zoom origin
            Vector2 localMousePos = this.ChangeCoordinatesTo(_origin, evt.localMousePosition);
            _origin.style.left = evt.localMousePosition.x;
            _origin.style.top = evt.localMousePosition.y;
            _contentContainer.style.left = _contentContainer.layout.xMin - localMousePos.x;
            _contentContainer.style.top = _contentContainer.layout.yMin - localMousePos.y;

            // calculate zoom value
            int wheelDir = (int) Mathf.Sign(-evt.delta.y);
            float scrollValue = wheelDir * _zoomSpeed;
            
            // zoom
            UpdateZoom(scrollValue);
        }

        private void OnDrag(Vector2 delta)
        {
            MarkDirtyRepaint();
            OnUpdated?.Invoke();
        }

        private void OnCustomStyleResolved(CustomStyleResolvedEvent evt)
        {
            ICustomStyle styles = evt.customStyle;

            if (styles.TryGetValue(_background_image_property, out Texture2D value))
            {
                // if there is no background image create it
                if (_backgroundImage == null)
                {
                    _backgroundImage = TiledImage.CreateBackground(value);
                    _backgroundImage.AddToClassList("viewport-background");

                    hierarchy.Add(_backgroundImage);
                    _backgroundImage.SendToBack();
                    _backgroundImage.StretchToParentSize();

                    _contentContainer.RegisterCallback<GeometryChangedEvent>(e => UpdateBackground());
                }
                else
                {
                    _backgroundImage.SetTexture(value);
                }

                UpdateBackground();
            }
        }

        #endregion

        /// <summary>
        /// Updates the zoom by delta, clamping it between a min and max value
        /// </summary>
        /// <param name="delta"></param>
        private void UpdateZoom(float delta)
        {
            float zoomValue = Mathf.Clamp(_origin.transform.scale.x + delta, _minZoomValue, _maxZoomValue);
            _origin.transform.scale = new Vector3(zoomValue, zoomValue, zoomValue);

            UpdateBackground();
            OnUpdated?.Invoke();
            _contentContainer.MarkDirtyRepaint();
            MarkDirtyRepaint();
        }

        /// <summary>
        /// Sets position and zoom without sending the updated event
        /// </summary>
        /// <param name="position"></param>
        /// <param name="originiPosition"></param>
        /// <param name="zoomValue"></param>
        public void SetPositionAndZoom(Vector2 position, Vector2 originiPosition, float zoomValue)
        {
            _origin.style.left = originiPosition.x;
            _origin.style.top = originiPosition.y;
            _contentContainer.style.left = position.x;
            _contentContainer.style.top = position.y;

            zoomValue = Mathf.Clamp(zoomValue, _minZoomValue, _maxZoomValue);
            _origin.transform.scale = new Vector3(zoomValue, zoomValue, zoomValue);

            _contentContainer.MarkDirtyRepaint();
        }

        private void UpdateBackground()
        {
            if (_backgroundImage == null)
                return;

            float zoomValue = _origin.transform.scale.x;
            _backgroundImage.SetScale(new Vector2(1 / zoomValue, 1 / zoomValue));
            _backgroundImage.SetOffset(_backgroundImage.WorldToLocal(_contentContainer.worldBound.position));
        }

        #endregion
    }
}