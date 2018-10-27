using SkiaSharp;

namespace FlutterBinding.Engine
{
    public class Engine
    {
        Engine() { }
        static Engine _instance;
        public static Engine Instance => _instance ?? (_instance = new Engine());

        public void LoadCanvas(SKCanvas canvas)
        {

        }
    }
}
