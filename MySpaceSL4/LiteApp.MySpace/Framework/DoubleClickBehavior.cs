using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Threading;

namespace LiteApp.MySpace.Framework
{
    /// <summary>
    /// More details: http://www.blog.jonnycornwell.com/2010/08/08/double-click-behavior/
    /// </summary>
    public class DoubleClickBehavior : Behavior<UIElement>
    {
        private readonly DispatcherTimer _timer = new DispatcherTimer();

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.MouseLeftButtonUp += AssociatedObjectMouseLeftButtonUp;
            _timer.Tick += new EventHandler(_timerTick);
            _timer.Interval = TimeSpan.FromSeconds(ClickInterval);
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.MouseLeftButtonUp -= AssociatedObjectMouseLeftButtonUp;
        }

        private void _timerTick(object sender, EventArgs e)
        {
            _timer.Stop();
        }

        private void AssociatedObjectMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!_timer.IsEnabled)
            {
                // If the timer is not already running then start it.
                _timer.Start();
            }
            else
            {
                _timer.Stop();
                if (CommandToExecute != null)
                {
                    // If the timer is active there has been a second event so execute the command.
                    CommandToExecute.Execute(this);

                }
            }
        }

        public static readonly DependencyProperty IntervalProperty =
            DependencyProperty.Register("ClickInterval",
            typeof(double),
            typeof(DoubleClickBehavior),
            new PropertyMetadata(0.25,
                new PropertyChangedCallback(OnClickIntervalChanged)));

        private static void OnClickIntervalChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            var doubleClick = (DoubleClickBehavior)sender;
            doubleClick._timer.Interval = TimeSpan.FromSeconds(doubleClick.ClickInterval);
        }

        public double ClickInterval
        {
            get { return (double)GetValue(IntervalProperty); }
            set { SetValue(IntervalProperty, value); }
        }

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("CommandToExecute",
            typeof(ICommand),
            typeof(DoubleClickBehavior),
            new PropertyMetadata(null));

        public ICommand CommandToExecute
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }
    }
}
