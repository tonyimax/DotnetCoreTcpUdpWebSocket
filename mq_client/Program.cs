using net_conn;

Console.WriteLine($"作者:林宏权 博客: https://blog.csdn.net/fittec?type=blog  QQ:296863766");
Console.WriteLine($"C# Mqtt客户端示例");
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
