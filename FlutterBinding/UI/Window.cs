using FlutterBinding.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using static FlutterBinding.Mapping.Types;

namespace FlutterBinding.UI
{
    /// Signature of callbacks that have no arguments and return no data.
    public delegate void VoidCallback();

    /// Signature for [Window.onBeginFrame].
    public delegate void FrameCallback(Duration duration);

    /// Signature for [Window.onPointerDataPacket].
    public delegate void PointerDataPacketCallback(PointerDataPacket packet);

    /// Signature for [Window.onSemanticsAction].
    public delegate void SemanticsActionCallback(int id, SemanticsAction action, ByteData args);

    /// Signature for responses to platform messages.
    ///
    /// Used as a parameter to [Window.sendPlatformMessage] and
    /// [Window.onPlatformMessage].
    public delegate void PlatformMessageResponseCallback(ByteData data);

    /// Signature for [Window.onPlatformMessage].
    public delegate void PlatformMessageCallback(String name, ByteData data, PlatformMessageResponseCallback callback);

    /// States that an application can be in.
    ///
    /// The values below describe notifications from the operating system.
    /// Applications should not expect to always receive all possible
    /// notifications. For example, if the users pulls out the battery from the
    /// device, no notification will be sent before the application is suddenly
    /// terminated, along with the rest of the operating system.
    ///
    /// See also:
    ///
    ///  * [WidgetsBindingObserver], for a mechanism to observe the lifecycle state
    ///    from the widgets layer.
    public enum AppLifecycleState
    {
        /// The application is visible and responding to user input.
        resumed,

        /// The application is in an inactive state and is not receiving user input.
        ///
        /// On iOS, this state corresponds to an app or the Flutter host view running
        /// in the foreground inactive state. Apps transition to this state when in
        /// a phone call, responding to a TouchID request, when entering the app
        /// switcher or the control center, or when the UIViewController hosting the
        /// Flutter app is transitioning.
        ///
        /// On Android, this corresponds to an app or the Flutter host view running
        /// in the foreground inactive state.  Apps transition to this state when
        /// another activity is focused, such as a split-screen app, a phone call,
        /// a picture-in-picture app, a system dialog, or another window.
        ///
        /// Apps in this state should assume that they may be [paused] at any time.
        inactive,

        /// The application is not currently visible to the user, not responding to
        /// user input, and running in the background.
        ///
        /// When the application is in this state, the engine will not call the
        /// [Window.onBeginFrame] and [Window.onDrawFrame] callbacks.
        ///
        /// Android apps in this state should assume that they may enter the
        /// [suspending] state at any time.
        paused,

        /// The application will be suspended momentarily.
        ///
        /// When the application is in this state, the engine will not call the
        /// [Window.onBeginFrame] and [Window.onDrawFrame] callbacks.
        ///
        /// On iOS, this state is currently unused.
        suspending,
    }

    /// A representation of distances for each of the four edges of a rectangle,
    /// used to encode the view insets and padding that applications should place
    /// around their user interface, as exposed by [Window.viewInsets] and
    /// [Window.padding]. View insets and padding are preferably read via
    /// [MediaQuery.of].
    ///
    /// For a generic class that represents distances around a rectangle, see the
    /// [EdgeInsets] class.
    ///
    /// See also:
    ///
    ///  * [WidgetsBindingObserver], for a widgets layer mechanism to receive
    ///    notifications when the padding changes.
    ///  * [MediaQuery.of], for the preferred mechanism for accessing these values.
    ///  * [Scaffold], which automatically applies the padding in material design
    ///    applications.
    public class WindowPadding
    {
        public WindowPadding(double left, double top, double right, double bottom)
        {
            this.left = left;
            this.top = top;
            this.right = right;
            this.bottom = bottom;
        }

        /// The distance from the left edge to the first unpadded pixel, in physical pixels.
        public readonly double left;

        /// The distance from the top edge to the first unpadded pixel, in physical pixels.
        public readonly double top;

        /// The distance from the right edge to the first unpadded pixel, in physical pixels.
        public readonly double right;

        /// The distance from the bottom edge to the first unpadded pixel, in physical pixels.
        public readonly double bottom;

        /// A window padding that has zeros for each edge.
        public static WindowPadding zero = new WindowPadding(left: 0.0, top: 0.0, right: 0.0, bottom: 0.0);

        public String toString()
        {
            return $"{nameof(WindowPadding)}(left: {left}, top: {top}, right: {right}, bottom: {bottom})";
        }
    }

    /// An identifier used to select a user's language and formatting preferences,
    /// consisting of a language and a country. This is a subset of locale
    /// identifiers as defined by BCP 47.
    ///
    /// Locales are canonicalized according to the "preferred value" entries in the
    /// [IANA Language Subtag
    /// Registry](https://www.iana.org/assignments/language-subtag-registry/language-subtag-registry).
    /// For example, `const Locale('he')` and `const Locale('iw')` are equal and
    /// both have the [languageCode] `he`, because `iw` is a deprecated language
    /// subtag that was replaced by the subtag `he`.
    ///
    /// See also:
    ///
    ///  * [Window.locale], which specifies the system's currently selected
    ///    [Locale].
    public class Locale
    {
        /// Creates a new Locale object. The first argument is the
        /// primary language subtag, the second is the region subtag.
        ///
        /// For example:
        ///
        /// ```dart
        /// const Locale swissFrench = const Locale('fr', 'CH');
        /// const Locale canadianFrench = const Locale('fr', 'CA');
        /// ```
        ///
        /// The primary language subtag must not be null. The region subtag is
        /// optional.
        ///
        /// The values are _case sensitive_, and should match the case of the relevant
        /// subtags in the [IANA Language Subtag
        /// Registry](https://www.iana.org/assignments/language-subtag-registry/language-subtag-registry).
        /// Typically this means the primary language subtag should be lowercase and
        /// the region subtag should be uppercase.
        public Locale(string _languageCode, string _countryCode = "")
        {
            this._languageCode = _languageCode;
            this._countryCode = _countryCode;
            // assert(_languageCode != null);
        }

        /// Empty locale constant. This is an invalid locale.
        public static Locale none = new Locale("", "");

        /// The primary language subtag for the locale.
        ///
        /// This must not be null.
        ///
        /// This is expected to be string registered in the [IANA Language Subtag
        /// Registry](https://www.iana.org/assignments/language-subtag-registry/language-subtag-registry)
        /// with the type "language". The string specified must match the case of the
        /// string in the registry.
        ///
        /// Language subtags that are deprecated in the registry and have a preferred
        /// code are changed to their preferred code. For example, `const
        /// Locale('he')` and `const Locale('iw')` are equal, and both have the
        /// [languageCode] `he`, because `iw` is a deprecated language subtag that was
        /// replaced by the subtag `he`.
        public String languageCode => _canonicalizeLanguageCode(_languageCode);
        readonly String _languageCode;

        static String _canonicalizeLanguageCode(String languageCode)
        {
            // This switch statement is generated by //flutter/tools/gen_locale.dart
            // Mappings generated for language subtag registry as of 2018-08-08.
            switch (languageCode)
            {
                case "in": return "id"; // Indonesian; deprecated 1989-01-01
                case "iw": return "he"; // Hebrew; deprecated 1989-01-01
                case "ji": return "yi"; // Yiddish; deprecated 1989-01-01
                case "jw": return "jv"; // Javanese; deprecated 2001-08-13
                case "mo": return "ro"; // Moldavian, Moldovan; deprecated 2008-11-22
                case "aam": return "aas"; // Aramanik; deprecated 2015-02-12
                case "adp": return "dz"; // Adap; deprecated 2015-02-12
                case "aue": return "ktz"; // =/Kx"au//"ein; deprecated 2015-02-12
                case "ayx": return "nun"; // Ayi (China); deprecated 2011-08-16
                case "bgm": return "bcg"; // Baga Mboteni; deprecated 2016-05-30
                case "bjd": return "drl"; // Bandjigali; deprecated 2012-08-12
                case "ccq": return "rki"; // Chaungtha; deprecated 2012-08-12
                case "cjr": return "mom"; // Chorotega; deprecated 2010-03-11
                case "cka": return "cmr"; // Khumi Awa Chin; deprecated 2012-08-12
                case "cmk": return "xch"; // Chimakum; deprecated 2010-03-11
                case "coy": return "pij"; // Coyaima; deprecated 2016-05-30
                case "cqu": return "quh"; // Chilean Quechua; deprecated 2016-05-30
                case "drh": return "khk"; // Darkhat; deprecated 2010-03-11
                case "drw": return "prs"; // Darwazi; deprecated 2010-03-11
                case "gav": return "dev"; // Gabutamon; deprecated 2010-03-11
                case "gfx": return "vaj"; // Mangetti Dune !Xung; deprecated 2015-02-12
                case "ggn": return "gvr"; // Eastern Gurung; deprecated 2016-05-30
                case "gti": return "nyc"; // Gbati-ri; deprecated 2015-02-12
                case "guv": return "duz"; // Gey; deprecated 2016-05-30
                case "hrr": return "jal"; // Horuru; deprecated 2012-08-12
                case "ibi": return "opa"; // Ibilo; deprecated 2012-08-12
                case "ilw": return "gal"; // Talur; deprecated 2013-09-10
                case "jeg": return "oyb"; // Jeng; deprecated 2017-02-23
                case "kgc": return "tdf"; // Kasseng; deprecated 2016-05-30
                case "kgh": return "kml"; // Upper Tanudan Kalinga; deprecated 2012-08-12
                case "koj": return "kwv"; // Sara Dunjo; deprecated 2015-02-12
                case "krm": return "bmf"; // Krim; deprecated 2017-02-23
                case "ktr": return "dtp"; // Kota Marudu Tinagas; deprecated 2016-05-30
                case "kvs": return "gdj"; // Kunggara; deprecated 2016-05-30
                case "kwq": return "yam"; // Kwak; deprecated 2015-02-12
                case "kxe": return "tvd"; // Kakihum; deprecated 2015-02-12
                case "kzj": return "dtp"; // Coastal Kadazan; deprecated 2016-05-30
                case "kzt": return "dtp"; // Tambunan Dusun; deprecated 2016-05-30
                case "lii": return "raq"; // Lingkhim; deprecated 2015-02-12
                case "lmm": return "rmx"; // Lamam; deprecated 2014-02-28
                case "meg": return "cir"; // Mea; deprecated 2013-09-10
                case "mst": return "mry"; // Cataelano Mandaya; deprecated 2010-03-11
                case "mwj": return "vaj"; // Maligo; deprecated 2015-02-12
                case "myt": return "mry"; // Sangab Mandaya; deprecated 2010-03-11
                case "nad": return "xny"; // Nijadali; deprecated 2016-05-30
                case "ncp": return "kdz"; // Ndaktup; deprecated 2018-03-08
                case "nnx": return "ngv"; // Ngong; deprecated 2015-02-12
                case "nts": return "pij"; // Natagaimas; deprecated 2016-05-30
                case "oun": return "vaj"; // !O!ung; deprecated 2015-02-12
                case "pcr": return "adx"; // Panang; deprecated 2013-09-10
                case "pmc": return "huw"; // Palumata; deprecated 2016-05-30
                case "pmu": return "phr"; // Mirpur Panjabi; deprecated 2015-02-12
                case "ppa": return "bfy"; // Pao; deprecated 2016-05-30
                case "ppr": return "lcq"; // Piru; deprecated 2013-09-10
                case "pry": return "prt"; // Pray 3; deprecated 2016-05-30
                case "puz": return "pub"; // Purum Naga; deprecated 2014-02-28
                case "sca": return "hle"; // Sansu; deprecated 2012-08-12
                case "skk": return "oyb"; // Sok; deprecated 2017-02-23
                case "tdu": return "dtp"; // Tempasuk Dusun; deprecated 2016-05-30
                case "thc": return "tpo"; // Tai Hang Tong; deprecated 2016-05-30
                case "thx": return "oyb"; // The; deprecated 2015-02-12
                case "tie": return "ras"; // Tingal; deprecated 2011-08-16
                case "tkk": return "twm"; // Takpa; deprecated 2011-08-16
                case "tlw": return "weo"; // South Wemale; deprecated 2012-08-12
                case "tmp": return "tyj"; // Tai Mène; deprecated 2016-05-30
                case "tne": return "kak"; // Tinoc Kallahan; deprecated 2016-05-30
                case "tnf": return "prs"; // Tangshewi; deprecated 2010-03-11
                case "tsf": return "taj"; // Southwestern Tamang; deprecated 2015-02-12
                case "uok": return "ema"; // Uokha; deprecated 2015-02-12
                case "xba": return "cax"; // Kamba (Brazil); deprecated 2016-05-30
                case "xia": return "acn"; // Xiandao; deprecated 2013-09-10
                case "xkh": return "waw"; // Karahawyana; deprecated 2016-05-30
                case "xsj": return "suj"; // Subi; deprecated 2015-02-12
                case "ybd": return "rki"; // Yangbye; deprecated 2012-08-12
                case "yma": return "lrr"; // Yamphe; deprecated 2012-08-12
                case "ymt": return "mtm"; // Mator-Taygi-Karagas; deprecated 2015-02-12
                case "yos": return "zom"; // Yos; deprecated 2013-09-10
                case "yuu": return "yug"; // Yugh; deprecated 2014-02-28
                default: return languageCode;
            }
        }

        /// The region subtag for the locale.
        ///
        /// This can be null.
        ///
        /// This is expected to be string registered in the [IANA Language Subtag
        /// Registry](https://www.iana.org/assignments/language-subtag-registry/language-subtag-registry)
        /// with the type "region". The string specified must match the case of the
        /// string in the registry.
        ///
        /// Region subtags that are deprecated in the registry and have a preferred
        /// code are changed to their preferred code. For example, `const Locale('de',
        /// 'DE')` and `const Locale('de', 'DD')` are equal, and both have the
        /// [countryCode] `DE`, because `DD` is a deprecated language subtag that was
        /// replaced by the subtag `DE`.
        String countryCode => _canonicalizeRegionCode(_countryCode);
        readonly String _countryCode;

        static String _canonicalizeRegionCode(String regionCode)
        {
            // This switch statement is generated by //flutter/tools/gen_locale.dart
            // Mappings generated for language subtag registry as of 2018-08-08.
            switch (regionCode)
            {
                case "BU": return "MM"; // Burma; deprecated 1989-12-05
                case "DD": return "DE"; // German Democratic Republic; deprecated 1990-10-30
                case "FX": return "FR"; // Metropolitan France; deprecated 1997-07-14
                case "TP": return "TL"; // East Timor; deprecated 2002-05-20
                case "YD": return "YE"; // Democratic Yemen; deprecated 1990-08-14
                case "ZR": return "CD"; // Zaire; deprecated 1997-07-14
                default: return regionCode;
            }
        }


        //public static bool operator ==(Locale first, Locale second)
        //{       
        //
        //    if (first.Equals(second))
        //        return true;
        //    if (!(second is Locale))
        //        return false;
        //    Locale typedOther = second;
        //    return second.languageCode == first.languageCode
        //        && second.countryCode == first.countryCode;
        //}

        //public static bool operator !=(Locale first, Locale second) => !(first == second);

        public int hashCode
        {
            get
            {
                int result = 373;
                result = 37 * result + languageCode.GetHashCode();
                if (_countryCode != null)
                    result = 37 * result + countryCode.GetHashCode();
                return result;
            }
        }

        public String toString()
        {
            if (_countryCode == null)
                return languageCode;
            return $"{languageCode}_{countryCode}";
        }
    }

    /// The most basic interface to the host operating system's user interface.
    ///
    /// There is a single Window instance in the system, which you can
    /// obtain from the [window] property.
    public class Window: Engine.Window.NativeWindow
    {
        static Window _instance;
        public static Window Instance => _instance ?? (_instance = new Window());


        private Window() { }

        /// The number of device pixels for each logical pixel. This number might not
        /// be a power of two. Indeed, it might not even be an integer. For example,
        /// the Nexus 6 has a device pixel ratio of 3.5.
        ///
        /// Device pixels are also referred to as physical pixels. Logical pixels are
        /// also referred to as device-independent or resolution-independent pixels.
        ///
        /// By definition, there are roughly 38 logical pixels per centimeter, or
        /// about 96 logical pixels per inch, of the physical display. The value
        /// returned by [devicePixelRatio] is ultimately obtained either from the
        /// hardware itself, the device drivers, or a hard-coded value stored in the
        /// operating system or firmware, and may be inaccurate, sometimes by a
        /// significant margin.
        ///
        /// The Flutter framework operates in logical pixels, so it is rarely
        /// necessary to directly deal with this property.
        ///
        /// When this changes, [onMetricsChanged] is called.
        ///
        /// See also:
        ///
        ///  * [WidgetsBindingObserver], for a mechanism at the widgets layer to
        ///    observe when this value changes.
        public double devicePixelRatio { get; set; } = 1.0;

        /// The dimensions of the rectangle into which the application will be drawn,
        /// in physical pixels.
        ///
        /// When this changes, [onMetricsChanged] is called.
        ///
        /// At startup, the size of the application window may not be known before Dart
        /// code runs. If this value is observed early in the application lifecycle,
        /// it may report [Size.zero].
        ///
        /// This value does not take into account any on-screen keyboards or other
        /// system UI. The [padding] and [viewInsets] properties provide a view into
        /// how much of each side of the application may be obscured by system UI.
        ///
        /// See also:
        ///
        ///  * [WidgetsBindingObserver], for a mechanism at the widgets layer to
        ///    observe when this value changes.
        public Size physicalSize { get; set; } = Size.zero;

        /// The number of physical pixels on each side of the display rectangle into
        /// which the application can render, but over which the operating system
        /// will likely place system UI, such as the keyboard, that fully obscures
        /// any content.
        ///
        /// When this changes, [onMetricsChanged] is called.
        ///
        /// See also:
        ///
        ///  * [WidgetsBindingObserver], for a mechanism at the widgets layer to
        ///    observe when this value changes.
        ///  * [MediaQuery.of], a simpler mechanism for the same.
        ///  * [Scaffold], which automatically applies the view insets in material
        ///    design applications.
        public WindowPadding viewInsets { get; set; } = WindowPadding.zero;

        /// The number of physical pixels on each side of the display rectangle into
        /// which the application can render, but which may be partially obscured by
        /// system UI (such as the system notification area), or or physical
        /// intrusions in the display (e.g. overscan regions on television screens or
        /// phone sensor housings).
        ///
        /// When this changes, [onMetricsChanged] is called.
        ///
        /// See also:
        ///
        ///  * [WidgetsBindingObserver], for a mechanism at the widgets layer to
        ///    observe when this value changes.
        ///  * [MediaQuery.of], a simpler mechanism for the same.
        ///  * [Scaffold], which automatically applies the padding in material design
        ///    applications.
        public WindowPadding padding { get; set; } = WindowPadding.zero;

        /// A callback that is invoked whenever the [devicePixelRatio],
        /// [physicalSize], [padding], or [viewInsets] values change, for example
        /// when the device is rotated or when the application is resized (e.g. when
        /// showing applications side-by-side on Android).
        ///
        /// The engine invokes this callback in the same zone in which the callback
        /// was set.
        ///
        /// The framework registers with this callback and updates the layout
        /// appropriately.
        ///
        /// See also:
        ///
        ///  * [WidgetsBindingObserver], for a mechanism at the widgets layer to
        ///    register for notifications when this is called.
        ///  * [MediaQuery.of], a simpler mechanism for the same.
        public VoidCallback onMetricsChanged
        {
            get { return _onMetricsChanged; }
            set
            {
                _onMetricsChanged = value;
                _onMetricsChangedZone = Zone.current;
            }
        }
        VoidCallback _onMetricsChanged;
        public Zone _onMetricsChangedZone;


        /// The system-reported default locale of the device.
        ///
        /// This establishes the language and formatting conventions that application
        /// should, if possible, use to render their user interface.
        ///
        /// This is the first locale selected by the user and is the user's
        /// primary locale (the locale the device UI is displayed in)
        ///
        /// This is equivalent to `locales.first` and will provide an empty non-null locale
        /// if the [locales] list has not been set or is empty.
        public Locale locale
        {
            get
            {
                if (locales != null && locales.Count > 0)
                {
                    return locales.First();
                }
                return Locale.none;
            }
        }

        /// The full system-reported supported locales of the device.
        ///
        /// This establishes the language and formatting conventions that application
        /// should, if possible, use to render their user interface.
        ///
        /// The list is ordered in order of priority, with lower-indexed locales being
        /// preferred over higher-indexed ones. The first element is the primary [locale].
        ///
        /// The [onLocaleChanged] callback is called whenever this value changes.
        ///
        /// See also:
        ///
        ///  * [WidgetsBindingObserver], for a mechanism at the widgets layer to
        ///    observe when this value changes.
        public List<Locale> locales { get; set;  }

        /// A callback that is invoked whenever [locale] changes value.
        ///
        /// The framework invokes this callback in the same zone in which the
        /// callback was set.
        ///
        /// See also:
        ///
        ///  * [WidgetsBindingObserver], for a mechanism at the widgets layer to
        ///    observe when this callback is invoked.
        public VoidCallback onLocaleChanged
        {
            get { return _onLocaleChanged; }
            set
            {
                _onLocaleChanged = value;
                _onLocaleChangedZone = Zone.current;
            }
        }
        VoidCallback _onLocaleChanged;
        public Zone _onLocaleChangedZone;

        /// The system-reported text scale.
        ///
        /// This establishes the text scaling factor to use when rendering text,
        /// according to the user's platform preferences.
        ///
        /// The [onTextScaleFactorChanged] callback is called whenever this value
        /// changes.
        ///
        /// See also:
        ///
        ///  * [WidgetsBindingObserver], for a mechanism at the widgets layer to
        ///    observe when this value changes.
        public double textScaleFactor { get; set; } = 1.0;

        /// The setting indicating whether time should always be shown in the 24-hour
        /// format.
        ///
        /// This option is used by [showTimePicker].
        public bool alwaysUse24HourFormat { get; set; } = false;

        /// A callback that is invoked whenever [textScaleFactor] changes value.
        ///
        /// The framework invokes this callback in the same zone in which the
        /// callback was set.
        ///
        /// See also:
        ///
        ///  * [WidgetsBindingObserver], for a mechanism at the widgets layer to
        ///    observe when this callback is invoked.
        public VoidCallback onTextScaleFactorChanged
        {
            get { return _onTextScaleFactorChanged; }
            set
            {
                _onTextScaleFactorChanged = value;
                _onTextScaleFactorChangedZone = Zone.current;
            }
        }
        VoidCallback _onTextScaleFactorChanged;
        public Zone _onTextScaleFactorChangedZone;

        /// A callback that is invoked to notify the application that it is an
        /// appropriate time to provide a scene using the [SceneBuilder] API and the
        /// [render] method. When possible, this is driven by the hardware VSync
        /// signal. This is only called if [scheduleFrame] has been called since the
        /// last time this callback was invoked.
        ///
        /// The [onDrawFrame] callback is invoked immediately after [onBeginFrame],
        /// after draining any microtasks (e.g. completions of any [Future]s) queued
        /// by the [onBeginFrame] handler.
        ///
        /// The framework invokes this callback in the same zone in which the
        /// callback was set.
        ///
        /// See also:
        ///
        ///  * [SchedulerBinding], the Flutter framework class which manages the
        ///    scheduling of frames.
        ///  * [RendererBinding], the Flutter framework class which manages layout and
        ///    painting.
        public FrameCallback onBeginFrame
        {
            get { return _onBeginFrame; }
            set
            {
                _onBeginFrame = value;
                _onBeginFrameZone = Zone.current;
            }
        }
        FrameCallback _onBeginFrame;
        public Zone _onBeginFrameZone;

        /// A callback that is invoked for each frame after [onBeginFrame] has
        /// completed and after the microtask queue has been drained. This can be
        /// used to implement a second phase of frame rendering that happens
        /// after any deferred work queued by the [onBeginFrame] phase.
        ///
        /// The framework invokes this callback in the same zone in which the
        /// callback was set.
        ///
        /// See also:
        ///
        ///  * [SchedulerBinding], the Flutter framework class which manages the
        ///    scheduling of frames.
        ///  * [RendererBinding], the Flutter framework class which manages layout and
        ///    painting.
        public VoidCallback onDrawFrame
        {
            get { return _onDrawFrame; }
            set
            {
                _onDrawFrame = value;
                _onDrawFrameZone = Zone.current;
            }
        }
        VoidCallback _onDrawFrame;
        public Zone _onDrawFrameZone;

        /// A callback that is invoked when pointer data is available.
        ///
        /// The framework invokes this callback in the same zone in which the
        /// callback was set.
        ///
        /// See also:
        ///
        ///  * [GestureBinding], the Flutter framework class which manages pointer
        ///    events.
        public PointerDataPacketCallback onPointerDataPacket
        {
            get { return _onPointerDataPacket; }
            set
            {
                _onPointerDataPacket = value;
                _onPointerDataPacketZone = Zone.current;
            }
        }
        PointerDataPacketCallback _onPointerDataPacket;
        public Zone _onPointerDataPacketZone;

        /// The route or path that the embedder requested when the application was
        /// launched.
        ///
        /// This will be the string "`/`" if no particular route was requested.
        ///
        /// ## Android
        ///
        /// On Android, calling
        /// [`FlutterView.setInitialRoute`](/javadoc/io/flutter/view/FlutterView.html#setInitialRoute-java.lang.String-)
        /// will set this value. The value must be set sufficiently early, i.e. before
        /// the [runApp] call is executed in Dart, for this to have any effect on the
        /// framework. The `createFlutterView` method in your `FlutterActivity`
        /// subclass is a suitable time to set the value. The application's
        /// `AndroidManifest.xml` file must also be updated to have a suitable
        /// [`<intent-filter>`](https://developer.android.com/guide/topics/manifest/intent-filter-element.html).
        ///
        /// ## iOS
        ///
        /// On iOS, calling
        /// [`FlutterViewController.setInitialRoute`](/objcdoc/Classes/FlutterViewController.html#/c:objc%28cs%29FlutterViewController%28im%29setInitialRoute:)
        /// will set this value. The value must be set sufficiently early, i.e. before
        /// the [runApp] call is executed in Dart, for this to have any effect on the
        /// framework. The `application:didFinishLaunchingWithOptions:` method is a
        /// suitable time to set this value.
        ///
        /// See also:
        ///
        ///  * [Navigator], a widget that handles routing.
        ///  * [SystemChannels.navigation], which handles subsequent navigation
        ///    requests from the embedder.
        public String defaultRouteName => _defaultRouteName();
        String _defaultRouteName()
        {
            // native 'Window_defaultRouteName';
            return string.Empty; // Tmp to resolve build
        }


        /// Requests that, at the next appropriate opportunity, the [onBeginFrame]
        /// and [onDrawFrame] callbacks be invoked.
        ///
        /// See also:
        ///
        ///  * [SchedulerBinding], the Flutter framework class which manages the
        ///    scheduling of frames.
        public void scheduleFrame()
        {
            // native 'Window_scheduleFrame';
        }

        /// Updates the application's rendering on the GPU with the newly provided
        /// [Scene]. This function must be called within the scope of the
        /// [onBeginFrame] or [onDrawFrame] callbacks being invoked. If this function
        /// is called a second time during a single [onBeginFrame]/[onDrawFrame]
        /// callback sequence or called outside the scope of those callbacks, the call
        /// will be ignored.
        ///
        /// To record graphical operations, first create a [PictureRecorder], then
        /// construct a [Canvas], passing that [PictureRecorder] to its constructor.
        /// After issuing all the graphical operations, call the
        /// [PictureRecorder.endRecording] function on the [PictureRecorder] to obtain
        /// the final [Picture] that represents the issued graphical operations.
        ///
        /// Next, create a [SceneBuilder], and add the [Picture] to it using
        /// [SceneBuilder.addPicture]. With the [SceneBuilder.build] method you can
        /// then obtain a [Scene] object, which you can display to the user via this
        /// [render] function.
        ///
        /// See also:
        ///
        ///  * [SchedulerBinding], the Flutter framework class which manages the
        ///    scheduling of frames.
        ///  * [RendererBinding], the Flutter framework class which manages layout and
        ///    painting.
        public void render(Scene scene)
        {
            this.Render(scene);
            // [DONE] native 'Window_render';
        }

        /// Whether the user has requested that [updateSemantics] be called when
        /// the semantic contents of window changes.
        ///
        /// The [onSemanticsEnabledChanged] callback is called whenever this value
        /// changes.
        public bool semanticsEnabled { get; set; } = false;

        /// A callback that is invoked when the value of [semanticsEnabled] changes.
        ///
        /// The framework invokes this callback in the same zone in which the
        /// callback was set.
        public VoidCallback onSemanticsEnabledChanged
        {
            get { return _onSemanticsEnabledChanged; }
            set
            {
                _onSemanticsEnabledChanged = value;
                _onSemanticsEnabledChangedZone = Zone.current;
            }
        }
        VoidCallback _onSemanticsEnabledChanged;
        public Zone _onSemanticsEnabledChangedZone;

        /// A callback that is invoked whenever the user requests an action to be
        /// performed.
        ///
        /// This callback is used when the user expresses the action they wish to
        /// perform based on the semantics supplied by [updateSemantics].
        ///
        /// The framework invokes this callback in the same zone in which the
        /// callback was set.
        public SemanticsActionCallback onSemanticsAction
        {
            get { return _onSemanticsAction; }
            set
            {
                _onSemanticsAction = value;
                _onSemanticsActionZone = Zone.current;
            }
        }
        SemanticsActionCallback _onSemanticsAction;
        public Zone _onSemanticsActionZone;

        /// Additional accessibility features that may be enabled by the platform.
        public AccessibilityFeatures accessibilityFeatures { get; set; }

        /// A callback that is invoked when the value of [accessibilityFlags] changes.
        ///
        /// The framework invokes this callback in the same zone in which the
        /// callback was set.
        public VoidCallback onAccessibilityFeaturesChanged
        {
            get { return _onAccessibilityFeaturesChanged; }
            set
            {
                _onAccessibilityFeaturesChanged = value;
                _onAccessibilityFlagsChangedZone = Zone.current;
            }
        }
        VoidCallback _onAccessibilityFeaturesChanged;
        public Zone _onAccessibilityFlagsChangedZone;

        /// Change the retained semantics data about this window.
        ///
        /// If [semanticsEnabled] is true, the user has requested that this funciton
        /// be called whenever the semantic content of this window changes.
        ///
        /// In either case, this function disposes the given update, which means the
        /// semantics update cannot be used further.
        public void updateSemantics(SemanticsUpdate update)
        {
            // native 'Window_updateSemantics';
        }

        /// Set the debug name associated with this window's root isolate.
        ///
        /// Normally debug names are automatically generated from the Dart port, entry
        /// point, and source file. For example: `main.dart$main-1234`.
        ///
        /// This can be combined with flutter tools `--isolate-filter` flag to debug
        /// specific root isolates. For example: `flutter attach --isolate-filter=[name]`.
        /// Note that this does not rename any child isolates of the root.
        public void setIsolateDebugName(String name)
        {
            // native 'Window_setIsolateDebugName';
        }

        /// Sends a message to a platform-specific plugin.
        ///
        /// The `name` parameter determines which plugin receives the message. The
        /// `data` parameter contains the message payload and is typically UTF-8
        /// encoded JSON but can be arbitrary data. If the plugin replies to the
        /// message, `callback` will be called with the response.
        ///
        /// The framework invokes [callback] in the same zone in which this method
        /// was called.
        public void sendPlatformMessage(String name,
                                 ByteData data,
                                 PlatformMessageResponseCallback callback)
        {
            String error =
                _sendPlatformMessage(name, _zonedPlatformMessageResponseCallback(callback), data);
            if (error != null)
                throw new Exception(error);
        }
        String _sendPlatformMessage(String name,
                                    PlatformMessageResponseCallback callback,
                                    ByteData data)
        {
            // native 'Window_sendPlatformMessage';
            return string.Empty; // Tmp to resolve build
        }

        /// Called whenever this window receives a message from a platform-specific
        /// plugin.
        ///
        /// The `name` parameter determines which plugin sent the message. The `data`
        /// parameter is the payload and is typically UTF-8 encoded JSON but can be
        /// arbitrary data.
        ///
        /// Message handlers must call the function given in the `callback` parameter.
        /// If the handler does not need to respond, the handler should pass null to
        /// the callback.
        ///
        /// The framework invokes this callback in the same zone in which the
        /// callback was set.
        public PlatformMessageCallback onPlatformMessage
        {
            get { return _onPlatformMessage; }
            set
            {
                _onPlatformMessage = value;
                _onPlatformMessageZone = Zone.current;
            }
        }
        PlatformMessageCallback _onPlatformMessage;
        public Zone _onPlatformMessageZone;

        /// Called by [_dispatchPlatformMessage].
        public void _respondToPlatformMessage(int responseId, ByteData data)
        {
            // native 'Window_respondToPlatformMessage';
        }
        /// Wraps the given [callback] in another callback that ensures that the
        /// original callback is called in the zone it was registered in.
        static PlatformMessageResponseCallback _zonedPlatformMessageResponseCallback(PlatformMessageResponseCallback callback)
        {
            if (callback == null)
                return null;

            // Store the zone in which the callback is being registered.
            Zone registrationZone = Zone.current;

            return (data) => registrationZone.runUnaryGuarded(callback, data);

        }
    }

    /// Additional accessibility features that may be enabled by the platform.
    ///
    /// It is not possible to enable these settings from Flutter, instead they are
    /// used by the platform to indicate that additional accessibility features are
    /// enabled.
    public class AccessibilityFeatures
    {
        public AccessibilityFeatures(int _index) { this._index = _index; }

        const int _kAccessibleNavigation = 1 << 0;
        const int _kInvertColorsIndex = 1 << 1;
        const int _kDisableAnimationsIndex = 1 << 2;
        const int _kBoldTextIndex = 1 << 3;
        const int _kReduceMotionIndex = 1 << 4;

        // A bitfield which represents each enabled feature.
        readonly int _index;

        /// Whether there is a running accessibility service which is changing the
        /// interaction model of the device.
        ///
        /// For example, TalkBack on Android and VoiceOver on iOS enable this flag.
        public bool accessibleNavigation => (_kAccessibleNavigation & _index) != 0;

        /// The platform is inverting the colors of the application.
        public bool invertColors => (_kInvertColorsIndex & _index) != 0;

        /// The platform is requesting that animations be disabled or simplified.
        public bool disableAnimations => (_kDisableAnimationsIndex & _index) != 0;

        /// The platform is requesting that text be rendered at a bold font weight.
        ///
        /// Only supported on iOS.
        public bool boldText => (_kBoldTextIndex & _index) != 0;

        /// The platform is requesting that certain animations be simplified and
        /// parallax effects removed.
        ///
        /// Only supported on iOS.
        public bool reduceMotion => (_kReduceMotionIndex & _index) != 0;

        public String toString()
        {
            List<String> features = new List<String>();
            if (accessibleNavigation)
                features.Add("accessibleNavigation");
            if (invertColors)
                features.Add("invertColors");
            if (disableAnimations)
                features.Add("disableAnimations");
            if (boldText)
                features.Add("boldText");
            if (reduceMotion)
                features.Add("reduceMotion");
            return $"AccessibilityFeatures{features}";
        }

        public static bool operator ==(AccessibilityFeatures first, AccessibilityFeatures other)
        {
            if (other.GetType() != first.GetType())
                return false;
            AccessibilityFeatures typedOther = other;
            return first._index == typedOther._index;
        }

        public static bool operator !=(AccessibilityFeatures first, AccessibilityFeatures other) => !(first == other);

        public int hashCode => _index.GetHashCode();
    }

    /// The [Window] singleton. This object exposes the size of the display, the
    /// core scheduler API, the input event callback, the graphics drawing API, and
    /// other such core services.
    //readonly Window window = new Window();
    // Do Window.Instance instead
}
