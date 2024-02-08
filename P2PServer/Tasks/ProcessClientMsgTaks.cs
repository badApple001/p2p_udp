using Newtonsoft.Json;
using P2PClient.Data;
using P2PServer.Common;
using System.Net;

namespace P2PServer.Tasks
{
    internal class ProcessClientMsgTaks : Task
    {
        public ProcessClientMsgTaks( )
        {

            P2pMgr.Ins.reciveCallback += OnClientMessage;
        }

        private void OnClientMessage( string? plaintext, IPEndPoint from )
        {
            if ( string.IsNullOrEmpty( plaintext ) ) { return; }

            try
            {
                var msg = JsonConvert.DeserializeObject<MsgBasic>( plaintext );

                if ( null != msg )
                {

#if DEBUG
                    Console.WriteLine( $"[recv]: {msg.type}|{plaintext}" );
#endif         

                    string ipv4_address = $"{from.Address.ToString( )}:{from.Port}";
                    UserData.Ins.AddOrRefresh( ipv4_address, from );

                    switch ( msg.type )
                    {
                        case MsgType.MsgLoginReq:
                            {

                                List<string> users = UserData.Ins.users.Keys.ToList( );

                                P2pMgr.Ins.Send( new MsgLoginRes( )
                                {
                                    ipv4_address = ipv4_address,
                                    friends = users.Where( s => s != ipv4_address ).ToList( )
                                }, from );

                                System.Threading.Tasks.Task.Run( async ( ) =>
                                {
                                    await System.Threading.Tasks.Task.Delay( 200 );

                                    foreach ( var user in UserData.Ins.users )
                                    {
                                        var friends = users.Where( t => t != user.Key ).ToList( );
                                        P2pMgr.Ins.Send( new MsgFriendsBroadcast( ) { friends = friends }, user.Value.ep );
                                        await System.Threading.Tasks.Task.Delay( 10 );
                                    }
                                } );
                            }
                            break;
                        case MsgType.MsgPingReq:
                            {
                                P2pMgr.Ins.Send( new MsgPingRes( ) { }, from ); ;
                            }
                            break;
                        case MsgType.MsgTickRes:
                            {
                                UserData.Ins.GetUser( ipv4_address ).tick_res_ts = DateTimeUtils.GetCurrentTimestamps_MillSeconds( );
                            }
                            break;

                        //转发A的打洞请求给B
                        case MsgType.MsgPunchReq:
                            {
                                var req = JsonConvert.DeserializeObject<MsgPunchReq>( plaintext );
                                P2pMgr.Ins.Send( req, UserData.Ins.GetUser( req.target_ip ).ep );
                            }
                            break;

                        //转发B的打洞返回给A
                        case MsgType.MsgPunchRes:
                            {
                                var req = JsonConvert.DeserializeObject<MsgPunchRes>( plaintext );
                                req.A2B = false;
                                P2pMgr.Ins.Send( req, UserData.Ins.GetUser( req.target_ip ).ep );
                            }
                            break;
                    }

                }
                else
                {
                    Logger.Error( $"msg is null" );
                }

            }
            catch ( Exception ex )
            {
                Logger.Error( $"{ex.Message}\n{ex.StackTrace}" );
            }
        }


        Queue<AsyncSendMsgInfo> queue = new Queue<AsyncSendMsgInfo>( );
        class AsyncSendMsgInfo
        {
            public Stack<IPEndPoint> epStack = new Stack<IPEndPoint>( );
            public byte[] data;
        }

        public void Send( MsgPunchReq req, IPEndPoint target ) => Send( req, new List<IPEndPoint>( ) { target } );
        public void Send( MsgBasic msg, IEnumerable<IPEndPoint> targets )
        {
            queue.Enqueue( new AsyncSendMsgInfo( )
            {
                epStack = new Stack<IPEndPoint>(targets),
                data = msg.GetBytes()
            } );
        }
      
        protected override void Process( )
        {
            if ( queue.Count > 0 )
            {
                if ( queue.Peek( ).epStack.Count == 0 )
                {
                    queue.Dequeue( );

                    if ( queue.Count == 0 )
                    {
                        return;
                    }
                }

                var ep = queue.Peek( ).epStack.Pop( );
                P2pMgr.Ins.Send( queue.Peek( ).data, ep );
            }
        }
    }

}