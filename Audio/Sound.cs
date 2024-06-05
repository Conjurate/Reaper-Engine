using Raylib_cs;

namespace Reaper;

public class Sound
{
    public float Volume
    {
        get => volume;
        set
        {
            volume = MathF.Min(1.0f, value);
            Raylib.SetSoundVolume(raylibSound, volume);
        }
    }
    public float Pitch
    {
        get => pitch;
        set
        {
            pitch = MathF.Min(1.0f, value);
            Raylib.SetSoundPitch(raylibSound, pitch);
        }
    }
    public float Pan
    {
        get => pan;
        set
        {
            pan = MathF.Min(1.0f, value);
            Raylib.SetSoundPan(raylibSound, pan);
        }
    }
    public bool IsReady => Raylib.IsSoundReady(raylibSound);
    public bool IsPlaying => Raylib.IsSoundPlaying(raylibSound);

    internal Raylib_cs.Sound raylibSound;

    private float volume = 1.0f;
    private float pitch = 1.0f;
    private float pan = 0.5f;

    internal Sound(Raylib_cs.Sound raylibSound)
    {
        this.raylibSound = raylibSound;
    }

    public void Play()
    {
        Raylib.PlaySound(raylibSound);
    }

    public void Stop()
    {
        Raylib.StopSound(raylibSound);
    }

    public void Pause()
    {
        Raylib.PauseSound(raylibSound);
    }

    public void Resume()
    {
        Raylib.ResumeSound(raylibSound);
    }
}
