using SkiaSharp;
using static FlutterBinding.Flow.Helper;

// Copyright 2016 The Chromium Authors. All rights reserved.
// Use of this source code is governed by a BSD-style license that can be
// found in the LICENSE file.

namespace FlutterBinding.Flow
{

    // Wrapper class for ExportNode to use on UI Thread. When ExportNodeHolder is
    // destroyed, a task is posted on the Rasterizer thread to dispose the resources
    // held by the ExportNode.
    public class ExportNodeHolder //: fml.RefCountedThreadSafe<ExportNodeHolder>
    {
        //public ExportNodeHolder(fml.RefPtr<fml.TaskRunner> gpu_task_runner, fml.RefPtr<zircon.dart.Handle> export_token_handle)
        //{
        //    this.gpu_task_runner_ = new fml.RefPtr<fml.TaskRunner>(std::move(gpu_task_runner));
        //    this.export_node_ = std::make_unique<ExportNode>(export_token_handle);
        //    FML_DCHECK(gpu_task_runner_);
        //}
        //public new void Dispose()
        //{
        //    //C++ TO C# CONVERTER TODO TASK: Only lambda expressions having all locals passed by reference can be converted to C#:
        //    //ORIGINAL LINE: gpu_task_runner_->PostTask(fml::MakeCopyable([export_node = std::move(export_node_)]()
        //    gpu_task_runner_.Dereference().PostTask(fml.GlobalMembers.MakeCopyable.functorMethod(() =>
        //    {
        //        export_node.Dispose(true);
        //    }));
        //    base.Dispose();
        //}

        //// Calls Bind() on the wrapped ExportNode.
        //public void Bind(SceneUpdateContext context, scenic.ContainerNode container, SKPoint offset, bool hit_testable)
        //{
        //    export_node_.Bind(context, container, offset, hit_testable);
        //}

        //public ExportNode export_node()
        //{
        //    return export_node_.get();
        //}

        //private fml.RefPtr<fml.TaskRunner> gpu_task_runner_ = new fml.RefPtr<fml.TaskRunner>();
        //private std::unique_ptr<ExportNode> export_node_ = new std::unique_ptr<ExportNode>();

        //C++ TO C# CONVERTER TODO TASK: C# has no concept of a 'friend' class:
        //  friend class::fml::internal::MakeRefCountedHelper<ExportNodeHolder>;
        //C++ TO C# CONVERTER TODO TASK: C# has no concept of a 'friend' class:
        //  friend class::fml::RefCountedThreadSafe<ExportNodeHolder>;
        //C++ TO C# CONVERTER TODO TASK: C# has no equivalent to ' = delete':
        //  ExportNodeHolder(const ExportNodeHolder&) = delete;
        //C++ TO C# CONVERTER TODO TASK: C# has no equivalent to ' = delete':
        //  ExportNodeHolder& operator =(const ExportNodeHolder&) = delete;
    }

    // Represents a node which is being exported from the session.
    // This object is created on the UI thread but the entity node it contains
    // must be created and destroyed by the rasterizer thread.
    public class ExportNode : System.IDisposable
    {
        //public ExportNode(fml.RefPtr<zircon.dart.Handle> export_token_handle)
        //{
        //    this.export_token_ = export_token_handle.Dereference().ReleaseHandle();
        //}

        public void Dispose()
        {
            // Ensure that we properly released the node.
            //FML_DCHECK(node_ == null);
            //FML_DCHECK(scene_update_context_ == null);
        }

        // Binds the export token to the entity node and adds it as a child of
        // the specified container. Must be called on the Rasterizer thread.
        //public void Bind(SceneUpdateContext context, scenic.ContainerNode container, SKPoint offset, bool hit_testable)
        //{
        //    if (export_token_ != null)
        //    {
        //        // Happens first time we bind.
        //        node_.reset(new scenic.EntityNode(container.session()));
        //        node_.Export(std::move(export_token_));

        //        // Add ourselves to the context so it can call Dispose() on us if the Scenic
        //        // session is closed.
        //        context.AddExportNode(this);
        //        scene_update_context_ = context;
        //    }

        //    if (node_ != null)
        //    {
        //        container.AddChild(*node_);
        //        node_.SetTranslation(offset.X, offset.Y, 0.0f);
        //        //node_.SetHitTestBehavior(hit_testable ? fuchsia.ui.gfx.HitTestBehavior.kDefault : fuchsia.ui.gfx.HitTestBehavior.kSuppress);
        //    }
        //}

        //C++ TO C# CONVERTER TODO TASK: C# has no concept of a 'friend' class:
        //  friend class SceneUpdateContext;
        //C++ TO C# CONVERTER TODO TASK: C# has no concept of a 'friend' class:
        //  friend class ExportNodeHolder;

        // Cleans up resources held and removes this ExportNode from
        // SceneUpdateContext. Must be called on the Rasterizer thread.
        private void Dispose(bool remove_from_scene_update_context)
        {
            // If scene_update_context_ is set, then we should still have a node left to
            // dereference.
            // If scene_update_context_ is null, then either:
            // 1. A node was never created, or
            // 2. A node was created but was already dereferenced (i.e. Dispose has
            // already been called).
            //FML_DCHECK(scene_update_context_ != null || node_ == null);

            //if (remove_from_scene_update_context && scene_update_context_ != null)
            //{
            //    scene_update_context_.RemoveExportNode(this);
            //}

            //scene_update_context_ = null;
            //export_token_.reset();
            //node_ = null;
        }

        // Member variables can only be read or modified on Rasterizer thread.
        private SceneUpdateContext scene_update_context_ = null;
        //private zx.eventpair export_token_ = new zx.eventpair();
        //private std::unique_ptr<scenic.EntityNode> node_ = new std::unique_ptr<scenic.EntityNode>();

        //C++ TO C# CONVERTER TODO TASK: C# has no equivalent to ' = delete':
        //  ExportNode(const ExportNode&) = delete;
        //C++ TO C# CONVERTER TODO TASK: C# has no equivalent to ' = delete':
        //  ExportNode& operator =(const ExportNode&) = delete;
    }

}