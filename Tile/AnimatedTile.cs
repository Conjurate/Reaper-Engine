namespace Reaper;

public class AnimatedTile : Tile
{
    private SpriteSheet sheet;
    private int curIndex;
    private float curTime;
    private float curDuration;

    public void Update()
    {
        curTime += Time.Delta;
        if (curTime >= curDuration)
        {
            curTime = 0;
            curIndex = (curIndex + 1) % sheet.Length;
            //SpriteFrame frame = sheet[curIndex];
            //display.Sprite = frame.Sprite;
            //curDuration = frame.Duration;
        }
    }
}
