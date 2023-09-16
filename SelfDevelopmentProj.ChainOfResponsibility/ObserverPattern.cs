namespace SelfDevelopmentProj.BehavioralPatterns
{
    internal class ObserverPattern
    {
        public static void Start()
        { 
            var subject = new Subject();
            var observer1 = new ObserverFirst();
            subject.Attach(observer1);

            var observer2 = new ObserverSecond();
            subject.Attach(observer2);

            subject.SomeBusinessLogic();
            subject.SomeBusinessLogic();

            subject.Detach(observer2);
            subject.SomeBusinessLogic();
        }
    }

    interface IObserver
    {
        void Update(ISubject subject);
    }

    interface ISubject
    {
        void Attach(IObserver observer);

        void Detach(IObserver observer);

        void Notify();
    }

    class Subject : ISubject
    {
        private IList<IObserver> _observers = new List<IObserver>();

        public int State { get; set; } = -0;

        public void Attach(IObserver observer)
        {
            Console.WriteLine("Subject: Attached an observer.");
            this._observers.Add(observer);
        }

        public void Detach(IObserver observer)
        {
            this._observers.Remove(observer);
            Console.WriteLine("Subject: Detached an observer.");
        }

        public void Notify()
        {
            Console.WriteLine("Subject: Notifying observers...");
            foreach (var observer in this._observers)
            {
                observer.Update(this);
            }
        }

        public void SomeBusinessLogic()
        {
            Console.WriteLine("\nSubject: I'm doing something important.");
            this.State = new Random().Next(0, 10);

            Thread.Sleep(15);

            Console.WriteLine("Subject: My state has just changed to: " + this.State);
            this.Notify();
        }
    }

    class ObserverFirst : IObserver
    {
        public void Update(ISubject subject)
        {
            if ((subject as Subject).State < 3)
            {
                Console.WriteLine("ConcreteObserverA: Reacted to the event.");
            }
        }
    }

    class ObserverSecond : IObserver
    {
        public void Update(ISubject subject)
        {
            if ((subject as Subject).State == 0 || (subject as Subject).State >= 2)
            {
                Console.WriteLine("ConcreteObserverB: Reacted to the event.");
            }
        }
    }
}
