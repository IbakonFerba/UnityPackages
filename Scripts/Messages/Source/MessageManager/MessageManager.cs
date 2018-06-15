using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace FK.MessageManager
{
    /// <summary>
    ///<para>MessageManager allows you to send Messages to any object that has registered an observer for that message type.
    /// If you are using .NET 4.x these messages can have up to 16 parameters, else they can have up to 4.</para>
    /// 
    /// <para>It is recommended to define your message IDs as const so they can be easily referenced and identified when you read your code.</para>
    /// 
    /// v1.0 06/2018
    /// Written by Fabian Kober
    /// fabian-kober@gmx.net
    /// </summary>
    public static class MessageManager
    {
        // ######################## STRUCTS & CLASSES ######################## //
        #region MESSAGE_COLLECTIONS
        /// <summary>
        /// Interface for a Message Collection witth a variable number of parameters
        /// </summary>
        public interface IMessageCollection
        {
        }

        /// <summary>
        /// Message Collection with a list of message IDs and a delegate associated to each of them
        /// </summary>
        public class MessageCollection : IMessageCollection
        {
            public Dictionary<int, Action> MessageGroups = new Dictionary<int, Action>();
        }

        /// <summary>
        /// Message Collection with a list of message IDs and a delegate associated to each of them
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public class MessageCollection<T> : IMessageCollection
        {
            public Dictionary<int, Action<T>> MessageGroups = new Dictionary<int, Action<T>>();
        }

        /// <summary>
        /// Message Collection with a list of message IDs and a delegate associated to each of them
        /// </summary>
        /// <typeparam name="T0"></typeparam>
        /// <typeparam name="T1"></typeparam>
        public class MessageCollection<T0, T1> : IMessageCollection
        {
            public Dictionary<int, Action<T0, T1>> MessageGroups = new Dictionary<int, Action<T0, T1>>();
        }

        /// <summary>
        /// Message Collection with a list of message IDs and a delegate associated to each of them
        /// </summary>
        /// <typeparam name="T0"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        public class MessageCollection<T0, T1, T2> : IMessageCollection
        {
            public Dictionary<int, Action<T0, T1, T2>> MessageGroups = new Dictionary<int, Action<T0, T1, T2>>();
        }

        /// <summary>
        /// Message Collection with a list of message IDs and a delegate associated to each of them
        /// </summary>
        /// <typeparam name="T0"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        public class MessageCollection<T0, T1, T2, T3> : IMessageCollection
        {
            public Dictionary<int, Action<T0, T1, T2, T3>> MessageGroups = new Dictionary<int, Action<T0, T1, T2, T3>>();
        }

#if NET_4_6
        /// <summary>
        /// Message Collection with a list of message IDs and a delegate associated to each of them
        /// </summary>
        /// <typeparam name="T0"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        public class MessageCollection<T0, T1, T2, T3, T4> : IMessageCollection
        {
            public Dictionary<int, Action<T0, T1, T2, T3, T4>> MessageGroups = new Dictionary<int, Action<T0, T1, T2, T3, T4>>();
        }

        /// <summary>
        /// Message Collection with a list of message IDs and a delegate associated to each of them
        /// </summary>
        /// <typeparam name="T0"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        public class MessageCollection<T0, T1, T2, T3, T4, T5> : IMessageCollection
        {
            public Dictionary<int, Action<T0, T1, T2, T3, T4, T5>> MessageGroups = new Dictionary<int, Action<T0, T1, T2, T3, T4, T5>>();
        }

        /// <summary>
        /// Message Collection with a list of message IDs and a delegate associated to each of them
        /// </summary>
        /// <typeparam name="T0"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <typeparam name="T6"></typeparam>
        public class MessageCollection<T0, T1, T2, T3, T4, T5, T6> : IMessageCollection
        {
            public Dictionary<int, Action<T0, T1, T2, T3, T4, T5, T6>> MessageGroups = new Dictionary<int, Action<T0, T1, T2, T3, T4, T5, T6>>();
        }

        /// <summary>
        /// Message Collection with a list of message IDs and a delegate associated to each of them
        /// </summary>
        /// <typeparam name="T0"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <typeparam name="T6"></typeparam>
        /// <typeparam name="T7"></typeparam>
        public class MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7> : IMessageCollection
        {
            public Dictionary<int, Action<T0, T1, T2, T3, T4, T5, T6, T7>> MessageGroups = new Dictionary<int, Action<T0, T1, T2, T3, T4, T5, T6, T7>>();
        }

        /// <summary>
        /// Message Collection with a list of message IDs and a delegate associated to each of them
        /// </summary>
        /// <typeparam name="T0"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <typeparam name="T6"></typeparam>
        /// <typeparam name="T7"></typeparam>
        /// <typeparam name="T8"></typeparam>
        public class MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7, T8> : IMessageCollection
        {
            public Dictionary<int, Action<T0, T1, T2, T3, T4, T5, T6, T7, T8>> MessageGroups = new Dictionary<int, Action<T0, T1, T2, T3, T4, T5, T6, T7, T8>>();
        }

        /// <summary>
        /// Message Collection with a list of message IDs and a delegate associated to each of them
        /// </summary>
        /// <typeparam name="T0"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <typeparam name="T6"></typeparam>
        /// <typeparam name="T7"></typeparam>
        /// <typeparam name="T8"></typeparam>
        /// <typeparam name="T9"></typeparam>
        public class MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> : IMessageCollection
        {
            public Dictionary<int, Action<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>> MessageGroups = new Dictionary<int, Action<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>>();
        }

        /// <summary>
        /// Message Collection with a list of message IDs and a delegate associated to each of them
        /// </summary>
        /// <typeparam name="T0"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <typeparam name="T6"></typeparam>
        /// <typeparam name="T7"></typeparam>
        /// <typeparam name="T8"></typeparam>
        /// <typeparam name="T9"></typeparam>
        /// <typeparam name="T10"></typeparam>
        public class MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> : IMessageCollection
        {
            public Dictionary<int, Action<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>> MessageGroups = new Dictionary<int, Action<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>>();
        }

        /// <summary>
        /// Message Collection with a list of message IDs and a delegate associated to each of them
        /// </summary>
        /// <typeparam name="T0"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <typeparam name="T6"></typeparam>
        /// <typeparam name="T7"></typeparam>
        /// <typeparam name="T8"></typeparam>
        /// <typeparam name="T9"></typeparam>
        /// <typeparam name="T10"></typeparam>
        /// <typeparam name="T11"></typeparam>
        public class MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> : IMessageCollection
        {
            public Dictionary<int, Action<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>> MessageGroups = new Dictionary<int, Action<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>>();
        }

        /// <summary>
        /// Message Collection with a list of message IDs and a delegate associated to each of them
        /// </summary>
        /// <typeparam name="T0"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <typeparam name="T6"></typeparam>
        /// <typeparam name="T7"></typeparam>
        /// <typeparam name="T8"></typeparam>
        /// <typeparam name="T9"></typeparam>
        /// <typeparam name="T10"></typeparam>
        /// <typeparam name="T11"></typeparam>
        /// <typeparam name="T12"></typeparam>
        public class MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> : IMessageCollection
        {
            public Dictionary<int, Action<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>> MessageGroups = new Dictionary<int, Action<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>>();
        }

        /// <summary>
        /// Message Collection with a list of message IDs and a delegate associated to each of them
        /// </summary>
        /// <typeparam name="T0"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <typeparam name="T6"></typeparam>
        /// <typeparam name="T7"></typeparam>
        /// <typeparam name="T8"></typeparam>
        /// <typeparam name="T9"></typeparam>
        /// <typeparam name="T10"></typeparam>
        /// <typeparam name="T11"></typeparam>
        /// <typeparam name="T12"></typeparam>
        /// <typeparam name="T13"></typeparam>
        public class MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> : IMessageCollection
        {
            public Dictionary<int, Action<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>> MessageGroups = new Dictionary<int, Action<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>>();
        }

        /// <summary>
        /// Message Collection with a list of message IDs and a delegate associated to each of them
        /// </summary>
        /// <typeparam name="T0"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <typeparam name="T6"></typeparam>
        /// <typeparam name="T7"></typeparam>
        /// <typeparam name="T8"></typeparam>
        /// <typeparam name="T9"></typeparam>
        /// <typeparam name="T10"></typeparam>
        /// <typeparam name="T11"></typeparam>
        /// <typeparam name="T12"></typeparam>
        /// <typeparam name="T13"></typeparam>
        /// <typeparam name="T14"></typeparam>
        public class MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> : IMessageCollection
        {
            public Dictionary<int, Action<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>> MessageGroups = new Dictionary<int, Action<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>>();
        }

        /// <summary>
        /// Message Collection with a list of message IDs and a delegate associated to each of them
        /// </summary>
        /// <typeparam name="T0"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <typeparam name="T6"></typeparam>
        /// <typeparam name="T7"></typeparam>
        /// <typeparam name="T8"></typeparam>
        /// <typeparam name="T9"></typeparam>
        /// <typeparam name="T10"></typeparam>
        /// <typeparam name="T11"></typeparam>
        /// <typeparam name="T12"></typeparam>
        /// <typeparam name="T13"></typeparam>
        /// <typeparam name="T14"></typeparam>
        /// <typeparam name="T15"></typeparam>
        public class MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> : IMessageCollection
        {
            public Dictionary<int, Action<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>> MessageGroups = new Dictionary<int, Action<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>>();
        }
#endif
        #endregion


        // ######################## PRIVATE VARS ######################## //
        /// <summary>
        /// All registered observers
        /// </summary>
        private static Dictionary<string, IMessageCollection> _messageCollections = new Dictionary<string, IMessageCollection>();


        // ######################## ADD OBSERVER ######################## //
        #region ADD_OBSERVER
        /// <summary>
        /// Registers the provided Method as an Observer for this Message ID
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="action"></param>
        public static void AddObserver(int messageId, Action action)
        {
            // get the key for _messageCollections from parameter types
            string key = "";
            // if there already is a collection for the given parameter types, add the provided action to it
            if (_messageCollections.ContainsKey(key))
            {
                MessageCollection collection = (MessageCollection)_messageCollections[key];

                if (!collection.MessageGroups.ContainsKey(messageId))
                    collection.MessageGroups.Add(messageId, null);

                collection.MessageGroups[messageId] += action;
            }
            else // create a new collection if there is no collection for these parameter types, then add the provided action to it
            {
                MessageCollection newCollection = new MessageCollection();
                newCollection.MessageGroups.Add(messageId, action);
                _messageCollections.Add(key, newCollection);
            }
        }

        /// <summary>
        /// Registers the provided Method as an Observer for this Message ID
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="messageId"></param>
        /// <param name="action"></param>
        public static void AddObserver<T>(int messageId, Action<T> action)
        {
            // get the key for _messageCollections from parameter types
            string key = typeof(T).FullName.ToString();
            // if there already is a collection for the given parameter types, add the provided action to it
            if (_messageCollections.ContainsKey(key))
            {
                MessageCollection<T> collection = (MessageCollection<T>)_messageCollections[key];

                if (!collection.MessageGroups.ContainsKey(messageId))
                    collection.MessageGroups.Add(messageId, null);

                collection.MessageGroups[messageId] += action;
            }
            else // create a new collection if there is no collection for these parameter types, then add the provided action to it
            {
                MessageCollection<T> newCollection = new MessageCollection<T>();
                newCollection.MessageGroups.Add(messageId, action);
                _messageCollections.Add(key, newCollection);
            }
        }

        /// <summary>
        /// Registers the provided Method as an Observer for this Message ID
        /// </summary>
        /// <typeparam name="T0"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <param name="messageId"></param>
        /// <param name="action"></param>
        public static void AddObserver<T0, T1>(int messageId, Action<T0, T1> action)
        {
            // get the key for _messageCollections from parameter types
            string key = GetKey(typeof(T0), typeof(T1));
            // if there already is a collection for the given parameter types, add the provided action to it
            if (_messageCollections.ContainsKey(key))
            {
                MessageCollection<T0, T1> collection = (MessageCollection<T0, T1>)_messageCollections[key];

                if (!collection.MessageGroups.ContainsKey(messageId))
                    collection.MessageGroups.Add(messageId, null);

                collection.MessageGroups[messageId] += action;
            }
            else // create a new collection if there is no collection for these parameter types, then add the provided action to it
            {
                MessageCollection<T0, T1> newCollection = new MessageCollection<T0, T1>();
                newCollection.MessageGroups.Add(messageId, action);
                _messageCollections.Add(key, newCollection);
            }
        }

        /// <summary>
        /// Registers the provided Method as an Observer for this Message ID
        /// </summary>
        /// <typeparam name="T0"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="messageId"></param>
        /// <param name="action"></param>
        public static void AddObserver<T0, T1, T2>(int messageId, Action<T0, T1, T2> action)
        {
            // get the key for _messageCollections from parameter types
            string key = GetKey(typeof(T0), typeof(T1), typeof(T2));
            // if there already is a collection for the given parameter types, add the provided action to it
            if (_messageCollections.ContainsKey(key))
            {
                MessageCollection<T0, T1, T2> collection = (MessageCollection<T0, T1, T2>)_messageCollections[key];

                if (!collection.MessageGroups.ContainsKey(messageId))
                    collection.MessageGroups.Add(messageId, null);

                collection.MessageGroups[messageId] += action;
            }
            else // create a new collection if there is no collection for these parameter types, then add the provided action to it
            {
                MessageCollection<T0, T1, T2> newCollection = new MessageCollection<T0, T1, T2>();
                newCollection.MessageGroups.Add(messageId, action);
                _messageCollections.Add(key, newCollection);
            }
        }

        /// <summary>
        /// Registers the provided Method as an Observer for this Message ID
        /// </summary>
        /// <typeparam name="T0"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <param name="messageId"></param>
        /// <param name="action"></param>
        public static void AddObserver<T0, T1, T2, T3>(int messageId, Action<T0, T1, T2, T3> action)
        {
            // get the key for _messageCollections from parameter types
            string key = GetKey(typeof(T0), typeof(T1), typeof(T2), typeof(T3));
            // if there already is a collection for the given parameter types, add the provided action to it
            if (_messageCollections.ContainsKey(key))
            {
                MessageCollection<T0, T1, T2, T3> collection = (MessageCollection<T0, T1, T2, T3>)_messageCollections[key];

                if (!collection.MessageGroups.ContainsKey(messageId))
                    collection.MessageGroups.Add(messageId, null);

                collection.MessageGroups[messageId] += action;
            }
            else // create a new collection if there is no collection for these parameter types, then add the provided action to it
            {
                MessageCollection<T0, T1, T2, T3> newCollection = new MessageCollection<T0, T1, T2, T3>();
                newCollection.MessageGroups.Add(messageId, action);
                _messageCollections.Add(key, newCollection);
            }
        }

#if NET_4_6
        /// <summary>
        /// Registers the provided Method as an Observer for this Message ID
        /// </summary>
        /// <typeparam name="T0"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <param name="messageId"></param>
        /// <param name="action"></param>
        public static void AddObserver<T0, T1, T2, T3, T4>(int messageId, Action<T0, T1, T2, T3, T4> action)
        {
            // get the key for _messageCollections from parameter types
            string key = GetKey(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4));
            // if there already is a collection for the given parameter types, add the provided action to it
            if (_messageCollections.ContainsKey(key))
            {
                MessageCollection<T0, T1, T2, T3, T4> collection = (MessageCollection<T0, T1, T2, T3, T4>)_messageCollections[key];

                if (!collection.MessageGroups.ContainsKey(messageId))
                    collection.MessageGroups.Add(messageId, null);

                collection.MessageGroups[messageId] += action;
            }
            else // create a new collection if there is no collection for these parameter types, then add the provided action to it
            {
                MessageCollection<T0, T1, T2, T3, T4> newCollection = new MessageCollection<T0, T1, T2, T3, T4>();
                newCollection.MessageGroups.Add(messageId, action);
                _messageCollections.Add(key, newCollection);
            }
        }

        /// <summary>
        /// Registers the provided Method as an Observer for this Message ID
        /// </summary>
        /// <typeparam name="T0"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <param name="messageId"></param>
        /// <param name="action"></param>
        public static void AddObserver<T0, T1, T2, T3, T4, T5>(int messageId, Action<T0, T1, T2, T3, T4, T5> action)
        {
            // get the key for _messageCollections from parameter types
            string key = GetKey(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5));
            // if there already is a collection for the given parameter types, add the provided action to it
            if (_messageCollections.ContainsKey(key))
            {
                MessageCollection<T0, T1, T2, T3, T4, T5> collection = (MessageCollection<T0, T1, T2, T3, T4, T5>)_messageCollections[key];

                if (!collection.MessageGroups.ContainsKey(messageId))
                    collection.MessageGroups.Add(messageId, null);

                collection.MessageGroups[messageId] += action;
            }
            else // create a new collection if there is no collection for these parameter types, then add the provided action to it
            {
                MessageCollection<T0, T1, T2, T3, T4, T5> newCollection = new MessageCollection<T0, T1, T2, T3, T4, T5>();
                newCollection.MessageGroups.Add(messageId, action);
                _messageCollections.Add(key, newCollection);
            }
        }

        /// <summary>
        /// Registers the provided Method as an Observer for this Message ID
        /// </summary>
        /// <typeparam name="T0"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <typeparam name="T6"></typeparam>
        /// <param name="messageId"></param>
        /// <param name="action"></param>
        public static void AddObserver<T0, T1, T2, T3, T4, T5, T6>(int messageId, Action<T0, T1, T2, T3, T4, T5, T6> action)
        {
            // get the key for _messageCollections from parameter types
            string key = GetKey(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6));
            // if there already is a collection for the given parameter types, add the provided action to it
            if (_messageCollections.ContainsKey(key))
            {
                MessageCollection<T0, T1, T2, T3, T4, T5, T6> collection = (MessageCollection<T0, T1, T2, T3, T4, T5, T6>)_messageCollections[key];

                if (!collection.MessageGroups.ContainsKey(messageId))
                    collection.MessageGroups.Add(messageId, null);

                collection.MessageGroups[messageId] += action;
            }
            else // create a new collection if there is no collection for these parameter types, then add the provided action to it
            {
                MessageCollection<T0, T1, T2, T3, T4, T5, T6> newCollection = new MessageCollection<T0, T1, T2, T3, T4, T5, T6>();
                newCollection.MessageGroups.Add(messageId, action);
                _messageCollections.Add(key, newCollection);
            }
        }

        /// <summary>
        /// Registers the provided Method as an Observer for this Message ID
        /// </summary>
        /// <typeparam name="T0"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <typeparam name="T6"></typeparam>
        /// <typeparam name="T7"></typeparam>
        /// <param name="messageId"></param>
        /// <param name="action"></param>
        public static void AddObserver<T0, T1, T2, T3, T4, T5, T6, T7>(int messageId, Action<T0, T1, T2, T3, T4, T5, T6, T7> action)
        {
            // get the key for _messageCollections from parameter types
            string key = GetKey(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7));
            // if there already is a collection for the given parameter types, add the provided action to it
            if (_messageCollections.ContainsKey(key))
            {
                MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7> collection = (MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7>)_messageCollections[key];

                if (!collection.MessageGroups.ContainsKey(messageId))
                    collection.MessageGroups.Add(messageId, null);

                collection.MessageGroups[messageId] += action;
            }
            else // create a new collection if there is no collection for these parameter types, then add the provided action to it
            {
                MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7> newCollection = new MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7>();
                newCollection.MessageGroups.Add(messageId, action);
                _messageCollections.Add(key, newCollection);
            }
        }

        /// <summary>
        /// Registers the provided Method as an Observer for this Message ID
        /// </summary>
        /// <typeparam name="T0"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <typeparam name="T6"></typeparam>
        /// <typeparam name="T7"></typeparam>
        /// <typeparam name="T8"></typeparam>
        /// <param name="messageId"></param>
        /// <param name="action"></param>
        public static void AddObserver<T0, T1, T2, T3, T4, T5, T6, T7, T8>(int messageId, Action<T0, T1, T2, T3, T4, T5, T6, T7, T8> action)
        {
            // get the key for _messageCollections from parameter types
            string key = GetKey(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8));
            // if there already is a collection for the given parameter types, add the provided action to it
            if (_messageCollections.ContainsKey(key))
            {
                MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7, T8> collection = (MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7, T8>)_messageCollections[key];

                if (!collection.MessageGroups.ContainsKey(messageId))
                    collection.MessageGroups.Add(messageId, null);

                collection.MessageGroups[messageId] += action;
            }
            else // create a new collection if there is no collection for these parameter types, then add the provided action to it
            {
                MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7, T8> newCollection = new MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7, T8>();
                newCollection.MessageGroups.Add(messageId, action);
                _messageCollections.Add(key, newCollection);
            }
        }

        /// <summary>
        /// Registers the provided Method as an Observer for this Message ID
        /// </summary>
        /// <typeparam name="T0"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <typeparam name="T6"></typeparam>
        /// <typeparam name="T7"></typeparam>
        /// <typeparam name="T8"></typeparam>
        /// <typeparam name="T9"></typeparam>
        /// <param name="messageId"></param>
        /// <param name="action"></param>
        public static void AddObserver<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(int messageId, Action<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> action)
        {
            // get the key for _messageCollections from parameter types
            string key = GetKey(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9));
            // if there already is a collection for the given parameter types, add the provided action to it
            if (_messageCollections.ContainsKey(key))
            {
                MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> collection = (MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>)_messageCollections[key];

                if (!collection.MessageGroups.ContainsKey(messageId))
                    collection.MessageGroups.Add(messageId, null);

                collection.MessageGroups[messageId] += action;
            }
            else // create a new collection if there is no collection for these parameter types, then add the provided action to it
            {
                MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> newCollection = new MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>();
                newCollection.MessageGroups.Add(messageId, action);
                _messageCollections.Add(key, newCollection);
            }
        }

        /// <summary>
        /// Registers the provided Method as an Observer for this Message ID
        /// </summary>
        /// <typeparam name="T0"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <typeparam name="T6"></typeparam>
        /// <typeparam name="T7"></typeparam>
        /// <typeparam name="T8"></typeparam>
        /// <typeparam name="T9"></typeparam>
        /// <typeparam name="T10"></typeparam>
        /// <param name="messageId"></param>
        /// <param name="action"></param>
        public static void AddObserver<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(int messageId, Action<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> action)
        {
            // get the key for _messageCollections from parameter types
            string key = GetKey(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10));
            // if there already is a collection for the given parameter types, add the provided action to it
            if (_messageCollections.ContainsKey(key))
            {
                MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> collection = (MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>)_messageCollections[key];

                if (!collection.MessageGroups.ContainsKey(messageId))
                    collection.MessageGroups.Add(messageId, null);

                collection.MessageGroups[messageId] += action;
            }
            else // create a new collection if there is no collection for these parameter types, then add the provided action to it
            {
                MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> newCollection = new MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>();
                newCollection.MessageGroups.Add(messageId, action);
                _messageCollections.Add(key, newCollection);
            }
        }

        /// <summary>
        /// Registers the provided Method as an Observer for this Message ID
        /// </summary>
        /// <typeparam name="T0"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <typeparam name="T6"></typeparam>
        /// <typeparam name="T7"></typeparam>
        /// <typeparam name="T8"></typeparam>
        /// <typeparam name="T9"></typeparam>
        /// <typeparam name="T10"></typeparam>
        /// <typeparam name="T11"></typeparam>
        /// <param name="messageId"></param>
        /// <param name="action"></param>
        public static void AddObserver<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(int messageId, Action<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> action)
        {
            // get the key for _messageCollections from parameter types
            string key = GetKey(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11));
            // if there already is a collection for the given parameter types, add the provided action to it
            if (_messageCollections.ContainsKey(key))
            {
                MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> collection = (MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>)_messageCollections[key];

                if (!collection.MessageGroups.ContainsKey(messageId))
                    collection.MessageGroups.Add(messageId, null);

                collection.MessageGroups[messageId] += action;
            }
            else // create a new collection if there is no collection for these parameter types, then add the provided action to it
            {
                MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> newCollection = new MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>();
                newCollection.MessageGroups.Add(messageId, action);
                _messageCollections.Add(key, newCollection);
            }
        }

        /// <summary>
        /// Registers the provided Method as an Observer for this Message ID
        /// </summary>
        /// <typeparam name="T0"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <typeparam name="T6"></typeparam>
        /// <typeparam name="T7"></typeparam>
        /// <typeparam name="T8"></typeparam>
        /// <typeparam name="T9"></typeparam>
        /// <typeparam name="T10"></typeparam>
        /// <typeparam name="T11"></typeparam>
        /// <typeparam name="T12"></typeparam>
        /// <param name="messageId"></param>
        /// <param name="action"></param>
        public static void AddObserver<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(int messageId, Action<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> action)
        {
            // get the key for _messageCollections from parameter types
            string key = GetKey(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12));
            // if there already is a collection for the given parameter types, add the provided action to it
            if (_messageCollections.ContainsKey(key))
            {
                MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> collection = (MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>)_messageCollections[key];

                if (!collection.MessageGroups.ContainsKey(messageId))
                    collection.MessageGroups.Add(messageId, null);

                collection.MessageGroups[messageId] += action;
            }
            else // create a new collection if there is no collection for these parameter types, then add the provided action to it
            {
                MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> newCollection = new MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>();
                newCollection.MessageGroups.Add(messageId, action);
                _messageCollections.Add(key, newCollection);
            }
        }

        /// <summary>
        /// Registers the provided Method as an Observer for this Message ID
        /// </summary>
        /// <typeparam name="T0"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <typeparam name="T6"></typeparam>
        /// <typeparam name="T7"></typeparam>
        /// <typeparam name="T8"></typeparam>
        /// <typeparam name="T9"></typeparam>
        /// <typeparam name="T10"></typeparam>
        /// <typeparam name="T11"></typeparam>
        /// <typeparam name="T12"></typeparam>
        /// <typeparam name="T13"></typeparam>
        /// <param name="messageId"></param>
        /// <param name="action"></param>
        public static void AddObserver<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(int messageId, Action<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> action)
        {
            // get the key for _messageCollections from parameter types
            string key = GetKey(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12), typeof(T13));
            // if there already is a collection for the given parameter types, add the provided action to it
            if (_messageCollections.ContainsKey(key))
            {
                MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> collection = (MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>)_messageCollections[key];

                if (!collection.MessageGroups.ContainsKey(messageId))
                    collection.MessageGroups.Add(messageId, null);

                collection.MessageGroups[messageId] += action;
            }
            else // create a new collection if there is no collection for these parameter types, then add the provided action to it
            {
                MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> newCollection = new MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>();
                newCollection.MessageGroups.Add(messageId, action);
                _messageCollections.Add(key, newCollection);
            }
        }

        /// <summary>
        /// Registers the provided Method as an Observer for this Message ID
        /// </summary>
        /// <typeparam name="T0"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <typeparam name="T6"></typeparam>
        /// <typeparam name="T7"></typeparam>
        /// <typeparam name="T8"></typeparam>
        /// <typeparam name="T9"></typeparam>
        /// <typeparam name="T10"></typeparam>
        /// <typeparam name="T11"></typeparam>
        /// <typeparam name="T12"></typeparam>
        /// <typeparam name="T13"></typeparam>
        /// <typeparam name="T14"></typeparam>
        /// <param name="messageId"></param>
        /// <param name="action"></param>
        public static void AddObserver<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(int messageId, Action<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> action)
        {
            // get the key for _messageCollections from parameter types
            string key = GetKey(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12), typeof(T13), typeof(T14));
            // if there already is a collection for the given parameter types, add the provided action to it
            if (_messageCollections.ContainsKey(key))
            {
                MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> collection = (MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>)_messageCollections[key];

                if (!collection.MessageGroups.ContainsKey(messageId))
                    collection.MessageGroups.Add(messageId, null);

                collection.MessageGroups[messageId] += action;
            }
            else // create a new collection if there is no collection for these parameter types, then add the provided action to it
            {
                MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> newCollection = new MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>();
                newCollection.MessageGroups.Add(messageId, action);
                _messageCollections.Add(key, newCollection);
            }
        }

        /// <summary>
        /// Registers the provided Method as an Observer for this Message ID
        /// </summary>
        /// <typeparam name="T0"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <typeparam name="T6"></typeparam>
        /// <typeparam name="T7"></typeparam>
        /// <typeparam name="T8"></typeparam>
        /// <typeparam name="T9"></typeparam>
        /// <typeparam name="T10"></typeparam>
        /// <typeparam name="T11"></typeparam>
        /// <typeparam name="T12"></typeparam>
        /// <typeparam name="T13"></typeparam>
        /// <typeparam name="T14"></typeparam>
        /// <typeparam name="T15"></typeparam>
        /// <param name="messageId"></param>
        /// <param name="action"></param>
        public static void AddObserver<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(int messageId, Action<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> action)
        {
            // get the key for _messageCollections from parameter types
            string key = GetKey(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12), typeof(T13), typeof(T14), typeof(T15));
            // if there already is a collection for the given parameter types, add the provided action to it
            if (_messageCollections.ContainsKey(key))
            {
                MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> collection = (MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>)_messageCollections[key];

                if (!collection.MessageGroups.ContainsKey(messageId))
                    collection.MessageGroups.Add(messageId, null);

                collection.MessageGroups[messageId] += action;
            }
            else // create a new collection if there is no collection for these parameter types, then add the provided action to it
            {
                MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> newCollection = new MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>();
                newCollection.MessageGroups.Add(messageId, action);
                _messageCollections.Add(key, newCollection);
            }
        }
#endif
        #endregion


        // ######################## REMOVE OBSERVER ######################## //
        #region REMOVE_OBSERVER
        /// <summary>
        /// Removes the provided Method from the Observers for this Message ID
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="action"></param>
        public static void RemoveObserver(int messageId, Action action)
        {
            // get the key for _messageCollections from parameter types
            string key = "";
            // if there is a collection for the given parameter types, check if it contains this message ID
            if (_messageCollections.ContainsKey(key))
            {
                MessageCollection collection = (MessageCollection)_messageCollections[key];
                if (collection.MessageGroups.ContainsKey(messageId))
                {
                    // remove the proivded action from observers and delete entry for message ID
                    // if there are no other observers registered and delete collection if there are no other message ID registered
                    collection.MessageGroups[messageId] -= action;

                    if (collection.MessageGroups[messageId] == null)
                        collection.MessageGroups.Remove(messageId);

                    if (collection.MessageGroups.Count == 0)
                        _messageCollections.Remove(key);
                }
            }
        }

        /// <summary>
        /// Removes the provided Method from the Observers for this Message ID
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="messageId"></param>
        /// <param name="action"></param>
        public static void RemoveObserver<T>(int messageId, Action<T> action)
        {
            // get the key for _messageCollections from parameter types
            string key = typeof(T).FullName.ToString();
            // if there is a collection for the given parameter types, check if it contains this message ID
            if (_messageCollections.ContainsKey(key))
            {
                MessageCollection<T> collection = (MessageCollection<T>)_messageCollections[key];
                if (collection.MessageGroups.ContainsKey(messageId))
                {
                    // remove the proivded action from observers and delete entry for message ID
                    // if there are no other observers registered and delete collection if there are no other message ID registered
                    collection.MessageGroups[messageId] -= action;

                    if (collection.MessageGroups[messageId] == null)
                        collection.MessageGroups.Remove(messageId);

                    if (collection.MessageGroups.Count == 0)
                        _messageCollections.Remove(key);
                }
            }
        }

        /// <summary>
        /// Removes the provided Method from the Observers for this Message ID
        /// </summary>
        /// <typeparam name="T0"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <param name="messageId"></param>
        /// <param name="action"></param>
        public static void RemoveObserver<T0, T1>(int messageId, Action<T0, T1> action)
        {
            // get the key for _messageCollections from parameter types
            string key = GetKey(typeof(T0), typeof(T1));
            // if there is a collection for the given parameter types, check if it contains this message ID
            if (_messageCollections.ContainsKey(key))
            {
                MessageCollection<T0, T1> collection = (MessageCollection<T0, T1>)_messageCollections[key];
                if (collection.MessageGroups.ContainsKey(messageId))
                {
                    // remove the proivded action from observers and delete entry for message ID
                    // if there are no other observers registered and delete collection if there are no other message ID registered
                    collection.MessageGroups[messageId] -= action;

                    if (collection.MessageGroups[messageId] == null)
                        collection.MessageGroups.Remove(messageId);

                    if (collection.MessageGroups.Count == 0)
                        _messageCollections.Remove(key);
                }
            }
        }

        /// <summary>
        /// Removes the provided Method from the Observers for this Message ID
        /// </summary>
        /// <typeparam name="T0"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="messageId"></param>
        /// <param name="action"></param>
        public static void RemoveObserver<T0, T1, T2>(int messageId, Action<T0, T1, T2> action)
        {
            // get the key for _messageCollections from parameter types
            string key = GetKey(typeof(T0), typeof(T1), typeof(T2));
            // if there is a collection for the given parameter types, check if it contains this message ID
            if (_messageCollections.ContainsKey(key))
            {
                MessageCollection<T0, T1, T2> collection = (MessageCollection<T0, T1, T2>)_messageCollections[key];
                if (collection.MessageGroups.ContainsKey(messageId))
                {
                    // remove the proivded action from observers and delete entry for message ID
                    // if there are no other observers registered and delete collection if there are no other message ID registered
                    collection.MessageGroups[messageId] -= action;

                    if (collection.MessageGroups[messageId] == null)
                        collection.MessageGroups.Remove(messageId);

                    if (collection.MessageGroups.Count == 0)
                        _messageCollections.Remove(key);
                }
            }
        }

        /// <summary>
        /// Removes the provided Method from the Observers for this Message ID
        /// </summary>
        /// <typeparam name="T0"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <param name="messageId"></param>
        /// <param name="action"></param>
        public static void RemoveObserver<T0, T1, T2, T3>(int messageId, Action<T0, T1, T2, T3> action)
        {
            // get the key for _messageCollections from parameter types
            string key = GetKey(typeof(T0), typeof(T1), typeof(T2), typeof(T3));
            // if there is a collection for the given parameter types, check if it contains this message ID
            if (_messageCollections.ContainsKey(key))
            {
                MessageCollection<T0, T1, T2, T3> collection = (MessageCollection<T0, T1, T2, T3>)_messageCollections[key];
                if (collection.MessageGroups.ContainsKey(messageId))
                {
                    // remove the proivded action from observers and delete entry for message ID
                    // if there are no other observers registered and delete collection if there are no other message ID registered
                    collection.MessageGroups[messageId] -= action;

                    if (collection.MessageGroups[messageId] == null)
                        collection.MessageGroups.Remove(messageId);

                    if (collection.MessageGroups.Count == 0)
                        _messageCollections.Remove(key);
                }
            }
        }

#if NET_4_6
        /// <summary>
        /// Removes the provided Method from the Observers for this Message ID
        /// </summary>
        /// <typeparam name="T0"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <param name="messageId"></param>
        /// <param name="action"></param>
        public static void RemoveObserver<T0, T1, T2, T3, T4>(int messageId, Action<T0, T1, T2, T3, T4> action)
        {
            // get the key for _messageCollections from parameter types
            string key = GetKey(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4));
            // if there is a collection for the given parameter types, check if it contains this message ID
            if (_messageCollections.ContainsKey(key))
            {
                MessageCollection<T0, T1, T2, T3, T4> collection = (MessageCollection<T0, T1, T2, T3, T4>)_messageCollections[key];
                if (collection.MessageGroups.ContainsKey(messageId))
                {
                    // remove the proivded action from observers and delete entry for message ID
                    // if there are no other observers registered and delete collection if there are no other message ID registered
                    collection.MessageGroups[messageId] -= action;

                    if (collection.MessageGroups[messageId] == null)
                        collection.MessageGroups.Remove(messageId);

                    if (collection.MessageGroups.Count == 0)
                        _messageCollections.Remove(key);
                }
            }
        }

        /// <summary>
        /// Removes the provided Method from the Observers for this Message ID
        /// </summary>
        /// <typeparam name="T0"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <param name="messageId"></param>
        /// <param name="action"></param>
        public static void RemoveObserver<T0, T1, T2, T3, T4, T5>(int messageId, Action<T0, T1, T2, T3, T4, T5> action)
        {
            // get the key for _messageCollections from parameter types
            string key = GetKey(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5));
            // if there is a collection for the given parameter types, check if it contains this message ID
            if (_messageCollections.ContainsKey(key))
            {
                MessageCollection<T0, T1, T2, T3, T4, T5> collection = (MessageCollection<T0, T1, T2, T3, T4, T5>)_messageCollections[key];
                if (collection.MessageGroups.ContainsKey(messageId))
                {
                    // remove the proivded action from observers and delete entry for message ID
                    // if there are no other observers registered and delete collection if there are no other message ID registered
                    collection.MessageGroups[messageId] -= action;

                    if (collection.MessageGroups[messageId] == null)
                        collection.MessageGroups.Remove(messageId);

                    if (collection.MessageGroups.Count == 0)
                        _messageCollections.Remove(key);
                }
            }
        }

        /// <summary>
        /// Removes the provided Method from the Observers for this Message ID
        /// </summary>
        /// <typeparam name="T0"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <typeparam name="T6"></typeparam>
        /// <param name="messageId"></param>
        /// <param name="action"></param>
        public static void RemoveObserver<T0, T1, T2, T3, T4, T5, T6>(int messageId, Action<T0, T1, T2, T3, T4, T5, T6> action)
        {
            // get the key for _messageCollections from parameter types
            string key = GetKey(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6));
            // if there is a collection for the given parameter types, check if it contains this message ID
            if (_messageCollections.ContainsKey(key))
            {
                MessageCollection<T0, T1, T2, T3, T4, T5, T6> collection = (MessageCollection<T0, T1, T2, T3, T4, T5, T6>)_messageCollections[key];
                if (collection.MessageGroups.ContainsKey(messageId))
                {
                    // remove the proivded action from observers and delete entry for message ID
                    // if there are no other observers registered and delete collection if there are no other message ID registered
                    collection.MessageGroups[messageId] -= action;

                    if (collection.MessageGroups[messageId] == null)
                        collection.MessageGroups.Remove(messageId);

                    if (collection.MessageGroups.Count == 0)
                        _messageCollections.Remove(key);
                }
            }
        }

        /// <summary>
        /// Removes the provided Method from the Observers for this Message ID
        /// </summary>
        /// <typeparam name="T0"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <typeparam name="T6"></typeparam>
        /// <typeparam name="T7"></typeparam>
        /// <param name="messageId"></param>
        /// <param name="action"></param>
        public static void RemoveObserver<T0, T1, T2, T3, T4, T5, T6, T7>(int messageId, Action<T0, T1, T2, T3, T4, T5, T6, T7> action)
        {
            // get the key for _messageCollections from parameter types
            string key = GetKey(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7));
            // if there is a collection for the given parameter types, check if it contains this message ID
            if (_messageCollections.ContainsKey(key))
            {
                MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7> collection = (MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7>)_messageCollections[key];
                if (collection.MessageGroups.ContainsKey(messageId))
                {
                    // remove the proivded action from observers and delete entry for message ID
                    // if there are no other observers registered and delete collection if there are no other message ID registered
                    collection.MessageGroups[messageId] -= action;

                    if (collection.MessageGroups[messageId] == null)
                        collection.MessageGroups.Remove(messageId);

                    if (collection.MessageGroups.Count == 0)
                        _messageCollections.Remove(key);
                }
            }
        }

        /// <summary>
        /// Removes the provided Method from the Observers for this Message ID
        /// </summary>
        /// <typeparam name="T0"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <typeparam name="T6"></typeparam>
        /// <typeparam name="T7"></typeparam>
        /// <typeparam name="T8"></typeparam>
        /// <param name="messageId"></param>
        /// <param name="action"></param>
        public static void RemoveObserver<T0, T1, T2, T3, T4, T5, T6, T7, T8>(int messageId, Action<T0, T1, T2, T3, T4, T5, T6, T7, T8> action)
        {
            // get the key for _messageCollections from parameter types
            string key = GetKey(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8));
            // if there is a collection for the given parameter types, check if it contains this message ID
            if (_messageCollections.ContainsKey(key))
            {
                MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7, T8> collection = (MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7, T8>)_messageCollections[key];
                if (collection.MessageGroups.ContainsKey(messageId))
                {
                    // remove the proivded action from observers and delete entry for message ID
                    // if there are no other observers registered and delete collection if there are no other message ID registered
                    collection.MessageGroups[messageId] -= action;

                    if (collection.MessageGroups[messageId] == null)
                        collection.MessageGroups.Remove(messageId);

                    if (collection.MessageGroups.Count == 0)
                        _messageCollections.Remove(key);
                }
            }
        }

        /// <summary>
        /// Removes the provided Method from the Observers for this Message ID
        /// </summary>
        /// <typeparam name="T0"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <typeparam name="T6"></typeparam>
        /// <typeparam name="T7"></typeparam>
        /// <typeparam name="T8"></typeparam>
        /// <typeparam name="T9"></typeparam>
        /// <param name="messageId"></param>
        /// <param name="action"></param>
        public static void RemoveObserver<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(int messageId, Action<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> action)
        {
            // get the key for _messageCollections from parameter types
            string key = GetKey(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9));
            // if there is a collection for the given parameter types, check if it contains this message ID
            if (_messageCollections.ContainsKey(key))
            {
                MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> collection = (MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>)_messageCollections[key];
                if (collection.MessageGroups.ContainsKey(messageId))
                {
                    // remove the proivded action from observers and delete entry for message ID
                    // if there are no other observers registered and delete collection if there are no other message ID registered
                    collection.MessageGroups[messageId] -= action;

                    if (collection.MessageGroups[messageId] == null)
                        collection.MessageGroups.Remove(messageId);

                    if (collection.MessageGroups.Count == 0)
                        _messageCollections.Remove(key);
                }
            }
        }

        /// <summary>
        /// Removes the provided Method from the Observers for this Message ID
        /// </summary>
        /// <typeparam name="T0"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <typeparam name="T6"></typeparam>
        /// <typeparam name="T7"></typeparam>
        /// <typeparam name="T8"></typeparam>
        /// <typeparam name="T9"></typeparam>
        /// <typeparam name="T10"></typeparam>
        /// <param name="messageId"></param>
        /// <param name="action"></param>
        public static void RemoveObserver<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(int messageId, Action<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> action)
        {
            // get the key for _messageCollections from parameter types
            string key = GetKey(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10));
            // if there is a collection for the given parameter types, check if it contains this message ID
            if (_messageCollections.ContainsKey(key))
            {
                MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> collection = (MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>)_messageCollections[key];
                if (collection.MessageGroups.ContainsKey(messageId))
                {
                    // remove the proivded action from observers and delete entry for message ID
                    // if there are no other observers registered and delete collection if there are no other message ID registered
                    collection.MessageGroups[messageId] -= action;

                    if (collection.MessageGroups[messageId] == null)
                        collection.MessageGroups.Remove(messageId);

                    if (collection.MessageGroups.Count == 0)
                        _messageCollections.Remove(key);
                }
            }
        }

        /// <summary>
        /// Removes the provided Method from the Observers for this Message ID
        /// </summary>
        /// <typeparam name="T0"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <typeparam name="T6"></typeparam>
        /// <typeparam name="T7"></typeparam>
        /// <typeparam name="T8"></typeparam>
        /// <typeparam name="T9"></typeparam>
        /// <typeparam name="T10"></typeparam>
        /// <typeparam name="T11"></typeparam>
        /// <param name="messageId"></param>
        /// <param name="action"></param>
        public static void RemoveObserver<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(int messageId, Action<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> action)
        {
            // get the key for _messageCollections from parameter types
            string key = GetKey(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11));
            // if there is a collection for the given parameter types, check if it contains this message ID
            if (_messageCollections.ContainsKey(key))
            {
                MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> collection = (MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>)_messageCollections[key];
                if (collection.MessageGroups.ContainsKey(messageId))
                {
                    // remove the proivded action from observers and delete entry for message ID
                    // if there are no other observers registered and delete collection if there are no other message ID registered
                    collection.MessageGroups[messageId] -= action;

                    if (collection.MessageGroups[messageId] == null)
                        collection.MessageGroups.Remove(messageId);

                    if (collection.MessageGroups.Count == 0)
                        _messageCollections.Remove(key);
                }
            }
        }

        /// <summary>
        /// Removes the provided Method from the Observers for this Message ID
        /// </summary>
        /// <typeparam name="T0"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <typeparam name="T6"></typeparam>
        /// <typeparam name="T7"></typeparam>
        /// <typeparam name="T8"></typeparam>
        /// <typeparam name="T9"></typeparam>
        /// <typeparam name="T10"></typeparam>
        /// <typeparam name="T11"></typeparam>
        /// <typeparam name="T12"></typeparam>
        /// <param name="messageId"></param>
        /// <param name="action"></param>
        public static void RemoveObserver<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(int messageId, Action<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> action)
        {
            // get the key for _messageCollections from parameter types
            string key = GetKey(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12));
            // if there is a collection for the given parameter types, check if it contains this message ID
            if (_messageCollections.ContainsKey(key))
            {
                MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> collection = (MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>)_messageCollections[key];
                if (collection.MessageGroups.ContainsKey(messageId))
                {
                    // remove the proivded action from observers and delete entry for message ID
                    // if there are no other observers registered and delete collection if there are no other message ID registered
                    collection.MessageGroups[messageId] -= action;

                    if (collection.MessageGroups[messageId] == null)
                        collection.MessageGroups.Remove(messageId);

                    if (collection.MessageGroups.Count == 0)
                        _messageCollections.Remove(key);
                }
            }
        }

        /// <summary>
        /// Removes the provided Method from the Observers for this Message ID
        /// </summary>
        /// <typeparam name="T0"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <typeparam name="T6"></typeparam>
        /// <typeparam name="T7"></typeparam>
        /// <typeparam name="T8"></typeparam>
        /// <typeparam name="T9"></typeparam>
        /// <typeparam name="T10"></typeparam>
        /// <typeparam name="T11"></typeparam>
        /// <typeparam name="T12"></typeparam>
        /// <typeparam name="T13"></typeparam>
        /// <param name="messageId"></param>
        /// <param name="action"></param>
        public static void RemoveObserver<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(int messageId, Action<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> action)
        {
            // get the key for _messageCollections from parameter types
            string key = GetKey(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12), typeof(T13));
            // if there is a collection for the given parameter types, check if it contains this message ID
            if (_messageCollections.ContainsKey(key))
            {
                MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> collection = (MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>)_messageCollections[key];
                if (collection.MessageGroups.ContainsKey(messageId))
                {
                    // remove the proivded action from observers and delete entry for message ID
                    // if there are no other observers registered and delete collection if there are no other message ID registered
                    collection.MessageGroups[messageId] -= action;

                    if (collection.MessageGroups[messageId] == null)
                        collection.MessageGroups.Remove(messageId);

                    if (collection.MessageGroups.Count == 0)
                        _messageCollections.Remove(key);
                }
            }
        }

        /// <summary>
        /// Removes the provided Method from the Observers for this Message ID
        /// </summary>
        /// <typeparam name="T0"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <typeparam name="T6"></typeparam>
        /// <typeparam name="T7"></typeparam>
        /// <typeparam name="T8"></typeparam>
        /// <typeparam name="T9"></typeparam>
        /// <typeparam name="T10"></typeparam>
        /// <typeparam name="T11"></typeparam>
        /// <typeparam name="T12"></typeparam>
        /// <typeparam name="T13"></typeparam>
        /// <typeparam name="T14"></typeparam>
        /// <param name="messageId"></param>
        /// <param name="action"></param>
        public static void RemoveObserver<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(int messageId, Action<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> action)
        {
            // get the key for _messageCollections from parameter types
            string key = GetKey(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12), typeof(T13), typeof(T14));
            // if there is a collection for the given parameter types, check if it contains this message ID
            if (_messageCollections.ContainsKey(key))
            {
                MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> collection = (MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>)_messageCollections[key];
                if (collection.MessageGroups.ContainsKey(messageId))
                {
                    // remove the proivded action from observers and delete entry for message ID
                    // if there are no other observers registered and delete collection if there are no other message ID registered
                    collection.MessageGroups[messageId] -= action;

                    if (collection.MessageGroups[messageId] == null)
                        collection.MessageGroups.Remove(messageId);

                    if (collection.MessageGroups.Count == 0)
                        _messageCollections.Remove(key);
                }
            }
        }

        /// <summary>
        /// Removes the provided Method from the Observers for this Message ID
        /// </summary>
        /// <typeparam name="T0"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <typeparam name="T6"></typeparam>
        /// <typeparam name="T7"></typeparam>
        /// <typeparam name="T8"></typeparam>
        /// <typeparam name="T9"></typeparam>
        /// <typeparam name="T10"></typeparam>
        /// <typeparam name="T11"></typeparam>
        /// <typeparam name="T12"></typeparam>
        /// <typeparam name="T13"></typeparam>
        /// <typeparam name="T14"></typeparam>
        /// <typeparam name="T15"></typeparam>
        /// <param name="messageId"></param>
        /// <param name="action"></param>
        public static void RemoveObserver<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(int messageId, Action<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> action)
        {
            // get the key for _messageCollections from parameter types
            string key = GetKey(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12), typeof(T13), typeof(T14), typeof(T15));
            // if there is a collection for the given parameter types, check if it contains this message ID
            if (_messageCollections.ContainsKey(key))
            {
                MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> collection = (MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>)_messageCollections[key];
                if (collection.MessageGroups.ContainsKey(messageId))
                {
                    // remove the proivded action from observers and delete entry for message ID
                    // if there are no other observers registered and delete collection if there are no other message ID registered
                    collection.MessageGroups[messageId] -= action;

                    if (collection.MessageGroups[messageId] == null)
                        collection.MessageGroups.Remove(messageId);

                    if (collection.MessageGroups.Count == 0)
                        _messageCollections.Remove(key);
                }
            }
        }
#endif
        #endregion


        // ######################## POST ######################## //
        #region POST
        /// <summary>
        /// Posts message to all observers registered for this Message Type
        /// </summary>
        /// <param name="messageId"></param>
        public static void Post(int messageId)
        {
            // get the key for _messageCollections from parameter types
            string key = "";
            // if there is a collection for the given parameter types, check if the collection contains this message ID and invoke the delegate with the provided parameters
            if (_messageCollections.ContainsKey(key))
            {
                MessageCollection collection = (MessageCollection)_messageCollections[key];
                if (collection.MessageGroups.ContainsKey(messageId))
                {
                    collection.MessageGroups[messageId]();
                }
            }
        }

        /// <summary>
        /// Posts message to all observers registered for this Message Type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="messageId"></param>
        /// <param name="param"></param>
        public static void Post<T>(int messageId, T param)
        {
            // get the key for _messageCollections from parameter types
            string key = typeof(T).FullName.ToString();
            // if there is a collection for the given parameter types, check if the collection contains this message ID and invoke the delegate with the provided parameters
            if (_messageCollections.ContainsKey(key))
            {
                MessageCollection<T> collection = (MessageCollection<T>)_messageCollections[key];
                if (collection.MessageGroups.ContainsKey(messageId))
                {
                    collection.MessageGroups[messageId](param);
                }
            }
        }

        /// <summary>
        /// Posts message to all observers registered for this Message Type
        /// </summary>
        /// <typeparam name="T0"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <param name="messageId"></param>
        /// <param name="param0"></param>
        /// <param name="param1"></param>
        public static void Post<T0, T1>(int messageId, T0 param0, T1 param1)
        {
            // get the key for _messageCollections from parameter types
            string key = GetKey(typeof(T0), typeof(T1));
            // if there is a collection for the given parameter types, check if the collection contains this message ID and invoke the delegate with the provided parameters
            if (_messageCollections.ContainsKey(key))
            {
                MessageCollection<T0, T1> collection = (MessageCollection<T0, T1>)_messageCollections[key];
                if (collection.MessageGroups.ContainsKey(messageId))
                {
                    collection.MessageGroups[messageId](param0, param1);
                }
            }
        }

        /// <summary>
        /// Posts message to all observers registered for this Message Type
        /// </summary>
        /// <typeparam name="T0"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="messageId"></param>
        /// <param name="param0"></param>
        /// <param name="param1"></param>
        /// <param name="param2"></param>
        public static void Post<T0, T1, T2>(int messageId, T0 param0, T1 param1, T2 param2)
        {
            // get the key for _messageCollections from parameter types
            string key = GetKey(typeof(T0), typeof(T1), typeof(T2));
            // if there is a collection for the given parameter types, check if the collection contains this message ID and invoke the delegate with the provided parameters
            if (_messageCollections.ContainsKey(key))
            {
                MessageCollection<T0, T1, T2> collection = (MessageCollection<T0, T1, T2>)_messageCollections[key];
                if (collection.MessageGroups.ContainsKey(messageId))
                {
                    collection.MessageGroups[messageId](param0, param1, param2);
                }
            }
        }

        /// <summary>
        /// Posts message to all observers registered for this Message Type
        /// </summary>
        /// <typeparam name="T0"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <param name="messageId"></param>
        /// <param name="param0"></param>
        /// <param name="param1"></param>
        /// <param name="param2"></param>
        /// <param name="param3"></param>
        public static void Post<T0, T1, T2, T3>(int messageId, T0 param0, T1 param1, T2 param2, T3 param3)
        {
            // get the key for _messageCollections from parameter types
            string key = GetKey(typeof(T0), typeof(T1), typeof(T2), typeof(T3));
            // if there is a collection for the given parameter types, check if the collection contains this message ID and invoke the delegate with the provided parameters
            if (_messageCollections.ContainsKey(key))
            {
                MessageCollection<T0, T1, T2, T3> collection = (MessageCollection<T0, T1, T2, T3>)_messageCollections[key];
                if (collection.MessageGroups.ContainsKey(messageId))
                {
                    collection.MessageGroups[messageId](param0, param1, param2, param3);
                }
            }
        }

#if NET_4_6
        /// <summary>
        /// Posts message to all observers registered for this Message Type
        /// </summary>
        /// <typeparam name="T0"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <param name="messageId"></param>
        /// <param name="param0"></param>
        /// <param name="param1"></param>
        /// <param name="param2"></param>
        /// <param name="param3"></param>
        /// <param name="param4"></param>
        public static void Post<T0, T1, T2, T3, T4>(int messageId, T0 param0, T1 param1, T2 param2, T3 param3, T4 param4)
        {
            // get the key for _messageCollections from parameter types
            string key = GetKey(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4));
            // if there is a collection for the given parameter types, check if the collection contains this message ID and invoke the delegate with the provided parameters
            if (_messageCollections.ContainsKey(key))
            {
                MessageCollection<T0, T1, T2, T3, T4> collection = (MessageCollection<T0, T1, T2, T3, T4>)_messageCollections[key];
                if (collection.MessageGroups.ContainsKey(messageId))
                {
                    collection.MessageGroups[messageId](param0, param1, param2, param3, param4);
                }
            }
        }

        /// <summary>
        /// Posts message to all observers registered for this Message Type
        /// </summary>
        /// <typeparam name="T0"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <param name="messageId"></param>
        /// <param name="param0"></param>
        /// <param name="param1"></param>
        /// <param name="param2"></param>
        /// <param name="param3"></param>
        /// <param name="param4"></param>
        /// <param name="param5"></param>
        public static void Post<T0, T1, T2, T3, T4, T5>(int messageId, T0 param0, T1 param1, T2 param2, T3 param3, T4 param4, T5 param5)
        {
            // get the key for _messageCollections from parameter types
            string key = GetKey(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5));
            // if there is a collection for the given parameter types, check if the collection contains this message ID and invoke the delegate with the provided parameters
            if (_messageCollections.ContainsKey(key))
            {
                MessageCollection<T0, T1, T2, T3, T4, T5> collection = (MessageCollection<T0, T1, T2, T3, T4, T5>)_messageCollections[key];
                if (collection.MessageGroups.ContainsKey(messageId))
                {
                    collection.MessageGroups[messageId](param0, param1, param2, param3, param4, param5);
                }
            }
        }

        /// <summary>
        /// Posts message to all observers registered for this Message Type
        /// </summary>
        /// <typeparam name="T0"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <typeparam name="T6"></typeparam>
        /// <param name="messageId"></param>
        /// <param name="param0"></param>
        /// <param name="param1"></param>
        /// <param name="param2"></param>
        /// <param name="param3"></param>
        /// <param name="param4"></param>
        /// <param name="param5"></param>
        /// <param name="param6"></param>
        public static void Post<T0, T1, T2, T3, T4, T5, T6>(int messageId, T0 param0, T1 param1, T2 param2, T3 param3, T4 param4, T5 param5, T6 param6)
        {
            // get the key for _messageCollections from parameter types
            string key = GetKey(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6));
            // if there is a collection for the given parameter types, check if the collection contains this message ID and invoke the delegate with the provided parameters
            if (_messageCollections.ContainsKey(key))
            {
                MessageCollection<T0, T1, T2, T3, T4, T5, T6> collection = (MessageCollection<T0, T1, T2, T3, T4, T5, T6>)_messageCollections[key];
                if (collection.MessageGroups.ContainsKey(messageId))
                {
                    collection.MessageGroups[messageId](param0, param1, param2, param3, param4, param5, param6);
                }
            }
        }

        /// <summary>
        /// Posts message to all observers registered for this Message Type
        /// </summary>
        /// <typeparam name="T0"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <typeparam name="T6"></typeparam>
        /// <typeparam name="T7"></typeparam>
        /// <param name="messageId"></param>
        /// <param name="param0"></param>
        /// <param name="param1"></param>
        /// <param name="param2"></param>
        /// <param name="param3"></param>
        /// <param name="param4"></param>
        /// <param name="param5"></param>
        /// <param name="param6"></param>
        /// <param name="param7"></param>
        public static void Post<T0, T1, T2, T3, T4, T5, T6, T7>(int messageId, T0 param0, T1 param1, T2 param2, T3 param3, T4 param4, T5 param5, T6 param6, T7 param7)
        {
            // get the key for _messageCollections from parameter types
            string key = GetKey(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7));
            // if there is a collection for the given parameter types, check if the collection contains this message ID and invoke the delegate with the provided parameters
            if (_messageCollections.ContainsKey(key))
            {
                MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7> collection = (MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7>)_messageCollections[key];
                if (collection.MessageGroups.ContainsKey(messageId))
                {
                    collection.MessageGroups[messageId](param0, param1, param2, param3, param4, param5, param6, param7);
                }
            }
        }

        /// <summary>
        /// Posts message to all observers registered for this Message Type
        /// </summary>
        /// <typeparam name="T0"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <typeparam name="T6"></typeparam>
        /// <typeparam name="T7"></typeparam>
        /// <typeparam name="T8"></typeparam>
        /// <param name="messageId"></param>
        /// <param name="param0"></param>
        /// <param name="param1"></param>
        /// <param name="param2"></param>
        /// <param name="param3"></param>
        /// <param name="param4"></param>
        /// <param name="param5"></param>
        /// <param name="param6"></param>
        /// <param name="param7"></param>
        /// <param name="param8"></param>
        public static void Post<T0, T1, T2, T3, T4, T5, T6, T7, T8>(int messageId, T0 param0, T1 param1, T2 param2, T3 param3, T4 param4, T5 param5, T6 param6, T7 param7, T8 param8)
        {
            // get the key for _messageCollections from parameter types
            string key = GetKey(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8));
            // if there is a collection for the given parameter types, check if the collection contains this message ID and invoke the delegate with the provided parameters
            if (_messageCollections.ContainsKey(key))
            {
                MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7, T8> collection = (MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7, T8>)_messageCollections[key];
                if (collection.MessageGroups.ContainsKey(messageId))
                {
                    collection.MessageGroups[messageId](param0, param1, param2, param3, param4, param5, param6, param7, param8);
                }
            }
        }

        /// <summary>
        /// Posts message to all observers registered for this Message Type
        /// </summary>
        /// <typeparam name="T0"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <typeparam name="T6"></typeparam>
        /// <typeparam name="T7"></typeparam>
        /// <typeparam name="T8"></typeparam>
        /// <typeparam name="T9"></typeparam>
        /// <param name="messageId"></param>
        /// <param name="param0"></param>
        /// <param name="param1"></param>
        /// <param name="param2"></param>
        /// <param name="param3"></param>
        /// <param name="param4"></param>
        /// <param name="param5"></param>
        /// <param name="param6"></param>
        /// <param name="param7"></param>
        /// <param name="param8"></param>
        /// <param name="param9"></param>
        public static void Post<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(int messageId, T0 param0, T1 param1, T2 param2, T3 param3, T4 param4, T5 param5, T6 param6, T7 param7, T8 param8, T9 param9)
        {
            // get the key for _messageCollections from parameter types
            string key = GetKey(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9));
            // if there is a collection for the given parameter types, check if the collection contains this message ID and invoke the delegate with the provided parameters
            if (_messageCollections.ContainsKey(key))
            {
                MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> collection = (MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>)_messageCollections[key];
                if (collection.MessageGroups.ContainsKey(messageId))
                {
                    collection.MessageGroups[messageId](param0, param1, param2, param3, param4, param5, param6, param7, param8, param9);
                }
            }
        }

        /// <summary>
        /// Posts message to all observers registered for this Message Type
        /// </summary>
        /// <typeparam name="T0"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <typeparam name="T6"></typeparam>
        /// <typeparam name="T7"></typeparam>
        /// <typeparam name="T8"></typeparam>
        /// <typeparam name="T9"></typeparam>
        /// <typeparam name="T10"></typeparam>
        /// <param name="messageId"></param>
        /// <param name="param0"></param>
        /// <param name="param1"></param>
        /// <param name="param2"></param>
        /// <param name="param3"></param>
        /// <param name="param4"></param>
        /// <param name="param5"></param>
        /// <param name="param6"></param>
        /// <param name="param7"></param>
        /// <param name="param8"></param>
        /// <param name="param9"></param>
        /// <param name="param10"></param>
        public static void Post<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(int messageId, T0 param0, T1 param1, T2 param2, T3 param3, T4 param4, T5 param5, T6 param6, T7 param7, T8 param8, T9 param9, T10 param10)
        {
            // get the key for _messageCollections from parameter types
            string key = GetKey(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10));
            // if there is a collection for the given parameter types, check if the collection contains this message ID and invoke the delegate with the provided parameters
            if (_messageCollections.ContainsKey(key))
            {
                MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> collection = (MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>)_messageCollections[key];
                if (collection.MessageGroups.ContainsKey(messageId))
                {
                    collection.MessageGroups[messageId](param0, param1, param2, param3, param4, param5, param6, param7, param8, param9, param10);
                }
            }
        }

        /// <summary>
        /// Posts message to all observers registered for this Message Type
        /// </summary>
        /// <typeparam name="T0"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <typeparam name="T6"></typeparam>
        /// <typeparam name="T7"></typeparam>
        /// <typeparam name="T8"></typeparam>
        /// <typeparam name="T9"></typeparam>
        /// <typeparam name="T10"></typeparam>
        /// <typeparam name="T11"></typeparam>
        /// <param name="messageId"></param>
        /// <param name="param0"></param>
        /// <param name="param1"></param>
        /// <param name="param2"></param>
        /// <param name="param3"></param>
        /// <param name="param4"></param>
        /// <param name="param5"></param>
        /// <param name="param6"></param>
        /// <param name="param7"></param>
        /// <param name="param8"></param>
        /// <param name="param9"></param>
        /// <param name="param10"></param>
        /// <param name="param11"></param>
        public static void Post<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(int messageId, T0 param0, T1 param1, T2 param2, T3 param3, T4 param4, T5 param5, T6 param6, T7 param7, T8 param8, T9 param9, T10 param10, T11 param11)
        {
            // get the key for _messageCollections from parameter types
            string key = GetKey(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11));
            // if there is a collection for the given parameter types, check if the collection contains this message ID and invoke the delegate with the provided parameters
            if (_messageCollections.ContainsKey(key))
            {
                MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> collection = (MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>)_messageCollections[key];
                if (collection.MessageGroups.ContainsKey(messageId))
                {
                    collection.MessageGroups[messageId](param0, param1, param2, param3, param4, param5, param6, param7, param8, param9, param10, param11);
                }
            }
        }

        /// <summary>
        /// Posts message to all observers registered for this Message Type
        /// </summary>
        /// <typeparam name="T0"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <typeparam name="T6"></typeparam>
        /// <typeparam name="T7"></typeparam>
        /// <typeparam name="T8"></typeparam>
        /// <typeparam name="T9"></typeparam>
        /// <typeparam name="T10"></typeparam>
        /// <typeparam name="T11"></typeparam>
        /// <typeparam name="T12"></typeparam>
        /// <param name="messageId"></param>
        /// <param name="param0"></param>
        /// <param name="param1"></param>
        /// <param name="param2"></param>
        /// <param name="param3"></param>
        /// <param name="param4"></param>
        /// <param name="param5"></param>
        /// <param name="param6"></param>
        /// <param name="param7"></param>
        /// <param name="param8"></param>
        /// <param name="param9"></param>
        /// <param name="param10"></param>
        /// <param name="param11"></param>
        /// <param name="param12"></param>
        public static void Post<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(int messageId, T0 param0, T1 param1, T2 param2, T3 param3, T4 param4, T5 param5, T6 param6, T7 param7, T8 param8, T9 param9, T10 param10, T11 param11, T12 param12)
        {
            // get the key for _messageCollections from parameter types
            string key = GetKey(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12));
            // if there is a collection for the given parameter types, check if the collection contains this message ID and invoke the delegate with the provided parameters
            if (_messageCollections.ContainsKey(key))
            {
                MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> collection = (MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>)_messageCollections[key];
                if (collection.MessageGroups.ContainsKey(messageId))
                {
                    collection.MessageGroups[messageId](param0, param1, param2, param3, param4, param5, param6, param7, param8, param9, param10, param11, param12);
                }
            }
        }

        /// <summary>
        /// Posts message to all observers registered for this Message Type
        /// </summary>
        /// <typeparam name="T0"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <typeparam name="T6"></typeparam>
        /// <typeparam name="T7"></typeparam>
        /// <typeparam name="T8"></typeparam>
        /// <typeparam name="T9"></typeparam>
        /// <typeparam name="T10"></typeparam>
        /// <typeparam name="T11"></typeparam>
        /// <typeparam name="T12"></typeparam>
        /// <typeparam name="T13"></typeparam>
        /// <param name="messageId"></param>
        /// <param name="param0"></param>
        /// <param name="param1"></param>
        /// <param name="param2"></param>
        /// <param name="param3"></param>
        /// <param name="param4"></param>
        /// <param name="param5"></param>
        /// <param name="param6"></param>
        /// <param name="param7"></param>
        /// <param name="param8"></param>
        /// <param name="param9"></param>
        /// <param name="param10"></param>
        /// <param name="param11"></param>
        /// <param name="param12"></param>
        /// <param name="param13"></param>
        public static void Post<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(int messageId, T0 param0, T1 param1, T2 param2, T3 param3, T4 param4, T5 param5, T6 param6, T7 param7, T8 param8, T9 param9, T10 param10, T11 param11, T12 param12, T13 param13)
        {
            // get the key for _messageCollections from parameter types
            string key = GetKey(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12), typeof(T13));
            // if there is a collection for the given parameter types, check if the collection contains this message ID and invoke the delegate with the provided parameters
            if (_messageCollections.ContainsKey(key))
            {
                MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> collection = (MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>)_messageCollections[key];
                if (collection.MessageGroups.ContainsKey(messageId))
                {
                    collection.MessageGroups[messageId](param0, param1, param2, param3, param4, param5, param6, param7, param8, param9, param10, param11, param12, param13);
                }
            }
        }

        /// <summary>
        /// Posts message to all observers registered for this Message Type
        /// </summary>
        /// <typeparam name="T0"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <typeparam name="T6"></typeparam>
        /// <typeparam name="T7"></typeparam>
        /// <typeparam name="T8"></typeparam>
        /// <typeparam name="T9"></typeparam>
        /// <typeparam name="T10"></typeparam>
        /// <typeparam name="T11"></typeparam>
        /// <typeparam name="T12"></typeparam>
        /// <typeparam name="T13"></typeparam>
        /// <typeparam name="T14"></typeparam>
        /// <param name="messageId"></param>
        /// <param name="param0"></param>
        /// <param name="param1"></param>
        /// <param name="param2"></param>
        /// <param name="param3"></param>
        /// <param name="param4"></param>
        /// <param name="param5"></param>
        /// <param name="param6"></param>
        /// <param name="param7"></param>
        /// <param name="param8"></param>
        /// <param name="param9"></param>
        /// <param name="param10"></param>
        /// <param name="param11"></param>
        /// <param name="param12"></param>
        /// <param name="param13"></param>
        /// <param name="param14"></param>
        public static void Post<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(int messageId, T0 param0, T1 param1, T2 param2, T3 param3, T4 param4, T5 param5, T6 param6, T7 param7, T8 param8, T9 param9, T10 param10, T11 param11, T12 param12, T13 param13, T14 param14)
        {
            // get the key for _messageCollections from parameter types
            string key = GetKey(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12), typeof(T13), typeof(T14));
            // if there is a collection for the given parameter types, check if the collection contains this message ID and invoke the delegate with the provided parameters
            if (_messageCollections.ContainsKey(key))
            {
                MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> collection = (MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>)_messageCollections[key];
                if (collection.MessageGroups.ContainsKey(messageId))
                {
                    collection.MessageGroups[messageId](param0, param1, param2, param3, param4, param5, param6, param7, param8, param9, param10, param11, param12, param13, param14);
                }
            }
        }

        /// <summary>
        /// Posts message to all observers registered for this Message Type
        /// </summary>
        /// <typeparam name="T0"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <typeparam name="T6"></typeparam>
        /// <typeparam name="T7"></typeparam>
        /// <typeparam name="T8"></typeparam>
        /// <typeparam name="T9"></typeparam>
        /// <typeparam name="T10"></typeparam>
        /// <typeparam name="T11"></typeparam>
        /// <typeparam name="T12"></typeparam>
        /// <typeparam name="T13"></typeparam>
        /// <typeparam name="T14"></typeparam>
        /// <typeparam name="T15"></typeparam>
        /// <param name="messageId"></param>
        /// <param name="param0"></param>
        /// <param name="param1"></param>
        /// <param name="param2"></param>
        /// <param name="param3"></param>
        /// <param name="param4"></param>
        /// <param name="param5"></param>
        /// <param name="param6"></param>
        /// <param name="param7"></param>
        /// <param name="param8"></param>
        /// <param name="param9"></param>
        /// <param name="param10"></param>
        /// <param name="param11"></param>
        /// <param name="param12"></param>
        /// <param name="param13"></param>
        /// <param name="param14"></param>
        /// <param name="param15"></param>
        public static void Post<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(int messageId, T0 param0, T1 param1, T2 param2, T3 param3, T4 param4, T5 param5, T6 param6, T7 param7, T8 param8, T9 param9, T10 param10, T11 param11, T12 param12, T13 param13, T14 param14, T15 param15)
        {
            // get the key for _messageCollections from parameter types
            string key = GetKey(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12), typeof(T13), typeof(T14), typeof(T15));
            // if there is a collection for the given parameter types, check if the collection contains this message ID and invoke the delegate with the provided parameters
            if (_messageCollections.ContainsKey(key))
            {
                MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> collection = (MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>)_messageCollections[key];
                if (collection.MessageGroups.ContainsKey(messageId))
                {
                    collection.MessageGroups[messageId](param0, param1, param2, param3, param4, param5, param6, param7, param8, param9, param10, param11, param12, param13, param14, param15);
                }
            }
        }
#endif
        #endregion


        // ######################## UTILITIES ######################## //
        /// <summary>
        /// Creates the key for the given types by appending all the TypeNames to each other
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        private static string GetKey(params Type[] types)
        {
            StringBuilder key = new StringBuilder();
            foreach (Type type in types)
            {
                key.Append(type.FullName.ToString());
            }

            return (key.ToString());
        }

        #region CLEAR_MESSAGE_TABLE
        /// <summary>
        /// Removes all Observers for this Message Type
        /// </summary>
        /// <param name="messageId"></param>
        public static void ClearMessageTable(int messageId)
        {
            // get the key for _messageCollections from parameter types
            string key = "";
            // if there is a collection for the given parameter types, check if it contains the messageID and if it does, delete that messageID
            if (_messageCollections.ContainsKey(key))
            {
                MessageCollection collection = (MessageCollection)_messageCollections[key];
                if (collection.MessageGroups.ContainsKey(messageId))
                    collection.MessageGroups.Remove(messageId);
            }
        }

        /// <summary>
        /// Removes all Observers for this Message Type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="messageId"></param>
        public static void ClearMessageTable<T>(int messageId)
        {
            // get the key for _messageCollections from parameter types
            string key = typeof(T).FullName.ToString();
            // if there is a collection for the given parameter types, check if it contains the messageID and if it does, delete that messageID
            if (_messageCollections.ContainsKey(key))
            {
                MessageCollection<T> collection = (MessageCollection<T>)_messageCollections[key];
                if (collection.MessageGroups.ContainsKey(messageId))
                    collection.MessageGroups.Remove(messageId);
            }
        }

        /// <summary>
        /// Removes all Observers for this Message Type
        /// </summary>
        /// <typeparam name="T0"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <param name="messageId"></param>
        public static void ClearMessageTable<T0, T1>(int messageId)
        {
            // get the key for _messageCollections from parameter types
            string key = GetKey(typeof(T0), typeof(T1));
            // if there is a collection for the given parameter types, check if it contains the messageID and if it does, delete that messageID
            if (_messageCollections.ContainsKey(key))
            {
                MessageCollection<T0, T1> collection = (MessageCollection<T0, T1>)_messageCollections[key];
                if (collection.MessageGroups.ContainsKey(messageId))
                    collection.MessageGroups.Remove(messageId);
            }
        }

        /// <summary>
        /// Removes all Observers for this Message Type
        /// </summary>
        /// <typeparam name="T0"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="messageId"></param>
        public static void ClearMessageTable<T0, T1, T2>(int messageId)
        {
            // get the key for _messageCollections from parameter types
            string key = GetKey(typeof(T0), typeof(T1), typeof(T2));
            // if there is a collection for the given parameter types, check if it contains the messageID and if it does, delete that messageID
            if (_messageCollections.ContainsKey(key))
            {
                MessageCollection<T0, T1, T2> collection = (MessageCollection<T0, T1, T2>)_messageCollections[key];
                if (collection.MessageGroups.ContainsKey(messageId))
                    collection.MessageGroups.Remove(messageId);
            }
        }

        /// <summary>
        /// Removes all Observers for this Message Type
        /// </summary>
        /// <typeparam name="T0"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <param name="messageId"></param>
        public static void ClearMessageTable<T0, T1, T2, T3>(int messageId)
        {
            // get the key for _messageCollections from parameter types
            string key = GetKey(typeof(T0), typeof(T1), typeof(T2), typeof(T3));
            // if there is a collection for the given parameter types, check if it contains the messageID and if it does, delete that messageID
            if (_messageCollections.ContainsKey(key))
            {
                MessageCollection<T0, T1, T2, T3> collection = (MessageCollection<T0, T1, T2, T3>)_messageCollections[key];
                if (collection.MessageGroups.ContainsKey(messageId))
                    collection.MessageGroups.Remove(messageId);
            }
        }

#if NET_4_6
        /// <summary>
        /// Removes all Observers for this Message Type
        /// </summary>
        /// <typeparam name="T0"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <param name="messageId"></param>
        public static void ClearMessageTable<T0, T1, T2, T3, T4>(int messageId)
        {
            // get the key for _messageCollections from parameter types
            string key = GetKey(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4));
            // if there is a collection for the given parameter types, check if it contains the messageID and if it does, delete that messageID
            if (_messageCollections.ContainsKey(key))
            {
                MessageCollection<T0, T1, T2, T3, T4> collection = (MessageCollection<T0, T1, T2, T3, T4>)_messageCollections[key];
                if (collection.MessageGroups.ContainsKey(messageId))
                    collection.MessageGroups.Remove(messageId);
            }
        }

        /// <summary>
        /// Removes all Observers for this Message Type
        /// </summary>
        /// <typeparam name="T0"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <param name="messageId"></param>
        public static void ClearMessageTable<T0, T1, T2, T3, T4, T5>(int messageId)
        {
            // get the key for _messageCollections from parameter types
            string key = GetKey(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5));
            // if there is a collection for the given parameter types, check if it contains the messageID and if it does, delete that messageID
            if (_messageCollections.ContainsKey(key))
            {
                MessageCollection<T0, T1, T2, T3, T4, T5> collection = (MessageCollection<T0, T1, T2, T3, T4, T5>)_messageCollections[key];
                if (collection.MessageGroups.ContainsKey(messageId))
                    collection.MessageGroups.Remove(messageId);
            }
        }

        /// <summary>
        /// Removes all Observers for this Message Type
        /// </summary>
        /// <typeparam name="T0"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <typeparam name="T6"></typeparam>
        /// <param name="messageId"></param>
        public static void ClearMessageTable<T0, T1, T2, T3, T4, T5, T6>(int messageId)
        {
            // get the key for _messageCollections from parameter types
            string key = GetKey(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6));
            // if there is a collection for the given parameter types, check if it contains the messageID and if it does, delete that messageID
            if (_messageCollections.ContainsKey(key))
            {
                MessageCollection<T0, T1, T2, T3, T4, T5, T6> collection = (MessageCollection<T0, T1, T2, T3, T4, T5, T6>)_messageCollections[key];
                if (collection.MessageGroups.ContainsKey(messageId))
                    collection.MessageGroups.Remove(messageId);
            }
        }

        /// <summary>
        /// Removes all Observers for this Message Type
        /// </summary>
        /// <typeparam name="T0"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <typeparam name="T6"></typeparam>
        /// <typeparam name="T7"></typeparam>
        /// <param name="messageId"></param>
        public static void ClearMessageTable<T0, T1, T2, T3, T4, T5, T6, T7>(int messageId)
        {
            // get the key for _messageCollections from parameter types
            string key = GetKey(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7));
            // if there is a collection for the given parameter types, check if it contains the messageID and if it does, delete that messageID
            if (_messageCollections.ContainsKey(key))
            {
                MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7> collection = (MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7>)_messageCollections[key];
                if (collection.MessageGroups.ContainsKey(messageId))
                    collection.MessageGroups.Remove(messageId);
            }
        }

        /// <summary>
        /// Removes all Observers for this Message Type
        /// </summary>
        /// <typeparam name="T0"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <typeparam name="T6"></typeparam>
        /// <typeparam name="T7"></typeparam>
        /// <typeparam name="T8"></typeparam>
        /// <param name="messageId"></param>
        public static void ClearMessageTable<T0, T1, T2, T3, T4, T5, T6, T7, T8>(int messageId)
        {
            // get the key for _messageCollections from parameter types
            string key = GetKey(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8));
            // if there is a collection for the given parameter types, check if it contains the messageID and if it does, delete that messageID
            if (_messageCollections.ContainsKey(key))
            {
                MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7, T8> collection = (MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7, T8>)_messageCollections[key];
                if (collection.MessageGroups.ContainsKey(messageId))
                    collection.MessageGroups.Remove(messageId);
            }
        }

        /// <summary>
        /// Removes all Observers for this Message Type
        /// </summary>
        /// <typeparam name="T0"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <typeparam name="T6"></typeparam>
        /// <typeparam name="T7"></typeparam>
        /// <typeparam name="T8"></typeparam>
        /// <typeparam name="T9"></typeparam>
        /// <param name="messageId"></param>
        public static void ClearMessageTable<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(int messageId)
        {
            // get the key for _messageCollections from parameter types
            string key = GetKey(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9));
            // if there is a collection for the given parameter types, check if it contains the messageID and if it does, delete that messageID
            if (_messageCollections.ContainsKey(key))
            {
                MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> collection = (MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>)_messageCollections[key];
                if (collection.MessageGroups.ContainsKey(messageId))
                    collection.MessageGroups.Remove(messageId);
            }
        }

        /// <summary>
        /// Removes all Observers for this Message Type
        /// </summary>
        /// <typeparam name="T0"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <typeparam name="T6"></typeparam>
        /// <typeparam name="T7"></typeparam>
        /// <typeparam name="T8"></typeparam>
        /// <typeparam name="T9"></typeparam>
        /// <typeparam name="T10"></typeparam>
        /// <param name="messageId"></param>
        public static void ClearMessageTable<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(int messageId)
        {
            // get the key for _messageCollections from parameter types
            string key = GetKey(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10));
            // if there is a collection for the given parameter types, check if it contains the messageID and if it does, delete that messageID
            if (_messageCollections.ContainsKey(key))
            {
                MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> collection = (MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>)_messageCollections[key];
                if (collection.MessageGroups.ContainsKey(messageId))
                    collection.MessageGroups.Remove(messageId);
            }
        }

        /// <summary>
        /// Removes all Observers for this Message Type
        /// </summary>
        /// <typeparam name="T0"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <typeparam name="T6"></typeparam>
        /// <typeparam name="T7"></typeparam>
        /// <typeparam name="T8"></typeparam>
        /// <typeparam name="T9"></typeparam>
        /// <typeparam name="T10"></typeparam>
        /// <typeparam name="T11"></typeparam>
        /// <param name="messageId"></param>
        public static void ClearMessageTable<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(int messageId)
        {
            // get the key for _messageCollections from parameter types
            string key = GetKey(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11));
            // if there is a collection for the given parameter types, check if it contains the messageID and if it does, delete that messageID
            if (_messageCollections.ContainsKey(key))
            {
                MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> collection = (MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>)_messageCollections[key];
                if (collection.MessageGroups.ContainsKey(messageId))
                    collection.MessageGroups.Remove(messageId);
            }
        }

        /// <summary>
        /// Removes all Observers for this Message Type
        /// </summary>
        /// <typeparam name="T0"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <typeparam name="T6"></typeparam>
        /// <typeparam name="T7"></typeparam>
        /// <typeparam name="T8"></typeparam>
        /// <typeparam name="T9"></typeparam>
        /// <typeparam name="T10"></typeparam>
        /// <typeparam name="T11"></typeparam>
        /// <typeparam name="T12"></typeparam>
        /// <param name="messageId"></param>
        public static void ClearMessageTable<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(int messageId)
        {
            // get the key for _messageCollections from parameter types
            string key = GetKey(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12));
            // if there is a collection for the given parameter types, check if it contains the messageID and if it does, delete that messageID
            if (_messageCollections.ContainsKey(key))
            {
                MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> collection = (MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>)_messageCollections[key];
                if (collection.MessageGroups.ContainsKey(messageId))
                    collection.MessageGroups.Remove(messageId);
            }
        }

        /// <summary>
        /// Removes all Observers for this Message Type
        /// </summary>
        /// <typeparam name="T0"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <typeparam name="T6"></typeparam>
        /// <typeparam name="T7"></typeparam>
        /// <typeparam name="T8"></typeparam>
        /// <typeparam name="T9"></typeparam>
        /// <typeparam name="T10"></typeparam>
        /// <typeparam name="T11"></typeparam>
        /// <typeparam name="T12"></typeparam>
        /// <typeparam name="T13"></typeparam>
        /// <param name="messageId"></param>
        public static void ClearMessageTable<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(int messageId)
        {
            // get the key for _messageCollections from parameter types
            string key = GetKey(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12), typeof(T13));
            // if there is a collection for the given parameter types, check if it contains the messageID and if it does, delete that messageID
            if (_messageCollections.ContainsKey(key))
            {
                MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> collection = (MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>)_messageCollections[key];
                if (collection.MessageGroups.ContainsKey(messageId))
                    collection.MessageGroups.Remove(messageId);
            }
        }

        /// <summary>
        /// Removes all Observers for this Message Type
        /// </summary>
        /// <typeparam name="T0"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <typeparam name="T6"></typeparam>
        /// <typeparam name="T7"></typeparam>
        /// <typeparam name="T8"></typeparam>
        /// <typeparam name="T9"></typeparam>
        /// <typeparam name="T10"></typeparam>
        /// <typeparam name="T11"></typeparam>
        /// <typeparam name="T12"></typeparam>
        /// <typeparam name="T13"></typeparam>
        /// <typeparam name="T14"></typeparam>
        /// <param name="messageId"></param>
        public static void ClearMessageTable<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(int messageId)
        {
            // get the key for _messageCollections from parameter types
            string key = GetKey(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12), typeof(T13), typeof(T14));
            // if there is a collection for the given parameter types, check if it contains the messageID and if it does, delete that messageID
            if (_messageCollections.ContainsKey(key))
            {
                MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> collection = (MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>)_messageCollections[key];
                if (collection.MessageGroups.ContainsKey(messageId))
                    collection.MessageGroups.Remove(messageId);
            }
        }

        /// <summary>
        /// Removes all Observers for this Message Type
        /// </summary>
        /// <typeparam name="T0"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <typeparam name="T6"></typeparam>
        /// <typeparam name="T7"></typeparam>
        /// <typeparam name="T8"></typeparam>
        /// <typeparam name="T9"></typeparam>
        /// <typeparam name="T10"></typeparam>
        /// <typeparam name="T11"></typeparam>
        /// <typeparam name="T12"></typeparam>
        /// <typeparam name="T13"></typeparam>
        /// <typeparam name="T14"></typeparam>
        /// <typeparam name="T15"></typeparam>
        /// <param name="messageId"></param>
        public static void ClearMessageTable<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(int messageId)
        {
            // get the key for _messageCollections from parameter types
            string key = GetKey(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12), typeof(T13), typeof(T14), typeof(T15));
            // if there is a collection for the given parameter types, check if it contains the messageID and if it does, delete that messageID
            if (_messageCollections.ContainsKey(key))
            {
                MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> collection = (MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>)_messageCollections[key];
                if (collection.MessageGroups.ContainsKey(messageId))
                    collection.MessageGroups.Remove(messageId);
            }
        }
#endif
        #endregion

        /// <summary>
        /// Removes all Observers for every message Type
        /// </summary>
        public static void ClearAllMessageTables()
        {
            _messageCollections = new Dictionary<string, IMessageCollection>();
        }

        #region LOG_OBSERVERS
        /// <summary>
        /// Creates a string with Target and Method for each entry in the invocation list and logs it to the console
        /// </summary>
        /// <param name="invocationList"></param>
        /// <param name="messageId"></param>
        private static void LogInvocationList(Delegate[] invocationList, int messageId)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("Message Type " + messageId + " has " + invocationList.Length + " Observers.");
            foreach (Delegate del in invocationList)
            {
                sb.AppendLine("Target: " + del.Target + "\t Method: " + del.Method);
            }

            Debug.Log(sb.ToString());
        }

        /// <summary>
        /// Logs all registered Observers for this Message Type to the Console
        /// </summary>
        /// <param name="messageId"></param>
        public static void LogObservers(int messageId)
        {
            // get the key for _messageCollections from parameter types
            string key = "";
            // if there is no collection for this key, log that there are no observers
            if (!_messageCollections.ContainsKey(key))
            {
                Debug.LogFormat("Message Type {0} has no Observers", messageId);
                return;
            }

            // if the collection has no entry for this messageID, log that there are no observers, else log the invocation List
            MessageCollection collection = (MessageCollection)_messageCollections[key];
            if (!collection.MessageGroups.ContainsKey(messageId))
            {
                Debug.LogFormat("Message Type {0} has no Observers", messageId);
                return;
            }

            Delegate[] invocationList = collection.MessageGroups[messageId].GetInvocationList();
            LogInvocationList(invocationList, messageId);
        }

        /// <summary>
        /// Logs all registered Observers for this Message Type to the Console
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="messageId"></param>
        public static void LogObservers<T>(int messageId)
        {
            // get the key for _messageCollections from parameter types
            string key = typeof(T).FullName.ToString();
            // if there is no collection for this key, log that there are no observers
            if (!_messageCollections.ContainsKey(key))
            {
                Debug.LogFormat("Message Type {0} has no Observers", messageId);
                return;
            }

            // if the collection has no entry for this messageID, log that there are no observers, else log the invocation List
            MessageCollection<T> collection = (MessageCollection<T>)_messageCollections[key];
            if (!collection.MessageGroups.ContainsKey(messageId))
            {
                Debug.LogFormat("Message Type {0} has no Observers", messageId);
                return;
            }

            Delegate[] invocationList = collection.MessageGroups[messageId].GetInvocationList();
            LogInvocationList(invocationList, messageId);
        }

        /// <summary>
        /// Logs all registered Observers for this Message Type to the Console
        /// </summary>
        /// <typeparam name="T0"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <param name="messageId"></param>
        public static void LogObservers<T0, T1>(int messageId)
        {
            // get the key for _messageCollections from parameter types
            string key = GetKey(typeof(T0), typeof(T1));
            // if there is no collection for this key, log that there are no observers
            if (!_messageCollections.ContainsKey(key))
            {
                Debug.LogFormat("Message Type {0} has no Observers", messageId);
                return;
            }

            // if the collection has no entry for this messageID, log that there are no observers, else log the invocation List
            MessageCollection<T0, T1> collection = (MessageCollection<T0, T1>)_messageCollections[key];
            if (!collection.MessageGroups.ContainsKey(messageId))
            {
                Debug.LogFormat("Message Type {0} has no Observers", messageId);
                return;
            }

            Delegate[] invocationList = collection.MessageGroups[messageId].GetInvocationList();
            LogInvocationList(invocationList, messageId);
        }

        /// <summary>
        /// Logs all registered Observers for this Message Type to the Console
        /// </summary>
        /// <typeparam name="T0"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="messageId"></param>
        public static void LogObservers<T0, T1, T2>(int messageId)
        {
            // get the key for _messageCollections from parameter types
            string key = GetKey(typeof(T0), typeof(T1), typeof(T2));
            // if there is no collection for this key, log that there are no observers
            if (!_messageCollections.ContainsKey(key))
            {
                Debug.LogFormat("Message Type {0} has no Observers", messageId);
                return;
            }

            // if the collection has no entry for this messageID, log that there are no observers, else log the invocation List
            MessageCollection<T0, T1, T2> collection = (MessageCollection<T0, T1, T2>)_messageCollections[key];
            if (!collection.MessageGroups.ContainsKey(messageId))
            {
                Debug.LogFormat("Message Type {0} has no Observers", messageId);
                return;
            }

            Delegate[] invocationList = collection.MessageGroups[messageId].GetInvocationList();
            LogInvocationList(invocationList, messageId);
        }

        /// <summary>
        /// Logs all registered Observers for this Message Type to the Console
        /// </summary>
        /// <typeparam name="T0"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <param name="messageId"></param>
        public static void LogObservers<T0, T1, T2, T3>(int messageId)
        {
            // get the key for _messageCollections from parameter types
            string key = GetKey(typeof(T0), typeof(T1), typeof(T2), typeof(T3));
            // if there is no collection for this key, log that there are no observers
            if (!_messageCollections.ContainsKey(key))
            {
                Debug.LogFormat("Message Type {0} has no Observers", messageId);
                return;
            }

            // if the collection has no entry for this messageID, log that there are no observers, else log the invocation List
            MessageCollection<T0, T1, T2, T3> collection = (MessageCollection<T0, T1, T2, T3>)_messageCollections[key];
            if (!collection.MessageGroups.ContainsKey(messageId))
            {
                Debug.LogFormat("Message Type {0} has no Observers", messageId);
                return;
            }

            Delegate[] invocationList = collection.MessageGroups[messageId].GetInvocationList();
            LogInvocationList(invocationList, messageId);
        }

#if NET_4_6
        /// <summary>
        /// Logs all registered Observers for this Message Type to the Console
        /// </summary>
        /// <typeparam name="T0"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <param name="messageId"></param>
        public static void LogObservers<T0, T1, T2, T3, T4>(int messageId)
        {
            // get the key for _messageCollections from parameter types
            string key = GetKey(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4));
            // if there is no collection for this key, log that there are no observers
            if (!_messageCollections.ContainsKey(key))
            {
                Debug.LogFormat("Message Type {0} has no Observers", messageId);
                return;
            }

            // if the collection has no entry for this messageID, log that there are no observers, else log the invocation List
            MessageCollection<T0, T1, T2, T3, T4> collection = (MessageCollection<T0, T1, T2, T3, T4>)_messageCollections[key];
            if (!collection.MessageGroups.ContainsKey(messageId))
            {
                Debug.LogFormat("Message Type {0} has no Observers", messageId);
                return;
            }

            Delegate[] invocationList = collection.MessageGroups[messageId].GetInvocationList();
            LogInvocationList(invocationList, messageId);
        }

        /// <summary>
        /// Logs all registered Observers for this Message Type to the Console
        /// </summary>
        /// <typeparam name="T0"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <param name="messageId"></param>
        public static void LogObservers<T0, T1, T2, T3, T4, T5>(int messageId)
        {
            // get the key for _messageCollections from parameter types
            string key = GetKey(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5));
            // if there is no collection for this key, log that there are no observers
            if (!_messageCollections.ContainsKey(key))
            {
                Debug.LogFormat("Message Type {0} has no Observers", messageId);
                return;
            }

            // if the collection has no entry for this messageID, log that there are no observers, else log the invocation List
            MessageCollection<T0, T1, T2, T3, T4, T5> collection = (MessageCollection<T0, T1, T2, T3, T4, T5>)_messageCollections[key];
            if (!collection.MessageGroups.ContainsKey(messageId))
            {
                Debug.LogFormat("Message Type {0} has no Observers", messageId);
                return;
            }

            Delegate[] invocationList = collection.MessageGroups[messageId].GetInvocationList();
            LogInvocationList(invocationList, messageId);
        }

        /// <summary>
        /// Logs all registered Observers for this Message Type to the Console
        /// </summary>
        /// <typeparam name="T0"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <typeparam name="T6"></typeparam>
        /// <param name="messageId"></param>
        public static void LogObservers<T0, T1, T2, T3, T4, T5, T6>(int messageId)
        {
            // get the key for _messageCollections from parameter types
            string key = GetKey(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6));
            // if there is no collection for this key, log that there are no observers
            if (!_messageCollections.ContainsKey(key))
            {
                Debug.LogFormat("Message Type {0} has no Observers", messageId);
                return;
            }

            // if the collection has no entry for this messageID, log that there are no observers, else log the invocation List
            MessageCollection<T0, T1, T2, T3, T4, T5, T6> collection = (MessageCollection<T0, T1, T2, T3, T4, T5, T6>)_messageCollections[key];
            if (!collection.MessageGroups.ContainsKey(messageId))
            {
                Debug.LogFormat("Message Type {0} has no Observers", messageId);
                return;
            }

            Delegate[] invocationList = collection.MessageGroups[messageId].GetInvocationList();
            LogInvocationList(invocationList, messageId);
        }

        /// <summary>
        /// Logs all registered Observers for this Message Type to the Console
        /// </summary>
        /// <typeparam name="T0"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <typeparam name="T6"></typeparam>
        /// <typeparam name="T7"></typeparam>
        /// <param name="messageId"></param>
        public static void LogObservers<T0, T1, T2, T3, T4, T5, T6, T7>(int messageId)
        {
            // get the key for _messageCollections from parameter types
            string key = GetKey(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7));
            // if there is no collection for this key, log that there are no observers
            if (!_messageCollections.ContainsKey(key))
            {
                Debug.LogFormat("Message Type {0} has no Observers", messageId);
                return;
            }

            // if the collection has no entry for this messageID, log that there are no observers, else log the invocation List
            MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7> collection = (MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7>)_messageCollections[key];
            if (!collection.MessageGroups.ContainsKey(messageId))
            {
                Debug.LogFormat("Message Type {0} has no Observers", messageId);
                return;
            }

            Delegate[] invocationList = collection.MessageGroups[messageId].GetInvocationList();
            LogInvocationList(invocationList, messageId);
        }

        /// <summary>
        /// Logs all registered Observers for this Message Type to the Console
        /// </summary>
        /// <typeparam name="T0"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <typeparam name="T6"></typeparam>
        /// <typeparam name="T7"></typeparam>
        /// <typeparam name="T8"></typeparam>
        /// <param name="messageId"></param>
        public static void LogObservers<T0, T1, T2, T3, T4, T5, T6, T7, T8>(int messageId)
        {
            // get the key for _messageCollections from parameter types
            string key = GetKey(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8));
            // if there is no collection for this key, log that there are no observers
            if (!_messageCollections.ContainsKey(key))
            {
                Debug.LogFormat("Message Type {0} has no Observers", messageId);
                return;
            }

            // if the collection has no entry for this messageID, log that there are no observers, else log the invocation List
            MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7, T8> collection = (MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7, T8>)_messageCollections[key];
            if (!collection.MessageGroups.ContainsKey(messageId))
            {
                Debug.LogFormat("Message Type {0} has no Observers", messageId);
                return;
            }

            Delegate[] invocationList = collection.MessageGroups[messageId].GetInvocationList();
            LogInvocationList(invocationList, messageId);
        }

        /// <summary>
        /// Logs all registered Observers for this Message Type to the Console
        /// </summary>
        /// <typeparam name="T0"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <typeparam name="T6"></typeparam>
        /// <typeparam name="T7"></typeparam>
        /// <typeparam name="T8"></typeparam>
        /// <typeparam name="T9"></typeparam>
        /// <param name="messageId"></param>
        public static void LogObservers<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(int messageId)
        {
            // get the key for _messageCollections from parameter types
            string key = GetKey(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9));
            // if there is no collection for this key, log that there are no observers
            if (!_messageCollections.ContainsKey(key))
            {
                Debug.LogFormat("Message Type {0} has no Observers", messageId);
                return;
            }

            // if the collection has no entry for this messageID, log that there are no observers, else log the invocation List
            MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> collection = (MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>)_messageCollections[key];
            if (!collection.MessageGroups.ContainsKey(messageId))
            {
                Debug.LogFormat("Message Type {0} has no Observers", messageId);
                return;
            }

            Delegate[] invocationList = collection.MessageGroups[messageId].GetInvocationList();
            LogInvocationList(invocationList, messageId);
        }

        /// <summary>
        /// Logs all registered Observers for this Message Type to the Console
        /// </summary>
        /// <typeparam name="T0"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <typeparam name="T6"></typeparam>
        /// <typeparam name="T7"></typeparam>
        /// <typeparam name="T8"></typeparam>
        /// <typeparam name="T9"></typeparam>
        /// <typeparam name="T10"></typeparam>
        /// <param name="messageId"></param>
        public static void LogObservers<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(int messageId)
        {
            // get the key for _messageCollections from parameter types
            string key = GetKey(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10));
            // if there is no collection for this key, log that there are no observers
            if (!_messageCollections.ContainsKey(key))
            {
                Debug.LogFormat("Message Type {0} has no Observers", messageId);
                return;
            }

            // if the collection has no entry for this messageID, log that there are no observers, else log the invocation List
            MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> collection = (MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>)_messageCollections[key];
            if (!collection.MessageGroups.ContainsKey(messageId))
            {
                Debug.LogFormat("Message Type {0} has no Observers", messageId);
                return;
            }

            Delegate[] invocationList = collection.MessageGroups[messageId].GetInvocationList();
            LogInvocationList(invocationList, messageId);
        }

        /// <summary>
        /// Logs all registered Observers for this Message Type to the Console
        /// </summary>
        /// <typeparam name="T0"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <typeparam name="T6"></typeparam>
        /// <typeparam name="T7"></typeparam>
        /// <typeparam name="T8"></typeparam>
        /// <typeparam name="T9"></typeparam>
        /// <typeparam name="T10"></typeparam>
        /// <typeparam name="T11"></typeparam>
        /// <param name="messageId"></param>
        public static void LogObservers<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(int messageId)
        {
            // get the key for _messageCollections from parameter types
            string key = GetKey(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11));
            // if there is no collection for this key, log that there are no observers
            if (!_messageCollections.ContainsKey(key))
            {
                Debug.LogFormat("Message Type {0} has no Observers", messageId);
                return;
            }

            // if the collection has no entry for this messageID, log that there are no observers, else log the invocation List
            MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> collection = (MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>)_messageCollections[key];
            if (!collection.MessageGroups.ContainsKey(messageId))
            {
                Debug.LogFormat("Message Type {0} has no Observers", messageId);
                return;
            }

            Delegate[] invocationList = collection.MessageGroups[messageId].GetInvocationList();
            LogInvocationList(invocationList, messageId);
        }

        /// <summary>
        /// Logs all registered Observers for this Message Type to the Console
        /// </summary>
        /// <typeparam name="T0"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <typeparam name="T6"></typeparam>
        /// <typeparam name="T7"></typeparam>
        /// <typeparam name="T8"></typeparam>
        /// <typeparam name="T9"></typeparam>
        /// <typeparam name="T10"></typeparam>
        /// <typeparam name="T11"></typeparam>
        /// <typeparam name="T12"></typeparam>
        /// <param name="messageId"></param>
        public static void LogObservers<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(int messageId)
        {
            // get the key for _messageCollections from parameter types
            string key = GetKey(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12));
            // if there is no collection for this key, log that there are no observers
            if (!_messageCollections.ContainsKey(key))
            {
                Debug.LogFormat("Message Type {0} has no Observers", messageId);
                return;
            }

            // if the collection has no entry for this messageID, log that there are no observers, else log the invocation List
            MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> collection = (MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>)_messageCollections[key];
            if (!collection.MessageGroups.ContainsKey(messageId))
            {
                Debug.LogFormat("Message Type {0} has no Observers", messageId);
                return;
            }

            Delegate[] invocationList = collection.MessageGroups[messageId].GetInvocationList();
            LogInvocationList(invocationList, messageId);
        }

        /// <summary>
        /// Logs all registered Observers for this Message Type to the Console
        /// </summary>
        /// <typeparam name="T0"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <typeparam name="T6"></typeparam>
        /// <typeparam name="T7"></typeparam>
        /// <typeparam name="T8"></typeparam>
        /// <typeparam name="T9"></typeparam>
        /// <typeparam name="T10"></typeparam>
        /// <typeparam name="T11"></typeparam>
        /// <typeparam name="T12"></typeparam>
        /// <typeparam name="T13"></typeparam>
        /// <param name="messageId"></param>
        public static void LogObservers<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(int messageId)
        {
            // get the key for _messageCollections from parameter types
            string key = GetKey(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12), typeof(T13));
            // if there is no collection for this key, log that there are no observers
            if (!_messageCollections.ContainsKey(key))
            {
                Debug.LogFormat("Message Type {0} has no Observers", messageId);
                return;
            }

            // if the collection has no entry for this messageID, log that there are no observers, else log the invocation List
            MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> collection = (MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>)_messageCollections[key];
            if (!collection.MessageGroups.ContainsKey(messageId))
            {
                Debug.LogFormat("Message Type {0} has no Observers", messageId);
                return;
            }

            Delegate[] invocationList = collection.MessageGroups[messageId].GetInvocationList();
            LogInvocationList(invocationList, messageId);
        }

        /// <summary>
        /// Logs all registered Observers for this Message Type to the Console
        /// </summary>
        /// <typeparam name="T0"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <typeparam name="T6"></typeparam>
        /// <typeparam name="T7"></typeparam>
        /// <typeparam name="T8"></typeparam>
        /// <typeparam name="T9"></typeparam>
        /// <typeparam name="T10"></typeparam>
        /// <typeparam name="T11"></typeparam>
        /// <typeparam name="T12"></typeparam>
        /// <typeparam name="T13"></typeparam>
        /// <typeparam name="T14"></typeparam>
        /// <param name="messageId"></param>
        public static void LogObservers<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(int messageId)
        {
            // get the key for _messageCollections from parameter types
            string key = GetKey(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12), typeof(T13), typeof(T14));
            // if there is no collection for this key, log that there are no observers
            if (!_messageCollections.ContainsKey(key))
            {
                Debug.LogFormat("Message Type {0} has no Observers", messageId);
                return;
            }

            // if the collection has no entry for this messageID, log that there are no observers, else log the invocation List
            MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> collection = (MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>)_messageCollections[key];
            if (!collection.MessageGroups.ContainsKey(messageId))
            {
                Debug.LogFormat("Message Type {0} has no Observers", messageId);
                return;
            }

            Delegate[] invocationList = collection.MessageGroups[messageId].GetInvocationList();
            LogInvocationList(invocationList, messageId);
        }

        /// <summary>
        /// Logs all registered Observers for this Message Type to the Console
        /// </summary>
        /// <typeparam name="T0"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <typeparam name="T6"></typeparam>
        /// <typeparam name="T7"></typeparam>
        /// <typeparam name="T8"></typeparam>
        /// <typeparam name="T9"></typeparam>
        /// <typeparam name="T10"></typeparam>
        /// <typeparam name="T11"></typeparam>
        /// <typeparam name="T12"></typeparam>
        /// <typeparam name="T13"></typeparam>
        /// <typeparam name="T14"></typeparam>
        /// <typeparam name="T15"></typeparam>
        /// <param name="messageId"></param>
        public static void LogObservers<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(int messageId)
        {
            // get the key for _messageCollections from parameter types
            string key = GetKey(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12), typeof(T13), typeof(T14), typeof(T15));
            // if there is no collection for this key, log that there are no observers
            if (!_messageCollections.ContainsKey(key))
            {
                Debug.LogFormat("Message Type {0} has no Observers", messageId);
                return;
            }

            // if the collection has no entry for this messageID, log that there are no observers, else log the invocation List
            MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> collection = (MessageCollection<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>)_messageCollections[key];
            if (!collection.MessageGroups.ContainsKey(messageId))
            {
                Debug.LogFormat("Message Type {0} has no Observers", messageId);
                return;
            }

            Delegate[] invocationList = collection.MessageGroups[messageId].GetInvocationList();
            LogInvocationList(invocationList, messageId);
        }
#endif
        #endregion
    }
}
