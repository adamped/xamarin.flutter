using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlutterBinding.Engine.Painting
{
    public class NativeCanvas : SKCanvas
    {
        public NativeCanvas(SKBitmap bitmap) : base(bitmap) { }

        public SKCanvas RecordingCanvas { get; set; }

        public void Constructor(NativePictureRecorder recorder,
                          double left,
                          double top,
                          double right,
                          double bottom)
        {
            var canvas = recorder.BeginRecording(new SKRect((float)left, (float)top, (float)right, (float)bottom));
            recorder.SetCanvas(canvas);
            RecordingCanvas = canvas;
        }
    }
}