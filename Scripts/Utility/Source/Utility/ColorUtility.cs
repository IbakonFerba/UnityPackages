using UnityEngine;

namespace FK.Utility
{
	/// <summary>
	/// <para>Utility functions for colors</para>
	///
	/// v1.0 12/2019
	/// Written by Fabian Kober
	/// fabian-kober@gmx.net
	/// </summary>
	public static class ColorUtility
	{
		private const float CONVERSION_DIVIDER = 30f / 360f;
		
		private static float HSLtoRGBValueConversion(float value, float h, float s, float l)
		{
			float k = (value + h / CONVERSION_DIVIDER) % 12;
			float a = s * Mathf.Min(l, 1 - l);

			return l - a * Mathf.Max(Mathf.Min(k - 3, 9 - k, 1), -1);
		}
		
		/// <summary>
		/// Returns a HSL color
		/// </summary>
		/// <param name="h">Hue [0,1]</param>
		/// <param name="s">Saturation [0,1]</param>
		/// <param name="l">Luminance [0,1]</param>
		/// <param name="a">Alpha [0,1]</param>
		/// <returns></returns>
		public static Color HSL(float h, float s, float l, float a = 1)
		{
			return new Color(HSLtoRGBValueConversion(0, h, s, l), HSLtoRGBValueConversion(8, h, s, l), HSLtoRGBValueConversion(4, h, s, l), 1);
		}

		/// <summary>
		/// Returns a HSL color
		/// </summary>
		/// <param name="h">Hue [0,360]</param>
		/// <param name="s">Saturation [0,100]</param>
		/// <param name="l">Luminance [0,100]</param>
		/// <param name="a">Alpha [0,1]</param>
		/// <returns></returns>
		public static Color HSL(int h, int s, int l, float a = 1)
		{
			return HSL(h / 360f, s / 100f, l / 100f, a);
		}

		/// <summary>
		/// Returns a RGB color
		/// </summary>
		/// <param name="r">Red [0,255]</param>
		/// <param name="g">Green [0,255]</param>
		/// <param name="b">Blue [0,255]</param>
		/// <param name="a">Alpha [0,255]</param>
		/// <returns></returns>
		public static Color RGB(int r, int g, int b, int a = 255)
		{
			return new Color(r/255f, g/255f, b/255f, a/255f);
		}
	}
}
