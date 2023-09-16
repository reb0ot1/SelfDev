// See https://aka.ms/new-console-template for more information


//for (int task = 1; task < 51; task++)
//{
//    ThreadPool.QueueUserWorkItem(new WaitCallback(TaskCallBack), task);
//    Console.WriteLine("Thread counts: "+ThreadPool.ThreadCount);
//}

var newTask = Task.Run(() =>
{
    Console.WriteLine("Test");
    Console.WriteLine("Thread counts: " + ThreadPool.ThreadCount);
    //Thread.Sleep(10000);
});

Console.WriteLine("Thread counts: " + ThreadPool.ThreadCount);
Thread.Sleep(10000);
Console.WriteLine("Thread counts: " + ThreadPool.ThreadCount);

static void TaskCallBack(Object ThreadNumber)
{
    string ThreadName = "Thread " + ThreadNumber.ToString();
    for (int i = 1; i < 10; i++)
        Console.WriteLine(ThreadName + ": " + i.ToString());
    Console.WriteLine(ThreadName + "Finished...");
}
