namespace SelfDevelopmentProj.StructuralPattern
{
    public class ProxyPattern
    {
        public static void Start()
        {
            var client = new Client();
            Console.WriteLine("Client: Executing the client code with a real subject:");
            var realSubject = new RealSubject();
            client.ClientCode(realSubject);

            Console.WriteLine();

            Console.WriteLine("Client: Executing the same client code with a proxy:");
            var proxySubject = new Proxy(realSubject);
            client.ClientCode(proxySubject);
        }

        class Client
        {
            // The client code is supposed to work with all objects (both subjects
            // and proxies) via the Subject interface in order to support both real
            // subjects and proxies. In real life, however, clients mostly work with
            // their real subjects directly. In this case, to implement the pattern
            // more easily, you can extend your proxy from the real subject's class.
            public void ClientCode(ISubject subject)
            {
                // ...

                subject.Request();

                // ...
            }
        }

        interface ISubject
        {
            void Request();
        }

        class RealSubject : ISubject
        {
            public void Request()
            {
                Console.WriteLine("RealSubject: Handling Request.");
            }
        }

        class Proxy : ISubject
        {
            private readonly ISubject _realSubject;

            public Proxy(ISubject subject)
            {
                _realSubject = subject;
            }

            public bool CheckAccess()
            {
                // Some real checks should go here.
                Console.WriteLine("Proxy: Checking access prior to firing a real request.");

                return true;
            }

            public void LogAccess()
            {
                Console.WriteLine("Proxy: Logging the time of request.");
            }

            // The most common applications of the Proxy pattern are lazy loading,
            // caching, controlling the access, logging, etc. A Proxy can perform
            // one of these things and then, depending on the result, pass the
            // execution to the same method in a linked RealSubject object.
            public void Request()
            {
                if (this.CheckAccess())
                {
                    this._realSubject.Request();

                    this.LogAccess();
                }
            }
        }
    }
}
