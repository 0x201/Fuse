var app = ConsoleApp.Create();

app.Add<SHA256Command>("sha256");

app.Run(args);