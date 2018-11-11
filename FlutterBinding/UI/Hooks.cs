using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using static FlutterBinding.Mapping.Helper;
using static FlutterBinding.Mapping.Types;
using static FlutterBinding.UI.Painting;

namespace FlutterBinding.UI
{
    public static class Hooks
    {      
        static void _updateWindowMetrics(double devicePixelRatio,
                                  double width,
                                  double height,
                                  double paddingTop,
                                  double paddingRight,
                                  double paddingBottom,
                                  double paddingLeft,
                                  double viewInsetTop,
                                  double viewInsetRight,
                                  double viewInsetBottom,
                                  double viewInsetLeft)
        {
            var window = Window.Instance;
            window.devicePixelRatio = devicePixelRatio;
            window.physicalSize = new Size(width, height);
            window.padding = new WindowPadding(
                top: paddingTop,
                right: paddingRight,
                bottom: paddingBottom,
                left: paddingLeft);
            window.viewInsets = new WindowPadding(
                top: viewInsetTop,
                right: viewInsetRight,
                bottom: viewInsetBottom,
                left: viewInsetLeft);
            _invoke(Window.Instance.onMetricsChanged, Window.Instance._onMetricsChangedZone);
        }

        delegate string _LocaleClosure();

        static String _localeClosure() => Window.Instance.locale.toString();

        static _LocaleClosure _getLocaleClosure() => _localeClosure;

        static void _updateLocales(List<String> locales)
        {
            const int stringsPerLocale = 4;
            int numLocales = (int)Math.Truncate((double)locales.Count / stringsPerLocale);
            Window.Instance.locales = new List<Locale>(numLocales);
            for (int localeIndex = 0; localeIndex < numLocales; localeIndex++)
            {
                Window.Instance.locales[localeIndex] = new Locale(locales[localeIndex * stringsPerLocale],
                                               locales[localeIndex * stringsPerLocale + 1]);
            }
            _invoke(Window.Instance.onLocaleChanged, Window.Instance._onLocaleChangedZone);
        }

        static void _updateUserSettingsData(String jsonData)
        {
            Dictionary<String, Object> data = JsonConvert.DeserializeObject<Dictionary<String, Object>>(jsonData);
            _updateTextScaleFactor(Convert.ToDouble(data["textScaleFactor"]));
            _updateAlwaysUse24HourFormat(Convert.ToBoolean(data["alwaysUse24HourFormat"]));
        }

        static void _updateTextScaleFactor(double textScaleFactor)
        {
            Window.Instance.textScaleFactor = textScaleFactor;
            _invoke(Window.Instance.onTextScaleFactorChanged, Window.Instance._onTextScaleFactorChangedZone);
        }

        static void _updateAlwaysUse24HourFormat(bool alwaysUse24HourFormat)
        {
            Window.Instance.alwaysUse24HourFormat = alwaysUse24HourFormat;
        }

        static void _updateSemanticsEnabled(bool enabled)
        {
            Window.Instance.semanticsEnabled = enabled;
            _invoke(Window.Instance.onSemanticsEnabledChanged, Window.Instance._onSemanticsEnabledChangedZone);
        }

        static void _updateAccessibilityFeatures(int values)
        {
            AccessibilityFeatures newFeatures = new AccessibilityFeatures(values);
            if (newFeatures == Window.Instance.accessibilityFeatures)
                return;
            Window.Instance.accessibilityFeatures = newFeatures;
            _invoke(Window.Instance.onAccessibilityFeaturesChanged, Window.Instance._onAccessibilityFlagsChangedZone);
        }

        static void _dispatchPlatformMessage(String name, ByteData data, int responseId)
        {
            if (Window.Instance.onPlatformMessage != null)
            {
                _invoke3<String, ByteData, PlatformMessageResponseCallback>(
                 (a, b, c) => Window.Instance.onPlatformMessage(a, b, c),
                  Window.Instance._onPlatformMessageZone,
                  name,
                  data,
                  (ByteData responseData) =>
                  {
                      Window.Instance._respondToPlatformMessage(responseId, responseData);
                  });
            }
            else
            {
                Window.Instance._respondToPlatformMessage(responseId, null);
            }
        }

        static void _dispatchPointerDataPacket(ByteData packet)
        {
            if (Window.Instance.onPointerDataPacket != null)
                _invoke1<PointerDataPacket>((d) => Window.Instance.onPointerDataPacket(d), Window.Instance._onPointerDataPacketZone, _unpackPointerDataPacket(packet));
        }

        static void _dispatchSemanticsAction(int id, int action, ByteData args)
        {
            _invoke3<int, SemanticsAction, ByteData>(
             (a, b, c) => Window.Instance.onSemanticsAction(a, b, c),
              Window.Instance._onSemanticsActionZone,
              id,
              SemanticsAction.values[action],
              args);
        }

        static void _beginFrame(int microseconds)
        {
            _invoke1<Duration>((d) => Window.Instance.onBeginFrame(d), Window.Instance._onBeginFrameZone, new Duration(microseconds: microseconds));
        }

        static void _drawFrame()
        {
            _invoke(Window.Instance.onDrawFrame, Window.Instance._onDrawFrameZone);
        }

        /// Invokes [callback] inside the given [zone].
        static void _invoke(VoidCallback callback, Zone zone)
        {
            if (callback == null)
                return;

            //assert(zone != null);

            if (identical(zone, Zone.current))
            {
                callback();
            }
            else
            {
                zone.runGuarded(callback);
            }
        }

        /// Invokes [callback] inside the given [zone] passing it [arg].
        static void _invoke1<A>(Action<A> callback, Zone zone, A arg)
        {
            if (callback == null)
                return;
            
            if (identical(zone, Zone.current))
            {
                callback(arg);
            }
            else
            {
                zone.runUnaryGuarded<A>(callback, arg);
            }
        }

        /// Invokes [callback] inside the given [zone] passing it [arg1] and [arg2].
        static void _invoke2<A1, A2>(Action<A1, A2> callback, Zone zone, A1 arg1, A2 arg2)
        {
            if (callback == null)
                return;
            
            if (identical(zone, Zone.current))
            {
                callback(arg1, arg2);
            }
            else
            {
                zone.runBinaryGuarded<A1, A2>(callback, arg1, arg2);
            }
        }

        /// Invokes [callback] inside the given [zone] passing it [arg1], [arg2] and [arg3].
        static void _invoke3<A1, A2, A3>(Action<A1, A2, A3> callback, Zone zone, A1 arg1, A2 arg2, A3 arg3)
        {
            if (callback == null)
                return;

            if (identical(zone, Zone.current))
            {
                callback(arg1, arg2, arg3);
            }
            else
            {
                zone.runGuarded(() =>
                {
                    callback(arg1, arg2, arg3);
                });
            }
        }

        // If this value changes, update the encoding code in the following files:
        //
        //  * pointer_data.cc
        //  * FlutterView.java
        const int _kPointerDataFieldCount = 19;

        static PointerDataPacket _unpackPointerDataPacket(ByteData packet)
        {
            const int kStride = 8; // Its an 8 const anyway - Int64List.bytesPerElement;
            const int kBytesPerPointerData = _kPointerDataFieldCount * kStride;
            int length = packet.lengthInBytes / kBytesPerPointerData;
            List<PointerData> data = new List<PointerData>(length);
            for (int i = 0; i < length; ++i)
            {
                int offset = i * _kPointerDataFieldCount;
                data[i] = new PointerData(
                  timeStamp: new Duration(microseconds: packet.getInt64(kStride * offset++, (int)_kFakeHostEndian)),
                  change: (PointerChange)packet.getInt64(kStride * offset++, (int)_kFakeHostEndian),
                  kind: (PointerDeviceKind)packet.getInt64(kStride * offset++, (int)_kFakeHostEndian),
                  device: packet.getInt64(kStride * offset++, (int)_kFakeHostEndian),
                  physicalX: packet.getFloat64(kStride * offset++, (int)_kFakeHostEndian),
                  physicalY: packet.getFloat64(kStride * offset++, (int)_kFakeHostEndian),
                  buttons: packet.getInt64(kStride * offset++, (int)_kFakeHostEndian),
                  obscured: packet.getInt64(kStride * offset++, (int)_kFakeHostEndian) != 0,
                  pressure: packet.getFloat64(kStride * offset++, (int)_kFakeHostEndian),
                  pressureMin: packet.getFloat64(kStride * offset++, (int)_kFakeHostEndian),
                  pressureMax: packet.getFloat64(kStride * offset++, (int)_kFakeHostEndian),
                  distance: packet.getFloat64(kStride * offset++, (int)_kFakeHostEndian),
                  distanceMax: packet.getFloat64(kStride * offset++, (int)_kFakeHostEndian),
                  radiusMajor: packet.getFloat64(kStride * offset++, (int)_kFakeHostEndian),
                  radiusMinor: packet.getFloat64(kStride * offset++, (int)_kFakeHostEndian),
                  radiusMin: packet.getFloat64(kStride * offset++, (int)_kFakeHostEndian),
                  radiusMax: packet.getFloat64(kStride * offset++, (int)_kFakeHostEndian),
                  orientation: packet.getFloat64(kStride * offset++, (int)_kFakeHostEndian),
                  tilt: packet.getFloat64(kStride * offset++, (int)_kFakeHostEndian)
                );
            }
            return new PointerDataPacket(data: data);
        }
    }
}