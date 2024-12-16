// See https://aka.ms/new-console-template for more information
using ConsoleTest;

Console.WriteLine("Hello, World!");

//IDictionary<string, string> dict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

//Console.WriteLine("Hello, World!");


//dict.Add(new KeyValuePair<string, string>("AAA", "1"));
//dict.Add(new KeyValuePair<string, string>("aAA", "2"));

//Console.WriteLine($"dict.Count:{dict.Count}");

//foreach (KeyValuePair<string, string> kvp in dict)
//{
//    Console.WriteLine(String.Join(":", kvp.Key, kvp.Value));
//}

//Console.ReadKey();
var startup = new Startup(null);
startup.Error += (object sender, TestErrorEventArgs args) => Console.WriteLine(args.Message);

