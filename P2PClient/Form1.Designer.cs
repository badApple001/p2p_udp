namespace P2PClient
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent( )
        {
            components = new System.ComponentModel.Container( );
            input_chat = new TextBox( );
            btn_send = new Button( );
            input_send = new TextBox( );
            btn_setting = new Button( );
            timer_frame = new System.Windows.Forms.Timer( components );
            list_friends = new ListBox( );
            SuspendLayout( );
            // 
            // input_chat
            // 
            input_chat.Location = new Point( 12, 12 );
            input_chat.Multiline = true;
            input_chat.Name = "input_chat";
            input_chat.ReadOnly = true;
            input_chat.ScrollBars = ScrollBars.Vertical;
            input_chat.Size = new Size( 338, 383 );
            input_chat.TabIndex = 1;
            // 
            // btn_send
            // 
            btn_send.Location = new Point( 233, 404 );
            btn_send.Name = "btn_send";
            btn_send.Size = new Size( 80, 31 );
            btn_send.TabIndex = 2;
            btn_send.Text = "发送";
            btn_send.UseVisualStyleBackColor = true;
            btn_send.Click += btn_send_Click;
            // 
            // input_send
            // 
            input_send.Font = new Font( "Microsoft YaHei UI", 14F, FontStyle.Regular, GraphicsUnit.Point );
            input_send.Location = new Point( 12, 404 );
            input_send.Name = "input_send";
            input_send.Size = new Size( 215, 31 );
            input_send.TabIndex = 3;
            // 
            // btn_setting
            // 
            btn_setting.BackColor = Color.Transparent;
            btn_setting.BackgroundImage = Properties.Resources.setting;
            btn_setting.BackgroundImageLayout = ImageLayout.Stretch;
            btn_setting.Cursor = Cursors.Hand;
            btn_setting.FlatAppearance.BorderSize = 0;
            btn_setting.FlatAppearance.MouseDownBackColor = Color.Transparent;
            btn_setting.FlatAppearance.MouseOverBackColor = Color.Transparent;
            btn_setting.FlatStyle = FlatStyle.Flat;
            btn_setting.Location = new Point( 319, 403 );
            btn_setting.Name = "btn_setting";
            btn_setting.Size = new Size( 31, 31 );
            btn_setting.TabIndex = 4;
            btn_setting.UseVisualStyleBackColor = false;
            btn_setting.Click += btn_setting_Click;
            // 
            // timer_frame
            // 
            timer_frame.Enabled = true;
            timer_frame.Interval = 16;
            timer_frame.Tick += timer_frame_Tick;
            // 
            // list_friends
            // 
            list_friends.Font = new Font( "Microsoft YaHei UI", 10F, FontStyle.Regular, GraphicsUnit.Point );
            list_friends.FormattingEnabled = true;
            list_friends.ItemHeight = 19;
            list_friends.Location = new Point( 356, 12 );
            list_friends.Name = "list_friends";
            list_friends.ScrollAlwaysVisible = true;
            list_friends.Size = new Size( 291, 422 );
            list_friends.TabIndex = 5;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF( 7F, 17F );
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size( 659, 442 );
            Controls.Add( list_friends );
            Controls.Add( btn_setting );
            Controls.Add( input_send );
            Controls.Add( btn_send );
            Controls.Add( input_chat );
            Name = "Form1";
            Text = "客户端";
            ResumeLayout( false );
            PerformLayout( );
        }

        #endregion
        private TextBox input_chat;
        private Button btn_send;
        private TextBox input_send;
        private Button btn_setting;
        private System.Windows.Forms.Timer timer_frame;
        private ListBox list_friends;
    }
}