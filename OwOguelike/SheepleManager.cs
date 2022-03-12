using System.Security.Claims;

namespace OwOguelike;

public class SheepleManager
{
    public static List<Player> Players = new();

    static SheepleManager()
    {
        
    }

    public static void AddPlayer(string inputId)
    {
        Players.Add(new Player(GetNextAvailableSlot(), new Keymap(), inputId));
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