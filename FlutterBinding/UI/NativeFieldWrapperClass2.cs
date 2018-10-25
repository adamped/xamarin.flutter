using FlutterBinding.Mapping;
using System;

namespace FlutterBinding.UI
{
    public class NativeFieldWrapperClass2
    {

        protected Future<T> _futurize<T>(Action<_Callback<T>> callback)
        {
            // Question, why is this so complicated for running a new Task.
            // Could be a Dart -> C# translation issue

            var result = default(T);
            
            var resolve = new _Callback<T>((t) => { result = t; });
            
            return new Future<T>(() => { return result; });
        }

    }
}
