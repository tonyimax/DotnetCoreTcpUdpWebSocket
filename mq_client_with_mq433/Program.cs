using System.Text;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Protocol;

public class mqtt_client 
{ 
    private IMqttClient? _client;
    public bool IsConnected { get; set; } = false; 
    public bool IsDisConnected { get; set; } = true;
    private string? _serverIp="127.0.0.1";
    private int _serverPort=1883;
    private Dictionary<string, bool>? _subscribeTopicList; 
    
    public void Start(string serverIp, int serverPort)
    {
        Console.WriteLine($"作者:林宏权 博客: https://blog.csdn.net/fittec?type=blog  QQ:296863766");
        Console.WriteLine($"C# Mqtt客户端示例（mqtt4.3.3）");
        _serverIp = serverIp; 
        _serverPort = serverPort;
        if (!string.IsNullOrEmpty(serverIp) & !string.IsNullOrWhiteSpace(serverIp) & serverPort > 0) 
        { 
            try 
            { 
                var options = new MqttClientOptions() 
                { 
                    ClientId = "CLIENT_ID_20250516_B",
                    Credentials = new MqttClientCredentials(
                        "ubuntu_dev_192_168_119_130",
                        Encoding.Default.GetBytes("6e94a638d6b35f43de8a2f0cd1644089cb004e19e8667cfd0e427a7437032e75"))
                }; 
                options.ChannelOptions = new MqttClientTcpOptions() 
                { 
                    Server = serverIp, 
                    Port = serverPort,
                };
                options.CleanSession = true; 
                options.KeepAlivePeriod = TimeSpan.FromSeconds(10);
                if (_client != null) 
                { 
                    _client.DisconnectAsync(); 
                    _client = null; 
                } 
                _client = new MqttFactory().CreateMqttClient(); 
                _client.ConnectedAsync += Client_ConnectedAsync;      
                _client.DisconnectedAsync += Client_DisconnectedAsync; 
                _client.ApplicationMessageReceivedAsync += Client_ApplicationMessageReceivedAsync; 
                var result = _client.ConnectAsync(options);
                result.Wait();
                if (result.Result.ResultCode != MqttClientConnectResultCode.Success)
                {
                    Console.WriteLine("MQTT客户端连接服务端FAIL===");
                }
                else
                {
                    Console.WriteLine("MQTT客户端连接服务端SUCCESS===");
                }
            } 
            catch (Exception ex) { 
                Console.WriteLine("MQT客户端连接服务端错误：{0}",ex.Message);
            }
        }
    } 

    public void Client_Disconnect()
    { 
        if (_client != null) 
        { 
            _client.DisconnectAsync(); 
            _client.Dispose(); 
            Console.WriteLine($"关闭MQT客户端成功！"); 
        }
    } 

    public void Client_ConnectAsync() 
    { 
        if (_client != null) 
        { 
            _client.ReconnectAsync();
            Console.WriteLine($"连接MQT服务端成功！"); 
        } 
    } 
    
    private Task Client_ConnectedAsync(MqttClientConnectedEventArgs arg)
    { 
        return Task.Run(() => 
        { 
            IsConnected = true; 
            IsDisConnected = false; 
            Console.WriteLine($"连接到MQT服务端成功.{arg.ConnectResult.AssignedClientIdentifier}");
            try 
            { 
                if (_subscribeTopicList != null && _subscribeTopicList.Count > 0) 
                { 
                    List<string> subscribeTopics = _subscribeTopicList.Keys.ToList();        
                    foreach (var topic in subscribeTopics) 
                        SubscribeAsync(topic); 
                } 
            } 
            catch (Exception ex) 
            { 
                Console.WriteLine("MQT客户端与服务端[{0}:{1}]建立连接订阅主题错误：{2}", _serverIp, _serverPort, ex.Message); 
            } 
        }); 
    } 
 

    private Task Client_DisconnectedAsync(MqttClientDisconnectedEventArgs arg) 
    { 
        return Task.Run(async () => 
        { 
            IsConnected = false; 
            IsDisConnected = true; 
            Console.WriteLine("已断开到MQT服务端的连接.尝试重新连接");
            try 
            { 
                await Task.Delay(30); 
                await _client.ReconnectAsync();
            } 
            catch (Exception ex) 
            { 
                Console.WriteLine("MQT客户端与服务端[{0}:{1}]断开连接退订主题错误：{2}", _serverIp, _serverPort, ex.Message);
            } 
        }); 
    } 

    public Task ReconnectedAsync() 
    { 
        try 
        { 
            if (_client != null) 
            { 
                _client.ReconnectAsync(); 
            } 
        } 
        catch (Exception ex) 
        { 
            Console.WriteLine("MQT客户端与服务端[{0}:{1}]重新连接退订主题错误：{2}", _serverIp, _serverPort, ex.Message); 
        } 
        return Task.CompletedTask; 
    } 

    private Task Client_ApplicationMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs? arg) 
    { 
        try 
        { 
            return Task.Run(() => 
            { 
                string msg = arg.ApplicationMessage.ConvertPayloadToString(); 
                Console.WriteLine("接收消息：{0} QoS= {1} 客户端= {2} 主题={3}",msg,
                    arg.ApplicationMessage.QualityOfServiceLevel,
                    arg.ClientId,arg.ApplicationMessage.Topic);
            });
        } 
        catch (Exception ex) 
        { 
            Console.WriteLine("MQT收到来自服务端[{0}]消息错误：{1}", (arg != null ? arg.ClientId:""), ex.Message);
        }
        return Task.CompletedTask; 
    } 
    

    public void SubscribeAsync(string topic) 
    { 
        try 
        { 
            /*if (_subscribeTopicList == null) 
                _subscribeTopicList = new Dictionary<string, bool>(); 
            if (_subscribeTopicList.ContainsKey(topic))
            { 
                Console.WriteLine("MQT客户端已经订阅主题[{0}]，不能重复订阅", topic); 
                return; 
            }*/ 
            _client?.SubscribeAsync(topic, MqttQualityOfServiceLevel.AtLeastOnce);
            /*bool isSubscribed = _client != null && _client.IsConnected;
            if (_subscribeTopicList.ContainsKey(topic) & _subscribeTopicList[topic])
            {
                _subscribeTopicList[topic] = isSubscribed;
            }
            else
            {
                _subscribeTopicList.Add(topic, isSubscribed);
            }*/
        } 
        catch (Exception ex) 
        { 
            Console.WriteLine("MQT客户端订阅主题[{0}]错误：{1}", topic, ex.Message);
        } 
    } 

    public void SubscribeAsync(List<string>? topicList) 
    { 
        try 
        { 
            if (topicList == null || topicList.Count == 0) 
                return; 
            foreach (var topic in topicList) 
                SubscribeAsync(topic); 
        } 
        catch (Exception ex) 
        { 
            Console.WriteLine("MQT客户端订阅主题集合错误：{0}", ex.Message); 
        } 
    } 

    public void UnsubscribeAsync(string topic, bool isRemove = true) 
    { 
        try 
        { 
            if (_subscribeTopicList == null || _subscribeTopicList.Count == 0 ||!_subscribeTopicList.ContainsKey(topic)) 
            { 
                Console.WriteLine("MQTT客户端退订主题[{0}]不存在", topic);
                return; 
            } 
            _client.UnsubscribeAsync(topic);
            if (isRemove) 
                _subscribeTopicList.Remove(topic); 
            else 
                _subscribeTopicList[topic] = false;
        } 
        catch (Exception ex) 
        { 
            Console.WriteLine("MQT客户端退订主题[{0}]错误：{1}", topic, ex.Message);
        } 
    } 

    
    public void UnsubscribeAsync(List<string>? topicList, bool isRemove = true) 
    { 
        try 
        { 
            if (topicList == null || topicList.Count == 0) 
                return; 
            foreach (var topic in topicList) 
                UnsubscribeAsync(topic, isRemove);
        } 
        catch (Exception ex) 
        { 
            Console.WriteLine("MQT客户端退订主题集合错误：{0}", ex.Message);
        } 
    } 

    public bool IsExistSubscribeAsync(string topic) 
    { 
        try 
        { 
            if (_subscribeTopicList == null || 
                _subscribeTopicList.Count == 0 || 
                !_subscribeTopicList.TryGetValue(topic, out var isExist))
                return false; 
            return _subscribeTopicList[topic];
        } 
        catch (Exception ex) 
        { 
            Console.WriteLine("MQT客户端订阅主题[{0}]是否存在错误：{1}", topic, ex.Message);
            return false;
        } 
    } 

    public async void PublishMessage(string topic, string message) 
    { 
        try 
        { 
            if (_client != null) 
            { 
                if (string.IsNullOrEmpty(message) | string.IsNullOrWhiteSpace(message))
                { 
                    Console.WriteLine("MQT客户端不能发布为空的消息！"); 
                    return;
                } 
                var result = await _client.PublishStringAsync(topic,message,MqttQualityOfServiceLevel.AtLeastOnce); 
                Console.WriteLine($"发布消息-主题：{topic}，消息：{message}，结果： {result.ReasonCode}");
            } 
            else 
            { 
                Console.WriteLine("MQT客户端未连接服务端，不能发布主题为[{0}]的消息：{1}", topic, message);
            } 
        } 
        catch (Exception ex) 
        { 
            Console.WriteLine("MQT客户端发布主题为[{0}]的消息：{1}，错误：{2}", topic, message, ex.Message); 
        } 
    }
    
    static async Task Main(string[] args)
    {
        var mq = new mqtt_client();
        mq.Start("43.136.40.107", 1883);
        Console.WriteLine("task run finished===");
        
        new Thread(() =>
        {
            Console.WriteLine("===publish message thread task running===");
            Task.Run(() =>
            {
                while (mq.IsConnected)
                {
                    Console.WriteLine("===>[{0}] isConnected: {1}",DateTime.Now,mq.IsConnected);
                    mq.PublishMessage("test/topic",string.Format("[{0}] Hello World",DateTime.Now));
                    Thread.Sleep(1000);
                }
            }).Wait();
        }).Start();
        
        Thread.Sleep(1000);
        
        new Thread(() =>
        {
            Console.WriteLine("===subscribe message thread task running===");
            Task.Run(() =>
            {
                while (mq.IsConnected)
                {
                    Console.WriteLine("===>[{0}] isConnected: {1}",DateTime.Now,mq.IsConnected);
                    mq.SubscribeAsync("test/topic");
                    Thread.Sleep(1000);
                }
            }).Wait();
        }).Start();
        
    }
}