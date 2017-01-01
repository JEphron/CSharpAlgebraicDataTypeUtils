using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace JME.UnionTypes.Tests
{
    [TestFixture]
    public class TestMaybe
    {
        [Test]
        public void TestNonePrimitiveIsNone()
        {
            var maybeInt = new Maybe<int>();
            Assert.That(maybeInt.IsNone);
        }

        [Test]
        public void TestNonePrimativeIsNotSome()
        {
            var maybeInt = new Maybe<int>();
            Assert.That(maybeInt.IsSome, Is.False);
        }

        [Test]
        public void TestSomePrimativeIsSome()
        {
            var maybeInt = new Maybe<int>(1);
            Assert.That(maybeInt.IsSome);
        }

        [Test]
        public void TestSomePrimativeIsNotNone()
        {
            var maybeInt = new Maybe<int>(1);
            Assert.That(maybeInt.IsNone, Is.False);
        }

        [Test]
        public void TestNoneObjectIsNone()
        {
            var maybeList = new Maybe<List<int>>();
            Assert.That(maybeList.IsNone);
        }

        [Test]
        public void TestNoneObjectIsNotSome()
        {
            var maybeList = new Maybe<List<int>>();
            Assert.That(maybeList.IsSome, Is.False);
        }

        [Test]
        public void TestSomeObjectIsSome()
        {
            var maybeList = new Maybe<List<int>>(new List<int> {1, 2, 3});
            Assert.That(maybeList.IsSome);
        }

        [Test]
        public void TestSomeObjectIsNotNone()
        {
            var maybeList = new Maybe<List<int>>(new List<int> {1, 2, 3});
            Assert.That(maybeList.IsNone, Is.False);
        }

        [Test]
        public void TestNullIsNone()
        {
            var maybeList = new Maybe<List<int>>(null);
            Assert.That(maybeList.IsNone);
        }

        [Test]
        public void TestMatchSomePrimativeTriggersHandlerImmediately()
        {
            var maybeInt = new Maybe<int>(10);
            maybeInt.Match(some: i => Assert.Pass(), none: Assert.Pass);
            Assert.Fail();
        }

        [Test]
        public void TestMatchNonePrimativeTriggersHandlerImmediately()
        {
            var maybeInt = new Maybe<int>();
            maybeInt.Match(some: i => Assert.Pass(), none: Assert.Pass);
            Assert.Fail();
        }

        [Test]
        public void TestMatchNonePrimativeTriggersNoneHandler()
        {
            var maybeInt = new Maybe<int>();
            maybeInt.Match(some: i => Assert.Fail(), none: Assert.Pass);
        }

        [Test]
        public void TestMatchSomePrimativeTriggersSomeHandler()
        {
            var maybeInt = new Maybe<int>(1);
            maybeInt.Match(some: i => Assert.Pass(), none: Assert.Fail);
        }

        [Test]
        public void TestMatchSomePrimativeReturnsSome()
        {
            var maybeInt = new Maybe<int>(1);
            var match = maybeInt.Match(some: i => 1, none: () => 0);
            Assert.That(match, Is.EqualTo(1));
        }

        [Test]
        public void TestMatchNonePrimativeReturnsNone()
        {
            var maybeInt = new Maybe<int>();
            var match = maybeInt.Match(some: i => 1, none: () => 0);
            Assert.That(match, Is.EqualTo(0));
        }

        [Test]
        public void TestMatchNoneObjectTriggersNoneHandlerImmediately()
        {
            var maybeList = new Maybe<List<int>>();
            maybeList.Match(some: i => Assert.Fail(), none: Assert.Pass);
        }

        [Test]
        public void TestMatchSomeObjectTriggersSomeHandlerImmediately()
        {
            var maybeList = new Maybe<List<int>>();
            maybeList.Match(some: i => Assert.Fail(), none: Assert.Pass);
        }

        [Test]
        public void TestMatchSomeObjectReturnsSome()
        {
            var maybeList = new Maybe<List<int>>(new List<int> {1, 2, 3});
            var list1 = new List<string>() {"a", "b"};
            var list2 = new List<string>() {"1", "2"};
            var match = maybeList.Match(some: i => list1, none: () => list2);
            Assert.That(match, Is.EqualTo(list1));
        }

        [Test]
        public void TestMatchNoneObjectReturnsNone()
        {
            var maybeList = new Maybe<List<int>>();
            var list1 = new List<string> {"a", "b"};
            var list2 = new List<string> {"1", "2"};
            var match = maybeList.Match(some: i => list1, none: () => list2);
            Assert.That(match, Is.EqualTo(list2));
        }

        [Test]
        public void TestMatchSomeObjectUsingAFunctionForSomeAndAValueForNoneReturnsTheResultOfTheFunction()
        {
            var list = new List<int> {1, 2, 3};
            var maybeList = new Maybe<List<int>>(list);
            const string noneValue = "none";
            const string someValue = "some";
            Func<List<int>, string> someFunction = (lst) => someValue;
            var match = maybeList.Match(some: someFunction, none: noneValue);
            Assert.That(match, Is.EqualTo(someFunction(list)));
        }

        [Test]
        public void TestMatchSomeObjectUsingAValueForSomeAndAFunctionForNoneReturnsTheValue()
        {
            var maybeList = new Maybe<List<int>>(new List<int> {1, 2, 3});
            const string someValue = "some";
            Func<string> noneFunction = () =>
            {
                Assert.Fail();
                return "none";
            };
            var match = maybeList.Match(some: someValue, none: noneFunction);
            Assert.That(match, Is.EqualTo(someValue));
        }

        [Test]
        public void TestMatchNoneObjectUsingAValueForSomeAndAFunctionForNoneReturnsTheResultOfTheFunction()
        {
            var maybeList = new Maybe<List<int>>();
            const string someValue = "some";
            const string noneValue = "none";
            Func<string> noneFunction = () => noneValue;
            var match = maybeList.Match(some: someValue, none: noneFunction);
            Assert.That(match, Is.EqualTo(noneFunction()));
        }

        [Test]
        public void TestMatchNoneObjectUsingAFunctionForSomeAndAValueForNoneReturnsTheValue()
        {
            var maybeList = new Maybe<List<int>>();
            const string noneValue = "none";
            Func<List<int>, string> someFunction = lst =>
            {
                Assert.Fail();
                return "some";
            };
            var match = maybeList.Match(some: someFunction, none: noneValue);
            Assert.That(match, Is.EqualTo(noneValue));
        }

        [Test]
        public void TestMatchSomeObjectUsingAFunctionForSomeAndAValueForNoneCallsTheFunctionImmediately()
        {
            var maybeList = new Maybe<List<int>>(new List<int> {1, 2, 3});
            maybeList.Match(some: list => Assert.Pass(), none: Assert.Fail);
        }

        [Test]
        public void TestMatchSomeObjectUsingAValueForSomeAndAValueForNoneReturnsTheValueForSome()
        {
            var maybeList = new Maybe<List<int>>(new List<int> {1, 2, 3});
            const string someValue = "some";
            const string noneValue = "none";
            var match = maybeList.Match(some: someValue, none: noneValue);
            Assert.That(match, Is.EqualTo(someValue));
        }

        [Test]
        public void TestMatchNoneObjectUsingAValueForSomeAndAValueForNoneReturnsTheValueForNone()
        {
            var maybeList = new Maybe<List<int>>();
            const string someValue = "some";
            const string noneValue = "none";
            var match = maybeList.Match(some: someValue, none: noneValue);
            Assert.That(match, Is.EqualTo(noneValue));
        }

        [Test]
        public void TestMapReturnsSomeWhenCalledOnSome()
        {
            var maybeList = new Maybe<List<string>>(new List<string> {"a", "b"});
            var mapResult = maybeList.Map(list => list[0]);
            Assert.That(mapResult.IsSome);
        }

        [Test]
        public void TestMapReturnsNoneWhenCalledOnNone()
        {
            var maybeList = new Maybe<List<string>>();
            var mapResult = maybeList.Map(list => list[0]);
            Assert.That(mapResult.IsNone);
        }

        [Test]
        public void TestOkayOrReturnsOkayWhenCalledOnSome()
        {
            var errorValue = "error";
            var someValue = new List<string> {"foo", "bar"};
            var maybeList = new Maybe<List<string>>(someValue);
            var okayOr = maybeList.OkayOr(errorValue);
            okayOr.Match(list => Assert.Pass(), err => Assert.Fail());
        }

        [Test]
        public void TestOkayOrReturnsErrorWhenCalledOnNone()
        {
            var errorValue = "error";
            var maybeList = new Maybe<List<string>>();
            var okayOr = maybeList.OkayOr(errorValue);
            okayOr.Match(list => Assert.Fail(), err => Assert.Pass());
        }

        [Test]
        public void TestValueOrReturnsSomeWhenCalledOnSomeAndPassedAValue()
        {
            const string some = "foo";
            const string alternative = "bar";
            var maybeString = new Maybe<string>(some);
            var result = maybeString.ValueOr(alternative);
            Assert.That(result, Is.EqualTo(some));
        }

        [Test]
        public void TestValueOrReturnsParameterWhenCalledOnNoneAndPassedAValue()
        {
            const string alternative = "bar";
            var maybeString = new Maybe<string>();
            var result = maybeString.ValueOr(alternative);
            Assert.That(result, Is.EqualTo(alternative));
        }

        [Test]
        public void TestValueOrReturnsSomeWhenCalledOnSomeAndPassedAFunction()
        {
            const string some = "foo";
            var maybeString = new Maybe<string>(some);
            var result = maybeString.ValueOr(() => "abc");
            Assert.That(result, Is.EqualTo(some));
        }

        [Test]
        public void TestValueOrReturnsParameterWhenCalledOnNoneAndPassedAFunction()
        {
            Func<string> alternative = () => "bar";
            var maybeString = new Maybe<string>();
            var result = maybeString.ValueOr(alternative);
            Assert.That(result, Is.EqualTo(alternative()));
        }

        [Test]
        public void TestOrDefaultReturnsSomeWhenCalledOnSome()
        {
            const string some = "foo";
            var maybeString = new Maybe<string>(some);
            var result = maybeString.OrDefault();
            Assert.That(result, Is.EqualTo(some));
        }

        [Test]
        public void TestOrDefaultReturnsDefaultWhenCalledOnNoneObject()
        {
            var maybeString = new Maybe<string>();
            var result = maybeString.OrDefault();
            Assert.That(result, Is.EqualTo(default(string)));
        }

        [Test]
        public void TestOrDefaultReturnsDefaultWhenCalledOnNonePrimative()
        {
            var maybeInt = new Maybe<int>();
            var result = maybeInt.OrDefault();
            Assert.That(result, Is.EqualTo(default(int)));
        }

        [Test]
        public void TestAsNoneReturnsNoneWhenCalledOnSome()
        {
            var maybeInt = new Maybe<int>(99);
            Assert.That(maybeInt.AsNone().IsNone);
        }

        [Test]
        public void TestAsNoneReturnsNoneWhenCalledOnNone()
        {
            var maybeInt = new Maybe<int>();
            Assert.That(maybeInt.AsNone().IsNone);
        }

        [Test]
        public void TestAsSomeReturnsSomeWhenCalledOnSome()
        {
            var maybeInt = new Maybe<int>(99);
            var result = maybeInt.AsSome(66).OrDefault();
            Assert.That(result, Is.EqualTo(66));
        }

        [Test]
        public void TestAsSomeReturnsSomeWhenCalledOnNone()
        {
            var maybeInt = new Maybe<int>();
            var result = maybeInt.AsSome(66).OrDefault();
            Assert.That(result, Is.EqualTo(66));
        }

        [Test]
        public void TestAndReturnsNoneWhenCalledOnNone()
        {
            var maybeString = new Maybe<string>();
            var result = maybeString.And(new Maybe<string>("foo"));
            Assert.That(result.IsNone);
        }

        [Test]
        public void TestAndReturnsArgumentWhenCalledOnSome()
        {
            var maybeString = new Maybe<string>("foo");
            var result = maybeString.And(new Maybe<string>("bar"));
            Assert.That(result.OrDefault(), Is.EqualTo("bar"));
        }

        [Test]
        public void TestOrReturnsSelfWhenCalledOnSome()
        {
            var maybeString = new Maybe<string>("foo");
            var result = maybeString.Or(new Maybe<string>("bar"));
            Assert.That(result.OrDefault(), Is.EqualTo("foo"));
        }

        [Test]
        public void TestOrReturnsArgumentWhenCalledOnNone()
        {
            var maybeString = new Maybe<string>();
            var result = maybeString.Or(new Maybe<string>("bar"));
            Assert.That(result.OrDefault(), Is.EqualTo("bar"));
        }

        [Test]
        public void TestAndThenReturnsTheValueOfTheFunctionWhenCalledOnSome()
        {
            var maybeString = new Maybe<string>("foo");
            var result = maybeString.AndThen(s => new Maybe<string>(s + "bar"));
            Assert.That(result.OrDefault(), Is.EqualTo("foobar"));
        }

        [Test]
        public void TestAndThenReturnsNoneWhenCalledOnNone()
        {
            var maybeString = new Maybe<string>();
            var result = maybeString.AndThen(s => new Maybe<string>("bar"));
            Assert.That(result.IsNone);
        }

        [Test]
        public void TestOrElseReturnsSelfWhenCalledOnSome()
        {
            var maybeString = new Maybe<string>("foo");
            var result = maybeString.OrElse(() => new Maybe<string>("bar"));
            Assert.That(result.OrDefault(), Is.EqualTo("foo"));
        }

        [Test]
        public void TestOrElseReturnsTheValueOfTheFunctionWhenCalledOnNone()
        {
            var maybeString = new Maybe<string>();
            var result = maybeString.OrElse(() => new Maybe<string>("bar"));
            Assert.That(result.OrDefault(), Is.EqualTo("bar"));
        }
    }
}