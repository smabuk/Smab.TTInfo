using Smab.TTInfo;

Console.WriteLine("Table Tennis 365 reader");

TT365Reader tt365 = new()
{
	CacheFolder = """c:\temp\tt365\cache""",
	CacheHours = 1,
};
