namespace OwOguelike;

public class PrivateContentProvider : DisposableResource, IContentProvider
{
    private readonly HashSet<DisposableResource> _loadedResources;
    private readonly Dictionary<Type, Func<string, object[], object>> _importers;

    public string ContentRoot { get; }

    public PrivateContentProvider(string contentRoot = null)
    {
      ContentRoot = contentRoot;
      if (string.IsNullOrEmpty(ContentRoot))
        ContentRoot = Path.Combine(AppContext.BaseDirectory, "Content");
      _loadedResources = new HashSet<DisposableResource>();
      _importers = new Dictionary<Type, Func<string, object[], object>>();
      RegisterImporters();
    }

    public T Load<T>(string relativePath, params object[] args) where T : DisposableResource
    {
      var key = typeof (T);
      if (!_importers.ContainsKey(key))
        throw new UnsupportedContentException("This type of content is not supported by this provider.", MakeAbsolutePath(relativePath));
      if (_importers[key](MakeAbsolutePath(relativePath), args) is T obj)
      {
        _loadedResources.Add(obj);
        obj.Disposing += OnResourceDisposing;
        return obj;
      }

      return null!;
    }

    public void Unload<T>(T resource) where T : DisposableResource
    {
      if (!_loadedResources.Contains(resource))
        throw new ContentNotLoadedException("The content you want to unload was never loaded in the first place.");
      resource.Dispose();
    }

    public Stream Open(string relativePath) => new FileStream(MakeAbsolutePath(relativePath), FileMode.Open);

    public byte[] Read(string relativePath) => File.ReadAllBytes(MakeAbsolutePath(relativePath));

    public void Track<T>(T resource) where T : DisposableResource
    {
      if (_loadedResources.Contains(resource))
        throw new InvalidOperationException("The content you want to track is already being tracked.");
      _loadedResources.Add(resource);
      resource.Disposing += OnResourceDisposing;
    }

    public void StopTracking<T>(T resource) where T : DisposableResource
    {
      if (!_loadedResources.Contains(resource))
        throw new ContentNotLoadedException("The content you want to stop tracking was never tracked in the first place.");
      resource.Disposing -= OnResourceDisposing;
      _loadedResources.Remove(resource);
    }

    public void RegisterImporter<T>(Func<string, object[], object> importer) where T : DisposableResource
    {
      Type key = typeof (T);
      if (_importers.ContainsKey(key))
        throw new InvalidOperationException("An importer for type " + key.Name + " was already registered.");
      _importers.Add(key, importer);
    }

    public void UnregisterImporter<T>() where T : DisposableResource
    {
      Type key = typeof (T);
      if (!_importers.ContainsKey(key))
        throw new InvalidOperationException("An importer for type " + key.Name + " was never registered, thus it cannot be unregistered.");
      _importers.Remove(key);
    }

    public bool IsImporterPresent<T>() where T : DisposableResource => _importers.ContainsKey(typeof (T));

    protected override void FreeManagedResources()
    {
      foreach (IDisposable disposable in new List<IDisposable>(_loadedResources))
        disposable.Dispose();
    }

    private void RegisterImporters()
    {
      RegisterImporter<Texture>((path, _) => new Texture(path));
      RegisterImporter<PixelShader>((path, _) => PixelShader.FromFile(path));
      RegisterImporter<VertexShader>((path, _) => VertexShader.FromFile(path));
      RegisterImporter<Effect>((path, _) => Effect.FromFile(path));
      RegisterImporter<BitmapFont>((path, _) => new BitmapFont(path));
      RegisterImporter<Sound>((path, _) => new Sound(path));
      RegisterImporter<Music>((path, _) => new Music(path));
      RegisterImporter<Cursor>((path, args) =>
      {
        Vector2 hotSpot = new Vector2();
        if (args.Length >= 1)
          hotSpot = (Vector2) args[0];
        return new Cursor(path, hotSpot);
      });
      RegisterImporter<TrueTypeFont>((path, args) => args.Length != 2 ? (args.Length != 1 ? new TrueTypeFont(path, 12) : (object) new TrueTypeFont(path, (int) args[0])) : new TrueTypeFont(path, (int) args[0], (string) args[1]));
    }

    private string MakeAbsolutePath(string relativePath) => Path.Combine(ContentRoot, relativePath);

    private void OnResourceDisposing(object sender, EventArgs e)
    {
      if (!(sender is DisposableResource disposableResource))
        return;
      disposableResource.Disposing -= OnResourceDisposing;
      _loadedResources.Remove(disposableResource);
    }
}