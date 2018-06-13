# Messaging
Utility for Messaging

# Packages
## MessageManager
- MessageManager allows you to send Messages to any object that has registered an observer for that message type.
If you are using .NET 4.x these messages can have up to 16 parameters, else they can have up to 4.
It is recommended to define your message IDs as const so they can be easily referenced and identified when you read your code.
The Message Manager is in the namespace `FK.MessageManager`
