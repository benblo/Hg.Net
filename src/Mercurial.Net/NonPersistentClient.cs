using System;
using System.Collections.Generic;
using System.IO;
using Mercurial.Versions;

namespace Mercurial
{
    /// <summary>
    /// This class implements <see cref="IClient"/> by spinning up an instance of the
    /// Mercurial executable for each command execution.
    /// </summary>
    public sealed class NonPersistentClient : IClient
    {
        /// <summary>
        /// This is the backing field for the <see cref="RepositoryPath"/> property.
        /// </summary>
        private readonly string _RepositoryPath;

        /// <summary>
        /// Initializes a new instance of the <see cref="NonPersistentClient"/> class.
        /// </summary>
        /// <param name="repositoryPath">
        /// The path to the repository (or not-yet-repository) that this <see cref="NonPersistentClient"/>
        /// will manage.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="repositoryPath"/> is <c>null</c> or empty.</para>
        /// </exception>
        public NonPersistentClient(string repositoryPath)
        {
            if (StringEx.IsNullOrWhiteSpace(repositoryPath))
                throw new ArgumentNullException("repositoryPath");

            _RepositoryPath = repositoryPath;
            ClientExecutable.Configuration.Refresh(repositoryPath);
        }

        /// <summary>
        /// Gets the path to the repository (or not-yet-repository) that the client
        /// is managing.
        /// </summary>
        public string RepositoryPath
        {
            get
            {
                return _RepositoryPath;
            }
        }

        /// <summary>
        /// Executes the given <see cref="IMercurialCommand"/> command without
        /// a repository.
        /// </summary>
        /// <param name="command">
        /// The <see cref="IMercurialCommand"/> command to execute.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="command"/> is <c>null</c>.</para>
        /// </exception>
        public static void Execute(IMercurialCommand command)
        {
            if (command == null)
                throw new ArgumentNullException("command");

            ClientExecutable.LazyInitialize();
            Execute(Path.GetTempPath(), command);
        }

        /// <summary>
        /// Executes the given <see cref="IMercurialCommand"/> command against
        /// the Mercurial repository.
        /// </summary>
        /// <param name="repositoryPath">
        /// The root path of the repository to execute the command in.
        /// </param>
        /// <param name="command">
        /// The <see cref="IMercurialCommand"/> command to execute.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="repositoryPath"/> is <c>null</c>.</para>
        /// <para>- or -</para>
        /// <para><paramref name="command"/> is <c>null</c>.</para>
        /// </exception>
        /// <exception cref="MercurialException">
        /// HG did not complete within the allotted time.
        /// </exception>
        public static void Execute(string repositoryPath, IMercurialCommand command)
        {
            if (StringEx.IsNullOrWhiteSpace(repositoryPath))
                throw new ArgumentNullException("repositoryPath");
            if (command == null)
                throw new ArgumentNullException("command");

            ClientExecutable.LazyInitialize();
            var encodingName = ClientExecutable.GetTerminalEncoding().WebName;
            //Console.WriteLine("ClientExecutable.GetTerminalEncoding(): " + encodingName);
            encodingName = "IBM437"; // this is the default value in .NET 4.7, but in 5.0+ the name is 'Codepage - 437', which Mercurial doesn't know about

            // trying (and failing) to get filenames with accents to show up correctly...
            // https://www.mercurial-scm.org/wiki/CharacterEncodingOnWindows
            //encodingName = "cp1252";
            //encodingName = "cp1251";

            //encodingName = "utf-8";
            //encodingName = "utf-16";
            //encodingName = "";
            //encodingName = "unicode";
            //encodingName = Encoding.ASCII.WebName;
            //encodingName = Encoding.UTF32.WebName;
            //encodingName = Encoding.UTF7.WebName;
            //encodingName = "cp437";
            //encodingName = "latin1";
            //encodingName = "ISO-8859";
            //encodingName = "8859";
            //encodingName = "iso-8859-1";
            //encodingName = "iso-8859-2";

            var specialArguments = (IEnumerable<string>)new[]
            {
                "--noninteractive", "--encoding", encodingName,
            };
            var environmentVariables = new KeyValuePair<string, string>[]
            {
                new KeyValuePair<string, string>("LANGUAGE", "EN"),
                new KeyValuePair<string, string>("HGENCODING", encodingName),
            };

            CommandProcessor.Execute(repositoryPath, ClientExecutable.ClientPath, command, environmentVariables, specialArguments);
            MercurialVersionBase.Current.WaitForLocksToDissipate(repositoryPath);
        }

        /// <summary>
        /// Executes the given <see cref="IMercurialCommand"/> command against
        /// the Mercurial repository.
        /// </summary>
        /// <param name="command">
        /// The <see cref="IMercurialCommand"/> command to execute.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="command"/> is <c>null</c>.</para>
        /// </exception>
        /// <exception cref="MercurialException">
        /// HG did not complete within the allotted time.
        /// </exception>
        void IClient.Execute(IMercurialCommand command)
        {
            Execute(_RepositoryPath, command);
        }
    }
}
