namespace OwOguelike;

public class GameCore : Game
{
    public static IContentProvider? ContentProvider;
    public static GraphicsManager GraphicsManager = null!;
    public new static Window Window = null!;
    public static readonly Log Log = LogManager.GetForCurrentAssembly();
    
    private InGameConsole _console;
    [ConsoleVariable("sv_cheats")] 
    private static bool CheatsHehe = false;

    private Sound _testSound;
    
    public GameCore() : base(new(false, false))
    {
        GraphicsManager = Graphics;
        Window = base.Window;
        RenderSettings.ShapeBlendingEnabled = true;
        _console = new InGameConsole(Window);
        LoadingScene.ShowLoadingScreen<GameplayScene>();
    }

    [ConsoleCommand("test")]
    public static string TestDebug()
    {
        return "Debug pog!";
    }

    protected override void LoadContent()
    {
        ContentProvider = Content;
        //_testSound = Sfx.PlayClip("hellyeah.ogg");
    }

    protected override void Update(float delta)
    {
        //_testSound.UpdatePanning(Mouse.GetPosition(), Window.Width / 2);
        SceneManager.ActiveScene?.Update(delta);
        _console.Update(delta);
    }

    protected override void Draw(RenderContext context)
    {
        SceneManager.ActiveScene?.Draw(context);
        _console.Draw(context);
    }

    protected override void MouseMoved(MouseMoveEventArgs e)
    {
        SceneManager.ActiveScene?.MouseMoved(e);
    }

    protected override void MousePressed(MouseButtonEventArgs e)
    {
        SceneManager.ActiveScene?.MousePressed(e);

        String inputId = "keyboard";
        
        if (!SheepleManager.HasPlayer(inputId))
        {
            SheepleManager.AddPlayer(inputId);
        }
        else
        {
            Player p = SheepleManager.GetPlayer(inputId);
            if (p.Keymap.IsBound(e.Button))
            {
                Control c = p.Keymap.GetBind(e.Button);
                SceneManager.ActiveScene?.ButtonControlPressed(new() {Control = c});
            }
        }
    }

    protected override void MouseReleased(MouseButtonEventArgs e)
    {
        SceneManager.ActiveScene?.MouseReleased(e);
        
        String inputId = "keyboard";
        
        if (!SheepleManager.HasPlayer(inputId))
        {
            SheepleManager.AddPlayer(inputId);
        }
        else
        {
            Player p = SheepleManager.GetPlayer(inputId);
            if (p.Keymap.IsBound(e.Button))
            {
                Control c = p.Keymap.GetBind(e.Button);
                SceneManager.ActiveScene?.ButtonControlReleased(new() {Control = c});
            }
        }
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

        if (!SheepleManager.HasPlayer("keyboard"))
        {
            SheepleManager.AddPlayer("keyboard");
        }
        else
        {
            Player p = SheepleManager.GetPlayer("keyboard");
            if (p.Keymap.IsBound(e.KeyCode))
            {
                Control c = p.Keymap.GetBind(e.KeyCode);
                SceneManager.ActiveScene?.ButtonControlPressed(new() {Control = c});
            }
        }
        
        if (e.KeyCode == KeyCode.Escape)
        {
            Quit();
        }
    }

    protected override void KeyReleased(KeyEventArgs e)
    {
        SceneManager.ActiveScene?.KeyReleased(e);
        
        if (!SheepleManager.HasPlayer("keyboard"))
        {
            SheepleManager.AddPlayer("keyboard");
        }
        else
        {
            Player p = SheepleManager.GetPlayer("keyboard");
            if (p.Keymap.IsBound(e.KeyCode))
            {
                Control c = p.Keymap.GetBind(e.KeyCode);
                SceneManager.ActiveScene?.ButtonControlReleased(new() {Control = c});
            }
        }
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

        String inputId = e.Controller.Info.Guid.ToString();
        
        if (!SheepleManager.HasPlayer(inputId))
        {
            SheepleManager.AddPlayer(inputId);
        }
        else
        {
            Player p = SheepleManager.GetPlayer(inputId);
            if (p.Keymap.IsBound(e.Button))
            {
                Control c = p.Keymap.GetBind(e.Button);
                SceneManager.ActiveScene?.ButtonControlPressed(new() {Control = c});
            }
        }
    }

    protected override void ControllerButtonReleased(ControllerButtonEventArgs e)
    {
        SceneManager.ActiveScene?.ControllerButtonReleased(e);
        
        String inputId = e.Controller.Info.Guid.ToString();
        
        if (!SheepleManager.HasPlayer(inputId))
        {
            SheepleManager.AddPlayer(inputId);
        }
        else
        {
            Player p = SheepleManager.GetPlayer(inputId);
            if (p.Keymap.IsBound(e.Button))
            {
                Control c = p.Keymap.GetBind(e.Button);
                SceneManager.ActiveScene?.ButtonControlReleased(new() {Control = c});
            }
        }
    }

    protected override void ControllerAxisMoved(ControllerAxisEventArgs e)
    {
        SceneManager.ActiveScene?.ControllerAxisMoved(e);

        if (e.Axis is not ControllerAxis.LeftTrigger or ControllerAxis.RightTrigger)
        {
            String inputId = e.Controller.Info.Guid.ToString();
        
            if (!SheepleManager.HasPlayer(inputId))
            {
                SheepleManager.AddPlayer(inputId);
            }
            else
            {
                Player p = SheepleManager.GetPlayer(inputId);
                if (p.Keymap.IsBound(e.Axis))
                {
                    Control c = p.Keymap.GetBind(e.Axis);
                    SceneManager.ActiveScene?.AxisControlMoved(new() {Control = c, Value = e.Value});
                }
            }
        }
    }
}