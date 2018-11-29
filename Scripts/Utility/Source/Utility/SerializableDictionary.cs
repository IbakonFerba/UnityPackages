using System.Collections.Generic;
using UnityEngine;

namespace FK.Utility
{
	/// <summary>
	/// <para>A Serializable Dictionary. To actually be serializable you need to make a non generic implementation of this (because the Unity Serialization System is f***ing annoying)</para>
	///
	/// v1.0 11/2018
	/// Written by Fabian Kober
	/// fabian-kober@gmx.net
	/// </summary>
	[System.Serializable]
	public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
	{
		// ######################## PRIVATE VARS ######################## //
		[SerializeField] private List<TKey> _keys = new List<TKey>();
		[SerializeField] private List<TValue> _values = new List<TValue>();


		// ######################## UNITY EVENT FUNCTIONS ######################## //	
		public void OnBeforeSerialize()
		{
			// make sure we start from clean lists
			_keys.Clear();
			_values.Clear();
			
			// save the keys and values of the dictionary in the lists, because they are serializable
			foreach (KeyValuePair<TKey,TValue> pair in this)
			{
				_keys.Add(pair.Key);
				_values.Add(pair.Value);
			}
		}

		public void OnAfterDeserialize()
		{
			// make sure we start from a clean dictionary
			this.Clear();
			
			// if the keys don't match the values, something is very wrong
			if(_keys.Count != _values.Count)
				throw new System.Exception($"There are {_keys.Count} keys and {_values.Count} values after deserialization. Make sure that both key and value types are serializable!");
			
			// add the key value pairs to the dictionary
			for(int i = 0; i < _keys.Count; ++i)
				this.Add(_keys[i], _values[i]);
		}
	}
}