using FlutterBinding.Flow.Layers;
using SkiaSharp;

namespace FlutterBinding.Engine
{
    public class Engine
    {
        Engine() { }
        static Engine _instance;
        public static Engine Instance => _instance ?? (_instance = new Engine());

        SKCanvas _canvas;
        public void LoadCanvas(SKCanvas canvas)
        {
            _canvas = canvas;
        }

        double _physicalWidth;
        double _physicalHeight;
        public void SetSize(double physicalWidth, double physicalHeight)
        {
            _physicalWidth = physicalWidth;
            _physicalHeight = physicalHeight;
        }

        public void Render(LayerTree layer_tree)
        {
            if (layer_tree == null)
                return;

            SKSizeI frame_size = new SKSizeI((int)_physicalWidth,
                                             (int)_physicalHeight);
            if (frame_size.IsEmpty)
                return;

            layer_tree.set_frame_size(frame_size);

            // ** Start Hack
            // I just wanted it outputted at this time, rather than frame dependant

            // var picture = layer_tree.Flatten(new SKRect(0, 0, frame_size.Width, frame_size.Height));
            var picture = ((PictureLayer)((ContainerLayer)layer_tree.root_layer_).layers_[0]).picture();
            //// get the screen density for scaling

            //var scale = _scale;
            //var scaledSize = new SKSize(frame_size.Width / scale, frame_size.Height / scale);

            ////// handle the device screen density
            //_canvas.Scale(_scale);

            //// make sure the canvas is blank
            //_canvas.Clear(SKColors.White);

            //// draw some text
            //var paint = new SKPaint
            //{
            //    Color = SKColors.Black,
            //    IsAntialias = true,
            //    Style = SKPaintStyle.Fill,
            //    TextAlign = SKTextAlign.Center,
            //    TextSize = 24
            //};
            //var coord = new SKPoint(scaledSize.Width / 2, (scaledSize.Height + paint.TextSize) / 2);
            //_canvas.DrawText("Xamarin.Flutter", coord, paint);
            _canvas.DrawPicture(picture);

            //_canvas.DrawText("Hello", scaledSize.Width / 2, (scaledSize.Height + paint.TextSize) / 2, paint);

            // ** End Hack

            // This is where we are meant to go
            //animator_.Render(layer_tree);
        }
    }
}
