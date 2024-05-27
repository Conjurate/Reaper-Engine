namespace Reaper;

[RequireModule(typeof(SpriteDisplay))]
public class Animator : EntityModule
{
    public bool Paused { get; set; }

    private Dictionary<string, (SpriteSheet, float[])> animations = [];
    private SpriteDisplay display;
    private Sprite baseSprite;

    private SpriteSheet curSheet;
    private float[] curDurations;
    private int curIndex;
    private float curTime;
    private float curDuration;

    private void Init()
    {
        display = GetModule<SpriteDisplay>();
        baseSprite = display.Sprite;
    }

    private void Update()
    {
        if (curSheet == null || Paused)
            return;

        curTime += Time.Delta;
        if (curTime >= curDuration)
        {
            curTime = 0;
            curIndex = (curIndex + 1) % curSheet.Length;
            display.Sprite = curSheet[curIndex];
            curDuration = curDurations[curIndex % curDurations.Length];
        }
    }

    public void Add(string id, SpriteSheet sheet, params float[] durations)
    {
        if (durations == null || durations.Length == 0)
            durations = [0.5f];

        animations[id] = (sheet, durations);
    }

    public void Play(string id)
    {
        if (animations.TryGetValue(id, out (SpriteSheet, float[]) animation))
        {
            curSheet = animation.Item1;
            curDurations = animation.Item2;
            curDuration = curDurations[0];
            curIndex = 0;
            curTime = 0;

            Sprite sprite = curSheet[curIndex];
            if (display != null)
                display.Sprite = sprite;
        }
    }

    public void Stop()
    {
        display.Sprite = baseSprite;
        curSheet = null;
        curDurations = null;
        curIndex = 0;
        curTime = 0;
        curDuration = 0;
    }
}
