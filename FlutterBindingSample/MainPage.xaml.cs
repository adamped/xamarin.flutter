using FlutterBinding.Engine;
using FlutterSDK.Widgets.Basic;
using FlutterSDK.Widgets.Framework;
using FlutterSDK.Widgets.Text;
using SkiaSharp.Views.UWP;
using Windows.UI.Xaml.Controls;

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

        public class MyApp : StatelessWidget
        {
            public override Widget Build(BuildContext context)
            {
                return new Center(
                    child: new Text("Hello Xamarin.Flutter!")
                  );
            }
        }

      

        public void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            var canvas = e.Surface.Canvas;

            var frame = ((Frame)Windows.UI.Xaml.Window.Current.Content);

            FlutterBinding.UI.Window.Instance.physicalSize = new FlutterBinding.UI.Size(frame.ActualWidth, frame.ActualHeight);

            Engine.Instance.LoadCanvas(e.Surface.Canvas);
            Engine.Instance.SetSize(frame.ActualWidth, frame.ActualHeight);

            //RunApp(new MyApp());
        }


        public void BeginFrame()
        {
            var window = FlutterBinding.UI.Window.Instance;

            double devicePixelRatio = window.devicePixelRatio;
            var physicalSize = window.physicalSize;
            var logicalSize = physicalSize / devicePixelRatio;

            var paragraphBuilder = new FlutterBinding.UI.ParagraphBuilder(new FlutterBinding.UI.ParagraphStyle());
            paragraphBuilder.addText("Hello World!");
            var paragraph = paragraphBuilder.build();

            paragraph.layout(new FlutterBinding.UI.ParagraphConstraints(width: logicalSize.width));

            var physicalBounds = FlutterBinding.UI.Offset.zero & physicalSize;
            var recorder = new FlutterBinding.UI.PictureRecorder();

            var canvas = new FlutterBinding.UI.Canvas(recorder, physicalBounds);
            canvas.scale((float)devicePixelRatio, (float)devicePixelRatio);
            canvas.drawParagraph(paragraph, new FlutterBinding.UI.Offset(
                (logicalSize.width - paragraph.maxIntrinsicWidth) / 2.0,
                (logicalSize.height - paragraph.height) / 2.0
                ));

            var picture = recorder.endRecording();

            var sceneBuilder = new FlutterBinding.UI.SceneBuilder();
            sceneBuilder.pushClipRect(physicalBounds);
            sceneBuilder.addPicture(FlutterBinding.UI.Offset.zero, picture);
            sceneBuilder.pop();

            var scene = sceneBuilder.build();

            window.render(scene);
        }
    }
}
