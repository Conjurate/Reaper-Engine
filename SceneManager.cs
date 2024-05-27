using static System.Formats.Asn1.AsnWriter;

namespace Reaper;

public static class SceneManager
{
    public static Scene ActiveScene => activeScene;

    private static Dictionary<string, Scene> scenes = [];
    private static Scene activeScene;
    private static string sceneToLoad;

    public static void AddScene(Scene scene)
    {
        if (scene == null || string.IsNullOrEmpty(scene.Name))
        {
            throw new ArgumentException("Scene must have a valid name.");
        }
        scenes[scene.Name] = scene;
    }

    public static bool RemoveScene(string name)
    {
        return scenes.Remove(name);
    }

    public static Scene GetScene(string name)
    {
        return scenes.GetValueOrDefault(name, null);
    }

    public static bool LoadScene(string name)
    {
        if (scenes.ContainsKey(name))
        {
            sceneToLoad = name;
            return true;
        }
        return false;
    }

    internal static void Update()
    {
        activeScene?.Update();

        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            Scene scene = GetScene(sceneToLoad);
            if (scene == null) 
                return;
            activeScene?.Unload();
            activeScene = scene;
            activeScene.Load();
            sceneToLoad = null;
        }
    }
}
