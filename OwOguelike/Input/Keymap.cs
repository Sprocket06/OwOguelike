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
    private Dictionary<Control, List<ControllerAxis>> _stickmap = new();

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

    public void Unbind(ControllerButton btn)
    {
        if (!IsBound(btn))
        {
            return;
        }

        Control c = GetBind(btn);

        _buttonmap[c].Remove(btn);
    }

    public void Unbind(MouseButton mbtn)
    {
        if (!IsBound(mbtn))
        {
            return;
        }

        Control c = GetBind(mbtn);

        _mousemap[c].Remove(mbtn);
    }

    public void Unbind(ControllerAxis axis)
    {
        if (!IsBound(axis))
        {
            return;
        }

        Control c = GetBind(axis);

        _stickmap[c].Remove(axis);
    }
    
    public void Bind(KeyCode key, Control control)
    {
        if (IsBound(key))
        {
            Unbind(key);
        }
        
        _keymap[control].Add(key);
    }

    public void Bind(ControllerButton btn, Control control)
    {
        if (IsBound(btn))
        {
            Unbind(btn);
        }
        
        _buttonmap[control].Add(btn);
    }
    
    public void Bind(MouseButton mbtn, Control control)
    {
        if (IsBound(mbtn))
        {
            Unbind(mbtn);
        }
        
        _mousemap[control].Add(mbtn);
    }

    public void Bind(ControllerAxis axis, Control control)
    {
        if (IsBound(axis))
        {
            Unbind(axis);
        }
        
        _stickmap[control].Add(axis);
    }

    public bool IsBound(KeyCode key)
    {
        return _keymap.Values.ToList().Exists(keys => keys.Contains(key));
    }

    public bool IsBound(ControllerButton btn)
    {
        return _buttonmap.Values.ToList().Exists(buttons => buttons.Contains(btn));
    }

    public bool IsBound(MouseButton mbtn)
    {
        return _mousemap.Values.ToList().Exists(mbuttons => mbuttons.Contains(mbtn));
    }

    public bool IsBound(ControllerAxis axis)
    {
        return _stickmap.Values.ToList().Exists(axes => axes.Contains(axis));
    }

    public Control GetBind(KeyCode key)
    {
        if (IsBound(key))
        {
            return _keymap.Keys.First(c => _keymap[c].Contains(key));
        }

        throw new InputNotBoundException();
    }

    public Control GetBind(ControllerButton btn)
    {
        if (IsBound(btn))
        {
            return _buttonmap.Keys.First(c => _buttonmap[c].Contains(btn));
        }

        throw new InputNotBoundException();
    }
    
    public Control GetBind(MouseButton btn)
    {
        if (IsBound(btn))
        {
            return _buttonmap.Keys.First(c => _mousemap[c].Contains(btn));
        }

        throw new InputNotBoundException();
    }

    public Control GetBind(ControllerAxis axis)
    {
        if (IsBound(axis))
        {
            return _stickmap.Keys.First(c => _stickmap[c].Contains(axis));
        }

        throw new InputNotBoundException();
    }
}