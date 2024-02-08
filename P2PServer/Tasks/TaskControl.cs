
namespace P2PServer.Tasks
{
    internal class TaskControl
    {

        public static bool isRunning = true;
        public static ulong tickCount { private set; get; } = 0;
        public static ulong realtime { private set; get; } = 0;

        public static List<Task> tasks { private set; get; } = new List<Task> { };


        public static void Enqueue<T>( ) where T : Task, new()
        {
            Enqueue( new T( ) );
        }

        public static void Enqueue( Task task )
        {
            if ( null != task )
            {
                if ( task.IsCompleted( ) )
                {
                    Type type = task.GetType( );
                    bool already = tasks.Find( t => t.GetType( ).Equals( type ) ) != null;
                    if ( already )
                    {
                        Logger.Error( $"type {type.ToString( )} has exisit." );
                    }
                }

                if ( !tasks.Contains( task ) )
                {
                    tasks.Add( task );
                }
                else
                {
                    Logger.Error( $"重复添加: {task.GetType( ).ToString( )}" );
                }
            }
            else
            {
                Logger.Error( "task is null" );
            }
        }


        public static void Update( )
        {
            ++tickCount;
            realtime = tickCount * 10;

            Task task;
            for ( int i = 0; i < tasks.Count; i++ )
            {
                task = tasks[ i ];
                task.Update( );
                if ( task.IsCompleted( ) )
                {
                    tasks.RemoveAt( i-- );
                }
            }

        }
    }
}
