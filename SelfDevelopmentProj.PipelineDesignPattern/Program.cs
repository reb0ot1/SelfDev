using static System.Runtime.InteropServices.JavaScript.JSType;

var pipeline = new Pipeline<string>();

pipeline.RegisterOperation(new Operation<string>(x => {
    Console.WriteLine($"Everyone likes {x}!");
    return true;
}));

pipeline.RegisterOperation(new WriteOperation());

pipeline.RegisterOperation(new Operation<string>(x =>
{
    if (x == "banana")
    {
        Console.WriteLine("This banana made the pipeline abort...");
        return false;
    }
    return true;
}));


pipeline.RegisterOperation(new Operation<string>(data => Console.WriteLine("This operation should not be called !")));

var verbose = new Pipeline<string>();
verbose.RegisterOperation(new Operation<string>(data => Console.WriteLine("Beginning of the pipeline...")));
verbose.RegisterOperation(pipeline);
verbose.RegisterOperation(new Operation<string>(data => Console.WriteLine("End of the pipeline...")));

verbose.Invoke("banana");

Console.WriteLine("The pipeline is asynchronous, so we should have more messages after this one : ");

public interface IOperation<T>
{
    void SetNext(IOperation<T> next);
    void Invoke(T data);
}

public class Pipeline<T> : IOperation<T>
{
    //private readonly List<IOperation<T>> _operations = new List<IOperation<T>>();
    public readonly LinkedList<IOperation<T>> Operations = new LinkedList<IOperation<T>>();
    private readonly IOperation<T> _terminate;
    public Pipeline()
    {
        this._terminate = new Operation<T>(x => { });
    }

    public void SetNext(IOperation<T> next)
    {
        _terminate.SetNext(next);
    }

    public void RegisterOperation(IOperation<T> operation)
    { 
        operation.SetNext(this._terminate);
        if (this.Operations.Any())
        {
            this.Operations.Last().SetNext(operation);
        }

        Operations.Add(operation);
    }

    public void Invoke(T data)
    {
        var operation = this.Operations.Any() ? this.Operations.First() : this._terminate;
        operation.Invoke(data);
    }
}

//public class ReverseOperation : IOperation<string>
//{
//    public bool Invoke(string data)
//    {
//        var reversedData = data.Reverse();
//        Console.WriteLine($"The original string {data} is reversed -> {string.Join("",reversedData)}");
//        return true;
//    }
//}

public class WriteOperation : IOperation<string>
{
    private IOperation<string> _next;

    public void Invoke(string data)
    {
        Task.Run(() =>
        {
            Console.WriteLine("Writing data to the dist ...");
            Thread.Sleep(100);
            Console.WriteLine("Data successfully written to the disk !");

            this._next?.Invoke(data);
        });
    }

    public void SetNext(IOperation<string> next)
    {
        this._next = next;
    }
}

public class BatchPipeline<T> : IOperation<T[]>
{
    private readonly Pipeline<T> _pipeline;

    public BatchPipeline(Pipeline<T> pipeline)
    {
        this._pipeline = pipeline;
    }

    public void Invoke(T[] data)
    {
        var items = data.Select(item => new Result<T>(item)).ToArray();

        foreach (var operation in this._pipeline.Operations)
        {
            var failed = true;

            foreach (var item in items)
            {
                if (!item.Success) continue;
                if (!operation.Invoke(item.Data))
                {
                    item.Fail();
                }
            }
        }
    }

    public void SetNext(IOperation<T[]> next)
    {
        throw new NotImplementedException();
    }
}


public class Operation<T> : IOperation<T>
{
    private readonly Func<T, bool> _action;
    private IOperation<T> _next;

    public Operation(Func<T, bool> action)
    {
        _action = action;
    }

    public Operation(Action<T> action)
    {
        this._action = data => { 
            action(data);
            return true;
        };
    }

    public void Invoke(T data)
    {
        if (this._action(data)) this._next.Invoke(data);
    }

    public void SetNext(IOperation<T> next)
    {
        this._next = next;
    }
}

class Result<T>
{
    public Result(T data)
    {
        this.Data = data;
    }

    public readonly T Data;
    public bool Success { get; private set; } = true;
    public void Fail() => Success = false;
}