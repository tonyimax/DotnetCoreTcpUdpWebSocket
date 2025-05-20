using net_conn;

Console.WriteLine("C# Udp Client Demo");
Udp udp = new Udp("192.168.169.14",8811);
udp.Client();