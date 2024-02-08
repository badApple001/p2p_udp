using P2PClient.Data;
using P2PServer.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2PServer.Tasks
{
    internal class TickTask : Task
    {

        float tm = 0f;
        protected override bool IsReady( )
        {
            tm += 10f;
            return tm >= 5 * 1000;
        }


        protected override void Process( )
        {
            tm = 0f;

            //移除离线用户
            var dictUsers = UserData.Ins.users;
            var loginoutUsers = dictUsers.Keys.ToList( ).Where( k => dictUsers[ k ].tick_res_ts - dictUsers[ k ].tick_req_ts >= 20 * 1000 ).ToList( );
            foreach ( var user in loginoutUsers )
            {
                dictUsers.Remove( user );
            }
            

            //更新好友列表
            if( loginoutUsers.Count > 0 )
            {
                System.Threading.Tasks.Task.Run( async ( ) =>
                {
                    await System.Threading.Tasks.Task.Delay( 100 );

                    List<string> users = UserData.Ins.users.Keys.ToList( );
                    foreach ( var user in UserData.Ins.users )
                    {
                        var friends = users.Where( t => t != user.Key ).ToList( );
                        P2pMgr.Ins.Send( new MsgFriendsBroadcast( ) { friends = friends }, user.Value.ep );
                        await System.Threading.Tasks.Task.Delay( 10 );
                    }
                } );
            }

            //发送心跳包
            var bytes = new MsgTickReq( ).GetBytes( );
            long currentTimestamps = DateTimeUtils.GetCurrentTimestamps_MillSeconds( );
            foreach ( var u in dictUsers )
            {
                P2pMgr.Ins.Send( bytes, u.Value.ep );
                u.Value.tick_req_ts = currentTimestamps;
                u.Value.tick_res_ts = u.Value.tick_req_ts + 20 * 1000;
            }
        }
    }
}
