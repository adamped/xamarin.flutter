using FlutterBinding.Engine.Painting;
using SkiaSharp;

namespace FlutterBinding.Engine.Text
{
    public class NativeParagraph
    {
        public string Text { get; set; }

        // Temporary, to use for all text, until SkiaSharp can be updated
        SKPaint _paint = new SKPaint
        {
            Color = SKColors.Black,
            IsAntialias = true,
            Style = SKPaintStyle.Fill,
            TextAlign = SKTextAlign.Center,
            TextSize = 24
        };

        public void Paint(SKCanvas canvas, double x, double y)
        {           
            canvas.DrawText(this.Text, (float)x, (float)y, _paint);            
        }

        double _width;
        public void Layout(double width)
        {
            _width = width;
        }

        public float Width => _paint.MeasureText(Text);
        public float Height => 24;
    }
}
