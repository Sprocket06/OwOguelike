namespace OwOguelike.Config;

public class Configuration
{
    public static readonly string ConfigPath = "Config/config.json";

    public static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        ReadCommentHandling = JsonCommentHandling.Skip
    };

    private static Configuration _instance = null!;
    public static Configuration CurrentConfig
    {
        get => _instance;
        set
        {
            _instance = value;
            Sync();
        }
    }

    public Dictionary<string, Keymap> Profiles { get; set; } = new(); //{"keyboard", SheepleManager.DefaultProfile};
    public VerticalSyncMode VSync { get; set; } = VerticalSyncMode.None;
    public bool MuteAudioOutOfFocus { get; set; }= true;
    public float StickDeadzone = 0.2f;

    static Configuration()
    {
        EnsureConfigCanBeSaved();
        if (File.Exists(ConfigPath))
        {
            try
            {
                CurrentConfig = JsonSerializer.Deserialize<Configuration>(File.ReadAllText(ConfigPath), JsonOptions)!;
            }
            catch (JsonException e)
            {
                GameCore.Log.Error("Could not load the config file: " + e.Message);
                // Don't actually throw cause we can just nuke the config and call it a day :)
                CurrentConfig = new Configuration();
            }
        }
        else
        {
            CurrentConfig = new Configuration();
            GameCore.Log.Info("Generating a new save file! :)");
        }
    }

    public void Save()
    {
        EnsureConfigCanBeSaved();
        File.WriteAllText(ConfigPath, JsonSerializer.Serialize(this, JsonOptions));
    }

    [ConsoleCommand("syncconf")]
    public static void Sync() => _instance.Save();

    private static void EnsureConfigCanBeSaved()
    {
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(ConfigPath)!);
        }
        catch (IOException e)
        {
            GameCore.Log.Error(e);
            throw new ConfigSaveException($"Config cannot be saved in this location: {Path.GetFullPath(ConfigPath)}", e);
        }
    }
}