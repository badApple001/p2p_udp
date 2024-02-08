using System.Text;
using System.Diagnostics;

public class Logger
{

    private static bool openAsyncTsk = false;
    private static Queue<string> log = new Queue<string>( );
    private static ulong lineId = 0;
    private static ulong logFileId = 0;
    private static int logDay = -1;
    private static string datestring = string.Empty;
    private readonly static string logDir = Environment.CurrentDirectory + "/Logger/";

    public static bool OpenAsyncTask
    {
        get
        {
            return openAsyncTsk;
        }

        set
        {
            if ( value != openAsyncTsk )
            {
                openAsyncTsk = value;
                if ( value )
                {
                    Task.Run( AsyncProcessMessage );
                }
            }
        }
    }


    public static void Info( string message )
    {
        if ( !openAsyncTsk )
        {
            openAsyncTsk = true;
            Task.Run( AsyncProcessMessage );

            if ( !Directory.Exists( logDir ) )
            {
                Directory.CreateDirectory( logDir );
            }
        }

        if ( logDay != DateTime.Now.Day )
        {
            logFileId = 0;
            lineId = 0;
            logDay = DateTime.Now.Day;
            datestring = DateTime.Now.ToString( "yy-MM-dd" );
        }

        log.Enqueue( DateTime.Now.ToString( "HH:mm:ss " ) + message );
    }

    //[Conditional( "DEBUG" )]
    //public static void Toast( string message )
    //{
    //    MessageBox.Show( message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Question );
    //    Info( "[Toast] " + message );
    //}

    [Conditional( "DEBUG" )]
    public static void Debug( string message )
    {
        Info( "[Debug] " + message );
    }

    [Conditional( "DEBUG" )]
    public static void Assert( bool condition, string message )
    {
        if ( !condition )
        {
            Info( "[Assert] " + message );
            throw new Exception( message );
        }
    }

    [Conditional( "ENABLE_LOG" )]
    [Conditional( "DEBUG" )]
    public static void Error( string message )
    {
        Info( "[Error] " + message );
    }

    private static void AsyncProcessMessage( )
    {
        while ( openAsyncTsk )
        {
            if ( log.Count > 0 )
            {

                #region 运行时调试输出

                string msg = log.Dequeue( );
                //System.Diagnostics.Debug.WriteLine( msg );
                Console.WriteLine( msg );
                #endregion



                #region 日志分割 

                if ( !msg.EndsWith( "\n" ) )
                {
                    msg += '\n';
                }
                int numLine = msg.Count( c => c == '\n' );
                lineId += ( ulong ) numLine;
                if ( lineId >= ushort.MaxValue )
                {
                    ++logFileId;
                    lineId = 0;
                }

                #endregion



                #region 写入本地日志文件中

                File.AppendAllText( $"{logDir}{datestring} [{logFileId}].log", msg, Encoding.UTF8 );

                #endregion
            }

            //挂起10毫秒
            Thread.Sleep( TimeSpan.FromMilliseconds( 10 ) );
        }
    }
}

