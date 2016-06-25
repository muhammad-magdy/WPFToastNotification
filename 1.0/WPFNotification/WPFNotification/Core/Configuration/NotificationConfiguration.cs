using System;

namespace WPFNotification.Core.Configuration
{
    /// <summary>
    /// notification configuration 
    /// </summary>
    public class NotificationConfiguration
    {
        #region Configuration Default values
        /// <summary>
        /// The default display duration for a notification window.
        /// </summary>
        private static readonly TimeSpan DefaultDisplayDuration = TimeSpan.FromSeconds(2);

        /// <summary>
        /// The default notifications window Width
        /// </summary>
        private const int DefaultWidth = 300;

        /// <summary>
        /// The default notifications window Height
        /// </summary>
        private const int DefaultHeight = 150;

        /// <summary>
        /// The default template of notification window
        /// </summary>
        private const string DefaultTemplateName = "notificationTemplate";
        #endregion

        #region constructor
        /// <summary>
        /// Initialises the configuration object.
        /// </summary>
        /// <param name="displayDuration">The notification display duration. set it TimeSpan.Zero to use default value </param>
        /// <param name="width">The notification width. set it to null to use default value</param>
        /// <param name="height">The notification height. set it to null to use default value</param>
        /// <param name="templateName">The notification template name. set it to null to use default value</param>
        /// <param name="notificationFlowDirection">The notification flow direction. set it to null to use default value (RightBottom)</param>
        public NotificationConfiguration(TimeSpan displayDuration, int? width, int? height, string templateName, NotificationFlowDirection? notificationFlowDirection)
        {
            DisplayDuration = displayDuration > TimeSpan.Zero ? displayDuration : DefaultDisplayDuration;
            Width = width.HasValue ? width : DefaultWidth;
            Height = height.HasValue ? height : DefaultHeight;
            TemplateName = !string.IsNullOrEmpty(templateName) ? templateName : DefaultTemplateName;
            NotificationFlowDirection = notificationFlowDirection ?? NotificationFlowDirection.RightBottom;
        }
        #endregion

        #region public Properties
        /// <summary>
        /// The default configuration object
        /// </summary>
        public static NotificationConfiguration DefaultConfiguration
        {
            get
            {
                return new NotificationConfiguration(DefaultDisplayDuration, DefaultWidth, DefaultHeight, DefaultTemplateName, NotificationFlowDirection.RightBottom);
            }
        }

        /// <summary>
        /// The display duration for a notification window.
        /// </summary>
        public TimeSpan DisplayDuration { get; private set; }

        /// <summary>
        /// Notifications window Width
        /// </summary>
        public int? Width { get; private set; }

        /// <summary>
        /// Notifications window Height
        /// </summary>
        public int? Height { get; private set; }

        /// <summary>
        /// The template of notification window
        /// </summary>
        public string TemplateName { get; private set; }
        /// <summary>
        /// The notification window flow direction
        /// </summary>
        public NotificationFlowDirection NotificationFlowDirection { get; set; }
        #endregion


    }
}
