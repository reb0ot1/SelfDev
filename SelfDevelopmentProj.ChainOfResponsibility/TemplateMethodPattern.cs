namespace SelfDevelopmentProj.BehavioralPatterns
{
    internal class TemplateMethodPattern
    {
        public static void Start()
        {
            Console.WriteLine("Same client code can work with different subclasses:");
            Client.ClientCode(new Class1());

            Console.WriteLine();

            Console.WriteLine("Same client code can work with different subclasses:");
            Client.ClientCode(new Class2());
        }
    }

    abstract class AbstractClass
    {
        public void TemplateMethod()
        {
            this.BaseOperation1();
            this.RequiredOperations1();
            this.BaseOperation2();
            this.Hook1();
            this.RequiredOperation2();
            this.BaseOperation3();
            this.Hook2();
        }

        private void BaseOperation1()
        {
            Console.WriteLine("AbstractClass says: I am doing the bulk of the work");
        }

        private void BaseOperation2()
        {
            Console.WriteLine("AbstractClass says: But I let subclasses override some operations");
        }

        private void BaseOperation3()
        {
            Console.WriteLine("AbstractClass says: But I am doing the bulk of the work anyway");
        }

        public abstract void RequiredOperations1();

        public abstract void RequiredOperation2();

        protected virtual void Hook1()
        {
        }

        protected virtual void Hook2()
        {
        }
    }

    class Class1 : AbstractClass
    {
        public override void RequiredOperation2()
        {
            Console.WriteLine("ConcreteClass1 says: Implemented Operation2");
        }

        public override void RequiredOperations1()
        {
            Console.WriteLine("ConcreteClass1 says: Implemented Operation1");
        }
    }
    class Class2 : AbstractClass
    {
        public override void RequiredOperation2()
        {
            Console.WriteLine("ConcreteClass2 says: Implemented Operation2");
        }

        public override void RequiredOperations1()
        {
            Console.WriteLine("ConcreteClass2 says: Implemented Operation1");
        }

        protected override void Hook1()
        {
            Console.WriteLine("ConcreteClass2 says: Overridden Hook1");
        }
    }

    class Client
    {
        // The client code calls the template method to execute the algorithm.
        // Client code does not have to know the concrete class of an object it
        // works with, as long as it works with objects through the interface of
        // their base class.
        public static void ClientCode(AbstractClass absClass)
        {
            absClass.TemplateMethod();
        }
    }

}
