using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using WPFNotification.Core.Configuration;
using WPFNotification.Model;
using WPFNotification.Services;
using WPFNotificationDemo.Core;
using WPFNotificationDemo.Model;
namespace WPFNotificationDemo.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private readonly INotificationDialogService _dialogService;
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        /// <param name="dailogService">The NotificationDialogService object</param>


        public MainViewModel(INotificationDialogService dialogService)
        {
            _dialogService = dialogService;
        }



        private RelayCommand _mailNotification;

        /// <summary>
        /// The mail notification command.
        /// create MailNotification and NotificationConfiguration objects  and send them to NotificationDialogService to show notification. 
        /// </summary>
        public RelayCommand MailNotification
        {
            get
            {
                return _mailNotification
                    ?? (_mailNotification = new RelayCommand(
                    () =>
                    {
                        var newNotification = new MailNotification()
                        {
                            Title = "Vacation Request",
                            Sender = "Mohamed Magdy",
                            Content = "I would like to request for vacation from 20/12/2015 to 30/12/2015 ............."
                        };
                        var configuration = new NotificationConfiguration(TimeSpan.Zero, null, null, Constants.MailNotificationTemplateName, null);
                        _dialogService.ShowNotificationWindow(newNotification, configuration);
                    }));
            }
        }


        private RelayCommand<NotificationFlowDirection> _addNotification;

        /// <summary>
        /// The AddNotification command.
        /// Create Notification object and send it to NotificationDialogService to display the notification.
        /// </summary>
        public RelayCommand<NotificationFlowDirection> AddNotification
        {
            get
            {
                return _addNotification
                    ?? (_addNotification = new RelayCommand<NotificationFlowDirection>(
                    (notificationFlowDirection) =>
                    {
                        var notificationConfiguration = NotificationConfiguration.DefaultConfiguration;
                        notificationConfiguration.NotificationFlowDirection = notificationFlowDirection;
                        var newNotification = new Notification()
                        {
                            Title = "Test Fail",
                            Message = "Test one Fail Please check your Machine Code and Try Again"
                            // ,ImgURL = "pack://application:,,,a/Resources/Images/warning.png"
                        };
                        _dialogService.ShowNotificationWindow(newNotification, notificationConfiguration);
                    }));
            }
        }

        private RelayCommand _clearNotifications;

        /// <summary>
        /// The clear All notifications command.
        /// Remove all notifications from notification list and buffer list.
        /// </summary>
        public RelayCommand ClearNotifications
        {
            get
            {
                return _clearNotifications
                    ?? (_clearNotifications = new RelayCommand(
                    () =>
                    {
                        _dialogService.ClearNotifications();
                    }));
            }
        }
    }
}