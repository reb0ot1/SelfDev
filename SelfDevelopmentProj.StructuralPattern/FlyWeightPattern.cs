using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SelfDevelopmentProj.StructuralPattern
{
    public class FlyWeightPattern
    {
        public static void Start()
        {
            // The client code usually creates a bunch of pre-populated
            // flyweights in the initialization stage of the application.
            var factory = new FlyweightFactory(
                new Car { Company = "Chevrolet", Model = "Camaro2018", Color = "pink" },
                new Car { Company = "Mercedes Benz", Model = "C300", Color = "black" },
                new Car { Company = "Mercedes Benz", Model = "C500", Color = "red" },
                new Car { Company = "BMW", Model = "M5", Color = "red" },
                new Car { Company = "BMW", Model = "X6", Color = "white" }
            );
            factory.ListFlyweights();

            AddCarToPoliceDatabase(factory, new Car
            {
                Number = "CL234IR",
                Owner = "James Doe",
                Company = "BMW",
                Model = "M5",
                Color = "red"
            });

            AddCarToPoliceDatabase(factory, new Car
            {
                Number = "CL234IR",
                Owner = "James Doe",
                Company = "BMW",
                Model = "X1",
                Color = "red"
            });

            factory.ListFlyweights();
        }

        private static void AddCarToPoliceDatabase(FlyweightFactory factory, Car car)
        {
            Console.WriteLine("\nClient: Adding a car to database.");

            Flyweight flyweight = factory.GetFlyweight(new Car
            {
                Color = car.Color,
                Company = car.Company,
                Model = car.Model
            });

            flyweight.Operation(car);
        }

        // The Flyweight stores a common portion of the state (also called intrinsic
        // state) that belongs to multiple real business entities. The Flyweight
        // accepts the rest of the state (extrinsic state, unique for each entity)
        // via its method parameters.
        class Flyweight
        {
            private Car _sharedState;

            public Flyweight(Car car)
            {
                _sharedState = car;
            }

            public void Operation(Car uniqueState)
            {
                string s = JsonSerializer.Serialize(this._sharedState);
                string u = JsonSerializer.Serialize(uniqueState);

                Console.WriteLine($"Flyweight: Displaying shared {s} and unique {u} state.");
            }
        }

        class FlyweightFactory
        { 
            private List<Tuple<Flyweight, string>> flyweights = new List<Tuple<Flyweight, string>>();

            public FlyweightFactory(params Car[] args) 
            {
                foreach (var elem in args)
                {
                    flyweights.Add(new Tuple<Flyweight, string>(new Flyweight(elem), this.GetKey(elem)));
                }
            }

            public Flyweight GetFlyweight(Car shareState)
            {
                string key = this.GetKey(shareState);
                var existingEntity = flyweights.FirstOrDefault(e => e.Item2 == key);
                if (existingEntity is null)
                {
                    Console.WriteLine("FlyweightFactory: Can't find a flyweight, creating new one.");
                    this.flyweights.Add(new Tuple<Flyweight, string>(new Flyweight(shareState), key));
                }
                else
                {
                    Console.WriteLine("FlyweightFactory: Reusing existing flyweight.");
                }

                existingEntity = flyweights.FirstOrDefault(e => e.Item2 == key);

                return existingEntity.Item1;
            }

            public void ListFlyweights()
            {
                var count = flyweights.Count;
                Console.WriteLine($"\nFlyweightFactory: I have {count} flyweights:");
                foreach (var flyweight in flyweights)
                {
                    Console.WriteLine(flyweight.Item2);
                }
            }

            private string GetKey(Car key)
            {
                List<string> elements = new();
                elements.Add(key.Model);
                elements.Add(key.Color);
                elements.Add(key.Company);

                if (key.Owner is not null && key.Number is not null)
                { 
                    elements.Add(key.Owner.ToString());
                    elements.Add(key.Number.ToString());
                }

                elements.Sort();

                return string.Join("_", elements);
            }
        }

        class Car
        {
            public string Owner { get; set; }

            public string Number { get; set; }

            public string Company { get; set; }

            public string Model { get; set; }

            public string Color { get; set; }
        }
    }
}
