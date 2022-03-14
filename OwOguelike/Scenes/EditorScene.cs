namespace OwOguelike.Scenes;

public class EditorScene : Scene
{
    private int[,] _tileMap;
    
    public EditorScene(int[,]? baseTiles)
    {
        GameCore.Window.Title = "Editor";
        _tileMap = baseTiles ?? new int[10,10];
    }
    
    public EditorScene() : this(null)
    {
        
    }

    [ConsoleCommand("editor")]
    public static void LoadEditor() => SceneManager.SetActiveScene<EditorScene>();
}