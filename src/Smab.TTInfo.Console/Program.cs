CommandApp<TTInfoCliCommand> app = new();

app.Configure(config =>
{
	config.AddExample("Reading");
	config.AddExample("Reading", "2022");
	config.AddExample("Reading", "-t", "king");
#if DEBUG
	//config.PropagateExceptions();
	config.ValidateExamples();
#endif
});

return await app.RunAsync(args);
