using net_conn;
Console.WriteLine($"作者:林宏权 博客: https://blog.csdn.net/fittec?type=blog  QQ:296863766");
Console.WriteLine("C# Udp客户端示例");
Udp udp = new Udp("106.52.84.138");
udp.Client();