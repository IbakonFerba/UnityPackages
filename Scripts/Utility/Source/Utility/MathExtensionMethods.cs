using UnityEngine;

namespace FK.Utility.MathExtensions
{
    /// <summary>
    /// Extensions for math operations on math types
    /// </summary>
    public static class MathExtensionMethods
    {
        /// <summary>
        /// Remaps a value in a specified range to another range
        /// </summary>
        /// <param name="value">The value to remap</param>
        /// <param name="from1">Low end of original range</param>
        /// <param name="to1">High and of original range</param>
        /// <param name="from2">Low end of remapped Range</param>
        /// <param name="to2">High end of remapped Range</param>
        /// <returns></returns>
        public static float Remap(this float value, float from1, float to1, float from2, float to2)
        {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }


        /// <summary>
        /// Formats a float as a time
        /// </summary>
        /// <param name="seconds"></param>
        /// <param name="format">This string defines the formating. You can use the following keywords seperated by colons: mil - milliseconds, sec - seconds, min - minutes, hr - hours. If you want the time to tisplay minutes and seconds, this string should be "min:sec"</param>
        /// <returns></returns>
        public static string FormatAsTime(this float seconds, string format)
        {
            string[] times = format.Split(':');

            string formattetTime = "";
            for(int i = 0; i < times.Length; ++i)
            {
                string time = times[i];
                switch(time)
                {
                    case "mil":
                        formattetTime += Mathf.FloorToInt((seconds % 1) * 100).ToString("00");
                        break;
                    case "sec":
                        formattetTime += Mathf.FloorToInt(seconds % 60).ToString("00");
                        break;
                    case "min":
                        formattetTime += Mathf.FloorToInt((seconds / 60) % 60).ToString("00");
                        break;
                    case "hr":
                        formattetTime += Mathf.FloorToInt((seconds / 60) / 60).ToString("00");
                        break;
                }

                if (i < times.Length - 1)
                    formattetTime += ":";
            }

            return formattetTime;
        }
    }
}
