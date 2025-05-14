using net_conn;

Console.WriteLine("C# Tcp Server Demo");
var tcp = new Tcp();
tcp.Listen("127.0.0.1",8000);

