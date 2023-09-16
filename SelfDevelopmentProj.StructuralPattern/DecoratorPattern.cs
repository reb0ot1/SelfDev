namespace SelfDevelopmentProj.StructuralPattern
{
    public class DecoratorPattern
    {
        public static void Start()
        {
            Client client = new Client();

            var simple = new ConcreteComponent();
            Console.WriteLine("Client: I get a simple component:");
            client.ClientCode(simple);
            Console.WriteLine();

            // Note how decorators can wrap not only simple components but the
            // other decorators as well.
            ConcreteDecoratorA concreteDecoratorA = new ConcreteDecoratorA(simple);
            ConcreteDecoratorB concreteDecoratorB = new ConcreteDecoratorB(concreteDecoratorA);

            Console.WriteLine("Client: Now I've got a decorated component:");
            client.ClientCode(concreteDecoratorB);
        }

        abstract class Component
        { 
            public abstract string Operation();
        }

        // Concrete Components provide default implementations of the operations.
        // There might be several variations of these classes
        class ConcreteComponent : Component
        {
            public override string Operation()
            {
                return nameof(ConcreteComponent);
            }
        }

        // The base Decorator class follows the same interface as the other
        // components. The primary purpose of this class is to define the wrapping
        // interface for all concrete decorators. The default implementation of the
        // wrapping code might include a field for storing a wrapped component and
        // the means to initialize it.
        abstract class Decorator : Component
        {
            protected Component _component;

            public Decorator(Component component)
            {
                _component = component;
            }

            public void SetComponent(Component component)
                => this._component = component;

            // The Decorator delegates all work to the wrapped component.
            public override string Operation()
            {
                if (this._component != null)
                { 
                    return this._component.Operation();
                }

                return string.Empty;
            }
        }

        // Concrete Decorators call the wrapped object and alter its result in some
        // way.
        class ConcreteDecoratorA : Decorator
        {
            public ConcreteDecoratorA(Component component) : base(component)
            {
            }

            // Decorators may call parent implementation of the operation, instead
            // of calling the wrapped object directly. This approach simplifies
            // extension of decorator classes.
            public override string Operation()
            {
                return $"{this.GetType().Name}({base.Operation()})";
            }
        }
        class ConcreteDecoratorB : Decorator
        {
            public ConcreteDecoratorB(Component component) : base(component)
            {
            }

            // Decorators can execute their behavior either before or after the call to
            // a wrapped object.
            public override string Operation()
            {
                return $"{this.GetType().Name}({base.Operation()})";
            }
        }
        class Client
        {
            // The client code works with all objects using the Component interface.
            // This way it can stay independent of the concrete classes of
            // components it works with.
            public void ClientCode(Component component)
            {
                Console.WriteLine("RESULT: " + component.Operation());
            }
        }

    }
}
