**ABANDONED: While this project is technically feasible, it's not feasible for a couple of developers in their spare time to finish. It was only meant as a proof of concept and has taken several months to reach its current status and it may even take several more to complete. As stated, this project was never expected to be commercially viable. Due to the effort remaining and no future for this project, it was decided it was not worth our time to complete. (Adam Pedley) - Personally I will now be focussing on the Flutter eco-system.**

# Flutter for Xamarin

*This project has no affiliation with Microsoft, Google or the Xamarin or Flutter teams.*

**This project is still under construction, and a full working version does not exist yet. We will let everyone know when we finally get things running end to end.**

The project is designed to make the Flutter SDK available on the .NET Framework, initially with the supported platforms of:

1) Xamarin.Android
2) Xamarin.iOS
3) UWP

There is no reason this couldn't also expand to any place where [SkiaSharp](https://github.com/mono/SkiaSharp) is supported.

This project is never expected to be commercially viable, unless it is picked up or supported by a larger company. As it currently stands, this is just a fun side project, done by a bunch of developers in their spare time. We offer no support for solutions ever built with this framework, or any guarantee of completion.

# Overview

To make this work, numerous components need to come together.

![alt text](https://github.com/adamped/xamarin.flutter/blob/master/XamarinFlutterProject.png?raw=true "Overview of project")

## Transpiler

The transpiler, will convert the existing Flutter SDK (written in Dart) to C#, for consumption in .NET applications.

Using the DartAnalyzer, we analyze the Flutter SDK, and output C#.

## Bindings

Flutter Bindings will include the connection between SkiaSharp and the Flutter SDK. We will not be using the actual Flutter engine, though we may do in the future.

Due to previous exploratory work, the Flutter engine is difficult to integrate with since we need to expose many C++ level APIs. Due to how this is implemented in the Flutter engine currently, it would require certain modifications and would be difficult to keep in sync with the master repository.

Hence, we will map the calls directly to SkiaSharp and Harfbuzz to draw directly on a SkiaCanvas. I could regret this, we shall see :)

## Shell

The Shell, is where the Skia Canvas is initialized and platform level events are collected, and sent through to the Bindings project.

# License

All code here is licensed under the [MIT license](LICENSE)
