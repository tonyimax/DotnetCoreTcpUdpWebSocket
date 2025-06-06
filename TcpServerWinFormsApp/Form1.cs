using net_conn;

namespace TcpServerWinFormsApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            var t = new Thread(new ParameterizedThreadStart(runServer));
            t.Start(this);
           
        }


        public void runServer(object? f) 
        {
            var p = Environment.GetCommandLineArgs();
            var ip = "0.0.0.0";
            var port = 8000;
            if (p.Length >= 2)
            {
                port = int.Parse(p[1]);
            }
            var tcp = new Tcp();
            tcp.NotifyListenSuccess(() =>
            {
                if (null != f)
                {
                    ((Form1)f).UpdateWindowText($"[{DateTime.Now}] 服务成功监听成功:[{ip}:{port}]");
                }
            });
            tcp.Listen(ip, port);

        }
        public void UpdateWindowText(String text) 
        {
            this.BeginInvoke(new Action(() => {
                this.Text = text;
            }));
        }
    }
}
