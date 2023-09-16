using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfDevelopmnetProj.ConsoleWithIHost
{
    public class NumberWorker
    {
        private readonly INumberService _service;

        public NumberWorker(INumberService service) => _service = service;

        public void PrintNumber()
        {
            var number = _service.GetPositiveNumber();
            Console.WriteLine($"My wonderful number is {number}");
        }
    }
}
