

namespace OwOguelike;

public class GameCore : Game
{
    private InGameConsole _console;
    [ConsoleVariable("sv_cheats")] private static bool CheatsHehe = false;
    
    public GameCore() : base(new(false, false))
    {
        _console = new InGameConsole(Window);
    }

    [ConsoleCommand("test")]
    public static string TestDebug()
    {
        return "Debug pog!";
    }

    protected override void Update(float delta)
    {
        _console.Update(delta);
        SceneManager.ActiveScene?.Update(delta);
    }

    protected override void Draw(RenderContext context)
    {
        _console.Draw(context);
        SceneManager.ActiveScene?.Draw(context);
    }

    protected override void MouseMoved(MouseMoveEventArgs e)
    {
        SceneManager.ActiveScene?.MouseMoved(e);
    }

    protected override void MousePressed(MouseButtonEventArgs e)
    {
        SceneManager.ActiveScene?.MousePressed(e);
    }

    protected override void MouseReleased(MouseButtonEventArgs e)
    {
        SceneManager.ActiveScene?.MouseReleased(e);
    }

    protected override void WheelMoved(MouseWheelEventArgs e)
    {
        _console.WheelMoved(e);
        SceneManager.ActiveScene?.WheelMoved(e);
    }

    protected override void KeyPressed(KeyEventArgs e)
    {
        _console.KeyPressed(e);
        SceneManager.ActiveScene?.KeyPressed(e);
        
        if (e.KeyCode == KeyCode.Escape)
        {
            Quit();
        }
    }

    protected override void KeyReleased(KeyEventArgs e)
    {
        SceneManager.ActiveScene?.KeyReleased(e);
    }

    protected override void TextInput(TextInputEventArgs e)
    {
        _console.TextInput(e);
        SceneManager.ActiveScene?.TextInput(e);
    }

    protected override void ControllerConnected(ControllerEventArgs e)
    {
        SceneManager.ActiveScene?.ControllerConnected(e);
    }

    protected override void ControllerDisconnected(ControllerEventArgs e)
    {
        SceneManager.ActiveScene?.ControllerDisconnected(e);
    }

    protected override void ControllerButtonPressed(ControllerButtonEventArgs e)
    {
        SceneManager.ActiveScene?.ControllerButtonPressed(e);
    }

    protected override void ControllerButtonReleased(ControllerButtonEventArgs e)
    {
        SceneManager.ActiveScene?.ControllerButtonReleased(e);
    }

    protected override void ControllerAxisMoved(ControllerAxisEventArgs e)
    {
        SceneManager.ActiveScene?.ControllerAxisMoved(e);
    }
}