using System.Collections.Generic;
using FK.Utility.ArraysAndLists;

namespace UnityEngine.Networking.NetworkSystem
{
    /// <summary>
    /// A Manager for Network Messages. This enables you to have multiple scripts monitoring the same network message and all of them actually receiving it.
    /// If you register a handler for a network message, the message will be removed after one handler received it. So if multiple handlers are monitoring the same
    /// message, only one of them will actually receive it.
    /// With this manager you can have as many scripts as you like monitoring the message, and you get the message allready deserialized back
    /// 
    /// Make sure to clean up handlers when deleting objects or changing scenes, as this class is static and everything persists!
    /// </summary>
    public static class NetworkMessageManager
    {
        // ######################## HELPER CLASSES ######################## //
        /// <summary>
        /// Non generic base class for HandlerContainers for easier handling
        /// </summary>
        public abstract class HandlerContainerBase
        {
            /// <summary>
            /// Type of messages handled by this container
            /// </summary>
            public short MessageId { get; protected set; }

            /// <summary>
            /// The NetworkClient this handler is registered on. NULL if on Server
            /// </summary>
            public NetworkClient Client;

            /// <summary>
            /// Is this handler registered on the server?
            /// </summary>
            protected bool _server;

            /// <summary>
            /// Removes the message Handler
            /// </summary>
            public void ClearHandler()
            {
                if (_server)
                {
                    NetworkServer.UnregisterHandler(MessageId);
                }
                else
                {
                    Client.UnregisterHandler(MessageId);
                }
            }
        }

        /// <summary>
        /// Container class that receives network messages and forewards them to all receivers
        /// </summary>
        /// <typeparam name="T">A Network Message Type. Needs to be derived from MessageBase</typeparam>
        public class HandlerContainer<T> : HandlerContainerBase where T : MessageBase, new()
        {

            /// <summary>
            /// Callback when message is recieved
            /// </summary>
            public ReceivedNetworkMessageDelegate<T> OnMessageReceived;

            /// <summary>
            /// Creates a new Handler Container
            /// </summary>
            /// <param name="msgId">The type of messages handled by this</param>
            /// <param name="server">If true this registers a handler on the server, if false on the client</param>
            public HandlerContainer(short msgId, bool server, NetworkClient client)
            {
                MessageId = msgId;
                Client = client;
                _server = server;

                if (server)
                {
                    NetworkServer.RegisterHandler(msgId, MessageHandler);
                }
                else
                {
                    client.RegisterHandler(msgId, MessageHandler);
                }
            }

            /// <summary>
            /// Removes the provided delegate from the OnMessageReceived Event
            /// </summary>
            /// <param name="del">Delegate to remove</param>
            public bool UnregisterDelegate(ReceivedNetworkMessageDelegate<T> del)
            {
                if (OnMessageReceived.GetInvocationList().Search(del) != -1)
                {
                    OnMessageReceived -= del;
                    return true;
                }

                return false;
            }

            /// <summary>
            /// The function for receiving the message, reading it and the distributing it to all observers
            /// </summary>
            /// <param name="msg"></param>
            private void MessageHandler(NetworkMessage msg)
            {
                // read the message
                T message = msg.ReadMessage<T>();

                // notify everyone
                if (OnMessageReceived != null)
                {
                    OnMessageReceived(message);
                }
            }
        }

        // ######################## ENUMS & DELEGATES ######################## //
        /// <summary>
        /// Delegate for Network message receiver functions
        /// </summary>
        /// <typeparam name="T">A Network Message Type. Needs to be derived from MessageBase</typeparam>
        /// <param name="msg">The deserialized message</param>
        public delegate void ReceivedNetworkMessageDelegate<T>(T msg) where T : MessageBase, new();


        // ######################## PUBLIC VARS ######################## //


        // ######################## PRIVATE VARS ######################## //
        /// <summary>
        /// List of all handler containers for the different message types
        /// </summary>
        private static List<HandlerContainerBase> _handlerContainers = new List<HandlerContainerBase>();



        // ######################## UNITY START & UPDATE ######################## //



        // ######################## INITS ######################## //
        /// <summary>
        /// Removes all registered handlers
        /// </summary>
        public static void ClearAllHandlers()
        {
            foreach(HandlerContainerBase handler in _handlerContainers)
            {
                handler.ClearHandler();
            }

            _handlerContainers = new List<HandlerContainerBase>();
        }

        // ######################## FUNCTIONALITY ######################## //
        /// <summary>
        /// Register a Handler
        /// </summary>
        /// <typeparam name="T">A Network Message Type. Needs to be derived from MessageBase</typeparam>
        /// <param name="server">Register on server or on client?</param>
        /// <param name="msgType">Message type number</param>
        /// <param name="handler">Function handler which will be invoked when this type of message is recieved</param>
        /// <param name="client">If registering on a client, this is the client to register to</param>
        private static void RegisterHandler<T>(bool server, short msgType, ReceivedNetworkMessageDelegate<T> handler, NetworkClient client = null) where T : MessageBase, new()
        {
            // go through all handler containers and see if we allready have one for this msgType
            foreach (HandlerContainerBase container in _handlerContainers)
            {
                if (container.MessageId == msgType) // If we found one, register the handler function
                {
                    if(!server && container.Client != client)
                    {
                        continue;
                    }

                    HandlerContainer<T> c = (HandlerContainer<T>)container;
                    c.OnMessageReceived += handler;
                    return;
                }
            }

            // if we reach this it means there is no container for this type of message yet, so we need to add one
            HandlerContainer<T> newContainer = new HandlerContainer<T>(msgType, server, client);
            newContainer.OnMessageReceived += handler;

            _handlerContainers.Add(newContainer);
        }

        /// <summary>
        /// Unregister a Handler
        /// </summary>
        /// <typeparam name="T">A Network Message Type. Needs to be derived from MessageBase</typeparam>
        /// <param name="msgType">Message type number</param>
        /// <param name="handler">Delegate to unregister</param>
        public static void UnregisterHandler<T>(short msgType, ReceivedNetworkMessageDelegate<T> handler) where T : MessageBase, new()
        {
            // go through all handler containers and look for the handler
            foreach (HandlerContainerBase container in _handlerContainers)
            {
                if (container.MessageId == msgType) // if we found the right handler container, unregister the delegate
                {
                    HandlerContainer<T> c = (HandlerContainer<T>)container;
                    if(!c.UnregisterDelegate(handler))
                    {
                        continue;
                    }
                    return;
                }
            }

            Debug.LogWarningFormat("Could not find {0} in registered handlers", handler.Method.Name);
        }


        /// <summary>
        /// Register a Handler for messages send to the server
        /// </summary>
        /// <typeparam name="T">A Network Message Type. Needs to be derived from MessageBase</typeparam>
        /// <param name="msgType">Message type number</param>
        /// <param name="handler">unction handler which will be invoked when this type of message is recieved</param>
        public static void RegisterHandlerServer<T>(short msgType, ReceivedNetworkMessageDelegate<T> handler) where T : MessageBase, new()
        {
            if(!NetworkServer.active)
            {
                Debug.LogError("Cannot Register a Handler on the Server because the this is not a Server");
                return;
            }
            //===================== code after this is only run on server ====================

            RegisterHandler<T>(true, msgType, handler);
        }

        /// <summary>
        /// Register a Handler for messages send to the client
        /// </summary>
        /// <typeparam name="T">A Network Message Type. Needs to be derived from MessageBase</typeparam>
        /// <param name="msgType">Message type number</param>
        /// <param name="handler">unction handler which will be invoked when this type of message is recieved</param>
        /// <param name="client">The client to register to</param>
        public static void RegisterHandlerClient<T>(short msgType, ReceivedNetworkMessageDelegate<T> handler, NetworkClient client) where T : MessageBase, new()
        {
            if(!NetworkClient.active)
            {
                Debug.LogError("Cannot Register a Handler on the Client because the this is not a Client");
                return;
            }
            //===================== code after this is only run on client ====================

            RegisterHandler<T>(false, msgType, handler, client);
        }

        /// <summary>
        /// Register a Handler for messages send to the client
        /// </summary>
        /// <typeparam name="T">A Network Message Type. Needs to be derived from MessageBase</typeparam>
        /// <param name="msgType">Message type number</param>
        /// <param name="handler">unction handler which will be invoked when this type of message is recieved</param>
        public static void RegisterHandlerClient<T>(short msgType, ReceivedNetworkMessageDelegate<T> handler) where T : MessageBase, new()
        {
            if (!NetworkClient.active)
            {
                Debug.LogError("Cannot Register a Handler on the Client because the this is not a Client");
                return;
            }
            //===================== code after this is only run on client ====================

            RegisterHandler<T>(false, msgType, handler, NetworkClient.allClients[0]);
        }

        /// <summary>
        /// Sends a network message from the server to all clients
        /// </summary>
        /// <param name="msgType">Message type number</param>
        /// <param name="msg">Message instance to send</param>
        public static void SendToClients(short msgType, MessageBase msg)
        {
            if (!NetworkServer.active)
            {
                Debug.LogError("Cannot Send to Clients because this is not a Server");
                return;
            }
            //===================== code after this is only run on server ====================

            NetworkServer.SendToAll(msgType, msg);
        }

        /// <summary>
        /// Sends a network Message from the client to the server
        /// </summary>
        /// <param name="msgType">Message type number</param>
        /// <param name="msg">Message instance to send</param>
        /// <param name="client">Client to send from</param>
        public static void SendToServer(short msgType, MessageBase msg, NetworkClient client)
        {
            if (!NetworkClient.active)
            {
                Debug.LogError("Cannot Send to Server because the this is not a Client");
                return;
            }
            //===================== code after this is only run on client ====================

            if(client == null)
            {
                Debug.LogError("Cannot send from Client that is NULL");
                return;
            }

            client.Send(msgType, msg);
        }

        /// <summary>
        /// Sends a network Message from the client to the server
        /// </summary>
        /// <param name="msgType">Message type number</param>
        /// <param name="msg">Message instance to send</param>
        public static void SendToServer(short msgType, MessageBase msg)
        {
            SendToServer(msgType, msg, NetworkClient.allClients[0]);
        }
    }
}