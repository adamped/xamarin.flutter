using SkiaSharp;
using static FlutterBinding.Flow.Helper;

// Copyright 2016 The Chromium Authors. All rights reserved.
// Use of this source code is governed by a BSD-style license that can be
// found in the LICENSE file.

namespace FlutterBinding.Flow.Layers
{

    // Layer that represents an embedded child.
    public class ChildSceneLayer : Layer
    {
        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //  ChildSceneLayer();
        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //  public void Dispose();

        public void set_offset(SKPoint offset)
        {
            //C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
            //ORIGINAL LINE: offset_ = offset;
            offset_.CopyFrom(offset);
        }

        public void set_size(SKSize size)
        {
            size_.CopyFrom(size);
        }

        public void set_export_node_holder(ref ExportNodeHolder export_node_holder)
        {
            export_node_holder_ = export_node_holder;
        }

        public void set_hit_testable(bool hit_testable)
        {
            hit_testable_ = hit_testable;
        }

        public override void Preroll(PrerollContext context, SKMatrix matrix)
        {
            set_needs_system_composite(true);
        }

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: void Paint(PaintContext& context) const override
        public override void Paint(PaintContext context)
        {
            FXL_NOTREACHED() << "This layer never needs painting.";
        }

        public override void UpdateScene(SceneUpdateContext context)
        {
            FML_DCHECK(needs_system_composite());

            // TODO(MZ-191): Set clip.
            // It's worth asking whether all children should be clipped implicitly
            // or whether we should leave this up to the Flutter application to decide.
            // In some situations, it might be useful to allow children to draw
            // outside of their layout bounds.
            if (export_node_holder_ != null)
            {
                //C++ TO C# CONVERTER TODO TASK: The following line was determined to contain a copy constructor call - this should be verified and a copy constructor should be created:
                //ORIGINAL LINE: context.AddChildScene(export_node_holder_->export_node(), offset_, hit_testable_);
                context.AddChildScene(export_node_holder_.Dereference().export_node(), new SKPoint(offset_), hit_testable_);
            }
        }

        private SKPoint offset_ = new SKPoint();
        private SKSize size_ = new SKSize();
        private ExportNodeHolder export_node_holder_ = new fml.RefPtr<ExportNodeHolder>();
        private bool hit_testable_ = true;

        //C++ TO C# CONVERTER TODO TASK: C# has no equivalent to ' = delete':
        //  ChildSceneLayer(const ChildSceneLayer&) = delete;
        //C++ TO C# CONVERTER TODO TASK: C# has no equivalent to ' = delete':
        //  ChildSceneLayer& operator =(const ChildSceneLayer&) = delete;
    }

}