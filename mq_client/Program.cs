using net_conn;

new Thread(() =>
{
    Task.Run(async () =>
    {
        Console.WriteLine("===>[{0}]Starting Task Publish===",DateTime.Now);
        await Mqtt.Publish();
    }).Wait();
}).Start();
        
new Thread(() =>
{
    Task.Run(async () =>
    {
        Console.WriteLine("===>[{0}]Starting Task Subscribe===",DateTime.Now);
        await Mqtt.Subscribe();
    }).Wait();
}).Start();
