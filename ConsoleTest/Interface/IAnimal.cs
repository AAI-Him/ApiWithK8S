using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTest.Interface
{
    public interface IAnimalOption
    {
        string? Name { get; set; }
    }

    public class NormalAnimalOption : IAnimalOption
    {
        public string? Name { get; set; }
    }

    public class AdvancedAnimalOption : IAnimalOption
    {
        public string? Name { get; set; }
        public Func<int, bool>? CanFly { set; get; }
    }

    public interface IAnimal
    {
        string Name { get; set; }
        IAnimal UseAnimalBuilder(string name);
        //T UseAnimalGenericBuilder<T>(string name);
        IAnimal UseAnimalBuilder(string name, Action<IAnimalOption> option);
        IAnimal Build();
    }
}
