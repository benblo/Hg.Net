using Mercurial.Net;

namespace Mercurial.Net.Tests
{
    internal class DummyCommand : MercurialCommandBase<DummyCommand>
    {
        public DummyCommand()
            : base("dummy")
        {
        }
    }
}