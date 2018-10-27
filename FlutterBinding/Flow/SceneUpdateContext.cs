using FlutterBinding.Flow.Layers;
using SkiaSharp;
using System;
using System.Collections.Generic;
using static FlutterBinding.Flow.Helper;

// Copyright 2016 The Chromium Authors. All rights reserved.
// Use of this source code is governed by a BSD-style license that can be
// found in the LICENSE file.

namespace FlutterBinding.Flow
{

    //C++ TO C# CONVERTER NOTE: C# has no need of forward class declarations:
    //class Layer;
    //C++ TO C# CONVERTER NOTE: C# has no need of forward class declarations:
    //class ExportNodeHolder;
    //C++ TO C# CONVERTER NOTE: C# has no need of forward class declarations:
    //class ExportNode;

    public class SceneUpdateContext : System.IDisposable
    {
        public abstract class SurfaceProducerSurface //: System.IDisposable
        {
            //C++ TO C# CONVERTER TODO TASK: C# has no equivalent to ' = default':
            //	virtual ~SurfaceProducerSurface() = default;

            public abstract int AdvanceAndGetAge();

            public abstract bool FlushSessionAcquireAndReleaseEvents();

            //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
            //ORIGINAL LINE: virtual bool IsValid() const = 0;
            public abstract bool IsValid();

            //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
            //ORIGINAL LINE: virtual SKSizeI GetSize() const = 0;
            public abstract SKSizeI GetSize();

            public abstract void SignalWritesFinished(Action on_writes_committed);

            //public abstract scenic.Image GetImage();

            //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
            //ORIGINAL LINE: virtual sk_sp<SKSurface> GetSkiaSurface() const = 0;
            public abstract SKSurface GetSkiaSurface();
        }

        public abstract class SurfaceProducer //: System.IDisposable
        {
            //C++ TO C# CONVERTER TODO TASK: C# has no equivalent to ' = default':
            //	virtual ~SurfaceProducer() = default;

            public abstract SurfaceProducerSurface ProduceSurface(SKSizeI size);

            public abstract void SubmitSurface(SurfaceProducerSurface surface);
        }

        public class Entity : System.IDisposable
        {
            public Entity(SceneUpdateContext context)
            {
                this.context_ = new SceneUpdateContext(context);
                this.previous_entity_ = context.top_entity_;
                this.entity_node_ = context.session();
                if (previous_entity_ != null)
                {
                    previous_entity_.entity_node_.AddChild(entity_node_);
                }
                context.top_entity_ = this;
            }
            public void Dispose()
            {
                FML_DCHECK(context_.top_entity_ == this);
                context_.top_entity_ = previous_entity_;
            }

            public SceneUpdateContext context()
            {
                return context_;
            }
            //public scenic.EntityNode entity_node()
            //{
            //    return entity_node_;
            //}

            private SceneUpdateContext context_;
            private readonly Entity previous_entity_;

            //private scenic.EntityNode entity_node_ = new scenic.EntityNode();
        }

        public class Clip : Entity
        {
            //public Clip(SceneUpdateContext context, scenic.Shape shape, SKRect shape_bounds) : base(context)
            //{
            //    scenic.ShapeNode shape_node = new scenic.ShapeNode(context.session());
            //    shape_node.SetShape(shape);
            //    shape_node.SetTranslation(shape_bounds.width() * 0.5f + shape_bounds.left(), shape_bounds.height() * 0.5f + shape_bounds.top(), 0.0f);

            //    entity_node().AddPart(shape_node);
            //    entity_node().SetClip(0u, true);
            //}
            //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
            //	public void Dispose();
        }

        public class Transform : Entity
        {
            public Transform(SceneUpdateContext context, SKMatrix transform) : base(context)
            {
                this.previous_scale_x_ = context.top_scale_x_;
                this.previous_scale_y_ = context.top_scale_y_;
                if (!transform.isIdentity())
                {
                    // TODO(MZ-192): The perspective and shear components in the matrix
                    // are not handled correctly.
                    MatrixDecomposition decomposition = new MatrixDecomposition(transform);
                    if (decomposition.IsValid())
                    {
                        entity_node().SetTranslation(decomposition.translation().x(), decomposition.translation().y(), decomposition.translation().z());

                        entity_node().SetScale(decomposition.scale().x(), decomposition.scale().y(), decomposition.scale().z());
                        context.top_scale_x_ *= decomposition.scale().x();
                        context.top_scale_y_ *= decomposition.scale().y();

                        entity_node().SetRotation(decomposition.rotation().fData[0], decomposition.rotation().fData[1], decomposition.rotation().fData[2], decomposition.rotation().fData[3]);
                    }
                }
            }
            public Transform(SceneUpdateContext context, float scale_x, float scale_y, float scale_z) : base(context)
            {
                this.previous_scale_x_ = context.top_scale_x_;
                this.previous_scale_y_ = context.top_scale_y_;
                if (scale_x != 1.0f || scale_y != 1.0f || scale_z != 1.0f)
                {
                    entity_node().SetScale(scale_x, scale_y, scale_z);
                    context.top_scale_x_ *= scale_x;
                    context.top_scale_y_ *= scale_y;
                }
            }
            public new void Dispose()
            {
                context().top_scale_x_ = previous_scale_x_;
                context().top_scale_y_ = previous_scale_y_;
                base.Dispose();
            }

            private readonly float previous_scale_x_;
            private readonly float previous_scale_y_;
        }

        public class Frame : Entity
        {
            public Frame(SceneUpdateContext context, SKRRect rrect, uint color, float elevation) : base(context)
            {
                this.rrect_ = new SKRRect(rrect);
                this.color_ = color;
                this.paint_bounds_ = new SKRect(SKRect.Empty);
                if (elevation != 0.0F)
                {
                    entity_node().SetTranslation(0.0f, 0.0f, elevation);
                }
            }
            public new void Dispose()
            {
                context().CreateFrame(entity_node(), rrect_, color_, paint_bounds_, paint_layers_);
                base.Dispose();
            }

            public void AddPaintedLayer(Layer layer)
            {
                FML_DCHECK(layer.needs_painting());
                paint_layers_.Add(layer);
                paint_bounds_.join(layer.paint_bounds());
            }

            private readonly SKRRect rrect_;
            private readonly uint color_;

            private List<Layer> paint_layers_ = new List<Layer>();
            private SKRect paint_bounds_ = new SKRect();
        }

        //public SceneUpdateContext(scenic.Session session, SurfaceProducer surface_producer)
        //{
        //    this.session_ = session;
        //    this.surface_producer_ = surface_producer;
        //    FML_DCHECK(surface_producer_ != null);
        //}

        public void Dispose()
        {
            // Release Mozart session resources for all ExportNodes.
            foreach (var export_node in export_nodes_)
            {
                export_node.Dispose(false);
            }
        }

        //public scenic.Session session()
        //{
        //    return session_;
        //}

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: bool has_metrics() const
        //public bool has_metrics()
        //{
        //    return !metrics_ == null;
        //}
        //public void set_metrics(fuchsia.ui.gfx.MetricsPtr metrics)
        //{
        //    metrics_ = std::move(metrics);
        //}
        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: const fuchsia::ui::gfx::MetricsPtr& metrics() const
        //public fuchsia.ui.gfx.MetricsPtr metrics()
        //{
        //    return metrics_;
        //}

        public void AddChildScene(ExportNode export_node, SKPoint offset, bool hit_testable)
        {
            FML_DCHECK(top_entity_);

            export_node.Bind(this, top_entity_.entity_node(), offset, hit_testable);
        }

        // Adds reference to |export_node| so we can call export_node->Dispose() in
        // our destructor. Caller is responsible for calling RemoveExportNode() before
        // |export_node| is destroyed.
        public void AddExportNode(ExportNode export_node)
        {
            export_nodes_.Add(export_node); // Might already have been added.
        }

        // Removes reference to |export_node|.
        public void RemoveExportNode(ExportNode export_node)
        {
            export_nodes_.erase(export_node);
        }

        // TODO(chinmaygarde): This method must submit the surfaces as soon as paint
        // tasks are done. However, given that there is no support currently for
        // Vulkan semaphores, we need to submit all the surfaces after an explicit
        // CPU wait. Once Vulkan semaphores are available, this method must return
        // void and the implementation must submit surfaces on its own as soon as the
        // specific canvas operations are done.
        public List<SceneUpdateContext.SurfaceProducerSurface> ExecutePaintTasks(CompositorContext.ScopedFrame frame)
        {
            TRACE_EVENT0("flutter", "SceneUpdateContext::ExecutePaintTasks");
            List<SurfaceProducerSurface> surfaces_to_submit = new List<SurfaceProducerSurface>();
            foreach (var task in paint_tasks_)
            {
                //FML_DCHECK(task.surface);
                SKCanvas canvas = task.surface.GetSkiaSurface().Canvas;
                Layer.PaintContext context = new Layer.PaintContext(canvas, frame.context().frame_time(), frame.context().engine_time(), frame.context().texture_registry(), frame.context().raster_cache(), false);
                canvas.RestoreToCount(1);
                canvas.Save();
                canvas.Clear(task.background_color);
                canvas.Scale(task.scale_x, task.scale_y);
                canvas.Translate(-task.left, -task.top);
                foreach (Layer layer in task.layers)
                {
                    layer.Paint(context);
                }
                surfaces_to_submit.emplace_back(task.surface);
            }
            paint_tasks_.Clear();
            return surfaces_to_submit;
        }

        private class PaintTask
        {
            public SurfaceProducerSurface surface;
            public float left;
            public float top;
            public float scale_x;
            public float scale_y;
            public uint background_color;
            public List<Layer> layers = new List<Layer>();
        }

        //private void CreateFrame(scenic.EntityNode entity_node, SKRRect rrect, uint color, SKRect paint_bounds, List<Layer> paint_layers)
        //{
        //    // Frames always clip their children.
        //    entity_node.SetClip(0u, true);

        //    // We don't need a shape if the frame is zero size.
        //    if (rrect.isEmpty())
        //    {
        //        return;
        //    }

        //    // Add a part which represents the frame's geometry for clipping purposes
        //    // and possibly for its texture.
        //    // TODO(MZ-137): Need to be able to express the radii as vectors.
        //    SKRect shape_bounds = rrect.getBounds();
        //    scenic.RoundedRectangle shape = new scenic.RoundedRectangle(session_, rrect.width(), rrect.height(), rrect.radii(SKRRect.Corner.kUpperLeft_Corner).x(), rrect.radii(SKRRect.Corner.kUpperRight_Corner).x(), rrect.radii(SKRRect.Corner.kLowerRight_Corner).x(), rrect.radii(SKRRect.Corner.kLowerLeft_Corner).x());
        //    scenic.ShapeNode shape_node = new scenic.ShapeNode(session_);
        //    shape_node.SetShape(shape);
        //    shape_node.SetTranslation(shape_bounds.width() * 0.5f + shape_bounds.left(), shape_bounds.height() * 0.5f + shape_bounds.top(), 0.0f);
        //    entity_node.AddPart(shape_node);

        //    // Check whether the painted layers will be visible.
        //    if (paint_bounds.isEmpty() || !paint_bounds.intersects(shape_bounds))
        //    {
        //        paint_layers.Clear();
        //    }

        //    // Check whether a solid color will suffice.
        //    if (paint_layers.Count == 0)
        //    {
        //        SetShapeColor(shape_node, color);
        //        return;
        //    }

        //    // Apply current metrics and transformation scale factors.
        //    float scale_x = metrics_.scale_x * top_scale_x_;
        //    float scale_y = metrics_.scale_y * top_scale_y_;

        //    // If the painted area only covers a portion of the frame then we can
        //    // reduce the texture size by drawing just that smaller area.
        //    //C++ TO C# CONVERTER TODO TASK: The following line was determined to contain a copy constructor call - this should be verified and a copy constructor should be created:
        //    //ORIGINAL LINE: SKRect inner_bounds = shape_bounds;
        //    SKRect inner_bounds = new SKRect(shape_bounds);
        //    inner_bounds.intersect(paint_bounds);
        //    if (inner_bounds != shape_bounds && rrect.contains(inner_bounds))
        //    {
        //        SetShapeColor(shape_node, color);

        //        scenic.Rectangle inner_shape = new scenic.Rectangle(session_, inner_bounds.width(), inner_bounds.height());
        //        scenic.ShapeNode inner_node = new scenic.ShapeNode(session_);
        //        inner_node.SetShape(inner_shape);
        //        inner_node.SetTranslation(inner_bounds.width() * 0.5f + inner_bounds.left(), inner_bounds.height() * 0.5f + inner_bounds.top(), 0.0f);
        //        entity_node.AddPart(inner_node);
        //        SetShapeTextureOrColor(inner_node, color, scale_x, scale_y, inner_bounds, std::move(paint_layers));
        //        return;
        //    }

        //    // Apply a texture to the whole shape.
        //    SetShapeTextureOrColor(shape_node, color, scale_x, scale_y, shape_bounds, std::move(paint_layers));
        //}
        //private void SetShapeTextureOrColor(scenic.ShapeNode shape_node, uint color, float scale_x, float scale_y, SKRect paint_bounds, List<Layer> paint_layers)
        //{
        //    scenic.Image image = GenerateImageIfNeeded(color, scale_x, scale_y, paint_bounds, std::move(paint_layers));
        //    if (image != null)
        //    {
        //        scenic.Material material = new scenic.Material(session_);
        //        material.SetTexture(image);
        //        shape_node.SetMaterial(material);
        //        return;
        //    }

        //    SetShapeColor(shape_node, color);
        //}
        //private void SetShapeColor(scenic.ShapeNode shape_node, uint color)
        //{
        //    if ((((color) >> 24) & 0xFF) == 0)
        //    {
        //        return;
        //    }

        //    scenic.Material material = new scenic.Material(session_);
        //    material.SetColor((((color) >> 16) & 0xFF), (((color) >> 8) & 0xFF), (((color) >> 0) & 0xFF), (((color) >> 24) & 0xFF));
        //    shape_node.SetMaterial(material);
        //}
        //private scenic.Image GenerateImageIfNeeded(uint color, float scale_x, float scale_y, SKRect paint_bounds, List<Layer> paint_layers)
        //{
        //    // Bail if there's nothing to paint.
        //    if (paint_layers.Count == 0)
        //    {
        //        return null;
        //    }

        //    // Bail if the physical bounds are empty after rounding.
        //    SKSizeI physical_size = SKSizeI.Make(paint_bounds.width() * scale_x, paint_bounds.height() * scale_y);
        //    if (physical_size.isEmpty())
        //    {
        //        return null;
        //    }

        //    // Acquire a surface from the surface producer and register the paint tasks.
        //    var surface = surface_producer_.ProduceSurface(physical_size);

        //    if (surface == null)
        //    {
        //        //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the following C++ macro:
        //        !((global::fml.ShouldCreateLogMessage(global::fml.LOG_ERROR))) ? ()0 : new global::fml.LogMessageVoidify() & (new global::fml.LogMessage(global::fml.LOG_ERROR, __FILE__, __LINE__, null).stream()) << "Could not acquire a surface from the surface producer " + "of size: " << physical_size.width() << "x" << physical_size.height();
        //        return null;
        //    }

        //    var image = surface.GetImage();

        //    // Enqueue the paint task.
        //    paint_tasks_.Add({.surface = std::move(surface), .left = paint_bounds.left(), .top = paint_bounds.top(), .scale_x = scale_x, .scale_y = scale_y, .background_color = color, .layers = std::move(paint_layers)});
        //    return image;
        //}

        private Entity top_entity_ = null;
        private float top_scale_x_ = 1.0f;
        private float top_scale_y_ = 1.0f;

        //private readonly scenic.Session session_;
        private readonly SurfaceProducer surface_producer_;

        //private fuchsia.ui.gfx.MetricsPtr metrics_ = new fuchsia.ui.gfx.MetricsPtr();

        private List<PaintTask> paint_tasks_ = new List<PaintTask>();

        // Save ExportNodes so we can dispose them in our destructor.
        private SortedSet<ExportNode> export_nodes_ = new SortedSet<ExportNode>();

        //C++ TO C# CONVERTER TODO TASK: C# has no equivalent to ' = delete':
        //  SceneUpdateContext(const SceneUpdateContext&) = delete;
        //C++ TO C# CONVERTER TODO TASK: C# has no equivalent to ' = delete':
        //  SceneUpdateContext& operator =(const SceneUpdateContext&) = delete;
    }

}