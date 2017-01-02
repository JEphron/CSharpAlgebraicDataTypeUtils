using JetBrains.Annotations;
using NUnit.Framework;

namespace JME.UnionTypes.Tests
{
    [TestFixture]
    public class TestResult
    {
        [Test]
        public void TestOkIsOk()
        {
            var resultOk = Result<int, string>.Ok(99);
            Assert.That(resultOk.IsOk);
        }

        [Test]
        public void TestOkIsNotErr()
        {
            var resultOk = Result<int, string>.Ok(99);
            Assert.That(resultOk.IsErr, Is.False);
        }

        [Test]
        public void TestErrIsErr()
        {
            var resultErr = Result<int, string>.Err("err");
            Assert.That(resultErr.IsErr);
        }

        [Test]
        public void TestErrIsNotOk()
        {
            var resultErr = Result<int, string>.Err("err");
            Assert.That(resultErr.IsOk, Is.False);
        }

        [Test]
        public void TestOkMatchesImmediately()
        {
            var resultOk = Result<int, string>.Ok(99);
            resultOk.Match(ok: _ => Assert.Pass(), err: _ => Assert.Pass());
            Assert.Fail();
        }

        [Test]
        public void TestErrMatchesImmediately()
        {
            var resultErr = Result<int, string>.Err("err");
            resultErr.Match(ok: _ => Assert.Pass(), err: _ => Assert.Pass());
            Assert.Fail();
        }

        [Test]
        public void TestOkMatchesOk()
        {
            var resultOk = Result<int, string>.Ok(99);
            resultOk.Match(ok: _ => Assert.Pass(), err: _ => Assert.Fail());
        }

        [Test]
        public void TestErrMatchesErr()
        {
            var resultErr = Result<int, string>.Err("err");
            resultErr.Match(ok: _ => Assert.Fail(), err: _ => Assert.Pass());
        }

        [Test]
        public void TestMatchOkReturnsTheResultOfTheOkHandler()
        {
            var resultOk = Result<int, string>.Ok(99);
            const string ok = "ok";
            const string err = "err";
            var match = resultOk.Match(ok: i => ok, err: str => err);
            Assert.That(match, Is.EqualTo(ok));
        }

        [Test]
        public void TestMatchErrReturnsTheResultOfTheErrHandler()
        {
            var resultErr = Result<int, string>.Err("err");
            const string ok = "ok";
            const string err = "err";
            var match = resultErr.Match(ok: i => ok, err: str => err);
            Assert.That(match, Is.EqualTo(err));
        }

        [Test]
        public void TestOkMethodReturnsAMaybeWithTheOkValueWhenCalledOnOk()
        {
            const int value = 99;
            var resultOk = Result<int, string>.Ok(value);
            var maybeInt = resultOk.Ok();
            Assert.That(maybeInt.OrDefault(), Is.EqualTo(value));
        }

        [Test]
        public void TestOkMethodReturnsNoneWhenCalledOnErr()
        {
            var resultErr = Result<int, string>.Err("err");
            var maybeInt = resultErr.Ok();
            Assert.That(maybeInt.IsNone);
        }

        [Test]
        public void TestErrMethodReturnsAMaybeWithTheErrValueWhenCalledOnErr()
        {
            const string err = "err";
            var resultErr = Result<int, string>.Err(err);
            var maybeStr = resultErr.Err();
            Assert.That(maybeStr.OrDefault(), Is.EqualTo(err));
        }

        [Test]
        public void TestErrMethodReturnsNoneWhenCalledOnOk()
        {
            var resultOk = Result<int, string>.Ok(99);
            var maybeStr = resultOk.Err();
            Assert.That(maybeStr.IsNone);
        }

        [Test]
        public void TestMapOverErrDoesNotExecuteFunction()
        {
            var resultErr = Result<int, string>.Err("err");
            resultErr.Map(_ =>
            {
                Assert.Fail();
                return 0;
            });
            Assert.Pass();
        }

        [Test]
        public void TestMapOverOkExecutesFunctionImmediately()
        {
            var resultOk = Result<int, string>.Ok(99);
            resultOk.Map(_ =>
            {
                Assert.Pass();
                return 0;
            });
            Assert.Fail();
        }

        [Test]
        public void TestMapOverErrReturnsErr()
        {
            const string err = "err";
            var resultErr = Result<int, string>.Err(err);
            var result = resultErr.Map(i => "doNothing");
            result.Match(ok: _ => Assert.Fail(), err: s => Assert.That(s == err));
        }

        [Test]
        public void TestMapOverOkReturnsMappedValue()
        {
            const int originalValue = 99;
            const string mappedValue = "foo";
            var resultOk = Result<int, string>.Ok(originalValue);
            var result = resultOk.Map(i => i + mappedValue);
            Assert.That(result.Ok().OrDefault(), Is.EqualTo(originalValue + mappedValue));
        }

        [Test]
        public void TestMapErrOverErrExecutesFunctionImmediately()
        {
            var resultErr = Result<int, string>.Err("err");
            resultErr.MapErr(s =>
            {
                Assert.Pass();
                return 0;
            });
            Assert.Fail();
        }

        [Test]
        public void TestMapErrOverOkDoesNotExecuteFunction()
        {
            var resultOk = Result<int, string>.Ok(99);
            resultOk.MapErr(s =>
            {
                Assert.Fail();
                return 0;
            });
            Assert.Pass();
        }

        [Test]
        public void TestMapErrOverErrReturnsMappedValueInErr()
        {
            const string err = "err";
            var resultErr = Result<int, string>.Err(err);
            var mapErr = resultErr.MapErr(s => s.Length);
            Assert.That(mapErr.Err().OrDefault(), Is.EqualTo(err.Length));
        }

        [Test]
        public void TestMapErrOverOkReturnsOk()
        {
            const int ok = 99;
            var resultOk = Result<int, string>.Ok(ok);
            var mapErr = resultOk.MapErr(s => "doNothing".Length);
            Assert.That(mapErr.Ok().OrDefault(), Is.EqualTo(ok));
        }
    }
}
