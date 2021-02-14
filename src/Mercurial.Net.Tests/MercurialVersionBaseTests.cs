using System;
using System.Collections.Generic;
using Mercurial.Net.Versions;
using NUnit.Framework;

namespace Mercurial.Net.Tests
{
    [TestFixture]
    public class MercurialVersionBaseTests
    {
        public static IEnumerable<object[]> GetImplementationFor_TestCases()
        {
            yield return new object[]
            {
                new Version(1, 8, 0, 0),
                typeof(MercurialVersionBase)
            };
            yield return new object[]
            {
                new Version(1, 7, 0, 0),
                typeof(MercurialVersionPre18)
            };
            yield return new object[]
            {
                new Version(1, 6, 0, 0),
                typeof(MercurialVersion16)
            };
            yield return new object[]
            {
                new Version(1, 6, 2, 0),
                typeof(MercurialVersion16)
            };
        }

        [Test]
        [Category("API")]
        [TestCaseSource(nameof(GetImplementationFor_TestCases))]
        public void GetImplementationFor_WithTestCases(Version version, Type type)
        {
            MercurialVersionBase implementation = MercurialVersionBase.GetImplementationFor(version);

            Assert.That(implementation.GetType(), Is.SameAs(type));
        }
    }
}