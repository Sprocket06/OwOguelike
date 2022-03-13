namespace OwOguelike.Input;

public class Keymap
{
    private Dictionary<ControlButton, List<KeyCode>> _keymap = new()
    {
        {ControlButton.MoveUp, new() {KeyCode.W}},
        {ControlButton.MoveDown, new() {KeyCode.S}},
        {ControlButton.MoveLeft, new() {KeyCode.A}},
        {ControlButton.MoveRight, new() {KeyCode.D}},
        {ControlButton.Action3, new() {KeyCode.E}},
        {ControlButton.Action4, new() {KeyCode.R}},
        {ControlButton.Menu, new(){KeyCode.Escape, KeyCode.Return}}
    };

    private Dictionary<ControlButton, List<ControllerButton>> _buttonmap = new()
    {
        {ControlButton.Action1, new() {ControllerButton.LeftBumper}},
        {ControlButton.Action2, new() {ControllerButton.RightBumper}},
        {ControlButton.Action3, new() {ControllerButton.A}},
        {ControlButton.Action4, new() {ControllerButton.B}},
        {ControlButton.Menu, new() {ControllerButton.Menu}}
    };

    private Dictionary<ControlButton, List<MouseButton>> _mousemap = new()
    {
        {ControlButton.Action1, new() {MouseButton.Left}},
        {ControlButton.Action2, new() {MouseButton.Right}}
    };

    private Dictionary<ControlAxis, List<ControllerAxis>> _stickmap = new()
    {
        {ControlAxis.LeftAxisX, new List<ControllerAxis>() {ControllerAxis.LeftStickX}},
        {ControlAxis.LeftAxisY, new() {ControllerAxis.LeftStickY}},
        {ControlAxis.RightAxisX, new() {ControllerAxis.RightStickX}},
        {ControlAxis.RightAxisY, new() {ControllerAxis.RightStickY}}
    };

    public override string ToString()
    {
        List<string> output = new();
        foreach (ControlButton controlBtn in Enum.GetValues<ControlButton>())
        {
            List<string> binds = new();
            
            if(_keymap.ContainsKey(controlBtn))binds.AddRange(_keymap[controlBtn].Select(c=>"KeyCode."+c.ToString()));
            if(_buttonmap.ContainsKey(controlBtn))binds.AddRange(_buttonmap[controlBtn].Select(c=> "ControllerButton"+c.ToString()));
            if(_mousemap.ContainsKey(controlBtn))binds.AddRange(_mousemap[controlBtn].Select(c=>"MouseButton."+c.ToString()));

            if (binds.Count == 0)
            {
                binds.Add("Not Bound");
            }
            
            output.Add($"{controlBtn.ToString()} : {string.Join(", ", binds)}");
        }

        foreach (ControlAxis axis in Enum.GetValues<ControlAxis>())
        {
            output.Add(_stickmap.ContainsKey(axis)
                ? $"{axis.ToString()} : {string.Join(", ", _stickmap[axis].Select(a => a.ToString()))}"
                : $"{axis.ToString()} | Not Bound");
        }
        
        return string.Join('\n', output);
    }

    public void Unbind(KeyCode key)
    {
        if (!IsBound(key))
        {
            return;
            // Might be correct to make this throw? Not sure.
            throw new InputNotBoundException();
        }

        ControlButton c = GetBind(key);

        _keymap[c].Remove(key);
    }

    public void Unbind(ControllerButton btn)
    {
        if (!IsBound(btn))
        {
            return;
        }

        ControlButton c = GetBind(btn);

        _buttonmap[c].Remove(btn);
    }

    public void Unbind(MouseButton mbtn)
    {
        if (!IsBound(mbtn))
        {
            return;
        }

        ControlButton c = GetBind(mbtn);

        _mousemap[c].Remove(mbtn);
    }

    public void Unbind(ControllerAxis axis)
    {
        if (!IsBound(axis))
        {
            return;
        }

        ControlAxis c = GetBind(axis);

        _stickmap[c].Remove(axis);
    }
    
    public void Bind(KeyCode key, ControlButton controlButton)
    {
        if (IsBound(key))
        {
            Unbind(key);
        }
        
        _keymap[controlButton].Add(key);
    }

    public void Bind(ControllerButton btn, ControlButton controlButton)
    {
        if (IsBound(btn))
        {
            Unbind(btn);
        }
        
        _buttonmap[controlButton].Add(btn);
    }
    
    public void Bind(MouseButton mbtn, ControlButton controlButton)
    {
        if (IsBound(mbtn))
        {
            Unbind(mbtn);
        }
        
        _mousemap[controlButton].Add(mbtn);
    }

    public void Bind(ControllerAxis axis, ControlAxis controlAxis)
    {
        if (IsBound(axis))
        {
            Unbind(axis);
        }
        
        _stickmap[controlAxis].Add(axis);
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

    public ControlButton GetBind(KeyCode key)
    {
        if (IsBound(key))
        {
            return _keymap.Keys.First(c => _keymap[c].Contains(key));
        }

        throw new InputNotBoundException();
    }

    public ControlButton GetBind(ControllerButton btn)
    {
        if (IsBound(btn))
        {
            return _buttonmap.Keys.First(c => _buttonmap[c].Contains(btn));
        }

        throw new InputNotBoundException();
    }
    
    public ControlButton GetBind(MouseButton btn)
    {
        if (IsBound(btn))
        {
            return _buttonmap.Keys.First(c => _mousemap[c].Contains(btn));
        }

        throw new InputNotBoundException();
    }

    public ControlAxis GetBind(ControllerAxis axis)
    {
        if (IsBound(axis))
        {
            return _stickmap.Keys.First(c => _stickmap[c].Contains(axis));
        }

        throw new InputNotBoundException();
    }
}