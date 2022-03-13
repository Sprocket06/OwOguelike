namespace OwOguelike.Scenes.SceneManagement;

public static class SceneManager
{
    private static readonly List<Scene> _scenes = new();

    public static Scene? ActiveScene { get; private set; }

    public static void SetActiveScene<T>() where T : Scene, new()
    {
        GameCore.Log.Info($"Loading scene... {typeof(T).Name}");
        if (!_scenes.Exists(x => x is T))
            _scenes.Add(new T());
        
        ActiveScene = _scenes.First(x => x is T);
    }

    public static void SetActiveScene(Scene scene)
    {
        GameCore.Log.Info($"Loading scene... {scene.GetType().Name}");
        if (!_scenes.Contains(scene))
            _scenes.Add(scene);

        ActiveScene = scene;
    }

    [ConsoleCommand("load_scene")]
    public static void LoadSceneByName(string name)
    {
        try
        {
            var type = Type.GetType($"{nameof(OwOguelike)}.{nameof(Scenes)}.{name}");
            if (type is null)
                throw new FileNotFoundException("This scene does not exist!");
            var scene = Activator.CreateInstance(type) as Scene;
            SetActiveScene(scene!);
        }
        catch (Exception e)
        {
            GameCore.Log.Error(e.Message);
        }
    }
}