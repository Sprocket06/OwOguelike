namespace OwOguelike;

public class SheepleManager
{
    public static List<Player> Players = new();

    //NOTE: Once config loading is a thing this should be replaced by the default control profile
    public static ControlProfile DefaultProfile = new();

    static SheepleManager()
    {
    }

    public static Player AddPlayer(string inputId)
    {
        ControlProfile profile;
        // ReSharper disable once ConvertIfStatementToConditionalTernaryExpression
        if (Configuration.CurrentConfig.ProfileMap.TryGetValue(inputId, out var profileName))
        {
            profile = Configuration.CurrentConfig.SavedProfiles.GetValueOrDefault(profileName, DefaultProfile);
        }
        else
        {
            profile = Configuration.CurrentConfig.SavedProfiles.GetValueOrDefault(
                Configuration.CurrentConfig.DefaultProfile, DefaultProfile);
        }

        var p = new Player(GetNextAvailableSlot(), profile, inputId);
        Players.Add(p);

        return p;
    }

    [ConsoleCommand("get_binds")]
    public static ControlProfile GetBinds(int playerNum = 0)
    {
        if (Players.Exists(p => p.PlayerNum == playerNum))
        {
            return Players.First(p => p.PlayerNum == playerNum).Profile;
        }

        return DefaultProfile;
    }

    public static void RemovePlayer(int playerNum)
    {
        Players.RemoveAll(p => p.PlayerNum == playerNum);
    }

    public static bool HasPlayer(string inputId)
    {
        return Players.Exists(p => p.InputID == inputId);
    }

    public static Player GetPlayer(string inputId)
    {
        if (!HasPlayer(inputId))
        {
            throw new InputIdNotAssignedException();
        }

        return Players.First(p => p.InputID == inputId);
    }

    private static int GetNextAvailableSlot()
    {
        if (Players.Count == 0)
        {
            return 0;
        }

        List<int> idMap = Players.Select(p => p.PlayerNum).ToList();
        return Enumerable.Range(0, idMap.Max()).Except(idMap).FirstOrDefault(idMap.Count);
    }
}