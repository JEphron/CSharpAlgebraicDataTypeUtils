using System;
using System.Collections.Generic;
using JetBrains.Annotations;
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
            Assert.IsTrue(maybeInt.IsNone());
        }

        [Test]
        public void TestNonePrimativeIsNotSome()
        {
            var maybeInt = new Maybe<int>();
            Assert.IsFalse(maybeInt.IsSome());
        }

        [Test]
        public void TestSomePrimativeIsSome()
        {
            var maybeInt = new Maybe<int>(1);
            Assert.IsTrue(maybeInt.IsSome());
        }

        [Test]
        public void TestSomePrimativeIsNotNone()
        {
            var maybeInt = new Maybe<int>(1);
            Assert.IsFalse(maybeInt.IsNone());
        }

        [Test]
        public void TestNoneObjectIsNone()
        {
            var maybeList = new Maybe<List<int>>();
            Assert.IsTrue(maybeList.IsNone());
        }

        [Test]
        public void TestNoneObjectIsNotSome()
        {
            var maybeList = new Maybe<List<int>>();
            Assert.IsFalse(maybeList.IsSome());
        }

        [Test]
        public void TestSomeObjectIsSome()
        {
            var maybeList = new Maybe<List<int>>(new List<int> {1, 2, 3});
            Assert.IsTrue(maybeList.IsSome());
        }

        [Test]
        public void TestSomeObjectIsNotNone()
        {
            var maybeList = new Maybe<List<int>>(new List<int> {1, 2, 3});
            Assert.IsFalse(maybeList.IsNone());
        }

        [Test]
        public void TestNullIsNone()
        {
            var maybeList = new Maybe<List<int>>(null);
            Assert.IsTrue(maybeList.IsNone());
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
            Assert.That(maybeInt.IsSome);
            var match = maybeInt.Match(some: i => 1, none: () => 0);
            Assert.That(match, Is.EqualTo(1));
        }

        [Test]
        public void TestMatchNonePrimativeReturnsNone()
        {
            var maybeInt = new Maybe<int>();
            Assert.That(maybeInt.IsNone);
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
            var list1 = new List<string>() { "a", "b"};
            var list2 = new List<string>() {"1", "2"};
            var match = maybeList.Match(some: i => list1, none: () => list2);
            Assert.That(match, Is.EqualTo(list1));
        }

        [Test]
        public void TestMatchNoneObjectReturnsNone()
        {
            var maybeList = new Maybe<List<int>>();
            var list1 = new List<string> { "a", "b" };
            var list2 = new List<string> { "1", "2" };
            var match = maybeList.Match(some: i => list1, none: () => list2);
            Assert.That(match, Is.EqualTo(list2));
        }

        [Test]
        public void TestMatchSomeObjectUsingAFunctionForSomeAndAValueForNoneReturnsTheResultOfTheFunction()
        {
            var list = new List<int> { 1, 2, 3 };
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
            var maybeList = new Maybe<List<int>>(new List<int> { 1, 2, 3 });
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
            var maybeList = new Maybe<List<int>>(new List<int> { 1, 2, 3 });
            maybeList.Match(some: list => Assert.Pass(), none: Assert.Fail);
        }

        [Test]
        public void TestMatchSomeObjectUsingAValueForSomeAndAValueForNoneReturnsTheValueForSome()
        {
            var maybeList = new Maybe<List<int>>(new List<int> {1,2,3});
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
            var maybeList = new Maybe<List<string>>(new List<string>{"a", "b"});
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
            var someValue = new List<string>{"foo", "bar"};
            var maybeList = new Maybe<List<string>>(someValue);
            var okayOr = maybeList.OkayOr(errorValue);
            okayOr.Match(list => Assert.Pass(), err => Assert.Fail());
        }

        [Test]
        public void TestOkayOrReturnsErrorWhenCalledOnNone()
        {
            var errorValue = "error";
            var someValue = new List<string>{"foo", "bar"};
            var maybeList = new Maybe<List<string>>(someValue);
            var okayOr = maybeList.OkayOr(errorValue);
            okayOr.Match(list => Assert.Fail(), err => Assert.Pass());
        }
    }
}