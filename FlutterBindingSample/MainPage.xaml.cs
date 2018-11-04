using FlutterBinding.Engine;
using FlutterBinding.UI;
using SkiaSharp;
using SkiaSharp.Views.UWP;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace FlutterBindingSample
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        bool _hasPainted = false;
        protected void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            var canvas = e.Surface.Canvas;


            if (_hasPainted)
                return;

            _hasPainted = true;
            var frame = ((Frame)Windows.UI.Xaml.Window.Current.Content);

            FlutterBinding.UI.Window.Instance.physicalSize = new FlutterBinding.UI.Size(frame.ActualWidth, frame.ActualHeight);

            //await Task.Delay(2000); // Ugly hack to test, that the Surface loads

            Engine.Instance.LoadCanvas(e.Surface.Canvas);
            Engine.Instance.SetSize(frame.ActualWidth, frame.ActualHeight);

            BeginFrame();
        }


        public void BeginFrame()
        {
            var window = FlutterBinding.UI.Window.Instance;

            double devicePixelRatio = window.devicePixelRatio;
            var physicalSize = window.physicalSize;
            var logicalSize = physicalSize / devicePixelRatio;

            var paragraphBuilder = new ParagraphBuilder(new ParagraphStyle());
            paragraphBuilder.addText("Hello, world!");
            var paragraph = paragraphBuilder.build();

            paragraph.layout(new ParagraphConstraints(width: logicalSize.width));

            var physicalBounds = Offset.zero & physicalSize;
            var recorder = new PictureRecorder();

            var canvas = new FlutterBinding.UI.Canvas(recorder, physicalBounds);
            canvas.scale((float)devicePixelRatio, (float)devicePixelRatio);
            canvas.drawParagraph(paragraph, new Offset(
                (logicalSize.width - paragraph.maxIntrinsicWidth) / 2.0,
                (logicalSize.height - paragraph.height) / 2.0
                ));

            var picture = recorder.endRecording();

            var sceneBuilder = new SceneBuilder();
            sceneBuilder.pushClipRect(physicalBounds);
            sceneBuilder.addPicture(Offset.zero, picture);
            sceneBuilder.pop();

            var scene = sceneBuilder.build();

            var display = Windows.Graphics.Display.DisplayInformation.GetForCurrentView();
            var _scale = display.LogicalDpi / 96.0f;

            window.render(scene);
        }
    }
}
