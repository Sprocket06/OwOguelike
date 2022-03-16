namespace OwOguelike.Config;

public class Configuration
{
    public const string ConfigPath = "Config/config.json";
    public const string ProfilesPath = "Config/Profiles/";

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

    [JsonInclude]
    public Dictionary<string, string> ProfileMap { get; private set; } = null!;

    public string DefaultProfile { get; set; } = "Default";

    [JsonIgnore]
    public Dictionary<string, ControlProfile> SavedProfiles { get; private set; } = null!;

    public VerticalSyncMode VSync { get; set; } = VerticalSyncMode.None;
    public bool MuteAudioOutOfFocus { get; set; } = true;

    static Configuration()
    {
        EnsureConfigCanBeSaved();
        if (File.Exists(ConfigPath))
        {
            try
            {
                _instance = JsonSerializer.Deserialize<Configuration>(File.ReadAllText(ConfigPath), JsonOptions)!;
            }
            catch (JsonException e)
            {
                GameCore.Log.Error("Could not load the config file: " + e.Message);
                // Don't actually throw cause we can just nuke the config and call it a day :)
                _instance = new Configuration();
            }
        }
        else
        {
            _instance = new Configuration();
            GameCore.Log.Info("Generating a new config file! :)");
        }

        _instance.SavedProfiles = new();
        _instance.ProfileMap = new();
        try
        {
            foreach (var file in Directory.EnumerateFiles(ProfilesPath, "*.json"))
            {
                var key = Path.GetFileNameWithoutExtension(file);
                var val = JsonSerializer.Deserialize<ControlProfile>(File.ReadAllText(file), JsonOptions)!;
                if (_instance.SavedProfiles.ContainsKey(key))
                    _instance.SavedProfiles[key] = val;
                else
                    _instance.SavedProfiles.Add(key, val);
            }
        }
        catch (IOException e)
        {
            GameCore.Log.Error("Could not read a profile file: " + e.Message);
        }
        catch (JsonException e)
        {
            GameCore.Log.Error("Could not deserialize a profile file: " + e.Message);
        }
        finally
        {
            if (_instance.SavedProfiles.Count <= 0 || !_instance.SavedProfiles.ContainsKey("Default"))
                _instance.SavedProfiles.Add("Default", SheepleManager.DefaultProfile);
            if (!_instance.ProfileMap.ContainsKey("keyboard"))
                _instance.ProfileMap.Add("keyboard", "Default");
        }

        Sync();
    }

    public void Save()
    {
        EnsureConfigCanBeSaved();
        File.WriteAllText(ConfigPath, JsonSerializer.Serialize(this, JsonOptions));
        foreach (var kvp in SavedProfiles)
        {
            File.WriteAllText(Path.Join(ProfilesPath, kvp.Key + ".json"),
                JsonSerializer.Serialize(kvp.Value, JsonOptions));
        }
    }

    [ConsoleCommand("syncconf")]
    public static void Sync() => _instance.Save();

    private static void EnsureConfigCanBeSaved()
    {
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(ConfigPath)!);
            Directory.CreateDirectory(ProfilesPath);
        }
        catch (IOException e)
        {
            GameCore.Log.Error(e);
            throw new ConfigSaveException($"Config cannot be saved in this location: {Path.GetFullPath(ConfigPath)}",
                e);
        }
    }
}