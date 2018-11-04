using FlutterBinding.Flow.Layers;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlutterBinding.Engine
{
    public class Animator
    {
        Animator() { }
        static Animator _instance;
        public static Animator Instance => _instance ?? (_instance = new Animator());

        bool paused_;
        bool regenerate_layer_tree_;
        bool frame_scheduled_;
        int notify_idle_task_id_;
        bool dimension_change_pending_;
        SKSizeI last_layer_tree_size_;




        public void BeginFrame()
        {
            if (producer_continuation_ == null)
            {
                // We may already have a valid pipeline continuation in case a previous
                // begin frame did not result in an Animation::Render. Simply reuse that
                // instead of asking the pipeline for a fresh continuation.
                //producer_continuation_ = layer_tree_pipeline_.Produce();

                if (producer_continuation_ == null)
                {
                    // If we still don't have valid continuation, the pipeline is currently
                    // full because the consumer is being too slow. Try again at the next
                    // frame interval.
                    //RequestFrame();
                    return;
                }
            }
        }

        public void Render(LayerTree layer_tree)
        {
            if (dimension_change_pending_ &&
                layer_tree.frame_size() != last_layer_tree_size_)
            {
                dimension_change_pending_ = false;
            }
            last_layer_tree_size_ = layer_tree.frame_size();

            //if (layer_tree != null)
            //{
            //    // Note the frame time for instrumentation.
            //    layer_tree.set_construction_time(fml::TimePoint::Now() -
            //                                      last_begin_frame_time_);
            //}

            // Commit the pending continuation.
            //producer_continuation_.Complete(layer_tree);

            //delegate_.OnAnimatorDraw(layer_tree_pipeline_);
        }

        ProducerContinuation producer_continuation_;
        AnimatorDelegate delegate_;
    }

    public class Pipeline<T>
    { }



    public class ProducerContinuation
    {
        public delegate void Continuation(LayerTree tree, uint UnnamedParameter2);

        private Continuation continuation_;
        uint trace_id_ = 0;
        public void Complete(LayerTree resource)
        {
            if (continuation_ != null)
            {
                continuation_(resource, trace_id_);
                continuation_ = null;
                //TRACE_EVENT_ASYNC_END0("flutter", "PipelineProduce", trace_id_);
                //TRACE_FLOW_STEP("flutter", "PipelineItem", trace_id_);
            }
        }
    }

    public class AnimatorDelegate
    {
        public void OnAnimatorBeginFrame(DateTimeOffset frame_time) { }

        public void OnAnimatorNotifyIdle(UInt64 deadline) { }

        public void OnAnimatorDraw(Pipeline<LayerTree> pipeline)
        {
        }

        public void OnAnimatorDrawLastLayerTree() { }
    }
}
