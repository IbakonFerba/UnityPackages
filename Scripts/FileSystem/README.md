# File System
Classes for working with the File System of the Device.

# Packages
## FileSystem
- ```FileSystem```: A FileSystem that acesses the File System of the used Device and loads entries as they are needed, while saving them when they where visited so they don't have to be loaded every time. It provides easy traversal of the File Tree while only Displaying Files you want to see if you give it a list of extensions. You can define a path you want to go to and it loads that path and gives you all entries and it provides a history of entered directories. If needed, it can recursively reload all the Nodes that are loaded.
