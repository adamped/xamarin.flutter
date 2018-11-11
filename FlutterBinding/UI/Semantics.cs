﻿using System;
using System.Collections.Generic;

namespace FlutterBinding.UI
{

    /// The possible actions that can be conveyed from the operating system
    /// accessibility APIs to a semantics node.
    public class SemanticsAction
    {
        private SemanticsAction(int index)
        {
            this.index = index;
        }

        const int _kTapIndex = 1 << 0;
        const int _kLongPressIndex = 1 << 1;
        const int _kScrollLeftIndex = 1 << 2;
        const int _kScrollRightIndex = 1 << 3;
        const int _kScrollUpIndex = 1 << 4;
        const int _kScrollDownIndex = 1 << 5;
        const int _kIncreaseIndex = 1 << 6;
        const int _kDecreaseIndex = 1 << 7;
        const int _kShowOnScreenIndex = 1 << 8;
        const int _kMoveCursorForwardByCharacterIndex = 1 << 9;
        const int _kMoveCursorBackwardByCharacterIndex = 1 << 10;
        const int _kSetSelectionIndex = 1 << 11;
        const int _kCopyIndex = 1 << 12;
        const int _kCutIndex = 1 << 13;
        const int _kPasteIndex = 1 << 14;
        const int _kDidGainAccessibilityFocusIndex = 1 << 15;
        const int _kDidLoseAccessibilityFocusIndex = 1 << 16;
        const int _kCustomAction = 1 << 17;
        const int _kDismissIndex = 1 << 18;
        const int _kMoveCursorForwardByWordIndex = 1 << 19;
        const int _kMoveCursorBackwardByWordIndex = 1 << 20;

        /// The numerical value for this action.
        ///
        /// Each action has one bit set in this bit field.
        public readonly int index;

        /// The equivalent of a user briefly tapping the screen with the finger
        /// without moving it.
        public static SemanticsAction tap = new SemanticsAction(_kTapIndex);

        /// The equivalent of a user pressing and holding the screen with the finger
        /// for a few seconds without moving it.
        public static SemanticsAction longPress = new SemanticsAction(_kLongPressIndex);

        /// The equivalent of a user moving their finger across the screen from right
        /// to left.
        ///
        /// This action should be recognized by controls that are horizontally
        /// scrollable.
        public static SemanticsAction scrollLeft = new SemanticsAction(_kScrollLeftIndex);

        /// The equivalent of a user moving their finger across the screen from left
        /// to right.
        ///
        /// This action should be recognized by controls that are horizontally
        /// scrollable.
        public static SemanticsAction scrollRight = new SemanticsAction(_kScrollRightIndex);

        /// The equivalent of a user moving their finger across the screen from
        /// bottom to top.
        ///
        /// This action should be recognized by controls that are vertically
        /// scrollable.
        public static SemanticsAction scrollUp = new SemanticsAction(_kScrollUpIndex);

        /// The equivalent of a user moving their finger across the screen from top
        /// to bottom.
        ///
        /// This action should be recognized by controls that are vertically
        /// scrollable.
        public static SemanticsAction scrollDown = new SemanticsAction(_kScrollDownIndex);

        /// A request to increase the value represented by the semantics node.
        ///
        /// For example, this action might be recognized by a slider control.
        public static SemanticsAction increase = new SemanticsAction(_kIncreaseIndex);

        /// A request to decrease the value represented by the semantics node.
        ///
        /// For example, this action might be recognized by a slider control.
        public static SemanticsAction decrease = new SemanticsAction(_kDecreaseIndex);

        /// A request to fully show the semantics node on screen.
        ///
        /// For example, this action might be send to a node in a scrollable list that
        /// is partially off screen to bring it on screen.
        public static SemanticsAction showOnScreen = new SemanticsAction(_kShowOnScreenIndex);

        /// Move the cursor forward by one character.
        ///
        /// This is for example used by the cursor control in text fields.
        ///
        /// The action includes a boolean argument, which indicates whether the cursor
        /// movement should extend (or start) a selection.
        public static SemanticsAction moveCursorForwardByCharacter = new SemanticsAction(_kMoveCursorForwardByCharacterIndex);

        /// Move the cursor backward by one character.
        ///
        /// This is for example used by the cursor control in text fields.
        ///
        /// The action includes a boolean argument, which indicates whether the cursor
        /// movement should extend (or start) a selection.
        public static SemanticsAction moveCursorBackwardByCharacter = new SemanticsAction(_kMoveCursorBackwardByCharacterIndex);

        /// Set the text selection to the given range.
        ///
        /// The provided argument is a Map<String, int> which includes the keys `base`
        /// and `extent` indicating where the selection within the `value` of the
        /// semantics node should start and where it should end. Values for both
        /// keys can range from 0 to length of `value` (inclusive).
        ///
        /// Setting `base` and `extent` to the same value will move the cursor to
        /// that position (without selecting anything).
        public static SemanticsAction setSelection = new SemanticsAction(_kSetSelectionIndex);

        /// Copy the current selection to the clipboard.
        public static SemanticsAction copy = new SemanticsAction(_kCopyIndex);

        /// Cut the current selection and place it in the clipboard.
        public static SemanticsAction cut = new SemanticsAction(_kCutIndex);

        /// Paste the current content of the clipboard.
        public static SemanticsAction paste = new SemanticsAction(_kPasteIndex);

        /// Indicates that the nodes has gained accessibility focus.
        ///
        /// This handler is invoked when the node annotated with this handler gains
        /// the accessibility focus. The accessibility focus is the
        /// green (on Android with TalkBack) or black (on iOS with VoiceOver)
        /// rectangle shown on screen to indicate what element an accessibility
        /// user is currently interacting with.
        ///
        /// The accessibility focus is different from the input focus. The input focus
        /// is usually held by the element that currently responds to keyboard inputs.
        /// Accessibility focus and input focus can be held by two different nodes!
        public static SemanticsAction didGainAccessibilityFocus = new SemanticsAction(_kDidGainAccessibilityFocusIndex);

        /// Indicates that the nodes has lost accessibility focus.
        ///
        /// This handler is invoked when the node annotated with this handler
        /// loses the accessibility focus. The accessibility focus is
        /// the green (on Android with TalkBack) or black (on iOS with VoiceOver)
        /// rectangle shown on screen to indicate what element an accessibility
        /// user is currently interacting with.
        ///
        /// The accessibility focus is different from the input focus. The input focus
        /// is usually held by the element that currently responds to keyboard inputs.
        /// Accessibility focus and input focus can be held by two different nodes!
        public static SemanticsAction didLoseAccessibilityFocus = new SemanticsAction(_kDidLoseAccessibilityFocusIndex);

        /// Indicates that the user has invoked a custom accessibility action.
        ///
        /// This handler is added automatically whenever a custom accessibility
        /// action is added to a semantics node.
        public static SemanticsAction customAction = new SemanticsAction(_kCustomAction);

        /// A request that the node should be dismissed.
        ///
        /// A [Snackbar], for example, may have a dismiss action to indicate to the
        /// user that it can be removed after it is no longer relevant. On Android,
        /// (with TalkBack) special hint text is spoken when focusing the node and
        /// a custom action is availible in the local context menu. On iOS,
        /// (with VoiceOver) users can perform a standard gesture to dismiss it.
        public static SemanticsAction dismiss = new SemanticsAction(_kDismissIndex);

        /// Move the cursor forward by one word.
        ///
        /// This is for example used by the cursor control in text fields.
        ///
        /// The action includes a boolean argument, which indicates whether the cursor
        /// movement should extend (or start) a selection.
        public static SemanticsAction moveCursorForwardByWord = new SemanticsAction(_kMoveCursorForwardByWordIndex);

        /// Move the cursor backward by one word.
        ///
        /// This is for example used by the cursor control in text fields.
        ///
        /// The action includes a boolean argument, which indicates whether the cursor
        /// movement should extend (or start) a selection.
        public static SemanticsAction moveCursorBackwardByWord = new SemanticsAction(_kMoveCursorBackwardByWordIndex);

        /// The possible semantics actions.
        ///
        /// The map's key is the [index] of the action and the value is the action
        /// itself.
        public static Dictionary<int, SemanticsAction> values = new Dictionary<int, SemanticsAction>{
            { _kTapIndex, tap },
             { _kLongPressIndex, longPress },
             { _kScrollLeftIndex, scrollLeft },
             { _kScrollRightIndex, scrollRight },
             { _kScrollUpIndex, scrollUp },
             { _kScrollDownIndex, scrollDown },
             { _kIncreaseIndex, increase },
             { _kDecreaseIndex, decrease },
             { _kShowOnScreenIndex, showOnScreen },
             { _kMoveCursorForwardByCharacterIndex, moveCursorForwardByCharacter },
             { _kMoveCursorBackwardByCharacterIndex, moveCursorBackwardByCharacter },
            {  _kSetSelectionIndex, setSelection },
             { _kCopyIndex, copy },
             { _kCutIndex, cut },
            {  _kPasteIndex, paste },
            {  _kDidGainAccessibilityFocusIndex, didGainAccessibilityFocus },
            {  _kDidLoseAccessibilityFocusIndex, didLoseAccessibilityFocus },
            {  _kCustomAction, customAction },
            {  _kDismissIndex, dismiss },
            {  _kMoveCursorForwardByWordIndex, moveCursorForwardByWord },
            {  _kMoveCursorBackwardByWordIndex, moveCursorBackwardByWord }  };

        public String toString()
        {
            switch (index)
            {
                case _kTapIndex:
                    return "SemanticsAction.tap";
                case _kLongPressIndex:
                    return "SemanticsAction.longPress";
                case _kScrollLeftIndex:
                    return "SemanticsAction.scrollLeft";
                case _kScrollRightIndex:
                    return "SemanticsAction.scrollRight";
                case _kScrollUpIndex:
                    return "SemanticsAction.scrollUp";
                case _kScrollDownIndex:
                    return "SemanticsAction.scrollDown";
                case _kIncreaseIndex:
                    return "SemanticsAction.increase";
                case _kDecreaseIndex:
                    return "SemanticsAction.decrease";
                case _kShowOnScreenIndex:
                    return "SemanticsAction.showOnScreen";
                case _kMoveCursorForwardByCharacterIndex:
                    return "SemanticsAction.moveCursorForwardByCharacter";
                case _kMoveCursorBackwardByCharacterIndex:
                    return "SemanticsAction.moveCursorBackwardByCharacter";
                case _kSetSelectionIndex:
                    return "SemanticsAction.setSelection";
                case _kCopyIndex:
                    return "SemanticsAction.copy";
                case _kCutIndex:
                    return "SemanticsAction.cut";
                case _kPasteIndex:
                    return "SemanticsAction.paste";
                case _kDidGainAccessibilityFocusIndex:
                    return "SemanticsAction.didGainAccessibilityFocus";
                case _kDidLoseAccessibilityFocusIndex:
                    return "SemanticsAction.didLoseAccessibilityFocus";
                case _kCustomAction:
                    return "SemanticsAction.customAction";
                case _kDismissIndex:
                    return "SemanticsAction.dismiss";
                case _kMoveCursorForwardByWordIndex:
                    return "SemanticsAction.moveCursorForwardByWord";
                case _kMoveCursorBackwardByWordIndex:
                    return "SemanticsAction.moveCursorBackwardByWord";
            }
            return null;
        }
    }

    /// A Boolean value that can be associated with a semantics node.
    public class SemanticsFlag
    {
        const int _kHasCheckedStateIndex = 1 << 0;
        const int _kIsCheckedIndex = 1 << 1;
        const int _kIsSelectedIndex = 1 << 2;
        const int _kIsButtonIndex = 1 << 3;
        const int _kIsTextFieldIndex = 1 << 4;
        const int _kIsFocusedIndex = 1 << 5;
        const int _kHasEnabledStateIndex = 1 << 6;
        const int _kIsEnabledIndex = 1 << 7;
        const int _kIsInMutuallyExclusiveGroupIndex = 1 << 8;
        const int _kIsHeaderIndex = 1 << 9;
        const int _kIsObscuredIndex = 1 << 10;
        const int _kScopesRouteIndex = 1 << 11;
        const int _kNamesRouteIndex = 1 << 12;
        const int _kIsHiddenIndex = 1 << 13;
        const int _kIsImageIndex = 1 << 14;
        const int _kIsLiveRegionIndex = 1 << 15;
        const int _kHasToggledStateIndex = 1 << 16;
        const int _kIsToggledIndex = 1 << 17;
        const int _kHasImplicitScrollingIndex = 1 << 18;

        public SemanticsFlag(int index)
        {
            this.index = index;
        }

        /// The numerical value for this flag.
        ///
        /// Each flag has one bit set in this bit field.
        public readonly int index;

        /// The semantics node has the quality of either being "checked" or "unchecked".
        ///
        /// This flag is mutually exclusive with [hasToggledState].
        ///
        /// For example, a checkbox or a radio button widget has checked state.
        ///
        /// See also:
        ///
        ///   * [SemanticsFlag.isChecked], which controls whether the node is "checked" or "unchecked".
        public static SemanticsFlag hasCheckedState = new SemanticsFlag(_kHasCheckedStateIndex);

        /// Whether a semantics node that [hasCheckedState] is checked.
        ///
        /// If true, the semantics node is "checked". If false, the semantics node is
        /// "unchecked".
        ///
        /// For example, if a checkbox has a visible checkmark, [isChecked] is true.
        ///
        /// See also:
        ///
        ///   * [SemanticsFlag.hasCheckedState], which enables a checked state.
        public static SemanticsFlag isChecked = new SemanticsFlag(_kIsCheckedIndex);


        /// Whether a semantics node is selected.
        ///
        /// If true, the semantics node is "selected". If false, the semantics node is
        /// "unselected".
        ///
        /// For example, the active tab in a tab bar has [isSelected] set to true.
        public static SemanticsFlag isSelected = new SemanticsFlag(_kIsSelectedIndex);

        /// Whether the semantic node represents a button.
        ///
        /// Platforms has special handling for buttons, for example Android's TalkBack
        /// and iOS's VoiceOver provides an additional hint when the focused object is
        /// a button.
        public static SemanticsFlag isButton = new SemanticsFlag(_kIsButtonIndex);

        /// Whether the semantic node represents a text field.
        ///
        /// Text fields are announced as such and allow text input via accessibility
        /// affordances.
        public static SemanticsFlag isTextField = new SemanticsFlag(_kIsTextFieldIndex);

        /// Whether the semantic node currently holds the user's focus.
        ///
        /// The focused element is usually the current receiver of keyboard inputs.
        public static SemanticsFlag isFocused = new SemanticsFlag(_kIsFocusedIndex);

        /// The semantics node has the quality of either being "enabled" or
        /// "disabled".
        ///
        /// For example, a button can be enabled or disabled and therefore has an
        /// "enabled" state. Static text is usually neither enabled nor disabled and
        /// therefore does not have an "enabled" state.
        public static SemanticsFlag hasEnabledState = new SemanticsFlag(_kHasEnabledStateIndex);

        /// Whether a semantic node that [hasEnabledState] is currently enabled.
        ///
        /// A disabled element does not respond to user interaction. For example, a
        /// button that currently does not respond to user interaction should be
        /// marked as disabled.
        public static SemanticsFlag isEnabled = new SemanticsFlag(_kIsEnabledIndex);

        /// Whether a semantic node is in a mutually exclusive group.
        ///
        /// For example, a radio button is in a mutually exclusive group because
        /// only one radio button in that group can be marked as [isChecked].
        public static SemanticsFlag isInMutuallyExclusiveGroup = new SemanticsFlag(_kIsInMutuallyExclusiveGroupIndex);

        /// Whether a semantic node is a header that divides content into sections.
        ///
        /// For example, headers can be used to divide a list of alphabetically
        /// sorted words into the sections A, B, C, etc. as can be found in many
        /// address book applications.
        public static SemanticsFlag isHeader = new SemanticsFlag(_kIsHeaderIndex);

        /// Whether the value of the semantics node is obscured.
        ///
        /// This is usually used for text fields to indicate that its content
        /// is a password or contains other sensitive information.
        public static SemanticsFlag isObscured = new SemanticsFlag(_kIsObscuredIndex);

        /// Whether the semantics node is the root of a subtree for which a route name
        /// should be announced.
        ///
        /// When a node with this flag is removed from the semantics tree, the
        /// framework will select the last in depth-first, paint order node with this
        /// flag.  When a node with this flag is added to the semantics tree, it is
        /// selected automatically, unless there were multiple nodes with this flag
        /// added.  In this case, the last added node in depth-first, paint order
        /// will be selected.
        ///
        /// From this selected node, the framework will search in depth-first, paint
        /// order for the first node with a [namesRoute] flag and a non-null,
        /// non-empty label. The [namesRoute] and [scopesRoute] flags may be on the
        /// same node. The label of the found node will be announced as an edge
        /// transition. If no non-empty, non-null label is found then:
        ///
        ///   * VoiceOver will make a chime announcement.
        ///   * TalkBack will make no announcement
        ///
        /// Semantic nodes annotated with this flag are generally not a11y focusable.
        ///
        /// This is used in widgets such as Routes, Drawers, and Dialogs to
        /// communicate significant changes in the visible screen.
        public static SemanticsFlag scopesRoute = new SemanticsFlag(_kScopesRouteIndex);

        /// Whether the semantics node label is the name of a visually distinct
        /// route.
        ///
        /// This is used by certain widgets like Drawers and Dialogs, to indicate
        /// that the node's semantic label can be used to announce an edge triggered
        /// semantics update.
        ///
        /// Semantic nodes annotated with this flag will still recieve a11y focus.
        ///
        /// Updating this label within the same active route subtree will not cause
        /// additional announcements.
        public static SemanticsFlag namesRoute = new SemanticsFlag(_kNamesRouteIndex);

        /// Whether the semantics node is considered hidden.
        ///
        /// Hidden elements are currently not visible on screen. They may be covered
        /// by other elements or positioned outside of the visible area of a viewport.
        ///
        /// Hidden elements cannot gain accessibility focus though regular touch. The
        /// only way they can be focused is by moving the focus to them via linear
        /// navigation.
        ///
        /// Platforms are free to completely ignore hidden elements and new platforms
        /// are encouraged to do so.
        ///
        /// Instead of marking an element as hidden it should usually be excluded from
        /// the semantics tree altogether. Hidden elements are only included in the
        /// semantics tree to work around platform limitations and they are mainly
        /// used to implement accessibility scrolling on iOS.
        public static SemanticsFlag isHidden = new SemanticsFlag(_kIsHiddenIndex);

        /// Whether the semantics node represents an image.
        ///
        /// Both TalkBack and VoiceOver will inform the user the the semantics node
        /// represents an image.
        public static SemanticsFlag isImage = new SemanticsFlag(_kIsImageIndex);

        /// Whether the semantics node is a live region.
        ///
        /// A live region indicates that updates to semantics node are important.
        /// Platforms may use this information to make polite announcements to the
        /// user to inform them of updates to this node.
        ///
        /// An example of a live region is a [SnackBar] widget. On Android, A live
        /// region causes a polite announcement to be generated automatically, even
        /// if the user does not have focus of the widget.
        public static SemanticsFlag isLiveRegion = new SemanticsFlag(_kIsLiveRegionIndex);

        /// The semantics node has the quality of either being "on" or "off".
        ///
        /// This flag is mutually exclusive with [hasCheckedState].
        ///
        /// For example, a switch has toggled state.
        ///
        /// See also:
        ///
        ///    * [SemanticsFlag.isToggled], which controls whether the node is "on" or "off".
        public static SemanticsFlag hasToggledState = new SemanticsFlag(_kHasToggledStateIndex);

        /// If true, the semantics node is "on". If false, the semantics node is
        /// "off".
        ///
        /// For example, if a switch is in the on position, [isToggled] is true.
        ///
        /// See also:
        ///
        ///   * [SemanticsFlag.hasToggledState], which enables a toggled state.
        public static SemanticsFlag isToggled = new SemanticsFlag(_kIsToggledIndex);

        /// Whether the platform can scroll the semantics node when the user attempts
        /// to move focus to an offscreen child.
        ///
        /// For example, a [ListView] widget has implicit scrolling so that users can
        /// easily move to the next visible set of children. A [TabBar] widget does
        /// not have implicit scrolling, so that users can navigate into the tab
        /// body when reaching the end of the tab bar.
        public static SemanticsFlag hasImplicitScrolling = new SemanticsFlag(_kHasImplicitScrollingIndex);

        /// The possible semantics flags.
        ///
        /// The map's key is the [index] of the flag and the value is the flag itself.
        public static Dictionary<int, SemanticsFlag> values = new Dictionary<int, SemanticsFlag>{
    { _kHasCheckedStateIndex, hasCheckedState},
    { _kIsCheckedIndex, isChecked},
    { _kIsSelectedIndex, isSelected},
    { _kIsButtonIndex, isButton},
    { _kIsTextFieldIndex, isTextField},
    { _kIsFocusedIndex, isFocused},
    { _kHasEnabledStateIndex, hasEnabledState},
    { _kIsEnabledIndex, isEnabled},
    { _kIsInMutuallyExclusiveGroupIndex, isInMutuallyExclusiveGroup},
    { _kIsHeaderIndex, isHeader},
    { _kIsObscuredIndex, isObscured},
    { _kScopesRouteIndex, scopesRoute},
    { _kNamesRouteIndex, namesRoute},
    { _kIsHiddenIndex, isHidden},
    { _kIsImageIndex, isImage},
    { _kIsLiveRegionIndex, isLiveRegion},
    { _kHasToggledStateIndex, hasToggledState},
    { _kIsToggledIndex, isToggled},
    { _kHasImplicitScrollingIndex, hasImplicitScrolling},
  };

        public String toString()
        {
            switch (index)
            {
                case _kHasCheckedStateIndex:
                    return "SemanticsFlag.hasCheckedState";
                case _kIsCheckedIndex:
                    return "SemanticsFlag.isChecked";
                case _kIsSelectedIndex:
                    return "SemanticsFlag.isSelected";
                case _kIsButtonIndex:
                    return "SemanticsFlag.isButton";
                case _kIsTextFieldIndex:
                    return "SemanticsFlag.isTextField";
                case _kIsFocusedIndex:
                    return "SemanticsFlag.isFocused";
                case _kHasEnabledStateIndex:
                    return "SemanticsFlag.hasEnabledState";
                case _kIsEnabledIndex:
                    return "SemanticsFlag.isEnabled";
                case _kIsInMutuallyExclusiveGroupIndex:
                    return "SemanticsFlag.isInMutuallyExclusiveGroup";
                case _kIsHeaderIndex:
                    return "SemanticsFlag.isHeader";
                case _kIsObscuredIndex:
                    return "SemanticsFlag.isObscured";
                case _kScopesRouteIndex:
                    return "SemanticsFlag.scopesRoute";
                case _kNamesRouteIndex:
                    return "SemanticsFlag.namesRoute";
                case _kIsHiddenIndex:
                    return "SemanticsFlag.isHidden";
                case _kIsImageIndex:
                    return "SemanticsFlag.isImage";
                case _kIsLiveRegionIndex:
                    return "SemanticsFlag.isLiveRegion";
                case _kHasToggledStateIndex:
                    return "SemanticsFlag.hasToggledState";
                case _kIsToggledIndex:
                    return "SemanticsFlag.isToggled";
                case _kHasImplicitScrollingIndex:
                    return "SemanticsFlag.hasImplicitScrolling";
            }
            return null;
        }
    }

    /// An object that creates [SemanticsUpdate] objects.
    ///
    /// Once created, the [SemanticsUpdate] objects can be passed to
    /// [Window.updateSemantics] to update the semantics conveyed to the user.
    public class SemanticsUpdateBuilder : NativeFieldWrapperClass2
    {
        /// Creates an empty [SemanticsUpdateBuilder] object.
        //@pragma('vm:entry-point')
        public SemanticsUpdateBuilder() { _constructor(); }
        void _constructor()
        {
            // native 'SemanticsUpdateBuilder_constructor';
        }

        /// Update the information associated with the node with the given `id`.
        ///
        /// The semantics nodes form a tree, with the root of the tree always having
        /// an id of zero. The `childrenInTraversalOrder` and `childrenInHitTestOrder`
        /// are the ids of the nodes that are immediate children of this node. The
        /// former enumerates children in traversal order, and the latter enumerates
        /// the same children in the hit test order. The two lists must have the same
        /// length and contain the same ids. They may only differ in the order the
        /// ids are listed in. For more information about different child orders, see
        /// [DebugSemanticsDumpOrder].
        ///
        /// The system retains the nodes that are currently reachable from the root.
        /// A given update need not contain information for nodes that do not change
        /// in the update. If a node is not reachable from the root after an update,
        /// the node will be discarded from the tree.
        ///
        /// The `flags` are a bit field of [SemanticsFlag]s that apply to this node.
        ///
        /// The `actions` are a bit field of [SemanticsAction]s that can be undertaken
        /// by this node. If the user wishes to undertake one of these actions on this
        /// node, the [Window.onSemanticsAction] will be called with `id` and one of
        /// the possible [SemanticsAction]s. Because the semantics tree is maintained
        /// asynchronously, the [Window.onSemanticsAction] callback might be called
        /// with an action that is no longer possible.
        ///
        /// The `label` is a string that describes this node. The `value` property
        /// describes the current value of the node as a string. The `increasedValue`
        /// string will become the `value` string after a [SemanticsAction.increase]
        /// action is performed. The `decreasedValue` string will become the `value`
        /// string after a [SemanticsAction.decrease] action is performed. The `hint`
        /// string describes what result an action performed on this node has. The
        /// reading direction of all these strings is given by `textDirection`.
        ///
        /// The fields 'textSelectionBase' and 'textSelectionExtent' describe the
        /// currently selected text within `value`.
        ///
        /// For scrollable nodes `scrollPosition` describes the current scroll
        /// position in logical pixel. `scrollExtentMax` and `scrollExtentMin`
        /// describe the maximum and minimum in-rage values that `scrollPosition` can
        /// be. Both or either may be infinity to indicate unbound scrolling. The
        /// value for `scrollPosition` can (temporarily) be outside this range, for
        /// example during an overscroll. `scrollChildren` is the count of the
        /// total number of child nodes that contribute semantics and `scrollIndex`
        /// is the index of the first visible child node that contributes semantics.
        ///
        /// The `rect` is the region occupied by this node in its own coordinate
        /// system.
        ///
        /// The `transform` is a matrix that maps this node's coordinate system into
        /// its parent's coordinate system.
        public void updateNode(
            int id = 0,
          int flags = 0,
          int actions = 0,
          int textSelectionBase = 0,
          int textSelectionExtent = 0,
          int scrollChildren = 0,
          int scrollIndex = 0,
          double scrollPosition = 0.0,
          double scrollExtentMax = 0.0,
          double scrollExtentMin = 0.0,
            Rect rect = null,
            String label = null,
            String hint = null,
            String value = null,
            String increasedValue = null,
            String decreasedValue = null,
            TextDirection textDirection = TextDirection.ltr,
            List<double> transform = null,
            List<int> childrenInTraversalOrder = null,
            List<int> childrenInHitTestOrder = null,
            //[Obsolete("use additionalActions instead")]
            List<int> customAcccessibilityActions = null,
            List<int> additionalActions = null)
        {
            if (transform.Count != 16)
                throw new ArgumentException("transform argument must have 16 entries.");
            _updateNode(
              id,
              flags,
              actions,
              textSelectionBase,
              textSelectionExtent,
              scrollChildren,
              scrollIndex,
              scrollPosition,
              scrollExtentMax,
              scrollExtentMin,
              rect.left,
              rect.top,
              rect.right,
              rect.bottom,
              label,
              hint,
              value,
              increasedValue,
              decreasedValue,
              (int)textDirection,
              transform,
              childrenInTraversalOrder,
              childrenInHitTestOrder,
              additionalActions ?? customAcccessibilityActions);
        }
        void _updateNode(
           int id,
           int flags,
           int actions,
           int textSelectionBase,
           int textSelectionExtent,
           int scrollChildren,
           int scrollIndex,
           double scrollPosition,
           double scrollExtentMax,
           double scrollExtentMin,
           double left,
           double top,
           double right,
           double bottom,
           String label,
           String hint,
           String value,
           String increasedValue,
           String decreasedValue,
           int textDirection,
           List<double> transform,
           List<int> childrenInTraversalOrder,
           List<int> childrenInHitTestOrder,
           List<int> additionalActions)
        {
            // native 'SemanticsUpdateBuilder_updateNode';
        }

        /// Update the custom semantics action associated with the given `id`.
        ///
        /// The name of the action exposed to the user is the `label`. For overriden
        /// standard actions this value is ignored.
        ///
        /// The `hint` should describe what happens when an action occurs, not the
        /// manner in which a tap is accomplished. For example, use "delete" instead
        /// of "double tap to delete".
        ///
        /// The text direction of the `hint` and `label` is the same as the global
        /// window.
        ///
        /// For overriden standard actions, `overrideId` corresponds with a
        /// [SemanticsAction.index] value. For custom actions this argument should not be
        /// provided.
        void updateCustomAction(int id = 0, String label = "", String hint = "", int overrideId = -1)
        {
            //assert(id != null);
            //assert(overrideId != null);
            _updateCustomAction(id, label, hint, overrideId);
        }
        void _updateCustomAction(int id, String label, String hint, int overrideId)
        {
            // native 'SemanticsUpdateBuilder_updateCustomAction';
        }

        /// Creates a [SemanticsUpdate] object that encapsulates the updates recorded
        /// by this object.
        ///
        /// The returned object can be passed to [Window.updateSemantics] to actually
        /// update the semantics retained by the system.
        public SemanticsUpdate build()
        {
            // native 'SemanticsUpdateBuilder_build';
            return null; // Tmp to resolve build
        }

    }

    /// An opaque object representing a batch of semantics updates.
    ///
    /// To create a SemanticsUpdate object, use a [SemanticsUpdateBuilder].
    ///
    /// Semantics updates can be applied to the system's retained semantics tree
    /// using the [Window.updateSemantics] method.
    public class SemanticsUpdate : NativeFieldWrapperClass2
    {
        /// This class is created by the engine, and should not be instantiated
        /// or extended directly.
        ///
        /// To create a SemanticsUpdate object, use a [SemanticsUpdateBuilder].
        //@pragma('vm:entry-point')
        //SemanticsUpdate();

        /// Releases the resources used by this semantics update.
        ///
        /// After calling this function, the semantics update is cannot be used
        /// further.
        void dispose()
        {
            // native 'SemanticsUpdate_dispose';
        }
    }
}
