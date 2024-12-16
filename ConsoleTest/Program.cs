// See https://aka.ms/new-console-template for more information
using ConsoleTest;
using System.Xml.Linq;

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
//var startup = new Startup(null);
//startup.Error += (object sender, TestErrorEventArgs args) => Console.WriteLine(args.Message);

var bird1 = new Bird();
bird1.UseAnimalBuilder("Bird", option => {
    option.Name = "";
}).Build();

var temp = new GenericBase<Bird, Bird, string>(bird1, bird1, "");
temp.Event += (t1, t2, t3) => {
    return true;
};

Console.WriteLine(bird1?.AnimalOption?.Name);
Console.WriteLine($"Name={bird1?.Name}");

