var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => {
  Test("Hello World!");
  return "Hello World!";
});

await app.RunAsync();



static void Test(string a) {
  Console.WriteLine(a);
}