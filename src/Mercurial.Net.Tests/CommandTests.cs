using System.Linq;
using Mercurial.Net;
using NUnit.Framework;

namespace Mercurial.Net.Tests
{
    [TestFixture]
    public class CommandTests : SingleRepositoryTestsBase
    {
        [Test]
        [Category("Integration")]
        public void Execute_WithInitCommand_InitializesTheRepository()
        {
            Repo.Execute(new InitCommand());
            Repo.Log().ToArray();
        }
    }
}