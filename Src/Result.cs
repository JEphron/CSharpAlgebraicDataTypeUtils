using System;
using JetBrains.Annotations;

namespace JME.UnionTypes
{
    public class Result<TOkay, TErr>
    {
        private readonly TErr _err;
        private readonly TOkay _okay;

        public Result([NotNull] TOkay okay)
        {
            if (okay == null) throw new ArgumentException("Argument to Result must not be null");
            _okay = okay;
        }

        public Result([NotNull] TErr err)
        {
            if (err == null) throw new ArgumentException("Argument to Result must not be null");
            _err = err;
        }

        public void Match([InstantHandle] Action<TOkay> okay, [InstantHandle] Action<TErr> err)
        {
            if (_err != null)
            {
                err(_err);
            }
            else
            {
                okay(_okay);
            }
        }

        public TReturn Match<TReturn>([InstantHandle] Func<TOkay, TReturn> okay, [InstantHandle] Func<TErr, TReturn> err)
        {
            return _err != null ? err(_err) : okay(_okay);
        }
    }
}