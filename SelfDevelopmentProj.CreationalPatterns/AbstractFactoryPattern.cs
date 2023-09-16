namespace SelfDevelopmentProj.CreationalPatterns
{
    public class AbstractFactoryPattern
    {
        public static void Start()
        {
            new ClientAbstractFactory().Main();
        }

        interface IAbstractFactory // Products from same type but with somekind of difference
        {
            //Example: Car
            IAbstactProductA CreateProductA();

            //Example: Bus
            IAbstractProductB CreateProductB();
        }

        interface IAbstractProductB
        {
            string UsefullFunction();

            string AnotherUsefullfunction(IAbstactProductA collaborator);
        }

        //Car product
        interface IAbstactProductA
        {
            string UsefullFunction();
        }

        class ConcreteFactory1 : IAbstractFactory
        {
            public IAbstactProductA CreateProductA()
            {
                return new ConcreteProductA1();
            }

            public IAbstractProductB CreateProductB()
            {
                return new ConcreteProductB1();
            }
        }

        class ConcreateFactory2 : IAbstractFactory
        {
            public IAbstactProductA CreateProductA()
            {
                return new ConcreteProductA2();
            }

            public IAbstractProductB CreateProductB()
            {
                return new ConcreteProductB2();
            }
        }

        //Bus with short base
        class ConcreteProductB1 : IAbstractProductB
        {
            public string AnotherUsefullfunction(IAbstactProductA collaborator)
            {
                var result = collaborator.UsefullFunction();
                return $"The result of the Bus with short base collaborating with the ({result})";
            }

            public string UsefullFunction()
            {
                return "The result of the product Bus with short base.";
            }
        }

        //Car coupe
        class ConcreteProductA1 : IAbstactProductA
        {
            public string UsefullFunction()
            {
                return "The result of the product Car Coupe.";
            }
        }

        //Car SUV
        class ConcreteProductA2 : IAbstactProductA
        {
            public string UsefullFunction()
            {
                return "The result of the product Car SUV.";
            }
        }

        //Bus with long base
        class ConcreteProductB2 : IAbstractProductB
        {
            public string AnotherUsefullfunction(IAbstactProductA collaborator)
            {
                var result = collaborator.UsefullFunction();
                return $"The result of the Bus with long base collaborating with the ({result})";
            }

            public string UsefullFunction()
            {
                return "The result of the product Bus with long base.";
            }
        }

        class ClientAbstractFactory
        {
            public void Main()
            {
                // The client code can work with any concrete factory class.
                Console.WriteLine("Client: Testing client code with the first factory type...");
                this.ClientMethod(new ConcreteFactory1());

                Console.WriteLine();

                Console.WriteLine("Client: Testing the same client code with the second factory type...");
                this.ClientMethod(new ConcreateFactory2());
            }

            void ClientMethod(IAbstractFactory factory)
            {
                var productA = factory.CreateProductA();
                var productB = factory.CreateProductB();

                Console.WriteLine(productB.UsefullFunction());
                Console.WriteLine(productB.AnotherUsefullfunction(productA));
            }

        }
    }
}
