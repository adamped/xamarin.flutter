using System;
using System.Collections.Generic;
using static FlutterBinding.UI.Lerp;

namespace FlutterBinding.UI
{

    /// Base class for [Size] and [Offset], which are both ways to describe
    /// a distance as a two-dimensional axis-aligned vector.
    public abstract class OffsetBase
    {
        /// Abstract const constructor. This constructor enables subclasses to provide
        /// const constructors so that they can be used in const expressions.
        ///
        /// The first argument sets the horizontal component, and the second the
        /// vertical component.
        public OffsetBase(double _dx, double _dy)
        {
            this._dx = _dx;
            this._dy = _dy;
        }

        protected readonly double _dx;
        protected readonly double _dy;

        /// Returns true if either component is [double.infinity], and false if both
        /// are finite (or negative infinity, or NaN).
        ///
        /// This is different than comparing for equality with an instance that has
        /// _both_ components set to [double.infinity].
        ///
        /// See also:
        ///
        ///  * [isFinite], which is true if both components are finite (and not NaN).
        public bool isInfinite => _dx >= double.PositiveInfinity || _dy >= double.PositiveInfinity;

        /// Whether both components are finite (neither infinite nor NaN).
        ///
        /// See also:
        ///
        ///  * [isInfinite], which returns true if either component is equal to
        ///    positive infinity.
        public bool isFinite => _dx.isFinite && _dy.isFinite;

        /// Less-than operator. Compares an [Offset] or [Size] to another [Offset] or
        /// [Size], and returns true if both the horizontal and vertical values of the
        /// left-hand-side operand are smaller than the horizontal and vertical values
        /// of the right-hand-side operand respectively. Returns false otherwise.
        ///
        /// This is a partial ordering. It is possible for two values to be neither
        /// less, nor greater than, nor equal to, another.
        public bool operator <(OffsetBase other) => _dx < other._dx && _dy < other._dy;

        /// Less-than-or-equal-to operator. Compares an [Offset] or [Size] to another
        /// [Offset] or [Size], and returns true if both the horizontal and vertical
        /// values of the left-hand-side operand are smaller than or equal to the
        /// horizontal and vertical values of the right-hand-side operand
        /// respectively. Returns false otherwise.
        ///
        /// This is a partial ordering. It is possible for two values to be neither
        /// less, nor greater than, nor equal to, another.
        public bool operator <=(OffsetBase other) => _dx <= other._dx && _dy <= other._dy;

        /// Greater-than operator. Compares an [Offset] or [Size] to another [Offset]
        /// or [Size], and returns true if both the horizontal and vertical values of
        /// the left-hand-side operand are bigger than the horizontal and vertical
        /// values of the right-hand-side operand respectively. Returns false
        /// otherwise.
        ///
        /// This is a partial ordering. It is possible for two values to be neither
        /// less, nor greater than, nor equal to, another.
        public bool operator >(OffsetBase other) => _dx > other._dx && _dy > other._dy;

        /// Greater-than-or-equal-to operator. Compares an [Offset] or [Size] to
        /// another [Offset] or [Size], and returns true if both the horizontal and
        /// vertical values of the left-hand-side operand are bigger than or equal to
        /// the horizontal and vertical values of the right-hand-side operand
        /// respectively. Returns false otherwise.
        ///
        /// This is a partial ordering. It is possible for two values to be neither
        /// less, nor greater than, nor equal to, another.
        public bool operator >=(OffsetBase other) => _dx > other._dx && _dy >= other._dy;

        /// Equality operator. Compares an [Offset] or [Size] to another [Offset] or
        /// [Size], and returns true if the horizontal and vertical values of the
        /// left-hand-side operand are equal to the horizontal and vertical values of
        /// the right-hand-side operand respectively. Returns false otherwise.

        public bool operator ==(dynamic other)
        {
            if (other is !OffsetBase)
                return false;
            final OffsetBase typedOther = other;
            return _dx == typedOther._dx &&
                   _dy == typedOther._dy;
        }


        public int hashCode => hashValues(_dx, _dy);

        public String toString() => $"{nameof(OffsetBase)}({_dx.toStringAsFixed(1)}, {_dy.toStringAsFixed(1)})";
    }

    /// An immutable 2D floating-point offset.
    ///
    /// Generally speaking, Offsets can be interpreted in two ways:
    ///
    /// 1. As representing a point in Cartesian space a specified distance from a
    ///    separately-maintained origin. For example, the top-left position of
    ///    children in the [RenderBox] protocol is typically represented as an
    ///    [Offset] from the top left of the parent box.
    ///
    /// 2. As a vector that can be applied to coordinates. For example, when
    ///    painting a [RenderObject], the parent is passed an [Offset] from the
    ///    screen's origin which it can add to the offsets of its children to find
    ///    the [Offset] from the screen's origin to each of the children.
    ///
    /// Because a particular [Offset] can be interpreted as one sense at one time
    /// then as the other sense at a later time, the same class is used for both
    /// senses.
    ///
    /// See also:
    ///
    ///  * [Size], which represents a vector describing the size of a rectangle.
    public class Offset : OffsetBase
    {
        /// Creates an offset. The first argument sets [dx], the horizontal component,
        /// and the second sets [dy], the vertical component.
        public Offset(double dx, double dy) : base(dx, dy) { }

        /// The x component of the offset.
        ///
        /// The y component is given by [dy].
        public double dx => _dx;

        /// The y component of the offset.
        ///
        /// The x component is given by [dx].
        public double dy => _dy;

        /// The magnitude of the offset.
        ///
        /// If you need this value to compare it to another [Offset]'s distance,
        /// consider using [distanceSquared] instead, since it is cheaper to compute.
        public double distance => Math.Sqrt(_dx * _dx + _dy * _dy);

        /// The square of the magnitude of the offset.
        ///
        /// This is cheaper than computing the [distance] itself.
        public double distanceSquared => _dx * _dx + _dy * _dy;

        /// The angle of this offset as radians clockwise from the positive x-axis, in
        /// the range -[pi] to [pi], assuming positive values of the x-axis go to the
        /// left and positive values of the y-axis go down.
        ///
        /// Zero means that [dy] is zero and [dx] is zero or positive.
        ///
        /// Values from zero to [pi]/2 indicate positive values of [dx] and [dy], the
        /// bottom-right quadrant.
        ///
        /// Values from [pi]/2 to [pi] indicate negative values of [dx] and positive
        /// values of [dy], the bottom-left quadrant.
        ///
        /// Values from zero to -[pi]/2 indicate positive values of [dx] and negative
        /// values of [dy], the top-right quadrant.
        ///
        /// Values from -[pi]/2 to -[pi] indicate negative values of [dx] and [dy],
        /// the top-left quadrant.
        ///
        /// When [dy] is zero and [dx] is negative, the [direction] is [pi].
        ///
        /// When [dx] is zero, [direction] is [pi]/2 if [dy] is positive and -[pi]/2
        /// if [dy] is negative.
        ///
        /// See also:
        ///
        ///  * [distance], to compute the magnitude of the vector.
        ///  * [Canvas.rotate], which uses the same convention for its angle.
        public double direction => Math.Atan2(dy, dx);

        /// An offset with zero magnitude.
        ///
        /// This can be used to represent the origin of a coordinate space.
        public static Offset zero = new Offset(0.0, 0.0);

        /// An offset with infinite x and y components.
        ///
        /// See also:
        ///
        ///  * [isInfinite], which checks whether either component is infinite.
        ///  * [isFinite], which checks whether both components are finite.
        // This is included for completeness, because [Size.infinite] exists.
        public static Offset infinite = new Offset(double.infinity, double.infinity);

        /// Returns a new offset with the x component scaled by `scaleX` and the y
        /// component scaled by `scaleY`.
        ///
        /// If the two scale arguments are the same, consider using the `*` operator
        /// instead:
        ///
        /// ```dart
        /// Offset a = const Offset(10.0, 10.0);
        /// Offset b = a * 2.0; // same as: a.scale(2.0, 2.0)
        /// ```
        ///
        /// If the two arguments are -1, consider using the unary `-` operator
        /// instead:
        ///
        /// ```dart
        /// Offset a = const Offset(10.0, 10.0);
        /// Offset b = -a; // same as: a.scale(-1.0, -1.0)
        /// ```
        public Offset scale(double scaleX, double scaleY) => new Offset(dx * scaleX, dy * scaleY);

        /// Returns a new offset with translateX added to the x component and
        /// translateY added to the y component.
        ///
        /// If the arguments come from another [Offset], consider using the `+` or `-`
        /// operators instead:
        ///
        /// ```dart
        /// Offset a = const Offset(10.0, 10.0);
        /// Offset b = const Offset(10.0, 10.0);
        /// Offset c = a + b; // same as: a.translate(b.dx, b.dy)
        /// Offset d = a - b; // same as: a.translate(-b.dx, -b.dy)
        /// ```
        public Offset translate(double translateX, double translateY) => new Offset(dx + translateX, dy + translateY);

        /// Unary negation operator.
        ///
        /// Returns an offset with the coordinates negated.
        ///
        /// If the [Offset] represents an arrow on a plane, this operator returns the
        /// same arrow but pointing in the reverse direction.
        public Offset operator -() => new Offset(-dx, -dy);

        /// Binary subtraction operator.
        ///
        /// Returns an offset whose [dx] value is the left-hand-side operand's [dx]
        /// minus the right-hand-side operand's [dx] and whose [dy] value is the
        /// left-hand-side operand's [dy] minus the right-hand-side operand's [dy].
        ///
        /// See also [translate].
        public Offset operator -(Offset other) => new Offset(dx - other.dx, dy - other.dy);

        /// Binary addition operator.
        ///
        /// Returns an offset whose [dx] value is the sum of the [dx] values of the
        /// two operands, and whose [dy] value is the sum of the [dy] values of the
        /// two operands.
        ///
        /// See also [translate].
        public Offset operator +(Offset other) => new Offset(dx + other.dx, dy + other.dy);

        /// Multiplication operator.
        ///
        /// Returns an offset whose coordinates are the coordinates of the
        /// left-hand-side operand (an Offset) multiplied by the scalar
        /// right-hand-side operand (a double).
        ///
        /// See also [scale].
        public Offset operator *(double operand) => new Offset(dx * operand, dy * operand);

        /// Division operator.
        ///
        /// Returns an offset whose coordinates are the coordinates of the
        /// left-hand-side operand (an Offset) divided by the scalar right-hand-side
        /// operand (a double).
        ///
        /// See also [scale].
        public Offset operator /(double operand) => new Offset(dx / operand, dy / operand);

        /// Integer (truncating) division operator.
        ///
        /// Returns an offset whose coordinates are the coordinates of the
        /// left-hand-side operand (an Offset) divided by the scalar right-hand-side
        /// operand (a double), rounded towards zero.
        public Offset operator ~/ (double operand) => new Offset((dx ~/ operand).toDouble(), (dy ~/ operand).toDouble());

        /// Modulo (remainder) operator.
        ///
        /// Returns an offset whose coordinates are the remainder of dividing the
        /// coordinates of the left-hand-side operand (an Offset) by the scalar
        /// right-hand-side operand (a double).
        public Offset operator %(double operand) => new Offset(dx % operand, dy % operand);

        /// Rectangle constructor operator.
        ///
        /// Combines an [Offset] and a [Size] to form a [Rect] whose top-left
        /// coordinate is the point given by adding this offset, the left-hand-side
        /// operand, to the origin, and whose size is the right-hand-side operand.
        ///
        /// ```dart
        /// Rect myRect = Offset.zero & const Size(100.0, 100.0);
        /// // same as: new Rect.fromLTWH(0.0, 0.0, 100.0, 100.0)
        /// ```
        public Rect operator &(Size other) => new Rect.fromLTWH(dx, dy, other.width, other.height);

        /// Linearly interpolate between two offsets.
        ///
        /// If either offset is null, this function interpolates from [Offset.zero].
        ///
        /// The `t` argument represents position on the timeline, with 0.0 meaning
        /// that the interpolation has not started, returning `a` (or something
        /// equivalent to `a`), 1.0 meaning that the interpolation has finished,
        /// returning `b` (or something equivalent to `b`), and values in between
        /// meaning that the interpolation is at the relevant point on the timeline
        /// between `a` and `b`. The interpolation can be extrapolated beyond 0.0 and
        /// 1.0, so negative values and values greater than 1.0 are valid (and can
        /// easily be generated by curves such as [Curves.elasticInOut]).
        ///
        /// Values for `t` are usually obtained from an [Animation<double>], such as
        /// an [AnimationController].
        public static Offset lerp(Offset a, Offset b, double t)
        {
            //assert(t != null);
            if (a == null && b == null)
                return null;
            if (a == null)
                return b * t;
            if (b == null)
                return a * (1.0 - t);
            return new Offset(lerpDouble(a.dx, b.dx, t), lerpDouble(a.dy, b.dy, t));
        }

        /// Compares two Offsets for equality.
        public bool operator ==(dynamic other)
        {
            if (other is !Offset)
                return false;
            Offset typedOther = other;
            return _dx == typedOther._dx &&
                   _dy == typedOther._dy;
        }

        public int hashCode => hashValues(_dx, _dy);

        public String toString() => $"Offset({dx.toStringAsFixed(1)}, {dy.toStringAsFixed(1)})";
    }

    /// Holds a 2D floating-point size.
    ///
    /// You can think of this as an [Offset] from the origin.
    public class Size : OffsetBase
    {
        /// Creates a [Size] with the given [width] and [height].
        public Size(double width, double height) : super(width, height) { }

        /// Creates an instance of [Size] that has the same values as another.
        // Used by the rendering library's _DebugSize hack.
        public Size.copy(Size source) : super(source.width, source.height) { }

        /// Creates a square [Size] whose [width] and [height] are the given dimension.
        ///
        /// See also:
        ///
        ///  * [new Size.fromRadius], which is more convenient when the available size
        ///    is the radius of a circle.
        public Size.square(double dimension) : super(dimension, dimension) { }

        /// Creates a [Size] with the given [width] and an infinite [height].
        public Size.fromWidth(double width) : super(width, double.infinity) { }

        /// Creates a [Size] with the given [height] and an infinite [width].
        public Size.fromHeight(double height) : super(double.infinity, height) { }

        /// Creates a square [Size] whose [width] and [height] are twice the given
        /// dimension.
        ///
        /// This is a square that contains a circle with the given radius.
        ///
        /// See also:
        ///
        ///  * [new Size.square], which creates a square with the given dimension.
        public Size.fromRadius(double radius) : super(radius* 2.0, radius* 2.0) { }

        /// The horizontal extent of this size.
        public double width => _dx;

        /// The vertical extent of this size.
        public double height => _dy;

        /// An empty size, one with a zero width and a zero height.
        public static Size zero = new Size(0.0, 0.0);

        /// A size whose [width] and [height] are infinite.
        ///
        /// See also:
        ///
        ///  * [isInfinite], which checks whether either dimension is infinite.
        ///  * [isFinite], which checks whether both dimensions are finite.
        public static Size infinite = new Size(double.PositiveInfinity, double.PositiveInfinity);

        /// Whether this size encloses a non-zero area.
        ///
        /// Negative areas are considered empty.
        public bool isEmpty => width <= 0.0 || height <= 0.0;

        /// Binary subtraction operator for [Size].
        ///
        /// Subtracting a [Size] from a [Size] returns the [Offset] that describes how
        /// much bigger the left-hand-side operand is than the right-hand-side
        /// operand. Adding that resulting [Offset] to the [Size] that was the
        /// right-hand-side operand would return a [Size] equal to the [Size] that was
        /// the left-hand-side operand. (i.e. if `sizeA - sizeB -> offsetA`, then
        /// `offsetA + sizeB -> sizeA`)
        ///
        /// Subtracting an [Offset] from a [Size] returns the [Size] that is smaller than
        /// the [Size] operand by the difference given by the [Offset] operand. In other
        /// words, the returned [Size] has a [width] consisting of the [width] of the
        /// left-hand-side operand minus the [Offset.dx] dimension of the
        /// right-hand-side operand, and a [height] consisting of the [height] of the
        /// left-hand-side operand minus the [Offset.dy] dimension of the
        /// right-hand-side operand.
        public OffsetBase operator -(OffsetBase other)
        {
            if (other is Size)
                return new Offset(width - other.width, height - other.height);
            if (other is Offset)
                return new Size(width - other.dx, height - other.dy);
            throw new ArgumentException(other);
        }

        /// Binary addition operator for adding an [Offset] to a [Size].
        ///
        /// Returns a [Size] whose [width] is the sum of the [width] of the
        /// left-hand-side operand, a [Size], and the [Offset.dx] dimension of the
        /// right-hand-side operand, an [Offset], and whose [height] is the sum of the
        /// [height] of the left-hand-side operand and the [Offset.dy] dimension of
        /// the right-hand-side operand.
        public Size operator +(Offset other) => new Size(width + other.dx, height + other.dy);

        /// Multiplication operator.
        ///
        /// Returns a [Size] whose dimensions are the dimensions of the left-hand-side
        /// operand (a [Size]) multiplied by the scalar right-hand-side operand (a
        /// [double]).
        public Size operator *(double operand) => new Size(width * operand, height * operand);

        /// Division operator.
        ///
        /// Returns a [Size] whose dimensions are the dimensions of the left-hand-side
        /// operand (a [Size]) divided by the scalar right-hand-side operand (a
        /// [double]).
        public Size operator /(double operand) => new Size(width / operand, height / operand);

        /// Integer (truncating) division operator.
        ///
        /// Returns a [Size] whose dimensions are the dimensions of the left-hand-side
        /// operand (a [Size]) divided by the scalar right-hand-side operand (a
        /// [double]), rounded towards zero.
        public Size operator ~/ (double operand) => new Size((width ~/ operand).toDouble(), (height ~/ operand).toDouble());

        /// Modulo (remainder) operator.
        ///
        /// Returns a [Size] whose dimensions are the remainder of dividing the
        /// left-hand-side operand (a [Size]) by the scalar right-hand-side operand (a
        /// [double]).
        public Size operator %(double operand) => new Size(width % operand, height % operand);

        /// The lesser of the magnitudes of the [width] and the [height].
        public double shortestSide => math.min(width.abs(), height.abs());

        /// The greater of the magnitudes of the [width] and the [height].
        public double longestSide => math.max(width.abs(), height.abs());

        // Convenience methods that do the equivalent of calling the similarly named
        // methods on a Rect constructed from the given origin and this size.

        /// The offset to the intersection of the top and left edges of the rectangle
        /// described by the given [Offset] (which is interpreted as the top-left corner)
        /// and this [Size].
        ///
        /// See also [Rect.topLeft].
        public Offset topLeft(Offset origin) => origin;

        /// The offset to the center of the top edge of the rectangle described by the
        /// given offset (which is interpreted as the top-left corner) and this size.
        ///
        /// See also [Rect.topCenter].
        public Offset topCenter(Offset origin) => new Offset(origin.dx + width / 2.0, origin.dy);

        /// The offset to the intersection of the top and right edges of the rectangle
        /// described by the given offset (which is interpreted as the top-left corner)
        /// and this size.
        ///
        /// See also [Rect.topRight].
        public Offset topRight(Offset origin) => new Offset(origin.dx + width, origin.dy);

        /// The offset to the center of the left edge of the rectangle described by the
        /// given offset (which is interpreted as the top-left corner) and this size.
        ///
        /// See also [Rect.centerLeft].
        public Offset centerLeft(Offset origin) => new Offset(origin.dx, origin.dy + height / 2.0);

        /// The offset to the point halfway between the left and right and the top and
        /// bottom edges of the rectangle described by the given offset (which is
        /// interpreted as the top-left corner) and this size.
        ///
        /// See also [Rect.center].
        public Offset center(Offset origin) => new Offset(origin.dx + width / 2.0, origin.dy + height / 2.0);

        /// The offset to the center of the right edge of the rectangle described by the
        /// given offset (which is interpreted as the top-left corner) and this size.
        ///
        /// See also [Rect.centerLeft].
        public Offset centerRight(Offset origin) => new Offset(origin.dx + width, origin.dy + height / 2.0);

        /// The offset to the intersection of the bottom and left edges of the
        /// rectangle described by the given offset (which is interpreted as the
        /// top-left corner) and this size.
        ///
        /// See also [Rect.bottomLeft].
        public Offset bottomLeft(Offset origin) => new Offset(origin.dx, origin.dy + height);

        /// The offset to the center of the bottom edge of the rectangle described by
        /// the given offset (which is interpreted as the top-left corner) and this
        /// size.
        ///
        /// See also [Rect.bottomLeft].
        public Offset bottomCenter(Offset origin) => new Offset(origin.dx + width / 2.0, origin.dy + height);

        /// The offset to the intersection of the bottom and right edges of the
        /// rectangle described by the given offset (which is interpreted as the
        /// top-left corner) and this size.
        ///
        /// See also [Rect.bottomRight].
        public Offset bottomRight(Offset origin) => new Offset(origin.dx + width, origin.dy + height);

        /// Whether the point specified by the given offset (which is assumed to be
        /// relative to the top left of the size) lies between the left and right and
        /// the top and bottom edges of a rectangle of this size.
        ///
        /// Rectangles include their top and left edges but exclude their bottom and
        /// right edges.
        public bool contains(Offset offset)
        {
            return offset.dx >= 0.0 && offset.dx < width && offset.dy >= 0.0 && offset.dy < height;
        }

        /// A [Size] with the [width] and [height] swapped.
        public Size flipped => new Size(height, width);

        /// Linearly interpolate between two sizes
        ///
        /// If either size is null, this function interpolates from [Size.zero].
        ///
        /// The `t` argument represents position on the timeline, with 0.0 meaning
        /// that the interpolation has not started, returning `a` (or something
        /// equivalent to `a`), 1.0 meaning that the interpolation has finished,
        /// returning `b` (or something equivalent to `b`), and values in between
        /// meaning that the interpolation is at the relevant point on the timeline
        /// between `a` and `b`. The interpolation can be extrapolated beyond 0.0 and
        /// 1.0, so negative values and values greater than 1.0 are valid (and can
        /// easily be generated by curves such as [Curves.elasticInOut]).
        ///
        /// Values for `t` are usually obtained from an [Animation<double>], such as
        /// an [AnimationController].
        public static Size lerp(Size a, Size b, double t)
        {
            assert(t != null);
            if (a == null && b == null)
                return null;
            if (a == null)
                return b * t;
            if (b == null)
                return a * (1.0 - t);
            return new Size(lerpDouble(a.width, b.width, t), lerpDouble(a.height, b.height, t));
        }

        /// Compares two Sizes for equality.
        // We don't compare the runtimeType because of _DebugSize in the framework.
        public bool operator ==(dynamic other)
        {
            if (other is !Size)
                return false;
            final Size typedOther = other;
            return _dx == typedOther._dx &&
                   _dy == typedOther._dy;
        }

        public int hashCode => hashValues(_dx, _dy);

        public String toString() => 'Size(${width?.toStringAsFixed(1)}, ${height?.toStringAsFixed(1)})';
    }

    /// An immutable, 2D, axis-aligned, floating-point rectangle whose coordinates
    /// are relative to a given origin.
    ///
    /// A Rect can be created with one its constructors or from an [Offset] and a
    /// [Size] using the `&` operator:
    ///
    /// ```dart
    /// Rect myRect = const Offset(1.0, 2.0) & const Size(3.0, 4.0);
    /// ```
    public class Rect
    {
        private Rect() { }

        public Rect(double one, double two, double three, double four)
        {
            _value[0] = one;
            _value[1] = two;
            _value[2] = three;
            _value[3] = four;
        }

        /// Construct a rectangle from its left, top, right, and bottom edges.
        public static Rect fromLTRB(double left, double top, double right, double bottom)
        {
            return new Rect(left, top, right, bottom);
        }

        /// Construct a rectangle from its left and top edges, its width, and its
        /// height.
        ///
        /// To construct a [Rect] from an [Offset] and a [Size], you can use the
        /// rectangle constructor operator `&`. See [Offset.&].
        public Rect fromLTWH(double left, double top, double width, double height)
        {
            _value
              ..[0] = left
              ..[1] = top
              ..[2] = left + width
              ..[3] = top + height;
        }

        /// Construct a rectangle that bounds the given circle.
        ///
        /// The `center` argument is assumed to be an offset from the origin.
        public Rect fromCircle(Offset center = null, double radius = 0.0)
        {
            _value
              ..[0] = center.dx - radius
              ..[1] = center.dy - radius
              ..[2] = center.dx + radius
              ..[3] = center.dy + radius;
        }

        /// Construct the smallest rectangle that encloses the given offsets, treating
        /// them as vectors from the origin.
        public Rect fromPoints(Offset a, Offset b)
        {
            _value
              ..[0] = math.min(a.dx, b.dx)
              ..[1] = math.min(a.dy, b.dy)
              ..[2] = math.max(a.dx, b.dx)
              ..[3] = math.max(a.dy, b.dy);
        }

        const int _kDataSize = 4;
        readonly List<double> _value = new List<double>(_kDataSize);

        /// The offset of the left edge of this rectangle from the x axis.
        public double left => _value[0];

        /// The offset of the top edge of this rectangle from the y axis.
        public double top => _value[1];

        /// The offset of the right edge of this rectangle from the x axis.
        public double right => _value[2];

        /// The offset of the bottom edge of this rectangle from the y axis.
        public double bottom => _value[3];

        /// The distance between the left and right edges of this rectangle.
        public double width => right - left;

        /// The distance between the top and bottom edges of this rectangle.
        public double height => bottom - top;

        /// The distance between the upper-left corner and the lower-right corner of
        /// this rectangle.
        public Size size => new Size(width, height);

        /// A rectangle with left, top, right, and bottom edges all at zero.
        public static readonly Rect zero = new Rect._();

        public const double _giantScalar = 1.0E+9; // matches kGiantRect from default_layer_builder.cc

        /// A rectangle that covers the entire coordinate space.
        ///
        /// This covers the space from -1e9,-1e9 to 1e9,1e9.
        /// This is the space over which graphics operations are valid.
        public static readonly Rect largest = Rect.fromLTRB(-_giantScalar, -_giantScalar, _giantScalar, _giantScalar);

        /// Whether any of the coordinates of this rectangle are equal to positive infinity.
        // included for consistency with Offset and Size
        public bool isInfinite => left >= double.PositiveInfinity
                                || top >= double.PositiveInfinity
                                || right >= double.PositiveInfinity
                                || bottom >= double.PositiveInfinity;


        /// Whether all coordinates of this rectangle are finite.
        public bool isFinite => left.isFinite && top.isFinite && right.isFinite && bottom.isFinite;

        /// Whether this rectangle encloses a non-zero area. Negative areas are
        /// considered empty.
        public bool isEmpty => left >= right || top >= bottom;

        /// Returns a new rectangle translated by the given offset.
        ///
        /// To translate a rectangle by separate x and y components rather than by an
        /// [Offset], consider [translate].
        public Rect shift(Offset offset)
        {
            return new Rect.fromLTRB(left + offset.dx, top + offset.dy, right + offset.dx, bottom + offset.dy);
        }

        /// Returns a new rectangle with translateX added to the x components and
        /// translateY added to the y components.
        ///
        /// To translate a rectangle by an [Offset] rather than by separate x and y
        /// components, consider [shift].
        public Rect translate(double translateX, double translateY)
        {
            return new Rect.fromLTRB(left + translateX, top + translateY, right + translateX, bottom + translateY);
        }

        /// Returns a new rectangle with edges moved outwards by the given delta.
        public Rect inflate(double delta)
        {
            return new Rect.fromLTRB(left - delta, top - delta, right + delta, bottom + delta);
        }

        /// Returns a new rectangle with edges moved inwards by the given delta.
        public Rect deflate(double delta) => inflate(-delta);

        /// Returns a new rectangle that is the intersection of the given
        /// rectangle and this rectangle. The two rectangles must overlap
        /// for this to be meaningful. If the two rectangles do not overlap,
        /// then the resulting Rect will have a negative width or height.
        public Rect intersect(Rect other)
        {
            return new Rect.fromLTRB(
              math.max(left, other.left),
              math.max(top, other.top),
              math.min(right, other.right),
              math.min(bottom, other.bottom)
            );
        }

        /// Returns a new rectangle which is the bounding box containing this
        /// rectangle and the given rectangle.
        public Rect expandToInclude(Rect other)
        {
            return new Rect.fromLTRB(
                math.min(left, other.left),
                math.min(top, other.top),
                math.max(right, other.right),
                math.max(bottom, other.bottom));
        }

        /// Whether `other` has a nonzero area of overlap with this rectangle.
        public bool overlaps(Rect other)
        {
            if (right <= other.left || other.right <= left)
                return false;
            if (bottom <= other.top || other.bottom <= top)
                return false;
            return true;
        }

        /// The lesser of the magnitudes of the [width] and the [height] of this
        /// rectangle.
        public double shortestSide => math.min(width.abs(), height.abs());

        /// The greater of the magnitudes of the [width] and the [height] of this
        /// rectangle.
        public double longestSide => math.max(width.abs(), height.abs());

        /// The offset to the intersection of the top and left edges of this rectangle.
        ///
        /// See also [Size.topLeft].
        public Offset topLeft => new Offset(left, top);

        /// The offset to the center of the top edge of this rectangle.
        ///
        /// See also [Size.topCenter].
        public Offset topCenter => new Offset(left + width / 2.0, top);

        /// The offset to the intersection of the top and right edges of this rectangle.
        ///
        /// See also [Size.topRight].
        public Offset topRight => new Offset(right, top);

        /// The offset to the center of the left edge of this rectangle.
        ///
        /// See also [Size.centerLeft].
        public Offset centerLeft => new Offset(left, top + height / 2.0);

        /// The offset to the point halfway between the left and right and the top and
        /// bottom edges of this rectangle.
        ///
        /// See also [Size.center].
        public Offset center => new Offset(left + width / 2.0, top + height / 2.0);

        /// The offset to the center of the right edge of this rectangle.
        ///
        /// See also [Size.centerLeft].
        public Offset centerRight => new Offset(right, top + height / 2.0);

        /// The offset to the intersection of the bottom and left edges of this rectangle.
        ///
        /// See also [Size.bottomLeft].
        public Offset bottomLeft => new Offset(left, bottom);

        /// The offset to the center of the bottom edge of this rectangle.
        ///
        /// See also [Size.bottomLeft].
        public Offset bottomCenter => new Offset(left + width / 2.0, bottom);

        /// The offset to the intersection of the bottom and right edges of this rectangle.
        ///
        /// See also [Size.bottomRight].
        public Offset bottomRight => new Offset(right, bottom);

        /// Whether the point specified by the given offset (which is assumed to be
        /// relative to the origin) lies between the left and right and the top and
        /// bottom edges of this rectangle.
        ///
        /// Rectangles include their top and left edges but exclude their bottom and
        /// right edges.
        public bool contains(Offset offset)
        {
            return offset.dx >= left && offset.dx < right && offset.dy >= top && offset.dy < bottom;
        }

        /// Linearly interpolate between two rectangles.
        ///
        /// If either rect is null, [Rect.zero] is used as a substitute.
        ///
        /// The `t` argument represents position on the timeline, with 0.0 meaning
        /// that the interpolation has not started, returning `a` (or something
        /// equivalent to `a`), 1.0 meaning that the interpolation has finished,
        /// returning `b` (or something equivalent to `b`), and values in between
        /// meaning that the interpolation is at the relevant point on the timeline
        /// between `a` and `b`. The interpolation can be extrapolated beyond 0.0 and
        /// 1.0, so negative values and values greater than 1.0 are valid (and can
        /// easily be generated by curves such as [Curves.elasticInOut]).
        ///
        /// Values for `t` are usually obtained from an [Animation<double>], such as
        /// an [AnimationController].
        public static Rect lerp(Rect a, Rect b, double t)
        {
            assert(t != null);
            if (a == null && b == null)
                return null;
            if (a == null)
                return new Rect.fromLTRB(b.left * t, b.top * t, b.right * t, b.bottom * t);
            if (b == null)
            {
                final double k = 1.0 - t;
                return new Rect.fromLTRB(a.left * k, a.top * k, a.right * k, a.bottom * k);
            }
            return new Rect.fromLTRB(
              lerpDouble(a.left, b.left, t),
              lerpDouble(a.top, b.top, t),
              lerpDouble(a.right, b.right, t),
              lerpDouble(a.bottom, b.bottom, t),




            );
        }

        public bool operator ==(dynamic other)
        {
            if (identical(this, other))
                return true;
            if (runtimeType != other.runtimeType)
                return false;
            final Rect typedOther = other;
            for (int i = 0; i < _kDataSize; i += 1)
            {
                if (_value[i] != typedOther._value[i])
                    return false;
            }
            return true;
        }

        public int hashCode => hashList(_value);

        String toString() => 'Rect.fromLTRB(${left.toStringAsFixed(1)}, ${top.toStringAsFixed(1)}, ${right.toStringAsFixed(1)}, ${bottom.toStringAsFixed(1)})';
    }

    /// A radius for either circular or elliptical shapes.
    public class Radius
    {
        /// Constructs a circular radius. [x] and [y] will have the same radius value.
        public const Radius.circular(double radius) : this.elliptical(radius, radius);

        /// Constructs an elliptical radius with the given radii.
        public const Radius.elliptical(this.x, this.y);

        /// The radius value on the horizontal axis.
        public readonly double x;

        /// The radius value on the vertical axis.
        public readonly double y;

        /// A radius with [x] and [y] values set to zero.
        ///
        /// You can use [Radius.zero] with [RRect] to have right-angle corners.
        public static const Radius zero = const Radius.circular(0.0);

        /// Unary negation operator.
        ///
        /// Returns a Radius with the distances negated.
        ///
        /// Radiuses with negative values aren't geometrically meaningful, but could
        /// occur as part of expressions. For example, negating a radius of one pixel
        /// and then adding the result to another radius is equivalent to subtracting
        /// a radius of one pixel from the other.
        public Radius operator -() => new Radius.elliptical(-x, -y);

        /// Binary subtraction operator.
        ///
        /// Returns a radius whose [x] value is the left-hand-side operand's [x]
        /// minus the right-hand-side operand's [x] and whose [y] value is the
        /// left-hand-side operand's [y] minus the right-hand-side operand's [y].
        public Radius operator -(Radius other) => new Radius.elliptical(x - other.x, y - other.y);

        /// Binary addition operator.
        ///
        /// Returns a radius whose [x] value is the sum of the [x] values of the
        /// two operands, and whose [y] value is the sum of the [y] values of the
        /// two operands.
        public Radius operator +(Radius other) => new Radius.elliptical(x + other.x, y + other.y);

        /// Multiplication operator.
        ///
        /// Returns a radius whose coordinates are the coordinates of the
        /// left-hand-side operand (a radius) multiplied by the scalar
        /// right-hand-side operand (a double).
        public Radius operator *(double operand) => new Radius.elliptical(x * operand, y * operand);

        /// Division operator.
        ///
        /// Returns a radius whose coordinates are the coordinates of the
        /// left-hand-side operand (a radius) divided by the scalar right-hand-side
        /// operand (a double).
        public Radius operator /(double operand) => new Radius.elliptical(x / operand, y / operand);

        /// Integer (truncating) division operator.
        ///
        /// Returns a radius whose coordinates are the coordinates of the
        /// left-hand-side operand (a radius) divided by the scalar right-hand-side
        /// operand (a double), rounded towards zero.
        public Radius operator ~/(double operand) => new Radius.elliptical((x ~/ operand).toDouble(), (y ~/ operand).toDouble());

  /// Modulo (remainder) operator.
  ///
  /// Returns a radius whose coordinates are the remainder of dividing the
  /// coordinates of the left-hand-side operand (a radius) by the scalar
  /// right-hand-side operand (a double).
  public Radius operator %(double operand) => new Radius.elliptical(x % operand, y % operand);

        /// Linearly interpolate between two radii.
        ///
        /// If either is null, this function substitutes [Radius.zero] instead.
        ///
        /// The `t` argument represents position on the timeline, with 0.0 meaning
        /// that the interpolation has not started, returning `a` (or something
        /// equivalent to `a`), 1.0 meaning that the interpolation has finished,
        /// returning `b` (or something equivalent to `b`), and values in between
        /// meaning that the interpolation is at the relevant point on the timeline
        /// between `a` and `b`. The interpolation can be extrapolated beyond 0.0 and
        /// 1.0, so negative values and values greater than 1.0 are valid (and can
        /// easily be generated by curves such as [Curves.elasticInOut]).
        ///
        /// Values for `t` are usually obtained from an [Animation<double>], such as
        /// an [AnimationController].
        public static Radius lerp(Radius a, Radius b, double t)
        {
            // assert(t != null);
            if (a == null && b == null)
                return null;
            if (a == null)
                return new Radius.elliptical(b.x * t, b.y * t);
            if (b == null)
            {
                final double k = 1.0 - t;
                return new Radius.elliptical(a.x * k, a.y * k);
            }
            return new Radius.elliptical(
              lerpDouble(a.x, b.x, t),
              lerpDouble(a.y, b.y, t),




            );
        }

        public bool operator ==(dynamic other)
        {
            if (identical(this, other))
                return true;
            if (runtimeType != other.runtimeType)
                return false;
            final Radius typedOther = other;
            return typedOther.x == x && typedOther.y == y;
        }

        public int hashCode => hashValues(x, y);

        public String toString()
        {
            return x == y ? 'Radius.circular(${x.toStringAsFixed(1)})' :
                            'Radius.elliptical(${x.toStringAsFixed(1)}, '
                          '${y.toStringAsFixed(1)})';
        }
    }

    /// An immutable rounded rectangle with the custom radii for all four corners.
    public class RRect
    {
        private RRect() { };

        /// Construct a rounded rectangle from its left, top, right, and bottom edges,
        /// and the same radii along its horizontal axis and its vertical axis.
        RRect.fromLTRBXY(double left, double top, double right, double bottom,
                         double radiusX, double radiusY) {
    _value
      ..[0] = left
      ..[1] = top
      ..[2] = right
      ..[3] = bottom
      ..[4] = radiusX
      ..[5] = radiusY
      ..[6] = radiusX
      ..[7] = radiusY
      ..[8] = radiusX
      ..[9] = radiusY
      ..[10] = radiusX
      ..[11] = radiusY;
  }

    /// Construct a rounded rectangle from its left, top, right, and bottom edges,
    /// and the same radius in each corner.
    RRect.fromLTRBR(double left, double top, double right, double bottom,
                    Radius radius) {
    _value
      ..[0] = left
      ..[1] = top
      ..[2] = right
      ..[3] = bottom
      ..[4] = radius.x
      ..[5] = radius.y
      ..[6] = radius.x
      ..[7] = radius.y
      ..[8] = radius.x
      ..[9] = radius.y
      ..[10] = radius.x
      ..[11] = radius.y;
  }

/// Construct a rounded rectangle from its bounding box and the same radii
/// along its horizontal axis and its vertical axis.
RRect.fromRectXY(Rect rect, double radiusX, double radiusY) {
    _value
      ..[0] = rect.left
      ..[1] = rect.top
      ..[2] = rect.right
      ..[3] = rect.bottom
      ..[4] = radiusX
      ..[5] = radiusY
      ..[6] = radiusX
      ..[7] = radiusY
      ..[8] = radiusX
      ..[9] = radiusY
      ..[10] = radiusX
      ..[11] = radiusY;
  }

  /// Construct a rounded rectangle from its bounding box and a radius that is
  /// the same in each corner.
  RRect.fromRectAndRadius(Rect rect, Radius radius) {
    _value
      ..[0] = rect.left
      ..[1] = rect.top
      ..[2] = rect.right
      ..[3] = rect.bottom
      ..[4] = radius.x
      ..[5] = radius.y
      ..[6] = radius.x
      ..[7] = radius.y
      ..[8] = radius.x
      ..[9] = radius.y
      ..[10] = radius.x
      ..[11] = radius.y;
  }

  /// Construct a rounded rectangle from its left, top, right, and bottom edges,
  /// and topLeft, topRight, bottomRight, and bottomLeft radii.
  ///
  /// The corner radii default to [Radius.zero], i.e. right-angled corners.
  RRect.fromLTRBAndCorners(
    double left,
    double top,
    double right,
    double bottom,
    Radius topLeft = Radius.zero,
    Radius topRight = Radius.zero,
    Radius bottomRight = Radius.zero,
    Radius bottomLeft = Radius.zero,
  ) {
    _value
      ..[0] = left
      ..[1] = top
      ..[2] = right
      ..[3] = bottom
      ..[4] = topLeft.x
      ..[5] = topLeft.y
      ..[6] = topRight.x
      ..[7] = topRight.y
      ..[8] = bottomRight.x
      ..[9] = bottomRight.y
      ..[10] = bottomLeft.x
      ..[11] = bottomLeft.y;
  }

  /// Construct a rounded rectangle from its bounding box and and topLeft,
  /// topRight, bottomRight, and bottomLeft radii.
  ///
  /// The corner radii default to [Radius.zero], i.e. right-angled corners
  RRect.fromRectAndCorners(
    Rect rect,
      Radius topLeft = Radius.zero,
      Radius topRight = Radius.zero,
      Radius bottomRight = Radius.zero,
      Radius bottomLeft = Radius.zero
  ) {
    _value
      ..[0] = rect.left
      ..[1] = rect.top
      ..[2] = rect.right
      ..[3] = rect.bottom
      ..[4] = topLeft.x
      ..[5] = topLeft.y
      ..[6] = topRight.x
      ..[7] = topRight.y
      ..[8] = bottomRight.x
      ..[9] = bottomRight.y
      ..[10] = bottomLeft.x
      ..[11] = bottomLeft.y;
  }

  RRect._fromList(List<double> list) {
    for (int i = 0; i<_kDataSize; i += 1)
      _value[i] = list[i];
  }

const int _kDataSize = 12;
readonly List<float> _value = new List<float>(_kDataSize);
RRect _scaled; // same RRect with scaled radii per side

/// The offset of the left edge of this rectangle from the x axis.
public double left => _value[0];

/// The offset of the top edge of this rectangle from the y axis.
public double top => _value[1];

/// The offset of the right edge of this rectangle from the x axis.
public double right => _value[2];

/// The offset of the bottom edge of this rectangle from the y axis.
public double bottom => _value[3];

/// The top-left horizontal radius.
public double tlRadiusX => _value[4];

/// The top-left vertical radius.
public double tlRadiusY => _value[5];

/// The top-left [Radius].
public Radius tlRadius => new Radius.elliptical(_value[4], _value[5]);

/// The top-right horizontal radius.
public double trRadiusX => _value[6];

/// The top-right vertical radius.
public double trRadiusY => _value[7];

/// The top-right [Radius].
public Radius trRadius => new Radius.elliptical(_value[6], _value[7]);

/// The bottom-right horizontal radius.
public double brRadiusX => _value[8];

/// The bottom-right vertical radius.
public double brRadiusY => _value[9];

/// The bottom-right [Radius].
public Radius brRadius => new Radius.elliptical(_value[8], _value[9]);

/// The bottom-left horizontal radius.
public double blRadiusX => _value[10];

/// The bottom-left vertical radius.
public double blRadiusY => _value[11];

/// The bottom-left [Radius].
public Radius blRadius => new Radius.elliptical(_value[10], _value[11]);

/// A rounded rectangle with all the values set to zero.
public static readonly RRect zero = new RRect._();

/// Returns a new [RRect] translated by the given offset.
public RRect shift(Offset offset)
{
    return new RRect.fromLTRBAndCorners(
      _value[0] + offset.dx,
      _value[1] + offset.dy,
      _value[2] + offset.dx,
      _value[3] + offset.dy,
      topLeft: new Radius.elliptical(
        _value[4],
        _value[5]
      ),
      topRight: new Radius.elliptical(
        _value[6],
        _value[7]
      ),
      bottomRight: new Radius.elliptical(
        _value[8],
        _value[9]
      ),
      bottomLeft: new Radius.elliptical(
        _value[10],
        _value[11]
      )
    );
}

/// Returns a new [RRect] with edges and radii moved outwards by the given
/// delta.
public RRect inflate(double delta)
{
    return new RRect.fromLTRBAndCorners(
      _value[0] - delta,
      _value[1] - delta,
      _value[2] + delta,
      _value[3] + delta,
      topLeft: new Radius.elliptical(
        _value[4] + delta,
        _value[5] + delta
      ),
      topRight: new Radius.elliptical(
        _value[6] + delta,
        _value[7] + delta
      ),
      bottomRight: new Radius.elliptical(
        _value[8] + delta,
        _value[9] + delta
      ),
      bottomLeft: new Radius.elliptical(
        _value[10] + delta,
        _value[11] + delta
      )
    );
}

/// Returns a new [RRect] with edges and radii moved inwards by the given delta.
public RRect deflate(double delta) => inflate(-delta);

/// The distance between the left and right edges of this rectangle.
public double width => right - left;

/// The distance between the top and bottom edges of this rectangle.
public double height => bottom - top;

/// The bounding box of this rounded rectangle (the rectangle with no rounded corners).
public Rect outerRect => new Rect.fromLTRB(left, top, right, bottom);

/// The non-rounded rectangle that is constrained by the smaller of the two
/// diagonals, with each diagonal traveling through the middle of the curve
/// corners. The middle of a corner is the intersection of the curve with its
/// respective quadrant bisector.
public Rect safeInnerRect
{
    get
    {
        const double kInsetFactor = 0.29289321881; // 1-cos(pi/4)

        final double leftRadius = math.max(blRadiusX, tlRadiusX);
        final double topRadius = math.max(tlRadiusY, trRadiusY);
        final double rightRadius = math.max(trRadiusX, brRadiusX);
        final double bottomRadius = math.max(brRadiusY, blRadiusY);

        return new Rect.fromLTRB(
          left + leftRadius * kInsetFactor,
          top + topRadius * kInsetFactor,
          right - rightRadius * kInsetFactor,
          bottom - bottomRadius * kInsetFactor
        );
    }
}
/// The rectangle that would be formed using the axis-aligned intersection of
/// the sides of the rectangle, i.e., the rectangle formed from the
/// inner-most centers of the ellipses that form the corners. This is the
/// intersection of the [wideMiddleRect] and the [tallMiddleRect]. If any of
/// the intersections are void, the resulting [Rect] will have negative width
/// or height.
public Rect middleRect
{
    get
    {
        final double leftRadius = math.max(blRadiusX, tlRadiusX);
        final double topRadius = math.max(tlRadiusY, trRadiusY);
        final double rightRadius = math.max(trRadiusX, brRadiusX);
        final double bottomRadius = math.max(brRadiusY, blRadiusY);
        return new Rect.fromLTRB(
          left + leftRadius,
          top + topRadius,
          right - rightRadius,
          bottom - bottomRadius
        );
    }
}

/// The biggest rectangle that is entirely inside the rounded rectangle and
/// has the full width of the rounded rectangle. If the rounded rectangle does
/// not have an axis-aligned intersection of its left and right side, the
/// resulting [Rect] will have negative width or height.
public Rect wideMiddleRect
{
    get
    {
        final double topRadius = math.max(tlRadiusY, trRadiusY);
        final double bottomRadius = math.max(brRadiusY, blRadiusY);
        return new Rect.fromLTRB(
          left,
          top + topRadius,
          right,
          bottom - bottomRadius
        );
    }
}

/// The biggest rectangle that is entirely inside the rounded rectangle and
/// has the full height of the rounded rectangle. If the rounded rectangle
/// does not have an axis-aligned intersection of its top and bottom side, the
/// resulting [Rect] will have negative width or height.
public Rect tallMiddleRect
{
    get
    {
        final double leftRadius = math.max(blRadiusX, tlRadiusX);
        final double rightRadius = math.max(trRadiusX, brRadiusX);
        return new Rect.fromLTRB(
          left + leftRadius,
          top,
          right - rightRadius,
          bottom
        );
    }
}

/// Whether this rounded rectangle encloses a non-zero area.
/// Negative areas are considered empty.
public bool isEmpty => left >= right || top >= bottom;

/// Whether all coordinates of this rounded rectangle are finite.
public bool isFinite => left.isFinite && top.isFinite && right.isFinite && bottom.isFinite;

/// Whether this rounded rectangle is a simple rectangle with zero
/// corner radii.
public bool isRect
{
    return (tlRadiusX == 0.0 || tlRadiusY == 0.0) &&
           (trRadiusX == 0.0 || trRadiusY == 0.0) &&
           (blRadiusX == 0.0 || blRadiusY == 0.0) &&
           (brRadiusX == 0.0 || brRadiusY == 0.0);
}

/// Whether this rounded rectangle has a side with no straight section.
public bool isStadium
{
    return tlRadius == trRadius
        && trRadius == brRadius
        && brRadius == blRadius
        && (width <= 2.0 * tlRadiusX || height <= 2.0 * tlRadiusY);
}

/// Whether this rounded rectangle has no side with a straight section.
public bool isEllipse
{
    return tlRadius == trRadius
        && trRadius == brRadius
        && brRadius == blRadius
        && width <= 2.0 * tlRadiusX
        && height <= 2.0 * tlRadiusY;
}

/// Whether this rounded rectangle would draw as a circle.
public bool isCircle => width == height && isEllipse;

/// The lesser of the magnitudes of the [width] and the [height] of this
/// rounded rectangle.
public double shortestSide => math.min(width.abs(), height.abs());

/// The greater of the magnitudes of the [width] and the [height] of this
/// rounded rectangle.
public double longestSide => math.max(width.abs(), height.abs());


/// The offset to the point halfway between the left and right and the top and
/// bottom edges of this rectangle.
public Offset center => new Offset(left + width / 2.0, top + height / 2.0);

// Returns the minimum between min and scale to which radius1 and radius2
// should be scaled with in order not to exceed the limit.
double _getMin(double min, double radius1, double radius2, double limit)
{
    final double sum = radius1 + radius2;
    if (sum > limit && sum != 0.0)
        return math.min(min, limit / sum);
    return min;
}

// Scales all radii so that on each side their sum will not pass the size of
// the width/height.
//
// Inspired from:
//   https://github.com/google/skia/blob/master/src/core/SkRRect.cpp#L164
void _scaleRadii()
{
    if (_scaled == null)
    {
        double scale = 1.0;
        final List<double> scaled = new List<double>.from(_value);

        scale = _getMin(scale, scaled[11], scaled[5], height);
        scale = _getMin(scale, scaled[4], scaled[6], width);
        scale = _getMin(scale, scaled[7], scaled[9], height);
        scale = _getMin(scale, scaled[8], scaled[10], width);

        if (scale < 1.0)
        {
            for (int i = 4; i < _kDataSize; i += 1)
                scaled[i] *= scale;
        }

        _scaled = new RRect._fromList(scaled);
    }
}

/// Whether the point specified by the given offset (which is assumed to be
/// relative to the origin) lies inside the rounded rectangle.
///
/// This method may allocate (and cache) a copy of the object with normalized
/// radii the first time it is called on a particular [RRect] instance. When
/// using this method, prefer to reuse existing [RRect]s rather than
/// recreating the object each time.
public bool contains(Offset point)
{
    if (point.dx < left || point.dx >= right || point.dy < top || point.dy >= bottom)
        return false; // outside bounding box

    _scaleRadii();

    double x;
    double y;
    double radiusX;
    double radiusY;
    // check whether point is in one of the rounded corner areas
    // x, y -> translate to ellipse center
    if (point.dx < left + _scaled.tlRadiusX &&
        point.dy < top + _scaled.tlRadiusY)
    {
        x = point.dx - left - _scaled.tlRadiusX;
        y = point.dy - top - _scaled.tlRadiusY;
        radiusX = _scaled.tlRadiusX;
        radiusY = _scaled.tlRadiusY;
    }
    else if (point.dx > right - _scaled.trRadiusX &&
             point.dy < top + _scaled.trRadiusY)
    {
        x = point.dx - right + _scaled.trRadiusX;
        y = point.dy - top - _scaled.trRadiusY;
        radiusX = _scaled.trRadiusX;
        radiusY = _scaled.trRadiusY;
    }
    else if (point.dx > right - _scaled.brRadiusX &&
             point.dy > bottom - _scaled.brRadiusY)
    {
        x = point.dx - right + _scaled.brRadiusX;
        y = point.dy - bottom + _scaled.brRadiusY;
        radiusX = _scaled.brRadiusX;
        radiusY = _scaled.brRadiusY;
    }
    else if (point.dx < left + _scaled.blRadiusX &&
             point.dy > bottom - _scaled.blRadiusY)
    {
        x = point.dx - left - _scaled.blRadiusX;
        y = point.dy - bottom + _scaled.blRadiusY;
        radiusX = _scaled.blRadiusX;
        radiusY = _scaled.blRadiusY;
    }
    else
    {
        return true; // inside and not within the rounded corner area
    }

    x = x / radiusX;
    y = y / radiusY;
    // check if the point is outside the unit circle
    if (x * x + y * y > 1.0)
        return false;
    return true;
}

/// Linearly interpolate between two rounded rectangles.
///
/// If either is null, this function substitutes [RRect.zero] instead.
///
/// The `t` argument represents position on the timeline, with 0.0 meaning
/// that the interpolation has not started, returning `a` (or something
/// equivalent to `a`), 1.0 meaning that the interpolation has finished,
/// returning `b` (or something equivalent to `b`), and values in between
/// meaning that the interpolation is at the relevant point on the timeline
/// between `a` and `b`. The interpolation can be extrapolated beyond 0.0 and
/// 1.0, so negative values and values greater than 1.0 are valid (and can
/// easily be generated by curves such as [Curves.elasticInOut]).
///
/// Values for `t` are usually obtained from an [Animation<double>], such as
/// an [AnimationController].
public static RRect lerp(RRect a, RRect b, double t)
{
    assert(t != null);
    if (a == null && b == null)
        return null;
    if (a == null)
    {
        return new RRect._fromList(< double >[
          b.left * t,
          b.top * t,
          b.right * t,
          b.bottom * t,
          b.tlRadiusX * t,
          b.tlRadiusY * t,
          b.trRadiusX * t,
          b.trRadiusY * t,
          b.brRadiusX * t,
          b.brRadiusY * t,
          b.blRadiusX * t,
          b.blRadiusY * t,


        ]);
    }
    if (b == null)
    {
        final double k = 1.0 - t;
        return new RRect._fromList(< double >[
          a.left * k,
          a.top * k,
          a.right * k,
          a.bottom * k,
          a.tlRadiusX * k,
          a.tlRadiusY * k,
          a.trRadiusX * k,
          a.trRadiusY * k,
          a.brRadiusX * k,
          a.brRadiusY * k,
          a.blRadiusX * k,
          a.blRadiusY * k,


        ]);
    }
    return new RRect._fromList(< double >[
      lerpDouble(a.left, b.left, t),
      lerpDouble(a.top, b.top, t),
      lerpDouble(a.right, b.right, t),
      lerpDouble(a.bottom, b.bottom, t),
      lerpDouble(a.tlRadiusX, b.tlRadiusX, t),
      lerpDouble(a.tlRadiusY, b.tlRadiusY, t),
      lerpDouble(a.trRadiusX, b.trRadiusX, t),
      lerpDouble(a.trRadiusY, b.trRadiusY, t),
      lerpDouble(a.brRadiusX, b.brRadiusX, t),
      lerpDouble(a.brRadiusY, b.brRadiusY, t),
      lerpDouble(a.blRadiusX, b.blRadiusX, t),
      lerpDouble(a.blRadiusY, b.blRadiusY, t),
    ]);
}

public bool operator ==(dynamic other)
{
    if (identical(this, other))
        return true;
    if (runtimeType != other.runtimeType)
        return false;
    final RRect typedOther = other;
    for (int i = 0; i < _kDataSize; i += 1)
    {
        if (_value[i] != typedOther._value[i])
            return false;
    }
    return true;
}

public int hashCode => hashList(_value);

public String toString()
{
    final String rect = '${left.toStringAsFixed(1)}, '
                        '${top.toStringAsFixed(1)}, '
                        '${right.toStringAsFixed(1)}, '
                        '${bottom.toStringAsFixed(1)}';
    if (tlRadius == trRadius &&
        trRadius == brRadius &&
        brRadius == blRadius)
    {
        if (tlRadius.x == tlRadius.y)
            return 'RRect.fromLTRBR($rect, ${tlRadius.x.toStringAsFixed(1)})';
        return 'RRect.fromLTRBXY($rect, ${tlRadius.x.toStringAsFixed(1)}, ${tlRadius.y.toStringAsFixed(1)})';
    }
    return 'RRect.fromLTRBAndCorners('
             '$rect, '
             'topLeft: $tlRadius, '
             'topRight: $trRadius, '
             'bottomRight: $brRadius, '
             'bottomLeft: $blRadius'
           ')';
}
}

/// A transform consisting of a translation, a rotation, and a uniform scale.
///
/// Used by [Canvas.drawAtlas]. This is a more efficient way to represent these
/// simple transformations than a full matrix.
// Modeled after Skia's SkRSXform.
public class RSTransform
{
    /// Creates an RSTransform.
    ///
    /// An [RSTransform] expresses the combination of a translation, a rotation
    /// around a particular point, and a scale factor.
    ///
    /// The first argument, `scos`, is the cosine of the rotation, multiplied by
    /// the scale factor.
    ///
    /// The second argument, `ssin`, is the sine of the rotation, multiplied by
    /// that same scale factor.
    ///
    /// The third argument is the x coordinate of the translation, minus the
    /// `scos` argument multiplied by the x-coordinate of the rotation point, plus
    /// the `ssin` argument multiplied by the y-coordinate of the rotation point.
    ///
    /// The fourth argument is the y coordinate of the translation, minus the `ssin`
    /// argument multiplied by the x-coordinate of the rotation point, minus the
    /// `scos` argument multiplied by the y-coordinate of the rotation point.
    ///
    /// The [new RSTransform.fromComponents] method may be a simpler way to
    /// construct these values. However, if there is a way to factor out the
    /// computations of the sine and cosine of the rotation so that they can be
    /// reused over multiple calls to this constructor, it may be more efficient
    /// to directly use this constructor instead.
    RSTransform(double scos, double ssin, double tx, double ty)
    {
        _value
          ..[0] = scos
          ..[1] = ssin
          ..[2] = tx
          ..[3] = ty;
    }

    /// Creates an RSTransform from its individual components.
    ///
    /// The `rotation` parameter gives the rotation in radians.
    ///
    /// The `scale` parameter describes the uniform scale factor.
    ///
    /// The `anchorX` and `anchorY` parameters give the coordinate of the point
    /// around which to rotate.
    ///
    /// The `translateX` and `translateY` parameters give the coordinate of the
    /// offset by which to translate.
    ///
    /// This constructor computes the arguments of the [new RSTransform]
    /// constructor and then defers to that constructor to actually create the
    /// object. If many [RSTransform] objects are being created and there is a way
    /// to factor out the computations of the sine and cosine of the rotation
    /// (which are computed each time this constructor is called) and reuse them
    /// over multiple [RSTransform] objects, it may be more efficient to directly
    /// use the more direct [new RSTransform] constructor instead.
    factory RSTransform.fromComponents(double rotation = 0.0,
      double scale = 0.0,
      double anchorX = 0.0,
      double anchorY = 0.0,
      double translateX = 0.0,
      double translateY = 0.0
    )
    {
        double scos = math.cos(rotation) * scale;
        double ssin = math.sin(rotation) * scale;
        double tx = translateX + -scos * anchorX + ssin * anchorY;
        double ty = translateY + -ssin * anchorX - scos * anchorY;
        return new RSTransform(scos, ssin, tx, ty);
    }

    readonly List<float> _value = new Float32List(4);

    /// The cosine of the rotation multiplied by the scale factor.
    public double scos => _value[0];

    /// The sine of the rotation multiplied by that same scale factor.
    public double ssin => _value[1];

    /// The x coordinate of the translation, minus [scos] multiplied by the
    /// x-coordinate of the rotation point, plus [ssin] multiplied by the
    /// y-coordinate of the rotation point.
    public double tx => _value[2];

    /// The y coordinate of the translation, minus [ssin] multiplied by the
    /// x-coordinate of the rotation point, minus [scos] multiplied by the
    /// y-coordinate of the rotation point.
    public double ty => _value[3];
}
}
