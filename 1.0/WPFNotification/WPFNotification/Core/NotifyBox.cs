﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Interactivity;
using System.Windows.Media;
using WPFNotification.Core.Configuration;
using WPFNotification.Core.Interactivity;

namespace WPFNotification.Core
{
    public static class NotifyBox
    {
        #region Fields
        /// <summary>
        /// Max number of notifications window
        /// </summary>
        private const int MAX_NOTIFICATIONS = 1;
      
        /// <summary>
        /// Number of notification windows
        /// </summary>
        private static int _notificationWindowsCount;

        /// <summary>
        /// The margin between notification windows.
        /// </summary>
        private const double Margin = 5;

        /// <summary>
        /// list of notifications window.
        /// </summary>
        private static readonly List<WindowInfo> NotificationWindows;

        /// <summary>
        /// buffer list of notifications window.
        /// </summary>
        private static readonly List<WindowInfo> NotificationsBuffer;

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises static members of the <see cref="NotifyBox"/> class.
        /// </summary>
        static NotifyBox()
        {
            NotificationWindows = new List<WindowInfo>();
            NotificationsBuffer = new List<WindowInfo>();
            _notificationWindowsCount = 0;
        }

        #endregion

        #region Public Static Methods
        /// <summary>
        /// Shows the specified notification.
        /// </summary>
        /// <param name="content">The notification content.</param>
        /// <param name="configuration">The notification configuration object.</param>
        public static void Show(object content, NotificationConfiguration configuration)
        {
            DataTemplate notificationTemplate = (DataTemplate)Application.Current.Resources[configuration.TemplateName];
            Window window = new Window
            {
                Title = "",
                Width = configuration.Width.Value,
                Height = configuration.Height.Value,
                Content = content,
                ShowActivated = false,
                AllowsTransparency = true,
                WindowStyle = WindowStyle.None,
                ShowInTaskbar = false,
                Topmost = true,
                Background = Brushes.Transparent,
                UseLayoutRounding = true,
                ContentTemplate = notificationTemplate
            };
            Show(window, configuration.DisplayDuration, configuration.NotificationFlowDirection);
        }

        /// <summary>
        /// Shows the specified notification.
        /// </summary>
        /// <param name="content">The notification content.</param>
        public static void Show(object content)
        {
            Show(content, NotificationConfiguration.DefaultConfiguration);
        }

        /// <summary>
        /// Shows the specified window as a notification.
        /// </summary>
        /// <param name="window">The window.</param>
        /// <param name="displayDuration">The display duration.</param>
        /// <param name="notificationFlowDirection">Flow direction</param>
        public static void Show(Window window, TimeSpan displayDuration, NotificationFlowDirection notificationFlowDirection)
        {
            BehaviorCollection behaviors = Interaction.GetBehaviors(window);
            behaviors.Add(new FadeBehavior());
            behaviors.Add(new SlideBehavior());
            SetWindowDirection(window, notificationFlowDirection);
            _notificationWindowsCount += 1;
            WindowInfo windowInfo = new WindowInfo()
            {
                Id = _notificationWindowsCount,
                DisplayDuration = displayDuration,
                Window = window
            };
            windowInfo.Window.Closed += Window_Closed;
            if (NotificationWindows.Count + 1 > MAX_NOTIFICATIONS)
            {
                NotificationsBuffer.Add(windowInfo);
            }
            else
            {
                Observable
              .Timer(displayDuration)
              .ObserveOnDispatcher()
              .Subscribe(x => OnTimerElapsed(windowInfo));
                NotificationWindows.Add(windowInfo);
                window.Show();
            }
        }

        /// <summary>
        /// Remove all notifications from notification list and buffer list.
        /// </summary>
        public static void ClearNotifications()
        {
            NotificationWindows.Clear();
            NotificationsBuffer.Clear();
           
            _notificationWindowsCount = 0;
        }


        #endregion

        #region Private Static Methods

        /// <summary>
        /// Called when the timer has elapsed. Removes any stale notifications.
        /// </summary>
        private static void OnTimerElapsed(WindowInfo windowInfo)
        {
            if (NotificationWindows.Count > 0 && NotificationWindows.All(i => i.Id != windowInfo.Id))
            {
                return;
            }

            if (windowInfo.Window.IsMouseOver)
            {
                Observable
                    .Timer(windowInfo.DisplayDuration)
                    .ObserveOnDispatcher()
                    .Subscribe(x => OnTimerElapsed(windowInfo));
            }
            else
            {
                BehaviorCollection behaviors = Interaction.GetBehaviors(windowInfo.Window);
                FadeBehavior fadeBehavior = behaviors.OfType<FadeBehavior>().First();
                SlideBehavior slideBehavior = behaviors.OfType<SlideBehavior>().First();

                fadeBehavior.FadeOut();
                slideBehavior.SlideOut();

                EventHandler eventHandler = null;
                eventHandler = (sender2, e2) =>
                {
                    fadeBehavior.FadeOutCompleted -= eventHandler;
                    NotificationWindows.Remove(windowInfo);
                    windowInfo.Window.Close();

                    if (NotificationsBuffer != null && NotificationsBuffer.Count > 0)
                    {
                        var bufferWindowInfo = NotificationsBuffer.First();
                        Observable
                         .Timer(bufferWindowInfo.DisplayDuration)
                         .ObserveOnDispatcher()
                         .Subscribe(x => OnTimerElapsed(bufferWindowInfo));
                        NotificationWindows.Add(bufferWindowInfo);
                        bufferWindowInfo.Window.Show();
                        NotificationsBuffer.Remove(bufferWindowInfo);
                    }
                };
                fadeBehavior.FadeOutCompleted += eventHandler;
            }
        }

        /// <summary>
        /// Called when the window is about to close. 
        /// Remove the notification window from notification windows list and add one from the buffer list.
        /// </summary>
        
        static void Window_Closed(object sender, EventArgs e)
        {
            var window = (Window)sender;
            if (NotificationWindows.Count > 0 && NotificationWindows.First().Window == window)
            {
                WindowInfo windowInfo = NotificationWindows.First();
                NotificationWindows.Remove(windowInfo);
                if (NotificationsBuffer != null && NotificationsBuffer.Count > 0)
                {
                    var bufferWindowInfo = NotificationsBuffer.First();
                    Observable
                     .Timer(bufferWindowInfo.DisplayDuration)
                     .ObserveOnDispatcher()
                     .Subscribe(x => OnTimerElapsed(bufferWindowInfo));
                    NotificationWindows.Add(bufferWindowInfo);
                    bufferWindowInfo.Window.Show();
                    NotificationsBuffer.Remove(bufferWindowInfo);
                }
            }
        }

        /// <summary>
        /// Display the notification window in specified direction of the screen
        /// </summary>
        /// <param name="window"> The window object</param>
        /// <param name="notificationFlowDirection"> Direction in which new notifications will appear.</param>
        private static void SetWindowDirection(Window window, NotificationFlowDirection notificationFlowDirection)
        {
            var workingArea = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea;
            var transform = PresentationSource.FromVisual(Application.Current.MainWindow).CompositionTarget.TransformFromDevice;
            var corner = transform.Transform(new Point(workingArea.Right, workingArea.Bottom));

            switch (notificationFlowDirection)
            {
                case NotificationFlowDirection.RightBottom:
                    window.Left = corner.X - window.Width - window.Margin.Right - Margin;
                    window.Top = corner.Y - window.Height - window.Margin.Top;
                    break;
                case NotificationFlowDirection.LeftBottom:
                    window.Left = 0;
                    window.Top = corner.Y - window.Height - window.Margin.Top;
                    break;
                case NotificationFlowDirection.LeftUp:
                    window.Left = 0;
                    window.Top = 0;
                    break;
                case NotificationFlowDirection.RightUp:
                    window.Left = corner.X - window.Width - window.Margin.Right - Margin;
                    window.Top = 0;
                    break;
                default:
                    window.Left = corner.X - window.Width - window.Margin.Right - Margin;
                    window.Top = corner.Y - window.Height - window.Margin.Top;
                    break;
            }
        }

        #endregion

        #region Private Classes

        /// <summary>
        /// Window metadata.
        /// </summary>
        private sealed class WindowInfo
        {
            public int Id { get; set; }
            /// <summary>
            /// Gets or sets the display duration.
            /// </summary>
            /// <value>
            /// The display duration.
            /// </value>
            public TimeSpan DisplayDuration { get; set; }

            /// <summary>
            /// Gets or sets the window.
            /// </summary>
            /// <value>
            /// The window.
            /// </value>
            public Window Window { get; set; }
        }

        #endregion
    }
}
