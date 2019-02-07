using System.Collections;
using UnityEngine;
using System;
using UnityEngine.UI;

namespace FK.Utility.Fading
{
    /// <summary>
    /// <para>An Assortment of Methods that allow you to easily Fade in and out different kinds of Objects</para>
    ///
    /// v2.5 02/2019
    /// Written by Fabian Kober
    /// fabian-kober@gmx.net
    /// </summary>
    public static class FadingMethods
    {
        public const string COROUTINE_TAG = "Fade";
        
        public static void StopAllFades()
        {
            CoroutineHost.StopAllTrackedCoroutines(COROUTINE_TAG);
        }
        
        #region CANVAS_GROUP

        /// <summary>
        /// Fades a Canvas Group in or out
        /// </summary>
        /// <param name="group">The Group to fade</param>
        /// <param name="fadeIn">Fade In?</param>
        /// <param name="duration">Amount of seconds the fading should take</param>
        /// <param name="deactivate">Should the GameObject be deactivated when done?</param>
        /// <param name="finished">Callback for when the Fading is finished</param>
        /// <param name="progressMapping">Function for mapping the progress, takes one float argument between 0 and 1 and should return a float between 0 and 1</param>
        /// <returns></returns>
        private static IEnumerator FadeCanvasGroup(CanvasGroup group, bool fadeIn, float duration, bool deactivate, Action finished, Func<float, float> progressMapping)
        {
            float progress = 0;
            while (progress < 1)
            {
                float mappedProgress = progressMapping?.Invoke(progress) ?? progress;
                group.alpha = fadeIn ? mappedProgress : 1 - mappedProgress;
                yield return null;
                progress += Time.deltaTime / duration;
            }

            group.alpha = fadeIn ? 1 : 0;

            finished?.Invoke();

            if (deactivate)
                group.gameObject.SetActive(false);
        }

        /// <summary>
        /// Fades a Canvas Group in or out
        /// </summary>
        /// <param name="group">The Group to fade</param>
        /// <param name="fadeIn">Fade In?</param>
        /// <param name="duration">Amount of seconds the fading should take</param>
        /// <param name="manageActive">If true the gameobject is activated or deactivated automatically</param>
        /// <param name="finished">Callback for when the Fading is finished</param>
        public static Coroutine Fade(this CanvasGroup group, bool fadeIn, float duration, bool manageActive, Action finished = null)
        {
            if (manageActive && fadeIn)
                group.gameObject.SetActive(true);

            CoroutineHost.StopTrackedCoroutine(group, COROUTINE_TAG);
            return CoroutineHost.StartTrackedCoroutine(FadeCanvasGroup(group, fadeIn, duration, manageActive && !fadeIn, finished, null), group, COROUTINE_TAG);
        }

        /// <summary>
        /// Fades a Canvas Group in or out
        /// </summary>
        /// <param name="group">The Group to fade</param>
        /// <param name="fadeIn">Fade In?</param>
        /// <param name="duration">Amount of seconds the fading should take</param>
        /// <param name="manageActive">If true the gameobject is activated or deactivated automatically</param>
        /// <param name="progressMapping">Function for mapping the progress, takes one float argument between 0 and 1 and should return a float between 0 and 1</param>
        /// <param name="finished">Callback for when the Fading is finished</param>
        /// <returns></returns>
        public static Coroutine Fade(this CanvasGroup group, bool fadeIn, float duration, bool manageActive, Func<float, float> progressMapping, Action finished = null)
        {
            if (manageActive && fadeIn)
                group.gameObject.SetActive(true);

            CoroutineHost.StopTrackedCoroutine(group, COROUTINE_TAG);
            return CoroutineHost.StartTrackedCoroutine(FadeCanvasGroup(group, fadeIn, duration, manageActive && !fadeIn, finished, progressMapping), group, COROUTINE_TAG);
        }

        #endregion

        #region GRAPHIC

        /// <summary>
        /// Lerps the Color of a Graphic
        /// </summary>
        /// <param name="graphic">The graphic to set the Color to</param>
        /// <param name="target">The Color to lerp to</param>
        /// <param name="duration">Amount of seconds the lerp should take</param>
        /// <param name="deactivate">Should the GameObject be deactivated when done?</param>
        /// <param name="finished">Callback for when the Fading is finished</param>
        /// <param name="progressMapping">Function for mapping the progress, takes one float argument between 0 and 1 and should return a float between 0 and 1</param>
        /// <returns></returns>
        public static IEnumerator LerpColor(Graphic graphic, Color target, float duration, bool deactivate, Action finished, Func<float, float> progressMapping)
        {
            Color start = graphic.color;

            float progress = 0;
            while (progress < 1)
            {
                float mappedProgress = progressMapping?.Invoke(progress) ?? progress;
                graphic.color = Color.LerpUnclamped(start, target, mappedProgress);
                yield return null;
                progress += Time.deltaTime / duration;
            }

            graphic.color = target;

            finished?.Invoke();

            if (deactivate)
                graphic.gameObject.SetActive(false);
        }

        /// <summary>
        /// Fades a graphic in or out
        /// </summary>
        /// <param name="graphic">The graphic to fade</param>
        /// <param name="fadeIn">Fade In?</param>
        /// <param name="duration">Amount of seconds the fading should take</param>
        /// <param name="manageActive">If true the gameobject is activated or deactivated automatically</param>
        /// <param name="finished">Callback for when the Fading is finished</param>
        /// <returns></returns>
        public static Coroutine Fade(this Graphic graphic, bool fadeIn, float duration, bool manageActive, Action finished = null)
        {
            if (manageActive && fadeIn)
                graphic.gameObject.SetActive(true);

            Color target = graphic.color;
            if (fadeIn)
            {
                target.a = 0;
                graphic.color = target;
            }

            target.a = fadeIn ? 1 : 0;

            CoroutineHost.StopTrackedCoroutine(graphic, COROUTINE_TAG);
            return CoroutineHost.StartTrackedCoroutine(LerpColor(graphic, target, duration, manageActive && !fadeIn, finished, null), graphic, COROUTINE_TAG);
        }

        /// <summary>
        /// Fades a graphic in or out
        /// </summary>
        /// <param name="graphic">The graphic to fade</param>
        /// <param name="fadeIn">Fade In?</param>
        /// <param name="duration">Amount of seconds the fading should take</param>
        /// <param name="manageActive">If true the gameobject is activated or deactivated automatically</param>
        /// <param name="progressMapping">Function for mapping the progress, takes one float argument between 0 and 1 and should return a float between 0 and 1</param>
        /// <param name="finished">Callback for when the Fading is finished</param>
        /// <returns></returns>
        public static Coroutine Fade(this Graphic graphic, bool fadeIn, float duration, bool manageActive, Func<float, float> progressMapping, Action finished = null)
        {
            if (manageActive && fadeIn)
                graphic.gameObject.SetActive(true);

            Color target = graphic.color;
            if (fadeIn)
            {
                target.a = 0;
                graphic.color = target;
            }

            target.a = fadeIn ? 1 : 0;

            CoroutineHost.StopTrackedCoroutine(graphic, COROUTINE_TAG);
            return CoroutineHost.StartTrackedCoroutine(LerpColor(graphic, target, duration, manageActive && !fadeIn, finished, progressMapping), graphic, COROUTINE_TAG);
        }
        
        public static void StopFade(this Graphic graphic)
        {
            CoroutineHost.StopAllTrackedCoroutines(graphic, COROUTINE_TAG);
        }

        #endregion

        #region FILL_IMAGE

        /// <summary>
        /// Fills an Image and sets it type to fill if it is not yet fill
        /// </summary>
        /// <param name="img">The Image to fill</param>
        /// <param name="fillIn">Fill In or Out?</param>
        /// <param name="duration">The Amount of time in seconds the filling will take</param>
        /// <param name="method">The Fill Method</param>
        /// <param name="clockwise">Fill clockwise?</param>
        /// <param name="deactivate">Should the GameObject be deactivated when done?</param>
        /// <param name="finished">Callback for when the Fading is finished</param>
        /// <param name="progressMapping">Function for mapping the progress, takes one float argument between 0 and 1 and should return a float between 0 and 1</param>
        /// <returns></returns>
        private static IEnumerator FillImage(Image img, bool fillIn, float duration, Image.FillMethod method, bool clockwise, bool deactivate, Action finished, Func<float, float> progressMapping)
        {
            if (img.type != Image.Type.Filled)
                img.type = Image.Type.Filled;
            img.fillMethod = method;
            img.fillClockwise = clockwise;

            float progress = 0.0f;
            while (progress < 1.0f)
            {
                float mappedProgress = progressMapping?.Invoke(progress) ?? progress;

                img.fillAmount = fillIn ? mappedProgress : 1 - mappedProgress;
                yield return null;
                progress += Time.deltaTime / duration;
            }

            img.fillAmount = fillIn ? 1 : 0;

            finished?.Invoke();

            if (deactivate)
                img.gameObject.SetActive(false);
        }

        /// <summary>
        /// Fills an Image and sets it type to fill if it is not yet fill
        /// </summary>
        /// <param name="img">The Image to fill</param>
        /// <param name="fillIn">Fill In or Out?</param>
        /// <param name="duration">The Amount of time in seconds the filling will take</param>
        /// <param name="manageActive">If true the gameobject is activated or deactivated automatically</param>
        /// <param name="finished">Callback for when the Fading is finished</param>
        /// <returns></returns>
        public static Coroutine Fill(this Image img, bool fillIn, float duration, bool manageActive, Action finished = null)
        {
            if (manageActive && fillIn)
                img.gameObject.SetActive(true);

            CoroutineHost.StopTrackedCoroutine(img, COROUTINE_TAG);
            return CoroutineHost.StartTrackedCoroutine(FillImage(img, fillIn, duration, img.fillMethod, img.fillClockwise, manageActive && !fillIn, finished, null), img, COROUTINE_TAG);
        }

        /// <summary>
        /// Fills an Image and sets it type to fill if it is not yet fill
        /// </summary>
        /// <param name="img">The Image to fill</param>
        /// <param name="fillIn">Fill In or Out?</param>
        /// <param name="duration">The Amount of time in seconds the filling will take</param>
        /// <param name="manageActive">If true the gameobject is activated or deactivated automatically</param>
        /// <param name="finished">Callback for when the Fading is finished</param>
        /// <param name="progressMapping">Function for mapping the progress, takes one float argument between 0 and 1 and should return a float between 0 and 1</param>
        /// <returns></returns>
        public static Coroutine Fill(this Image img, bool fillIn, float duration, bool manageActive, Func<float, float> progressMapping, Action finished = null)
        {
            if (manageActive && fillIn)
                img.gameObject.SetActive(true);

            CoroutineHost.StopTrackedCoroutine(img, COROUTINE_TAG);
            return CoroutineHost.StartTrackedCoroutine(FillImage(img, fillIn, duration, img.fillMethod, img.fillClockwise, manageActive && !fillIn, finished, progressMapping), img, COROUTINE_TAG);
        }

        /// <summary>
        /// Fills an Image and sets it type to fill if it is not yet fill
        /// </summary>
        /// <param name="img">The Image to fill</param>
        /// <param name="fillIn">Fill In or Out?</param>
        /// <param name="duration">The Amount of time in seconds the filling will take</param>
        /// <param name="manageActive">If true the gameobject is activated or deactivated automatically</param>
        /// <param name="method">The Fill Method</param>
        /// <param name="clockwise">Fill clockwise?</param>
        /// <param name="finished">Callback for when the Fading is finished</param>
        /// <returns></returns>
        public static Coroutine Fill(this Image img, bool fillIn, float duration, bool manageActive, Image.FillMethod method, bool clockwise, Action finished = null)
        {
            if (manageActive && fillIn)
                img.gameObject.SetActive(true);

            CoroutineHost.StopTrackedCoroutine(img, COROUTINE_TAG);
            return CoroutineHost.StartTrackedCoroutine(FillImage(img, fillIn, duration, method, clockwise, manageActive && !fillIn, finished, null), img, COROUTINE_TAG);
        }

        /// <summary>
        /// Fills an Image and sets it type to fill if it is not yet fill
        /// </summary>
        /// <param name="img">The Image to fill</param>
        /// <param name="fillIn">Fill In or Out?</param>
        /// <param name="duration">The Amount of time in seconds the filling will take</param>
        /// <param name="manageActive">If true the gameobject is activated or deactivated automatically</param>
        /// <param name="method">The Fill Method</param>
        /// <param name="clockwise">Fill clockwise?</param>
        /// <param name="finished">Callback for when the Fading is finished</param>
        /// <param name="progressMapping">Function for mapping the progress, takes one float argument between 0 and 1 and should return a float between 0 and 1</param>
        /// <returns></returns>
        public static Coroutine Fill(this Image img, bool fillIn, float duration, bool manageActive, Image.FillMethod method, bool clockwise, Func<float, float> progressMapping,
            Action finished = null)
        {
            if (manageActive && fillIn)
                img.gameObject.SetActive(true);

            CoroutineHost.StopTrackedCoroutine(img, COROUTINE_TAG);
            return CoroutineHost.StartTrackedCoroutine(FillImage(img, fillIn, duration, method, clockwise, manageActive && !fillIn, finished, progressMapping), img, COROUTINE_TAG);
        }
        
        public static void StopFill(this Image image)
        {
            CoroutineHost.StopAllTrackedCoroutines(image, COROUTINE_TAG);
        }

        #endregion

        #region MATERIAL

        /// <summary>
        /// Lerps the Color of a material
        /// </summary>
        /// <param name="material">The material to set the Color to</param>
        /// <param name="target">The Color to lerp to</param>
        /// <param name="duration">Amount of seconds the lerp should take</param>
        /// <param name="finished">Callback for when the Fading is finished</param>
        /// <param name="progressMapping">Function for mapping the progress, takes one float argument between 0 and 1 and should return a float between 0 and 1</param>
        /// <returns></returns>
        public static IEnumerator LerpColor(Material material, Color target, float duration, Action finished, Func<float, float> progressMapping)
        {
            Color start = material.color;

            float progress = 0;
            while (progress < 1)
            {
                float mappedProgress = progressMapping?.Invoke(progress) ?? progress;
                material.color = Color.LerpUnclamped(start, target, mappedProgress);
                yield return null;
                progress += Time.deltaTime / duration;
            }

            material.color = target;

            finished?.Invoke();
        }

        /// <summary>
        /// Fades a Material in or out
        /// </summary>
        /// <param name="material">The Material to fade</param>
        /// <param name="fadeIn">Fade In?</param>
        /// <param name="duration">Amount of seconds the fading should take</param>
        /// <param name="finished">Callback for when the Fading is finished</param>
        /// <returns></returns>
        public static Coroutine Fade(this Material material, bool fadeIn, float duration, Action finished = null)
        {
            Color target = material.color;
            if (fadeIn)
            {
                target.a = 0;
                material.color = target;
            }

            target.a = fadeIn ? 1 : 0;

            CoroutineHost.StopTrackedCoroutine(material, COROUTINE_TAG);
            return CoroutineHost.StartTrackedCoroutine(LerpColor(material, target, duration, finished, null), material, COROUTINE_TAG);
        }

        /// <summary>
        /// Fades a Material in or out
        /// </summary>
        /// <param name="material">The Material to fade</param>
        /// <param name="fadeIn">Fade In?</param>
        /// <param name="duration">Amount of seconds the fading should take</param>
        /// <param name="progressMapping">Function for mapping the progress, takes one float argument between 0 and 1 and should return a float between 0 and 1</param>
        /// <param name="finished">Callback for when the Fading is finished</param>
        /// <returns></returns>
        public static Coroutine Fade(this Material material, bool fadeIn, float duration, Func<float, float> progressMapping, Action finished = null)
        {
            Color target = material.color;
            if (fadeIn)
            {
                target.a = 0;
                material.color = target;
            }

            target.a = fadeIn ? 1 : 0;

            CoroutineHost.StopTrackedCoroutine(material, COROUTINE_TAG);
            return CoroutineHost.StartTrackedCoroutine(LerpColor(material, target, duration, finished, progressMapping), material, COROUTINE_TAG);
        }
        
        public static void StopFade(this Material mat)
        {
            CoroutineHost.StopAllTrackedCoroutines(mat, COROUTINE_TAG);
        }
        

        #endregion

        #region RENDERER

        /// <summary>
        /// Lerps the Color of a Renderer
        /// </summary>
        /// <param name="rend">The Renderer to set the Color to</param>
        /// <param name="targets">The Colors to lerp to</param>
        /// <param name="duration">Amount of seconds the lerp should take</param>
        /// <param name="deactivate">Should the GameObject be deactivated when done?</param>
        /// <param name="finished">Callback for when the Fading is finished</param>
        /// <param name="progressMapping">Function for mapping the progress, takes one float argument between 0 and 1 and should return a float between 0 and 1</param>
        /// <returns></returns>
        public static IEnumerator LerpColor(Renderer rend, Color[] targets, float duration, bool deactivate, Action finished, Func<float, float> progressMapping)
        {
            Color[] start = new Color[rend.materials.Length];

            for (int i = 0; i < rend.materials.Length; ++i)
            {
                start[i] = rend.materials[i].color;
            }

            float progress = 0;
            while (progress < 1)
            {
                float mappedProgress = progressMapping?.Invoke(progress) ?? progress;
                for (int i = 0; i < rend.materials.Length; ++i)
                {
                    rend.materials[i].color = Color.LerpUnclamped(start[i], targets[i], mappedProgress);
                }

                yield return null;
                progress += Time.deltaTime / duration;
            }

            for (int i = 0; i < rend.materials.Length; ++i)
            {
                rend.materials[i].color = targets[i];
            }

            finished?.Invoke();

            if (deactivate)
                rend.gameObject.SetActive(false);
        }

        /// <summary>
        /// Fades a Renderer in or out
        /// </summary>
        /// <param name="rend">The Renderer to fade</param>
        /// <param name="fadeIn">Fade In?</param>
        /// <param name="duration">Amount of seconds the fading should take</param>
        /// <param name="manageActive">If true the gameobject is activated or deactivated automatically</param>
        /// <param name="finished">Callback for when the Fading is finished</param>
        /// <returns></returns>
        public static Coroutine Fade(this Renderer rend, bool fadeIn, float duration, bool manageActive, Action finished = null)
        {
            if (manageActive && fadeIn)
                rend.gameObject.SetActive(true);

            Color[] targets = new Color[rend.materials.Length];
            for (int i = 0; i < rend.materials.Length; ++i)
            {
                targets[i] = rend.materials[i].color;
                if (fadeIn)
                {
                    targets[i].a = 0;
                    rend.materials[i].color = targets[i];
                }

                targets[i].a = fadeIn ? 1 : 0;
            }

            CoroutineHost.StopTrackedCoroutine(rend, COROUTINE_TAG);
            return CoroutineHost.StartTrackedCoroutine(LerpColor(rend, targets, duration, manageActive && !fadeIn, finished, null), rend, COROUTINE_TAG);
        }

        /// <summary>
        /// Fades a Renderer in or out
        /// </summary>
        /// <param name="rend">The Renderer to fade</param>
        /// <param name="fadeIn">Fade In?</param>
        /// <param name="duration">Amount of seconds the fading should take</param>
        /// <param name="manageActive">If true the gameobject is activated or deactivated automatically</param>
        /// <param name="progressMapping">Function for mapping the progress, takes one float argument between 0 and 1 and should return a float between 0 and 1</param>
        /// <param name="finished">Callback for when the Fading is finished</param>
        /// <returns></returns>
        public static Coroutine Fade(this Renderer rend, bool fadeIn, float duration, bool manageActive, Func<float, float> progressMapping, Action finished = null)
        {
            if (manageActive && fadeIn)
                rend.gameObject.SetActive(true);

            Color[] targets = new Color[rend.materials.Length];
            for (int i = 0; i < rend.materials.Length; ++i)
            {
                targets[i] = rend.materials[i].color;
                if (fadeIn)
                {
                    targets[i].a = 0;
                    rend.materials[i].color = targets[i];
                }

                targets[i].a = fadeIn ? 1 : 0;
            }

            CoroutineHost.StopTrackedCoroutine(rend, COROUTINE_TAG);
            return CoroutineHost.StartTrackedCoroutine(LerpColor(rend, targets, duration, manageActive && !fadeIn, finished, progressMapping), rend, COROUTINE_TAG);
        }
        
        public static void StopFade(this Renderer rend)
        {
            CoroutineHost.StopAllTrackedCoroutines(rend, COROUTINE_TAG);
        }

        #endregion

        #region SPRITE

        /// <summary>
        /// Lerps the Color of a sprite
        /// </summary>
        /// <param name="sprite">The material to set the sprite to</param>
        /// <param name="target">The Color to lerp to</param>
        /// <param name="duration">Amount of seconds the lerp should take</param>
        /// <param name="deactivate">Should the GameObject be deactivated when done?</param>
        /// <param name="finished">Callback for when the Fading is finished</param>
        /// <param name="progressMapping">Function for mapping the progress, takes one float argument between 0 and 1 and should return a float between 0 and 1</param>
        /// <returns></returns>
        public static IEnumerator LerpColor(SpriteRenderer sprite, Color target, float duration, bool deactivate, Action finished, Func<float, float> progressMapping)
        {
            Color start = sprite.color;

            float progress = 0;
            while (progress < 1)
            {
                float mappedProgress = progressMapping?.Invoke(progress) ?? progress;
                sprite.color = Color.LerpUnclamped(start, target, mappedProgress);
                yield return null;
                progress += Time.deltaTime / duration;
            }

            sprite.color = target;

            finished?.Invoke();

            if (deactivate)
                sprite.gameObject.SetActive(false);
        }

        /// <summary>
        /// Fades a Sprite in or out
        /// </summary>
        /// <param name="sprite">The Sprite to fade</param>
        /// <param name="fadeIn">Fade In?</param>
        /// <param name="duration">Amount of seconds the fading should take</param>
        /// <param name="manageActive">If true the gameobject is activated or deactivated automatically</param>
        /// <param name="finished">Callback for when the Fading is finished</param>
        /// <returns></returns>
        public static Coroutine Fade(this SpriteRenderer sprite, bool fadeIn, float duration, bool manageActive, Action finished = null)
        {
            if (manageActive && fadeIn)
                sprite.gameObject.SetActive(true);

            Color target = sprite.color;
            if (fadeIn)
            {
                target.a = 0;
                sprite.color = target;
            }

            target.a = fadeIn ? 1 : 0;

            CoroutineHost.StopTrackedCoroutine(sprite, COROUTINE_TAG);
            return CoroutineHost.StartTrackedCoroutine(LerpColor(sprite, target, duration, manageActive && !fadeIn, finished, null), sprite, COROUTINE_TAG);
        }

        /// <summary>
        /// Fades a Sprite in or out
        /// </summary>
        /// <param name="sprite">The Sprite to fade</param>
        /// <param name="fadeIn">Fade In?</param>
        /// <param name="duration">Amount of seconds the fading should take</param>
        /// <param name="manageActive">If true the gameobject is activated or deactivated automatically</param>
        /// <param name="progressMapping">Function for mapping the progress, takes one float argument between 0 and 1 and should return a float between 0 and 1</param>
        /// <param name="finished">Callback for when the Fading is finished</param>
        /// <returns></returns>
        public static Coroutine Fade(this SpriteRenderer sprite, bool fadeIn, float duration, bool manageActive, Func<float, float> progressMapping, Action finished = null)
        {
            if (manageActive && fadeIn)
                sprite.gameObject.SetActive(true);

            Color target = sprite.color;
            if (fadeIn)
            {
                target.a = 0;
                sprite.color = target;
            }

            target.a = fadeIn ? 1 : 0;

            CoroutineHost.StopTrackedCoroutine(sprite, COROUTINE_TAG);
            return CoroutineHost.StartTrackedCoroutine(LerpColor(sprite, target, duration, manageActive && !fadeIn, finished, progressMapping), sprite, COROUTINE_TAG);
        }
        
        public static void StopFade(this SpriteRenderer sprite)
        {
            CoroutineHost.StopAllTrackedCoroutines(sprite, COROUTINE_TAG);
        }

        #endregion

        #region AUDIO  

        /// <summary>
        /// Fades the Volume of an Audio Source
        /// </summary>
        /// <param name="source">The Audio Source to fade</param>
        /// <param name="targetVolume">The volume to fade to</param>
        /// <param name="duration">Amount of time in seconds the Fade should take</param>
        /// <param name="deactivate">Should the GameObject be deactivated when done?</param>
        /// <param name="progressMapping">A delegate function to map a value between 0 and 1 used as the progress for lerping that returns a new value between 0 and 1</param>
        /// <param name="finished">Delegate function that is called when the interpolation is finished</param>
        /// <returns></returns>
        private static IEnumerator FadeVolume(AudioSource source, float targetVolume, float duration, bool deactivate, Func<float, float> progressMapping, Action finished)
        {
            float startVolume = source.volume;
            float clampedTargetVolume = Mathf.Clamp01(targetVolume);

            float progress = 0;
            while (progress < 1)
            {
                float mappedProgress = progressMapping?.Invoke(progress) ?? progress;
                source.volume = Mathf.LerpUnclamped(startVolume, clampedTargetVolume, mappedProgress);
                yield return null;
                progress += Time.deltaTime / duration;
            }

            source.volume = targetVolume;

            finished?.Invoke();

            if (deactivate)
                source.gameObject.SetActive(false);
        }

        /// <summary>
        /// Fades the Volume of an Audio Source to a specified target volume
        /// </summary>
        /// <param name="source">The Audio Source to fade</param>
        /// <param name="targetVolume">The volume to fade to</param>
        /// <param name="duration">Amount of time in seconds the Fade should take</param>
        /// <param name="finished">Delegate function that is called when the interpolation is finished</param>
        /// <returns></returns>
        public static Coroutine FadeVolume(this AudioSource source, float targetVolume, float duration, Action finished = null)
        {
            CoroutineHost.StopTrackedCoroutine(source, COROUTINE_TAG);
            return CoroutineHost.StartTrackedCoroutine(FadeVolume(source, targetVolume, duration, false, null, finished), source, COROUTINE_TAG);
        }

        /// <summary>
        /// Fades the Volume of an Audio Source to a specified target volume
        /// </summary>
        /// <param name="source">The Audio Source to fade</param>
        /// <param name="targetVolume">The volume to fade to</param>
        /// <param name="duration">Amount of time in seconds the Fade should take</param>
        /// <param name="progressMapping">A delegate function to map a value between 0 and 1 used as the progress for lerping that returns a new value between 0 and 1</param>
        /// <param name="finished">Delegate function that is called when the interpolation is finished</param>
        /// <returns></returns>
        public static Coroutine FadeVolume(this AudioSource source, float targetVolume, float duration, Func<float, float> progressMapping, Action finished = null)
        {
            CoroutineHost.StopTrackedCoroutine(source, COROUTINE_TAG);
            return CoroutineHost.StartTrackedCoroutine(FadeVolume(source, targetVolume, duration, false, progressMapping, finished), source, COROUTINE_TAG);
        }

        /// <summary>
        /// Fades the Volume of an Audio Source completely in or out
        /// </summary>
        /// <param name="source">The Audio Source to fade</param>
        /// <param name="fadeIn">If true this fades the Volume to 1, else to 0</param>
        /// <param name="duration">Amount of time in seconds the Fade should take</param>
        /// <param name="manageActive">If true the gameobject is activated or deactivated automatically</param>
        /// <param name="finished">Delegate function that is called when the interpolation is finished</param>
        /// <returns></returns>
        public static Coroutine Fade(this AudioSource source, bool fadeIn, float duration, bool manageActive, Action finished = null)
        {
            if (manageActive && fadeIn)
                source.gameObject.SetActive(true);

            if (fadeIn)
                source.volume = 0;

            CoroutineHost.StopTrackedCoroutine(source, COROUTINE_TAG);
            return CoroutineHost.StartTrackedCoroutine(FadeVolume(source, fadeIn ? 1 : 0, duration, manageActive && !fadeIn, null, finished), source, COROUTINE_TAG);
        }

        /// <summary>
        /// Fades the Volume of an Audio Source completely in or out
        /// </summary>
        /// <param name="source">The Audio Source to fade</param>
        /// <param name="fadeIn">If true this fades the Volume to 1, else to 0</param>
        /// <param name="duration">Amount of time in seconds the Fade should take</param>
        /// <param name="manageActive">If true the gameobject is activated or deactivated automatically</param>
        /// <param name="progressMapping">A delegate function to map a value between 0 and 1 used as the progress for lerping that returns a new value between 0 and 1</param>
        /// <param name="finished">Delegate function that is called when the interpolation is finished</param>
        /// <returns></returns>
        public static Coroutine Fade(this AudioSource source, bool fadeIn, float duration, bool manageActive, Func<float, float> progressMapping, Action finished = null)
        {
            if (manageActive && fadeIn)
                source.gameObject.SetActive(true);

            if (fadeIn)
                source.volume = 0;

            CoroutineHost.StopTrackedCoroutine(source, COROUTINE_TAG);
            return CoroutineHost.StartTrackedCoroutine(FadeVolume(source, fadeIn ? 1 : 0, duration, manageActive && !fadeIn, progressMapping, finished), source, COROUTINE_TAG);
        }
        
        public static void StopFade(this AudioSource source)
        {
            CoroutineHost.StopAllTrackedCoroutines(source, COROUTINE_TAG);
        }

        #endregion
    }
}