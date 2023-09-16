using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfDevelopmentProj.BehavioralPatterns
{
    public class MementoPattern
    {
        public static void Start()
        {
            Originator originator = new Originator("This is my initial state.");
            CareTaker careTaker = new CareTaker(originator);

            careTaker.BackUp();
            originator.DoSomething();

            careTaker.BackUp();
            originator.DoSomething();
            careTaker.BackUp();

            Console.WriteLine();
            careTaker.ShowHistory();
        }

        class Originator
        {
            private string _state;


            public Originator(string state)
            {
                this._state = state;
            }

            public void DoSomething()
            {
                Console.WriteLine("Originator: I'm doing something important.");
                this._state = this.GenerateRandomString(30);
                Console.WriteLine($"Originator: and my state has changed to: {_state}");
            }

            public IMemento Save()
            { 
                return new ConcreteMemento(this._state);
            }

            public void Restore(IMemento memento)
            {
                if (!(memento is ConcreteMemento))
                {
                    throw new Exception("Unknown memento class " + memento.ToString());
                }

                this._state = memento.GetState();
                Console.Write($"Originator: My state has changed to: {_state}");
            }

            private string GenerateRandomString(int v = 10)
            {
                string allowedSymbols = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
                string result = string.Empty;

                while (v > 0)
                {
                    result += allowedSymbols[new Random().Next(0, allowedSymbols.Length)];
                    Thread.Sleep(12);

                    v--;
                }

                return result;
            }
        }

        interface IMemento
        {
            string GetName();
            string GetState();
            DateTime GetDate();
        }

        class ConcreteMemento : IMemento
        {
            private string _state;

            private DateTime _date;

            public ConcreteMemento(string state)
            {
                this._state=state;
                this._date = DateTime.UtcNow;
            }

            public DateTime GetDate()
            {
                return this._date;
            }

            public string GetName()
            {
                return $"{this._date} / ({this._state.Substring(0, 9)})...";
            }

            public string GetState()
            {
                return this._state;
            }
        }

        class CareTaker
        {
            private IList<IMemento> _mementos = new List<IMemento>();

            private Originator _originator;

            public CareTaker(Originator originator)
            {
                this._originator = originator;
            }

            public void BackUp()
            {
                Console.WriteLine("\nCaretaker: Saving Originator's state...");
                this._mementos.Add(this._originator.Save());
            }

            public void Undo() 
            {
                if (!this._mementos.Any())
                {
                    return;
                }

                var mem = this._mementos.Last();
                this._mementos.Remove(mem);

                Console.WriteLine("Caretaker: Restoring state to: " + mem.GetName());

                try
                {
                    this._originator.Restore(mem);
                }
                catch (Exception)
                {
                    this.Undo();
                }
            }

            public void ShowHistory()
            {
                Console.WriteLine("Caretaker: Here's the list of mementos:");

                foreach (var memento in this._mementos)
                {
                    Console.WriteLine(memento.GetName());
                }
            }
        }
    }
}
