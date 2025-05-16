using System.Text;
using MQTTnet;
using MQTTnet.Protocol;
using MQTTnet.Server;

public class mqtt_server
{
    String _ip = "127.0.0.1";
    int _port = 1883;

    public string Ip
    {
        get => _ip;
        set => _ip = value ?? throw new ArgumentNullException(nameof(value));
    }

    public int Port
    {
        get => _port;
        set => _port = value;
    }

    private MqttServer? _server;
    public event EventHandler<InterceptingPublishEventArgs>? OnMessageReceived;
    public event EventHandler<bool>? ServerStauts;
    public event EventHandler<ClientConnectedEventArgs>? ClientConnected;
    public event EventHandler<ClientDisconnectedEventArgs>? ClientDisconnected;
    public event EventHandler<ClientSubscribedTopicEventArgs>? ClientSubscribedTopic;
    public event EventHandler<ClientUnsubscribedTopicEventArgs>? ClientUnsubscribedTopic;
    
    public Task StartMqtServer(string ip, int port) 
    { 
        Ip = ip;
        Port = port;
        MqttServerOptions mqtServerOptions = new MqttServerOptionsBuilder()                     
               .WithDefaultEndpoint()     
               .WithDefaultEndpointBoundIPAddress(System.Net.IPAddress.Parse(ip))
               .WithDefaultEndpointPort(port) 
               .WithDefaultCommunicationTimeout(TimeSpan.FromMilliseconds(500)).Build();
              
        _server = new MqttFactory().CreateMqttServer(mqtServerOptions);
        _server.ValidatingConnectionAsync += Server_ValidatingConnectionAsync; 
        _server.ClientConnectedAsync += Server_ClientConnectedAsync;
        _server.ClientDisconnectedAsync += Server_ClientDisconnectedAsync;
        _server.ClientSubscribedTopicAsync += Server_ClientSubscribedTopicAsync;
        _server.ClientUnsubscribedTopicAsync += Server_ClientUnsubscribedTopicAsync;
        _server.InterceptingPublishAsync += Server_InterceptingPublishAsync;
        _server.ClientAcknowledgedPublishPacketAsync += Server_ClientAcknowledgedPublishPacketAsync;        
        _server.InterceptingClientEnqueueAsync += Server_InterceptingClientEnqueueAsync;
        _server.ApplicationMessageNotConsumedAsync += Server_ApplicationMessageNotConsumedAsync;
        _server.StartedAsync += Server_StartedAsync;
        _server.StoppedAsync += Server_StoppedAsync;
        return _server.StartAsync();
    }
    
    private Task Server_ApplicationMessageNotConsumedAsync(ApplicationMessageNotConsumedEventArgs e)
    { 
        try 
        {
            Console.WriteLine($"【MesageNotConsumed】-SenderId:{e.SenderId}-Mesage:{e.ApplicationMessage.ConvertPayloadToString()}");
        }
        catch (Exception ex) 
        { 
            Console.WriteLine($"Server_AplicationMesageNotConsumedAsync出现异常：{ex.Message}"); 
        }
        return Task.CompletedTask;
    }
    
    private Task Server_InterceptingClientEnqueueAsync(InterceptingClientApplicationMessageEnqueueEventArgs e)  
    { 
        try 
        {
            Console.WriteLine($"【InterceptingClientEnqueue】-SenderId:{e.SenderClientId}-Mesage:{e.ApplicationMessage.ConvertPayloadToString()}"); 
        } 
        catch (Exception ex) 
        {
            Console.WriteLine($"Server_InterceptingClientEnqueueAsync出现异常：{ex.Message}");
        }
        return Task.CompletedTask;
    }
    
    private Task Server_ClientAcknowledgedPublishPacketAsync(ClientAcknowledgedPublishPacketEventArgs e)         
    { 
        try 
        { 
            Console.WriteLine($"【ClientAcknowledgedPublishPacket】-SenderId:" +
                              $"{e.ClientId}-Mesage:{
                                  Encoding.UTF8.GetString(e.PublishPacket.PayloadSegment.ToArray())}");         
        } 
        catch (Exception ex) 
        { 
            Console.WriteLine($"Server_ClientAcknowledgedPublishPacketAsync出现异常：{ex.Message}"); 
        } 
        return Task.CompletedTask; 
    }
    
    private Task Server_InterceptingPublishAsync(InterceptingPublishEventArgs e) 
    { 
        try 
        { 
            string client = e.ClientId; string topic = e.ApplicationMessage.Topic; 
            string contents = e.ApplicationMessage.ConvertPayloadToString();
            OnMessageReceived?.Invoke(this, e); 
            Console.WriteLine($"接收到消息：Client：【{client}】 Topic：【{topic}】 Mesage：【{contents}】"); 
        } 
        catch (Exception ex) 
        { 
            Console.WriteLine($"Server_InterceptingPublishAsync出现异常：{ex.Message}");
        } 
        return Task.CompletedTask;
    }
 

    Task Server_StoppedAsync(EventArgs arg) 
    { 
        return Task.Run(() => 
        { 
            ServerStauts?.Invoke(this, false); 
            Console.WriteLine($"服务端【IP:Port】已停止MQT"); 
        }); 
    }
    
    public Task Server_StartedAsync(EventArgs e) 
    {
        return Task.Run(() => 
        {
            ServerStauts?.Invoke(this, true); 
            Console.WriteLine("服务端【{0}:{1}】已启用MQTT",Ip,Port); 
        });
    }
    
    private Task Server_ClientUnsubscribedTopicAsync(ClientUnsubscribedTopicEventArgs e)         
    { 
        return Task.Run(() => 
        { 
            ClientUnsubscribedTopic?.Invoke(this, e); 
            Console.WriteLine($"客户端【{e.ClientId}】退订主题【{e.TopicFilter}】"); 
        });
    }
    
    private Task Server_ClientSubscribedTopicAsync(ClientSubscribedTopicEventArgs e) 
    { 
        return Task.Run(() =>  
        { 
            ClientSubscribedTopic?.Invoke(this, e); 
            Console.WriteLine($"客户端【{e.ClientId}】订阅主题【{e.TopicFilter.Topic}】");         
        }); 
    }
    
    private Task Server_ClientDisconnectedAsync(ClientDisconnectedEventArgs e) 
    { 
        return Task.Run(()=> 
        { 
            ClientDisconnected?.Invoke(this, e); 
            Console.WriteLine($"客户端已断开.ClientId:【{e.ClientId}】,Endpoint:【{e.Endpoint}】.ReasonCode:【{e.ReasonCode}】,DisconnectType:【{e.DisconnectType}】"); 
        });
    }
    
    private Task Server_ClientConnectedAsync(ClientConnectedEventArgs e) 
    { 
        return Task.Run(() => 
        { 
            ClientConnected?.Invoke(this, e); 
            Console.WriteLine($"客户端已连接.ClientId:【{e.ClientId}】,Endpoint:【{e.Endpoint}】"); 
        }); 
    }

    private Task Server_ValidatingConnectionAsync(ValidatingConnectionEventArgs e) 
    { 
        return Task.Run(() => 
        { 
            if (String.IsNullOrEmpty(e.Password))
            {
                e.ReasonCode = MqttConnectReasonCode.Success; 
                Console.WriteLine($"客户端已验证成功.ClientId:【{e.ClientId}】,Endpoint:【{e.Endpoint}】"); 
            } 
            else
            {
                e.ReasonCode = MqttConnectReasonCode.BadUserNameOrPassword;        
                Console.WriteLine($"客户端验证失败.ClientId:【{e.ClientId}】,Endpoint:【{e.Endpoint}】");         
            } 
        });
    }
    
    static async Task Main(string[] args)
    {
        mqtt_server s = new mqtt_server();
        await s.StartMqtServer("0.0.0.0", 18883);
        while (true)
        {
            Console.WriteLine("[{0}] mqtt_server running ...", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            Thread.Sleep(1000);
        }
    }
}
 