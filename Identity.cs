namespace Reaper;

public class Identity
{
    private int idCounter;

    public Identity(int startId = 0)
    {
        idCounter = startId;
    }

    public int NextId => ++idCounter;
}
