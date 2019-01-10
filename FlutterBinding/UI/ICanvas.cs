using System.Collections.Generic;
using SkiaSharp;

namespace FlutterBinding.UI
{
    public interface ICanvas
    {
        void ClipPath(Path path, bool doAntiAlias = true);
        void ClipRect(Rect rect, ClipOp clipOp = default(ClipOp), bool doAntiAlias = true);
        void ClipRRect(RRect rrect, bool doAntiAlias = true);
        void DrawColor(Color color, BlendMode blendMode);
        void DrawImageNine(SKImage image, Rect center, Rect dst, SKPaint paint);
        void DrawLine(Offset p1, Offset p2, SKPaint paint);
        void DrawPaint(SKPaint paint);
        void DrawParagraph(Paragraph paragraph, Offset offset);
        void DrawPicture(SKPicture picture);
        void DrawPoints(PointMode pointMode, List<Offset> points, SKPaint paint);
        void DrawRawAtlas(SKImage atlas, List<double> rstTransforms, List<double> rects, List<uint> colors, BlendMode blendMode, Rect cullRect, SKPaint paint);
        void DrawRawPoints(PointMode pointMode, List<double> points, SKPaint paint);
        void DrawShadow(Path path, Color color, double elevation, bool transparentOccluder);
        void DrawVertices(SKVertices vertices, BlendMode blendMode, SKPaint paint);
        int GetSaveCount();
        void Restore();
        void Rotate(double radians);
        void Save();
        void Scale(double sx, double sy = default(double));
        void Skew(double sx, double sy);
        void Transform(List<float> matrix4);
        void Translate(double dx, double dy);
    }
}