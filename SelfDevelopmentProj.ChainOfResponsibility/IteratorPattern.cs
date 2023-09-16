using System.Collections;

namespace SelfDevelopmentProj.BehavioralPatterns
{
    public class IteratorPattern
    {
        public static void Start()
        {
            var collection = new WordsCollection();
            collection.AddItem("First");
            collection.AddItem("Second");
            collection.AddItem("Third");

            foreach (var item in collection)
            {
                Console.WriteLine(item);
            }

            Console.WriteLine("Reverse:");

            collection.ReverseDirection();

            foreach (var item in collection)
            {
                Console.WriteLine(item);
            }
        }
    }

    abstract class Iterator : IEnumerator
    {
        object IEnumerator.Current => Current();

        public abstract int Key();
        public abstract object Current();
        public abstract bool MoveNext();

        public abstract void Reset();
    }

    abstract class IteratorAggregate : IEnumerable
    {
        public abstract IEnumerator GetEnumerator();
    }

    class AphabeticOrderIterator : Iterator
    {
        private WordsCollection _collection;
        private bool _reverse;
        private int _position = -1;

        public AphabeticOrderIterator(WordsCollection collection, bool reverse = false)
        {
            this._collection = collection;
            this._reverse = reverse;

            if (reverse)
            {
                this._position = this._collection.GetItems().Count;
            }
        }

        public override object Current()
        {
            return this._collection.GetItems()[this._position];
        }

        public override int Key()
        {
            return this._position;
        }

        public override bool MoveNext()
        {
            var updatedPossition = this._position + (this._reverse ? -1 : 1);
            if (updatedPossition >= 0 && updatedPossition < this._collection.GetItems().Count)
            {
                this._position = updatedPossition;
                return true;
            }

            return false;
        }

        public override void Reset()
        {
            this._position = this._reverse ? this._collection.GetItems().Count - 1 : 0;
        }
    }

    class WordsCollection : IteratorAggregate
    {
        List<string> _collection = new List<string>();
        bool _direction;

        public void ReverseDirection()
        {
            _direction = !_direction;
        }

        public List<string> GetItems()
            => this._collection;

        public void AddItem(string item)
            => this._collection.Add(item);

        public override IEnumerator GetEnumerator()
        {
            return new AphabeticOrderIterator(this, this._direction);
        }
    }
}
