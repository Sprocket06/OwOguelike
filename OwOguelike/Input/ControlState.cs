﻿namespace OwOguelike.Input;

public class ControlState
{
    public Dictionary<ControlButton, bool> Buttons { get; private set; }
        = new(Enum.GetValues<ControlButton>().Select(v => new KeyValuePair<ControlButton, bool>(v, false)));

    public Dictionary<ControlAxis, short> Axes { get; private set; } =
        new(Enum.GetValues<ControlAxis>().Select(v => new KeyValuePair<ControlAxis, short>(v, 0)));

    public void SetButton(ControlButton btn, bool value)
    {
        Buttons[btn] = value;

        if (btn is ControlButton.MoveDown or ControlButton.MoveLeft or ControlButton.MoveRight or ControlButton.MoveUp)
        {
            SetAxis(ControlAxis.LeftAxisX, (short)((Buttons[ControlButton.MoveLeft] ? -short.MaxValue : 0) +
                                                    (Buttons[ControlButton.MoveRight] ? short.MaxValue : 0)));
            SetAxis(ControlAxis.LeftAxisY, (short)((Buttons[ControlButton.MoveUp] ? -short.MaxValue : 0) +
                                                   (Buttons[ControlButton.MoveDown] ? short.MaxValue : 0)));
        }
    }

    public bool GetButton(ControlButton btn) => Buttons[btn];

    public void SetAxis(ControlAxis axis, short value) => Axes[axis] = value;

    public short GetRawAxis(ControlAxis axis) => Axes[axis];
    
    public float GetRawNormalizedAxis(ControlAxis axis) => Axes[axis] != 0 ? (float)Axes[axis] / short.MaxValue : 0;

    public short GetAxis(ControlAxis axis)
    {
        var val = GetRawAxis(axis);
        return Math.Abs((float)val) / short.MaxValue < Configuration.CurrentConfig.StickDeadzone ? (short)0 : val;
    }
    
    public float GetNormalizedAxis(ControlAxis axis)
    {
        var val = GetRawAxis(axis);
        return val != 0 ? (float)val / short.MaxValue : 0;
    }

    public Vector2 AssembleRawVector(ControlAxis x, ControlAxis y) => new(GetRawNormalizedAxis(x), GetRawNormalizedAxis(y));

    public Vector2 AssembleVector(ControlAxis x, ControlAxis y) => new(GetNormalizedAxis(x), GetNormalizedAxis(y));
}