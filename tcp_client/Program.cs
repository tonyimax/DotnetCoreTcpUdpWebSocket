using net_conn;

var ip = "127.0.0.1";
var port = 8000;
var data = "Hello, World!";
bool isQuit = false;

Console.WriteLine($"作者:林宏权 博客: https://blog.csdn.net/fittec?type=blog  QQ:296863766");
Console.WriteLine("默认IP地址:127.0.0.1 默认端口:8000 如果使用其它端口，请以参数形式使用，如: tcp_client 服务器IP地址 服务器端口号");
var p= Environment.GetCommandLineArgs();
if (p.Length >= 3)
{
    ip = p[1];
    port = int.Parse(p[2]);
}

Tcp tcp = new Tcp();
tcp.Connect(ip, port);
new Thread(() =>
{
    while (!isQuit)
    {
        Console.ForegroundColor= ConsoleColor.Yellow;
        Console.WriteLine("===>[{0}]Send:{1}",DateTime.Now,data);
        tcp.Send(data);
        Thread.Sleep(500);
    }
    Console.WriteLine("===>[{0}]Tcp Write Thread Quit ===",DateTime.Now);
}).Start();

Thread.Sleep(1000);

new Thread(() =>
{
    while (!isQuit)
    {
        Console.ForegroundColor= ConsoleColor.Green;
        Console.WriteLine("<===[{0}]Recv:{1}",DateTime.Now,tcp.Receive());
        Thread.Sleep(500);
    }
    Console.WriteLine("===>[{0}]Tcp Read Thread Quit ===",DateTime.Now);
}).Start();

if (Console.ReadKey().Key == ConsoleKey.Q)
{
    isQuit = true;
    Console.WriteLine("\nQuit");
}

