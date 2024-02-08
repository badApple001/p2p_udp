using P2PServer.Tasks;


//添加初始化P2P服务器任务
TaskControl.Enqueue<InitP2pServerTask>( );
//添加处理客户端消息任务
TaskControl.Enqueue<ProcessClientMsgTaks>( );
//添加服务器心跳任务
TaskControl.Enqueue<TickTask>( );


while ( TaskControl.isRunning )
{
    Thread.Sleep( 10 );
    TaskControl.Update( );
}

