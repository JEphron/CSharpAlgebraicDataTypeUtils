using System;

namespace JME.UnionTypes
{
    public sealed class Union3<T1, T2, T3>
    {
        private readonly Maybe<T1> _t1;
        private readonly Maybe<T2> _t2;
        private readonly Maybe<T3> _t3;

        public Union3(T1 v)
        {
            _t1 = new Maybe<T1>(v);
        }

        public Union3(T2 v)
        {
            _t2 = new Maybe<T2>(v);
        }

        public Union3(T3 v)
        {
            _t3 = new Maybe<T3>(v);
        }

        public void Match(Action<T1> f1, Action<T2> f2, Action<T3> f3)
        {
            _t1.Match(some: f1, none: () =>
            {
                _t2.Match(some: f2, none: () =>
                {
                    _t3.Match(some: f3, none: () =>
                    {

                    });
                });
            });

        }

        public TResult Match<TResult>(Func<T1, TResult> f1, Func<T2, TResult> f2, Func<T3, TResult> f3)
        {
            return _t1.Match(
                some: f1,
                none: () =>
                {
                    return _t2.Match(
                        some: f2,
                        none: () =>
                        {
                            return _t3.Match(
                                some: t => f3(t),
                                none: () => { throw new Exception("No match in Union3"); });
                        });
                });
        }
    }
}