using UnityEngine;

namespace FK.Utility
{
    /// <summary>
    /// <para>This class contains functions to map a value between 0 and 1 to different easing functions. Some of them return values bigger than 1 or smaller than 0 during easing, but they all start at 0 and end up at 1.</para>
    /// <para>Created using https://gist.github.com/gre/1650294 and Timothée Groleaus Easing Function Generator (http://www.timotheegroleau.com/Flash/experiments/easing_function_generator.htm)</para>
    ///
    /// v2.1 08/2018
    /// Written by Fabian Kober
    /// fabian-kober@gmx.net
    /// </summary>
    public static class Easing
    {
        #region IN

        /// <summary>
        /// Very smooth in fading that gains speed relatively quickly
        /// </summary>
        /// <param name="progress"></param>
        /// <returns></returns>
        public static float InQuadratic(float progress)
        {
            return progress * progress;
        }

        /// <summary>
        /// Smooth in fading that gains speed relatively quickly, but slower as InQuadratic
        /// </summary>
        /// <param name="progress"></param>
        /// <returns></returns>
        public static float InCubic(float progress)
        {
            return progress * progress * progress;
        }

        /// <summary>
        /// Quite hard in fading that is pretty slow at the beginning and speeds up much towards the end
        /// </summary>
        /// <param name="progress"></param>
        /// <returns></returns>
        public static float InQuartic(float progress)
        {
            return progress * progress * progress * progress;
        }

        /// <summary>
        /// Hardest in fading that is very slow at the beginning and gains a lot of speed towards the end
        /// </summary>
        /// <param name="progress"></param>
        /// <returns></returns>
        public static float InQuintic(float progress)
        {
            return progress * progress * progress * progress * progress;
        }

        #endregion


        #region OUT

        /// <summary>
        /// Very smooth out fading that slows down very soft
        /// </summary>
        /// <param name="progress"></param>
        /// <returns></returns>
        public static float OutQuadratic(float progress)
        {
            return progress * (2 - progress);
        }
        
        /// <summary>
        /// Smooth out fading that slows down soft
        /// </summary>
        /// <param name="progress"></param>
        /// <returns></returns>
        public static float OutCubic(float progress)
        {
            return (--progress)*progress*progress+1;
        }

        /// <summary>
        /// Less smooth out fading than OutCubic that slows down a bit harder
        /// </summary>
        /// <param name="progress"></param>
        /// <returns></returns>
        public static float OutQuartic(float progress)
        {
            return 1-(--progress)*progress*progress*progress;
        }

        /// <summary>
        /// Quite hard out fading that slows down quite hard
        /// </summary>
        /// <param name="progress"></param>
        /// <returns></returns>
        public static float OutQuintic(float progress)
        {
            return 1+(--progress)*progress*progress*progress*progress;
        }

        #endregion


        #region IN_OUT

        /// <summary>
        /// Very smooth in out fading that speeds up and slows down very soft
        /// </summary>
        /// <param name="progress"></param>
        /// <returns></returns>
        public static float InOutQadratic(float progress)
        {
            return progress < .5 ? 2 * progress * progress : -1 + (4 - 2 * progress) * progress;
        }
        
        /// <summary>
        /// Smooth in out fading that speeds up and slows down rather soft
        /// </summary>
        /// <param name="progress"></param>
        /// <returns></returns>
        public static float InOutCubic(float progress)
        {
            return progress < .5 ? 4 * progress * progress * progress : (progress - 1) * (2 * progress - 2) * (2 * progress - 2) + 1;
        }

        /// <summary>
        /// Harder in out fading that speeds up and slows down harder than InOutCubic
        /// </summary>
        /// <param name="progress"></param>
        /// <returns></returns>
        public static float InOutQartic(float progress)
        {
            return progress < .5 ? 8 * progress * progress * progress * progress : 1 - 8 * (--progress) * progress * progress * progress;
        }
        
        /// <summary>
        /// Hardest in out fading that speeds up and slows down very hard
        /// </summary>
        /// <param name="progress"></param>
        /// <returns></returns>
        public static float InOutQuintic(float progress)
        {
            return progress<.5 ? 16*progress*progress*progress*progress*progress : 1+16*(--progress)*progress*progress*progress*progress;
        }

        #endregion


        #region OUT_IN

        /// <summary>
        /// Slows down and speeds up again in the middle in a soft manner
        /// </summary>
        /// <param name="progress"></param>
        /// <returns></returns>
        public static float OutInCubic(float progress)
        {
            return 4 * progress * progress * progress + -6 * progress * progress + 3 * progress;
        }

        #endregion


        #region BACK_IN

        /// <summary>
        /// Goes below 0 in the beginning to then accelerate softly
        /// </summary>
        /// <param name="progress"></param>
        /// <returns></returns>
        public static float BackInCubic(float progress)
        {
            return 4 * progress * progress * progress + -3 * progress * progress;
        }

        /// <summary>
        /// Goes farther below 0 than BackInCubic and accelerates more quickly
        /// </summary>
        /// <param name="progress"></param>
        /// <returns></returns>
        public static float BackInQuartic(float progress)
        {
            return 2 * progress * progress * progress * progress + 2 * progress * progress * progress + -3 * progress * progress;
        }

        #endregion


        #region OUT_BACK

        /// <summary>
        /// Goes fast in the beginning and overshoots 1, then slowly goes down to 1
        /// </summary>
        /// <param name="progress"></param>
        /// <returns></returns>
        public static float OutBackCubic(float progress)
        {
            return 4 * progress * progress * progress + -9 * progress * progress + 6 * progress;
        }

        /// <summary>
        /// Goes very fast in the beginning and overshoots 1 more than OutBackCubic, then slowly goes down to 1
        /// </summary>
        /// <param name="progress"></param>
        /// <returns></returns>
        public static float OutBackQuartic(float progress)
        {
            return -2 * progress * progress * progress * progress + 10 * progress * progress * progress + -15 * progress * progress + 8 * progress;
        }

        #endregion


        #region IN_ELASTIC

        /// <summary>
        /// Oscillates around 0 in the beginning, then goes to 1 quite fast
        /// </summary>
        /// <param name="progress"></param>
        /// <returns></returns>
        public static float InElasticSmall(float progress)
        {
            return 33 * progress * progress * progress * progress * progress + -59 * progress * progress * progress * progress + 32 * progress * progress * progress + -5 * progress * progress;
        }

        /// <summary>
        /// Oscillates around 0 in the beginning stronger than InElasticSmall, then goes to 1 quite fast
        /// </summary>
        /// <param name="progress"></param>
        /// <returns></returns>
        public static float InElasticBig(float progress)
        {
            return 56 * progress * progress * progress * progress * progress + -105 * progress * progress * progress * progress + 60 * progress * progress * progress + -10 * progress * progress;
        }

        #endregion


        #region OUT_ELASTIC

        /// <summary>
        /// Goes to 1 fast, overshoots it and oscillates until it comes to an hold at 1
        /// </summary>
        /// <param name="progress"></param>
        /// <returns></returns>
        public static float OutElasticSmall(float progress)
        {
            return 33 * progress * progress * progress * progress * progress + -106 * progress * progress * progress * progress + 126 * progress * progress * progress + -67 * progress * progress +
                   15 * progress;
        }

        /// <summary>
        /// Goes to 1 fast, overshoots it and oscillates stronger than OutElasticSmall until it comes to an hold at 1
        /// </summary>
        /// <param name="progress"></param>
        /// <returns></returns>
        public static float OutElasticBig(float progress)
        {
            return 56 * progress * progress * progress * progress * progress + -175 * progress * progress * progress * progress + 200 * progress * progress * progress + -100 * progress * progress +
                   20 * progress;
        }

        #endregion

        
        #region CUSTOM
        /// <summary>
        /// This function allows you to Ease using a provided AnimationCurve. Keep in mind than only values between 0 an 1 on the x Axis will be used
        /// </summary>
        /// <param name="curve">The Animation Curve to use</param>
        /// <param name="progress">The progress between 0 and 1</param>
        /// <returns></returns>
        public static float CustomCurve(AnimationCurve curve, float progress)
        {
            return curve.Evaluate(progress);
        }
        #endregion
    }
}