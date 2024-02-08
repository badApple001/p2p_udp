using Newtonsoft.Json;
using P2PClient.Data;
using P2PServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

public class P2pMgr
{
    public static P2pMgr Ins { get; private set; } = new P2pMgr( );


    private UdpClient? client;
    private IPEndPoint? serverEp;
    public Action<string?, IPEndPoint>? reciveCallback;

    public void ConnectServer( )
    {
        string ip = UserData.Ins.ip;
        int port = UserData.Ins.port;
        if ( IPAddress.TryParse( ip, out var address ) && port > 200 && port <= ushort.MaxValue )
        {
            serverEp = new IPEndPoint( address, port );
            client = new UdpClient( serverEp );
            Task.Factory.StartNew( Recive, TaskCreationOptions.LongRunning );
        }
        else
        {
            //Logger.Toast( $"错误的IP地址\nip: {ip}\nport: {port}" );
            Logger.Info( $"错误的IP地址\nip: {ip}\nport: {port}" );
        }
    }

    private void Recive( )
    {
        IPEndPoint from = new IPEndPoint( IPAddress.Any, 1_1000 );
        Encoding encoding = Encoding.UTF8;
        while ( null != client )
        {
            byte[] buffer = client.Receive( ref from );
            if ( null != buffer && buffer.Length > 0 )
            {
                string jsonstr = encoding.GetString( buffer, 0, buffer.Length );
                if ( !string.IsNullOrEmpty( jsonstr ) )
                {
                    reciveCallback?.Invoke( jsonstr, from );
                }
                else
                {
                    Logger.Error( $"jsonstr parse fail: {jsonstr}" );
                }
            }
        }
    }

    public void Send( byte[] buffer, IPEndPoint to )
    {
        client?.Send( buffer, to );
    }

    public void Send( MsgBasic msg, IPEndPoint to )
    {
        client?.Send( msg.GetBytes( ), to );
    }

    public void Send( byte[] buffer, string ipv4 )
    {
        Logger.Assert( ipv4.Split( '.' ).Length == 4 && ipv4.Split( ':' ).Length == 2, $"ip地址格式不正确 {ipv4}" );

        var ip_port = ipv4.Split( ':' );
        Send( buffer, new IPEndPoint( IPAddress.Parse( ip_port[ 0 ] ), ushort.Parse( ip_port[ 1 ] ) ) );
    }

    public void Send( MsgBasic msg, string ipv4 ) => Send( msg.GetBytes( ), ipv4 );
}
