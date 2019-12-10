using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace FK.UIElements
{
    /// <summary>
    /// <para>A UI Elements Manipulator for draggable elemenets</para>
    ///
    /// v1.1 12/2019
    /// Written by Fabian Kober
    /// fabian-kober@gmx.net
    /// </summary>
    public class DragManipulator : MouseManipulator
    {
        // ######################## EVENTS ######################## //

        #region EVENTS

        public event Action<MouseDownEvent> OnActivated;
        public event Action<MouseUpEvent> OnDeactivated;
        public event Action<Vector2> OnUpdated;

        #endregion


        // ######################## PROPERTIES ######################## //
        /// <summary>
        /// The start position relative to the move target. This might not be the same as the manipulator target!
        /// </summary>
        public Vector2 StartPosition;


        // ######################## PRIVATE VARS ######################## //
        private bool _active;
        private bool _snapCenterToMouse;

        private VisualElement _moveTarget;


        // ######################## INITS ######################## //

        #region CONSTRUCTORS

        public DragManipulator(ManipulatorActivationFilter activationFilter, bool snapCenterToMouse, Action<Vector2> updateCallback = null)
        {
            activators.Add(activationFilter);
            _active = false;
            _snapCenterToMouse = snapCenterToMouse;
            _moveTarget = null;

            OnUpdated += updateCallback;
        }

        public DragManipulator(ManipulatorActivationFilter activationFilter, bool snapCenterToMouse, VisualElement moveTarget, Action<Vector2> updateCallback = null)
        {
            activators.Add(activationFilter);
            _active = false;
            _snapCenterToMouse = snapCenterToMouse;
            _moveTarget = moveTarget;

            OnUpdated += updateCallback;
        }

        public DragManipulator(ManipulatorActivationFilter activationFilter, bool snapCenterToMouse, VisualElement moveTarget, Action<Vector2> updateCallback, Action<MouseDownEvent> activatedCallback,
            Action<MouseUpEvent> deactivatedCallback)
        {
            activators.Add(activationFilter);
            _active = false;
            _snapCenterToMouse = snapCenterToMouse;

            _moveTarget = moveTarget;

            OnUpdated += updateCallback;
            OnActivated += activatedCallback;
            OnDeactivated += deactivatedCallback;
        }

        #endregion

        // ######################## FUNCTIONALITY ######################## //

        #region FUNCTIONALITY

        protected override void RegisterCallbacksOnTarget()
        {
            target.RegisterCallback<MouseDownEvent>(OnMouseDown);
            target.RegisterCallback<MouseMoveEvent>(OnMouseMove);
            target.RegisterCallback<MouseUpEvent>(OnMouseUp);
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            target.UnregisterCallback<MouseDownEvent>(OnMouseDown);
            target.UnregisterCallback<MouseMoveEvent>(OnMouseMove);
            target.UnregisterCallback<MouseUpEvent>(OnMouseUp);
        }

        #region EVENT HANDLING

        private void OnMouseDown(MouseDownEvent evt)
        {
            if (_active)
            {
                evt.StopImmediatePropagation();
                return;
            }

            if (!target.ContainsPoint(evt.localMousePosition) || !CanStartManipulation(evt))
                return;

            if (_moveTarget == null)
                _moveTarget = target;

            
            if (_snapCenterToMouse)
            {
                StartPosition = new Vector2(_moveTarget.layout.width * 0.5f, _moveTarget.layout.height * 0.5f);
                Vector2 position = _moveTarget.ChangeCoordinatesTo(_moveTarget.parent, target.ChangeCoordinatesTo(_moveTarget, evt.localMousePosition) - StartPosition);
                _moveTarget.style.left = position.x;
                _moveTarget.style.top = position.y;
            }
            else
            {
                StartPosition = target.ChangeCoordinatesTo(_moveTarget, evt.localMousePosition);
            }

            _active = true;
            target.CaptureMouse();
            OnActivated?.Invoke(evt);
            evt.StopPropagation();
        }

        private void OnMouseMove(MouseMoveEvent evt)
        {
            if (!_active || !target.HasMouseCapture())
                return;

            Vector2 delta = target.ChangeCoordinatesTo(_moveTarget, evt.localMousePosition) - StartPosition;
            _moveTarget.style.top = _moveTarget.layout.yMin + delta.y;
            _moveTarget.style.left = _moveTarget.layout.xMin + delta.x;

            OnUpdated?.Invoke(delta);
            evt.StopPropagation();
        }

        private void OnMouseUp(MouseUpEvent evt)
        {
            if (!_active || !target.HasMouseCapture() || !CanStopManipulation(evt))
                return;

            _active = false;
            target.ReleaseMouse();
            OnDeactivated?.Invoke(evt);
            evt.StopPropagation();
        }

        #endregion

        #endregion
    }
}