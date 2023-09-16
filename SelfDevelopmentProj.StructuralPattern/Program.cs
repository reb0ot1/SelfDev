// See https://aka.ms/new-console-template for more information

using SelfDevelopmentProj.StructuralPattern;


Console.WriteLine("List of patterns to view:");
Console.WriteLine("adapter, bridge, composite, decorator, facade, flyweight, proxy");

Console.WriteLine("Please write a pattern:");

var patternUsed = Console.ReadLine();

switch (patternUsed)
{
	case "adapter":
        //Adapter is a structural design pattern, which allows incompatible objects to collaborate.
        AdapterPattern.Start();
        break;
    case "bridge":
        //Bridge is a structural design pattern that divides business logic or
        //huge class into separate class hierarchies that can be developed independently.
        BridgePattern.Start();
        break;
    case "composite":
        //Composite is a structural design pattern that allows composing objects
        //into a tree-like structure and work with the it as if it was a singular object.
        CompositePattern.Start();
        break;
    case "decorator":
        //Decorator is a structural pattern that allows adding new behaviors to objects dynamically
        //by placing them inside special wrapper objects, called decorators.
        DecoratorPattern.Start();
        break;
    case "facade":
        //Facade is a structural design pattern that provides a simplified (but limited)
        //interface to a complex system of classes, library or framework.
        FacadePattern.Start();
        break;
    case "flyweight":
        //Flyweight is a structural design pattern that lets you fit more objects
        //into the available amount of RAM by sharing common parts of state
        //between multiple objects instead of keeping all of the data in each object.
        FlyWeightPattern.Start();
        break;
    case "proxy":
        //Proxy is a structural design pattern that provides an object that acts as a substitute
        //for a real service object used by a client. A proxy receives client requests,
        //does some work (access control, caching, etc.) and then passes the request to a service object.
        ProxyPattern.Start();
        break;
    default:
        Console.WriteLine("No pattern matched.");
		break;
}