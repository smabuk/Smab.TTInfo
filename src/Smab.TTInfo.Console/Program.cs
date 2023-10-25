CommandApp<TTInfoCliCommand> app = new();

app.Configure(config =>
{
	config.AddExample("Reading");
	config.AddExample("Reading", "2022");
	config.AddExample("Reading", "-t", "king");
	config.AddExample("Reading", "-p", "mark", "-v", "paul");
#if DEBUG
	config.PropagateExceptions();
	config.ValidateExamples();
#endif
});

try {
	return await app.RunAsync(args);
}
catch (Exception ex) {
	AnsiConsole.MarkupLine("");
	AnsiConsole.MarkupLine($"Error: [Red]{ex.Message}[/]");
	return -1;
}

