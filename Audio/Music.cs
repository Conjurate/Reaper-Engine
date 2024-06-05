using Raylib_cs;
using System;

namespace Reaper;

public class Music
{
    public float Volume
    {
        get => volume;
        set
        {
            volume = MathF.Min(1.0f, value);
            Raylib.SetMusicVolume(raylibMusic, volume);
        }
    }
    public float Pitch
    {
        get => pitch;
        set
        {
            pitch = MathF.Min(1.0f, value);
            Raylib.SetMusicPitch(raylibMusic, pitch);
        }
    }
    public float Pan
    {
        get => pan;
        set
        {
            pan = MathF.Min(1.0f, value);
            Raylib.SetMusicPan(raylibMusic, pan);
        }
    }
    public bool IsReady => Raylib.IsMusicReady(raylibMusic);
    public bool IsPlaying => Raylib.IsMusicStreamPlaying(raylibMusic);
    public float TimeLength => Raylib.GetMusicTimeLength(raylibMusic);
    public float TimePlayed => Raylib.GetMusicTimePlayed(raylibMusic);


    internal Raylib_cs.Music raylibMusic;

    private float volume = 1.0f;
    private float pitch = 1.0f;
    private float pan = 0.5f;

    internal Music(Raylib_cs.Music raylibMusic)
    {
        this.raylibMusic = raylibMusic;
    }

    public void Play()
    {
        Raylib.PlayMusicStream(raylibMusic);
    }

    public void Stop()
    {
        Raylib.StopMusicStream(raylibMusic);
    }

    public void Pause()
    {
        Raylib.PauseMusicStream(raylibMusic);
    }

    public void Resume()
    {
        Raylib.ResumeMusicStream(raylibMusic);
    }

    public void Update()
    {
        Raylib.UpdateMusicStream(raylibMusic);
    }
}
