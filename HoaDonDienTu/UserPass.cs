using System.Runtime.CompilerServices;

public class UserPass
{
    [CompilerGenerated]
    private string a;

    [CompilerGenerated]
    private string b;

    [CompilerGenerated]
    private string c;

    public string username
    {
        [CompilerGenerated]
        get
        {
            return a;
        }
        [CompilerGenerated]
        set
        {
            a = value;
        }
    }

    public string password
    {
        [CompilerGenerated]
        get
        {
            return b;
        }
        [CompilerGenerated]
        set
        {
            b = value;
        }
    }

    public string ma_user
    {
        [CompilerGenerated]
        get
        {
            return c;
        }
        [CompilerGenerated]
        set
        {
            c = value;
        }
    }
}
