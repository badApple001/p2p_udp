using P2PClient.Data;
using P2PServer.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace P2PClient
{
    public partial class SettingForm : Form
    {
        public SettingForm( )
        {
            InitializeComponent( );
        }



        private void textBox2_TextChanged( object sender, EventArgs e )
        {

        }

        private void input_ip_TextChanged( object sender, EventArgs e )
        {

        }

        private void button1_Click( object sender, EventArgs e )
        {

            string content = input_port.Text;
            if ( !ushort.TryParse( content, out ushort v ) )
            {
                Logger.Toast( $"{v} 不是一个正确的端口" );
                return;
            }
            ClientData.Ins.serverPort = v;

            content = input_ip.Text;
            content = content.TrimStart( ' ' ).TrimEnd( ' ' );
            ClientData.Ins.serverIp = content;


            P2pMgr.Ins.ConnectServer( );
            P2pMgr.Ins.Send2Server( new MsgPingReq( ) );
            Logger.Debug( $"修改服务器地址 {ClientData.Ins.serverIp}:{ClientData.Ins.serverPort}" );
            ClientData.Ins.ping_server_reqTimeStamps = DateTimeUtils.GetCurrentTimestamps_MillSeconds( );
        }

        private void timer1_Tick( object sender, EventArgs e )
        {
            label_ping.Text = ClientData.Ins.ping_server_timestamps.ToString( ) + "ms";
            label_ping.ForeColor = ClientData.Ins.ping_server_timestamps < 200 ? Color.Green : Color.Red;
        }

        private void timer2_Tick( object sender, EventArgs e )
        {
            P2pMgr.Ins.ConnectServer( );
            P2pMgr.Ins.Send2Server( new MsgPingReq( ) );
            ClientData.Ins.ping_server_reqTimeStamps = DateTimeUtils.GetCurrentTimestamps_MillSeconds( );
        }
    }
}
