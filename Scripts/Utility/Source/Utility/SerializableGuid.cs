using System;
using UnityEngine;

namespace FK.Utility
{
    /// <summary>
    /// <para>A serializable system.Guid</para>
    ///
    /// v1.1 11/2019
    /// Written by Fabian Kober
    /// fabian-kober@gmx.net
    /// </summary>
    [System.Serializable]
    public struct SerializableGuid : ISerializationCallbackReceiver
    {
        // ######################## PROPERTIES ######################## //
        /// <summary>
        /// The actual System.Guid behind this
        /// </summary>
        public Guid Value => _guid;


        // ######################## PRIVATE VARS ######################## //
        [SerializeField] private string _serializedGuid;
        private Guid _guid;


        // ######################## OPERATORS ######################## //
        #region Operators

        public static implicit operator SerializableGuid(Guid value)
        {
            return new SerializableGuid(value);
        }

        public static bool operator ==(SerializableGuid a, SerializableGuid b)
        {
            return a.Value == b.Value;
        }

        public static bool operator ==(SerializableGuid a, Guid b)
        {
            return a.Value == b;
        }

        public static bool operator ==(Guid a, SerializableGuid b)
        {
            return a == b.Value;
        }

        public static bool operator !=(SerializableGuid a, SerializableGuid b)
        {
            return a.Value != b.Value;
        }

        public static bool operator !=(SerializableGuid a, Guid b)
        {
            return a.Value != b;
        }

        public static bool operator !=(Guid a, SerializableGuid b)
        {
            return a != b.Value;
        }

        #endregion


        // ######################## UNITY EVENT FUNCTIONS ######################## //
        public void OnBeforeSerialize()
        {
            _serializedGuid = _guid.ToString();
        }

        public void OnAfterDeserialize()
        {
            _guid = Guid.Parse(_serializedGuid);
        }


        // ######################## INITS ######################## //
        public SerializableGuid(Guid value)
        {
            _serializedGuid = "";
            _guid = value;
        }


        // ######################## FUNCTIONALITY ######################## //
        public int CompareTo(Guid value)
        {
            return Value.CompareTo(value);
        }

        public int CompareTo(SerializableGuid value)
        {
            return Value.CompareTo(value.Value);
        }

        public int CompareTo(System.Object value)
        {
            return Value.CompareTo(value);
        }

        public bool Equals(Guid value)
        {
            return Value.Equals(value);
        }

        public bool Equals(SerializableGuid value)
        {
            return Value.Equals(value.Value);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is SerializableGuid))
                return false;
            
            return Value.Equals(((SerializableGuid)obj).Value);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}