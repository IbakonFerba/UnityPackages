using UnityEditor.UIElements;

[assembly: UxmlNamespacePrefix("FK.UIElements", "fk")]

namespace FK.UIElements
{
    /// <summary>
    /// Predefined USS Classes as constants
    /// </summary>
    public static class UssClasses
    {
        /// <summary>
        /// The float class makes an object positioned absolute
        /// </summary>
        public const string FLOAT = "float";

        /// <summary>
        /// The full class makes an object fill in its parent completele
        /// </summary>
        public const string FULL = "full";

        /// <summary>
        /// Makes a Visual Element appear rounded by using the border radius
        /// </summary>
        public const string ROUNDED = "rounded";

        /// <summary>
        /// Makes the container a flex row container
        /// </summary>
        public const string ROW = "row";

        /// <summary>
        /// Makes the container a flex reverse row container
        /// </summary>
        public const string ROW_REVERSE = "row-reverse";

        /// <summary>
        /// Makes the element take up as much space as available in a flex container
        /// </summary>
        public const string FULL_FLEX = "full-flex";

        /// <summary>
        /// Makes the element expand to fill all available width
        /// </summary>
        public const string FILL_WIDHT = "fill-width";

        /// <summary>
        /// Makes the element expand to fill all available height
        /// </summary>
        public const string FILL_HEIGHT = "fill-height";

        /// <summary>
        /// Sets all slicing values for the background image to 0
        /// </summary>
        public const string NO_SLICE = "no-slice";
    }
}