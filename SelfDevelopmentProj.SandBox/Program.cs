using System.Diagnostics.Tracing;
using System.Threading.Channels;
using System.Threading.Tasks.Sources;
using System.IO.Pipelines;
using System;

namespace SelfDevelopmentProj.SandBox
{
    internal class Program
    {
        //()()
        //)))(((
        //()))
        //((()
        //((()())))
        //((()))())))
        //))()))(

        public static int GetOper2(string str)
        {
            var char1Counter = 0;
            var char2Counter = 0;
            var char1 = '(';
            var char2 = ')';
            var open = false;
            var close = false;
            var oper = 0;

            for (int i = 0; i < str.Length; i++)
            {
                if (char1 == str[i])
                {
                    if (!open)
                    {
                        open = true;
                        close = false;
                        char1Counter = 0;
                        char2Counter = 0;
                    }

                    char1Counter++;
                    oper++;
                }

                if (char2 == str[i])
                {
                    if (!close)
                    {
                        open = false;
                        close = true;
                    }

                    char2Counter++;

                    if (char1Counter < char2Counter)
                    {
                        oper++;
                    }
                    else
                    {
                        oper--;
                    }
                }
            }

            return Math.Abs(oper);
        }
        public static int GetOper(string str)
        {
            int result = 0;
            var splitedString = str.Split("()");

            if (splitedString.Length == 1)
            {
                for (int j = 0; j < splitedString[0].Length; j++)
                {
                    if (splitedString[0][j] == ')')
                    {
                        result++;
                    }

                    if (splitedString[0][j] == '(')
                    {
                        result++;
                    }
                }
            }
            else
            {
                for (int i = 0; i < splitedString.Length - 1; i++)
                {
                    var char1CountLeft = 0;
                    var char2CountRigth = 0;

                    var leftStr = splitedString[i];
                    var rigthStr = splitedString[i + 1];

                    if (i == 0)
                    {
                        for (int j = 0; j < leftStr.Length; j++)
                        {
                            if (leftStr[j] == ')')
                            {
                                result++;
                            }
                        }
                    }

                    if (i == splitedString.Length - 2)
                    {
                        for (int j = 0; j < leftStr.Length; j++)
                        {
                            if (rigthStr[j] == '(')
                            {
                                result++;
                            }
                        }
                    }

                    for (int j = 0; j < leftStr.Length; j++)
                    {
                        if (leftStr[j] == '(')
                        {
                            char1CountLeft++;
                        }
                    }

                    for (int j = 0; j < rigthStr.Length; j++)
                    {
                        if (rigthStr[i] == ')')
                        {
                            char2CountRigth++;
                        }
                    }

                    var resultChar1 = Math.Abs(char1CountLeft - char2CountRigth);

                    result += resultChar1;

                }
            }

            return result;

        }

        static async Task Main(string[] args)
        {
            //()()
            //)))(((
            //()))
            //((()
            //((()())))
            //((()))())))
            //))()))(
            var str = "(((((((((((((((((((((((())";
            int[] array = { 52, 96, 67, 71, 42, 38, 39, 40, 14 };
            var newArray = SortArray(array, 0, array.Length - 1);
            var a = new String("ttest1");
            var b = new String("ttest2");

            b = a;

            a = new String("test3");

            Console.WriteLine(b);

            //Console.WriteLine(GetOper(str));
            //Console.WriteLine(GetOper2(str));

            //var distance = 20;
            //var left = 0;
            //var rigth = 20;

            //var list = new List<string>();

            //for (int i = 1; i < 20; i++)
            //{
            //    var leftResult = left + i;
            //    var rigthResult = rigth - i;
            //    var message = string.Format("{0}{1}{2}", new string('*', leftResult), new string(' ', distance), new string('*', rigthResult));
            //    Console.WriteLine(message);
            //    Thread.Sleep(500);
            //}

            //for (int i = 1; i < 20; i++)
            //{
            //    var leftResult = left + i;
            //    var rigthResult = rigth - i;
            //    Console.WriteLine(string.Format("{0}{1}{2}", new string('*', rigthResult), new string(' ', distance), new string('*', leftResult)));
            //    Thread.Sleep(500);
            //}
            //int a = 15;
            //int b = 3;

            //int c = a << b;

            //Console.WriteLine(c);

            //var list = new List<KeyValuePair<string, int>>();

            //list.Add(new KeyValuePair<string, int>("tt1", 5));
            //list.Add(new KeyValuePair<string, int>("tt1", 6));

            //var pipe = new Pipe();
            //var tasks = new List<Task>();

            ////Semaphores(pipe, tasks);
            //Channels(pipe, tasks);

            //var consumer = Task.Run(async () =>
            //{

            //    var reader = pipe.Reader;
            //    while (true)
            //    {
            //        var result = await reader.ReadAsync();
            //        var buffer = result.Buffer;
            //        ServiceEventSource.Log.ConsumeBytes(buffer.Length);
            //        reader.AdvanceTo(buffer.End);
            //    }
            //});

            //tasks.Add(consumer);

            //await Task.WhenAll(tasks);
        }

        //private static void Channels(Pipe pipe, List<Task> tasks)
        //{
        //    var channel = Channel.CreateBounded<(byte[], ManualResetValueTaskSource<object?>)>(new BoundedChannelOptions(100)
        //    {
        //        SingleReader = true
        //    });

        //    // Multiple producers
        //    for (int i = 0; i < 50; i++)
        //    {
        //        var producer1 = Task.Run(async () =>
        //        {
        //            var buffer = new byte[4096];

        //            var mrvts = new ManualResetValueTaskSource<object?>();

        //            while (true)
        //            {
        //                Array.Fill(buffer, (byte)i);
        //                await channel.Writer.WriteAsync((buffer, mrvts));
        //                await new ValueTask(mrvts, mrvts.Version);
        //                mrvts.Reset();
        //            }
        //        });
        //    }

        //    var producer = Task.Run(async () =>
        //    {
        //        var writer = pipe.Writer;

        //        await foreach (var (data, ch) in channel.Reader.ReadAllAsync())
        //        {
        //            await writer.WriteAsync(data);
        //            ch.SetResult(null);
        //        }
        //    });

        //    tasks.Add(producer);
        //}

        public static int[] SortArray(int[] array, int leftIndex, int rightIndex)
        {
            var i = leftIndex;
            var j = rightIndex;
            var pivot = array[leftIndex];
            while (i <= j)
            {
                while (array[i] < pivot)
                {
                    i++;
                }

                while (array[j] > pivot)
                {
                    j--;
                }
                if (i <= j)
                {
                    int temp = array[i];
                    array[i] = array[j];
                    array[j] = temp;
                    i++;
                    j--;
                }
            }

            if (leftIndex < j)
                SortArray(array, leftIndex, j);
            if (i < rightIndex)
                SortArray(array, i, rightIndex);
            return array;
        }

        private static void Semaphores(Pipe pipe, List<Task> tasks)
        {
            var s = new SemaphoreSlim(1);
            // Multiple producers
            for (int i = 0; i < 50; i++)
            {
                var producer = Task.Run(async () =>
                {
                    var writer = pipe.Writer;

                    var buffer = new byte[4096];

                    while (true)
                    {
                        await s.WaitAsync();
                        try
                        {
                            Array.Fill(buffer, (byte)i);
                            await writer.WriteAsync(buffer);
                        }
                        finally
                        {
                            s.Release();
                        }
                    }
                });

                tasks.Add(producer);
            }
        }

    }
    //public class ServiceEventSource : EventSource
    //{
    //    public static ServiceEventSource Log = new ServiceEventSource();
    //    private IncrementingPollingCounter? _invocationRateCounter;

    //    public long _bytesConsumed;

    //    public ServiceEventSource() : base("MyApp")
    //    {

    //    }

    //    protected override void OnEventCommand(EventCommandEventArgs command)
    //    {
    //        if (command.Command == EventCommand.Enable)
    //        {
    //            _invocationRateCounter = new IncrementingPollingCounter("transfer-rate", this, () => Volatile.Read(ref _bytesConsumed))
    //            {
    //                DisplayRateTimeScale = TimeSpan.FromSeconds(1),
    //                DisplayUnits = "B"
    //            };
    //        }
    //    }

    //    internal void ConsumeBytes(long bytesConsumed)
    //    {
    //        Interlocked.Add(ref _bytesConsumed, bytesConsumed);
    //    }
    //}
}

//internal sealed class ManualResetValueTaskSource<T> : IValueTaskSource<T>, IValueTaskSource
//{
//    private ManualResetValueTaskSourceCore<T> _core; // mutable struct; do not make this readonly

//    public bool RunContinuationsAsynchronously { get => _core.RunContinuationsAsynchronously; set => _core.RunContinuationsAsynchronously = value; }
//    public short Version => _core.Version;
//    public void Reset() => _core.Reset();
//    public void SetResult(T result) => _core.SetResult(result);
//    public void SetException(Exception error) => _core.SetException(error);

//    public T GetResult(short token) => _core.GetResult(token);
//    void IValueTaskSource.GetResult(short token) => _core.GetResult(token);
//    public ValueTaskSourceStatus GetStatus(short token) => _core.GetStatus(token);
//    public void OnCompleted(Action<object?> continuation, object? state, short token, ValueTaskSourceOnCompletedFlags flags) => _core.OnCompleted(continuation, state, token, flags);

//    public ValueTaskSourceStatus GetStatus() => _core.GetStatus(_core.Version);

//    public void TrySetResult(T result)
//    {
//        if (_core.GetStatus(_core.Version) == ValueTaskSourceStatus.Pending)
//        {
//            _core.SetResult(result);
//        }
//    }
//}