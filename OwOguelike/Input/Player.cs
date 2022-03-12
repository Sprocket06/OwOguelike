namespace OwOguelike.Input;

public class Player
{
    public int PlayerNum { get; set; }
    public Keymap Keymap { get; set; }
    public string InputID { get; set; }
    public Entity? Puppet { get; set; }

    public Player(int index, Keymap map, string inputId)
    {
        PlayerNum = index;
        Keymap = map;
        InputID = inputId;
    }
}