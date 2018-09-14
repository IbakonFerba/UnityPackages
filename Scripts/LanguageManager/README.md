# Language Manager

This package contains a Language Management System in the namespace `FK.Language`.

Features:
- Static Language Manager that is not bound to a scene
  - Strings are loaded from a .json file in the Streaming Assets folder (allows updating the strings even after the Build Process is done without having to build again)
    - structured into different categories
  - Asynchonous loading by default, synchronous loading possible (but not recommended)
  - Language Changing at runtime with no need to restart the application
  - Can use the System Language of the user running the app as the default language (if there are strings for it)
  - Saves the last used Language and uses that on the next startup (can be disabled)
  - Supports rich text
  
- Language Texts for Legacy and Text Mesh Pro which set their text automatically whenever the language is changed

- Strings File Editor running inside the Unity Editor for an intuitive way of editing the strings and a guarateed working structure of the .json file (can be opened via _Window/Language Manager_)
  - Automatic generation of the file with placeholder text from all Language Texts in all Scenes
  - Adding, removing and renaming of Languages
  - Adding, removing, renaming, editing and moving of Categories and Language dependent strings
  - Duplicating of existing categories

- Automatic setup: Add Language Texts to your scenes, Open the Language Manager Editor and let it generate the file with placeholder strings and everything works!
