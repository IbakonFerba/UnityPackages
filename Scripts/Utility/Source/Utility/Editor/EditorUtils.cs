using UnityEngine;

namespace FK.Editor
{
	/// <summary>
	/// <para>General utilities for Editor Scripting</para>
	///
	/// v1.0 09/2018
	/// Written by Fabian Kober
	/// fabian-kober@gmx.net
	/// </summary>
	public static class EditorUtils
	{
		/// <summary>
		/// Returns a 1x1 colored Texture to use as a Backround for GUI Elements
		/// </summary>
		/// <param name="col"></param>
		/// <returns></returns>
		public static Texture2D GetBackgroundColor(Color col)
		{
			Texture2D tex = new Texture2D(1, 1);
			tex.SetPixel(0,0,col);
			tex.Apply();

			return tex;
		}
	}
}