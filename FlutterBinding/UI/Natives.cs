﻿using System;
using System.Collections.Generic;
using System.Text;

namespace FlutterBinding.UI
{

    // NOTE:
    // I'm commenting this all out because it seems like Dart related functions we don't need
    // However the Dart Schedule Frame extension, I am suspecting is the hook that the IDE
    // uses to schedule a frame, e.g. trigger a hot reload.

    //// Corelib 'print' implementation.
    //void _print(dynamic arg)
    //    {
    //        _Logger._printString(arg.toString());
    //    }

    //    class _Logger
    //    {
    //        static void _printString(String s)
    //        {
    //            // native 'Logger_PrintString';
    //        }
    //}

    //    // A service protocol extension to schedule a frame to be rendered into the
    //    // window.
    //    Task<developer.ServiceExtensionResponse> _scheduleFrame(
    //        String method,
    //        Map<String, String> parameters
    //        ) async {
    //  // Schedule the frame.
    //  window.scheduleFrame();
    //  // Always succeed.
    //  return new developer.ServiceExtensionResponse.result(json.encode(<String, String>{
    //    'type': 'Success',
    //  }));
    //}

    //////@pragma('vm:entry-point')
    //void _setupHooks()
    //{
    //    assert(() {
    //        // In debug mode, register the schedule frame extension.
    //        developer.registerExtension('ext.ui.window.scheduleFrame', _scheduleFrame);
    //        return true;
    //    } ());
    //}

    //void saveCompilationTrace(String filePath)
    //{
    //    final dynamic result = _saveCompilationTrace();
    //    if (result is Error)
    //        throw result;

    //    final File file = new File(filePath);
    //    file.writeAsBytesSync(result);
    //}

    //dynamic _saveCompilationTrace() native 'SaveCompilationTrace';

    //void _scheduleMicrotask(void callback()) native 'ScheduleMicrotask';

    //int _getCallbackHandle(Function closure) native 'GetCallbackHandle';
    //Function _getCallbackFromHandle(int handle) native 'GetCallbackFromHandle';

    //// Required for gen_snapshot to work correctly.
    //int _isolateId;

    //////@pragma('vm:entry-point')
    //Function _getPrintClosure() => _print;
    //////@pragma('vm:entry-point')
    //Function _getScheduleMicrotaskClosure() => _scheduleMicrotask;

    //// Though the "main" symbol is not included in any of the libraries imported
    //// above, the builtin library will be included manually during VM setup. This
    //// symbol is only necessary for precompilation. It is marked as a stanalone
    //// entry point into the VM. This prevents the precompiler from tree shaking
    //// away "main".
    //////@pragma('vm:entry-point')
    //Function _getMainClosure() => main;
}
