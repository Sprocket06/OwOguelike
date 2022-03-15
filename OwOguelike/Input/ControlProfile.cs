namespace OwOguelike.Input;

public class ControlProfile
{
    [JsonInclude]
    public Dictionary<ControlButton, List<KeyCode>> KeyMap { get; private set; } = new()
    {
        {ControlButton.MoveUp, new() {KeyCode.W}},
        {ControlButton.MoveDown, new() {KeyCode.S}},
        {ControlButton.MoveLeft, new() {KeyCode.A}},
        {ControlButton.MoveRight, new() {KeyCode.D}},
        {ControlButton.Action3, new() {KeyCode.E}},
        {ControlButton.Action4, new() {KeyCode.R}},
        {ControlButton.Menu, new(){KeyCode.Escape, KeyCode.Return}}
    };

    [JsonInclude]
    public Dictionary<ControlButton, List<ControllerButton>> ButtonMap { get; private set; } = new()
    {
        {ControlButton.Action1, new() {ControllerButton.LeftBumper}},
        {ControlButton.Action2, new() {ControllerButton.RightBumper}},
        {ControlButton.Action3, new() {ControllerButton.A}},
        {ControlButton.Action4, new() {ControllerButton.B}},
        {ControlButton.Menu, new() {ControllerButton.Menu}}
    };

    [JsonInclude]
    public Dictionary<ControlButton, List<MouseButton>> MouseMap { get; private set; }= new()
    {
        {ControlButton.Action1, new() {MouseButton.Left}},
        {ControlButton.Action2, new() {MouseButton.Right}}
    };

    [JsonInclude]
    public Dictionary<ControlAxis, List<ControllerAxis>> StickMap { get; private set; } = new()
    {
        {ControlAxis.LeftAxisX, new List<ControllerAxis>() {ControllerAxis.LeftStickX}},
        {ControlAxis.LeftAxisY, new() {ControllerAxis.LeftStickY}},
        {ControlAxis.RightAxisX, new() {ControllerAxis.RightStickX}},
        {ControlAxis.RightAxisY, new() {ControllerAxis.RightStickY}}
    };
    
    [JsonInclude]
    public float ControllerDeadzone { get; set; } = 0.2f;

    public override string ToString()
    {
        List<string> output = new();
        foreach (var controlBtn in Enum.GetValues<ControlButton>())
        {
            List<string> binds = new();
            
            if(KeyMap.ContainsKey(controlBtn))binds.AddRange(KeyMap[controlBtn].Select(c=>"KeyCode."+c.ToString()));
            if(ButtonMap.ContainsKey(controlBtn))binds.AddRange(ButtonMap[controlBtn].Select(c=> "ControllerButton"+c.ToString()));
            if(MouseMap.ContainsKey(controlBtn))binds.AddRange(MouseMap[controlBtn].Select(c=>"MouseButton."+c.ToString()));

            if (binds.Count == 0)
            {
                binds.Add("Not Bound");
            }
            
            output.Add($"{controlBtn.ToString()} : {string.Join(", ", binds)}");
        }

        foreach (ControlAxis axis in Enum.GetValues<ControlAxis>())
        {
            output.Add(StickMap.ContainsKey(axis)
                ? $"{axis.ToString()} : {string.Join(", ", StickMap[axis].Select(a => a.ToString()))}"
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

        KeyMap[c].Remove(key);
    }

    public void Unbind(ControllerButton btn)
    {
        if (!IsBound(btn))
        {
            return;
        }

        ControlButton c = GetBind(btn);

        ButtonMap[c].Remove(btn);
    }

    public void Unbind(MouseButton mbtn)
    {
        if (!IsBound(mbtn))
        {
            return;
        }

        ControlButton c = GetBind(mbtn);

        MouseMap[c].Remove(mbtn);
    }

    public void Unbind(ControllerAxis axis)
    {
        if (!IsBound(axis))
        {
            return;
        }

        ControlAxis c = GetBind(axis);

        StickMap[c].Remove(axis);
    }
    
    public void Bind(KeyCode key, ControlButton controlButton)
    {
        if (IsBound(key))
        {
            Unbind(key);
        }
        
        KeyMap[controlButton].Add(key);
    }

    public void Bind(ControllerButton btn, ControlButton controlButton)
    {
        if (IsBound(btn))
        {
            Unbind(btn);
        }
        
        ButtonMap[controlButton].Add(btn);
    }
    
    public void Bind(MouseButton mbtn, ControlButton controlButton)
    {
        if (IsBound(mbtn))
        {
            Unbind(mbtn);
        }
        
        MouseMap[controlButton].Add(mbtn);
    }

    public void Bind(ControllerAxis axis, ControlAxis controlAxis)
    {
        if (IsBound(axis))
        {
            Unbind(axis);
        }
        
        StickMap[controlAxis].Add(axis);
    }

    public bool IsBound(KeyCode key)
    {
        return KeyMap.Values.ToList().Exists(keys => keys.Contains(key));
    }

    public bool IsBound(ControllerButton btn)
    {
        return ButtonMap.Values.ToList().Exists(buttons => buttons.Contains(btn));
    }

    public bool IsBound(MouseButton mbtn)
    {
        return MouseMap.Values.ToList().Exists(mbuttons => mbuttons.Contains(mbtn));
    }

    public bool IsBound(ControllerAxis axis)
    {
        return StickMap.Values.ToList().Exists(axes => axes.Contains(axis));
    }

    public ControlButton GetBind(KeyCode key)
    {
        if (IsBound(key))
        {
            return KeyMap.Keys.First(c => KeyMap[c].Contains(key));
        }

        throw new InputNotBoundException();
    }

    public ControlButton GetBind(ControllerButton btn)
    {
        if (IsBound(btn))
        {
            return ButtonMap.Keys.First(c => ButtonMap[c].Contains(btn));
        }

        throw new InputNotBoundException();
    }
    
    public ControlButton GetBind(MouseButton btn)
    {
        if (IsBound(btn))
        {
            return ButtonMap.Keys.First(c => MouseMap[c].Contains(btn));
        }

        throw new InputNotBoundException();
    }

    public ControlAxis GetBind(ControllerAxis axis)
    {
        if (IsBound(axis))
        {
            return StickMap.Keys.First(c => StickMap[c].Contains(axis));
        }

        throw new InputNotBoundException();
    }
}