using UnityEngine;

namespace FK.Utility
{
	/// <summary>
	/// <para>A Class for Hosting Coroutines. Whenever you need an extra MonoBehaviour to run a Coroutine on, you can use the Instance Property of this Host</para>
	///
	/// v1.0 09/2018
	/// Written by Fabian Kober
	/// fabian-kober@gmx.net
	/// </summary>
	public class CoroutineHost : MonoBehaviour
	{
		// ######################## PROPERTIES ######################## //
		/// <summary>
		/// The Instance used to run Coroutines. Initializes itself if no Instance exists yet
		/// </summary>
		public static CoroutineHost Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new GameObject("<Master_of_Coroutines>", typeof(CoroutineHost)).GetComponent<CoroutineHost>();
				}

				return _instance;
			}
		}

		// ######################## PRIVATE VARS ######################## //
		private static CoroutineHost _instance;

		// ######################## UNITY EVENT FUNCTIONS ######################## //
		void Awake()
		{
			if(_instance == null)
			{
				_instance = this;
			} else if(Instance != this)
			{
				if(Application.isEditor)
				{
					DestroyImmediate(this);
				} else
				{
					Destroy(this);
				}

				Debug.LogWarning("Tried to Instantiate second instance of Coroutine Host. Additional Instance was destroyed.");
			}
		}

		private void OnDestroy()
		{
			if(_instance == this)
			{
				_instance = null;
			}
		}
	}
}