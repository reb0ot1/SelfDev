namespace SelfDevelopmentProj.StructuralPattern
{
    public class BridgePattern
    {
        public static void Start()
        {
            var impl1 = new ConcreteImplementationA();
            var impl2 = new ConcreteImplementationB();

            Abstraction abstraction;
            // The client code should be able to work with any pre-configured
            // abstraction-implementation combination.
            abstraction = new Abstraction(impl1);

            ClientCode(abstraction);

            abstraction = new ExtentedAbstraction(impl2);
            ClientCode(abstraction);
        }

        // Except for the initialization phase, where an Abstraction object gets
        // linked with a specific Implementation object, the client code should
        // only depend on the Abstraction class. This way the client code can
        // support any abstraction-implementation combination.
        private static void ClientCode(Abstraction abstraction)
        {
            Console.WriteLine(abstraction.Operation());
        }

        // The Abstraction defines the interface for the "control" part of the two
        // class hierarchies. It maintains a reference to an object of the
        // Implementation hierarchy and delegates all of the real work to this
        // object.
        class Abstraction
        {
            protected readonly IImplementation _implementation;

            public Abstraction(IImplementation implementation)
            {
                _implementation = implementation;
            }

            public virtual string Operation()
            {
                return "Abstract: Base operation with:\n" +
                _implementation.OperationImplementation();
            }
        }

        // You can extend the Abstraction without changing the Implementation
        // classes.
        class ExtentedAbstraction : Abstraction
        {
            public ExtentedAbstraction(IImplementation implementation) : base(implementation)
            {
            }

            public override string Operation()
            {
                return "ExtendedAbstracttion: Extended operation with:\n" +
                _implementation.OperationImplementation();
            }
        }

        // The Implementation defines the interface for all implementation classes.
        // It doesn't have to match the Abstraction's interface. In fact, the two
        // interfaces can be entirely different. Typically the Implementation
        // interface provides only primitive operations, while the Abstraction
        // defines higher- level operations based on those primitives.
        interface IImplementation
        {
            string OperationImplementation();
        }

        // Each Concrete Implementation corresponds to a specific platform and
        // implements the Implementation interface using that platform's API.
        class ConcreteImplementationA : IImplementation
        {
            public string OperationImplementation()
            {
                return "ConcreteImplementationA: The result in platform A.\n";
            }
        }

        class ConcreteImplementationB : IImplementation
        {
            public string OperationImplementation()
            {
                return "ConcreteImplementationA: The result in platform B.\n";
            }
        }
    }
}
