using Mercurial.Net;
using NUnit.Framework;

namespace Mercurial.Net.Tests
{
    public abstract class SingleRepositoryTestsBase : RepositoryTestsBase
    {
        protected Repository Repo
        {
            get;
            private set;
        }

        [SetUp]
        public virtual void SetUp()
        {
            Repo = GetRepository();
        }

        [TearDown]
        public override void TearDown()
        {
            Repo.Dispose();
            Repo = null;

            base.TearDown();
        }
    }
}