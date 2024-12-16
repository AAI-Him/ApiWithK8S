using ConsoleTest.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTest
{
    public class Bird(
    ) : IAnimal, IDisposable
    {
        private bool _disposed = false;
        public string? Name { get; set; }
        public IAnimalOption? AnimalOption { get; set; }

        public IAnimal UseAnimalBuilder(string name)
        {
            Name = name;
            return this;
        }

        public IAnimal UseAnimalBuilder(string name, Action<IAnimalOption> option)
        {
            Name = name;
            AnimalOption = Activator.CreateInstance<IAnimalOption>();
            option(AnimalOption);
            return this;
        }

        public IAnimal Build()
        {
            return this;
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }
        }
    }
}
