using SkiaSharp;

namespace FlutterBinding.Engine.Painting
{
    public class NativePictureRecorder: SKPictureRecorder
    {

        protected SKCanvas Canvas;
        public void SetCanvas(SKCanvas canvas)
            => this.Canvas = canvas;

    }
}
