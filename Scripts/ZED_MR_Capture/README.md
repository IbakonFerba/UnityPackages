# ZED_MR_Capture

## Introduction
The [Stereolabs ZED](https://www.stereolabs.com/zed/) camera is a stereoscopic high definition camera that produces depth the same way the human brain does, by measuring the offset between two captured images.

It can be used for a multitude of different things, from photogrammetry over mixed reality pass through apps to - you guessed it - mixed reality capture. For the latter, it has a few important features: It provides per pixel occlusion between the real world and the virtual world as well as virtual lighting on real world objects and the toolkit already has a green screen support out of the box (at least for Unity). This, however, you have to set up in the editor and once the project is built, all the settings are permanent, which means the build will only work for the specific green screen setup you tested it with.

The goal of this project was to create a flexible green screen solution which you just have to drop into a scene as a Prefab and that is editable at runtime.

## Requirements

* ZED camera with some way to attach a Vive tracker (e.g. the 3D printed tracker mount)
* Vive tracker with tracking setup
* SteamVR for tracking the tracker
* The latest version of the [ZED Plugin for Unity] (https://github.com/stereolabs/zed-unity/releases/) (This package was created with [v2.7](https://github.com/stereolabs/zed-unity/releases/tag/v2.7.0))
* The fitting version of the [ZED SDK](https://www.stereolabs.com/developers/release/) (The version you need is specified in the *Compatibility* section at the Unity plugin download, this package was created with v2.7)
* The fitting Unity version (The version you need is specified in the *Compatibility* section at the Unity plugin download, for this package I used Unity 2018.2)
* The ZED Mixed Reality Capture package


* Optionally an *active* USB 3 extension cord (the builtin one is pretty short)

And of course a Unity VR application that should be captured ;)

## Setting up the Hardware
Mount the Vive tracker on the ZED camera as firmly as possible. The only thing that is important with this is that the tracker is not rotated about the Y axis relative to the ZED camera and that it faces roughly forward measured on the X axis. The rotation and position offset can then be corrected in the software later.

Make sure the ZED is plugged into a USB 3 port, otherwise it will not work.

Once you have plugged in the Vive tracker dongle and paired the tracker, you are good to go!

## Setting up the Software
### ZED SDK
First, you need to install the SDK. This is a pretty straight forward process for the most part, just follow the instructions of the installer.

At some point, however, it will install NVIDIA Cuda, which in my experience is a tricky step. You have to **disable the Visual Studio Integration** in the Cuda installer, otherwise the installation will fail every time. If the integrated Cuda installer of the SDK does not work, you can download the correct version of Cuda from the [NVIDIA website](https://developer.nvidia.com/cuda-zone) and install it manually (If you need to do this, make sure the SDK is completely uninstalled, then run the Cuda installer first and once that is finished run the SDK installer)

Once the SDK installation is finished, you can test if it was successful by running the *ZED Diagnostics* tool that was installed with the SDK.

### Unity Project
Once you have installed the SDK, you can set up your project.

First, you need to install the [ZED Plugin for Unity](https://github.com/stereolabs/zed-unity/releases/) and SteamVR. When installing the ZED package, you might get some compiler errors. These happen in some example scripts for some reason, you will have to comment out the broken regions in those scripts.

Once you have those two packages installed, you can install the ZED Mixed Reality Capture package.

Inside the folder *ZED_MR_Capture/Prefabs* you will find the *MR_Capture_Rig* prefab, which you just have to drop into your scene. Now you just have to assign a SteamVR pose action that is using the correct tracker to the SteamVR Behaviour Pose.

## Using the MR Capture
The output of the MR capture is shown on the PC screen.

When you start your game for the first time, you will be guided through the rotation and offset calibration, which will be saved in the Streaming Assets once you are done. At every new start, you are asked if you want to load the existing config or want to calibrate from scratch.

With **H** you can hide or unhide the UI that lets you edit the green screen settings (You can change this key in the ZED Green Screen Options component inside the prefab). You can set up the chroma key settings here as well as the Garbage Matte and also save your settings into the Streaming Assets folder.

## Garbage Matte
The Garbage Matte allows you to mask out the areas that are outside the Green Screen to only show the virtual world there. Once you enter the Garbage Matte edit mode, you can place the corners of matte quads with the mouse cursor. Inside these quads, chroma key will be applied, outside the real world image will be discarded.
