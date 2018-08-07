using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace FK.Utility.UI
{
    /// <summary>
    /// <para>Forces the Handles of the Scrollbars of a ScrollRect to have a fixed size</para>
    /// 
    /// v1.0 06/2018
    /// Written by Fabian Kober
    /// fabian-kober@gmx.net
    /// </summary>
    [RequireComponent(typeof(ScrollRect))]
    public class ScrollbarHandleSizeFixer : MonoBehaviour
    {
        // ######################## PUBLIC VARS ######################## //
        public float Size = 0f;

        // ######################## PRIVATE VARS ######################## //
        private Scrollbar _scrollbarVertical;
        private Scrollbar _scrollbarHorizontal;



        // ######################## UNITY START & UPDATE ######################## //
        void Awake() { Init(); }


        // ######################## INITS ######################## //
        /// <summary>Does the init for this behaviour</summary>
        private void Init()
        {
            // get the scrollbars
            ScrollRect sr = GetComponent<ScrollRect>();
            _scrollbarHorizontal = sr.horizontalScrollbar;
            _scrollbarVertical = sr.verticalScrollbar;

            // add listener
            sr.onValueChanged.AddListener(x => FixSize());

            // fix size initially
            StartCoroutine(CoFixSize());
        }

        // ######################## FUNCTIONALITY ######################## //
        private void FixSize()
        {
            if (_scrollbarHorizontal)
                _scrollbarHorizontal.size = Size;

            if (_scrollbarVertical)
                _scrollbarVertical.size = Size;
        }

        // ######################## COROUTINES ######################## //
        private IEnumerator CoFixSize()
        {
            yield return null;
            FixSize();
        }
    }
}
