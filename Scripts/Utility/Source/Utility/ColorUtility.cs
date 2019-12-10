using UnityEngine;

namespace FK.Utility
{
	/// <summary>
	/// <para>Utility functions for colors</para>
	///
	/// v1.1 12/2019
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
			return new Color(HSLtoRGBValueConversion(0, h, s, l), HSLtoRGBValueConversion(8, h, s, l), HSLtoRGBValueConversion(4, h, s, l), a);
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

		/// <summary>
		/// Returns the hue of a color between 0 and 360
		/// </summary>
		/// <param name="color"></param>
		/// <returns></returns>
		public static int Hue(this Color color)
		{
			float cMin = Mathf.Min(color.r, color.g, color.b);
			float cMax = Mathf.Max(color.r, color.g, color.b);
			float delta = cMax - cMin;

			float hue = 0;
			if (delta == 0f)
				hue = 0;
			else if (cMax == color.r)
				hue = ((color.g - color.b) / delta) % 6;
			else if (cMax == color.g)
				hue = (color.b - color.r) / delta + 2;
			else
				hue = (color.r - color.g) / delta + 4;

			hue = Mathf.RoundToInt(hue * 60f);
			if (hue < 0)
				hue += 360;

			return (int)hue;
		}
		
		/// <summary>
		/// Returns the HSL saturation of a color between 0 and 100
		/// </summary>
		/// <param name="color"></param>
		/// <returns></returns>
		public static int Saturation(this Color color)
		{
			float cMin = Mathf.Min(color.r, color.g, color.b);
			float cMax = Mathf.Max(color.r, color.g, color.b);
			float delta = cMax - cMin;
			
			return Mathf.RoundToInt((delta == 0 ? 0 : delta / (1 - Mathf.Abs(2f * (color.Luminance()/100f) - 1))) * 100);
		}

		/// <summary>
		/// Returns the HSL luminance of a color between 0 and 100
		/// </summary>
		/// <param name="color"></param>
		/// <returns></returns>
		public static int Luminance(this Color color)
		{
			float cMin = Mathf.Min(color.r, color.g, color.b);
			float cMax = Mathf.Max(color.r, color.g, color.b);

			return Mathf.RoundToInt(((cMax + cMin) / 2) * 100);
		}
	}
}
