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
    }
}
