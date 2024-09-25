using Integration.Backend;
using Integration.Service;

namespace Integration;

public abstract class Program
{
    public static void Main(string[] args)
    {
        var service = new ItemIntegrationService();
        Console.WriteLine("ItemIntegrationService:");

        ThreadPool.QueueUserWorkItem(_ => service.SaveItem("a"));
        ThreadPool.QueueUserWorkItem(_ => service.SaveItem("b"));
        ThreadPool.QueueUserWorkItem(_ => service.SaveItem("c"));

        Thread.Sleep(500);

        ThreadPool.QueueUserWorkItem(_ => service.SaveItem("a"));
        ThreadPool.QueueUserWorkItem(_ => service.SaveItem("b"));
        ThreadPool.QueueUserWorkItem(_ => service.SaveItem("c"));

        Thread.Sleep(5000);

        Console.WriteLine("Everything recorded:");

        service.GetAllItems().ForEach(Console.WriteLine);

        Console.WriteLine("------------------------------------------------");

        var distributedService = new DistributedSystemIntegrationService();
        Console.WriteLine("DistributedSystemIntegrationService:");

        ThreadPool.QueueUserWorkItem(_ => distributedService.SaveItem("a"));
        ThreadPool.QueueUserWorkItem(_ => distributedService.SaveItem("b"));
        ThreadPool.QueueUserWorkItem(_ => distributedService.SaveItem("c"));

        Thread.Sleep(500);

        ThreadPool.QueueUserWorkItem(_ => distributedService.SaveItem("a"));
        ThreadPool.QueueUserWorkItem(_ => distributedService.SaveItem("b"));
        ThreadPool.QueueUserWorkItem(_ => distributedService.SaveItem("c"));

        Thread.Sleep(5000);

        Console.WriteLine("Everything recorded:");

        distributedService.GetAllItems().ForEach(Console.WriteLine);


        Console.ReadLine();
    }
}