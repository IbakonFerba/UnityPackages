using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace FK.Utility.UI
{
    /// <summary>
    /// <para>Swaps the sprite of an selectable when it is selected, hovered over or clicked</para>
    /// 
    /// v1.0 06/2018
    /// Written by Fabian Kober
    /// fabian-kober@gmx.net
    /// </summary>
    [RequireComponent(typeof(Selectable))]
    public class SpriteSwapper : EventTrigger
    {
        // ######################## LINKS TO UNITY OBJECTS ######################## //
        public Sprite HighlightedSprite;
        public Sprite PressedSprite;
        public Sprite DisabledSprite;

        // ######################## PRIVATE VARS ######################## //
        private Image _targetGraphic;
        private Sprite _normalSprite;
        private Selectable _selectable;

        private bool _selected;
        private bool _interactable;



        // ######################## UNITY START & UPDATE ######################## //
        void Awake() { Init(); }

        void Update()
        {
            // if the selectable is disabled but we have not set the disabled sprite yet, set it
            if (!_selectable.interactable && _interactable)
            {
                _targetGraphic.sprite = DisabledSprite;
                _interactable = false;
            }
            else if (_selectable.interactable && !_interactable) // if the selectable is enabled but we still have the disabled sprite set, set the correct sprite
            {
                _targetGraphic.sprite = _selected ? HighlightedSprite : _normalSprite;
                _interactable = true;
            }
        }



        // ######################## INITS ######################## //
        /// <summary>Does the init for this behaviour</summary>
        private void Init()
        {
            _selectable = GetComponent<Selectable>();
            _targetGraphic = _selectable.targetGraphic.GetComponent<Image>();

            _normalSprite = _targetGraphic.sprite;
            _interactable = !_selectable.interactable;
        }

        private void OnEnable()
        {
            Deselect();
        }


        // ######################## FUNCTIONALITY ######################## //
        public override void OnSelect(BaseEventData eventData)
        {
            Select();
        }

        public override void OnDeselect(BaseEventData eventData)
        {
            Deselect();
        }

        public override void OnPointerEnter(PointerEventData data)
        {
            Select();
        }

        public override void OnPointerExit(PointerEventData data)
        {
            if (data.selectedObject == null || data.selectedObject.GetComponent<Selectable>() != _selectable)
            {
                Deselect();
            }
        }

        public override void OnPointerDown(PointerEventData data)
        {
            if (!_interactable)
                return;

            _targetGraphic.sprite = PressedSprite;
        }


        public override void OnPointerUp(PointerEventData data)
        {
            if (!_interactable)
                return;

            _targetGraphic.sprite = _selected ? HighlightedSprite : _normalSprite;
        }


        // ######################## UTILITIES ######################## //
        private void Select()
        {
            _selected = true;

            if (!_interactable)
                return;

            _targetGraphic.sprite = HighlightedSprite;
        }

        private void Deselect()
        {
            _selected = false;

            if (!_interactable)
                return;

            _targetGraphic.sprite = _normalSprite;
        }
    }
}