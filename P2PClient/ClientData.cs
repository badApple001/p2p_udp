using P2PClient.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace P2PClient
{
    public class ClientData
    {
        public static ClientData Ins { get; private set; } = new ClientData( );

        public string serverIp = "121.37.128.219";
        public int serverPort = 1_1000;

        public long ping_server_timestamps = 1000;
        public long ping_server_reqTimeStamps = 0;



        public List<string> friends = new List<string>( );
        /// <summary>
        /// 自己的ip
        /// </summary>
        public string ipv4_address = "127.0.0.1";


        public Dictionary<string, IPEndPoint> bindUsers = new Dictionary<string, IPEndPoint>( );
        public Dictionary<string, long> ping_player_timestamps = new Dictionary<string, long>( );
        public Dictionary<string, ushort> ping_player_duration = new Dictionary<string, ushort>( );


        public Dictionary<ulong, MsgTextReq> chatHistory = new Dictionary<ulong, MsgTextReq>( );
        public ulong chatID = 0;
    }
}
