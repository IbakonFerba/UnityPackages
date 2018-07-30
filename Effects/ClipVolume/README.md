# ClipVolume
This package contains a special Shader, that is only rendered inside a specified Box Volume.
You can create a Volume via the GameObject create menu, it is under *Effects/Clip Volume*. This will create a GameObject with a ClipVolume component. You can define the size of the volume in that component and you will see the volume as a green gizmo. Every child object of the Volume that has one of the Clip Volume Shaders will only be rendered inside that volume.
