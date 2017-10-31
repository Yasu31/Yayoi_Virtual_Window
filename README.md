# Virtual_Window_3

## Locate face position via faceOSC

Requires [FaceOSC](https://github.com/kylemcdonald/ofxFaceTracker/releases) (by Kyle McDonald) to run in the background in order to receive head position information.
Uses [UnityOSC](http://thomasfredericks.github.io/UnityOSC/) (by Thomas Fredericks) to receive that information via OSC.
OSC(Open Sound Control) is a network protocol for transferring data between apps and devices.

## Calculating face position

gets raw values of face's X, Y & scale from FaceOSC via OSC, and computes real life coordinates of user's face. Several parameters about the camera and display must be fine-tuned to get an accurate value.

## Displays scene

A Yayoi mirror is placed in scene, but it is much too heavy to handle with my computer at the moment- must reduce vertices.
It uses a custom projection matrix, in order to emulate a CAVE environment for the user.
