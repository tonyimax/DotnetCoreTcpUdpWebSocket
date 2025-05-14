using net_conn;

var ip = "127.0.0.1";
var port = 8000;
var data = "Hello, World!";
bool isQuit = false;


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

