using UnityEngine;
using UnityEngine.UI;

namespace FK.Utility.UI
{
    /// <summary>
    /// <para>Displays the value of a slider on a text component</para>
    /// 
    /// v1.0 06/2018
    /// Written by Fabian Kober
    /// fabian-kober@gmx.net
    /// </summary>
    [RequireComponent(typeof(Slider))]
    public class SliderValueDisplay : MonoBehaviour
    {
        // ######################## LINKS TO UNITY OBJECTS ######################## //
        public Text DisplayText;

        // ######################## PUBLIC VARS ######################## //
        [Tooltip("If the value of the slider should be formatted, enter a string Format here")]
        public string Format;

        // ######################## PRIVATE VARS ######################## //
        private Slider _slider;



        // ######################## UNITY START & UPDATE ######################## //
        void Awake() { Init(); }


        // ######################## INITS ######################## //
        /// <summary>Does the init for this behaviour</summary>
        private void Init()
        {
            _slider = GetComponent<Slider>();
            _slider.onValueChanged.AddListener(x => DisplayText.text = x.ToString(Format));

            // set initial value
            DisplayText.text = _slider.value.ToString(Format);
        }
    }
}
