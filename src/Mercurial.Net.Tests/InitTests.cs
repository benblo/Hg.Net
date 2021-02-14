using Mercurial.Net;
using NUnit.Framework;

namespace Mercurial.Net.Tests
{
    [TestFixture]
    public class InitTests : SingleRepositoryTestsBase
    {
        [Test]
        [Category("Integration")]
        public void Init_WithInitializedRepo_ThrowsMercurialExecutionException()
        {
            Repo.Init();
            Assert.Throws<MercurialExecutionException>(() => Repo.Init());
        }
    }
}