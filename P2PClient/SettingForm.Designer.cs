namespace P2PClient
{
    partial class SettingForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose( bool disposing )
        {
            if ( disposing && ( components != null ) )
            {
                components.Dispose( );
            }
            base.Dispose( disposing );
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent( )
        {
            components = new System.ComponentModel.Container( );
            label1 = new Label( );
            input_ip = new TextBox( );
            input_port = new TextBox( );
            label2 = new Label( );
            label3 = new Label( );
            label_ping = new Label( );
            button1 = new Button( );
            timer1 = new System.Windows.Forms.Timer( components );
            timer2 = new System.Windows.Forms.Timer( components );
            SuspendLayout( );
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font( "Microsoft YaHei UI", 12F, FontStyle.Bold, GraphicsUnit.Point );
            label1.ForeColor = Color.DarkSlateGray;
            label1.ImageAlign = ContentAlignment.TopLeft;
            label1.Location = new Point( 22, 9 );
            label1.Name = "label1";
            label1.Size = new Size( 90, 22 );
            label1.TabIndex = 0;
            label1.Text = "中继服务器";
            // 
            // input_ip
            // 
            input_ip.Location = new Point( 22, 34 );
            input_ip.Name = "input_ip";
            input_ip.Size = new Size( 90, 23 );
            input_ip.TabIndex = 1;
            input_ip.Text = "43.136.64.180";
            input_ip.TextChanged += input_ip_TextChanged;
            // 
            // input_port
            // 
            input_port.Location = new Point( 118, 34 );
            input_port.Name = "input_port";
            input_port.Size = new Size( 46, 23 );
            input_port.TabIndex = 4;
            input_port.Text = "11000";
            input_port.TextChanged += textBox2_TextChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font( "Microsoft YaHei UI", 12F, FontStyle.Bold, GraphicsUnit.Point );
            label2.ForeColor = Color.DarkSlateGray;
            label2.ImageAlign = ContentAlignment.TopLeft;
            label2.Location = new Point( 118, 9 );
            label2.Name = "label2";
            label2.Size = new Size( 42, 22 );
            label2.TabIndex = 6;
            label2.Text = "端口";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font( "Microsoft YaHei UI", 12F, FontStyle.Bold, GraphicsUnit.Point );
            label3.ForeColor = Color.DarkSlateGray;
            label3.ImageAlign = ContentAlignment.TopLeft;
            label3.Location = new Point( 184, 9 );
            label3.Name = "label3";
            label3.Size = new Size( 42, 22 );
            label3.TabIndex = 7;
            label3.Text = "延迟";
            // 
            // label_ping
            // 
            label_ping.AutoSize = true;
            label_ping.Font = new Font( "Microsoft YaHei UI", 10F, FontStyle.Regular, GraphicsUnit.Point );
            label_ping.ForeColor = Color.Red;
            label_ping.Location = new Point( 184, 37 );
            label_ping.Name = "label_ping";
            label_ping.Size = new Size( 60, 20 );
            label_ping.TabIndex = 8;
            label_ping.Text = "1000ms";
            // 
            // button1
            // 
            button1.Font = new Font( "Microsoft YaHei UI", 12F, FontStyle.Bold, GraphicsUnit.Point );
            button1.ForeColor = Color.DimGray;
            button1.Location = new Point( 263, 9 );
            button1.Name = "button1";
            button1.Size = new Size( 111, 48 );
            button1.TabIndex = 9;
            button1.Text = "确认修改";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // timer1
            // 
            timer1.Enabled = true;
            timer1.Tick += timer1_Tick;
            // 
            // timer2
            // 
            timer2.Enabled = true;
            timer2.Interval = 1000;
            timer2.Tick += timer2_Tick;
            // 
            // SettingForm
            // 
            AutoScaleDimensions = new SizeF( 7F, 17F );
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size( 398, 69 );
            Controls.Add( button1 );
            Controls.Add( label_ping );
            Controls.Add( label3 );
            Controls.Add( label2 );
            Controls.Add( input_port );
            Controls.Add( input_ip );
            Controls.Add( label1 );
            Name = "SettingForm";
            Text = "设置";
            ResumeLayout( false );
            PerformLayout( );
        }

        #endregion

        private Label label1;
        private TextBox input_ip;
        private TextBox input_port;
        private Label label2;
        private Label label3;
        private Label label_ping;
        private Button button1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Timer timer2;
    }
}