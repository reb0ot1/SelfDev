namespace SelfDevelopmentProj.BehavioralPatterns
{
    internal class StrategyPattern
    {
        public static void Start()
        {
            var strategy1 = new Strategy1();
            var strategy2 = new Strategy2();

            var context = new StrategyPatternContext();
            context.SetStrategy(strategy1);

            context.DoContextStuff();

            context.SetStrategy(strategy2);
            context.DoContextStuff();
        }
    }

    class StrategyPatternContext
    {
        private IStrategy _strategy;

        public StrategyPatternContext()
        {

        }

        public StrategyPatternContext(IStrategy strategy)
        {
            _strategy = strategy;
        }

        public void SetStrategy(IStrategy strategy)
        {
            this._strategy = strategy;
        }

        public void DoContextStuff()
        {
            var arr = new List<string> { "d", "a", "e", "f", "b", "c" };

            var result =  this._strategy.DoTheWork(arr);

            Console.WriteLine(string.Join(",", result as List<string>));
        }
    }

    interface IStrategy
    {
        object DoTheWork(object obj);
    }

    class Strategy1 : IStrategy
    {
        public object DoTheWork(object obj)
        {
            List<string> result = obj as List<string>;
            result.Sort();

            return result;
        }
    }

    class Strategy2 : IStrategy
    {
        public object DoTheWork(object obj)
        {
            List<string> result = obj as List<string>;
            result.Sort();
            result.Reverse();

            return result;
        }
    }

}
