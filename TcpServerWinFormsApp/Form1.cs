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
            tcp.NotifyUpdateRow((_ip, _port, _iparea) => {
                FillGridRow(_ip,_port,_iparea);
            });
            tcp.Listen(ip, port);

        }
        public void UpdateWindowText(String text) 
        {
            this.BeginInvoke(new Action(() => {
                this.Text = text;
            }));
        }

        public void FillGridRow(string ip,string port ,string iparea) 
        {
            string[] data = {ip,port,iparea };
            for (int i = 0; i < 3; i++) 
            {
                lock (this) 
                {
                    var c = new DataGridViewTextBoxCell();
                    c.Value = data[i];
                    dataGridView1.Rows[0].Cells[i + 1] = c;
                }
            }
            /*var c1 = new DataGridViewTextBoxCell();
            c1.Value = ip;
            var c2 = new DataGridViewTextBoxCell();
            c1.Value = ip;
            var c3 = new DataGridViewTextBoxCell();
            c1.Value = ip;
            c2.Value = port;
            c3.Value = iparea;
            dataGridView1.Rows[0].Cells[1] = c1;
            dataGridView1.Rows[0].Cells[2] = c2;
            dataGridView1.Rows[0].Cells[3] = c3;*/
        }
    }
}
