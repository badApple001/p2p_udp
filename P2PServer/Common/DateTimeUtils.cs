using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2PServer.Common
{
    internal class DateTimeUtils
    {

        //获取时间戳 毫秒级
        public static long GetCurrentTimestamps_MillSeconds( )
        {
            return ( DateTime.Now.ToUniversalTime( ).Ticks - 621355968000000000 ) / 10000;
        }

        //获取时间戳 秒级
        public static long GetCurrentTimestamps_Seconds( )
        {
            return ( DateTime.Now.ToUniversalTime( ).Ticks - 621355968000000000 ) / 10000000;
        }

    }
}
