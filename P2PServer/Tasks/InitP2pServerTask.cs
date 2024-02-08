using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2PServer.Tasks
{
    internal class InitP2pServerTask : Task
    {

        public override bool IsCompleted( )
        {
            return true;
        }

        protected override void Process( )
        {
            P2pMgr.Ins.ConnectServer( );
            Logger.Debug( "服务器已启动" );
        }
    }
}
