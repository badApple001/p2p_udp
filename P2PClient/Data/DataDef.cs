using Newtonsoft.Json;
using System.Text;

namespace P2PClient.Data
{


    public enum MsgType
    {

        //连接服务器请求
        MsgLoginReq,
        MsgLoginRes,

        //打洞请求
        MsgPunchReq,
        MsgPunchRes,


        //测延时
        MsgPingReq,
        MsgPingRes,


        //心跳包 防止NAT自动关闭端口
        MsgTickReq,
        MsgTickRes,


        //广播最新的在线好友列表
        MsgFriendsBroadcast,


        //文本消息
        MsgTextReq,
        MsgTextRes,
    }


    [Serializable]
    public class MsgBasic
    {

        public MsgType type;
        public string? data;

        public virtual byte[] GetBytes( )
        {
            return Encoding.UTF8.GetBytes( JsonConvert.SerializeObject( this ) );
        }
    }


    [Serializable]
    public class MsgFriendsBroadcast : MsgBasic
    {
        public MsgFriendsBroadcast( ) { type = MsgType.MsgFriendsBroadcast; }
        public List<string> friends = new List<string>( );
    }


    [Serializable]
    public class MsgTextReq : MsgBasic
    {
        public MsgTextReq( ) { type = MsgType.MsgTextReq; }
        public string ipv4_address = "";
        public string content = "";
        public ulong chatId = 0;
    }


    [Serializable]
    public class MsgTextRes : MsgBasic
    {
        public MsgTextRes( ) { type = MsgType.MsgTextRes; }
        public ulong chatId = 0;
    }


    [Serializable]
    public class MsgLoginReq : MsgBasic
    {
        public MsgLoginReq( ) { type = MsgType.MsgLoginReq; }

    }

    [Serializable]
    public class MsgLoginRes : MsgBasic
    {
        public MsgLoginRes( ) { type = MsgType.MsgLoginRes; }
        public string ipv4_address = "";  // 1.1.1.1:5566
        public List<string> friends = new List<string>( ); //在线的好友列表
    }

    [Serializable]
    public class MsgPunchReq : MsgBasic
    {
        public MsgPunchReq( ) { type = MsgType.MsgPunchReq; }
        
        public string target_ip = "";
        public string source_ip = "";
    }


    [Serializable]
    public class MsgPunchRes : MsgBasic
    {
        
        public string source_ip = "";
        public string target_ip = "";  // 1.1.1.1:5566
 

        public bool A2B = true;
        public MsgPunchRes( ) { type = MsgType.MsgPunchRes; }
    }


    [Serializable]
    public class MsgPingReq : MsgBasic
    {
        public MsgPingReq( ) { type = MsgType.MsgPingReq; }

    }

    [Serializable]
    public class MsgPingRes : MsgBasic
    {
        public MsgPingRes( ) { type = MsgType.MsgPingRes; }
    }

    [Serializable]
    public class MsgTickReq : MsgBasic
    {
        public MsgTickReq( ) { type = MsgType.MsgTickReq; }
    }


    [Serializable]
    public class MsgTickRes : MsgBasic
    {
        public MsgTickRes( ) { type = MsgType.MsgTickRes; }
    }

}
