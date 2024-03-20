using Newtonsoft.Json;
using P2PClient.Data;
using P2PServer.Common;
using System.Net;
using System.Timers;

namespace P2PClient
{
    public partial class Form1 : Form
    {

        public Form1( )
        {
            InitializeComponent( );
            InitTimer( );

            FriendListMgr.Ins.SetView( list_friends );
            P2pMgr.Ins.ConnectServer( );
            P2pMgr.Ins.Send2Server( new MsgLoginReq( ) );
            P2pMgr.Ins.reciveCallback += OnReciveMessage;
        }


        //�򶴳�ʱ��ʱ��
        System.Timers.Timer? punch_req_timeout_timer;
        public void InitTimer( )
        {
            punch_req_timeout_timer = new System.Timers.Timer( 1000 );
            punch_req_timeout_timer.Elapsed += new ElapsedEventHandler( OnTmrTrg );
        }


        //��������Ϣ ���������ͻ���ֱ�Ӵ�͸��������Ϣ
        private void OnReciveMessage( string? plaintext, IPEndPoint from )
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

                    string from_ipv4_address = $"{from.Address.ToString( )}:{from.Port}";

                    switch ( msg.type )
                    {
                        case MsgType.MsgLoginRes:
                            {
                                var req = JsonConvert.DeserializeObject<MsgLoginRes>( plaintext );
                                ClientData.Ins.ipv4_address = req.ipv4_address;
                                ClientData.Ins.friends = req.friends;

                                mainThreadMsgQueue.Enqueue( new MsgFriendsBroadcast( )
                                {
                                    friends = req.friends
                                } );
                            }
                            break;
                        case MsgType.MsgFriendsBroadcast:
                            {
                                var req = JsonConvert.DeserializeObject<MsgFriendsBroadcast>( plaintext );
                                ClientData.Ins.friends = req.friends;
                                mainThreadMsgQueue.Enqueue( req );
                            }
                            break;
                        case MsgType.MsgPingReq:
                            {
                                P2pMgr.Ins.Send( new MsgPingRes( ), from );
                            }
                            break;
                        case MsgType.MsgPingRes:
                            {
                                if ( ClientData.Ins.ping_player_timestamps.TryGetValue( from_ipv4_address, out long v ) )
                                {
                                    var currentTimeStamps = DateTimeUtils.GetCurrentTimestamps_MillSeconds( );
                                    var duration = ( ushort ) ( currentTimeStamps - v );
                                    ClientData.Ins.ping_player_duration[ from_ipv4_address ] = duration;
                                    Logger.Debug( $"PING����: {from_ipv4_address} {duration}ms" );

                                }
                                else
                                {
                                    ClientData.Ins.ping_server_timestamps = DateTimeUtils.GetCurrentTimestamps_MillSeconds( ) - ClientData.Ins.ping_server_reqTimeStamps;
                                }
                            }
                            break;
                        case MsgType.MsgTickReq:
                            {

                                P2pMgr.Ins.Send2Server( new MsgTickRes( ) );
                            }
                            break;
                        case MsgType.MsgTextReq:
                            {
                                var req = JsonConvert.DeserializeObject<MsgTextReq>( plaintext );
                                P2pMgr.Ins.Send( new MsgTextRes( ) { chatId = req.chatId }, from );
                                mainThreadMsgQueue.Enqueue( req );
                            }
                            break;
                        case MsgType.MsgTextRes:
                            {
                                var req = JsonConvert.DeserializeObject<MsgTextRes>( plaintext );
                                mainThreadMsgQueue.Enqueue( req );
                            }
                            break;



                        //���յ������� �ɷ�����ת�������� 
                        case MsgType.MsgPunchReq:
                            {

                                //���˿ͻ��˵� ��һ��NAT��������
                                if ( from_ipv4_address == $"{ClientData.Ins.serverIp}:{ClientData.Ins.serverPort}" )
                                {

                                    var req = JsonConvert.DeserializeObject<MsgPunchReq>( plaintext );

                                    var resmg = new MsgPunchRes( )
                                    {
                                        source_ip = ClientData.Ins.ipv4_address,
                                        target_ip = req.source_ip
                                    };

                                    //��P�˷��ͷ���
                                    P2pMgr.Ins.Send( resmg, req.source_ip );
                                    //����������� ����������ת��
                                    P2pMgr.Ins.Send2Server( resmg );

                                    Logger.Debug( $"������: {from_ipv4_address}" );
                                }
                            }
                            break;

                        //���յ��򶴷���
                        case MsgType.MsgPunchRes:
                            {
                                var req = JsonConvert.DeserializeObject<MsgPunchRes>( plaintext );

                                //����NAT �ɿͻ���ֱ�ӷ��͹�����
                                if ( req.A2B )
                                {
                                    if ( !ClientData.Ins.bindUsers.TryGetValue( from_ipv4_address, out var ep ) )
                                    {
                                        //���湫��ipv4
                                        ClientData.Ins.bindUsers[ from_ipv4_address ] = new IPEndPoint( IPAddress.Parse( from.Address.ToString( ) ), from.Port );

                                        //��P�˷��ͷ���
                                        P2pMgr.Ins.Send( new MsgPunchRes( )
                                        {
                                            source_ip = ClientData.Ins.ipv4_address,
                                            target_ip = from_ipv4_address
                                        }, from_ipv4_address );

                                        Logger.Debug( $"����NAT��: {from_ipv4_address}" );
                                    }

                                }
                                //�ɷ�����ת������
                                else
                                {
                                    punch_req_timeout_timer.Stop( );
                                    Logger.Debug( $"B->������->A ����: {from_ipv4_address}" );
                                }
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

        private void btn_send_Click( object sender, EventArgs e )
        {
            string content = input_send.Text;
            if ( string.IsNullOrEmpty( content ) || string.IsNullOrWhiteSpace( content ) )
            {
                return;
            }

            if ( list_friends.SelectedIndex == -1 )
            {
                Logger.Toast( "��ѡ��һ������" );
                return;
            }


            int index = list_friends.SelectedIndex;
            if ( index >= ClientData.Ins.friends.Count )
            {
                Logger.Toast( $"�±�Խ�� {index} �����˺����б�" );
                return;
            }

            string ipv4_address = ClientData.Ins.friends[ index ];
            Logger.Debug( $"text->{ipv4_address}: {input_send.Text}" );


            //������Ϣ����
            var chatMsg = new MsgTextReq( )
            {
                content = content,
                ipv4_address = ClientData.Ins.ipv4_address,
                chatId = ClientData.Ins.chatID
            };
            p2pPkgs.Enqueue( new P2pData( )
            {
                target_ip = ipv4_address,
                pkg = chatMsg
            } );
            ClientData.Ins.chatHistory[ ClientData.Ins.chatID++ ] = chatMsg;


            //��������
            input_send.Text = "";

            //��
            if ( !ClientData.Ins.bindUsers.ContainsKey( ipv4_address ) )
            {
                var msg = new MsgPunchReq( )
                {
                    target_ip = ipv4_address,
                    source_ip = ClientData.Ins.ipv4_address
                };
                P2pMgr.Ins.Send( msg, ipv4_address );
                P2pMgr.Ins.Send2Server( msg );

                //Logger.Debug( $"��->{ipv4_address}" );


                //�򶴳�ʱ
                punch_req_timeout_timer.Stop( );
                punch_req_timeout_timer.Interval = 2000;
                punch_req_timeout_timer.AutoReset = false;
                punch_req_timeout_timer.Start( );
            }

        }

        object @lock = new object( );
        private void OnTmrTrg( object sender, System.Timers.ElapsedEventArgs e )
        {
            lock ( @lock )
            {
                Logger.Toast( "�򶴳�ʱ... " );
            }
        }


        private void btn_setting_Click( object sender, EventArgs e )
        {
            SettingForm winform = new SettingForm( );
            winform.StartPosition = FormStartPosition.CenterScreen;
            winform.ShowDialog( );
        }


        /// <summary>
        /// ��p��ֱ�ӵ�p�˵���Ϣ
        /// </summary>
        public class P2pData
        {
            public string target_ip;
            public MsgBasic pkg;
        }
        //ÿ��1��ˢ��ping
        float tm_req_ping = 0f;
        //һЩ��ui��������Ϣ �ŵ���������� ���н��߳�������
        Queue<MsgBasic> mainThreadMsgQueue = new Queue<MsgBasic>( );
        //���ֱ�ӷ��͵���ҵ���Ϣ �ŵ����������
        Queue<P2pData> p2pPkgs = new Queue<P2pData>( );
        //���߳�ѭ�� ÿһ֡������
        private void timer_frame_Tick( object sender, EventArgs e )
        {
            if ( mainThreadMsgQueue.Count > 0 )
            {
                MsgBasic msgBasic = mainThreadMsgQueue.Dequeue( );
                switch ( msgBasic.type )
                {
                    case MsgType.MsgFriendsBroadcast:
                        {
                            if ( msgBasic is MsgFriendsBroadcast msg )
                            {
                                FriendListMgr.Ins.Update( msg.friends );
                            }
                        }
                        break;
                    case MsgType.MsgTextReq:
                        {
                            if ( msgBasic is MsgTextReq req )
                            {
                                input_chat.Text += $"from [{req.ipv4_address}]: {req.content}\r\n";
                            }
                        }
                        break;
                    case MsgType.MsgTextRes:
                        {
                            if ( msgBasic is MsgTextRes req )
                            {
                                if ( ClientData.Ins.chatHistory.TryGetValue( req.chatId, out var chat ) )
                                {
                                    input_chat.Text += $"to [{chat.ipv4_address}]: {chat.content}\r\n";
                                }
                            }
                        }
                        break;
                }
            }


            tm_req_ping += 1000f / 60;
            if ( tm_req_ping >= 1000 )
            {
                tm_req_ping = 0f;

                if ( ClientData.Ins.bindUsers.Count > 0 )
                {
                    //Logger.Debug( "PING���� ���ֻ�Ծ ����NAT���Զ��ر�" );
                    long currentTimeStamps = DateTimeUtils.GetCurrentTimestamps_MillSeconds( );
                    var msgbytes = new MsgPingReq( ).GetBytes( );
                    foreach ( var u in ClientData.Ins.bindUsers )
                    {
                        ClientData.Ins.ping_player_timestamps[ u.Key ] = currentTimeStamps;
                        P2pMgr.Ins.Send( msgbytes, u.Value );


                        if ( ClientData.Ins.ping_player_duration.TryGetValue( u.Key, out var duration ) )
                        {
                            int index = ClientData.Ins.friends.IndexOf( u.Key );
                            if ( index >= 0 )
                            {
                                FriendListMgr.Ins.UpdatePing( index, u.Key, duration );
                            }
                        }
                    }
                }
            }


            if ( p2pPkgs.Count > 0 && ClientData.Ins.bindUsers.TryGetValue( p2pPkgs.Peek( ).target_ip, out var ep ) )
            {
                P2pMgr.Ins.Send( p2pPkgs.Peek( ).pkg, ep );
                p2pPkgs.Dequeue( );
            }

        }


    }
}