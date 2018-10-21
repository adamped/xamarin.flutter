using SkiaSharp;

namespace FlutterBinding.UI
{
    public class FlutterSurface
    {
        private readonly float _scale;

        public FlutterSurface(float scale)
        {
            _scale = scale;
        }

        public void OnPaintSurface(SKSurface surface, SKImageInfo info)
        {
            var canvas = surface.Canvas;

            // get the screen density for scaling
            var scaledSize = new SKSize(info.Width / _scale, info.Height / _scale);

            // handle the device screen density
            canvas.Scale(_scale);

            // make sure the canvas is blank
            canvas.Clear(new SKColor(0,145, 234, 255));

            // draw some text
            var paint = new SKPaint
            {
                Color       = SKColors.WhiteSmoke,
                IsAntialias = true,
                Style       = SKPaintStyle.Fill,
                TextAlign   = SKTextAlign.Center,
                TextSize    = 24
            };
            var coord = new SKPoint(scaledSize.Width / 2, (scaledSize.Height + paint.TextSize) / 2);
            canvas.DrawText("Xamarin.Flutter", coord, paint);
        }
    }
}
