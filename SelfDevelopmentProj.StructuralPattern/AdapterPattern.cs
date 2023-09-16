using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfDevelopmentProj.StructuralPattern
{
    public class AdapterPattern
    {
        public static void Start()
        {
            var adaptee = new Adaptee();
            var adapter = new Adapter(adaptee);

            Console.WriteLine("Adaptee interface is incompatible with the client.");
            Console.WriteLine("But with adapter client can call it's method.");

            Console.WriteLine(adapter.GetRequest());
        }

        interface ITarget
        {
            string GetRequest();
        }

        class Adaptee
        {
            public string GetSpecificRequest()
            {
                return "Specific request.";
            }
        }

        class Adapter : ITarget
        {
            private readonly Adaptee _adaptee;

            public Adapter(Adaptee adaptee)
            {
                _adaptee = adaptee;
            }

            public string GetRequest()
            {
                return $"This is '{this._adaptee.GetSpecificRequest()}'";
            }
        }
    }
}
