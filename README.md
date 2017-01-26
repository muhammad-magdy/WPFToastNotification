# Toast Notification
Fancy toast notification for WPF applications easy to use and support MVVM pattern. A small notification window with image, title and content displayed at the bottom of the screen. You can use the default implementation or build your own custom notification.

[![NuGet](https://img.shields.io/nuget/dt/WPFNotification.svg)](https://www.nuget.org/packages/WPFNotification)

## Demo
Check out the **toast notification demo app**. The app demonstrates How to use the toast notification to display it with the default implementation or with your own custom implementation. The [full source](https://github.com/muhammad-magdy/WPFToastNotification/tree/master/1.0/WPFNotification/WPFNotificationDemo) of the demo app is included in the source code of this project.

## Documentation
See some [screenshots](https://github.com/muhammad-magdy/WPFToastNotification/wiki/Screenshots) to get an idea of how the toast notification will be displayed on the screen. And visit the [wiki](https://github.com/muhammad-magdy/WPFToastNotification/wiki) to learn how to use it into your application.

![](http://i1042.photobucket.com/albums/b426/muhammed_magdy/MailNotification_intro_zpsibibsvpb.png)

## Features
* Simple, lightweight and fancy toast notification with title, content and overridable predefined image displayed at the bottom of the screen
* Support MVVM pattern
* Configurable, you can use the default notification configuration or reconfigure it with the following attributes 
  * Display duration
  * Width
  * Height
  * Notification template 
  * Notification Flow Direction,That set direction in which new notifications will appear. Available options are:
    * RightBottom (default)
    * LeftBottom
    * LeftUp
    * RightUp
* Customizable
  * You can impelement your own notification
* Show one notification per time and if there are other notifications they will be placed in a queue and will be shown when the place is available
* Prevent the notification from fading out when hovering on it
* Allow to clear all notification from the notification list and the buffer list
* **Support Windows 7 or later.** 

## Acknowledgements
* Adapted the NotifyBox from [Elysium](https://elysiumextra.codeplex.com)


