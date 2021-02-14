using System;
using System.Globalization;
using Mercurial.Net;
using NUnit.Framework;

namespace Mercurial.Net.Tests
{
    [TestFixture]
    [Category("Integration")]
    public class AdditionalArgumentsTests : SingleRepositoryTestsBase
    {
        [Test]
        public void Log_WithUnknownArgument_ThrowsMercurialExecutionException()
        {
            Repo.Init();

            var command = new LogCommand();
            command.AdditionalArguments.Add("--dummyargument");

            Assert.Throws<MercurialExecutionException>(() => Repo.Execute(command));
        }

        [Test]
        public void Log_WithUnknownArgument_ThrowsMercurialExecutionExceptionThatMentionesArgument()
        {
            Repo.Init();

            var command = new LogCommand();
            command.AdditionalArguments.Add("--dummyargument");

            try
            {
                Repo.Execute(command);
            }
            catch (MercurialExecutionException ex)
            {
                Assert.That(ex.Message, Contains.Substring("hg log: option --dummyargument not recognized"));
                return;
            }
            catch (Exception ex)
            {
                Assert.Fail(string.Format(CultureInfo.InvariantCulture, "Log with unknown argument should not have thrown {0}", ex.GetType().Name));
            }
            Assert.Fail("Log with unknown argument should have thrown MercurialExecutionException");
        }
    }
}