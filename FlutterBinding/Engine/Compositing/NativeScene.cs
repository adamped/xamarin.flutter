using FlutterBinding.Flow.Layers;
using FlutterBinding.UI;
using SkiaSharp;

namespace FlutterBinding.Engine.Compositing
{
    public class NativeScene
    {

        public static NativeScene Create(Layer rootLayer,
                                 uint rasterizerTracingThreshold,
                                 bool checkerboardRasterCacheImages,
                                 bool checkerboardOffscreenLayers)
        {
            return new NativeScene(
                rootLayer, rasterizerTracingThreshold,
                checkerboardRasterCacheImages, checkerboardOffscreenLayers);
        }

        LayerTree m_layerTree;


        public LayerTree TakeLayerTree()
        {
            return m_layerTree;
        }

        public NativeScene(Layer rootLayer,
             uint rasterizerTracingThreshold,
             bool checkerboardRasterCacheImages,
             bool checkerboardOffscreenLayers)
        {
            m_layerTree = new LayerTree();
            m_layerTree.set_root_layer(rootLayer);
            m_layerTree.set_rasterizer_tracing_threshold(rasterizerTracingThreshold);
            m_layerTree.set_checkerboard_raster_cache_images(
                checkerboardRasterCacheImages);
            m_layerTree.set_checkerboard_offscreen_layers(checkerboardOffscreenLayers);
        }

        public string ToImage(int width,
                              int height,
                              _Callback<Image> raw_image_callback)
        {

            if (raw_image_callback == null)
            {
                return "Image callback was invalid";
            }

            if (m_layerTree == null)
            {
                return "Scene did not contain a layer tree.";
            }

            if (width == 0 || height == 0)
            {
                return "Image dimensions for scene were invalid.";
            }

            // We can't create an image on this task runner because we don't have a
            // graphics context. Even if we did, it would be slow anyway. Also, this
            // thread owns the sole reference to the layer tree. So we flatten the layer
            // tree into a picture and use that as the thread transport mechanism.

            var picture_bounds = new SKSizeI(width, height);
            var picture = m_layerTree.Flatten(new SKRect(0, 0, width, height));

            if (picture == null)
            {
                // Already in Dart scope.
                return "Could not flatten scene into a layer tree.";
            }

            // TODO: Call Callback to create actual image.
           
            return string.Empty;
        }
    }
}
