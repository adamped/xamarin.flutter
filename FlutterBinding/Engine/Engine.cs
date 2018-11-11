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

            var picture = layer_tree.Flatten(new SKRect(0, 0, frame_size.Width, frame_size.Height));
          
            _canvas.DrawPicture(picture);

        }
    }
}
