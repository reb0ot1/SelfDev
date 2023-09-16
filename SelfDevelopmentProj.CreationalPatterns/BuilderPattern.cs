using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfDevelopmentProj.CreationalPatterns
{
    public class BuilderPattern
    {
        public static void Start()
        {
            var builder = new ConcreteBuilder();

            builder.BuildPartA();

            var product = builder.GetProduct();

            Console.WriteLine(product.ListParts());
        }

        interface IBuilder
        {
            void BuildPartA();
            void BuildPartB();
            void BuildPartC();
        }

        class ConcreteBuilder : IBuilder
        {
            private Product _product;

            public ConcreteBuilder()
            {
                this.Reset();
            }

            private void Reset()
            {
                this._product = new();
            }

            public void BuildPartA()
            {
                this._product.Add("Part A");
            }

            public void BuildPartB()
            {
                this._product.Add("Part B");
            }

            public void BuildPartC()
            {
                this._product.Add("Part C");
            }

            public Product GetProduct()
            {
                var result = this._product;

                this.Reset();

                return result;
            }
        }

        class Product
        {
            private List<object> _parts = new();

            public void Add(string part)
            {
                this._parts.Add(part);
            }

            public string ListParts()
            {
                var str = string.Join(", ", this._parts);

                return str;
            }
        }

        class Director
        {
            private IBuilder _builder;

            public Director(IBuilder builder)
            {
                _builder = builder;
            }

            public void BuildBaseParts()
            {
                this._builder.BuildPartA();
            }

            public void BuildFullFeaturedProduct()
            {
                this._builder.BuildPartA();
                this._builder.BuildPartB();
                this._builder.BuildPartC();
            }
        }
    }
}
