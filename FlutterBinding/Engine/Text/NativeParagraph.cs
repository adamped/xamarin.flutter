using FlutterBinding.Engine.Painting;
using SkiaSharp;

namespace FlutterBinding.Engine.Text
{
    public class NativeParagraph
    {
        public string Text { get; set; }

        public void Paint(SkiaSharp.SKCanvas canvas, double x, double y)
        {
            var paint = new SKPaint
            {
                Color = SKColors.Black,
                IsAntialias = true,
                Style = SKPaintStyle.Fill,
                TextAlign = SKTextAlign.Center,
                TextSize = 24
            };
            canvas.DrawText(this.Text, (float)x, (float)y, paint);
        }

        double _width;
        public void Layout(double width)
        {
            _width = width;
        }

    }
}
