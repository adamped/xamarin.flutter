// Copyright 2017 The Chromium Authors. All rights reserved.
// Use of this source code is governed by a BSD-style license that can be
// found in the LICENSE file.
//

namespace FlutterBinding.Flow
{

    public class EmbeddedViewParams
    {
        public SkPoint offsetPixels = new SkPoint();
        public SkSize sizePoints = new SkSize();
    }

    // This is only used on iOS when running in a non headless mode,
    // in this case ViewEmbedded is a reference to the
    // FlutterPlatformViewsController which is owned by FlutterViewController.
    public class ExternalViewEmbedder : System.IDisposable
    {
        //C++ TO C# CONVERTER TODO TASK: C# has no equivalent to ' = default':
        //  ExternalViewEmbedder() = default;

        // Must be called on the UI thread.
        public virtual void CompositeEmbeddedView(int view_id, EmbeddedViewParams @params)
        {
        }

        //C++ TO C# CONVERTER TODO TASK: C# has no equivalent to ' = default':
        //  virtual ~ExternalViewEmbedder() = default;

        //C++ TO C# CONVERTER TODO TASK: C# has no equivalent to ' = delete':
        //  ExternalViewEmbedder(const ExternalViewEmbedder&) = delete;
        //C++ TO C# CONVERTER TODO TASK: C# has no equivalent to ' = delete':
        //  ExternalViewEmbedder& operator =(const ExternalViewEmbedder&) = delete;
    }

} // namespace flow

