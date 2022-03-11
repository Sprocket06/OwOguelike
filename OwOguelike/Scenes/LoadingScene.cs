namespace OwOguelike.Scenes;

public class LoadingScene : Scene
{
    private static readonly float FADE_TIME = 2.0f;
    
    public bool Loaded = false;
    public int PercentLoaded = 0;
    
    private bool _loading = false;
    private int _stuffToDo;
    private int _stuffDone;
    private int _sceneStuffToDo;
    private float _fadeTimer = FADE_TIME;

    private Scene? _sceneToLoad;

    private Queue<AudioClip> _sfxToLoad = new();

    public LoadingScene(Scene? postScene = null)
    {
        _sceneToLoad = postScene;
    }

    public static void ShowLoadingScreen(Scene? postScene = null)
    {
        SceneManager.SetActiveScene(new LoadingScene(postScene));
    }

    public static void ShowLoadingScreen<T>() where T : Scene, new() => ShowLoadingScreen(new T());

    public override void Update(float delta)
    {
        if (!Loaded && !_loading)
        {
            StartLoading();
        }

        if (!Loaded)
        {
            try
            {
                if (_sfxToLoad.TryDequeue(out var clip))
                {
                    Sfx.LoadClip(clip);
                }
            }
            catch (Exception e)
            {
                GameCore.Log.Error(e.Message);
            }

            _sceneToLoad?.LoadStep();
            _stuffDone++;
            PercentLoaded = (int)((_stuffDone / (float)_stuffToDo) * 100);
            if (_stuffDone >= _stuffToDo)
            {
                Loaded = true;
            }
        }
        else
        {
            if (_fadeTimer < 0)
            {
                if(_sceneToLoad is not null)
                    SceneManager.SetActiveScene(_sceneToLoad);
            }
            _fadeTimer -= delta;
        }
    }

    public override void Draw(RenderContext context)
    {
        context.Clear(Color.Gray);
        context.DrawString(PercentLoaded.ToString(), Vector2.Zero, Color.White);
        if (Loaded)
        {
            var opacity = (FADE_TIME - _fadeTimer) / FADE_TIME;
            context.Rectangle(ShapeMode.Fill, Vector2.Zero, GameCore.Window.Size, new Color(0, 0, 0, (byte)(255 * opacity)));
        }
    }

    public void StartLoading()
    {
        _loading = true;
        QueueSfx();
        _stuffToDo = _sfxToLoad.Count + _sceneToLoad?.GetLoadSteps() ?? 0;
        _stuffDone = 0;
    }
    
    public void QueueSfx()
    {
        foreach (var clip in Enum.GetValues<AudioClip>())
        {
            var field = typeof(AudioClip).GetField(clip.ToString());
            var isMusic = field!.IsDefined(typeof(MusicAttribute), false);
            
            if(!isMusic)
                _sfxToLoad.Enqueue(clip);
        }
    }
}