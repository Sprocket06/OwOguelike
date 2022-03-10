using System.Security.Cryptography;

namespace OwOguelike.Input;

public class Keymap
{
    private static Dictionary<Control, List<KeyCode>> DEFAULT_KEYBINDS = new Dictionary<Control, List<KeyCode>>()
    {
        {Control.MoveUp, new() {KeyCode.W}},
        {Control.MoveDown, new() {KeyCode.S}},
        {Control.MoveLeft, new() {KeyCode.A}},
        {Control.MoveRight, new() {KeyCode.D}}
    };

    private Dictionary<Control, List<KeyCode>> _keymap = new();
    private Dictionary<Control, List<ControllerButton>> _buttonmap = new();
    private Dictionary<Control, List<MouseButton>> _mousemap = new();

    public void Unbind(KeyCode key)
    {
        if (!IsBound(key))
        {
            return;
            // Might be correct to make this throw? Not sure.
            throw new InputNotBoundException();
        }

        Control c = GetBind(key);

        _keymap[c].Remove(key);
    }
    
    public void Bind(KeyCode key, Control control)
    {
        if (IsBound(key))
        {
            Unbind(key);
        }
        
        _keymap[control].Add(key);
    }

    public bool IsBound(KeyCode key)
    {
        return _keymap.Values.ToList().Exists(keys => keys.Contains(key));
    }

    public Control GetBind(KeyCode key)
    {
        if (IsBound(key))
        {
            return _keymap.Keys.First(c => _keymap[c].Contains(key));
        }

        throw new InputNotBoundException();
    }
}