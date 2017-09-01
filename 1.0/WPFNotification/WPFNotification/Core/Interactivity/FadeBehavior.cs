using System;
using System.Windows;
using System.Windows.Interactivity;
using System.Windows.Media.Animation;

namespace WPFNotification.Core.Interactivity
{
    /// <summary>
    /// The fade behaviour.
    /// </summary>
    public sealed class FadeBehavior : Behavior<FrameworkElement>
    {
        #region Dependency Properties

        public static readonly DependencyProperty BeginTimeProperty = DependencyProperty.Register(
            "BeginTime",
            typeof(TimeSpan),
            typeof(FadeBehavior),
            new PropertyMetadata(new TimeSpan()));

        public static readonly DependencyProperty DurationProperty = DependencyProperty.Register(
            "Duration",
            typeof(TimeSpan),
            typeof(FadeBehavior),
            new PropertyMetadata(new TimeSpan(0, 0, 0, 0, 300)));

        public static readonly DependencyProperty IsAnimatingOnIsVisibleChangedProperty = DependencyProperty.Register(
            "IsAnimatingOnIsVisibleChanged",
            typeof(bool),
            typeof(FadeBehavior),
            new PropertyMetadata(true));

        public static readonly DependencyProperty IsAnimatingOnLoadedProperty = DependencyProperty.Register(
            "IsAnimatingOnLoaded",
            typeof(bool),
            typeof(FadeBehavior),
            new PropertyMetadata(true));

        #endregion

        #region Events

        /// <summary>
        /// The fade in completed.
        /// </summary>
        public event EventHandler FadeInCompleted;

        /// <summary>
        /// The fade out completed.
        /// </summary>
        public event EventHandler FadeOutCompleted;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the begin time.
        /// </summary>
        public TimeSpan BeginTime
        {
            get { return (TimeSpan)GetValue(BeginTimeProperty); }
            set { SetValue(BeginTimeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the duration.
        /// </summary>
        public TimeSpan Duration
        {
            get { return (TimeSpan)GetValue(DurationProperty); }
            set { SetValue(DurationProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether is animating on is visible changed.
        /// </summary>
        public bool IsAnimatingOnIsVisibleChanged
        {
            get { return (bool)GetValue(IsAnimatingOnIsVisibleChangedProperty); }
            set { SetValue(IsAnimatingOnIsVisibleChangedProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether is animating on loaded.
        /// </summary>
        public bool IsAnimatingOnLoaded
        {
            get { return (bool)GetValue(IsAnimatingOnLoadedProperty); }
            set { SetValue(IsAnimatingOnLoadedProperty, value); }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// The fade in.
        /// </summary>
        public void FadeIn()
        {
            Storyboard storyboard = GetFadeInStoryboard(BeginTime, Duration);
            EventHandler eventHandler = null;
            eventHandler = (sender, e) =>
            {
                storyboard.Completed -= eventHandler;
                OnFadeInCompleted(this, EventArgs.Empty);
            };
            storyboard.Completed += eventHandler;
            storyboard.Begin(AssociatedObject);
        }

        /// <summary>
        /// The fade out.
        /// </summary>
        public void FadeOut()
        {
            Storyboard storyboard = GetFadeOutStoryboard(BeginTime, Duration);
            EventHandler eventHandler = null;
            eventHandler = (sender, e) =>
            {
                storyboard.Completed -= eventHandler;
                OnFadeOutCompleted(this, EventArgs.Empty);
            };
            storyboard.Completed += eventHandler;
            storyboard.Begin(AssociatedObject);
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// The on attached.
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.IsVisibleChanged += OnIsVisibleChanged;
            AssociatedObject.Loaded += OnLoaded;
        }

        /// <summary>
        /// The on detaching.
        /// </summary>
        protected override void OnDetaching()
        {
            base.OnDetaching();

            AssociatedObject.IsVisibleChanged -= OnIsVisibleChanged;
            AssociatedObject.Loaded -= OnLoaded;
        }

        #endregion

        #region Private Static Methods

        /// <summary>
        /// The get fade in storyboard.
        /// </summary>
        /// <param name="beginTime"> The begin time. </param>
        /// <param name="duration"> The duration. </param>
        /// <returns> The <see cref="Storyboard"/>. </returns>
        private static Storyboard GetFadeInStoryboard(TimeSpan beginTime, TimeSpan duration)
        {
            DoubleAnimationUsingKeyFrames animation = new DoubleAnimationUsingKeyFrames
            {
                FillBehavior = FillBehavior.HoldEnd
            };
            animation.KeyFrames.Add(
                new SplineDoubleKeyFrame
                {
                    KeyTime = KeyTime.FromTimeSpan(new TimeSpan()),
                    Value = 0
                });
            animation.KeyFrames.Add(
                new SplineDoubleKeyFrame
                {
                    KeyTime = KeyTime.FromTimeSpan(beginTime),
                    Value = 0
                });
            animation.KeyFrames.Add(
                new SplineDoubleKeyFrame
                {
                    KeyTime = KeyTime.FromTimeSpan(beginTime + duration),
                    Value = 1
                });

            Storyboard storyboard = new Storyboard();
            Storyboard.SetTargetProperty(animation, new PropertyPath("(UIElement.Opacity)"));
            storyboard.Children.Add(animation);

            return storyboard;
        }

        /// <summary>
        /// The get fade out storyboard.
        /// </summary>
        /// <param name="beginTime"> The begin time. </param>
        /// <param name="duration"> The duration. </param>
        /// <returns> The <see cref="Storyboard"/>. </returns>
        private static Storyboard GetFadeOutStoryboard(TimeSpan beginTime, TimeSpan duration)
        {
            DoubleAnimationUsingKeyFrames animation = new DoubleAnimationUsingKeyFrames
            {
                FillBehavior = FillBehavior.HoldEnd
            };
            animation.KeyFrames.Add(
                new SplineDoubleKeyFrame
                {
                    KeyTime = KeyTime.FromTimeSpan(beginTime),
                    Value = 1
                });
            animation.KeyFrames.Add(
                new SplineDoubleKeyFrame
                {
                    KeySpline = new KeySpline(0.5, 0, 1, 0.75),
                    KeyTime = KeyTime.FromTimeSpan(beginTime + duration),
                    Value = 0
                });

            Storyboard storyboard = new Storyboard();
            Storyboard.SetTargetProperty(animation, new PropertyPath("(UIElement.Opacity)"));
            storyboard.Children.Add(animation);

            return storyboard;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// The on fade in completed.
        /// </summary>
        /// <param name="sender"> The sender. </param>
        /// <param name="e"> The e. </param>
        private void OnFadeInCompleted(object sender, EventArgs e)
        {
            EventHandler eventHandler = FadeInCompleted;
            eventHandler?.Invoke(sender, e);
        }

        /// <summary>
        /// The on fade out completed.
        /// </summary>
        ///  <param name="sender"> The sender. </param>
        /// <param name="e"> The e. </param>
        private void OnFadeOutCompleted(object sender, EventArgs e)
        {
            EventHandler eventHandler = FadeOutCompleted;
            eventHandler?.Invoke(sender, e);
        }

        /// <summary>
        /// The on is visible changed.
        /// </summary>
        /// <param name="sender"> The sender. </param>
        /// <param name="e"> The e. </param>
        private void OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (IsAnimatingOnIsVisibleChanged)
            {
                if (AssociatedObject.Visibility == Visibility.Visible)
                {
                    FadeIn();
                }

                // this.FadeOut();
            }
        }

        /// <summary>
        /// The on loaded.
        /// </summary>
        /// <param name="sender"> The sender. </param>
        /// <param name="e"> The e. </param>
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (IsAnimatingOnLoaded)
            {
                FadeIn();
            }
        }

        #endregion
    }
}
