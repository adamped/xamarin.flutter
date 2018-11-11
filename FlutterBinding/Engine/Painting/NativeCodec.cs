using FlutterBinding.UI;
using SkiaSharp;
using System.Collections.Generic;

namespace FlutterBinding.Engine.Painting
{
    public static class NativeCodec
    {
        public static string InstantiateImageCodec(List<int> list, _Callback<SKCodec> callback, _ImageInfo imageInfo, double decodedCacheRatioCap)
        {           
            return null;
        }

        public static SKImage GetImage(this SKCodecFrameInfo info, SKCodec codec)
        {
            return SKImage.Create(codec.Info);
        }
    }
}
