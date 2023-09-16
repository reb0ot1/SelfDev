namespace SelfDevelopmentProj.StructuralPattern
{
    public class CompositePattern
    {
        public static void Start()
        {
            var client = new Client();
            var leaf = new Leaf();
            Console.WriteLine("Client: I get a simple component:");
            client.ClientCode(leaf);

            Composite tree = new Composite();
            Composite branch1 = new Composite();
            branch1.AddComponent(new Leaf());
            branch1.AddComponent(new Leaf());

            Composite branch2 = new Composite();
            branch2.AddComponent(new Leaf());

            tree.AddComponent(branch1);
            tree.AddComponent(branch2);

            Console.WriteLine("Client: Now I've got a composite tree:");
            client.ClientCode(tree);

            Console.Write("Client: I don't need to check the components classes even when managing the tree:\n");
            client.ClientCode2(tree, leaf);
        }

        class Client
        {
            public void ClientCode(Component leaf)
            {
                Console.WriteLine($"RESULT: {leaf.Operation()}\n");
            }

            public void ClientCode2(Component component1, Component component2)
            {
                if (component1.IsComposite())
                {
                    component1.AddComponent(component2);
                }

                Console.WriteLine($"RESULT: {component1.Operation()}");
            }
        }

        // The base Component class declares common operations for both simple and
        // complex objects of a composition.
        abstract class Component
        {
            public Component()
            {

            }

            // The base Component may implement some default behavior or leave it to
            // concrete classes (by declaring the method containing the behavior as
            // "abstract").
            public abstract string Operation();

            // In some cases, it would be beneficial to define the child-management
            // operations right in the base Component class. This way, you won't
            // need to expose any concrete component classes to the client code,
            // even during the object tree assembly. The downside is that these
            // methods will be empty for the leaf-level components.
            public virtual void AddComponent(Component component)
            { 
                throw new NotImplementedException();
            }

            public virtual void RemoveComponent(Component component)
            {
                throw new NotImplementedException();
            }

            // You can provide a method that lets the client code figure out whether
            // a component can bear children.
            public virtual bool IsComposite()
            {
                return true;
            }
        }

        class Leaf : Component
        {
            public override string Operation()
            {
                return "Leaf";
            }

            public override bool IsComposite()
            {
                return false;
            }
        }

        class Composite : Component
        {
            protected readonly List<Component> components = new List<Component>();

            public override string Operation()
            {
                int i = 0;
                string result = "Branch(";

                foreach (Component comp in components)
                {
                    result += comp.Operation();
                    if (i != this.components.Count - 1)
                    {
                        result += "+";
                    }
                }

                result += ")";

                return result;
            }

            public override void AddComponent(Component component)
            {
                this.components.Add(component);
            }

            public override void RemoveComponent(Component component)
            {
                this.components.Remove(component);
            }
        }
    }
}
