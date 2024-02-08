using P2PClient;
using System.Windows.Forms;

public class FriendListMgr
{
    public static FriendListMgr Ins { get; private set; } = new FriendListMgr( );
    private ListBox view;
    public void SetView( ListBox view ) { this.view = view; }

    public void Append( string text, Color color )
    {

        view.Items.Add( text );

    }


    public void Update( List<string> friends )
    {
        view.Items.Clear( );

        int i = 0;
        foreach ( string friend in friends )
        {
            Append( $"[{i++}] {friend}", Color.Green );
        }
    }

    public void UpdatePing( int index, string friend, ushort ping )
    {
        if ( index >= 0 && index < view.Items.Count )
        {
            view.Items[ index ] = $"[{index}] {friend} {ping}ms";
        }
    }
}
