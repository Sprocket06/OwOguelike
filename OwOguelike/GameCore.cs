namespace OwOguelike;

public class GameCore : Game
{
    public static IContentProvider? ContentProvider;
    public static GraphicsManager GraphicsManager = null!;
    public new static Window Window = null!;
    public static readonly Log Log = LogManager.GetForCurrentAssembly();
    
    public static ScanCode ConsoleKey { get; } = ScanCode.Grave;
    
    private readonly InGameConsole _console;
    private TrueTypeFont _debugFont;
    
    public GameCore() : base(new(false, false))
    {
        GraphicsManager = Graphics;
        GraphicsManager.VerticalSyncMode = Configuration.CurrentConfig.VSync;
        Configuration.Sync();
        Window = base.Window;
        RenderSettings.ShapeBlendingEnabled = true;
        var assembly = Assembly.GetExecutingAssembly();
        var motd = string.Empty;
        
        var motdResource = assembly.GetManifestResourceStream($"{nameof(OwOguelike)}.Content.Embedded.motd.txt");
        if (motdResource is not null)
            using (var reader = new StreamReader(motdResource))
                motd = reader.ReadToEnd();

        _console = new InGameConsole(Window, 20, null, motd);
        Log.SinkTo(new CommanderSink(_console));
    }
    
    [ConVar("test")]
    [ConsoleHidden]
    public static string TestDebug(int test = 1, string test2 = "gaming")
    {
        return test.ToString() + test2;
    }

    protected override void LoadContent()
    {
        ContentProvider = Content;
        _debugFont = ContentProvider.Load<TrueTypeFont>("Fonts/JetBrainsMono-Regular.ttf");
        // Dont play unless you want noise lol
        //var song = ContentProvider.Load<Music>("Audio/Music/FUSE/To the Moon.wav");
        //song.Play();
        LoadingScene.ShowLoadingScreen<GameplayScene>();
    }

    protected override void Update(float delta)
    {
        SceneManager.ActiveScene?.Update(delta);
        _console.Update(delta);
    }

    protected override void Draw(RenderContext context)
    {
        SceneManager.ActiveScene?.Draw(context);
        _console.Draw(context);
        
        // Debug stuff
        if (DebugVars.ShowFPS >= 1)
        {
            List<string> debugInfo = new();
            debugInfo.Add(PerformanceCounter.FPS.ToString() + " FPS");
            
            if (DebugVars.ShowFPS >= 2)
            {
                debugInfo[0] += $" {PerformanceCounter.Delta:0.00}ms";
                debugInfo.Add(SceneManager.ActiveScene?.GetType().Name ?? "No Loaded Scene");
            }

            if (DebugVars.ShowFPS >= 3)
            {
                debugInfo.Add(LevelManager.ActiveLevel?.GetType().Name ?? "No Loaded Level");
            }

            var y = 4;
            foreach (var t in debugInfo)
            {
                var measure = _debugFont.Measure(t);
                context.DrawString(_debugFont, t, Window.Width - measure.Width - 4, y, Color.White);
                y += measure.Height;
            }
        }
    }

    protected override void MouseMoved(MouseMoveEventArgs e)
    {
        SceneManager.ActiveScene?.MouseMoved(e);
    }

    protected override void MousePressed(MouseButtonEventArgs e)
    {
        SceneManager.ActiveScene?.MousePressed(e);

        var inputId = "keyboard";
        
        if (!SheepleManager.HasPlayer(inputId))
        {
            if (SheepleManager.DefaultProfile.IsBound(e.Button))
            {
                var c = SheepleManager.DefaultProfile.GetBind(e.Button);
                SceneManager.ActiveScene?.ButtonControlPressed(new("keyboard", c));
            }
        }
        else
        {
            var p = SheepleManager.GetPlayer(inputId);
            if (p.Profile.IsBound(e.Button))
            {
                var c = p.Profile.GetBind(e.Button);
                SceneManager.ActiveScene?.ButtonControlPressed(new("keyboard", c));
            }
        }
    }

    protected override void MouseReleased(MouseButtonEventArgs e)
    {
        SceneManager.ActiveScene?.MouseReleased(e);
        
        var inputId = "keyboard";
        
        if (!SheepleManager.HasPlayer(inputId))
        {
            if (SheepleManager.DefaultProfile.IsBound(e.Button))
            {
                var c = SheepleManager.DefaultProfile.GetBind(e.Button);
                SceneManager.ActiveScene?.ButtonControlReleased(new("keyboard", c));
            }
        }
        else
        {
            var p = SheepleManager.GetPlayer(inputId);
            if (p.Profile.IsBound(e.Button))
            {
                var c = p.Profile.GetBind(e.Button);
                SceneManager.ActiveScene?.ButtonControlReleased(new("keyboard", c));
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
        if (e.ScanCode == ConsoleKey)
        {
            _console.Toggle();
        }
        _console.KeyPressed(e);
        
        if (_console.IsOpen())
            return;

        SceneManager.ActiveScene?.KeyPressed(e);

        if (!SheepleManager.HasPlayer("keyboard"))
        {
            if (SheepleManager.DefaultProfile.IsBound(e.KeyCode))
            {
                var c = SheepleManager.DefaultProfile.GetBind(e.KeyCode);
                SceneManager.ActiveScene?.ButtonControlPressed(new("keyboard", c));
            }
        }
        else
        {
            var p = SheepleManager.GetPlayer("keyboard");
            if (p.Profile.IsBound(e.KeyCode))
            {
                var c = p.Profile.GetBind(e.KeyCode);
                SceneManager.ActiveScene?.ButtonControlPressed(new("keyboard", c));
            }
        }
        
        if (e.KeyCode == KeyCode.Escape)
        {
            Quit();
        }
    }

    protected override void KeyReleased(KeyEventArgs e)
    {
        if (_console.IsOpen())
            return;
        
        SceneManager.ActiveScene?.KeyReleased(e);
        
        if (!SheepleManager.HasPlayer("keyboard"))
        {
            if (SheepleManager.DefaultProfile.IsBound(e.KeyCode))
            {
                var c = SheepleManager.DefaultProfile.GetBind(e.KeyCode);
                SceneManager.ActiveScene?.ButtonControlReleased(new("keyboard", c));
            }
        }
        else
        {
            var p = SheepleManager.GetPlayer("keyboard");
            if (p.Profile.IsBound(e.KeyCode))
            {
                var c = p.Profile.GetBind(e.KeyCode);
                SceneManager.ActiveScene?.ButtonControlReleased(new("keyboard", c));
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

        var inputId = e.Controller.Info.Guid.ToString();
        
        if (!SheepleManager.HasPlayer(inputId))
        {
            if (SheepleManager.DefaultProfile.IsBound(e.Button))
            {
                var c = SheepleManager.DefaultProfile.GetBind(e.Button);
                SceneManager.ActiveScene?.ButtonControlPressed(new(inputId, c));
            }
        }
        else
        {
            var p = SheepleManager.GetPlayer(inputId);
            if (p.Profile.IsBound(e.Button))
            {
                var c = p.Profile.GetBind(e.Button);
                SceneManager.ActiveScene?.ButtonControlPressed(new(inputId, c));
            }
        }
    }

    protected override void ControllerButtonReleased(ControllerButtonEventArgs e)
    {
        SceneManager.ActiveScene?.ControllerButtonReleased(e);
        
        var inputId = e.Controller.Info.Guid.ToString();
        
        if (!SheepleManager.HasPlayer(inputId))
        {
            if (SheepleManager.DefaultProfile.IsBound(e.Button))
            {
                var c = SheepleManager.DefaultProfile.GetBind(e.Button);
                SceneManager.ActiveScene?.ButtonControlReleased(new(inputId, c));
            }
        }
        else
        {
            var p = SheepleManager.GetPlayer(inputId);
            if (p.Profile.IsBound(e.Button))
            {
                var c = p.Profile.GetBind(e.Button);
                SceneManager.ActiveScene?.ButtonControlReleased(new(inputId, c));
            }
        }
    }

    protected override void ControllerAxisMoved(ControllerAxisEventArgs e)
    {
        SceneManager.ActiveScene?.ControllerAxisMoved(e);

        if (e.Axis is not ControllerAxis.LeftTrigger or ControllerAxis.RightTrigger)
        {
            var inputId = e.Controller.Info.Guid.ToString();
        
            if (!SheepleManager.HasPlayer(inputId))
            {
                if (SheepleManager.DefaultProfile.IsBound(e.Axis))
                {
                    var c = SheepleManager.DefaultProfile.GetBind(e.Axis);
                    SceneManager.ActiveScene?.AxisControlMoved(new(inputId, c, e.Value));
                }
            }
            else
            {
                var p = SheepleManager.GetPlayer(inputId);
                if (p.Profile.IsBound(e.Axis))
                {
                    var c = p.Profile.GetBind(e.Axis);
                    SceneManager.ActiveScene?.AxisControlMoved(new(inputId, c, e.Value));
                }
            }
        }
    }
}