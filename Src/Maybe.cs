using System;
using JetBrains.Annotations;

namespace JME.UnionTypes
{
    public class Maybe<T>
    {
        private readonly T _t;
        private readonly bool _isSome;

        public Maybe(T t)
        {
            _isSome = t != null;
            _t = t;
        }

        public Maybe()
        {
            _isSome = false;
        }

        public bool IsSome
        {
            get { return _isSome; }
        }

        public bool IsNone
        {
            get { return !IsSome; }
        }

        public void Match([InstantHandle] Action<T> some, [InstantHandle] Action none)
        {
            if (IsSome)
                some(_t);
            else
                none();
        }

        public TReturn Match<TReturn>([InstantHandle] Func<T, TReturn> some, [InstantHandle] Func<TReturn> none)
        {
            return IsSome ? some(_t) : none();
        }

        public TReturn Match<TReturn>(TReturn some, [InstantHandle] Func<TReturn> none)
        {
            return IsSome ? some : none();
        }

        public TReturn Match<TReturn>([InstantHandle] Func<T, TReturn> some, TReturn none)
        {
            return IsSome ? some(_t) : none;
        }

        [Pure]
        public TReturn Match<TReturn>(TReturn some, TReturn none)
        {
            return IsSome ? some : none;
        }

        public Maybe<TConverted> Map<TConverted>([InstantHandle] Func<T, TConverted> fn)
        {
            return Match(
                some: x => new Maybe<TConverted>(fn(x)),
                none: () => new Maybe<TConverted>());
        }

        [Pure]
        public Result<T, TErr> OkayOr<TErr>(TErr err)
        {
            return Match(
                some: x => Result<T, TErr>.Okay(x),
                none: () => Result<T, TErr>.Err(err));
        }

        [Pure]
        public T ValueOr(T defaultValue)
        {
            return IsSome ? _t : defaultValue;
        }

        public T ValueOr([InstantHandle] Func<T> action)
        {
            return IsSome ? _t : action();
        }

        [Pure]
        [CanBeNull]
        public T OrDefault()
        {
            return IsSome ? _t : default(T);
        }

        [Pure]
        public Maybe<T> AsNone()
        {
            return new Maybe<T>();
        }

        [Pure]
        public Maybe<T> AsSome(T some)
        {
            return new Maybe<T>(some);
        }

        public override string ToString()
        {
            return string.Format("Maybe<{0}> [{1}]", typeof(T), IsSome ? string.Format("Some({0})", _t) : "None()");
        }

        public Maybe<T> And(Maybe<T> other)
        {
            return Match(some: other, none: this);
        }

        public Maybe<T> AndThen(Func<T, Maybe<T>> func)
        {
            return Match(some: func, none: this);
        }

        public Maybe<T> Or(Maybe<T> other)
        {
            return Match(some: this, none: other);
        }

        public Maybe<T> OrElse(Func<Maybe<T>> func)
        {
            return Match(some: this, none: func);
        }
    }
}