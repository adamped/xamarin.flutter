using System;
using Windows.Graphics.Display;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using FlutterBinding.UI;
using SkiaSharp.Views.UWP;

namespace FlutterBindingSample
{
    //[TemplatePart(Name = "Content")]
    public sealed class FlutterCanvas : ContentControl
    {
        private const string       CanvasKey = "Canvas";

        public SKXamlCanvas Canvas { get; private set; }
        public FlutterSurface Surface { get; private set; }

        public FlutterCanvas()
        {
            DefaultStyleKey = typeof(FlutterCanvas);
        }

        /// <exception cref="ArgumentNullException"></exception>
        /// <inheritdoc />
        protected override void OnApplyTemplate()
        {
            if (Canvas != null)
                Canvas.PaintSurface -= OnCanvasOnPaintSurface;

            Canvas = GetTemplateChild(CanvasKey) as SKXamlCanvas;
            if (Canvas == null)
                return;

            var display = DisplayInformation.GetForCurrentView();
            var scale   = display.LogicalDpi / 96.0f;

            Surface = new FlutterSurface(scale);
            Canvas.PaintSurface += OnCanvasOnPaintSurface;

            base.OnApplyTemplate();
        }

        private void OnCanvasOnPaintSurface(object s, SKPaintSurfaceEventArgs args)
        {
            Surface.OnPaintSurface(args.Surface, args.Info);
        }

        /// <inheritdoc />
        protected override void OnContentChanged(object oldContent, object newContent)
        {
            if (oldContent is FrameworkElement oldElement)
                oldElement.SizeChanged -= OnSizeChanged;

            if (newContent is FrameworkElement newElement)
                newElement.SizeChanged += OnSizeChanged;

            base.OnContentChanged(oldContent, newContent);
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            // TODO: Notify
        }
    }
}
