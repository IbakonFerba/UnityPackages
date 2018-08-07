using UnityEngine;
using UnityEngine.UI;
using FK.Utility.Fading;

namespace FK.Utility.UI
{
    /// <summary>
    /// <para>This class swaps the sprite of a toggle instead of overlaying another graphic</para>
    /// 
    /// v2.1 06/2018
    /// Written by Fabian Kober
    /// fabian-kober@gmx.net
    /// </summary>
    [RequireComponent(typeof(Toggle))]
    public class ToggleSpriteSwapper : MonoBehaviour
    {
        // ######################## PUBLIC VARS ######################## //
        public float FadeDuration = 0f;

        // ######################## PRIVATE VARS ######################## //
        private Toggle _toggle;
        private Graphic _offGraphic;
        private Graphic _onGraphic;

        // ######################## UNITY START & UPDATE ######################## //
        void Awake() { Init(); }

        private void OnEnable()
        {
            SwapSpriteImmediate();
        }


        // ######################## INITS ######################## //
        /// <summary>Does the init for this behaviour</summary>
        private void Init()
        {
            // get references
            _toggle = GetComponent<Toggle>();
            _offGraphic = _toggle.targetGraphic;
            _onGraphic = _toggle.graphic;

            // disable the graphic changing of the toggle
            _toggle.graphic = null;

            // add a listener
            _toggle.onValueChanged.AddListener(SwapSprite);
        }


        // ######################## FUNCTIONALITY ######################## //
        /// <summary>
        /// Swaps the sprite when the Toggle is changed
        /// </summary>
        /// <param name="toggleOn"></param>
        private void SwapSprite(bool toggleOn)
        {
            // don't use coroutines if the Fade Duration is 0
            if (FadeDuration == 0.0f)
            {
                SwapSpriteImmediate();
                return;
            }

            // change the target graphic
            _toggle.targetGraphic = toggleOn ? _onGraphic : _offGraphic;

            // fade
            _offGraphic.Fade(this, !toggleOn, FadeDuration, false);
            _onGraphic.Fade(this, toggleOn, FadeDuration, false);
        }

        /// <summary>
        /// Swaps the sprite immediately without using Coroutines
        /// </summary>
        private void SwapSpriteImmediate()
        {
            // change the target graphic
            _toggle.targetGraphic = _toggle.isOn ? _onGraphic : _offGraphic;

            // change alpha of the Graphics
            Color off = _offGraphic.color;
            Color on = _onGraphic.color;

            off.a = _toggle.isOn ? 0 : 1;
            on.a = 1 - off.a;

            _offGraphic.color = off;
            _onGraphic.color = on;
        }
    }
}
