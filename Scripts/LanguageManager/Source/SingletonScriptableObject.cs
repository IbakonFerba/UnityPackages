using System.Linq;
using UnityEngine;

namespace FK.Utility
{
	/// <summary>
	/// <para>Base Class for a Scriptable Object Singleton</para>
	///
	/// v1.0 09/2018
	/// Written by Fabian Kober
	/// fabian-kober@gmx.net
	/// </summary>
	public class SingletonScriptableObject<T> : ScriptableObject where T : SingletonScriptableObject<T>
	{
		// ######################## PROPERTIES ######################## //
		public static T Instance
		{
			get
			{
				if (!_instance)
					_instance = Resources.FindObjectsOfTypeAll<T>().FirstOrDefault();
				return _instance;
			}
		}

		// ######################## PRIVATE VARS ######################## //
		private static T _instance;
	}
}