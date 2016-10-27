# Livestreamer-gui
A livestreamer gui written in c#, with some addedd goodies like:

- History of watched streams, with titles
- Quick option to select quality
- Quick option to get seekable Twitch VODs
- A local favourites list, with titles
- A field to pass arguments to livestreamer as if using the command line

At the moment quick options and titles in the history work only with Twitch, Twitch VODs and YouTube. 
Any other website compatible with livestreamer works leaving all settings as default.

---

## Major known problems

- If livestreamer can't stream a specific video (protected video on youtube or trying to watch a video at an unavailable quality setting) the video won't play and won't signal why. No easy fix for this.

- [Screenshots](http://imgur.com/a/HdpOt)  
- [Download the installer from here](https://1drv.ms/f/s!AlZZhB75siHNg7QzEL3E4rpYW5GCHA)  
- [Todo list](https://trello.com/b/uUrYz6AP/livestreamer-gui)

---
Requires having Livestreamer already installed [Link](http://docs.livestreamer.io).  
Needs Visual Studio if you want to compile it yourself.  
Requires .Net 4.5.2 framework, so I don't know how it works with Windows versions older than Windows 8, if there is request, I may work on it.