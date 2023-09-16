namespace SelfDevelopmentProj.BehavioralPattern
{
    public class ChainOfResponsibility
    {
        public static void Start()
        {
            var monkey = new MonkeyHandler();
            var dog = new DogHandler();
            var cat = new CatHandler();

            monkey.SetNext(dog).SetNext(cat);

            Console.WriteLine("Chain {0}>{1}>{2}", "monkey", "dog", "cat");

            Client.ClientCode(monkey);

            Console.WriteLine();
        }
    }

    interface IHandler
    {
        IHandler SetNext(IHandler handler);

        object Handle(object request);
    }

    abstract class AbstractHandler : IHandler
    {
        private IHandler _nextHandler;

        public IHandler SetNext(IHandler handler)
        {
            this._nextHandler = handler;

            return this._nextHandler;
        }

        public virtual object Handle(object request)
        {
            if (this._nextHandler != null)
            {
                return this._nextHandler.Handle(request);
            }
            else
            {
                return null;
            }
        }
    }

    class MonkeyHandler : AbstractHandler
    {
        public override object Handle(object request)
        {
            if ((request as string) != "Banana")
            {
                return "Monkey \n";
            }
            else
            {
                return base.Handle(request);
            }
        }
    }

     class CatHandler : AbstractHandler
    {
        public override object Handle(object request)
        {
            if ((request as string) != "Fish")
            {
                return "Cat \n";
            }
            else
            {
                return base.Handle(request);
            }
        }
    }

    class DogHandler : AbstractHandler
    {
        public override object Handle(object request)
        {
            if ((request as string) != "Meat ball")
            {
                return "DOG \n";
            }
            else
            {
                return base.Handle(request);
            }
        }
    }

    class Client
    {
        public static void ClientCode(AbstractHandler handler)
        {
            Console.WriteLine("Start");

            foreach (var food in new List<string> { "Nut", "Banana", "Fish" })
            {
                var result = handler.Handle(food);

                if (result != null)
                {
                    Console.Write(result);
                }
                else
                {
                    Console.Write("Food left untouched.");
                }
            }
        }
    }
}
