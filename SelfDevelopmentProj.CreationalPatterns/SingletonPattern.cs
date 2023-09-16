namespace SelfDevelopmentProj.CreationalPatterns
{
    public class SingletonPattern
    {
        public static void Start()
        {
            var sing1 = Singleton.GetInstance();
            var sing2 = Singleton.GetInstance();
            var varr1 = sing1.GetHashCode();
            var varr2 = sing1.GetHashCode();

            Console.WriteLine(varr1);
            Console.WriteLine(varr2);

            if (sing1 == sing2)
            {
                Console.WriteLine("Singleton works, both variables contain the same instance.");
            }
            else
            {
                Console.WriteLine("Singleton failed, variables contain different instances.");
            }
        }

        public static void StartAsync()
        {
            Console.WriteLine(
                "{0}\n{1}\n\n{2}\n",
                "If you see the same value, then singleton was reused (yay!)",
                "If you see different values, then 2 singletons were created (booo!!)",
                "RESULT:"
            );

            var thread1 = new Thread(() =>
            {
                TestSingleton("VAL1");
            });

            var thread2 = new Thread(() =>
            {
                TestSingleton("VAL2");
            });

            thread1.Start();
            thread2.Start();

            thread1.Join();
            thread2.Join();
        }

        private static void TestSingleton(string value)
        {
            SingletonThreadSafe singleton = SingletonThreadSafe.GetInstance(value);
            Console.WriteLine(singleton.Value);
        }

        public sealed class Singleton
        {
            private static Singleton _instance;

            private Singleton()
            {
            }

            public static Singleton GetInstance()
            {
                if (_instance == null)
                {
                    _instance = new Singleton();
                }

                return _instance;
            }

            public void SomeBusinessLogin()
            { 
                
            }
        }

        public sealed class SingletonThreadSafe
        {
            private readonly static object _lock = new object();

            private static SingletonThreadSafe _instance;

            private SingletonThreadSafe()
            {
            }

            public static SingletonThreadSafe GetInstance(string valuePassed)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new SingletonThreadSafe();
                        _instance.Value = valuePassed;
                    }
                }

                return _instance;
            }

            public void SomeBusinessLogin()
            {

            }

            public string Value { get; set; }
        }
    }
}
