using P2PServer.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace P2PServer
{
    //TODO: 追加数据库
    internal class UserData
    {
        public static UserData Ins { private set; get; } = new UserData( );

        public readonly string ip = "0.0.0.0";
        public readonly int port = 1_1000;

        public Dictionary<string, Client> users = new Dictionary<string, Client>( );

        public void AddOrRefresh( string key, IPEndPoint ep )
        {
            if ( !users.TryGetValue( key, out var client ) )
            {
                client = new Client( );
                users.Add( key, client );
            }

            client.ep = ep;
            client.activity_ts = DateTimeUtils.GetCurrentTimestamps_Seconds( );
        }

        public Client GetUser( string key )
        {
            if ( !users.TryGetValue( key, out var client ) )
            {
                var ip_port = key.Split( ':' );
                if ( ip_port.Length < 2 || !ushort.TryParse( ip_port[ 1 ], out var port ) )
                {
                    throw new ArgumentException( $"IP format fail: {key}" );
                }

                AddOrRefresh( key, new IPEndPoint( IPAddress.Parse( ip_port[ 0 ] ), port ) );

                return GetUser( key );
            }
            return client;
        }

    }


    public class Client
    {

        public IPEndPoint? ep;
        public long activity_ts = 0;
        public long tick_req_ts = 0;
        public long tick_res_ts = 0;
    }

}
