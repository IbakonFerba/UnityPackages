# Networking

This is a collection of Networking Scripts for Unity.

# Packages
## NetworkingUtilities
 - `NetworkMessageManager`: A Manager for UNET Network Messages. This enables you to have multiple scripts monitoring the same network message and all of them actually receiving it. If you register a handler for a network message on the Server or the Client, the message will be removed after one handler received it. So if multiple handlers are monitoring the same message, only one of them will actually receive it.
With this manager you can have as many scripts as you like monitoring the message, and you get the message allready deserialized back
Make sure to clean up handlers using `UnregisterHandler` or `ClearAllHandlers` when deleting objects or changing scenes, as this class is static and everything persists!
