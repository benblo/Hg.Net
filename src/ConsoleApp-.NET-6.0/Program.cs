using Mercurial;

var repoPath = @"C:\Work\misc\DvcsSandbox\DvcsSandbox-test-repo-hg";
var repo = new Repository(repoPath);
Console.WriteLine(repo);
//Console.WriteLine($"repo: {repo.Path}");

{
	var status = repo.Status().ToArray();
	Console.WriteLine($"Status: {status.Length}");
	foreach (var file in status)
	{
		Console.WriteLine(file);
		var diff = repo.Diff(new DiffCommand().WithNames(file.Path));
		Console.WriteLine(diff);
		Console.WriteLine();
	}
}

{
	var cmd = new DiffCommand();
	var diff = repo.Diff(cmd);
	Console.WriteLine("//////////");
	Console.WriteLine(diff);
	Console.WriteLine("//////////");
	Console.WriteLine(cmd.Names.Count);
}

{
	var diff = repo.Diff(new DiffCommand().WithNames("root.txt"));
	Console.WriteLine("//////////");
	Console.WriteLine(diff);
	Console.WriteLine("//////////");
}

{
	//repo.Diff()
}