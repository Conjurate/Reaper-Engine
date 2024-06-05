using System.Runtime.CompilerServices;

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
    private bool once;
    private bool reverse;

    // Queue for animations to play after initialization
    private string queuedId;
    private bool queuedOnce;
    private bool queuedReverse;

    private void Init()
    {
        display = GetModule<SpriteDisplay>();
        baseSprite = display.Sprite;
    }

    private void Load()
    {
        if (queuedId != null)
        {
            Play(queuedId, queuedOnce, queuedReverse);
            queuedId = null;
        }
    }

    private void Update()
    {
        if (curSheet == null || Paused)
            return;

        curTime += Time.Delta;
        if (curTime >= curDuration)
        {
            curTime = 0;

            if (once)
            {
                if ((!reverse && curIndex >= curSheet.Length - 1) || (reverse && curIndex <= 0))
                {
                    Stop();
                    return;
                }
            }

            curIndex = (curIndex + (reverse ? -1 : 1)) % curSheet.Length;
            if (curIndex < 0) curIndex = curSheet.Length - 1;
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

    public void Play(string id, bool once = false, bool reverse = false)
    {
        if (Owner == null || !Owner.Initialized)
        {
            queuedId = id;
            queuedOnce = once;
            queuedReverse = reverse;
            return;
        }
        this.once = once;
        this.reverse = reverse;
        if (animations.TryGetValue(id, out (SpriteSheet, float[]) animation))
        {
            curSheet = animation.Item1;
            curDurations = animation.Item2;
            curTime = 0;

            if (!reverse)
            {
                curDuration = curDurations[0];
                curIndex = 0;
            } else
            {
                curDuration = curDurations[^1];
                curIndex = curSheet.Length - 1;
            }

            Sprite sprite = curSheet[curIndex];
            if (display != null)
                display.Sprite = sprite;
        }
    }

    public void Stop()
    {
        once = false;
        reverse = false;
        display.Sprite = baseSprite;
        curSheet = null;
        curDurations = null;
        curIndex = 0;
        curTime = 0;
        curDuration = 0;
    }
}
