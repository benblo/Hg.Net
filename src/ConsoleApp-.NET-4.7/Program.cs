using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
	internal class Program
	{
		static void Main(string[] args)
		{
			var repoPath = @"C:\Work\misc\DvcsSandbox\DvcsSandbox-test-repo-hg";
			var repo = new Mercurial.Repository(repoPath);
			Console.WriteLine(repo);
			//Console.WriteLine($"repo: {repo.Path}");

			var status = repo.Status().ToArray();
			Console.WriteLine($"Status: {status.Length}");
			foreach (var file in status)
			{
				Console.WriteLine(file);
			}

			Console.WriteLine();
			Console.WriteLine("press a key...");
			Console.ReadKey();
		}
	}
}