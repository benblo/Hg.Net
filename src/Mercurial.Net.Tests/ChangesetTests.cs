using Mercurial.Net;
using NUnit.Framework;

namespace Mercurial.Net.Tests
{
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    public class ChangesetTests
    {
        [Test]
        [Category("API")]
        public void Equals_ChangesetAndObject_ReturnsFalse()
        {
            var set = new Changeset();
            var obj = new object();

            Assert.That(set.Equals(obj), Is.False);
        }
    }
}