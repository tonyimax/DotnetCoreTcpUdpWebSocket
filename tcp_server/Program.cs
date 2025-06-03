using net_conn;
Console.WriteLine($"作者:林宏权 博客: https://blog.csdn.net/fittec?type=blog  QQ:296863766");
Console.WriteLine("C# Tcp 服务器示例,默认端口:8000 如果使用其它端口请以参数的形式使用，如: tcp_server 端口号");
var p = Environment.GetCommandLineArgs();
var port = 8000;
if (p.Length >= 2)
{
    port = int.Parse(p[1]);
}
var tcp = new Tcp();
tcp.Listen("0.0.0.0",port);


