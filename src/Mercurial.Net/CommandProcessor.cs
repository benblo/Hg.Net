using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace Mercurial.Net
{
    /// <summary>
    /// This class handles executing external executables in order to process
    /// commands.
    /// </summary>
    public static class CommandProcessor
    {
        /// <summary>
        /// Executes the given executable to process the given command.
        /// </summary>
        /// <param name="workingDirectory">
        /// The working directory while executing the command.
        /// </param>
        /// <param name="executable">
        /// The full path to and name of the executable to execute.
        /// </param>
        /// <param name="command">
        /// The options to the executable.
        /// </param>
        /// <param name="environmentVariables">
        /// An array of <see cref="System.Collections.Generic.KeyValuePair{TKey,TValue}"/> objects, containing environment variable
        /// overrides to use while executing the executable.
        /// </param>
        /// <param name="specialArguments">
        /// Any special arguments to pass to the executable, not defined by the <paramref name="command"/>
        /// object, typically common arguments that should always be passed to the executable.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        /// <para><paramref name="workingDirectory"/> is <c>null</c> or empty.</para>
        /// <para>- or -</para>
        /// <para><paramref name="executable"/> is <c>null</c> or empty.</para>
        /// <para>- or -</para>
        /// <para><paramref name="command"/> is <c>null</c>.</para>
        /// <para>- or -</para>
        /// <para><paramref name="environmentVariables"/> is <c>null</c>.</para>
        /// <para>- or -</para>
        /// <para><paramref name="specialArguments"/> is <c>null</c>.</para>
        /// </exception>
        /// <exception cref="MercurialException">
        /// <para>The executable did not finish in the allotted time.</para>
        /// </exception>
        public static void Execute(
            string workingDirectory,
            string executable,
            ICommand command,
            KeyValuePair<string, string>[] environmentVariables,
            IEnumerable<string> specialArguments)
        {
            if (StringEx.IsNullOrWhiteSpace(workingDirectory))
                throw new ArgumentNullException(nameof(workingDirectory));
            if (StringEx.IsNullOrWhiteSpace(executable))
                throw new ArgumentNullException(nameof(executable));
            if (command == null)
                throw new ArgumentNullException(nameof(command));
            if (environmentVariables == null)
                throw new ArgumentNullException(nameof(environmentVariables));
            if (specialArguments == null)
                throw new ArgumentNullException(nameof(specialArguments));

            command.Validate();
            command.Before();

            IEnumerable<string> arguments = specialArguments;
            arguments = arguments.Concat(command.Arguments.Where(a => !StringEx.IsNullOrWhiteSpace(a)));
            arguments = arguments.Concat(command.AdditionalArguments.Where(a => !StringEx.IsNullOrWhiteSpace(a)));

            string argumentsString = string.Join(" ", arguments.ToArray());

            var startInfo = new ProcessStartInfo
            {
                FileName = executable,
                WorkingDirectory = workingDirectory,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                UseShellExecute = false,
                ErrorDialog = false,
                Arguments = command.Command + " " + argumentsString,
            };
            foreach (var kvp in environmentVariables)
                startInfo.EnvironmentVariables[kvp.Key] = kvp.Value;
            ClientExecutable.LazyInitialize();
            startInfo.StandardErrorEncoding = ClientExecutable.GetMainEncoding();
            startInfo.StandardOutputEncoding = ClientExecutable.GetMainEncoding();

            command.Observer?.Executing(command.Command, argumentsString);


            using Process process = Process.Start(startInfo);
            Func<StreamReader, Action<string>, string> reader;
            if (command.Observer != null)
            {
                reader = delegate(StreamReader streamReader, Action<string> logToObserver)
                {
                    var output = new StringBuilder();
                    string line;
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        logToObserver(line);
                        if (output.Length > 0)
                            output.Append(Environment.NewLine);
                        output.Append(line);
                    }

                    return output.ToString();
                };
            }
            else
                reader = (streamReader, logToObserver) => streamReader.ReadToEnd();

            string standardOutput = reader(process.StandardOutput,
                line => command.Observer.Output(line));
            string errorOutput = reader(process.StandardError,
                line => command.Observer.ErrorOutput(line));

            int timeout = Timeout.Infinite;
            if (command.Timeout > 0)
                timeout = 1000 * command.Timeout;

            process.WaitForExit(timeout);

            if (command.Observer != null)
                command.Observer.Executed(command.Command, argumentsString, process.ExitCode, standardOutput,
                    errorOutput);

            command.After(process.ExitCode, standardOutput, errorOutput);
        }
    }
}