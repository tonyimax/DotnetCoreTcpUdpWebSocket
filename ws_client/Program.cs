using System.Net.WebSockets;
using System.Text;

using (var client = new ClientWebSocket())
{
    Uri uri = new Uri("ws://127.0.0.1:5000");
    await client.ConnectAsync(uri, CancellationToken.None);
    Console.WriteLine("WebSocket connected.");
            
    string message = "Hello, WebSocket server!";
    ArraySegment<byte> buffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes(message));
    await client.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
    Console.WriteLine("Message sent.");

    byte[] receiveBuffer = new byte[1024];
    WebSocketReceiveResult result = await client.ReceiveAsync(new ArraySegment<byte>(receiveBuffer), CancellationToken.None);
    string receivedMessage = Encoding.UTF8.GetString(receiveBuffer, 0, result.Count);
    Console.WriteLine("Received: " + receivedMessage);

    await client.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
    Console.WriteLine("WebSocket closed.");
}