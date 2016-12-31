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
            Maybe<int> maybeInt = new Maybe<int>();
            Assert.IsTrue(maybeInt.IsNone());
        }

        [Test]
        public void TestNonePrimativeIsNotSome()
        {
            Maybe<int> maybeInt = new Maybe<int>();
            Assert.IsFalse(maybeInt.IsSome());
        }

        [Test]
        public void TestSomePrimativeIsSome()
        {
            Maybe<int> maybeInt = new Maybe<int>(1);
            Assert.IsTrue(maybeInt.IsSome());
        }

        [Test]
        public void TestSomePrimativeIsNotNone()
        {
            Maybe<int> maybeInt = new Maybe<int>(1);
            Assert.IsFalse(maybeInt.IsNone());
        }

        [Test]
        public void TestNoneObjectIsNone()
        {
            Maybe<List<int>> maybeList = new Maybe<List<int>>();
            Assert.IsTrue(maybeList.IsNone());
        }

        [Test]
        public void TestNoneObjectIsNotSome()
        {
            Maybe<List<int>> maybeList = new Maybe<List<int>>();
            Assert.IsFalse(maybeList.IsSome());
        }

        [Test]
        public void TestSomeObjectIsSome()
        {
            Maybe<List<int>> maybeList = new Maybe<List<int>>(new List<int> {1, 2, 3});
            Assert.IsTrue(maybeList.IsSome());
        }

        [Test]
        public void TestSomeObjectIsNotNone()
        {
            Maybe<List<int>> maybeList = new Maybe<List<int>>(new List<int> {1, 2, 3});
            Assert.IsFalse(maybeList.IsNone());
        }

        [Test]
        public void TestNullIsNone()
        {
            Maybe<List<int>> maybeList = new Maybe<List<int>>(null);
            Assert.IsTrue(maybeList.IsNone());
        }

        [Test]
        public void TestMatchNonePrimativeTriggersNoneHandler()
        {
            Maybe<int> maybeInt = new Maybe<int>();
            maybeInt.Match(some: i => Assert.Fail("[some] handler should not be called"), none: Assert.Pass);
            Assert.Fail("Match(...) should execute immediately");
        }

        [Test]
        public void TestMatchSomePrimativeTriggersSomeHandler()
        {
            Maybe<int> maybeInt = new Maybe<int>(1);
            maybeInt.Match(some: i => Assert.Pass(), none: () => Assert.Fail("[none] handler should not be called"));
            Assert.Fail("Match(...) should execute immediately");
        }

        [Test]
        public void TestMatchSomePrimativeReturnsSome()
        {
            Maybe<int> maybeInt = new Maybe<int>(1);
            Assert.That(maybeInt.IsSome);
            int match = maybeInt.Match(some: i => 1, none: () => 0);
            Assert.That(match, Is.EqualTo(1));
        }

        [Test]
        public void TestMatchNonePrimativeReturnsNone()
        {
            Maybe<int> maybeInt = new Maybe<int>();
            Assert.That(maybeInt.IsNone);
            int match = maybeInt.Match(some: i => 1, none: () => 0);
            Assert.That(match, Is.EqualTo(0));
        }

        [Test]
        public void TestMatchNoneObjectTriggersNoneHandlerImmediately()
        {
            Maybe<List<int>> maybeList = new Maybe<List<int>>();
            maybeList.Match(some: i => Assert.Fail("[some] handler should not be called"), none: Assert.Pass);
            Assert.Fail("match should execute immediately");
        }

        [Test]
        public void TestMatchSomeObjectTriggersSomeHandlerImmediately()
        {
            Maybe<List<int>> maybeList = new Maybe<List<int>>();
            maybeList.Match(some: i => Assert.Fail("[some] handler should not be called"), none: Assert.Pass);
            Assert.Fail("match should execute immediately");
        }

        [Test]
        public void TestMatchSomeObjectReturnsSome()
        {
            Maybe<List<int>> maybeList = new Maybe<List<int>>(new List<int> {1, 2, 3});
            var list1 = new List<string>() { "a", "b"};
            var list2 = new List<string>() {"1", "2"};
            var match = maybeList.Match(some: i => list1, none: () => list2);
            Assert.That(match, Is.EqualTo(list1));
        }

        [Test]
        public void TestMatchNoneObjectReturnsNone()
        {
            Maybe<List<int>> maybeList = new Maybe<List<int>>();
            var list1 = new List<string> { "a", "b" };
            var list2 = new List<string> { "1", "2" };
            var match = maybeList.Match(some: i => list1, none: () => list2);
            Assert.That(match, Is.EqualTo(list2));
        }

        [Test]
        public void TestMatchSomeObjectUsingAFunctionForSomeAndAValueForNoneReturnsTheResultOfTheFunction()
        {
            var list = new List<int> { 1, 2, 3 };
            Maybe<List<int>> maybeList = new Maybe<List<int>>(list);
            const string noneValue = "none";
            const string someValue = "some";
            Func<List<int>, string> someFunction = (lst) => someValue;
            string match = maybeList.Match(some: someFunction, none: noneValue);
            Assert.That(match, Is.EqualTo(someFunction(list)));
        }

        [Test]
        public void TestMatchSomeObjectUsingAValueForSomeAndAFunctionForNoneReturnsTheValue()
        {
            Maybe<List<int>> maybeList = new Maybe<List<int>>(new List<int> { 1, 2, 3 });
            const string someValue = "some";
            Func<string> noneFunction = () =>
            {
                Assert.Fail("[none] handler should not be called");
                return "none";
            };
            string match = maybeList.Match(some: someValue, none: noneFunction);
            Assert.That(match, Is.EqualTo(someValue));
        }

        [Test]
        public void TestMatchNoneObjectUsingAValueForSomeAndAFunctionForNoneReturnsTheResultOfTheFunction()
        {
            Maybe<List<int>> maybeList = new Maybe<List<int>>();
            const string someValue = "some";
            const string noneValue = "none";
            Func<string> noneFunction = () => noneValue;
            string match = maybeList.Match(some: someValue, none: noneFunction);
            Assert.That(match, Is.EqualTo(noneFunction()));
        }

        [Test]
        public void TestMatchNoneObjectUsingAFunctionForSomeAndAValueForNoneReturnsTheValue()
        {
            Maybe<List<int>> maybeList = new Maybe<List<int>>();
            const string noneValue = "none";
            Func<List<int>, string> someFunction = lst =>
            {
                Assert.Fail("[some] handler should not be called");
                return "some";
            };
            string match = maybeList.Match(some: someFunction, none: noneValue);
            Assert.That(match, Is.EqualTo(noneValue));
        }

        [Test]
        public void TestMatchSomeObjectUsingAFunctionForSomeAndAValueForNoneCallsTheFunctionImmediately()
        {
            Maybe<List<int>> maybeList = new Maybe<List<int>>(new List<int> { 1, 2, 3 });
            maybeList.Match(some: list => Assert.Pass(), none: () => Assert.Fail("[none] handler should not be called"));
            Assert.Fail("match should execute immediately");
        }

        // todo: finish the tests
    }
}