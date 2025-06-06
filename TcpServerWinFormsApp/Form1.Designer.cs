namespace TcpServerWinFormsApp
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            dataGridView1 = new DataGridView();
            序号 = new DataGridViewTextBoxColumn();
            远程主机 = new DataGridViewTextBoxColumn();
            远程端口 = new DataGridViewTextBoxColumn();
            IP归属地 = new DataGridViewTextBoxColumn();
            连接时间 = new DataGridViewTextBoxColumn();
            连接状态 = new DataGridViewTextBoxColumn();
            连接时长 = new DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { 序号, 远程主机, 远程端口, IP归属地, 连接时间, 连接状态, 连接时长 });
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.Location = new Point(0, 0);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 82;
            dataGridView1.Size = new Size(1992, 931);
            dataGridView1.TabIndex = 0;
            // 
            // 序号
            // 
            序号.HeaderText = "序号";
            序号.MinimumWidth = 10;
            序号.Name = "序号";
            // 
            // 远程主机
            // 
            远程主机.HeaderText = "远程主机";
            远程主机.MinimumWidth = 10;
            远程主机.Name = "远程主机";
            // 
            // 远程端口
            // 
            远程端口.HeaderText = "远程端口";
            远程端口.MinimumWidth = 10;
            远程端口.Name = "远程端口";
            // 
            // IP归属地
            // 
            IP归属地.HeaderText = "IP归属地";
            IP归属地.MinimumWidth = 10;
            IP归属地.Name = "IP归属地";
            // 
            // 连接时间
            // 
            连接时间.HeaderText = "连接时间";
            连接时间.MinimumWidth = 10;
            连接时间.Name = "连接时间";
            // 
            // 连接状态
            // 
            连接状态.HeaderText = "连接状态";
            连接状态.MinimumWidth = 10;
            连接状态.Name = "连接状态";
            // 
            // 连接时长
            // 
            连接时长.HeaderText = "连接时长";
            连接时长.MinimumWidth = 10;
            连接时长.Name = "连接时长";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(14F, 31F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1992, 931);
            Controls.Add(dataGridView1);
            Name = "Form1";
            Text = "C#实现的网关服务监控程序";
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView dataGridView1;
        private DataGridViewTextBoxColumn 序号;
        private DataGridViewTextBoxColumn 远程主机;
        private DataGridViewTextBoxColumn 远程端口;
        private DataGridViewTextBoxColumn IP归属地;
        private DataGridViewTextBoxColumn 连接时间;
        private DataGridViewTextBoxColumn 连接状态;
        private DataGridViewTextBoxColumn 连接时长;
    }
}
