using System;
using System.Collections.Generic;
using static FlutterBinding.Mapping.Types;

namespace FlutterBinding.UI
{

    /// How the pointer has changed since the last report.
    public enum PointerChange
    {
        /// The input from the pointer is no longer directed towards this receiver.
        cancel,

        /// The device has started tracking the pointer.
        ///
        /// For example, the pointer might be hovering above the device, having not yet
        /// made contact with the surface of the device.
        add,

        /// The device is no longer tracking the pointer.
        ///
        /// For example, the pointer might have drifted out of the device's hover
        /// detection range or might have been disconnected from the system entirely.
        remove,

        /// The pointer has moved with respect to the device while not in contact with
        /// the device.
        hover,

        /// The pointer has made contact with the device.
        down,

        /// The pointer has moved with respect to the device while in contact with the
        /// device.
        move,

        /// The pointer has stopped making contact with the device.
        up,
    }

    /// The kind of pointer device.
    public enum PointerDeviceKind
    {
        /// A touch-based pointer device.
        touch,

        /// A mouse-based pointer device.
        mouse,

        /// A pointer device with a stylus.
        stylus,

        /// A pointer device with a stylus that has been inverted.
        invertedStylus,

        /// An unknown pointer device.
        unknown
    }

    /// Information about the state of a pointer.
    public class PointerData
    {
        /// Creates an object that represents the state of a pointer.
        public PointerData(
          Duration timeStamp = null,
          PointerChange change = PointerChange.cancel,
          PointerDeviceKind kind = PointerDeviceKind.touch,
          int device = 0,
          double physicalX = 0.0,
          double physicalY = 0.0,
          int buttons = 0,
          bool obscured = false,
          double pressure = 0.0,
          double pressureMin = 0.0,
          double pressureMax = 0.0,
          double distance = 0.0,
          double distanceMax = 0.0,
          double radiusMajor = 0.0,
          double radiusMinor = 0.0,
          double radiusMin = 0.0,
          double radiusMax = 0.0,
          double orientation = 0.0,
          double tilt = 0.0
      )
        {
            if (timeStamp == null)
                timeStamp = Duration.zero;

            this.timeStamp = timeStamp;
            this.change = change;
            this.kind = kind;
            this.device = device;
            this.physicalX = physicalX;
            this.physicalY = physicalY;
            this.buttons = buttons;
            this.obscured = obscured;
            this.pressure = pressure;
            this.pressureMin = pressureMin;
            this.pressureMax = pressureMax;
            this.distance = distance;
            this.distanceMax = distanceMax;
            this.radiusMajor = radiusMajor;
            this.radiusMinor = radiusMinor;
            this.radiusMin = radiusMin;
            this.radiusMax = radiusMax;
            this.orientation = orientation;
            this.tilt = tilt;
        }

        /// Time of event dispatch, relative to an arbitrary timeline.
        public readonly Duration timeStamp;

        /// How the pointer has changed since the last report.
        public readonly PointerChange change;

        /// The kind of input device for which the event was generated.
        public readonly PointerDeviceKind kind;

        /// Unique identifier for the pointing device, reused across interactions.
        public readonly int device;

        /// X coordinate of the position of the pointer, in physical pixels in the
        /// global coordinate space.
        public readonly double physicalX;

        /// Y coordinate of the position of the pointer, in physical pixels in the
        /// global coordinate space.
        public readonly double physicalY;

        /// Bit field using the *Button constants (primaryMouseButton,
        /// secondaryStylusButton, etc). For example, if this has the value 6 and the
        /// [kind] is [PointerDeviceKind.invertedStylus], then this indicates an
        /// upside-down stylus with both its primary and secondary buttons pressed.
        public readonly int buttons;

        /// Set if an application from a different security domain is in any way
        /// obscuring this application's window. (Aspirational; not currently
        /// implemented.)
        public readonly bool obscured;

        /// The pressure of the touch as a number ranging from 0.0, indicating a touch
        /// with no discernible pressure, to 1.0, indicating a touch with "normal"
        /// pressure, and possibly beyond, indicating a stronger touch. For devices
        /// that do not detect pressure (e.g. mice), returns 1.0.
        public readonly double pressure;

        /// The minimum value that [pressure] can return for this pointer. For devices
        /// that do not detect pressure (e.g. mice), returns 1.0. This will always be
        /// a number less than or equal to 1.0.
        public readonly double pressureMin;

        /// The maximum value that [pressure] can return for this pointer. For devices
        /// that do not detect pressure (e.g. mice), returns 1.0. This will always be
        /// a greater than or equal to 1.0.
        public readonly double pressureMax;

        /// The distance of the detected object from the input surface (e.g. the
        /// distance of a stylus or finger from a touch screen), in arbitrary units on
        /// an arbitrary (not necessarily linear) scale. If the pointer is down, this
        /// is 0.0 by definition.
        public readonly double distance;

        /// The maximum value that a distance can return for this pointer. If this
        /// input device cannot detect "hover touch" input events, then this will be
        /// 0.0.
        public readonly double distanceMax;

        /// The radius of the contact ellipse along the major axis, in logical pixels.
        public readonly double radiusMajor;

        /// The radius of the contact ellipse along the minor axis, in logical pixels.
        public readonly double radiusMinor;

        /// The minimum value that could be reported for radiusMajor and radiusMinor
        /// for this pointer, in logical pixels.
        public readonly double radiusMin;

        /// The minimum value that could be reported for radiusMajor and radiusMinor
        /// for this pointer, in logical pixels.
        public readonly double radiusMax;

        /// For PointerDeviceKind.touch events:
        ///
        /// The angle of the contact ellipse, in radius in the range:
        ///
        ///    -pi/2 < orientation <= pi/2
        ///
        /// ...giving the angle of the major axis of the ellipse with the y-axis
        /// (negative angles indicating an orientation along the top-left /
        /// bottom-right diagonal, positive angles indicating an orientation along the
        /// top-right / bottom-left diagonal, and zero indicating an orientation
        /// parallel with the y-axis).
        ///
        /// For PointerDeviceKind.stylus and PointerDeviceKind.invertedStylus events:
        ///
        /// The angle of the stylus, in radians in the range:
        ///
        ///    -pi < orientation <= pi
        ///
        /// ...giving the angle of the axis of the stylus projected onto the input
        /// surface, relative to the positive y-axis of that surface (thus 0.0
        /// indicates the stylus, if projected onto that surface, would go from the
        /// contact point vertically up in the positive y-axis direction, pi would
        /// indicate that the stylus would go down in the negative y-axis direction;
        /// pi/4 would indicate that the stylus goes up and to the right, -pi/2 would
        /// indicate that the stylus goes to the left, etc).
        public readonly double orientation;

        /// For PointerDeviceKind.stylus and PointerDeviceKind.invertedStylus events:
        ///
        /// The angle of the stylus, in radians in the range:
        ///
        ///    0 <= tilt <= pi/2
        ///
        /// ...giving the angle of the axis of the stylus, relative to the axis
        /// perpendicular to the input surface (thus 0.0 indicates the stylus is
        /// orthogonal to the plane of the input surface, while pi/2 indicates that
        /// the stylus is flat on that surface).
        public readonly double tilt;

        public String toString() => $"{nameof(PointerData)}(x: {physicalX}, y: {physicalY})";

        /// Returns a complete textual description of the information in this object.
        public String toStringFull()
        {
            return $"{nameof(PointerData)}(" +
                   $"timeStamp: {timeStamp}, " +
                   $"change: {change}, " +
                   $"kind: {kind}, " +
                   $"device: {device}, " +
                   $"physicalX: {physicalX}, " +
                   $"physicalY: {physicalY}, " +
                   $"buttons: {buttons}, " +
                   $"pressure: {pressure}, " +
                   $"pressureMin: {pressureMin}, " +
                   $"pressureMax: {pressureMax}, " +
                   $"distance: {distance}, " +
                   $"distanceMax: {distanceMax}, " +
                   $"radiusMajor: {radiusMajor}, " +
                   $"radiusMinor: {radiusMinor}, " +
                   $"radiusMin: {radiusMin}, " +
                   $"radiusMax: {radiusMax}, " +
                   $"orientation: {orientation}, " +
                   $"tilt: {tilt}" +
                    ")";
        }
    }

    /// A sequence of reports about the state of pointers.
    public class PointerDataPacket
    {
        /// Creates a packet of pointer data reports.
        public PointerDataPacket(List<PointerData> data = null)
        {
            if (data == null)
                data = new List<PointerData>();

            this.data = data;
        }

        /// Data about the individual pointers in this packet.
        ///
        /// This list might contain multiple pieces of data about the same pointer.
        public readonly List<PointerData> data;
    }
}
