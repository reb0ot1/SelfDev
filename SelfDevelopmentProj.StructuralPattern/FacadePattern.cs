using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfDevelopmentProj.StructuralPattern
{
    public class FacadePattern
    {
        public static void Start()
        {
            var facade = new Facade(new SubSystem1(), new SubSystem2());
            Client.ClientCode(facade);
        }

        class Client
        {
            // The client code works with complex subsystems through a simple
            // interface provided by the Facade. When a facade manages the lifecycle
            // of the subsystem, the client might not even know about the existence
            // of the subsystem. This approach lets you keep the complexity under
            // control.
            public static void ClientCode(Facade facade)
            {
                Console.Write(facade.Operation());
            }
        }

        class Facade
        {
            private readonly SubSystem1 _subsystem1;
            private readonly SubSystem2 _subsystem2;

            public Facade(SubSystem1 subSystem1, SubSystem2 subSystem2)
            {
                this._subsystem1 = subSystem1;
                this._subsystem2 = subSystem2;
            }

            public string Operation()
            {
                string result = "Facade initializes subsystems:\n";
                result += this._subsystem1.operation1();
                result += this._subsystem2.operation1();
                result += "Facade orders subsystems to perform the action:\n";
                result += this._subsystem1.operationN();
                result += this._subsystem2.operationZ();
                return result;
            }
        }

        class SubSystem1
        {
            public string operation1()
            {
                return "Subsystem1: Ready!\n";
            }

            public string operationN()
            {
                return "Subsystem1: Go!\n";
            }
        }

        class SubSystem2
        {
            public string operation1()
            {
                return "Subsystem2: Get ready!\n";
            }

            public string operationZ()
            {
                return "Subsystem2: Fire!\n";
            }
        }
    }
}
