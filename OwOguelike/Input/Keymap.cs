using System.Security.Cryptography;

namespace OwOguelike.Input;

public class Keymap
{
    public static Dictionary<Control, List<KeyCode>> DEFAULT_KEYBINDS = new Dictionary<Control, List<KeyCode>>()
    {
        {Control.MoveUp, new() {KeyCode.W}},
        {Control.MoveDown, new() {KeyCode.S}},
        {Control.MoveLeft, new() {KeyCode.A}},
        {Control.MoveRight, new() {KeyCode.D}}
    };

    public Dictionary<Control, List<KeyCode>> KeyMap = new();
    public Dictionary<Control, List<ControllerButton>> ButtonMap = new();
    public Dictionary<Control, List<MouseButton>> MouseMap = new();
    public Dictionary<Control, List<ControllerAxis>> StickMap = new();

    public void Unbind(KeyCode key)
    {
        if (!IsBound(key))
        {
            return;
            // Might be correct to make this throw? Not sure.
            throw new InputNotBoundException();
        }

        var c = GetBind(key);

        KeyMap[c].Remove(key);
    }

    public void Unbind(ControllerButton btn)
    {
        if (!IsBound(btn))
        {
            return;
        }

        var c = GetBind(btn);

        ButtonMap[c].Remove(btn);
    }

    public void Unbind(MouseButton mbtn)
    {
        if (!IsBound(mbtn))
        {
            return;
        }

        var c = GetBind(mbtn);

        MouseMap[c].Remove(mbtn);
    }

    public void Unbind(ControllerAxis axis)
    {
        if (!IsBound(axis))
        {
            return;
        }

        var c = GetBind(axis);

        StickMap[c].Remove(axis);
    }
    
    public void Bind(KeyCode key, Control control)
    {
        if (IsBound(key))
        {
            Unbind(key);
        }
        
        KeyMap[control].Add(key);
    }

    public void Bind(ControllerButton btn, Control control)
    {
        if (IsBound(btn))
        {
            Unbind(btn);
        }
        
        ButtonMap[control].Add(btn);
    }
    
    public void Bind(MouseButton mbtn, Control control)
    {
        if (IsBound(mbtn))
        {
            Unbind(mbtn);
        }
        
        MouseMap[control].Add(mbtn);
    }

    public void Bind(ControllerAxis axis, Control control)
    {
        if (IsBound(axis))
        {
            Unbind(axis);
        }
        
        StickMap[control].Add(axis);
    }

    public bool IsBound(KeyCode key) => KeyMap.Values.ToList().Exists(keys => keys.Contains(key));

    public bool IsBound(ControllerButton btn) => ButtonMap.Values.ToList().Exists(buttons => buttons.Contains(btn));

    public bool IsBound(MouseButton mbtn) => MouseMap.Values.ToList().Exists(mbuttons => mbuttons.Contains(mbtn));

    public bool IsBound(ControllerAxis axis) => StickMap.Values.ToList().Exists(axes => axes.Contains(axis));

    public Control GetBind(KeyCode key)
    {
        if (IsBound(key))
        {
            return KeyMap.Keys.First(c => KeyMap[c].Contains(key));
        }

        throw new InputNotBoundException();
    }

    public Control GetBind(ControllerButton btn)
    {
        if (IsBound(btn))
        {
            return ButtonMap.Keys.First(c => ButtonMap[c].Contains(btn));
        }

        throw new InputNotBoundException();
    }
    
    public Control GetBind(MouseButton btn)
    {
        if (IsBound(btn))
        {
            return ButtonMap.Keys.First(c => MouseMap[c].Contains(btn));
        }

        throw new InputNotBoundException();
    }

    public Control GetBind(ControllerAxis axis)
    {
        if (IsBound(axis))
        {
            return StickMap.Keys.First(c => StickMap[c].Contains(axis));
        }

        throw new InputNotBoundException();
    }
}