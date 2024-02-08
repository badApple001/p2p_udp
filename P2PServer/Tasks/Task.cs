using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2PServer.Tasks
{
    internal class Task
    {
        public virtual bool IsCompleted( )
        {
            return false;
        }

        protected virtual bool IsReady( )
        {
            return true;
        }


        protected virtual void Process( )
        {

        }

        public void Update( )
        {
            if ( IsReady( ) )
            {
                Process( );
            }
        }

    }
}
