using Avalonia;
using Avalonia.Animation;
using Avalonia.Controls;
using Avalonia.Threading;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace WPF.Controls
{
    public class TransitioningContentControl : ContentControl
    {
        private CancellationTokenSource? _lastTransitionCts;
        private object? _currentContent;

        /// <summary>
        /// Defines the <see cref="PageTransition"/> property.
        /// </summary>
        public static readonly StyledProperty<IPageTransition?> PageTransitionProperty =
            AvaloniaProperty.Register<TransitioningContentControl, IPageTransition?>(nameof(PageTransition),
                new CrossFade(TimeSpan.FromSeconds(0.200)));

        /// <summary>
        /// Defines the <see cref="CurrentContent"/> property.
        /// </summary>
        public static readonly DirectProperty<TransitioningContentControl, object?> CurrentContentProperty =
            AvaloniaProperty.RegisterDirect<TransitioningContentControl, object?>(nameof(CurrentContent),
                o => o.CurrentContent);

        /// <summary>
        /// Gets or sets the animation played when content appears and disappears.
        /// </summary>
        public IPageTransition? PageTransition
        {
            get => GetValue(PageTransitionProperty);
            set => SetValue(PageTransitionProperty, value);
        }

        /// <summary>
        /// Gets the content currently displayed on the screen.
        /// </summary>
        public object? CurrentContent
        {
            get => _currentContent;
            private set => SetAndRaise(CurrentContentProperty, ref _currentContent, value);
        }

        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnAttachedToVisualTree(e);

            Dispatcher.UIThread.Post(() => UpdateContentWithTransition(Content));
        }

        protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnDetachedFromVisualTree(e);

            _lastTransitionCts?.Cancel();
        }

        protected override void OnPropertyChanged<T>(AvaloniaPropertyChangedEventArgs<T> change)
        {
            base.OnPropertyChanged(change);

            if (change.Property == ContentProperty)
            {
                Dispatcher.UIThread.Post(() => UpdateContentWithTransition(Content));
            }
        }

        /// <summary>
        /// Updates the content with transitions.
        /// </summary>
        /// <param name="content">New content to set.</param>
        private async void UpdateContentWithTransition(object? content)
        {
            if (VisualRoot is null)
            {
                return;
            }

            _lastTransitionCts?.Cancel();
            _lastTransitionCts = new CancellationTokenSource();

            if (PageTransition != null)
                await PageTransition.Start(this, null, true, _lastTransitionCts.Token);

            CurrentContent = content;

            if (PageTransition != null)
                await PageTransition.Start(null, this, true, _lastTransitionCts.Token);
        }
    }
}
