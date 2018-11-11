using static FlutterBinding.Flow.Helper;
using SkiaSharp;

// Copyright 2017 The Chromium Authors. All rights reserved.
// Use of this source code is governed by a BSD-style license that can be
// found in the LICENSE file.
//

namespace FlutterBinding.Flow
{

    public class EmbeddedViewParams
    {
        public SKPoint offsetPixels = new SKPoint();
        public SKSize sizePoints = new SKSize();
    }

    // This is only used on iOS when running in a non headless mode,
    // in this case ViewEmbedded is a reference to the
    // FlutterPlatformViewsController which is owned by FlutterViewController.
    public class ExternalViewEmbedder
    {      

        // Must be called on the UI thread.
        public virtual void CompositeEmbeddedView(ulong view_id, EmbeddedViewParams @params)
        {
        }
    }

} // namespace flow

