using net_conn;

Console.WriteLine($"作者:林宏权 博客: https://blog.csdn.net/fittec?type=blog  QQ:296863766");
Console.WriteLine($"C# WebSocket服务器示例");
var server = new WebSocketServer();
server.StartListening();
Console.WriteLine("Press any key to exit...");
Console.ReadKey();

//nodejs websocket client 
//node ws-client.js
//save below to ws-client.js
// const ws = new WebSocket('ws://192.168.119.130:5000');
// ws.onopen = () => {
//     console.log('Connected to the server');
//     ws.send('Hello Server!');
// };
// ws.onmessage = (event) => {
//     console.log(`Received message from server: ${event.data}`);
// };
// ws.onclose = () => {
//     console.log('Disconnected from the server');
// };