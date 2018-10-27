using System;
using System.Threading.Tasks;

namespace FlutterBinding.Mapping
{
    public class Future<T> : Task<T>
    {
        public Future(Func<T> function) : base(function) { }
    }


    public class Future : Task
    {
        public Future(Action action) : base(action) { }
    }

}