![GitHub release (latest SemVer)](https://img.shields.io/github/v/release/dante-signal31/Unity-Measuring-Tape)
[![License](https://img.shields.io/badge/License-BSD%203--Clause-blue.svg)](https://opensource.org/licenses/BSD-3-Clause)
[![GitHub issues](https://img.shields.io/github/issues/dante-signal31/Unity-Measuring-Tape)](https://github.com/dante-signal31/Unity-Measuring-Tape/issues)
[![GitHub commit activity](https://img.shields.io/github/commit-activity/y/dante-signal31/Unity-Measuring-Tape)](https://github.com/dante-signal31/Unity-Measuring-Tape/commits/main)
[![GitHub last commit](https://img.shields.io/github/last-commit/dante-signal31/Unity-Measuring-Tape)](https://github.com/dante-signal31/Unity-Measuring-Tape/commits/main)

# Unity measuring tape

An Unity tool to measure distances in level scenes.

By default, Unity editor lacks of a built-in tool to measure distances. There are 
plenty of options at the Unity Assets Store, but they are payed ones. With this
tool I want to offer a free and open source alternative. 

## Usage

The most straightforward way to use the tool us dragging the provided prefab into
your scene hierarchy. Then, you can move the entire tape dragging its transform or 
you can drag either of its ends. A line will be drawn between those ends, displaying
measured distance near its center.

Provided prefab is only an empty transform with this tool script attached. You
can use the tool script as a component of your own prefabs. Just be aware that 
the tool is set up to be drawn always, even when the tool object is not selected. 
To hide the measuring tape you must disable its GameObject, or the tool script 
itself in the inspector.

When you move the measuring tape ends, their relative positions are displayed at 
the inspector, where they can be edited too. If you edit them at the inspector, 
the tape ends will be updated accordingly. Just remember that you are editing 
positions relative to measuring tape root transform.

As a convenience, the tool inspector shows the tape ends global positions as 
read only fields under those of the relative position fields.

## Configuration

The tool script comes with default values hardcoded. If you want to change 
those default values you can change them both at the code level or at the 
prefab level.

As it was said before, tool script lets you set the relative positions of
tape ends. Apart from that, every other configuration field refers to visual
appearance. Those configuration fields are:

* *Color*: Color for this tool.
* *Thickness*: Line thickness.
* *End Width*: Width for the perpendicular ends of the line.
* *End Alignment*: End bias.
* *Text Size*: Text size for the distance.
* *Text Distance*: Distance between the text and the line.

Most of those fields are self-explanatory. 

*End Width* refers to the crosslines at the ends of the tape. This field defines
its length. 

By default that crossline at the end is centered with the tape. *End Alignment" is 
a value from -1 to 1 that can be used to displace the crossline to the right or left
of the tape.

You can bias too the distance text position using *Text Distance*. Change it to
place distance text nearer or farther from the tape. Be aware about you can use
negative values to place the text at the other side of the tape.

## Install

You can download the Assets folder of this repository and drag it over your own
Assets folder. Just remember to keep scripts at their proper folders: MonoBehaviour
script at Scripts folder and CustomEditor script at Editor folder.

You may download too the Unity package provided at the Releases section of this 
repository and drag it into your Unity editor.
  

