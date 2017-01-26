using System;
using System.Windows;
using System.Windows.Interactivity;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace WPFNotification.Core.Interactivity
{
    /// <summary>
    /// The slide behaviour.
    /// </summary>
    public sealed class SlideBehavior : Behavior<FrameworkElement>
    {
        #region Dependency Properties

        public static readonly DependencyProperty BeginTimeProperty = DependencyProperty.Register(
            "BeginTime",
            typeof(TimeSpan),
            typeof(SlideBehavior),
            new PropertyMetadata(new TimeSpan()));

        public static readonly DependencyProperty DurationProperty = DependencyProperty.Register(
            "Duration",
            typeof(TimeSpan),
            typeof(SlideBehavior),
            new PropertyMetadata(new TimeSpan(0, 0, 0, 0, 300)));

        public static readonly DependencyProperty IsAnimatingOnIsVisibleChangedProperty = DependencyProperty.Register(
            "IsAnimatingOnIsVisibleChanged",
            typeof(bool),
            typeof(SlideBehavior),
            new PropertyMetadata(true));

        public static readonly DependencyProperty IsAnimatingOnLoadedProperty = DependencyProperty.Register(
            "IsAnimatingOnLoaded",
            typeof(bool),
            typeof(SlideBehavior),
            new PropertyMetadata(true));

        public static readonly DependencyProperty OffsetProperty = DependencyProperty.Register(
            "Offset",
            typeof(double),
            typeof(SlideBehavior),
            new PropertyMetadata(40.0D));

        #endregion

        #region Events

        /// <summary>
        /// The slide in completed.
        /// </summary>
        public event EventHandler SlideInCompleted;

        /// <summary>
        /// The slide out completed.
        /// </summary>
        public event EventHandler SlideOutCompleted;

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

        /// <summary>
        /// Gets or sets the offset.
        /// </summary>
        public double Offset
        {
            get { return (double)GetValue(OffsetProperty); }
            set { SetValue(OffsetProperty, value); }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// The slide in.
        /// </summary>
        public void SlideIn()
        {
            AssociatedObject.IsHitTestVisible = false;
            AssociatedObject.RenderTransform = new TranslateTransform(0, 0);

            Storyboard storyboard = GetSlideInStoryboard(BeginTime, Duration, Offset);
            EventHandler eventHandler = null;
            eventHandler = (sender, e) =>
            {
                storyboard.Completed -= eventHandler;
                AssociatedObject.IsHitTestVisible = true;
                OnSlideInCompleted(this, EventArgs.Empty);
            };
            storyboard.Completed += eventHandler;
            storyboard.Begin(AssociatedObject);
        }

        /// <summary>
        /// The slide out.
        /// </summary>
        public void SlideOut()
        {
            AssociatedObject.IsHitTestVisible = false;
            AssociatedObject.RenderTransform = new TranslateTransform(0, 0);

            Storyboard storyboard = GetSlideOutStoryboard(BeginTime, Duration, Offset);
            EventHandler eventHandler = null;
            eventHandler = (sender, e) =>
            {
                storyboard.Completed -= eventHandler;
                AssociatedObject.IsHitTestVisible = true;
                OnSlideOutCompleted(this, EventArgs.Empty);
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
        /// The get slide in storyboard.
        /// </summary>
        /// <param name="beginTime"> The begin time. </param>
        /// <param name="duration"> The duration. </param>
        /// <param name="offset"> The offset. </param>
        /// <returns> The <see cref="Storyboard"/>. </returns>
        private static Storyboard GetSlideInStoryboard(
            TimeSpan beginTime,
            TimeSpan duration,
            double offset)
        {
            DoubleAnimationUsingKeyFrames animation = new DoubleAnimationUsingKeyFrames
            {
                FillBehavior = FillBehavior.HoldEnd
            };
            animation.KeyFrames.Add(
                new SplineDoubleKeyFrame
                {
                    KeyTime = KeyTime.FromTimeSpan(beginTime),
                    Value = offset
                });
            animation.KeyFrames.Add(
                new SplineDoubleKeyFrame
                {
                    KeySpline = new KeySpline(0, 0.5, 0.5, 1),
                    KeyTime = KeyTime.FromTimeSpan(beginTime + duration),
                    Value = 0.0
                });

            Storyboard storyboard = new Storyboard();
            Storyboard.SetTargetProperty(animation, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.X)"));
            storyboard.Children.Add(animation);

            return storyboard;
        }

        /// <summary>
        /// The get slide out storyboard.
        /// </summary>
        /// <param name="beginTime"> The begin time. </param>
        /// <param name="duration"> The duration. </param>
        /// <param name="offset"> The offset. </param>
        /// <returns> The <see cref="Storyboard"/>. </returns>
        private static Storyboard GetSlideOutStoryboard(
            TimeSpan beginTime,
            TimeSpan duration,
            double offset)
        {
            DoubleAnimationUsingKeyFrames animation = new DoubleAnimationUsingKeyFrames();
            animation.KeyFrames.Add(
                new SplineDoubleKeyFrame()
                {
                    KeyTime = KeyTime.FromTimeSpan(beginTime),
                    Value = 0.0
                });
            animation.KeyFrames.Add(
                new SplineDoubleKeyFrame()
                {
                    KeySpline = new KeySpline(0.5, 0, 1, 0.75),
                    KeyTime = KeyTime.FromTimeSpan(beginTime + duration),
                    Value = offset,
                });

            Storyboard storyboard = new Storyboard();
            Storyboard.SetTargetProperty(animation, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.Y)"));
            storyboard.Children.Add(animation);

            return storyboard;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// The on slide in completed.
        /// </summary>
        /// <param name="sender"> The sender. </param>
        /// <param name="e"> The e. </param>
        private void OnSlideInCompleted(object sender, EventArgs e)
        {
            EventHandler eventHandler = SlideInCompleted;
            eventHandler?.Invoke(sender, e);
        }

        /// <summary>
        /// The on slide out completed.
        /// </summary>
        /// <param name="sender"> The sender. </param>
        /// <param name="e"> The e. </param>
        private void OnSlideOutCompleted(object sender, EventArgs e)
        {
            EventHandler eventHandler = SlideOutCompleted;
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
                    SlideIn();
                }

                // this.SlideOut();
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
                SlideIn();
            }
        }

        #endregion
    }
}
